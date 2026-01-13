using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProcessosClientes;
using Sapienza.Lexus.advProcessosClientes.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessosClientes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProcessosClientes;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProcessosClientesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProcessosClientesAppService _advProcessosClientesAppService;

    public CreateModalModel(
        IadvProcessosClientesAppService advProcessosClientesAppService
    )
    {
        _advProcessosClientesAppService = advProcessosClientesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProcessosClientesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProcessosClientesViewModel, CreateUpdateadvProcessosClientesDto>(ViewModel);
        await _advProcessosClientesAppService.CreateAsync(dto);
        return NoContent();
    }
}
