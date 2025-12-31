using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using Volo.CmsKit.Polls;
using Volo.CmsKit.Public.Polls;
using Xunit;

namespace Volo.CmsKit.Pro.Polls;
public class PollPublicAppService_Tests : CmsKitProApplicationTestBase
{
    private readonly IPollPublicAppService _pollPublicAppService;
    private readonly CmsKitProTestData _cmsKitProTestData;
    private readonly ICurrentUser _currentUser;
    public PollPublicAppService_Tests()
    {
        _pollPublicAppService = GetRequiredService<IPollPublicAppService>();
        _cmsKitProTestData = GetRequiredService<CmsKitProTestData>();
        _currentUser = GetRequiredService<ICurrentUser>();
    }

    [Fact]
    public async Task FindByWidgetAsync()
    {
        var poll = await _pollPublicAppService.FindByAvailableWidgetAsync(_cmsKitProTestData.Widget);

        poll.ShouldNotBeNull();
        poll.Id.ShouldBe(_cmsKitProTestData.PollId);
    }

    [Fact]
    public async Task FindByCodeAsync()
    {
        var poll = await _pollPublicAppService.FindByCodeAsync(_cmsKitProTestData.Code);

        poll.ShouldNotBeNull();
        poll.Id.ShouldBe(_cmsKitProTestData.PollId);
    }

    [Fact]
    public async Task SubmitVoteAsync()
    {
        await _pollPublicAppService.SubmitVoteAsync(
            _cmsKitProTestData.PollId,
            new SubmitPollInput()
            {
                PollOptionIds = new[] { _cmsKitProTestData.PollOptionId }
            });

        UsingDbContext(context =>
        {
            var pollUserVotes = context.Set<PollUserVote>().ToList();

            pollUserVotes
                .Any(c => c.PollId == _cmsKitProTestData.PollId && c.UserId == _currentUser.Id)
                .ShouldBeTrue();
        });
    }
    
    [Fact]
    public async Task SubmitMultipleVoteAsync()
    {
        var poll = await _pollPublicAppService.FindByCodeAsync(_cmsKitProTestData.Code);
        
        var voteCountBefore = poll.VoteCount;
        
        await _pollPublicAppService.SubmitVoteAsync(
            _cmsKitProTestData.PollId,
            new SubmitPollInput()
            {
                PollOptionIds = new[] { _cmsKitProTestData.PollOptionId, _cmsKitProTestData.PollOptionId2}
            });

        UsingDbContext(context =>
        {
            var pollUserVotes = context.Set<PollUserVote>().ToList();

            pollUserVotes
                .Any(c => c.PollId == _cmsKitProTestData.PollId)
                .ShouldBeTrue();
        });
        
        poll = await _pollPublicAppService.FindByCodeAsync(_cmsKitProTestData.Code);
        
        var voteCountAfter = poll.VoteCount;
        
        voteCountAfter.ShouldBeGreaterThanOrEqualTo(voteCountBefore + 2);
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpUnitOfWorkDefaultOptions>(options =>
        {
            options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
        });
    }

    [Fact]
    public async Task GetResultAsync()
    {
        var url = await _pollPublicAppService.GetResultAsync(_cmsKitProTestData.PollId);

        url.ShouldNotBeNull();
        url.Question.ShouldBe(_cmsKitProTestData.Question);
    }

}
