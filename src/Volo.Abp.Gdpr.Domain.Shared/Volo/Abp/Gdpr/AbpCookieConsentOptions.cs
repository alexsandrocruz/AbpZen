using System;

namespace Volo.Abp.Gdpr;

public class AbpCookieConsentOptions
{
    public bool IsEnabled { get; set; }
    
    public string CookiePolicyUrl { get; set; }
    
    public string PrivacyPolicyUrl { get; set; }

    public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(180);
}