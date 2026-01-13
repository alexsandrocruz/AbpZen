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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvClientesHistoricosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesHistoricosAppService _advClientesHistoricosAppService;

    public EditModalModel(
        IadvClientesHistoricosAppService advClientesHistoricosAppService
    )
    {
        _advClientesHistoricosAppService = advClientesHistoricosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advClientesHistoricosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advClientesHistoricosDto, EditadvClientesHistoricosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvClientesHistoricosViewModel, CreateUpdateadvClientesHistoricosDto>(ViewModel);
        await _advClientesHistoricosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
