using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.opoTipos;
using Sapienza.Lexus.opoTipos.Dtos;
using Sapienza.Lexus.Web.Pages.opoTipos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.opoTipos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateopoTiposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IopoTiposAppService _opoTiposAppService;

    public CreateModalModel(
        IopoTiposAppService opoTiposAppService
    )
    {
        _opoTiposAppService = opoTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateopoTiposViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateopoTiposViewModel, CreateUpdateopoTiposDto>(ViewModel);
        await _opoTiposAppService.CreateAsync(dto);
        return NoContent();
    }
}
