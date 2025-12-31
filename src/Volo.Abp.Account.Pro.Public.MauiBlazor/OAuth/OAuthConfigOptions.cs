namespace Volo.Abp.Account.Pro.Public.MauiBlazor.OAuth;

public class OAuthConfigOptions
{
    public string Authority { get; set; }

    public bool RequireHttpsMetadata { get; set; }

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }

    public string Scope { get; set; }

    public string GrantType { get; set; } = "code";
}
