using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advClientes;
using Sapienza.Lexus.advClientes.Dtos;
using Sapienza.Lexus.Web.Pages.advClientes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advClientes;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvClientesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesAppService _advClientesAppService;

    public EditModalModel(
        IadvClientesAppService advClientesAppService
    )
    {
        _advClientesAppService = advClientesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advClientesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advClientesDto, EditadvClientesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvClientesViewModel, CreateUpdateadvClientesDto>(ViewModel);
        await _advClientesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
