using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Gdpr;

public abstract class GdprRequestRepository_Tests<TStartupModule> : GdprTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected readonly IGdprRequestRepository _gdprRequestRepository;
    protected readonly GdprTestData _gdprTestData;

    public GdprRequestRepository_Tests()
    {
        _gdprRequestRepository = GetRequiredService<IGdprRequestRepository>();
        _gdprTestData = GetRequiredService<GdprTestData>();
    }

    [Fact]
    public async Task GetCountByUserIdAsync()
    {
        var count = await _gdprRequestRepository.GetCountByUserIdAsync(_gdprTestData.UserId1);
        count.ShouldBeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetListByUserIdAsync()
    {
        var requests = await _gdprRequestRepository.GetListAsync(_gdprTestData.UserId1);
        requests.ShouldNotBeEmpty();
        requests.Count.ShouldBeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task FindLatestRequestTimeOfUserAsync()
    {
        var latestRequestTime = await _gdprRequestRepository.FindLatestRequestTimeOfUserAsync(_gdprTestData.UserId1);
        latestRequestTime.ShouldNotBeNull();
    }
}