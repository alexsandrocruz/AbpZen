using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProcessosMeritos;
using Sapienza.Lexus.advProcessosMeritos.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessosMeritos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProcessosMeritos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProcessosMeritosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProcessosMeritosAppService _advProcessosMeritosAppService;

    public CreateModalModel(
        IadvProcessosMeritosAppService advProcessosMeritosAppService
    )
    {
        _advProcessosMeritosAppService = advProcessosMeritosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProcessosMeritosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProcessosMeritosViewModel, CreateUpdateadvProcessosMeritosDto>(ViewModel);
        await _advProcessosMeritosAppService.CreateAsync(dto);
        return NoContent();
    }
}
