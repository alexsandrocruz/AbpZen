using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProFases;
using Sapienza.Lexus.advProFases.Dtos;
using Sapienza.Lexus.Web.Pages.advProFases.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProFases;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProFasesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProFasesAppService _advProFasesAppService;

    public CreateModalModel(
        IadvProFasesAppService advProFasesAppService
    )
    {
        _advProFasesAppService = advProFasesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProFasesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProFasesViewModel, CreateUpdateadvProFasesDto>(ViewModel);
        await _advProFasesAppService.CreateAsync(dto);
        return NoContent();
    }
}
