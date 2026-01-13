using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.usuAreas;
using Sapienza.Lexus.usuAreas.Dtos;
using Sapienza.Lexus.Web.Pages.usuAreas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.usuAreas;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateusuAreasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IusuAreasAppService _usuAreasAppService;

    public CreateModalModel(
        IusuAreasAppService usuAreasAppService
    )
    {
        _usuAreasAppService = usuAreasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateusuAreasViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateusuAreasViewModel, CreateUpdateusuAreasDto>(ViewModel);
        await _usuAreasAppService.CreateAsync(dto);
        return NoContent();
    }
}
