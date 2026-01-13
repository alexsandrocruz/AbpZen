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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditopoOrcamentosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IopoOrcamentosAppService _opoOrcamentosAppService;

    public EditModalModel(
        IopoOrcamentosAppService opoOrcamentosAppService
    )
    {
        _opoOrcamentosAppService = opoOrcamentosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _opoOrcamentosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<opoOrcamentosDto, EditopoOrcamentosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditopoOrcamentosViewModel, CreateUpdateopoOrcamentosDto>(ViewModel);
        await _opoOrcamentosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
