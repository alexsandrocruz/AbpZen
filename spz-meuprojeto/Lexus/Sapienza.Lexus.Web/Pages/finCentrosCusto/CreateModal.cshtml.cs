using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finCentrosCusto;
using Sapienza.Lexus.finCentrosCusto.Dtos;
using Sapienza.Lexus.Web.Pages.finCentrosCusto.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finCentrosCusto;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinCentrosCustoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinCentrosCustoAppService _finCentrosCustoAppService;

    public CreateModalModel(
        IfinCentrosCustoAppService finCentrosCustoAppService
    )
    {
        _finCentrosCustoAppService = finCentrosCustoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinCentrosCustoViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinCentrosCustoViewModel, CreateUpdatefinCentrosCustoDto>(ViewModel);
        await _finCentrosCustoAppService.CreateAsync(dto);
        return NoContent();
    }
}
