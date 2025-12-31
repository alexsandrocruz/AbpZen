using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Authorization;
using Volo.Abp.Users;
using Xunit;

namespace Volo.Abp.Gdpr;

public class GdprRequestAppService_Tests : GdprApplicationTestBase
{
    protected IGdprRequestAppService _gdprRequestAppService;
    protected ICurrentUser _currentUser;
    protected GdprTestData _gdprTestData;

    public GdprRequestAppService_Tests()
    {
        _gdprRequestAppService = GetRequiredService<IGdprRequestAppService>();
        _gdprTestData = GetRequiredService<GdprTestData>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _currentUser = Substitute.For<ICurrentUser>();
        services.AddSingleton(_currentUser);
    }

    [Fact]
    public async Task IsNewRequestAllowedAsync()
    {
        Login(_gdprTestData.UserId1);

        var requestAllowed = await _gdprRequestAppService.IsNewRequestAllowedAsync();
        requestAllowed.ShouldBeFalse();
    }

    [Fact]
    public async Task GetListByUserIdAsync()
    {
        Login(_gdprTestData.UserId1);
            
        var gdprRequests = await _gdprRequestAppService.GetListAsync(new GdprRequestInput
        {
            UserId = _currentUser.GetId()
        });
        gdprRequests.Items.ShouldNotBeEmpty();
        gdprRequests.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetUserDataAsync_Should_Throw_AbpAuthorizationException_IfTokenNotValid()
    {
        Should.Throw<AbpAuthorizationException>(async () =>
        {
            var token = "<wrong-download-token>";
            await _gdprRequestAppService.GetUserDataAsync(_gdprTestData.RequestId1, token);
        });
    }

    [Fact]
    public async Task PrepareDataAsync_Should_Throw_BusinessException_IfRequestIsNotAllowed()
    {
        Should.Throw<BusinessException>(async () =>
        {
            Login(_gdprTestData.UserId1);
            await _gdprRequestAppService.PrepareUserDataAsync();
        });
    }
    
    private void Login(Guid userId)
    {
        _currentUser.Id.Returns(userId);
        _currentUser.IsAuthenticated.Returns(true);
    }
}