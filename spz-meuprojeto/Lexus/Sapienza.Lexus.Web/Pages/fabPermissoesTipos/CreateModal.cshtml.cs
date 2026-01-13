using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabPermissoesTipos;
using Sapienza.Lexus.fabPermissoesTipos.Dtos;
using Sapienza.Lexus.Web.Pages.fabPermissoesTipos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabPermissoesTipos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefabPermissoesTiposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabPermissoesTiposAppService _fabPermissoesTiposAppService;

    public CreateModalModel(
        IfabPermissoesTiposAppService fabPermissoesTiposAppService
    )
    {
        _fabPermissoesTiposAppService = fabPermissoesTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefabPermissoesTiposViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefabPermissoesTiposViewModel, CreateUpdatefabPermissoesTiposDto>(ViewModel);
        await _fabPermissoesTiposAppService.CreateAsync(dto);
        return NoContent();
    }
}
