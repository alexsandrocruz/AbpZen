using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabHistoricoTipos;
using Sapienza.Lexus.fabHistoricoTipos.Dtos;
using Sapienza.Lexus.Web.Pages.fabHistoricoTipos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabHistoricoTipos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefabHistoricoTiposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabHistoricoTiposAppService _fabHistoricoTiposAppService;

    public CreateModalModel(
        IfabHistoricoTiposAppService fabHistoricoTiposAppService
    )
    {
        _fabHistoricoTiposAppService = fabHistoricoTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefabHistoricoTiposViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefabHistoricoTiposViewModel, CreateUpdatefabHistoricoTiposDto>(ViewModel);
        await _fabHistoricoTiposAppService.CreateAsync(dto);
        return NoContent();
    }
}
