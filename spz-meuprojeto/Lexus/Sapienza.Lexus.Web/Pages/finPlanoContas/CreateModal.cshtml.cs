using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finPlanoContas;
using Sapienza.Lexus.finPlanoContas.Dtos;
using Sapienza.Lexus.Web.Pages.finPlanoContas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finPlanoContas;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinPlanoContasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinPlanoContasAppService _finPlanoContasAppService;

    public CreateModalModel(
        IfinPlanoContasAppService finPlanoContasAppService
    )
    {
        _finPlanoContasAppService = finPlanoContasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinPlanoContasViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinPlanoContasViewModel, CreateUpdatefinPlanoContasDto>(ViewModel);
        await _finPlanoContasAppService.CreateAsync(dto);
        return NoContent();
    }
}
