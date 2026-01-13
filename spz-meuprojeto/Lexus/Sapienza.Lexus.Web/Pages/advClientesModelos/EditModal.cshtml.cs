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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvClientesModelosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesModelosAppService _advClientesModelosAppService;

    public EditModalModel(
        IadvClientesModelosAppService advClientesModelosAppService
    )
    {
        _advClientesModelosAppService = advClientesModelosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advClientesModelosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advClientesModelosDto, EditadvClientesModelosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvClientesModelosViewModel, CreateUpdateadvClientesModelosDto>(ViewModel);
        await _advClientesModelosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
