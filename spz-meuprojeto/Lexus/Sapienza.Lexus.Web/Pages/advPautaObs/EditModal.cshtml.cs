using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPautaObs;
using Sapienza.Lexus.advPautaObs.Dtos;
using Sapienza.Lexus.Web.Pages.advPautaObs.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPautaObs;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvPautaObsViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPautaObsAppService _advPautaObsAppService;

    public EditModalModel(
        IadvPautaObsAppService advPautaObsAppService
    )
    {
        _advPautaObsAppService = advPautaObsAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advPautaObsAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advPautaObsDto, EditadvPautaObsViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvPautaObsViewModel, CreateUpdateadvPautaObsDto>(ViewModel);
        await _advPautaObsAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
