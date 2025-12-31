using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.OpenIddict.Applications.Dtos;

public abstract class ApplicationCreateOrUpdateDtoBase : ExtensibleObject
{
    [Required]
    public string ApplicationType { get; set; }

    [Required]
    public string ClientId { get; set; }

    [Required]
    public string DisplayName { get; set; }

    public string ClientType { get; set; }

    public string ClientSecret { get; set; }

    public string ConsentType { get; set; }

    public HashSet<string> ExtensionGrantTypes { get; set; }

    public HashSet<string> PostLogoutRedirectUris { get; set; }

    public HashSet<string> RedirectUris { get; set; }

    public bool AllowPasswordFlow { get; set; }

    public bool AllowClientCredentialsFlow { get; set; }

    public bool AllowAuthorizationCodeFlow { get; set; }

    public bool AllowRefreshTokenFlow { get; set; }

    public bool AllowHybridFlow { get; set; }

    public bool AllowImplicitFlow { get; set; }

    public bool AllowLogoutEndpoint { get; set; }

    public bool AllowDeviceEndpoint { get; set; }

    public HashSet<string> Scopes { get; set; }

    public string ClientUri { get; set; }

    public string LogoUri { get; set; }

    public ApplicationCreateOrUpdateDtoBase()
        : base(false)
    {

    }
}
