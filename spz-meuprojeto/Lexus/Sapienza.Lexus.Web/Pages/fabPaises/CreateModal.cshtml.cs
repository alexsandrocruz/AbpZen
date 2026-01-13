using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabPaises;
using Sapienza.Lexus.fabPaises.Dtos;
using Sapienza.Lexus.Web.Pages.fabPaises.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabPaises;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefabPaisesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabPaisesAppService _fabPaisesAppService;

    public CreateModalModel(
        IfabPaisesAppService fabPaisesAppService
    )
    {
        _fabPaisesAppService = fabPaisesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefabPaisesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefabPaisesViewModel, CreateUpdatefabPaisesDto>(ViewModel);
        await _fabPaisesAppService.CreateAsync(dto);
        return NoContent();
    }
}
