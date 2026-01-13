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

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvAgeTiposCompromissosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvAgeTiposCompromissosAppService _advAgeTiposCompromissosAppService;

    public CreateModalModel(
        IadvAgeTiposCompromissosAppService advAgeTiposCompromissosAppService
    )
    {
        _advAgeTiposCompromissosAppService = advAgeTiposCompromissosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvAgeTiposCompromissosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvAgeTiposCompromissosViewModel, CreateUpdateadvAgeTiposCompromissosDto>(ViewModel);
        await _advAgeTiposCompromissosAppService.CreateAsync(dto);
        return NoContent();
    }
}
