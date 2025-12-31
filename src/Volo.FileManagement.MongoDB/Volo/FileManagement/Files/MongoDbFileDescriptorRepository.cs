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
using Volo.FileManagement.MongoDB;

namespace Volo.FileManagement.Files;

public class MongoDbFileDescriptorRepository : MongoDbRepository<IFileManagementMongoDbContext, FileDescriptor, Guid>, IFileDescriptorRepository
{
    public MongoDbFileDescriptorRepository(IMongoDbContextProvider<IFileManagementMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<FileDescriptor> FindAsync(string name, Guid? directoryId = null, CancellationToken cancellationToken = default)
    {
        return await base.FindAsync(x => x.Name == name && x.DirectoryId == directoryId, cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<FileDescriptor>> GetListAsync(
        Guid? directoryId,
        string filter = null,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf(!string.IsNullOrWhiteSpace(filter), x => x.Name.Contains(filter))
            .Where(x => x.DirectoryId == directoryId)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? FileDescriptorConsts.DefaultSorting : sorting)
            .As<IMongoQueryable<FileDescriptor>>()
            .PageBy<FileDescriptor,IMongoQueryable<FileDescriptor>>(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> CountDirectoryFilesAsync(
        Guid? directoryId,
        string filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(x => x.DirectoryId == directoryId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter), x => x.Name.Contains(filter))
            .As<IMongoQueryable<FileDescriptor>>()
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetTotalSizeAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken)).Select(x => x.Size).SumAsync(GetCancellationToken(cancellationToken));
    }
}
