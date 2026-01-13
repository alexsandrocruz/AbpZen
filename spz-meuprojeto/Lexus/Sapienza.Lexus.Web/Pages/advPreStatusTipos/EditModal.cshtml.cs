using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPreStatusTipos;
using Sapienza.Lexus.advPreStatusTipos.Dtos;
using Sapienza.Lexus.Web.Pages.advPreStatusTipos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPreStatusTipos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvPreStatusTiposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreStatusTiposAppService _advPreStatusTiposAppService;

    public EditModalModel(
        IadvPreStatusTiposAppService advPreStatusTiposAppService
    )
    {
        _advPreStatusTiposAppService = advPreStatusTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advPreStatusTiposAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advPreStatusTiposDto, EditadvPreStatusTiposViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvPreStatusTiposViewModel, CreateUpdateadvPreStatusTiposDto>(ViewModel);
        await _advPreStatusTiposAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
