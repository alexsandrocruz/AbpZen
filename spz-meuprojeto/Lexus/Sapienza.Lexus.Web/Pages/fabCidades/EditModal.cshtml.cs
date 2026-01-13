using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabCidades;
using Sapienza.Lexus.fabCidades.Dtos;
using Sapienza.Lexus.Web.Pages.fabCidades.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabCidades;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfabCidadesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabCidadesAppService _fabCidadesAppService;

    public EditModalModel(
        IfabCidadesAppService fabCidadesAppService
    )
    {
        _fabCidadesAppService = fabCidadesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _fabCidadesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<fabCidadesDto, EditfabCidadesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfabCidadesViewModel, CreateUpdatefabCidadesDto>(ViewModel);
        await _fabCidadesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
