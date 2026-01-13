using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.logCampos;
using Sapienza.Lexus.logCampos.Dtos;
using Sapienza.Lexus.Web.Pages.logCampos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.logCampos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatelogCamposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IlogCamposAppService _logCamposAppService;

    public CreateModalModel(
        IlogCamposAppService logCamposAppService
    )
    {
        _logCamposAppService = logCamposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatelogCamposViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatelogCamposViewModel, CreateUpdatelogCamposDto>(ViewModel);
        await _logCamposAppService.CreateAsync(dto);
        return NoContent();
    }
}
