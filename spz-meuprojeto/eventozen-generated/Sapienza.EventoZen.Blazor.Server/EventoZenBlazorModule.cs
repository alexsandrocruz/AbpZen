using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Sapienza.EventoZen.Blazor.Menus;
using Sapienza.EventoZen.Localization;
using Sapienza.EventoZen.MultiTenancy;
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
using Volo.Abp.AspNetCore.Components.Server.LeptonXTheme;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling;
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
using Volo.Abp.LeptonX.Shared;
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

namespace Sapienza.EventoZen.Blazor
{
    [DependsOn(
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpAspNetCoreSerilogModule),
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
        typeof(EventoZenHttpApiClientModule),
        typeof(AbpAspNetCoreMvcUiLeptonXThemeModule),
        typeof(AbpAspNetCoreComponentsServerLeptonXThemeModule),
        typeof(CmsKitProAdminBlazorServerModule)
       )]
    [DependsOn(typeof(AbpGdprBlazorServerModule))]
    [DependsOn(typeof(ChatBlazorServerModule), typeof(ChatSignalRModule))]
    public class EventoZenBlazorModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(EventoZenResource),
                    typeof(EventoZenDomainSharedModule).Assembly,
                    typeof(EventoZenApplicationContractsModule).Assembly,
                    typeof(EventoZenBlazorModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            Configure<LeptonXThemeBlazorOptions>(options =>
            {
                options.Layout = LeptonXBlazorLayouts.SideMenu;
            });

            Configure<LeptonXThemeMvcOptions>(options =>
            {
                options.ApplicationLayout = LeptonXMvcLayouts.SideMenu;
            });

            Configure<LeptonXThemeOptions>(options =>
            {
                /* To test without style changes, comment out the following lines:*/
                // options.Styles.Clear();
                // options.DefaultStyle = LeptonXStyleNames.System;
            });

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
                    options.Scope.Add("Sapienza.EventoZen");
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
                    options.FileSets.ReplaceEmbeddedByPhysical<EventoZenDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Sapienza.EventoZen.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<EventoZenApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Sapienza.EventoZen.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<EventoZenBlazorModule>(hostingEnvironment.ContentRootPath);
                    // options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreMvcUiLeptonXThemeModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX", Path.DirectorySeparatorChar)));
                    // options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreComponentsWebLeptonXThemeModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Volo.Abp.AspNetCore.Components.Server.LeptonXTheme", Path.DirectorySeparatorChar)));
                });
            }
        }

        private void ConfigureBundles()
        {
            Configure<AbpBundlingOptions>(options =>
            {
                options.StyleBundles.Configure(
                    LeptonXThemeBundles.Styles.Global,
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
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Sapienza.EventoZen API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
        }

        private void ConfigureCache(IConfiguration configuration)
        {
            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = "Sapienza.EventoZen:";
            });
        }

        private void ConfigureDataProtection(
            ServiceConfigurationContext context,
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment)
        {
            var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("Sapienza.EventoZen");
            if (!hostingEnvironment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "Sapienza.EventoZen-Protection-Keys");
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
                options.MenuContributors.Add(new EventoZenMenuContributor(configuration));
            });
        }

        private void ConfigureRouter()
        {
            Configure<AbpRouterOptions>(options =>
            {
                options.AppAssembly = typeof(EventoZenBlazorModule).Assembly;
            });
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<EventoZenBlazorModule>();
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
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sapienza.EventoZen API");
            });
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}
