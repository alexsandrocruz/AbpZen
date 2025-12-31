using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.CmsKit.Faqs;
using Volo.CmsKit.Newsletters;
using Volo.CmsKit.PageFeedbacks;
using Volo.CmsKit.Polls;
using Volo.CmsKit.UrlShorting;

namespace Volo.CmsKit.Pro;

public class CmsKitProDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly CmsKitProTestData _cmsKitProTestData;
    private readonly INewsletterRecordRepository _newsletterRecordRepository;
    private readonly IShortenedUrlRepository _shortenedUrlRepository;
    private readonly IPollRepository _pollRepository;
    private readonly IPageFeedbackRepository _pageFeedbackRepository;
    private readonly IOptions<CmsKitPageFeedbackOptions> _pageFeedbackOptions;
    private readonly IPageFeedbackSettingRepository _pageFeedbackSettingRepository;
    private readonly PageFeedbackManager _pageFeedbackManager;
    private readonly IPollUserVoteRepository _pollUserVoteRepository;
    private readonly IFaqSectionRepository _faqSectionRepository;
    private readonly IFaqQuestionRepository _faqQuestionRepository;
   

    public CmsKitProDataSeedContributor(
        IGuidGenerator guidGenerator,
        CmsKitProTestData cmsKitProTestData,
        INewsletterRecordRepository newsletterRecordRepository,
        IShortenedUrlRepository shortenedUrlRepository,
        IPollRepository pollRepository, 
        IPageFeedbackRepository pageFeedbackRepository, 
        IOptions<CmsKitPageFeedbackOptions> pageFeedbackOptions, 
        IPageFeedbackSettingRepository pageFeedbackSettingRepository, 
        PageFeedbackManager pageFeedbackManager, 
        IPollUserVoteRepository pollUserVoteRepository,
        IFaqSectionRepository faqSectionRepository,
        IFaqQuestionRepository faqQuestionRepository)
    {
        _guidGenerator = guidGenerator;
        _cmsKitProTestData = cmsKitProTestData;
        _newsletterRecordRepository = newsletterRecordRepository;
        _shortenedUrlRepository = shortenedUrlRepository;
        _pollRepository = pollRepository;
        _pageFeedbackRepository = pageFeedbackRepository;
        _pageFeedbackOptions = pageFeedbackOptions;
        _pageFeedbackSettingRepository = pageFeedbackSettingRepository;
        _pageFeedbackManager = pageFeedbackManager;
        _pollUserVoteRepository = pollUserVoteRepository;
        _faqQuestionRepository = faqQuestionRepository;
        _faqSectionRepository = faqSectionRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await SeedNewsletterRecordAsync();
        await SeedShortenedUrlAsync();
        await SeedPollAsync();
        await SeedPageFeedbackAsync();
        await SeedPageFeedbackSettingAsync();
        await SeedFaqSectionAsync();
    }
    
    private async Task SeedNewsletterRecordAsync()
    {
        var newsletterRecord = new NewsletterRecord(_cmsKitProTestData.NewsletterEmailId, _cmsKitProTestData.Email);

        newsletterRecord.AddPreferences(new NewsletterPreference(_guidGenerator.Create(), newsletterRecord.Id,
            _cmsKitProTestData.Preference, _cmsKitProTestData.Source, "sourceUrl")
        );

        await _newsletterRecordRepository.InsertAsync(newsletterRecord);

        var newsletterRecord2 = new NewsletterRecord(_guidGenerator.Create(), "info@volosoft.io");

        newsletterRecord2.AddPreferences(new NewsletterPreference(_guidGenerator.Create(), newsletterRecord2.Id,
            "preference2", "source2", "sourceUrl2")
        );

        await _newsletterRecordRepository.InsertAsync(newsletterRecord2);

        var newsletterRecord3 = new NewsletterRecord(_guidGenerator.Create(), "info@aspnetzero.io");

        newsletterRecord3.AddPreferences(new NewsletterPreference(_guidGenerator.Create(), newsletterRecord3.Id,
            "preference3", "source3", "sourceUrl3")
        );

        await _newsletterRecordRepository.InsertAsync(newsletterRecord3);
    }

    private async Task SeedShortenedUrlAsync()
    {
        var shortenedUrl = new ShortenedUrl(_cmsKitProTestData.ShortenedUrlId1, _cmsKitProTestData.ShortenedUrlSource1, _cmsKitProTestData.ShortenedUrlTarget1);

        await _shortenedUrlRepository.InsertAsync(shortenedUrl);

        var shortenedUrl2 = new ShortenedUrl(_cmsKitProTestData.ShortenedUrlId2, _cmsKitProTestData.ShortenedUrlSource2, _cmsKitProTestData.ShortenedUrlTarget2);

        await _shortenedUrlRepository.InsertAsync(shortenedUrl2);
    }

    private async Task SeedPollAsync()
    {
        var poll = new Poll(
            _cmsKitProTestData.PollId,
            _cmsKitProTestData.Question,
            _cmsKitProTestData.Code,
            _cmsKitProTestData.Widget,
            _cmsKitProTestData.Name,
            DateTime.UtcNow,
            allowMultipleVote: true,
            endDate: DateTime.UtcNow.AddYears(1)
            );
        poll.AddPollOption(_cmsKitProTestData.PollOptionId, "yes", 0, null).Increase();
        poll.AddPollOption(_cmsKitProTestData.PollOptionId2, "no", 1, null);
        poll.Increase();
        
        await _pollUserVoteRepository.InsertAsync(new PollUserVote(_guidGenerator.Create(),_cmsKitProTestData.PollId, _cmsKitProTestData.User1Id, _cmsKitProTestData.PollOptionId));
        await _pollUserVoteRepository.InsertAsync(new PollUserVote(_guidGenerator.Create(),_cmsKitProTestData.PollId, _cmsKitProTestData.User2Id, _cmsKitProTestData.PollOptionId));
        await _pollRepository.InsertAsync(poll);
    }
    
    private async Task SeedPageFeedbackAsync()
    {
        _pageFeedbackOptions.Value.EntityTypes.Add(new PageFeedbackEntityTypeDefinition(_cmsKitProTestData.EntityType1));
        _pageFeedbackOptions.Value.EntityTypes.Add(new PageFeedbackEntityTypeDefinition(_cmsKitProTestData.EntityType2));

        var pageFeedback = await _pageFeedbackManager.CreateAsync(
            _cmsKitProTestData.EntityType1,
            _cmsKitProTestData.EntityId1,
            "http://localhost",
            true,
            "User Note",
            _cmsKitProTestData.PageFeedbackUserId
        );
        
        await _pageFeedbackRepository.InsertAsync(pageFeedback);
        
        var pageFeedback2 = await _pageFeedbackManager.CreateAsync(
            _cmsKitProTestData.EntityType2,
            _cmsKitProTestData.EntityId2,
            "http://localhost",
            false,
            "User Note",
            _cmsKitProTestData.PageFeedbackUserId
        );

        pageFeedback2.SetAdminNote("Admin Note");
        pageFeedback2.IsHandled = true;
        
        await _pageFeedbackRepository.InsertAsync(pageFeedback2);
        
        var pageFeedback3 = await _pageFeedbackManager.CreateAsync(
            _cmsKitProTestData.EntityType1,
            _cmsKitProTestData.NullEntityId,
            "http://localhost",
            false,
            "User Note",
            _cmsKitProTestData.PageFeedbackUserId
        );
        
        await _pageFeedbackRepository.InsertAsync(pageFeedback3);
    }
    
    private async Task SeedPageFeedbackSettingAsync()
    {
        var pageFeedbackSetting = _pageFeedbackManager.CreateDefaultSetting(_cmsKitProTestData.FallBackEmailAddresses.JoinAsString(PageFeedbackConst.EmailAddressesSeparator));
        
        await _pageFeedbackSettingRepository.InsertAsync(pageFeedbackSetting);
        
        var pageFeedbackSetting2 = await _pageFeedbackManager.CreateSettingForEntityTypeAsync(
            _cmsKitProTestData.EntityType1,
            new List<string> {_cmsKitProTestData.EmailAddresses, _cmsKitProTestData.EmailAddresses2}.JoinAsString(PageFeedbackConst.EmailAddressesSeparator)
        );

        await _pageFeedbackSettingRepository.InsertAsync(pageFeedbackSetting2);
    }
    private async Task SeedFaqSectionAsync()
    {
        var faqSection = new FaqSection(
            _cmsKitProTestData.FaqSectionId,
            _cmsKitProTestData.FaqSectionGroupName,
            _cmsKitProTestData.FaqSectionName,
            _cmsKitProTestData.FaqSectionOrder);

        await _faqSectionRepository.InsertAsync(faqSection);
    }
}
