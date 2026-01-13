using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabPermissoesTipos;
using Sapienza.Lexus.fabPermissoesTipos.Dtos;
using Sapienza.Lexus.Web.Pages.fabPermissoesTipos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabPermissoesTipos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfabPermissoesTiposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabPermissoesTiposAppService _fabPermissoesTiposAppService;

    public EditModalModel(
        IfabPermissoesTiposAppService fabPermissoesTiposAppService
    )
    {
        _fabPermissoesTiposAppService = fabPermissoesTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _fabPermissoesTiposAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<fabPermissoesTiposDto, EditfabPermissoesTiposViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfabPermissoesTiposViewModel, CreateUpdatefabPermissoesTiposDto>(ViewModel);
        await _fabPermissoesTiposAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
