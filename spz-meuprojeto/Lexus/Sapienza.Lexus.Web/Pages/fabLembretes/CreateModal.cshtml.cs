using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabLembretes;
using Sapienza.Lexus.fabLembretes.Dtos;
using Sapienza.Lexus.Web.Pages.fabLembretes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabLembretes;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefabLembretesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabLembretesAppService _fabLembretesAppService;

    public CreateModalModel(
        IfabLembretesAppService fabLembretesAppService
    )
    {
        _fabLembretesAppService = fabLembretesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefabLembretesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefabLembretesViewModel, CreateUpdatefabLembretesDto>(ViewModel);
        await _fabLembretesAppService.CreateAsync(dto);
        return NoContent();
    }
}
