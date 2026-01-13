using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finPlanoContasDet;
using Sapienza.Lexus.finPlanoContasDet.Dtos;
using Sapienza.Lexus.Web.Pages.finPlanoContasDet.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finPlanoContasDet;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinPlanoContasDetViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinPlanoContasDetAppService _finPlanoContasDetAppService;

    public CreateModalModel(
        IfinPlanoContasDetAppService finPlanoContasDetAppService
    )
    {
        _finPlanoContasDetAppService = finPlanoContasDetAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinPlanoContasDetViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinPlanoContasDetViewModel, CreateUpdatefinPlanoContasDetDto>(ViewModel);
        await _finPlanoContasDetAppService.CreateAsync(dto);
        return NoContent();
    }
}
