using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advVerTipos;
using Sapienza.Lexus.advVerTipos.Dtos;
using Sapienza.Lexus.Web.Pages.advVerTipos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advVerTipos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvVerTiposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvVerTiposAppService _advVerTiposAppService;

    public EditModalModel(
        IadvVerTiposAppService advVerTiposAppService
    )
    {
        _advVerTiposAppService = advVerTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advVerTiposAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advVerTiposDto, EditadvVerTiposViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvVerTiposViewModel, CreateUpdateadvVerTiposDto>(ViewModel);
        await _advVerTiposAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
