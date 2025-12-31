using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Forms.Responses;

public interface IResponseRepository : IBasicRepository<FormResponse, Guid>
{
    public Task<List<FormResponse>> GetListAsync(
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        string sorting = null,
        string filter = null,
        CancellationToken cancellationToken = default
    );

    public Task<List<FormResponse>> GetListByFormIdAsync(
        Guid formId,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        string sorting = null,
        string filter = null,
        CancellationToken cancellationToken = default
    );

    public Task<List<FormWithResponse>> GetByUserId(Guid userId, CancellationToken cancellationToken = default);

    public Task<long> GetCountByFormIdAsync(Guid formId, string filter = null,
        CancellationToken cancellationToken = default);

    public Task<bool> UserResponseExistsAsync(Guid formId, Guid userId, CancellationToken cancellationToken = default);

    public Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default);
}
