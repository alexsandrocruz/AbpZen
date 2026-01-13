using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabDatasEFeriados;
using Sapienza.Lexus.fabDatasEFeriados.Dtos;
using Sapienza.Lexus.Web.Pages.fabDatasEFeriados.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabDatasEFeriados;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfabDatasEFeriadosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabDatasEFeriadosAppService _fabDatasEFeriadosAppService;

    public EditModalModel(
        IfabDatasEFeriadosAppService fabDatasEFeriadosAppService
    )
    {
        _fabDatasEFeriadosAppService = fabDatasEFeriadosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _fabDatasEFeriadosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<fabDatasEFeriadosDto, EditfabDatasEFeriadosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfabDatasEFeriadosViewModel, CreateUpdatefabDatasEFeriadosDto>(ViewModel);
        await _fabDatasEFeriadosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
