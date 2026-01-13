using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProMeritos;
using Sapienza.Lexus.advProMeritos.Dtos;
using Sapienza.Lexus.Web.Pages.advProMeritos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProMeritos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProMeritosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProMeritosAppService _advProMeritosAppService;

    public EditModalModel(
        IadvProMeritosAppService advProMeritosAppService
    )
    {
        _advProMeritosAppService = advProMeritosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProMeritosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProMeritosDto, EditadvProMeritosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProMeritosViewModel, CreateUpdateadvProMeritosDto>(ViewModel);
        await _advProMeritosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
