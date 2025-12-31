using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Applications.Dtos;

namespace Volo.Abp.OpenIddict.Pro.Web.Pages.OpenIddict.Applications;

public class TokenLifetimeModal : OpenIddictProPageModel
{
    protected IApplicationAppService ApplicationAppService { get; }

    [BindProperty]
    public Guid ApplicationId { get; set; }

    [BindProperty]
    public ApplicationTokenLifetimeModalView TokenLifetime { get; set; }

    public TokenLifetimeModal(IApplicationAppService applicationAppService)
    {
        ApplicationAppService = applicationAppService;
    }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        ApplicationId = id;
        var applicationTokenLifetime = await ApplicationAppService.GetTokenLifetimeAsync(id);
        TokenLifetime = ObjectMapper.Map<ApplicationTokenLifetimeDto, ApplicationTokenLifetimeModalView>(applicationTokenLifetime);
        return Page();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();

        await ApplicationAppService.SetTokenLifetimeAsync(ApplicationId, ObjectMapper.Map<ApplicationTokenLifetimeModalView, ApplicationTokenLifetimeDto>(TokenLifetime));

        return NoContent();
    }
}

public class ApplicationTokenLifetimeModalView
{
    public double? AccessTokenLifetime { get; set; }

    public double? AuthorizationCodeLifetime  { get; set; }

    public double? DeviceCodeLifetime  { get; set; }

    public double? IdentityTokenLifetime { get; set; }

    public double? RefreshTokenLifetime { get; set; }

    public double? UserCodeLifetime { get; set; }
}
