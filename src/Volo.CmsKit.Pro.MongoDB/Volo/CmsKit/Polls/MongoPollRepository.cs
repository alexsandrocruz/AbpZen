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

namespace Volo.CmsKit.Polls;
public class MongoPollRepository : MongoDbRepository<ICmsKitProMongoDbContext, Poll, Guid>, IPollRepository
{
    public MongoPollRepository(IMongoDbContextProvider<ICmsKitProMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<Poll> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken)).FirstOrDefaultAsync(p => p.Id == id);
    }

    public virtual async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf(!filter.IsNullOrWhiteSpace(), t => t.Question.Contains(filter) || t.Name.Contains(filter))
            .As<IMongoQueryable<Poll>>()
            .LongCountAsync(cancellationToken: GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Poll>> GetListAsync(
            string filter = null,
            string sorting = null,
            int skipCount = 0,
            int maxResultCount = Int32.MaxValue,
            CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);

        var query = (await GetMongoQueryableAsync(token))
            .WhereIf(!filter.IsNullOrWhiteSpace(), t => t.Question.Contains(filter) || t.Name.Contains(filter));

        query = query.OrderBy(sorting.IsNullOrEmpty() ? $"{nameof(Poll.CreationTime)} desc" : sorting);

        return await query.As<IMongoQueryable<Poll>>()
            .PageBy<Poll, IMongoQueryable<Poll>>(skipCount, maxResultCount)
            .ToListAsync(token);
    }

    public virtual async Task<List<Poll>> GetListByWidgetAsync(string widget, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(widget))
        {
            return new List<Poll>();
        }

        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(p => p.Widget == widget).
            ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<Poll> FindByAvailableWidgetAsync(string widget, DateTime now, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(widget))
        {
            return null;
        }

        return await (await GetMongoQueryableAsync(cancellationToken))
            .FirstOrDefaultAsync(p =>
                    p.Widget == widget && ((p.StartDate <= now && (p.EndDate.HasValue == false || p.EndDate >= now)) ||
                                           (p.ResultShowingEndDate.HasValue && p.ResultShowingEndDate >= now))
                , GetCancellationToken(cancellationToken));
    }

    public virtual async Task<Poll> FindByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return null;
        }

        return (await (await GetMongoQueryableAsync(cancellationToken))
            .FirstOrDefaultAsync(p => p.Code == code, GetCancellationToken(cancellationToken)));
    }

    public virtual async Task<Dictionary<Poll, List<PollUserVote>>> GetPollWithPollUserVotesAsync(Guid id)
    {
        var dbContext = await GetDbContextAsync();
        var poll = (await GetMongoQueryableAsync()).Where(x => x.Id == id);

        var query = from p in poll
                    join puv in dbContext.PollUserVotes.AsQueryable() on p.Id equals puv.PollId into iL2
                    select new {
                        poll = p,
                        pollUserVotes = iL2
                    };

        var result = await AsyncExecuter.ToListAsync(query);

        var map = new Dictionary<Poll, List<PollUserVote>>();

        foreach (var row in result)
        {
            if (!map.ContainsKey(row.poll))
            {
                map.Add(row.poll, new List<PollUserVote>());
            }

            if (row.pollUserVotes != null)
            {
                map[row.poll] = row.pollUserVotes.ToList();
            }
        }

        return map;
    }

    public virtual async Task<PollWithUserVotes> GetPollWithPollUserVotesAsync(Guid id, Guid userId)
    {
        var dbContext = await GetDbContextAsync();
        return await (from poll in await GetMongoQueryableAsync()
            join userVote in dbContext.PollUserVotes
                on poll.Id equals userVote.PollId into userVotes
            where poll.Id == id
            select new PollWithUserVotes
            {
                Poll = poll,
                UserVotes = userVotes.Where(x => x.PollId == id && x.UserId == userId)
            }).FirstAsync();
    }

    public virtual async Task<Poll> FindByDateRangeAndWidgetAsync(DateTime startDate, DateTime? endDate = null, DateTime? resultShowingEndDate = null, string widget = null,
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

        return await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken))).FirstOrDefaultAsync(expression, cancellationToken: GetCancellationToken(cancellationToken));
    }
}
