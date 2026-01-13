using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advAgeTiposCompromissos;
using Sapienza.Lexus.advAgeTiposCompromissos.Dtos;
using Sapienza.Lexus.Web.Pages.advAgeTiposCompromissos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advAgeTiposCompromissos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvAgeTiposCompromissosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvAgeTiposCompromissosAppService _advAgeTiposCompromissosAppService;

    public EditModalModel(
        IadvAgeTiposCompromissosAppService advAgeTiposCompromissosAppService
    )
    {
        _advAgeTiposCompromissosAppService = advAgeTiposCompromissosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advAgeTiposCompromissosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advAgeTiposCompromissosDto, EditadvAgeTiposCompromissosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvAgeTiposCompromissosViewModel, CreateUpdateadvAgeTiposCompromissosDto>(ViewModel);
        await _advAgeTiposCompromissosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
