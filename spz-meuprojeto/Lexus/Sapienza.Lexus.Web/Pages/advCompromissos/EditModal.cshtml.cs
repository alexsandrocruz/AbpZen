using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advCompromissos;
using Sapienza.Lexus.advCompromissos.Dtos;
using Sapienza.Lexus.Web.Pages.advCompromissos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advCompromissos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvCompromissosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCompromissosAppService _advCompromissosAppService;

    public EditModalModel(
        IadvCompromissosAppService advCompromissosAppService
    )
    {
        _advCompromissosAppService = advCompromissosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advCompromissosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advCompromissosDto, EditadvCompromissosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvCompromissosViewModel, CreateUpdateadvCompromissosDto>(ViewModel);
        await _advCompromissosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
