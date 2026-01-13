using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProfissionaisNaturezas;
using Sapienza.Lexus.advProfissionaisNaturezas.Dtos;
using Sapienza.Lexus.Web.Pages.advProfissionaisNaturezas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProfissionaisNaturezas;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProfissionaisNaturezasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProfissionaisNaturezasAppService _advProfissionaisNaturezasAppService;

    public CreateModalModel(
        IadvProfissionaisNaturezasAppService advProfissionaisNaturezasAppService
    )
    {
        _advProfissionaisNaturezasAppService = advProfissionaisNaturezasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProfissionaisNaturezasViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProfissionaisNaturezasViewModel, CreateUpdateadvProfissionaisNaturezasDto>(ViewModel);
        await _advProfissionaisNaturezasAppService.CreateAsync(dto);
        return NoContent();
    }
}
