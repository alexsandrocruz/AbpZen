using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProEscritorios;
using Sapienza.Lexus.advProEscritorios.Dtos;
using Sapienza.Lexus.Web.Pages.advProEscritorios.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProEscritorios;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProEscritoriosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProEscritoriosAppService _advProEscritoriosAppService;

    public EditModalModel(
        IadvProEscritoriosAppService advProEscritoriosAppService
    )
    {
        _advProEscritoriosAppService = advProEscritoriosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProEscritoriosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProEscritoriosDto, EditadvProEscritoriosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProEscritoriosViewModel, CreateUpdateadvProEscritoriosDto>(ViewModel);
        await _advProEscritoriosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
