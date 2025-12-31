using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Scopes.Dtos;
using Xunit;

namespace Volo.Abp.OpenIddict;

public abstract class ScopeAppService_Tests<TStartupModule> : OpenIddictProTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IOpenIddictScopeRepository ScopeRepository { get; }
    protected IScopeAppService ScopeAppService { get; }

    public ScopeAppService_Tests()
    {
        ScopeRepository = ServiceProvider.GetRequiredService<IOpenIddictScopeRepository>();
        ScopeAppService = ServiceProvider.GetRequiredService<IScopeAppService>();
    }

    [Fact]
    public async Task GetAsync()
    {
        var scope = await ScopeRepository.FindByNameAsync("api1");
        scope.ShouldNotBeNull();

        var scopeDto = await ScopeAppService.GetAsync(scope.Id);
        scopeDto.ShouldNotBeNull();
        scopeDto.Name.ShouldBe("api1");
        scopeDto.DisplayName.ShouldBe("API 1");
        scopeDto.Description.ShouldBe("API 1 Description");
        scopeDto.Resources.ShouldContain(x => x == "api1_audiences");
    }

    [Fact]
    public async Task GetListAsync()
    {
        var dtos = await ScopeAppService.GetListAsync(new GetScopeListInput());
        dtos.Items.Count.ShouldBe(2);

        dtos.Items.ShouldContain(x => x.Name == "api1");
        dtos.Items.ShouldContain(x => x.Name == "api2");
    }

    [Fact]
    public async Task CreateAsync()
    {
        var scope = await ScopeRepository.FindByNameAsync("api1");
        scope.ShouldNotBeNull();

        var scopeDto = await ScopeAppService.CreateAsync(new CreateScopeInput()
        {
            Name = "api3",
            DisplayName = "API 3",
            Description = "API 3 Description",
            Resources = new HashSet<string>() { "api3_audiences" }
        });

        scopeDto.ShouldNotBeNull();
        scopeDto.Name.ShouldBe("api3");
        scopeDto.DisplayName.ShouldBe("API 3");
        scopeDto.Description.ShouldBe("API 3 Description");
        scopeDto.Resources.ShouldContain(x => x == "api3_audiences");
    }

    [Fact]
    public async Task UpdateAsync()
    {
        var scope = await ScopeRepository.FindByNameAsync("api1");
        scope.ShouldNotBeNull();

        var scopeDto = await ScopeAppService.UpdateAsync(scope.Id, new UpdateScopeInput()
        {
            Name = "api4",
            DisplayName = "API 4",
            Description = "API 4 Description",
            Resources = new HashSet<string>() { "api4_audiences" }
        });

        scopeDto.ShouldNotBeNull();
        scopeDto.Name.ShouldBe("api4");
        scopeDto.DisplayName.ShouldBe("API 4");
        scopeDto.Description.ShouldBe("API 4 Description");
        scopeDto.Resources.ShouldContain(x => x == "api4_audiences");
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var scope = await ScopeRepository.FindByNameAsync("api1");
        scope.ShouldNotBeNull();

        await ScopeAppService.DeleteAsync(scope.Id);

        (await ScopeRepository.FindByNameAsync("api1")).ShouldBeNull();
    }

    [Fact]
    public async Task GetAllScopesAsync()
    {
        var scopesDto = await ScopeAppService.GetAllScopesAsync();

        scopesDto.Count.ShouldBe(5 + 2);

        scopesDto.ShouldContain(x => x.Name == "api1");
        scopesDto.ShouldContain(x => x.Name == "api2");
    }
}
