using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.Abp.Gdpr.Web.Pages.Gdpr.Components.CookieConsent;

[Widget(
    ScriptFiles = new [] { "/Pages/Gdpr/Components/CookieConsent/Default.js" }
)]
public class AbpCookieConsentViewComponent : AbpViewComponent
{
    protected AbpCookieConsentOptions CookieConsentOptions { get; }

    public AbpCookieConsentViewComponent(IOptions<AbpCookieConsentOptions> cookieConsentOptions)
    {
        CookieConsentOptions = cookieConsentOptions.Value;
    }

    public IViewComponentResult Invoke()
    {
        var consentFeature = HttpContext.Features.Get<ITrackingConsentFeature>();
        var showCookieConsentBanner = CookieConsentOptions.IsEnabled && (!consentFeature?.CanTrack ?? false);

        if (!showCookieConsentBanner)
        {
            return new ContentViewComponentResult(string.Empty);
        }
        
        return View("~/Pages/Gdpr/Components/CookieConsent/Default.cshtml", new AbpCookieConsentViewModel 
        {
            Options = CookieConsentOptions,
            CookieString = consentFeature.CreateConsentCookie()
        });
    }
}

public class AbpCookieConsentViewModel 
{
    public string CookieString { get; set; }
    
    public AbpCookieConsentOptions Options { get; set; }
}