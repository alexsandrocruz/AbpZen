using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProcessos;
using Sapienza.Lexus.advProcessos.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProcessos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProcessosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProcessosAppService _advProcessosAppService;

    public CreateModalModel(
        IadvProcessosAppService advProcessosAppService
    )
    {
        _advProcessosAppService = advProcessosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProcessosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProcessosViewModel, CreateUpdateadvProcessosDto>(ViewModel);
        await _advProcessosAppService.CreateAsync(dto);
        return NoContent();
    }
}
