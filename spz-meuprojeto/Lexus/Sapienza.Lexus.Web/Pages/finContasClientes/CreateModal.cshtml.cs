using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finContasClientes;
using Sapienza.Lexus.finContasClientes.Dtos;
using Sapienza.Lexus.Web.Pages.finContasClientes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finContasClientes;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinContasClientesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinContasClientesAppService _finContasClientesAppService;

    public CreateModalModel(
        IfinContasClientesAppService finContasClientesAppService
    )
    {
        _finContasClientesAppService = finContasClientesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinContasClientesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinContasClientesViewModel, CreateUpdatefinContasClientesDto>(ViewModel);
        await _finContasClientesAppService.CreateAsync(dto);
        return NoContent();
    }
}
