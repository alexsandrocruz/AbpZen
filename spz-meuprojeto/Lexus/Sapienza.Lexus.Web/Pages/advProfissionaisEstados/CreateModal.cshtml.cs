using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProfissionaisEstados;
using Sapienza.Lexus.advProfissionaisEstados.Dtos;
using Sapienza.Lexus.Web.Pages.advProfissionaisEstados.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProfissionaisEstados;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProfissionaisEstadosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProfissionaisEstadosAppService _advProfissionaisEstadosAppService;

    public CreateModalModel(
        IadvProfissionaisEstadosAppService advProfissionaisEstadosAppService
    )
    {
        _advProfissionaisEstadosAppService = advProfissionaisEstadosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProfissionaisEstadosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProfissionaisEstadosViewModel, CreateUpdateadvProfissionaisEstadosDto>(ViewModel);
        await _advProfissionaisEstadosAppService.CreateAsync(dto);
        return NoContent();
    }
}
