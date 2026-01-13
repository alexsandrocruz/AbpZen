using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advCliTiposHistoricos;
using Sapienza.Lexus.advCliTiposHistoricos.Dtos;
using Sapienza.Lexus.Web.Pages.advCliTiposHistoricos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advCliTiposHistoricos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvCliTiposHistoricosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliTiposHistoricosAppService _advCliTiposHistoricosAppService;

    public CreateModalModel(
        IadvCliTiposHistoricosAppService advCliTiposHistoricosAppService
    )
    {
        _advCliTiposHistoricosAppService = advCliTiposHistoricosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvCliTiposHistoricosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvCliTiposHistoricosViewModel, CreateUpdateadvCliTiposHistoricosDto>(ViewModel);
        await _advCliTiposHistoricosAppService.CreateAsync(dto);
        return NoContent();
    }
}
