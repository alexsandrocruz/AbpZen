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
using Volo.Forms.Forms;
using Volo.Forms.MongoDB;
using Volo.Forms.Responses;

namespace Volo.Forms;

public class MongoFormResponseRepository : MongoDbRepository<IFormsMongoDbContext, FormResponse, Guid>, IResponseRepository
{
    public MongoFormResponseRepository(IMongoDbContextProvider<IFormsMongoDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public virtual async Task<List<FormResponse>> GetListAsync(
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        string sorting = null,
        string filter = null,
        CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);

        return await (await GetMongoQueryableAsync(token))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(FormResponse.CreationTime) : sorting)
            .As<IMongoQueryable<FormResponse>>()
            .PageBy<FormResponse, IMongoQueryable<FormResponse>>(skipCount, maxResultCount)
            .ToListAsync(token);
    }

    public virtual async Task<List<FormResponse>> GetListByFormIdAsync(
        Guid formId,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        string sorting = null,
        string filter = null,
        CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);

        return await (await GetMongoQueryableAsync(token))
            .Where(q => q.FormId == formId)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(FormResponse.CreationTime) : sorting)
            .As<IMongoQueryable<FormResponse>>()
            .PageBy<FormResponse, IMongoQueryable<FormResponse>>(skipCount, maxResultCount)
            .ToListAsync(token);
    }

    public virtual async Task<List<FormWithResponse>> GetByUserId(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);

        var responses = await (await GetMongoQueryableAsync<FormResponse>(token))
            .Where(q => q.UserId == userId)
            .ToListAsync(token);
        var formIds = responses.Select(q => q.FormId).Distinct().ToList();

        var forms = await (await GetMongoQueryableAsync<Form>(token))
            .Where(q => formIds.Contains(q.Id))
            .ToListAsync(token);

        return responses.Select(response => new FormWithResponse()
        {
            Response = response,
            Form = forms.FirstOrDefault(q => q.Id == response.FormId)
        }).ToList();
    }

    public virtual async Task<long> GetCountByFormIdAsync(
        Guid formId,
        string filter = null,
        CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);

        return await (await GetMongoQueryableAsync(token))
            .As<IMongoQueryable<FormResponse>>()
            .LongCountAsync(q => q.FormId == formId, token);
    }

    public virtual async Task<bool> UserResponseExistsAsync(Guid formId, Guid userId, CancellationToken cancellationToken = default)
    {
        var queryable = (await GetQueryableAsync()).As<IMongoQueryable<FormResponse>>();

        return await queryable.AnyAsync(x =>
                    x.FormId == formId &&
                    x.UserId.HasValue &&
                    x.UserId == userId,
                GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);

        return await (await GetMongoQueryableAsync(token))
            .As<IMongoQueryable<FormResponse>>()
            .LongCountAsync(token);
    }
}
