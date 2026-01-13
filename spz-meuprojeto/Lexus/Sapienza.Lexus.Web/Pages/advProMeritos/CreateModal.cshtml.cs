using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProMeritos;
using Sapienza.Lexus.advProMeritos.Dtos;
using Sapienza.Lexus.Web.Pages.advProMeritos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProMeritos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProMeritosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProMeritosAppService _advProMeritosAppService;

    public CreateModalModel(
        IadvProMeritosAppService advProMeritosAppService
    )
    {
        _advProMeritosAppService = advProMeritosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProMeritosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProMeritosViewModel, CreateUpdateadvProMeritosDto>(ViewModel);
        await _advProMeritosAppService.CreateAsync(dto);
        return NoContent();
    }
}
