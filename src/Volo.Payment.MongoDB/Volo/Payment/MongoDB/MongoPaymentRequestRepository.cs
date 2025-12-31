using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Payment.Requests;

namespace Volo.Payment.MongoDB;

public class MongoPaymentRequestRepository : MongoDbRepository<IPaymentMongoDbContext, PaymentRequest, Guid>, IPaymentRequestRepository
{
    public MongoPaymentRequestRepository(IMongoDbContextProvider<IPaymentMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<List<PaymentRequest>> GetListAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default
    )
    {
        return await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken)))
            .Where(x => x.State != PaymentRequestState.Completed && x.CreationTime >= startDate &&
                        x.CreationTime <= endDate && x.CreatorId.HasValue)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<PaymentRequest> GetBySubscriptionAsync(string externalSubscriptionId, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken)))
            .FirstOrDefaultAsync(x => x.ExternalSubscriptionId == externalSubscriptionId)
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
        var query = (await CreateFilteredQueryAsync(
                filter,
                creationDateMax,
                creationDateMin,
                paymentType,
                state,
                cancellationToken))
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(PaymentRequest.CreationTime) : sorting)
            .Skip(skipCount)
            .Take(maxResultCount);

        return await AsyncExecuter.ToListAsync(query, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> GetCountAsync(
        string filter,
        DateTime? creationDateMax = null,
        DateTime? creationDateMin = null,
        PaymentType? paymentType = null,
        PaymentRequestState? state = null,
        CancellationToken cancellationToken = default)
    {
        return await (await CreateFilteredQueryAsync(filter, creationDateMax, creationDateMin, paymentType, state, cancellationToken))
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    private async Task<IMongoQueryable<PaymentRequest>> CreateFilteredQueryAsync(
        string filter,
        DateTime? creationDateMax = null,
        DateTime? creationDateMin = null,
        PaymentType? paymentType = null,
        PaymentRequestState? state = null,
        CancellationToken cancellationToken = default)
    {
        var queryable = (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken)));

        if (!filter.IsNullOrEmpty())
        {
            queryable = queryable.Where(p =>
                p.Currency.ToLower().Contains(filter)
                || p.Gateway.ToLower().Contains(filter)
                || p.ExternalSubscriptionId == filter);
        }

        if (creationDateMax != null)
        {
            queryable = queryable.Where(p => p.CreationTime <= creationDateMax);
        }

        if (creationDateMin != null)
        {
            queryable = queryable.Where(p => p.CreationTime <= creationDateMin);
        }

        if (paymentType != null)
        {
            queryable = queryable.Where(p => p.Products.Any(pr => pr.PaymentType == paymentType));
        }

        if (state != null)
        {
            queryable = queryable.Where(p => p.State == state);
        }

        return queryable;
    }
}
