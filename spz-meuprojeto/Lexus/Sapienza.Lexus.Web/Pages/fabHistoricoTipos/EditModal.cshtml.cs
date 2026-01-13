using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabHistoricoTipos;
using Sapienza.Lexus.fabHistoricoTipos.Dtos;
using Sapienza.Lexus.Web.Pages.fabHistoricoTipos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabHistoricoTipos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfabHistoricoTiposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabHistoricoTiposAppService _fabHistoricoTiposAppService;

    public EditModalModel(
        IfabHistoricoTiposAppService fabHistoricoTiposAppService
    )
    {
        _fabHistoricoTiposAppService = fabHistoricoTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _fabHistoricoTiposAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<fabHistoricoTiposDto, EditfabHistoricoTiposViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfabHistoricoTiposViewModel, CreateUpdatefabHistoricoTiposDto>(ViewModel);
        await _fabHistoricoTiposAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
