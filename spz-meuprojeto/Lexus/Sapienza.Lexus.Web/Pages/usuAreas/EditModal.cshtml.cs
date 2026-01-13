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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditusuAreasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IusuAreasAppService _usuAreasAppService;

    public EditModalModel(
        IusuAreasAppService usuAreasAppService
    )
    {
        _usuAreasAppService = usuAreasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _usuAreasAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<usuAreasDto, EditusuAreasViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditusuAreasViewModel, CreateUpdateusuAreasDto>(ViewModel);
        await _usuAreasAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
