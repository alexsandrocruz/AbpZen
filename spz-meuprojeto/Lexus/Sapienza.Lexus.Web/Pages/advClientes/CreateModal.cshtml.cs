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

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvClientesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesAppService _advClientesAppService;

    public CreateModalModel(
        IadvClientesAppService advClientesAppService
    )
    {
        _advClientesAppService = advClientesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvClientesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvClientesViewModel, CreateUpdateadvClientesDto>(ViewModel);
        await _advClientesAppService.CreateAsync(dto);
        return NoContent();
    }
}
