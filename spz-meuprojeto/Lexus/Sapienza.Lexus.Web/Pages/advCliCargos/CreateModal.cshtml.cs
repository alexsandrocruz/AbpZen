using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advCliCargos;
using Sapienza.Lexus.advCliCargos.Dtos;
using Sapienza.Lexus.Web.Pages.advCliCargos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advCliCargos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvCliCargosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliCargosAppService _advCliCargosAppService;

    public CreateModalModel(
        IadvCliCargosAppService advCliCargosAppService
    )
    {
        _advCliCargosAppService = advCliCargosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvCliCargosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvCliCargosViewModel, CreateUpdateadvCliCargosDto>(ViewModel);
        await _advCliCargosAppService.CreateAsync(dto);
        return NoContent();
    }
}
