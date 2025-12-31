using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.FileManagement.EntityFrameworkCore;

namespace Volo.FileManagement.Files;

public class EfCoreFileDescriptorRepository : EfCoreRepository<IFileManagementDbContext, FileDescriptor, Guid>, IFileDescriptorRepository
{
    public EfCoreFileDescriptorRepository(IDbContextProvider<IFileManagementDbContext> dbContextProvider) : base(dbContextProvider)
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
        return await (await GetDbSetAsync()).Where(x => x.DirectoryId == directoryId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter), x => x.Name.Contains(filter))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? FileDescriptorConsts.DefaultSorting : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> CountDirectoryFilesAsync(
        Guid? directoryId,
        string filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.DirectoryId == directoryId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter), x => x.Name.Contains(filter))
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetTotalSizeAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Select(x => x.Size).SumAsync(GetCancellationToken(cancellationToken));
    }
}
