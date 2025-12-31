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

namespace Volo.CmsKit.Polls;
public class EfCorePollRepository : EfCoreRepository<ICmsKitProDbContext, Poll, Guid>, IPollRepository
{
    public EfCorePollRepository(IDbContextProvider<ICmsKitProDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<Poll>> WithDetailsAsync()
    {
        return (await GetQueryableAsync())
            .IncludeDetails();
    }

    public virtual async Task<Dictionary<Poll, List<PollUserVote>>> GetPollWithPollUserVotesAsync(Guid id)
    {
        var dbContext = await GetDbContextAsync();
        
        var query = from p in await WithDetailsAsync()
            join puv in dbContext.PollUserVotes on p.Id equals puv.PollId into iL2
            from iL3 in iL2.DefaultIfEmpty()
            where p.Id == id
            select new {
                poll = p,
                pollUserVotes = iL3
            };

        var result = await query.ToListAsync();

        var map = new Dictionary<Poll, List<PollUserVote>>();

        foreach (var row in result)
        {
            if (!map.ContainsKey(row.poll))
            {
                map.Add(row.poll, new List<PollUserVote>());
            }

            if (row.pollUserVotes != null)
            {
                map[row.poll].Add(row.pollUserVotes);
            }
        }

        return map;
    }

    public virtual async Task<PollWithUserVotes> GetPollWithPollUserVotesAsync(Guid id, Guid userId)
    {
        var dbContext = await GetDbContextAsync();
        var poll = await (await WithDetailsAsync()).Where(x => x.Id == id).FirstAsync();

        return new PollWithUserVotes 
        {
            Poll = poll,
            UserVotes = await dbContext.PollUserVotes.Where(x => x.PollId == id && x.UserId == userId).ToListAsync()
        };
    }

    public async Task<Poll> FindByDateRangeAndWidgetAsync(DateTime startDate, DateTime? endDate = null, DateTime? resultShowingEndDate = null, string widget = null,
        CancellationToken cancellationToken = default)
    {
        var expression = PredicateBuilder.New<Poll>();
        
        expression = expression.And(p => p.StartDate <= startDate && (!p.EndDate.HasValue || p.EndDate.Value >= startDate));
        
        if (endDate.HasValue)
        {
            expression = expression.Or(p => p.StartDate <= endDate && p.EndDate >= endDate);
        }
        
        if (resultShowingEndDate.HasValue)
        {
            expression = expression.Or(p => p.StartDate <= resultShowingEndDate && (!p.EndDate.HasValue || p.EndDate.Value >= resultShowingEndDate));
        }
        
        expression = expression.And(p => p.Widget == widget);

        return await (await GetDbSetAsync()).FirstOrDefaultAsync(expression, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Poll>> GetListByWidgetAsync(string widget, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(widget))
        {
            return new List<Poll>();
        }

        return await (await GetDbSetAsync())
            .Where(p => p.Widget == widget).
            ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<Poll> FindByAvailableWidgetAsync(string widget, DateTime now, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(widget))
        {
            return null;
        }

        return await (await GetDbSetAsync())
            .IncludeDetails()
            .FirstOrDefaultAsync(p => p.Widget == widget && (
                    (p.StartDate <= now && (p.EndDate.HasValue == false || p.EndDate >= now)) || 
                    (p.ResultShowingEndDate.HasValue && p.ResultShowingEndDate >= now)),
                GetCancellationToken(cancellationToken));
        }

    public virtual async Task<Poll> FindByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return null;
        }

        return (await (await GetDbSetAsync())
            .IncludeDetails()
            .FirstOrDefaultAsync(p => p.Code == code, GetCancellationToken(cancellationToken)));
    }

    public virtual async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
                .WhereIf(!filter.IsNullOrWhiteSpace(), t => t.Question.Contains(filter) || t.Name.Contains(filter))
                .LongCountAsync(cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Poll>> GetListAsync(string filter = null, string sorting = null, int skipCount = 0, int maxResultCount = int.MaxValue, CancellationToken cancellationToken = default)
    {
        var query = (await GetDbSetAsync())
               .WhereIf(!filter.IsNullOrWhiteSpace(), t => t.Question.Contains(filter) || t.Name.Contains(filter));

        query = query.OrderBy(sorting.IsNullOrEmpty() ? $"{nameof(Poll.CreationTime)} desc" : sorting);

        return await query.PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
