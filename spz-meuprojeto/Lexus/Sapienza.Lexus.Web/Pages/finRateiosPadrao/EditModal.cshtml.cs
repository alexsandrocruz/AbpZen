using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finRateiosPadrao;
using Sapienza.Lexus.finRateiosPadrao.Dtos;
using Sapienza.Lexus.Web.Pages.finRateiosPadrao.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finRateiosPadrao;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinRateiosPadraoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinRateiosPadraoAppService _finRateiosPadraoAppService;

    public EditModalModel(
        IfinRateiosPadraoAppService finRateiosPadraoAppService
    )
    {
        _finRateiosPadraoAppService = finRateiosPadraoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finRateiosPadraoAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finRateiosPadraoDto, EditfinRateiosPadraoViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinRateiosPadraoViewModel, CreateUpdatefinRateiosPadraoDto>(ViewModel);
        await _finRateiosPadraoAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
