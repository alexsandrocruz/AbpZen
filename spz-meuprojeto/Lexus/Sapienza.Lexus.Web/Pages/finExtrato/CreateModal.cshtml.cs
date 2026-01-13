using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finExtrato;
using Sapienza.Lexus.finExtrato.Dtos;
using Sapienza.Lexus.Web.Pages.finExtrato.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finExtrato;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinExtratoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinExtratoAppService _finExtratoAppService;

    public CreateModalModel(
        IfinExtratoAppService finExtratoAppService
    )
    {
        _finExtratoAppService = finExtratoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinExtratoViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinExtratoViewModel, CreateUpdatefinExtratoDto>(ViewModel);
        await _finExtratoAppService.CreateAsync(dto);
        return NoContent();
    }
}
