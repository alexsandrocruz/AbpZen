using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp;
using Volo.CmsKit.Admin.Polls;
using Volo.CmsKit.Polls;
using Xunit;

namespace Volo.CmsKit.Pro.Polls;
public class PollAdminAppService_Tests : CmsKitProApplicationTestBase
{
    private readonly CmsKitProTestData _cmsKitProTestData;
    private readonly IPollAdminAppService _pollAdminAppService;

    public PollAdminAppService_Tests()
    {
        _cmsKitProTestData = GetRequiredService<CmsKitProTestData>();
        _pollAdminAppService = GetRequiredService<IPollAdminAppService>();
    }

    [Fact]
    public async Task CreateAsync()
    {
        var newPoll = new CreatePollDto
        {
            Question = "Have you ever used ABP framework",
            Code = Guid.NewGuid().ToString().Substring(0,8),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddYears(1),
            PollOptions = new System.Collections.ObjectModel.Collection<PollOptionDto>()
            {
                new PollOptionDto() { Text = "Yes"},
                new PollOptionDto() { Text = "No"}
            }
        };
        await _pollAdminAppService.CreateAsync(newPoll);

        UsingDbContext(context =>
        {
            var polls = context.Set<Poll>().ToList();

            polls
                .Any(c => c.Question == newPoll.Question)
                .ShouldBeTrue();
        });
    }
    
    [Fact]
    public async Task CreateWithInvalidDateRangeAsync()
    {
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddYears(1);
        var newPoll = new CreatePollDto
        {
            Question = "Have you ever used ABP framework",
            Code = Guid.NewGuid().ToString().Substring(0,8),
            StartDate = startDate,
            EndDate = endDate,
            Widget = "same",
            PollOptions = new System.Collections.ObjectModel.Collection<PollOptionDto>()
            {
                new PollOptionDto() { Text = "Yes"},
                new PollOptionDto() { Text = "No"}
            }
        };
        
        await _pollAdminAppService.CreateAsync(newPoll);
        
        var invalidNewPoll = new CreatePollDto
        {
            Question = "Have you ever used ABP framework",
            Code = Guid.NewGuid().ToString().Substring(0,8),
            StartDate = startDate,
            EndDate = endDate,
            Widget = "same",
            PollOptions = new System.Collections.ObjectModel.Collection<PollOptionDto>()
            {
                new PollOptionDto() { Text = "Yes"},
                new PollOptionDto() { Text = "No"}
            }
        };

        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _pollAdminAppService.CreateAsync(invalidNewPoll);
        });
    }

    [Fact]
    public async Task UpdateAsync()
    {
        var poll = await _pollAdminAppService.UpdateAsync(_cmsKitProTestData.PollId, new UpdatePollDto
        {
            Question = "Have you ever used ABP framework?",
            Code= _cmsKitProTestData.Code
        });

        UsingDbContext(context =>
        {
            var updatedPoll = context.Set<Poll>().FirstOrDefault(u => u.Id == _cmsKitProTestData.PollId);

            updatedPoll.ShouldNotBeNull();
            updatedPoll.Question.ShouldBe(poll.Question);
        });
    }
    
    [Fact]
    public async Task UpdateWithInvalidDateRangeAsync()
    {
        var startDate = DateTime.Now.AddYears(1);
        var endDate = DateTime.Now.AddYears(2);
        
        var newPoll = new CreatePollDto
        {
            Question = "Have you ever used ABP framework",
            Code = Guid.NewGuid().ToString().Substring(0,8),
            StartDate = startDate,
            EndDate = endDate,
            Widget = "same",
            PollOptions = new System.Collections.ObjectModel.Collection<PollOptionDto>()
            {
                new PollOptionDto() { Text = "Yes"},
                new PollOptionDto() { Text = "No"}
            }
        };
        
        await _pollAdminAppService.CreateAsync(newPoll);
        
        await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            await _pollAdminAppService.UpdateAsync(_cmsKitProTestData.PollId, new UpdatePollDto
            {
                Question = "Have you ever used ABP framework?",
                Code= _cmsKitProTestData.Code,
                StartDate = startDate,
                EndDate = endDate,
                Widget = "same"
            });
        });
    }

    [Fact]
    public async Task DeleteAsync()
    {
        await _pollAdminAppService.DeleteAsync(_cmsKitProTestData.PollId);

        UsingDbContext(context =>
        {
            var deletedPoll = context.Set<Poll>().FirstOrDefault(u => u.Id == _cmsKitProTestData.PollId);

            deletedPoll.ShouldBeNull();
        });
    }

    [Fact]
    public async Task GetListAsync()
    {
        var result = await _pollAdminAppService.GetListAsync(new GetPollListInput
        {
            Filter = null
        });

        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(1);
        result.Items.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetAsync()
    {
        var poll = await _pollAdminAppService.GetAsync(_cmsKitProTestData.PollId);

        poll.ShouldNotBeNull();
        poll.Id.ShouldBe(_cmsKitProTestData.PollId);
    }

    [Fact]
    public async Task GetWidgetsAsync()
    {
        var result = await _pollAdminAppService.GetWidgetsAsync();
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetResultAsync()
    {
        var result = await _pollAdminAppService.GetResultAsync(_cmsKitProTestData.PollId);

        result.ShouldNotBeNull();
        result.Question.ShouldBe(_cmsKitProTestData.Question);
    }

}
