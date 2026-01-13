using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finLancamentos_BKP;
using Sapienza.Lexus.finLancamentos_BKP.Dtos;
using Sapienza.Lexus.Web.Pages.finLancamentos_BKP.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finLancamentos_BKP;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinLancamentos_BKPViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinLancamentos_BKPAppService _finLancamentos_BKPAppService;

    public CreateModalModel(
        IfinLancamentos_BKPAppService finLancamentos_BKPAppService
    )
    {
        _finLancamentos_BKPAppService = finLancamentos_BKPAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinLancamentos_BKPViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinLancamentos_BKPViewModel, CreateUpdatefinLancamentos_BKPDto>(ViewModel);
        await _finLancamentos_BKPAppService.CreateAsync(dto);
        return NoContent();
    }
}
