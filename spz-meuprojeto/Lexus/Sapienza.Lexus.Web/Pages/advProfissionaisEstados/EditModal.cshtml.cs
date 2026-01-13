using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProfissionaisEstados;
using Sapienza.Lexus.advProfissionaisEstados.Dtos;
using Sapienza.Lexus.Web.Pages.advProfissionaisEstados.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProfissionaisEstados;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProfissionaisEstadosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProfissionaisEstadosAppService _advProfissionaisEstadosAppService;

    public EditModalModel(
        IadvProfissionaisEstadosAppService advProfissionaisEstadosAppService
    )
    {
        _advProfissionaisEstadosAppService = advProfissionaisEstadosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProfissionaisEstadosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProfissionaisEstadosDto, EditadvProfissionaisEstadosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProfissionaisEstadosViewModel, CreateUpdateadvProfissionaisEstadosDto>(ViewModel);
        await _advProfissionaisEstadosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
