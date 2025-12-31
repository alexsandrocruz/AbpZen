using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.CmsKit.Polls;
using Xunit;

namespace Volo.CmsKit.Pro.Polls;
public abstract class PollRepository_Tests<TStartupModule> : CmsKitProTestBase<TStartupModule>
        where TStartupModule : IAbpModule
{
    private readonly CmsKitProTestData _cmsKitProTestData;
    private readonly IPollRepository _pollRepository;

    protected PollRepository_Tests()
    {
        _cmsKitProTestData = GetRequiredService<CmsKitProTestData>();
        _pollRepository = GetRequiredService<IPollRepository>();
    }
    [Fact]
    public async Task GetListAsync()
    {
        var polls = await _pollRepository.GetListAsync();

        polls.ShouldNotBeNull();
        polls.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetCountAsync()
    {
        var count = await _pollRepository.GetCountAsync(null);

        count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetPollWithPollUserVotesAsync()
    {
        var poll = await _pollRepository.GetPollWithPollUserVotesAsync(
            _cmsKitProTestData.PollId);

        poll.Keys.First().Id.ShouldBe(_cmsKitProTestData.PollId);
    }

    [Fact]
    public async Task WithDetailsAsync()
    {
        var pollWithDetails = await _pollRepository.WithDetailsAsync();
        pollWithDetails.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetListByWidgetAsync()
    {
        var polls = await _pollRepository.GetListByWidgetAsync(_cmsKitProTestData.Widget);
        
        polls.ShouldNotBeNull();
        polls.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetIdByWidgetAsync()
    {
        var poll = await _pollRepository.FindByAvailableWidgetAsync(_cmsKitProTestData.Widget, DateTime.Now);
        
        poll.ShouldNotBeNull();
        poll.Id.ShouldBe(_cmsKitProTestData.PollId);
    }

    [Fact]
    public async Task GetIdByNameAsync()
    {
        var poll = await _pollRepository.FindByCodeAsync(_cmsKitProTestData.Code);
        
        poll.ShouldNotBeNull();
        poll.Id.ShouldBe(_cmsKitProTestData.PollId);
    }
    
    [Fact]
    public async Task GetPollWithPollUserVotesById_UserId_Async()
    {
        var pollWithPollUserVotes = await _pollRepository.GetPollWithPollUserVotesAsync(_cmsKitProTestData.PollId,  _cmsKitProTestData.User1Id);
        
        pollWithPollUserVotes.Poll.Id.ShouldBe(_cmsKitProTestData.PollId);
        pollWithPollUserVotes.Poll.PollOptions.Count.ShouldBe(2);
        pollWithPollUserVotes.Poll.PollOptions.First().Id.ShouldBe(_cmsKitProTestData.PollOptionId);
        pollWithPollUserVotes.Poll.PollOptions.Last().Id.ShouldBe(_cmsKitProTestData.PollOptionId2);
        pollWithPollUserVotes.UserVotes.Count().ShouldBe(1);
        pollWithPollUserVotes.UserVotes.First().PollOptionId.ShouldBe(_cmsKitProTestData.PollOptionId);
        pollWithPollUserVotes.UserVotes.First().UserId.ShouldBe(_cmsKitProTestData.User1Id);
    }
}
