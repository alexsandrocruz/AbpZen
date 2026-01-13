using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabDatasEFeriados;
using Sapienza.Lexus.fabDatasEFeriados.Dtos;
using Sapienza.Lexus.Web.Pages.fabDatasEFeriados.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabDatasEFeriados;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefabDatasEFeriadosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabDatasEFeriadosAppService _fabDatasEFeriadosAppService;

    public CreateModalModel(
        IfabDatasEFeriadosAppService fabDatasEFeriadosAppService
    )
    {
        _fabDatasEFeriadosAppService = fabDatasEFeriadosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefabDatasEFeriadosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefabDatasEFeriadosViewModel, CreateUpdatefabDatasEFeriadosDto>(ViewModel);
        await _fabDatasEFeriadosAppService.CreateAsync(dto);
        return NoContent();
    }
}
