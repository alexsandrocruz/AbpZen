using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Payment.Requests;

namespace Volo.Payment.EntityFrameworkCore;

public class EfCorePaymentRequestRepository : EfCoreRepository<IPaymentDbContext, PaymentRequest, Guid>,
    IPaymentRequestRepository
{
    public EfCorePaymentRequestRepository(IDbContextProvider<IPaymentDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public override async Task<IQueryable<PaymentRequest>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }

    public virtual async Task<List<PaymentRequest>> GetListAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default
    )
    {
        return await (await GetDbSetAsync())
            .Where(x => x.State != PaymentRequestState.Completed && x.CreationTime >= startDate &&
                        x.CreationTime <= endDate && x.CreatorId.HasValue)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<PaymentRequest> GetBySubscriptionAsync(string externalSubscriptionId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
                   .FirstOrDefaultAsync(x => x.ExternalSubscriptionId == externalSubscriptionId,
                       GetCancellationToken(cancellationToken))
               ?? throw new EntityNotFoundException(typeof(PaymentRequest));
    }

    public virtual async Task<List<PaymentRequest>> GetPagedListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter,
        DateTime? creationDateMax = null,
        DateTime? creationDateMin = null,
        PaymentType? paymentType = null,
        PaymentRequestState? state = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var query = CreateFilteredQuery(
                includeDetails ? await WithDetailsAsync() : await GetQueryableAsync(),
                filter,
                creationDateMax,
                creationDateMin,
                paymentType,
                state)
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(PaymentRequest.CreationTime) : sorting)
            .Skip(skipCount)
            .Take(maxResultCount);

        return await query.ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> GetCountAsync(
        string filter,
        DateTime? creationDateMax = null,
        DateTime? creationDateMin = null,
        PaymentType? paymentType = null,
        PaymentRequestState? state = null,
        CancellationToken cancellationToken = default)
    {
        var query = CreateFilteredQuery(
                        await GetQueryableAsync(),
                        filter,
                        creationDateMax,
                        creationDateMin,
                        paymentType,
                        state);

        return await (query).CountAsync(GetCancellationToken(cancellationToken));
    }

    private IQueryable<PaymentRequest> CreateFilteredQuery(
        IQueryable<PaymentRequest> queryable,
        string filter,
        DateTime? creationDateMax = null,
        DateTime? creationDateMin = null,
        PaymentType? paymentType = null,
        PaymentRequestState? state = null)
    {
        return queryable
            .WhereIf(!filter.IsNullOrEmpty(),
                p => p.Currency.Contains(filter) || p.Gateway.Contains(filter) ||
                        p.ExternalSubscriptionId == filter)
            .WhereIf(creationDateMax != null, p => p.CreationTime <= creationDateMax)
            .WhereIf(creationDateMin != null, p => p.CreationTime >= creationDateMin)
            .WhereIf(paymentType != null, p => p.Products.Any(pr => pr.PaymentType == paymentType))
            .WhereIf(state != null, p => p.State == state);
    }
}
