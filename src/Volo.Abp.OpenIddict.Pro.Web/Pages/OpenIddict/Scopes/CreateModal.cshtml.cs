using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.ObjectExtending;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Scopes.Dtos;

namespace Volo.Abp.OpenIddict.Pro.Web.Pages.OpenIddict.Scopes;

public class CreateModal : OpenIddictProPageModel
{
    [BindProperty]
    public ScopeCreateModalView Scope { get; set; }

    protected virtual IScopeAppService ScopeAppService { get; }
    
    public CreateModal(IScopeAppService scopeAppService)
    {
        ScopeAppService = scopeAppService;
    }
    
    public virtual Task<IActionResult> OnGetAsync()
    {
        Scope = new ScopeCreateModalView();
        return Task.FromResult<IActionResult>(Page());
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();

        var input = ObjectMapper.Map<ScopeCreateModalView, CreateScopeInput>(Scope);
        await ScopeAppService.CreateAsync(input);

        return NoContent();
    }
    
}

public class ScopeCreateModalView : ExtensibleObject
{
    [Required]
    [RegularExpression(@"\w+", ErrorMessage = "TheScopeNameCannotContainSpaces")]
    public string Name { get; set; }

    public string DisplayName { get; set; }
    
    public string Description { get; set; }

    [TextArea]
    public string Resources { get; set; }
}