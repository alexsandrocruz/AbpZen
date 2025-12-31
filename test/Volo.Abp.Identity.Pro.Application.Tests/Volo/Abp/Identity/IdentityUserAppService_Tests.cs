using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using Xunit;

namespace Volo.Abp.Identity;

public class IdentityUserAppService_Tests : AbpIdentityApplicationTestBase
{
    private readonly IIdentityUserAppService _userAppService;
    private readonly IIdentityUserRepository _userRepository;
    private readonly IOrganizationUnitRepository _organizationUnitRepository;
    private readonly IIdentityRoleRepository _identityRoleRepository;
    private readonly UnitOfWorkManager _unitOfWorkManager;
    private readonly IdentityTestData _testData;
    private ICurrentUser _currentUser;

    public IdentityUserAppService_Tests()
    {
        _userAppService = GetRequiredService<IIdentityUserAppService>();
        _userRepository = GetRequiredService<IIdentityUserRepository>();
        _organizationUnitRepository = GetRequiredService<IOrganizationUnitRepository>();
        _identityRoleRepository = GetRequiredService<IIdentityRoleRepository>();
        _unitOfWorkManager = GetRequiredService<UnitOfWorkManager>();
        _testData = GetRequiredService<IdentityTestData>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _currentUser = Substitute.For<ICurrentUser>();
        _currentUser.Id.Returns(Guid.NewGuid());
        services.AddSingleton(_currentUser);
    }

    [Fact]
    public async Task GetAsync()
    {
        //Arrange

        var johnNash = GetUser("john.nash");

        //Act

        var result = await _userAppService.GetAsync(johnNash.Id);

        //Assert

        result.Id.ShouldBe(johnNash.Id);
        result.UserName.ShouldBe(johnNash.UserName);
        result.Email.ShouldBe(johnNash.Email);
        result.LockoutEnabled.ShouldBe(johnNash.LockoutEnabled);
        result.PhoneNumber.ShouldBe(johnNash.PhoneNumber);
    }

    [Fact]
    public async Task GetListAsync()
    {
        //Act

        var result = await _userAppService.GetListAsync(new GetIdentityUsersInput());

        //Assert

        result.TotalCount.ShouldBeGreaterThan(0);
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task CreateAsync()
    {
        //Arrange

        var input = new IdentityUserCreateDto
        {
            UserName = Guid.NewGuid().ToString(),
            Email = CreateRandomEmail(),
            LockoutEnabled = true,
            PhoneNumber = CreateRandomPhoneNumber(),
            Password = "123qwE4r*",
            RoleNames = new[] { "moderator" }
        };

        //Act

        var result = await _userAppService.CreateAsync(input);

        //Assert

        result.Id.ShouldNotBe(Guid.Empty);
        result.UserName.ShouldBe(input.UserName);
        result.Email.ShouldBe(input.Email);
        result.LockoutEnabled.ShouldBe(input.LockoutEnabled);
        result.PhoneNumber.ShouldBe(input.PhoneNumber);

        var user = await _userRepository.GetAsync(result.Id);
        user.Id.ShouldBe(result.Id);
        user.UserName.ShouldBe(input.UserName);
        user.Email.ShouldBe(input.Email);
        user.LockoutEnabled.ShouldBe(input.LockoutEnabled);
        user.PhoneNumber.ShouldBe(input.PhoneNumber);
    }

    [Fact]
    public async Task UpdateAsync()
    {
        //Arrange

        var johnNash = GetUser("john.nash");

        var input = new IdentityUserUpdateDto
        {
            UserName = johnNash.UserName,
            LockoutEnabled = true,
            PhoneNumber = CreateRandomPhoneNumber(),
            Email = CreateRandomEmail(),
            RoleNames = new[] { "admin", "moderator" },
            ConcurrencyStamp = johnNash.ConcurrencyStamp,
            Surname = johnNash.Surname,
            Name = johnNash.Name
        };

        //Act

        var result = await _userAppService.UpdateAsync(johnNash.Id, input);

        //Assert

        result.Id.ShouldBe(johnNash.Id);
        result.UserName.ShouldBe(input.UserName);
        result.Email.ShouldBe(input.Email);
        result.LockoutEnabled.ShouldBe(input.LockoutEnabled);
        result.PhoneNumber.ShouldBe(input.PhoneNumber);

        var user = await _userRepository.GetAsync(result.Id);
        user.Id.ShouldBe(result.Id);
        user.UserName.ShouldBe(input.UserName);
        user.Email.ShouldBe(input.Email);
        user.LockoutEnabled.ShouldBe(input.LockoutEnabled);
        user.PhoneNumber.ShouldBe(input.PhoneNumber);
        user.Roles.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Update_OU_Role_Test()
    {
        var testUserId = Guid.NewGuid();
        using (var uow = _unitOfWorkManager.Begin())
        {
            var ou1 = await _organizationUnitRepository.GetAsync("OU1");
            ou1.AddRole(_testData.RoleModeratorId);
            await _organizationUnitRepository.UpdateAsync(ou1, true);
            var roles = await _organizationUnitRepository.GetRolesAsync(new []{ ou1.Id });
            roles.Count.ShouldBe(1);

            var adminRole = await _identityRoleRepository.FindByNormalizedNameAsync("ADMIN");
            var testUser = new IdentityUser(testUserId, "test", "test@abp.io");
            testUser.AddRole(_testData.RoleModeratorId);
            testUser.AddRole(adminRole.Id);
            testUser.AddOrganizationUnit(ou1.Id);
            await _userRepository.InsertAsync(testUser);

            await uow.CompleteAsync();
        }

        var userDto = await _userAppService.GetAsync(testUserId);
        userDto.RoleNames.Count.ShouldBe(2);

        var input = new IdentityUserUpdateDto
        {
            UserName = "test2",
            Email = CreateRandomEmail(),
            RoleNames = new []{ "ADMIN" },
            OrganizationUnitIds = Array.Empty<Guid>()
        };

        //Act
        var result = await _userAppService.UpdateAsync(testUserId, input);

        //Assert
        var user = await _userRepository.GetAsync(result.Id);
        user.Id.ShouldBe(result.Id);
        user.Roles.Count.ShouldBe(2);
    }

    [Fact]
    public async Task UpdateAsync_Concurrency_Exception()
    {
        //Get user
        var johnNash = await _userAppService.GetAsync(_testData.UserJohnId);

        //Act
        var input = new IdentityUserUpdateDto
        {
            Name = "John-updated",
            Surname = "Nash-updated",
            UserName = johnNash.UserName,
            LockoutEnabled = true,
            PhoneNumber = CreateRandomPhoneNumber(),
            Email = CreateRandomEmail(),
            RoleNames = new[] { "admin", "moderator" },
            ConcurrencyStamp = johnNash.ConcurrencyStamp
        };
        await _userAppService.UpdateAsync(johnNash.Id, input);

        //Second update with same input will throw exception because the entity has been modified
        (await Assert.ThrowsAsync<AbpIdentityResultException>(async () =>
        {
            await _userAppService.UpdateAsync(johnNash.Id, input);
        })).Message.ShouldContain("Optimistic concurrency failure");
    }

    [Fact]
    public async Task DeleteAsync()
    {
        //Arrange

        var johnNash = GetUser("john.nash");

        //Act

        await _userAppService.DeleteAsync(johnNash.Id);

        //Assert

        FindUser("john.nash").ShouldBeNull();
    }

    [Fact]
    public async Task GetRolesAsync()
    {
        //Arrange

        var johnNash = GetUser("john.nash");

        //Act

        var result = await _userAppService.GetRolesAsync(johnNash.Id);

        //Assert

        result.Items.Count.ShouldBe(3);
        result.Items.ShouldContain(r => r.Name == "manager"); // Organization Unit Role
        result.Items.ShouldContain(r => r.Name == "moderator");
        result.Items.ShouldContain(r => r.Name == "supporter");
    }

    [Fact]
    public async Task UpdateRolesAsync()
    {
        //Arrange

        var johnNash = GetUser("john.nash");

        //Act

        await _userAppService.UpdateRolesAsync(
            johnNash.Id,
            new IdentityUserUpdateRolesDto
            {
                RoleNames = new[] { "admin", "moderator" }
            }
        );

        //Assert

        var roleNames = await _userRepository.GetRoleNamesAsync(johnNash.Id);
        roleNames.Count.ShouldBe(3);
        roleNames.ShouldContain("manager"); // Organization Unit Role
        roleNames.ShouldContain("admin");
        roleNames.ShouldContain("moderator");
    }

    [Fact]
    public async Task Should_Restore_Roles_When_User_Removed_From_Organization()
    {
        //Arrange
        var testUserId = Guid.NewGuid();
        Guid? ou1Id;
        using (var uow = _unitOfWorkManager.Begin())
        {
            var testUser = new IdentityUser(testUserId, "test", "test@abp.io");
            testUser.AddRole(_testData.RoleModeratorId);
            await _userRepository.InsertAsync(testUser);

            var ou1 = await _organizationUnitRepository.GetAsync("OU1");
            ou1Id = ou1.Id;
            ou1.AddRole(_testData.RoleModeratorId);
            var adminRole = await _identityRoleRepository.FindByNormalizedNameAsync("ADMIN");
            ou1.AddRole(adminRole.Id);
            await _organizationUnitRepository.UpdateAsync(ou1, true);
            var roles = await _organizationUnitRepository.GetRolesAsync(new []{ ou1.Id });
            roles.Count.ShouldBe(2);

            await uow.CompleteAsync();
        }

        var userDto = await _userAppService.GetAsync(testUserId);
        userDto.RoleNames.Count.ShouldBe(1);

        var input = new IdentityUserUpdateDto
        {
            UserName = "test2",
            Email = CreateRandomEmail(),
            RoleNames = new []{ "moderator" },
            OrganizationUnitIds = new[] { ou1Id.Value}
        };

        var result = await _userAppService.UpdateAsync(testUserId, input);
        var user = await _userAppService.GetAsync(result.Id);
        user.RoleNames.Count.ShouldBe(2);

        //Act
        input.OrganizationUnitIds = Array.Empty<Guid>();
        result = await _userAppService.UpdateAsync(testUserId, input);

        //Assert
        user = await _userAppService.GetAsync(result.Id);
        user.Id.ShouldBe(result.Id);
        user.RoleNames.Count.ShouldBe(1);
    }

    [Fact]
    public async Task UpdateClaimsAsync()
    {
        //Arrange

        var johnNash = GetUser("john.nash");

        //Act

        var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () => await _userAppService.UpdateClaimsAsync(
            johnNash.Id,
            [
                new IdentityUserClaimDto { ClaimType = "SocialNumber", ClaimValue = "Invalid value" }
            ]
        ));
        
        //Assert
        
        exception.Message.ShouldBe("Claim value 'SocialNumber' is invalid.");

        //Act

        await _userAppService.UpdateClaimsAsync(
                johnNash.Id,
                [
                    new IdentityUserClaimDto { ClaimType = "SocialNumber", ClaimValue = "123456789" }
                ]
            );
       
        //Assert
        
        johnNash = await _userRepository.GetAsync(johnNash.Id, includeDetails: true);
        johnNash.Claims.ShouldContain(x => x.ClaimType == "SocialNumber" && x.ClaimValue == "123456789");
    }

    private static string CreateRandomEmail()
    {
        return Guid.NewGuid().ToString("N").Left(16) + "@abp.io";
    }

    private static string CreateRandomPhoneNumber()
    {
        return RandomHelper.GetRandom(10000000, 100000000).ToString();
    }
}
