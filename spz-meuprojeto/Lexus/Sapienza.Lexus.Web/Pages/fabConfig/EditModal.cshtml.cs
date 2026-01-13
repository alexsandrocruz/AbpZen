using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabConfig;
using Sapienza.Lexus.fabConfig.Dtos;
using Sapienza.Lexus.Web.Pages.fabConfig.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabConfig;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfabConfigViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabConfigAppService _fabConfigAppService;

    public EditModalModel(
        IfabConfigAppService fabConfigAppService
    )
    {
        _fabConfigAppService = fabConfigAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _fabConfigAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<fabConfigDto, EditfabConfigViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfabConfigViewModel, CreateUpdatefabConfigDto>(ViewModel);
        await _fabConfigAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
