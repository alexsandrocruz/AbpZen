using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.MongoDB;

namespace Volo.CmsKit.PageFeedbacks;

public class MongoPageFeedbackRepository : MongoDbRepository<ICmsKitProMongoDbContext, PageFeedback, Guid>, IPageFeedbackRepository
{
    public MongoPageFeedbackRepository(IMongoDbContextProvider<ICmsKitProMongoDbContext> dbContextProvider) : base(dbContextProvider)
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
            .As<IMongoQueryable<PageFeedback>>()
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

    protected virtual async Task<IMongoQueryable<PageFeedback>> GetFilteredQueryableAsync(
        string entityType = null,
        string entityId = null,
        bool? isUseful = null,
        string url = null,
        bool? isHandled = null,
        bool? hasUserNote = null,
        bool? hasAdminNote = null)
    {
        return (await GetMongoQueryableAsync())
                .WhereIf(!entityType.IsNullOrWhiteSpace(), x => x.EntityType == entityType)
                .WhereIf(!entityId.IsNullOrWhiteSpace(), x => x.EntityId == entityId)
                .WhereIf(isUseful.HasValue, x => x.IsUseful == isUseful)
                .WhereIf(!url.IsNullOrWhiteSpace(), x => x.Url == url)
                .WhereIf(isHandled.HasValue, x => x.IsHandled == isHandled)
                .WhereIf(hasUserNote.HasValue, x => string.IsNullOrWhiteSpace(x.UserNote) != hasUserNote)
                .WhereIf(hasAdminNote.HasValue, x => string.IsNullOrWhiteSpace(x.AdminNote) != hasAdminNote)
                .As<IMongoQueryable<PageFeedback>>();
    }
}