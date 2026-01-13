using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advClientesModelos;
using Sapienza.Lexus.advClientesModelos.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesModelos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advClientesModelos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvClientesModelosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesModelosAppService _advClientesModelosAppService;

    public CreateModalModel(
        IadvClientesModelosAppService advClientesModelosAppService
    )
    {
        _advClientesModelosAppService = advClientesModelosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvClientesModelosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvClientesModelosViewModel, CreateUpdateadvClientesModelosDto>(ViewModel);
        await _advClientesModelosAppService.CreateAsync(dto);
        return NoContent();
    }
}
