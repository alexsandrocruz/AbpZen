using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenIddict.Abstractions;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.ObjectExtending;
using Volo.Abp.OpenIddict.Applications.Dtos;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Scopes.Dtos;

namespace Volo.Abp.OpenIddict.Pro.Web.Pages.OpenIddict.Applications;

public class CreateModal : OpenIddictProPageModel
{
    protected IScopeAppService ScopeAppService { get; }

    protected IApplicationAppService ApplicationAppService { get; }

    public List<ScopeDto> Scopes { get; set; }

    public List<SelectListItem> ApplicationTypes { get; set; }
    public List<SelectListItem> Types { get; set; }

    public List<SelectListItem> ConsentTypes { get; set; }

    [BindProperty]
    public ApplicationCreateModalView Application { get; set; }


    public CreateModal(IScopeAppService scopeAppService, IApplicationAppService applicationAppService)
    {
        ScopeAppService = scopeAppService;
        ApplicationAppService = applicationAppService;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        Application = new ApplicationCreateModalView();
        Scopes = await ScopeAppService.GetAllScopesAsync();
        ApplicationTypes = new List<SelectListItem>
        {
            new("Web", OpenIddictConstants.ApplicationTypes.Web),
            new("Native", OpenIddictConstants.ApplicationTypes.Native)
        };
        Types = new List<SelectListItem>
        {
            new("Confidential client", OpenIddictConstants.ClientTypes.Confidential),
            new("Public client", OpenIddictConstants.ClientTypes.Public)
        };
        ConsentTypes = new List<SelectListItem>
        {
            new("Explicit consent", OpenIddictConstants.ConsentTypes.Explicit),
            new("External consent", OpenIddictConstants.ConsentTypes.External),
            new("Implicit consent", OpenIddictConstants.ConsentTypes.Implicit),
            new("Systematic consent", OpenIddictConstants.ConsentTypes.Systematic),
        };

        return Page();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();

        var input = ObjectMapper.Map<ApplicationCreateModalView, CreateApplicationInput>(Application);
        await ApplicationAppService.CreateAsync(input);

        return NoContent();
    }
}

public class ApplicationCreateModalView : ExtensibleObject
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

    public string ClientUri { get; set; }

    public string LogoUri { get; set; }

    [TextArea]
    public string ExtensionGrantTypes { get; set; }

    [TextArea]
    public string PostLogoutRedirectUris { get; set; }

    [TextArea]
    public string RedirectUris { get; set; }

    public bool AllowPasswordFlow { get; set; }

    public bool AllowClientCredentialsFlow { get; set; }

    public bool AllowAuthorizationCodeFlow { get; set; }

    public bool AllowRefreshTokenFlow { get; set; }

    public bool AllowHybridFlow { get; set; }

    public bool AllowImplicitFlow { get; set; }

    public bool AllowLogoutEndpoint { get; set; }

    public bool AllowDeviceEndpoint { get; set; }

    public HashSet<string> Scopes { get; set; }
}
