using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.PageFeedbacks;

public class EfCorePageFeedbackSettingRepository : EfCoreRepository<ICmsKitProDbContext, PageFeedbackSetting, Guid>,
    IPageFeedbackSettingRepository
{
    public EfCorePageFeedbackSettingRepository(IDbContextProvider<ICmsKitProDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public virtual async Task<List<PageFeedbackSetting>> GetListByEntityTypesAsync(List<string> entityTypes, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => entityTypes.Contains(x.EntityType))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<PageFeedbackSetting> FindByEntityTypeAsync(
        string entityType,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.EntityType == entityType)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task DeleteOldSettingsAsync(
        List<string> existingEntityTypes,
        CancellationToken cancellationToken = default)
    {
        await (await GetQueryableAsync())
            .Where(x => !existingEntityTypes.Contains(x.EntityType))
            .ExecuteDeleteAsync(GetCancellationToken(cancellationToken));
    }
}