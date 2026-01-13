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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvCliCargosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliCargosAppService _advCliCargosAppService;

    public EditModalModel(
        IadvCliCargosAppService advCliCargosAppService
    )
    {
        _advCliCargosAppService = advCliCargosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advCliCargosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advCliCargosDto, EditadvCliCargosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvCliCargosViewModel, CreateUpdateadvCliCargosDto>(ViewModel);
        await _advCliCargosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
