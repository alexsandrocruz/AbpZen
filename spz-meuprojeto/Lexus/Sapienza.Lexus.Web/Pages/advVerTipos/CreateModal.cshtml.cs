using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advVerTipos;
using Sapienza.Lexus.advVerTipos.Dtos;
using Sapienza.Lexus.Web.Pages.advVerTipos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advVerTipos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvVerTiposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvVerTiposAppService _advVerTiposAppService;

    public CreateModalModel(
        IadvVerTiposAppService advVerTiposAppService
    )
    {
        _advVerTiposAppService = advVerTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvVerTiposViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvVerTiposViewModel, CreateUpdateadvVerTiposDto>(ViewModel);
        await _advVerTiposAppService.CreateAsync(dto);
        return NoContent();
    }
}
