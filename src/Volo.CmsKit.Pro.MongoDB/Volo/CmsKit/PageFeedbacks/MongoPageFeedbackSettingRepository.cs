using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Volo.CmsKit.PageFeedbacks;

public class MongoPageFeedbackSettingRepository : MongoDbRepository<ICmsKitProMongoDbContext, PageFeedbackSetting, Guid>, IPageFeedbackSettingRepository
{
    public MongoPageFeedbackSettingRepository(IMongoDbContextProvider<ICmsKitProMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<List<PageFeedbackSetting>> GetListByEntityTypesAsync(List<string> entityTypes, CancellationToken cancellationToken = default)
    {
        var queryable = await GetMongoQueryableAsync(GetCancellationToken(cancellationToken));
        return await queryable.Where(x => entityTypes.Contains(x.EntityType)).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<PageFeedbackSetting> FindByEntityTypeAsync(string entityType,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken)))
            .Where(x => x.EntityType == entityType)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task DeleteOldSettingsAsync(List<string> existingEntityTypes,
        CancellationToken cancellationToken = default)
    {
        await (await GetCollectionAsync(GetCancellationToken(cancellationToken))).DeleteManyAsync(x=> !existingEntityTypes.Contains(x.EntityType), GetCancellationToken(cancellationToken));
    }
}