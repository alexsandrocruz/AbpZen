using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabPaises;
using Sapienza.Lexus.fabPaises.Dtos;
using Sapienza.Lexus.Web.Pages.fabPaises.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabPaises;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfabPaisesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabPaisesAppService _fabPaisesAppService;

    public EditModalModel(
        IfabPaisesAppService fabPaisesAppService
    )
    {
        _fabPaisesAppService = fabPaisesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _fabPaisesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<fabPaisesDto, EditfabPaisesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfabPaisesViewModel, CreateUpdatefabPaisesDto>(ViewModel);
        await _fabPaisesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
