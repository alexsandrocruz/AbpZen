using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProNaturezas;
using Sapienza.Lexus.advProNaturezas.Dtos;
using Sapienza.Lexus.Web.Pages.advProNaturezas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProNaturezas;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProNaturezasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProNaturezasAppService _advProNaturezasAppService;

    public CreateModalModel(
        IadvProNaturezasAppService advProNaturezasAppService
    )
    {
        _advProNaturezasAppService = advProNaturezasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProNaturezasViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProNaturezasViewModel, CreateUpdateadvProNaturezasDto>(ViewModel);
        await _advProNaturezasAppService.CreateAsync(dto);
        return NoContent();
    }
}
