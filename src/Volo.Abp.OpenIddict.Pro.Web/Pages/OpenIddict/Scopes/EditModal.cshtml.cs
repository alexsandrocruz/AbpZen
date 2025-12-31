using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.ObjectExtending;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Scopes.Dtos;

namespace Volo.Abp.OpenIddict.Pro.Web.Pages.OpenIddict.Scopes;

public class EditModal : OpenIddictProPageModel
{
    [BindProperty]
    public ScopeEditModelView Scope { get; set; }
    
    protected IScopeAppService ScopeAppService { get; }
    
    public EditModal(IScopeAppService scopeAppService)
    {
        ScopeAppService = scopeAppService;
    }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        Scope = ObjectMapper.Map<ScopeDto, ScopeEditModelView>(
            await ScopeAppService.GetAsync(id)
        );

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();
        
        var input = ObjectMapper.Map<ScopeEditModelView, UpdateScopeInput>(Scope);
        await ScopeAppService.UpdateAsync(Scope.Id, input);

        return NoContent();
    }
}

public class ScopeEditModelView : ExtensibleObject
{
    public Guid Id { get; set; }
        
    [Required]
    [RegularExpression(@"\w+", ErrorMessage = "TheScopeNameCannotContainSpaces")]
    public string Name { get; set; }

    public string DisplayName { get; set; }
    
    public string Description { get; set; }

    [TextArea]
    public string Resources { get; set; }
}