using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.OpenIddict.Pro.Web.Menus;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.PageToolbars;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.OpenIddict.Permissions;
using Volo.Abp.OpenIddict.Pro.Web.Pages.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Pro.Web.Pages.OpenIddict.Scopes;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.OpenIddict.Pro.Web;

[DependsOn(
    typeof(AbpOpenIddictProApplicationContractsModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
    typeof(AbpPermissionManagementWebModule),
    typeof(AbpAutoMapperModule)
    )]
public class AbpOpenIddictProWebModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AbpOpenIddictResource), typeof(AbpOpenIddictProWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpOpenIddictProWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new OpenIddictProMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpOpenIddictProWebModule>();
        });

        context.Services.AddAutoMapperObjectMapper<AbpOpenIddictProWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AbpOpenIddictProWebModule>(validate: true);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/OpenIddict/Scopes/Index", AbpOpenIddictProPermissions.Scope.Default);
            options.Conventions.AuthorizePage("/OpenIddict/Scopes/CreateModal", AbpOpenIddictProPermissions.Scope.Create);
            options.Conventions.AuthorizePage("/OpenIddict/Scopes/EditModal", AbpOpenIddictProPermissions.Scope.Update);

            options.Conventions.AuthorizePage("/OpenIddict/Applications/Index", AbpOpenIddictProPermissions.Application.Default);
            options.Conventions.AuthorizePage("/OpenIddict/Applications/CreateModal", AbpOpenIddictProPermissions.Application.Create);
            options.Conventions.AuthorizePage("/OpenIddict/Applications/EditModal", AbpOpenIddictProPermissions.Application.Update);
        });

        Configure<AbpPageToolbarOptions>(options =>
        {
            options.Configure<Volo.Abp.OpenIddict.Pro.Web.Pages.OpenIddict.Scopes.IndexModel>(
                toolbar =>
                {
                    toolbar.AddButton(
                        LocalizableString.Create<AbpOpenIddictResource>("NewScope"),
                        icon: "plus",
                        name: "CreateScope",
                        id: "CreateNewButtonId",
                        requiredPolicyName: AbpOpenIddictProPermissions.Scope.Create
                    );
                }
            );

            options.Configure<Volo.Abp.OpenIddict.Pro.Web.Pages.OpenIddict.Applications.IndexModel>(
                toolbar =>
                {
                    toolbar.AddButton(
                        LocalizableString.Create<AbpOpenIddictResource>("NewApplication"),
                        icon: "plus",
                        name: "CreateApplication",
                        id: "CreateNewButtonId",
                        requiredPolicyName: AbpOpenIddictProPermissions.Application.Create
                    );
                }
            );
        });

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(AbpOpenIddictProRemoteServiceConsts.ModuleName);
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpOpenIddictResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    OpenIddictModuleExtensionConsts.ModuleName,
                    OpenIddictModuleExtensionConsts.EntityNames.Application,
                    createFormTypes: new[] { typeof(ApplicationCreateModalView) },
                    editFormTypes: new[] { typeof(ApplicationEditModalView) }
                );

            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    OpenIddictModuleExtensionConsts.ModuleName,
                    OpenIddictModuleExtensionConsts.EntityNames.Scope,
                    createFormTypes: new[] { typeof(ScopeCreateModalView) },
                    editFormTypes: new[] { typeof(ScopeEditModelView) }
                );
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
