using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProcessosMeritos;
using Sapienza.Lexus.advProcessosMeritos.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessosMeritos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProcessosMeritos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProcessosMeritosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProcessosMeritosAppService _advProcessosMeritosAppService;

    public EditModalModel(
        IadvProcessosMeritosAppService advProcessosMeritosAppService
    )
    {
        _advProcessosMeritosAppService = advProcessosMeritosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProcessosMeritosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProcessosMeritosDto, EditadvProcessosMeritosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProcessosMeritosViewModel, CreateUpdateadvProcessosMeritosDto>(ViewModel);
        await _advProcessosMeritosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
