using System;
using System.Threading.Tasks;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.SettingManagement;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.PageFeedbacks;

namespace Volo.CmsKit.Public.PageFeedbacks;

[RequiresFeature(CmsKitProFeatures.PageFeedbackEnable)]
[RequiresGlobalFeature(PageFeedbackFeature.Name)]
public class PageFeedbackPublicAppService : PublicAppService, IPageFeedbackPublicAppService
{
    protected virtual IPageFeedbackRepository PageFeedbackRepository { get; }
    protected virtual PageFeedbackManager PageFeedbackManager { get; }
    protected virtual IPageFeedbackSettingRepository PageFeedbackSettingRepository { get; }
    protected virtual PageFeedbackEmailSender PageFeedbackEmailSender { get; }
    
    protected virtual ISettingManager SettingManager { get; }

    public PageFeedbackPublicAppService(
        IPageFeedbackRepository pageFeedbackRepository, 
        PageFeedbackManager pageFeedbackManager, 
        IPageFeedbackSettingRepository pageFeedbackSettingRepository, 
        PageFeedbackEmailSender pageFeedbackEmailSender, 
        ISettingManager settingManager)
    {
        PageFeedbackRepository = pageFeedbackRepository;
        PageFeedbackManager = pageFeedbackManager;
        PageFeedbackSettingRepository = pageFeedbackSettingRepository;
        PageFeedbackEmailSender = pageFeedbackEmailSender;
        SettingManager = settingManager;
    }

    public virtual async Task<PageFeedbackDto> CreateAsync(CreatePageFeedbackInput input)
    {
        var pageFeedback = await PageFeedbackManager.CreateAsync(
            input.EntityType,
            input.EntityId,
            input.Url,
            input.IsUseful,
            input.UserNote,
            input.FeedbackUserId
        );
        
        if (pageFeedback.IsUseful == false && pageFeedback.UserNote.IsNullOrWhiteSpace() && await GetRequireCommentsForNegativeFeedbackAsync())
        {
            throw new RequireCommentsForNegativeFeedbackException();
        }
        
        if (pageFeedback.UserNote.IsNullOrWhiteSpace() && await GetIsAutoHandledAsync())
        {
            pageFeedback.IsHandled = true;
        }
        
        await PageFeedbackRepository.InsertAsync(pageFeedback);
        
        if(!pageFeedback.UserNote.IsNullOrWhiteSpace())
        {
            await SendEmailsAsync(pageFeedback);
        }
        
        return ObjectMapper.Map<PageFeedback, PageFeedbackDto>(pageFeedback);
    }

    public virtual async Task<PageFeedbackDto> InitializeUserNoteAsync(InitializeUserNoteInput input)
    {
        var pageFeedback = await PageFeedbackRepository.GetAsync(input.Id);
        pageFeedback.InitializeUserNote(input.UserNote, input.IsUseful);
        
        if (pageFeedback.IsHandled && !pageFeedback.UserNote.IsNullOrWhiteSpace() && await GetIsAutoHandledAsync())
        {
            pageFeedback.IsHandled = false;
        }
        
        await PageFeedbackRepository.UpdateAsync(pageFeedback);
        
        if(!pageFeedback.UserNote.IsNullOrWhiteSpace())
        {
            await SendEmailsAsync(pageFeedback);
        }
        
        return ObjectMapper.Map<PageFeedback, PageFeedbackDto>(pageFeedback);
    }

    public async Task<PageFeedbackDto> ChangeIsUsefulAsync(ChangeIsUsefulInput input)
    {
        var pageFeedback = await PageFeedbackRepository.GetAsync(input.Id);
        pageFeedback.IsUseful = input.IsUseful;
        await PageFeedbackRepository.UpdateAsync(pageFeedback);
        return ObjectMapper.Map<PageFeedback, PageFeedbackDto>(pageFeedback);
    }

    protected virtual async Task SendEmailsAsync(PageFeedback pageFeedback)
    {
        var setting = await PageFeedbackSettingRepository.FindByEntityTypeAsync(pageFeedback.EntityType);
        if (setting == null || setting.EmailAddresses.IsNullOrWhiteSpace())
        {
            setting = await PageFeedbackSettingRepository.FindByEntityTypeAsync(PageFeedbackConst.DefaultSettingEntityType);
        }
        
        if (setting == null)
        {
            return;
        }

        await PageFeedbackEmailSender.QueueAsync(pageFeedback, setting.EmailAddresses.Split(PageFeedbackConst.EmailAddressesSeparator));
    }
    
    protected virtual async Task<bool> GetIsAutoHandledAsync()
    {
        return (await SettingManager.GetOrNullForCurrentTenantAsync(CmsKitProSettingNames.PageFeedback.IsAutoHandled)).To<bool>();
    }
    
    protected virtual async Task<bool> GetRequireCommentsForNegativeFeedbackAsync()
    {
        return (await SettingManager.GetOrNullForCurrentTenantAsync(CmsKitProSettingNames.PageFeedback.RequireCommentsForNegativeFeedback)).To<bool>();
    }
}