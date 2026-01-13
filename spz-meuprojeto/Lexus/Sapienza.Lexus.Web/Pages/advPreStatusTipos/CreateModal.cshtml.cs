using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPreStatusTipos;
using Sapienza.Lexus.advPreStatusTipos.Dtos;
using Sapienza.Lexus.Web.Pages.advPreStatusTipos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPreStatusTipos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvPreStatusTiposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreStatusTiposAppService _advPreStatusTiposAppService;

    public CreateModalModel(
        IadvPreStatusTiposAppService advPreStatusTiposAppService
    )
    {
        _advPreStatusTiposAppService = advPreStatusTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvPreStatusTiposViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvPreStatusTiposViewModel, CreateUpdateadvPreStatusTiposDto>(ViewModel);
        await _advPreStatusTiposAppService.CreateAsync(dto);
        return NoContent();
    }
}
