using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabEstados;
using Sapienza.Lexus.fabEstados.Dtos;
using Sapienza.Lexus.Web.Pages.fabEstados.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabEstados;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfabEstadosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabEstadosAppService _fabEstadosAppService;

    public EditModalModel(
        IfabEstadosAppService fabEstadosAppService
    )
    {
        _fabEstadosAppService = fabEstadosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _fabEstadosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<fabEstadosDto, EditfabEstadosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfabEstadosViewModel, CreateUpdatefabEstadosDto>(ViewModel);
        await _fabEstadosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
