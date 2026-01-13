using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProTipos;
using Sapienza.Lexus.advProTipos.Dtos;
using Sapienza.Lexus.Web.Pages.advProTipos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProTipos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProTiposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProTiposAppService _advProTiposAppService;

    public CreateModalModel(
        IadvProTiposAppService advProTiposAppService
    )
    {
        _advProTiposAppService = advProTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProTiposViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProTiposViewModel, CreateUpdateadvProTiposDto>(ViewModel);
        await _advProTiposAppService.CreateAsync(dto);
        return NoContent();
    }
}
