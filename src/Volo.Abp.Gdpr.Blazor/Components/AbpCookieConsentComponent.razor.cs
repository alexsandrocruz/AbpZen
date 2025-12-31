using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Components.Web;

namespace Volo.Abp.Gdpr.Blazor.Components;

public partial class AbpCookieConsentComponent
{
    protected const string CookieKey = ".AspNet.Consent";
    
    [Inject]
    protected ICookieService CookieService { get; set; }
    
    [Inject]
    protected IOptions<AbpCookieConsentOptions> AbpCookieConsentOptions { get; set; }

    protected bool ShowCookieConsent { get; set; }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ShowCookieConsent = AbpCookieConsentOptions.Value.IsEnabled 
                             && string.IsNullOrEmpty(await CookieService.GetAsync(CookieKey));
            
            await InvokeAsync(StateHasChanged);
        }
    }

    protected virtual async Task AcceptCookieConsentAsync()
    {
        await CookieService.SetAsync(CookieKey, bool.TrueString, new CookieOptions
        {
            ExpireDate = DateTimeOffset.Now.Add(AbpCookieConsentOptions.Value.Expiration)
        });
        ShowCookieConsent = false;
        await InvokeAsync(StateHasChanged);
    }

    private static string GetLink(string url, string text)
    {
        return $"<a href='{url}' class='text-white' target='_blank'>{text}</a>";
    }

    protected virtual MarkupString GetCookieConsentText()
    {
        var builder = new StringBuilder();
        builder.Append(L["ThisWebsiteUsesCookie"].Value);

        if (!string.IsNullOrWhiteSpace(AbpCookieConsentOptions.Value.CookiePolicyUrl) &&
            !string.IsNullOrWhiteSpace(AbpCookieConsentOptions.Value.PrivacyPolicyUrl))
        {
            builder.AppendLine(L["CookieConsentAgreePolicies",
                GetLink(AbpCookieConsentOptions.Value.CookiePolicyUrl, L["CookiePolicy"].Value),
                GetLink(AbpCookieConsentOptions.Value.PrivacyPolicyUrl, L["PrivacyPolicy"].Value)].Value);
        }
        else if (!string.IsNullOrWhiteSpace(AbpCookieConsentOptions.Value.CookiePolicyUrl))
        {
            builder.AppendLine(L["CookieConsentAgreePolicy", GetLink(AbpCookieConsentOptions.Value.CookiePolicyUrl, L["CookiePolicy"].Value)].Value);
        }
        else if (!string.IsNullOrWhiteSpace(AbpCookieConsentOptions.Value.PrivacyPolicyUrl))
        {
            builder.AppendLine(L["CookieConsentAgreePolicy", GetLink(AbpCookieConsentOptions.Value.PrivacyPolicyUrl, L["PrivacyPolicy"].Value)].Value);
        }

        return (MarkupString)builder.ToString();
    }
}