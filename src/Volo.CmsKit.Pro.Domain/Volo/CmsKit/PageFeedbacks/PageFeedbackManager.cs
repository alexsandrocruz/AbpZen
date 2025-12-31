using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.PageFeedbacks;

public class PageFeedbackManager : DomainService
{
    protected IPageFeedbackEntityTypeDefinitionStore PageFeedbackEntityTypeDefinitionStore { get; }
    
    public PageFeedbackManager(IPageFeedbackEntityTypeDefinitionStore pageFeedbackEntityTypeDefinitionStore)
    {
        PageFeedbackEntityTypeDefinitionStore = pageFeedbackEntityTypeDefinitionStore;
    }

    public virtual async Task<PageFeedback> CreateAsync(
        [NotNull] string entityType,
        string entityId,
        string url,
        bool isUseful,
        [NotNull] string userNote,
        Guid feedbackUserId)
    {
        if (!await PageFeedbackEntityTypeDefinitionStore.IsDefinedAsync(entityType))
        {
            throw new EntityCantHavePageFeedbackException(entityType);
        }

        return new PageFeedback(GuidGenerator.Create(), entityType, entityId, url, isUseful, userNote, tenantId: CurrentTenant.Id, feedbackUserId: feedbackUserId);
    }
    
    public virtual async Task<PageFeedbackSetting> CreateSettingForEntityTypeAsync(
        [NotNull] string entityType,
        [CanBeNull] string emailAddresses)
    {
        if (!await PageFeedbackEntityTypeDefinitionStore.IsDefinedAsync(entityType))
        {
            throw new EntityCantHavePageFeedbackException(entityType);
        }

        return new PageFeedbackSetting(GuidGenerator.Create(), entityType, emailAddresses, CurrentTenant.Id);
    }
    
    public virtual PageFeedbackSetting CreateDefaultSetting([CanBeNull] string emailAddresses)
    {
        return new PageFeedbackSetting(GuidGenerator.Create(), PageFeedbackConst.DefaultSettingEntityType, emailAddresses, CurrentTenant.Id);
    }
}