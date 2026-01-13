using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.opoOrcamentos;
using Sapienza.Lexus.opoOrcamentos.Dtos;
using Sapienza.Lexus.Web.Pages.opoOrcamentos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.opoOrcamentos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateopoOrcamentosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IopoOrcamentosAppService _opoOrcamentosAppService;

    public CreateModalModel(
        IopoOrcamentosAppService opoOrcamentosAppService
    )
    {
        _opoOrcamentosAppService = opoOrcamentosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateopoOrcamentosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateopoOrcamentosViewModel, CreateUpdateopoOrcamentosDto>(ViewModel);
        await _opoOrcamentosAppService.CreateAsync(dto);
        return NoContent();
    }
}
