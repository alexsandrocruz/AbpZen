using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProEscritorios;
using Sapienza.Lexus.advProEscritorios.Dtos;
using Sapienza.Lexus.Web.Pages.advProEscritorios.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProEscritorios;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProEscritoriosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProEscritoriosAppService _advProEscritoriosAppService;

    public CreateModalModel(
        IadvProEscritoriosAppService advProEscritoriosAppService
    )
    {
        _advProEscritoriosAppService = advProEscritoriosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProEscritoriosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProEscritoriosViewModel, CreateUpdateadvProEscritoriosDto>(ViewModel);
        await _advProEscritoriosAppService.CreateAsync(dto);
        return NoContent();
    }
}
