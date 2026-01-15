using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Sapienza.Dominus.Blazor.Menus;
using Sapienza.Dominus.Localization;
using Sapienza.Dominus.MultiTenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.IO;
using Volo.Abp;
using Volo.Abp.Account.Pro.Admin.Blazor.Server;
using Volo.Abp.Account.Pro.Public.Blazor.Server;
using Volo.Abp.Account.Public.Web.Impersonation;
using Volo.Abp.AspNetCore.Authentication.OpenIdConnect;
using Volo.Abp.AspNetCore.Components.Server.BasicTheme;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.MultiTenancy;

using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;


using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AuditLogging.Blazor.Server;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Http.Client.Web;
using Volo.Abp.Identity.Pro.Blazor;
using Volo.Abp.Identity.Pro.Blazor.Server;
using Volo.Abp.LanguageManagement.Blazor.Server;

using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TextTemplateManagement.Blazor.Server;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Volo.Saas.Host.Blazor;
using Volo.Saas.Host.Blazor.Server;
using Volo.CmsKit.Pro.Admin.Blazor.Server;
using Volo.Abp.Gdpr.Blazor.Server;
using Volo.Abp.OpenIddict.Pro.Blazor.Server;
using Microsoft.AspNetCore.SignalR;
using Volo.Chat.Blazor.Server;
using Volo.Chat;

namespace Sapienza.Dominus.Blazor
{
    [DependsOn(
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpAspNetCoreMvcClientModule),
        typeof(AbpAspNetCoreAuthenticationOpenIdConnectModule),
        typeof(AbpHttpClientWebModule),
        typeof(AbpAccountPublicWebImpersonationModule),
        typeof(AbpHttpClientIdentityModelWebModule),
        typeof(AbpAccountAdminBlazorServerModule),
        typeof(AbpAccountPublicBlazorServerModule),
        typeof(AbpAuditLoggingBlazorServerModule),
        typeof(AbpIdentityProBlazorServerModule),
        typeof(AbpOpenIddictProBlazorServerModule),
        typeof(LanguageManagementBlazorServerModule),
        typeof(SaasHostBlazorServerModule),
        typeof(TextTemplateManagementBlazorServerModule),
        typeof(DominusHttpApiClientModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAspNetCoreComponentsServerBasicThemeModule),
        typeof(CmsKitProAdminBlazorServerModule)
       )]
    [DependsOn(typeof(AbpGdprBlazorServerModule))]
    [DependsOn(typeof(ChatBlazorServerModule), typeof(ChatSignalRModule))]
    public class DominusBlazorServerModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(DominusResource),
                    typeof(DominusDomainSharedModule).Assembly,
                    typeof(DominusApplicationContractsModule).Assembly,
                    typeof(DominusBlazorServerModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();




            ConfigureUrls(configuration);
            ConfigureAuthentication(context, configuration);
            ConfigureImpersonation(context, configuration);
            ConfigureAutoMapper();
            ConfigureVirtualFileSystem(hostingEnvironment);
            ConfigureBundles();
            ConfigureSwaggerServices(context.Services);
            ConfigureCache(configuration);
            ConfigureDataProtection(context, configuration, hostingEnvironment);
            ConfigureBlazorise(context);
            ConfigureRouter();
            ConfigureMenu(configuration);

            Configure<HubOptions>(options =>
            {
                options.DisableImplicitFromServicesParameters = true;
            });
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
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
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);;
                    options.ResponseType = OpenIdConnectResponseType.CodeIdToken;

                    options.ClientId = configuration["AuthServer:ClientId"];
                    options.ClientSecret = configuration["AuthServer:ClientSecret"];

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
            context.Services.Configure<SaasHostBlazorOptions>(options =>
            {
                options.EnableTenantImpersonation = true;
            });
            context.Services.Configure<AbpIdentityProBlazorOptions>(options =>
            {
                options.EnableUserImpersonation = true;
            });
        }

        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<DominusDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Sapienza.Sapienza.Dominus.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<DominusApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Sapienza.Sapienza.Dominus.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<DominusBlazorServerModule>(hostingEnvironment.ContentRootPath);
                });
            }
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

        private void ConfigureCache(IConfiguration configuration)
        {
            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = "Dominus:";
            });
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

        private void ConfigureBlazorise(ServiceConfigurationContext context)
        {
            context.Services
                .AddBootstrap5Providers()
                .AddFontAwesomeIcons();
        }

        private void ConfigureMenu(IConfiguration configuration)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new DominusMenuContributor(configuration));
            });
        }

        private void ConfigureRouter()
        {
            Configure<AbpRouterOptions>(options =>
            {
                options.AppAssembly = typeof(DominusBlazorServerModule).Assembly;
            });
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<DominusBlazorServerModule>();
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var env = context.GetEnvironment();
            var app = context.GetApplicationBuilder();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCorrelationId();
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
