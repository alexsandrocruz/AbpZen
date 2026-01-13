using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finPlanoContasGrupos;
using Sapienza.Lexus.finPlanoContasGrupos.Dtos;
using Sapienza.Lexus.Web.Pages.finPlanoContasGrupos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finPlanoContasGrupos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinPlanoContasGruposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinPlanoContasGruposAppService _finPlanoContasGruposAppService;

    public CreateModalModel(
        IfinPlanoContasGruposAppService finPlanoContasGruposAppService
    )
    {
        _finPlanoContasGruposAppService = finPlanoContasGruposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinPlanoContasGruposViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinPlanoContasGruposViewModel, CreateUpdatefinPlanoContasGruposDto>(ViewModel);
        await _finPlanoContasGruposAppService.CreateAsync(dto);
        return NoContent();
    }
}
