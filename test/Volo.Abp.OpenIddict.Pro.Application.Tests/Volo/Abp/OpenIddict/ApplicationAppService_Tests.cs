using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Applications.Dtos;
using Xunit;

namespace Volo.Abp.OpenIddict;

public abstract class ApplicationAppService_Tests<TStartupModule> : OpenIddictProTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IOpenIddictApplicationRepository ApplicationRepository { get; }
    protected IApplicationAppService ApplicationAppService { get; }

    public ApplicationAppService_Tests()
    {
        ApplicationRepository = ServiceProvider.GetRequiredService<IOpenIddictApplicationRepository>();
        ApplicationAppService = ServiceProvider.GetRequiredService<IApplicationAppService>();
    }

    [Fact]
    public async Task GetAsync()
    {
        var app = await ApplicationRepository.FindByClientIdAsync("MVC");
        app.ShouldNotBeNull();

        var appDto = await ApplicationAppService.GetAsync(app.Id);
        appDto.ShouldNotBeNull();
        appDto.ClientId.ShouldBe(app.ClientId);
        appDto.ClientSecret.ShouldBeNullOrEmpty();
        appDto.ClientType.ShouldBe(app.ClientType);
        appDto.DisplayName.ShouldBe(app.DisplayName);
        appDto.ConsentType.ShouldBe(app.ConsentType);
        appDto.ClientUri.ShouldBe(app.ClientUri);
        appDto.LogoUri.ShouldBe(app.LogoUri);

        appDto.PostLogoutRedirectUris.ShouldContain("https://mvc.abp.io");
        appDto.RedirectUris.ShouldContain("https://mvc.abp.io");
    }

    [Fact]
    public async Task GetListAsync()
    {
        var dtos = await ApplicationAppService.GetListAsync(new GetApplicationListInput());
        dtos.Items.Count.ShouldBe(2);

        dtos.Items.ShouldContain(x => x.ClientId == "MVC");
        dtos.Items.ShouldContain(x => x.ClientId == "NG");
    }

    [Fact]
    public async Task CreateAsync()
    {
        var appDto = await ApplicationAppService.CreateAsync(new CreateApplicationInput()
        {
            ApplicationType = OpenIddictConstants.ApplicationTypes.Web,
            ClientId = "MyClient",
            DisplayName = "My Client Display Name",
            ClientType = OpenIddictConstants.ClientTypes.Public,
            ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
            AllowPasswordFlow = true,
            AllowClientCredentialsFlow = false,
            AllowAuthorizationCodeFlow = true,
            AllowDeviceEndpoint = false,
            AllowHybridFlow = true,
            AllowImplicitFlow = false,
            AllowLogoutEndpoint = true,
            AllowRefreshTokenFlow = false,
            RedirectUris = new HashSet<string>() {"https://localhost:44301/signin-oidc"},
            PostLogoutRedirectUris = new HashSet<string>() {"https://localhost:44301/signout-callback-oidc"},
            ExtensionGrantTypes = new HashSet<string>()
            {
                "my_grant_type",
                "my_other_grant_type"
            },
            Scopes = new HashSet<string>()
            {
                "api1"
            },
            ClientUri = "https://localhost:44301",
            LogoUri = "https://localhost:44301/logo.png"
        });

        appDto.ApplicationType.ShouldBe(OpenIddictConstants.ApplicationTypes.Web);
        appDto.ClientId.ShouldBe("MyClient");
        appDto.DisplayName.ShouldBe("My Client Display Name");
        appDto.ClientType.ShouldBe(OpenIddictConstants.ClientTypes.Public);
        appDto.ConsentType.ShouldBe(OpenIddictConstants.ConsentTypes.Explicit);

        appDto.PostLogoutRedirectUris.ShouldContain("https://localhost:44301/signout-callback-oidc");
        appDto.RedirectUris.ShouldContain("https://localhost:44301/signin-oidc");

        appDto.ClientUri.ShouldBe("https://localhost:44301");
        appDto.LogoUri.ShouldBe("https://localhost:44301/logo.png");

        appDto.ExtensionGrantTypes.Count.ShouldBe(2);
        appDto.ExtensionGrantTypes.ShouldContain(x => x == "my_grant_type");
        appDto.ExtensionGrantTypes.ShouldContain(x => x == "my_other_grant_type");

        appDto.AllowPasswordFlow.ShouldBeTrue();
        appDto.AllowClientCredentialsFlow.ShouldBeFalse();
        appDto.AllowAuthorizationCodeFlow.ShouldBeTrue();
        appDto.AllowDeviceEndpoint.ShouldBeFalse();
        appDto.AllowHybridFlow.ShouldBeTrue();
        appDto.AllowImplicitFlow.ShouldBeTrue();
        appDto.AllowLogoutEndpoint.ShouldBeTrue();
        appDto.AllowRefreshTokenFlow.ShouldBeFalse();

        appDto.Scopes.ShouldContain(x => x == "api1");
    }

    [Fact]
    public async Task UpdateAsync()
    {
        var app = await ApplicationRepository.FindByClientIdAsync("MVC");
        app.ShouldNotBeNull();

        var appDto = await ApplicationAppService.UpdateAsync(app.Id, new UpdateApplicationInput()
        {
            ApplicationType = OpenIddictConstants.ApplicationTypes.Native,
            ClientId = "MyClient",
            DisplayName = "My Client Display Name",
            ClientType = OpenIddictConstants.ClientTypes.Confidential,
            ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
            AllowPasswordFlow = true,
            AllowClientCredentialsFlow = true,
            AllowAuthorizationCodeFlow = true,
            AllowDeviceEndpoint = false,
            RedirectUris = new HashSet<string>() {"https://localhost:44301/signin-oidc"},
            PostLogoutRedirectUris = new HashSet<string>() {"https://localhost:44301/signout-callback-oidc"},
            ExtensionGrantTypes = new HashSet<string>()
            {
                "my_grant_type",
                "my_other_grant_type"
            },
            Scopes = new HashSet<string>()
            {
                "api1"
            },
            ClientUri = "https://localhost",
            LogoUri = "https://localhost/logo.png"
        });

        appDto.ApplicationType.ShouldBe(OpenIddictConstants.ApplicationTypes.Native);
        appDto.ClientId.ShouldBe("MyClient");
        appDto.DisplayName.ShouldBe("My Client Display Name");
        appDto.ClientType.ShouldBe(OpenIddictConstants.ClientTypes.Confidential);
        appDto.ClientSecret.ShouldBeNullOrEmpty();
        appDto.ConsentType.ShouldBe(OpenIddictConstants.ConsentTypes.Explicit);

        appDto.PostLogoutRedirectUris.ShouldContain("https://localhost:44301/signout-callback-oidc");
        appDto.RedirectUris.ShouldContain("https://localhost:44301/signin-oidc");

        appDto.ExtensionGrantTypes.Count.ShouldBe(2);
        appDto.ExtensionGrantTypes.ShouldContain(x => x == "my_grant_type");
        appDto.ExtensionGrantTypes.ShouldContain(x => x == "my_other_grant_type");

        appDto.ClientUri.ShouldBe("https://localhost");
        appDto.LogoUri.ShouldBe("https://localhost/logo.png");

        appDto.ClientUri.ShouldBe("https://localhost");
        appDto.LogoUri.ShouldBe("https://localhost/logo.png");

        appDto.AllowPasswordFlow.ShouldBeTrue();
        appDto.AllowClientCredentialsFlow.ShouldBeTrue();
        appDto.AllowAuthorizationCodeFlow.ShouldBeTrue();
        appDto.AllowImplicitFlow.ShouldBeFalse();
        appDto.AllowHybridFlow.ShouldBeFalse();
        appDto.AllowRefreshTokenFlow.ShouldBeFalse();
        appDto.AllowLogoutEndpoint.ShouldBeFalse();
        appDto.AllowDeviceEndpoint.ShouldBeFalse();

        appDto.Scopes.ShouldContain(x => x == "api1");
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var app = await ApplicationRepository.FindByClientIdAsync("MVC");
        app.ShouldNotBeNull();

        await ApplicationAppService.DeleteAsync(app.Id);

        (await ApplicationRepository.FindByClientIdAsync("MVC")).ShouldBeNull();
    }

    [Fact]
    public async Task TokenLifetimeAsync()
    {
        var app = await ApplicationRepository.FindByClientIdAsync("MVC");
        app.ShouldNotBeNull();

        var tokenLifetime = await ApplicationAppService.GetTokenLifetimeAsync(app.Id);
        tokenLifetime.AccessTokenLifetime.ShouldBeNull();
        tokenLifetime.AuthorizationCodeLifetime.ShouldBeNull();
        tokenLifetime.DeviceCodeLifetime.ShouldBeNull();
        tokenLifetime.IdentityTokenLifetime.ShouldBeNull();
        tokenLifetime.RefreshTokenLifetime.ShouldBeNull();

        await ApplicationAppService.SetTokenLifetimeAsync(app.Id, new ApplicationTokenLifetimeDto()
        {
            AccessTokenLifetime = 100,
            AuthorizationCodeLifetime = 200,
            DeviceCodeLifetime = 300,
            IdentityTokenLifetime = 400,
            RefreshTokenLifetime = 500,
            UserCodeLifetime = 600
        });

        tokenLifetime = await ApplicationAppService.GetTokenLifetimeAsync(app.Id);
        tokenLifetime.AccessTokenLifetime.ShouldBe(100);
        tokenLifetime.AuthorizationCodeLifetime.ShouldBe(200);
        tokenLifetime.DeviceCodeLifetime.ShouldBe(300);
        tokenLifetime.IdentityTokenLifetime.ShouldBe(400);
        tokenLifetime.RefreshTokenLifetime.ShouldBe(500);
        tokenLifetime.UserCodeLifetime.ShouldBe(600);

        await ApplicationAppService.SetTokenLifetimeAsync(app.Id, new ApplicationTokenLifetimeDto()
        {
            AccessTokenLifetime = 100,
            AuthorizationCodeLifetime = null,
            DeviceCodeLifetime = 300,
            IdentityTokenLifetime = null,
            RefreshTokenLifetime = 500,
            UserCodeLifetime = null
        });

        tokenLifetime = await ApplicationAppService.GetTokenLifetimeAsync(app.Id);
        tokenLifetime.AccessTokenLifetime.ShouldBe(100);
        tokenLifetime.AuthorizationCodeLifetime.ShouldBeNull();
        tokenLifetime.DeviceCodeLifetime.ShouldBe(300);
        tokenLifetime.IdentityTokenLifetime.ShouldBeNull();
        tokenLifetime.RefreshTokenLifetime.ShouldBe(500);
        tokenLifetime.UserCodeLifetime.ShouldBeNull();
    }
}
