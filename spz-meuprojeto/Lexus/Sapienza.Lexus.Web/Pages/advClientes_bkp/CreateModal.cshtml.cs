using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advClientes_bkp;
using Sapienza.Lexus.advClientes_bkp.Dtos;
using Sapienza.Lexus.Web.Pages.advClientes_bkp.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advClientes_bkp;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvClientes_bkpViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientes_bkpAppService _advClientes_bkpAppService;

    public CreateModalModel(
        IadvClientes_bkpAppService advClientes_bkpAppService
    )
    {
        _advClientes_bkpAppService = advClientes_bkpAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvClientes_bkpViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvClientes_bkpViewModel, CreateUpdateadvClientes_bkpDto>(ViewModel);
        await _advClientes_bkpAppService.CreateAsync(dto);
        return NoContent();
    }
}
