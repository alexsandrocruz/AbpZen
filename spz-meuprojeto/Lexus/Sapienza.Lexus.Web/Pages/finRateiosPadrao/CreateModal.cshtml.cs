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

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinRateiosPadraoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinRateiosPadraoAppService _finRateiosPadraoAppService;

    public CreateModalModel(
        IfinRateiosPadraoAppService finRateiosPadraoAppService
    )
    {
        _finRateiosPadraoAppService = finRateiosPadraoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinRateiosPadraoViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinRateiosPadraoViewModel, CreateUpdatefinRateiosPadraoDto>(ViewModel);
        await _finRateiosPadraoAppService.CreateAsync(dto);
        return NoContent();
    }
}
