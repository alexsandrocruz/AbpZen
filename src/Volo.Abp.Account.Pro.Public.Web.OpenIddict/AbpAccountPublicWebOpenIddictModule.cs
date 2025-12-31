using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Server;
using OpenIddict.Validation;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Public.Web;
using Volo.Abp.Account.Web.ExtensionGrants;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Account.Web;

[DependsOn(
    typeof(AbpAccountPublicWebModule),
    typeof(AbpOpenIddictAspNetCoreModule)
    )]
public class AbpAccountPublicWebOpenIddictModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(AccountResource),
                typeof(AbpAccountPublicApplicationContractsModule).Assembly,
                typeof(AbpAccountPublicWebOpenIddictModule).Assembly
            );
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAccountPublicWebOpenIddictModule).Assembly);
        });

        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            builder.AddEventHandler(OpenIddictImpersonateInferEndpointType.Descriptor);
            builder.AddEventHandler(LinkLoginExtensionGrantProcessJsonResponse.Descriptor);
            builder.AddEventHandler(OpenIddictCreateIdentitySession.Descriptor);
            builder.AddEventHandler(OpenIddictValidateIdentitySessionServerHandler.Descriptor);
            builder.AddEventHandler(OpenIddictRevokeIdentitySessionOnRevocation.Descriptor);
            builder.AddEventHandler(OpenIddictRevokeIdentitySessionOnLogout.Descriptor);

            builder.Configure(openIddictServerOptions =>
            {
                openIddictServerOptions.GrantTypes.Add(LinkLoginExtensionGrant.ExtensionGrantName);
                openIddictServerOptions.GrantTypes.Add(ImpersonationExtensionGrant.ExtensionGrantName);
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountPublicWebOpenIddictModule>();
        });

        Configure<AbpOpenIddictExtensionGrantsOptions>(options =>
        {
            options.Grants.Add(LinkLoginExtensionGrant.ExtensionGrantName, new LinkLoginExtensionGrant());
            options.Grants.Add(ImpersonationExtensionGrant.ExtensionGrantName, new ImpersonationExtensionGrant());
        });

        Configure<AbpRequestLocalizationOptions>(options =>
        {
            options.RequestLocalizationOptionConfigurators.Add((serviceProvider, localizationOptions) =>
            {
                localizationOptions.RequestCultureProviders.InsertAfter(
                    x => x.GetType() == typeof(QueryStringRequestCultureProvider),
                    new OpenIddictReturnUrlRequestCultureProvider());
                return Task.CompletedTask;
            });
        });

        Configure<OpenIddictServerOptions>(options =>
        {
            options.AuthorizationEndpointUris.Add(new Uri("Account/ImpersonateTenant", UriKind.RelativeOrAbsolute));
            options.AuthorizationEndpointUris.Add(new Uri("Account/ImpersonateUser", UriKind.RelativeOrAbsolute));
            options.AuthorizationEndpointUris.Add(new Uri("Account/DelegatedImpersonate", UriKind.RelativeOrAbsolute));
            options.AuthorizationEndpointUris.Add(new Uri("Account/BackToImpersonator", UriKind.RelativeOrAbsolute));
        });

        Configure<AbpOpenIddictClaimsPrincipalOptions>(options =>
        {
            options.ClaimsPrincipalHandlers.Add<OpenIddictImpersonateClaimsPrincipalHandler>();
        });

        context.Services.Add(OpenIddictValidateIdentitySessionValidationHandler.Descriptor.ServiceDescriptor);
        Configure<OpenIddictValidationOptions>(builder =>
        {
            builder.Handlers.Add(OpenIddictValidateIdentitySessionValidationHandler.Descriptor);
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
