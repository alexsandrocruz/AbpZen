using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sapienza.Dominus.MongoDB;
using Sapienza.Dominus.MultiTenancy;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Public.Web;
using Volo.Abp.Account.Public.Web.Impersonation;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Account;
using Volo.Abp.Account.Public.Web.ExternalProviders;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Microsoft.AspNetCore.Hosting;
using Sapienza.Dominus.HealthChecks;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Identity;
using Volo.Abp.Swashbuckle;
using Volo.Saas.Host;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.Data;
using Volo.Abp.Threading;
using OpenIddict.Server.AspNetCore;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;

namespace Sapienza.Dominus
{
    [DependsOn(
        typeof(DominusHttpApiModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(DominusApplicationModule),
        typeof(DominusMongoDbModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAccountPublicWebImpersonationModule),
        typeof(AbpAccountPublicWebOpenIddictModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpAspNetCoreSerilogModule)
        )]
    public class DominusHttpApiHostModule : AbpModule
    {
         public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<OpenIddictBuilder>(builder =>
            {
                builder.AddValidation(options =>
                {
                    options.AddAudiences("Dominus");
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (!Convert.ToBoolean(configuration["App:DisablePII"]))
            {
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            }

            if (!Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]))
            {
                Configure<OpenIddictServerAspNetCoreOptions>(options =>
                {
                    options.DisableTransportSecurityRequirement = true;
                });
            }

            ConfigureAuthentication(context);
            ConfigureUrls(configuration);
            ConfigureBundles();
            ConfigureConventionalControllers();
            ConfigureAuthentication(context, configuration);
            ConfigureImpersonation(context, configuration);
            ConfigureSwagger(context, configuration);
            ConfigureVirtualFileSystem(context);
            ConfigureCors(context, configuration);
            ConfigureExternalProviders(context);
            ConfigureHealthChecks(context);
        }

        private void ConfigureHealthChecks(ServiceConfigurationContext context)
        {
            context.Services.AddDominusHealthChecks();
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
                options.Applications["Angular"].RootUrl = configuration["App:AngularUrl"];
                options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
                options.Applications["Angular"].Urls[AccountUrlNames.EmailConfirmation] = "account/email-confirmation";
                options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"].Split(','));
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context)
        {
            context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
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


        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<DominusDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Sapienza.Sapienza.Dominus.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<DominusDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Sapienza.Sapienza.Dominus.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<DominusApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Sapienza.Sapienza.Dominus.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<DominusApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Sapienza.Sapienza.Dominus.Application"));
                    // options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreMvcUiBasicThemeModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic", Path.DirectorySeparatorChar)));
                });
            }
        }

        private void ConfigureConventionalControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(DominusApplicationModule).Assembly);
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]); ;
                    options.Audience = "Dominus";
                    options.BackchannelHttpHandler = new HttpClientHandler()
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                });
        }

        private static void ConfigureSwagger(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAbpSwaggerGenWithOAuth(
                configuration["AuthServer:Authority"],
                new Dictionary<string, string>
                {
                    {"Dominus", "Dominus API"}
                },
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo {Title = "Dominus API", Version = "v1"});
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                });
        }

        private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.Trim().RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        private void ConfigureExternalProviders(ServiceConfigurationContext context)
        {
            context.Services.AddAuthentication()
                .AddGoogle(GoogleDefaults.AuthenticationScheme, _ => { })
                .WithDynamicOptions<GoogleOptions, GoogleHandler>(
                    GoogleDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.WithProperty(x => x.ClientId);
                        options.WithProperty(x => x.ClientSecret, isSecret: true);
                    }
                )
                .AddMicrosoftAccount(MicrosoftAccountDefaults.AuthenticationScheme, options =>
                {
                    //Personal Microsoft accounts as an example.
                    options.AuthorizationEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize";
                    options.TokenEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/token";
                })
                .WithDynamicOptions<MicrosoftAccountOptions, MicrosoftAccountHandler>(
                    MicrosoftAccountDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.WithProperty(x => x.ClientId);
                        options.WithProperty(x => x.ClientSecret, isSecret: true);
                    }
                )
                .AddTwitter(TwitterDefaults.AuthenticationScheme, options => options.RetrieveUserDetails = true)
                .WithDynamicOptions<TwitterOptions, TwitterHandler>(
                    TwitterDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.WithProperty(x => x.ConsumerKey);
                        options.WithProperty(x => x.ConsumerSecret, isSecret: true);
                    }
                );
        }

        private void ConfigureImpersonation(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.Configure<AbpAccountOptions>(options =>
            {
                options.TenantAdminUserName = "admin";
                options.ImpersonationTenantPermission = SaasHostPermissions.Tenants.Impersonation;
                options.ImpersonationUserPermission = IdentityPermissions.Users.Impersonation;
            });
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
            app.UseCors();
            app.UseAuthentication();
            app.UseAbpOpenIddictValidation();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseUnitOfWork();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Dominus API");

                var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
            });
            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();

            using var scope = app.ApplicationServices.CreateScope();
            AsyncHelper.RunSync(async ()=> await scope.ServiceProvider.GetService<IDataSeeder>().SeedAsync());
        }
    }
}
