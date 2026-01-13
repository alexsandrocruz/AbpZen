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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvCliTiposHistoricosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliTiposHistoricosAppService _advCliTiposHistoricosAppService;

    public EditModalModel(
        IadvCliTiposHistoricosAppService advCliTiposHistoricosAppService
    )
    {
        _advCliTiposHistoricosAppService = advCliTiposHistoricosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advCliTiposHistoricosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advCliTiposHistoricosDto, EditadvCliTiposHistoricosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvCliTiposHistoricosViewModel, CreateUpdateadvCliTiposHistoricosDto>(ViewModel);
        await _advCliTiposHistoricosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
