using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.flwConfig;
using Sapienza.Lexus.flwConfig.Dtos;
using Sapienza.Lexus.Web.Pages.flwConfig.ViewModels;

namespace Sapienza.Lexus.Web.Pages.flwConfig;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateflwConfigViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IflwConfigAppService _flwConfigAppService;

    public CreateModalModel(
        IflwConfigAppService flwConfigAppService
    )
    {
        _flwConfigAppService = flwConfigAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateflwConfigViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateflwConfigViewModel, CreateUpdateflwConfigDto>(ViewModel);
        await _flwConfigAppService.CreateAsync(dto);
        return NoContent();
    }
}
