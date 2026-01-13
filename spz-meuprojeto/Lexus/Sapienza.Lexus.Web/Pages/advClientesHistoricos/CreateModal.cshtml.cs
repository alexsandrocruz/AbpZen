using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advClientesHistoricos;
using Sapienza.Lexus.advClientesHistoricos.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesHistoricos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advClientesHistoricos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvClientesHistoricosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesHistoricosAppService _advClientesHistoricosAppService;

    public CreateModalModel(
        IadvClientesHistoricosAppService advClientesHistoricosAppService
    )
    {
        _advClientesHistoricosAppService = advClientesHistoricosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvClientesHistoricosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvClientesHistoricosViewModel, CreateUpdateadvClientesHistoricosDto>(ViewModel);
        await _advClientesHistoricosAppService.CreateAsync(dto);
        return NoContent();
    }
}
