using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.Forms.Localization;
using Volo.Forms.Permissions;
using Volo.Forms.Web.Menus;

namespace Volo.Forms.Web;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
    typeof(FormsApplicationContractsModule)
)]
public class FormsWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(FormsResource), typeof(FormsWebModule).Assembly, typeof(FormsApplicationContractsModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(FormsWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.MinificationIgnoredFiles.Add("/Pages/Forms/Shared/Components/FormQuestions/Vue-question-choice.js");
            options.MinificationIgnoredFiles.Add("/Pages/Forms/Shared/Components/FormQuestions/Vue-question-types.js");
            options.MinificationIgnoredFiles.Add("/Pages/Forms/Shared/Components/FormQuestions/Vue-question-item.js");
            options.MinificationIgnoredFiles.Add("/Pages/Forms/Shared/Components/FormQuestions/Default.js");

            options.MinificationIgnoredFiles.Add("/Pages/Forms/Shared/Components/FormResponses/Vue-block-response-component.js");
            options.MinificationIgnoredFiles.Add("/Pages/Forms/Shared/Components/FormResponses/Vue-response-chart.js");
            options.MinificationIgnoredFiles.Add("/Pages/Forms/Shared/Components/FormResponses/Vue-response-answers.js");
            options.MinificationIgnoredFiles.Add("/Pages/Forms/Shared/Components/FormResponses/Default.js");

            options.MinificationIgnoredFiles.Add("/Pages/Forms/Shared/Components/ViewForm/Vue-email-property.js");
            options.MinificationIgnoredFiles.Add("/Pages/Forms/Shared/Components/ViewForm/Vue-answer.js");
            options.MinificationIgnoredFiles.Add("/Pages/Forms/Shared/Components/ViewForm/Default.js");

            options.MinificationIgnoredFiles.Add("/Pages/Forms/Shared/Components/ViewResponse/Default.js");
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new FormsMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<FormsWebModule>("Volo.Forms.Web");
        });

        context.Services.AddAutoMapperObjectMapper<FormsWebModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<FormsWebModule>(validate: true);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Pages/Forms/Index/", FormsPermissions.Forms.Default);
            options.Conventions.AuthorizePage("/Pages/Forms/CreateModal/", FormsPermissions.Forms.Default);
            options.Conventions.AuthorizePage("/Pages/Forms/SendModal/", FormsPermissions.Forms.Default);
            options.Conventions.AuthorizeFolder("/Pages/Forms/Questions/", FormsPermissions.Forms.Default);
        });

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(FormsRemoteServiceConsts.ModuleName);
        });
    }
}
