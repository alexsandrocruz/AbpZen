using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProTipos;
using Sapienza.Lexus.advProTipos.Dtos;
using Sapienza.Lexus.Web.Pages.advProTipos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProTipos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProTiposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProTiposAppService _advProTiposAppService;

    public EditModalModel(
        IadvProTiposAppService advProTiposAppService
    )
    {
        _advProTiposAppService = advProTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProTiposAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProTiposDto, EditadvProTiposViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProTiposViewModel, CreateUpdateadvProTiposDto>(ViewModel);
        await _advProTiposAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
