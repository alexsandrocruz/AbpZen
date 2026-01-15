using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Sapienza.Dominus.Localization;
using Sapienza.Dominus.MultiTenancy;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.Web.Menus;
using StackExchange.Redis;
using Volo.Abp.Caching.StackExchangeRedis;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Account.Admin.Web;
using Volo.Abp.Account.Public.Web.Impersonation;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AuditLogging.Web;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Http.Client.Web;
using Volo.Abp.Identity.Web;
using Volo.Abp.LanguageManagement;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TextTemplateManagement.Web;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.Saas.Host;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Chat;
using Volo.Chat.Web;
using Volo.FileManagement.Web;
using Volo.Forms.Web;

using Volo.CmsKit.Pro.Web;
using Volo.Abp.Gdpr.Web;
using Volo.Abp.OpenIddict.Pro.Web;
using Volo.CmsKit.Pages;

namespace Sapienza.Dominus.Web
{
    [DependsOn(
        typeof(DominusHttpApiClientModule),
        typeof(AbpAccountPublicWebImpersonationModule),
        typeof(AbpAspNetCoreMvcClientModule),
        typeof(AbpAutofacModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpFeatureManagementWebModule),
        typeof(AbpAccountAdminWebModule),
        typeof(AbpHttpClientWebModule),
        typeof(AbpHttpClientIdentityModelWebModule),
        typeof(AbpIdentityWebModule),
        typeof(AbpAuditLoggingWebModule),
        typeof(SaasHostWebModule),
        typeof(AbpOpenIddictProWebModule),
        typeof(LanguageManagementWebModule),
        typeof(TextTemplateManagementWebModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule)
        )]
    [DependsOn(typeof(ChatSignalRModule))]
    [DependsOn(typeof(ChatWebModule))]
    [DependsOn(typeof(FileManagementWebModule))]
    [DependsOn(typeof(FormsWebModule))]
    [DependsOn(typeof(CmsKitProWebModule))]
    [DependsOn(typeof(AbpGdprWebModule))]
    public class DominusWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(DominusResource),
                    typeof(DominusDomainSharedModule).Assembly,
                    typeof(DominusApplicationContractsModule).Assembly,
                    typeof(DominusWebModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            /*
            Configure<BasicThemeMvcOptions>(options =>
            {
                options.ApplicationLayout = BasicMvcLayouts.SideMenu;
            });

            Configure<BasicThemeOptions>(options =>
            {
                options.DefaultStyle = BasicStyleNames.System;
            });
            */

            ConfigureBundles();
            ConfigurePages(configuration);
            ConfigureCache(configuration);
            ConfigureDataProtection(context, configuration, hostingEnvironment);
            ConfigureUrls(configuration);
            ConfigureAuthentication(context, configuration);
            ConfigureImpersonation(context, configuration);
            ConfigureAutoMapper();
            ConfigureVirtualFileSystem(hostingEnvironment);
            ConfigureNavigationServices(configuration);
            ConfigureSwaggerServices(context.Services);
            ConfigureMultiTenancy();
        }

        private void ConfigureBundles()
        {
            Configure<AbpBundlingOptions>(options =>
            {
                options.StyleBundles.Configure(
                    BasicThemeBundles.Styles.Global,
                    bundle =>
                    {
                        bundle.AddFiles("/global-styles.css");
                    }
                );
            });
        }

        private void ConfigurePages(IConfiguration configuration)
        {
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/HostDashboard", DominusPermissions.Dashboard.Host);
                options.Conventions.AuthorizePage("/TenantDashboard", DominusPermissions.Dashboard.Tenant);
            });
        }

        private void ConfigureCache(IConfiguration configuration)
        {
            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = "Dominus:";
            });
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureMultiTenancy()
        {
            Configure<AbpMultiTenancyOptions>(options => { options.IsEnabled = MultiTenancyConsts.IsEnabled; });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies", options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(365);
                })
                .AddAbpOpenIdConnect("oidc", options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.ResponseType = OpenIdConnectResponseType.CodeIdToken;

                    options.ClientId = configuration["AuthServer:ClientId"];
                    options.ClientSecret = configuration["AuthServer:ClientSecret"];

                    options.UsePkce = true;
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Add("roles");
                    options.Scope.Add("email");
                    options.Scope.Add("phone");
                    options.Scope.Add("Dominus");
                });
        }

        private void ConfigureImpersonation(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.Configure<AbpSaasHostWebOptions>(options =>
            {
                options.EnableTenantImpersonation = true;
            });
            context.Services.Configure<AbpIdentityWebOptions>(options =>
            {
                options.EnableUserImpersonation = true;
            });
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<DominusWebModule>();
            });
        }

        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<DominusWebModule>();

                if (hostingEnvironment.IsDevelopment())
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<DominusDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}Sapienza.Sapienza.Dominus.Domain.Shared", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<DominusApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}Sapienza.Sapienza.Dominus.Application.Contracts", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<DominusWebModule>(hostingEnvironment.ContentRootPath);
                    // options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreMvcUiBasicThemeModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic", Path.DirectorySeparatorChar)));
                }
            });
        }

        private void ConfigureNavigationServices(IConfiguration configuration)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new DominusMenuContributor(configuration));
            });

            Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new DominusToolbarContributor());
            });
        }

        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddAbpSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Dominus API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
        }

        private void ConfigureDataProtection(
            ServiceConfigurationContext context,
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment)
        {
            var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("Dominus");
            if (!hostingEnvironment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "Dominus-Protection-Keys");
            }
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
               options.SwaggerEndpoint("/swagger/v1/swagger.json", "Dominus API");
            });
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}
