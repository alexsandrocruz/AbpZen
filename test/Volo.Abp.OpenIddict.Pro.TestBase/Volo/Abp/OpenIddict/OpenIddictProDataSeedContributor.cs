using System;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict.Applications;

namespace Volo.Abp.OpenIddict;

public class OpenIddictProDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly ICurrentTenant _currentTenant;
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictScopeManager _scopeManager;

    public OpenIddictProDataSeedContributor(
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant,
        IOpenIddictApplicationManager applicationManager,
        IOpenIddictScopeManager scopeManager)
    {
        _guidGenerator = guidGenerator;
        _currentTenant = currentTenant;
        _applicationManager = applicationManager;
        _scopeManager = scopeManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        var app1 = new AbpApplicationDescriptor()
        {
            ClientId = "MVC",
            DisplayName = "MVC Client",
            ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
            ClientType = OpenIddictConstants.ClientTypes.Confidential,
            ClientSecret = "1q2w3E*",
            ClientUri = "https://mvc.abp.io",
            LogoUri = "https://mvc.abp.io/logo.png"
        };

        app1.RedirectUris.Add(new Uri("https://mvc.abp.io"));
        app1.PostLogoutRedirectUris.Add(new Uri("https://mvc.abp.io"));

        app1.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);

        await _applicationManager.CreateAsync(app1);

        var app2 = new AbpApplicationDescriptor()
        {
            ClientId = "NG",
            DisplayName = "NG Client",
            ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
            ClientType = OpenIddictConstants.ClientTypes.Confidential,
            ClientSecret = "1q2w3E*"
        };

        app1.RedirectUris.Add(new Uri("https://ng.abp.io"));
        app1.PostLogoutRedirectUris.Add(new Uri("https://ng.abp.io"));

        app1.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
        app1.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);

        await _applicationManager.CreateAsync(app2);

        var scope1 = new OpenIddictScopeDescriptor()
        {
            Name = "api1",
            DisplayName = "API 1",
            Description = "API 1 Description"
        };
        scope1.Resources.Add("api1_audiences");
        await _scopeManager.CreateAsync(scope1);

        var scope2 = new OpenIddictScopeDescriptor()
        {
            Name = "api2",
            DisplayName = "API 2",
            Description = "API 2 Description"
        };
        scope2.Resources.Add("api2_audiences");
        await _scopeManager.CreateAsync(scope2);
    }
}
