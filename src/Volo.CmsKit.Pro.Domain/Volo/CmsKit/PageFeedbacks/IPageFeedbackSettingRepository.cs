using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.PageFeedbacks;

public interface IPageFeedbackSettingRepository : IBasicRepository<PageFeedbackSetting, Guid>
{
    Task<List<PageFeedbackSetting>> GetListByEntityTypesAsync(List<string> entityTypes, CancellationToken cancellationToken = default);

    Task<PageFeedbackSetting> FindByEntityTypeAsync(
        [CanBeNull] string entityType,
        CancellationToken cancellationToken = default
    );

    Task DeleteOldSettingsAsync(List<string> existingEntityTypes, CancellationToken cancellationToken = default);
}