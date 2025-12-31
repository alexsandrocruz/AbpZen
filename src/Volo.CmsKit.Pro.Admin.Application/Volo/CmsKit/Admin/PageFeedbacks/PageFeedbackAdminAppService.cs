using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.PageFeedbacks;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.PageFeedbacks;

[RequiresFeature(CmsKitProFeatures.PageFeedbackEnable)]
[RequiresGlobalFeature(PageFeedbackFeature.Name)]
[Authorize(CmsKitProAdminPermissions.PageFeedbacks.Default)]
public class PageFeedbackAdminAppService : CmsKitProAdminAppService, IPageFeedbackAdminAppService
{
    protected IPageFeedbackRepository PageFeedbackRepository { get; }
    protected IPageFeedbackEntityTypeDefinitionStore EntityTypeDefinitionStore { get; }
    protected IPageFeedbackSettingRepository PageFeedbackSettingRepository { get; }
    protected PageFeedbackManager PageFeedbackManager { get; }

    public PageFeedbackAdminAppService(
        IPageFeedbackRepository pageFeedbackRepository,
        IPageFeedbackEntityTypeDefinitionStore entityTypeDefinitionStore,
        IPageFeedbackSettingRepository pageFeedbackSettingRepository,
        PageFeedbackManager pageFeedbackManager)
    {
        PageFeedbackRepository = pageFeedbackRepository;
        EntityTypeDefinitionStore = entityTypeDefinitionStore;
        PageFeedbackSettingRepository = pageFeedbackSettingRepository;
        PageFeedbackManager = pageFeedbackManager;
    }

    public virtual async Task<PageFeedbackDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<PageFeedback, PageFeedbackDto>(
            await PageFeedbackRepository.GetAsync(id)
        );
    }

    public virtual async Task<PagedResultDto<PageFeedbackDto>> GetListAsync(GetPageFeedbackListInput input)
    {
        var pageFeedbacks = await PageFeedbackRepository.GetListAsync(
            input.EntityType,
            input.EntityId,
            input.IsUseful,
            input.Url,
            input.IsHandled,
            input.Sorting,
            input.SkipCount,
            input.MaxResultCount,
            hasUserNote: input.HasUserNote,
            hasAdminNote: input.HasAdminNote
        );

        var totalCount = await PageFeedbackRepository.GetCountAsync(
            input.EntityType,
            input.EntityId,
            input.IsUseful,
            input.Url,
            input.IsHandled,
            hasUserNote: input.HasUserNote,
            hasAdminNote: input.HasAdminNote
        );

        return new PagedResultDto<PageFeedbackDto> {
            Items = ObjectMapper.Map<List<PageFeedback>, List<PageFeedbackDto>>(pageFeedbacks), 
            TotalCount = totalCount
        };
    }

    [Authorize(CmsKitProAdminPermissions.PageFeedbacks.Update)]
    public virtual async Task<PageFeedbackDto> UpdateAsync(Guid id, UpdatePageFeedbackDto dto)
    {
        var pageFeedback = await PageFeedbackRepository.GetAsync(id);

        pageFeedback.SetAdminNote(
            dto.AdminNote
        );

        pageFeedback.IsHandled = dto.IsHandled;

        pageFeedback = await PageFeedbackRepository.UpdateAsync(pageFeedback);

        return ObjectMapper.Map<PageFeedback, PageFeedbackDto>(pageFeedback);
    }

    [Authorize(CmsKitProAdminPermissions.PageFeedbacks.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await PageFeedbackRepository.DeleteAsync(id);
    }

    public virtual async Task<List<string>> GetEntityTypesAsync()
    {
        var definitions = await EntityTypeDefinitionStore.GetPageFeedbackEntityTypeDefinitionsAsync();
        return definitions
            .Select(x => x.EntityType)
            .ToList();
    }

    [Authorize(CmsKitProAdminPermissions.PageFeedbacks.Settings)]
    public virtual async Task<List<PageFeedbackSettingDto>> GetSettingsAsync()
    {
        return ObjectMapper.Map<List<PageFeedbackSetting>, List<PageFeedbackSettingDto>>(
            await PageFeedbackSettingRepository.GetListAsync()
        );
    }

    [Authorize(CmsKitProAdminPermissions.PageFeedbacks.Settings)]
    public virtual async Task UpdateSettingsAsync(UpdatePageFeedbackSettingsInput input)
    {
        var entityTypes = input.Settings
            .Select(x => x.EntityType)
            .ToList();
        
        var settingsInDatabase = await PageFeedbackSettingRepository.GetListByEntityTypesAsync(entityTypes);
        var newSettings = new List<PageFeedbackSetting>();

        foreach (var setting in input.Settings)
        {
            // update if existing setting
            var settingInDatabase = settingsInDatabase.FirstOrDefault(x => x.EntityType == setting.EntityType);
            if (settingInDatabase != null)
            {
                settingInDatabase.SetEmailAddresses(setting.EmailAddresses);
                continue;
            }

            // Create if new setting
            if (setting.EntityType != null)
            {
                newSettings.Add(
                    await PageFeedbackManager.CreateSettingForEntityTypeAsync(
                        setting.EntityType,
                        setting.EmailAddresses
                    )
                );
                
                continue;
            }

            // default email addresses
            var defaultSettingInDatabase = await PageFeedbackSettingRepository.FindByEntityTypeAsync(PageFeedbackConst.DefaultSettingEntityType);
            if (defaultSettingInDatabase != null)
            {
                defaultSettingInDatabase.SetEmailAddresses(setting.EmailAddresses);
                settingsInDatabase.Add(defaultSettingInDatabase);
            }
            else
            {
                newSettings.Add(PageFeedbackManager.CreateDefaultSetting(setting.EmailAddresses));
            }
        }

        var existingEntityTypes = await GetEntityTypesAsync();
        existingEntityTypes.Add(PageFeedbackConst.DefaultSettingEntityType);
        
        await PageFeedbackSettingRepository.DeleteOldSettingsAsync(existingEntityTypes);
        if (newSettings.Any())
        {
            await PageFeedbackSettingRepository.InsertManyAsync(newSettings);
        }

        if (settingsInDatabase.Any())
        {
            await PageFeedbackSettingRepository.UpdateManyAsync(settingsInDatabase);
        }
    }
}