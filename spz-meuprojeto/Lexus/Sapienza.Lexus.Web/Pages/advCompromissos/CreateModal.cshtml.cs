using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advCompromissos;
using Sapienza.Lexus.advCompromissos.Dtos;
using Sapienza.Lexus.Web.Pages.advCompromissos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advCompromissos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvCompromissosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCompromissosAppService _advCompromissosAppService;

    public CreateModalModel(
        IadvCompromissosAppService advCompromissosAppService
    )
    {
        _advCompromissosAppService = advCompromissosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvCompromissosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvCompromissosViewModel, CreateUpdateadvCompromissosDto>(ViewModel);
        await _advCompromissosAppService.CreateAsync(dto);
        return NoContent();
    }
}
