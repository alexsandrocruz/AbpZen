using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabEstados;
using Sapienza.Lexus.fabEstados.Dtos;
using Sapienza.Lexus.Web.Pages.fabEstados.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabEstados;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefabEstadosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabEstadosAppService _fabEstadosAppService;

    public CreateModalModel(
        IfabEstadosAppService fabEstadosAppService
    )
    {
        _fabEstadosAppService = fabEstadosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefabEstadosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefabEstadosViewModel, CreateUpdatefabEstadosDto>(ViewModel);
        await _fabEstadosAppService.CreateAsync(dto);
        return NoContent();
    }
}
