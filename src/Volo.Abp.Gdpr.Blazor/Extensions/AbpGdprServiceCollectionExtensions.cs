using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.Gdpr.Blazor.Components;

namespace Volo.Abp.Gdpr.Blazor.Extensions;

public static class AbpGdprServiceCollectionExtensions
{
    public static IServiceCollection AddAbpCookieConsent(this IServiceCollection services, Action<AbpCookieConsentOptions> cookieConsentOptions = null)
    {
        Check.NotNull(services, nameof(services));

        services.Configure<AbpCookieConsentOptions>(options =>
        {
            cookieConsentOptions?.Invoke(options);
        });
        
        return services.Configure<AbpDynamicLayoutComponentOptions>(options =>
        {
            options.Components.Add(typeof(AbpCookieConsentComponent), null);
        });
    }
}
