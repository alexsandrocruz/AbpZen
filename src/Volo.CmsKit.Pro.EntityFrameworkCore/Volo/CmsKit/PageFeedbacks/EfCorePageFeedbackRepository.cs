using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.PageFeedbacks;

public class EfCorePageFeedbackRepository : EfCoreRepository<ICmsKitProDbContext, PageFeedback, Guid>,
    IPageFeedbackRepository
{
    public EfCorePageFeedbackRepository(IDbContextProvider<ICmsKitProDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public virtual async Task<List<PageFeedback>> GetListAsync(
        string entityType = null, 
        string entityId = null,
        bool? isUseful = null, 
        string url = null,
        bool? isHandled = null, 
        string sorting = null, 
        int skipCount = 0, 
        int maxResultCount = int.MaxValue,
        bool? hasUserNote = null,
        bool? hasAdminNote = null,
        CancellationToken cancellationToken = default)
    {
        var queryable = await GetFilteredQueryableAsync(entityType, entityId, isUseful, url, isHandled, hasUserNote, hasAdminNote);
        return await queryable
            .OrderBy(sorting ?? $"{nameof(PageFeedback.CreationTime)} DESC")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetCountAsync(
        string entityType = null, 
        string entityId = null, 
        bool? isUseful = null,
        string url = null,
        bool? isHandled = null, 
        bool? hasUserNote = null,
        bool? hasAdminNote = null,
        CancellationToken cancellationToken = default)
    {
        var queryable = await GetFilteredQueryableAsync(entityType, entityId, isUseful, url, isHandled, hasUserNote, hasAdminNote);
        return await queryable.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual async Task<IQueryable<PageFeedback>> GetFilteredQueryableAsync(
        string entityType = null,
        string entityId = null,
        bool? isUseful = null,
        string url = null,
        bool? isHandled = null,
        bool? hasUserNote = null,
        bool? hasAdminNote = null)
    {
        return (await GetQueryableAsync())
            .WhereIf(!entityType.IsNullOrWhiteSpace(), x => x.EntityType == entityType)
            .WhereIf(!entityId.IsNullOrWhiteSpace(), x => x.EntityId == entityId)
            .WhereIf(isUseful.HasValue, x => x.IsUseful == isUseful)
            .WhereIf(!url.IsNullOrWhiteSpace(), x => x.Url == url)
            .WhereIf(isHandled.HasValue, x => x.IsHandled == isHandled)
            .WhereIf(hasUserNote.HasValue, x => string.IsNullOrWhiteSpace(x.UserNote) != hasUserNote)
            .WhereIf(hasAdminNote.HasValue, x => string.IsNullOrWhiteSpace(x.AdminNote) != hasAdminNote);
    }
}