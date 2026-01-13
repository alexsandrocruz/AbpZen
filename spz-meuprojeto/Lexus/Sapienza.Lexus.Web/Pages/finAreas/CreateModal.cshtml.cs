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

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinAreasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinAreasAppService _finAreasAppService;

    public CreateModalModel(
        IfinAreasAppService finAreasAppService
    )
    {
        _finAreasAppService = finAreasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinAreasViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinAreasViewModel, CreateUpdatefinAreasDto>(ViewModel);
        await _finAreasAppService.CreateAsync(dto);
        return NoContent();
    }
}
