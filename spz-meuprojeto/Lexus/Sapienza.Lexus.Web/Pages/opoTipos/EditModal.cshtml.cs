using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.opoTipos;
using Sapienza.Lexus.opoTipos.Dtos;
using Sapienza.Lexus.Web.Pages.opoTipos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.opoTipos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditopoTiposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IopoTiposAppService _opoTiposAppService;

    public EditModalModel(
        IopoTiposAppService opoTiposAppService
    )
    {
        _opoTiposAppService = opoTiposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _opoTiposAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<opoTiposDto, EditopoTiposViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditopoTiposViewModel, CreateUpdateopoTiposDto>(ViewModel);
        await _opoTiposAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
