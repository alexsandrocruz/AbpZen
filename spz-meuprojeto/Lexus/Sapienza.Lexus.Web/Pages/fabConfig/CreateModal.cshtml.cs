using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabConfig;
using Sapienza.Lexus.fabConfig.Dtos;
using Sapienza.Lexus.Web.Pages.fabConfig.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabConfig;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefabConfigViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabConfigAppService _fabConfigAppService;

    public CreateModalModel(
        IfabConfigAppService fabConfigAppService
    )
    {
        _fabConfigAppService = fabConfigAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefabConfigViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefabConfigViewModel, CreateUpdatefabConfigDto>(ViewModel);
        await _fabConfigAppService.CreateAsync(dto);
        return NoContent();
    }
}
