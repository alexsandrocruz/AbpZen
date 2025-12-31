using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Volo.Abp.Gdpr.Web.Extensions;

public static class AbpGdprApplicationBuilderExtensions
{
    public static IApplicationBuilder UseAbpCookieConsent(this IApplicationBuilder app)
    {
        var abpCookieConsentOptions = app.ApplicationServices.GetService<IOptions<AbpCookieConsentOptions>>();
        if (abpCookieConsentOptions != null && abpCookieConsentOptions.Value.IsEnabled)
        {
            app.UseCookiePolicy();
        }

        return app;
    }
}