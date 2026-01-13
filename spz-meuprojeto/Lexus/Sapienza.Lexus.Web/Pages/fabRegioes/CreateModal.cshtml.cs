using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabRegioes;
using Sapienza.Lexus.fabRegioes.Dtos;
using Sapienza.Lexus.Web.Pages.fabRegioes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabRegioes;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefabRegioesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabRegioesAppService _fabRegioesAppService;

    public CreateModalModel(
        IfabRegioesAppService fabRegioesAppService
    )
    {
        _fabRegioesAppService = fabRegioesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefabRegioesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefabRegioesViewModel, CreateUpdatefabRegioesDto>(ViewModel);
        await _fabRegioesAppService.CreateAsync(dto);
        return NoContent();
    }
}
