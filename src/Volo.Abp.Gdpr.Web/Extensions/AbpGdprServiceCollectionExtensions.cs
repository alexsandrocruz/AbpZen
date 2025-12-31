using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Gdpr.Web.Pages.Gdpr.Components.CookieConsent;
using Volo.Abp.Ui.LayoutHooks;

namespace Volo.Abp.Gdpr.Web.Extensions;

public static class AbpGdprServiceCollectionExtensions
{
    public static IServiceCollection AddAbpCookieConsent(this IServiceCollection services, Action<AbpCookieConsentOptions> cookieConsentOptions = null)
    {
        Check.NotNull(services, nameof(services));

        services.Configure<AbpLayoutHookOptions>(hookOptions => 
        {
            hookOptions.Add(
                LayoutHooks.Body.First,
                typeof(AbpCookieConsentViewComponent),
                layout: StandardLayouts.Application
            );
        });

        var abpCookieConsentOptions = new AbpCookieConsentOptions();
        cookieConsentOptions?.Invoke(abpCookieConsentOptions);

        services.Configure<AbpCookieConsentOptions>(cookieConsentOptions);

        services.AddCookiePolicy(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.ConsentCookie.Expiration = abpCookieConsentOptions.Expiration;
        });

        return services;
    }
}