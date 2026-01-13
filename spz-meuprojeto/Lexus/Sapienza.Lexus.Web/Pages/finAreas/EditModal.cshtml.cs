using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finAreas;
using Sapienza.Lexus.finAreas.Dtos;
using Sapienza.Lexus.Web.Pages.finAreas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finAreas;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinAreasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinAreasAppService _finAreasAppService;

    public EditModalModel(
        IfinAreasAppService finAreasAppService
    )
    {
        _finAreasAppService = finAreasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finAreasAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finAreasDto, EditfinAreasViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinAreasViewModel, CreateUpdatefinAreasDto>(ViewModel);
        await _finAreasAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
