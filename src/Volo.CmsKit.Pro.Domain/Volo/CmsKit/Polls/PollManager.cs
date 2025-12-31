using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Volo.CmsKit.Polls;
public class PollManager : CmsKitProDomainServiceBase
{
    protected IPollRepository PollRepository { get; }
    protected IPollUserVoteRepository PollUserVoteRepository { get; }

    public PollManager(IPollRepository pollRepository, IPollUserVoteRepository pollUserVoteRepository)
    {
        PollRepository = pollRepository;
        PollUserVoteRepository = pollUserVoteRepository;
    }

    public virtual async Task SubmitVoteAsync(Guid id, Guid userId, Guid[] pollOptionIds)
    {
        try
        {
            var pollWithVotes = await PollRepository.GetPollWithPollUserVotesAsync(id, userId);

            var poll = pollWithVotes.Poll;
            var votedList = pollWithVotes.UserVotes.ToList();

            if (votedList.Any())
            {
                throw new PollUserVoteVotedBySameUserException(userId, poll.Id);
            }
            
            var votes = new List<PollUserVote>();

            if (poll.AllowMultipleVote)
            {
                foreach (var pollOptionId in pollOptionIds)
                {
                    AddPollUserVote(userId, poll, votes, pollOptionId, votedList);
                    poll.Increase();
                }
            }
            else
            {
                if (pollOptionIds.Length != 1)
                {
                    throw new PollAllowSingleVoteException(poll.AllowMultipleVote);
                }

                var pollOptionId = pollOptionIds.First();
                AddPollUserVote(userId, poll, votes, pollOptionId, votedList);
                poll.Increase();
            }
        
            await PollUserVoteRepository.InsertManyAsync(votes);

            await PollRepository.UpdateAsync(poll, autoSave: true);
        }
        catch (AbpDbConcurrencyException e)
        {
            Logger.LogException(e);
            throw new BusinessException(CmsKitProErrorCodes.Poll.PollSubmitVoteConcurrencyException);
        }
    }

    private void AddPollUserVote(Guid userId, Poll poll, List<PollUserVote> votes, Guid pollOptionId, List<PollUserVote> votedList)
    {
        if (poll.PollOptions.All(p => p.Id != pollOptionId))
        {
            throw new EntityNotFoundException(typeof(Guid), pollOptionId);
        }   
        
        votes.Add(new PollUserVote(GuidGenerator.Create(), poll.Id, userId, pollOptionId, poll.TenantId));

        poll.PollOptions.First(p => p.Id == pollOptionId).Increase();
    }

    public virtual async Task EnsureExistAsync(string code)
    {
        var pool = await PollRepository.FindByCodeAsync(code);
        if (pool is not null)
        {
            throw new PollHasAlreadySameCodeException(code);
        }
    }
}
