using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advCliPrioridades;
using Sapienza.Lexus.advCliPrioridades.Dtos;
using Sapienza.Lexus.Web.Pages.advCliPrioridades.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advCliPrioridades;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvCliPrioridadesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliPrioridadesAppService _advCliPrioridadesAppService;

    public CreateModalModel(
        IadvCliPrioridadesAppService advCliPrioridadesAppService
    )
    {
        _advCliPrioridadesAppService = advCliPrioridadesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvCliPrioridadesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvCliPrioridadesViewModel, CreateUpdateadvCliPrioridadesDto>(ViewModel);
        await _advCliPrioridadesAppService.CreateAsync(dto);
        return NoContent();
    }
}
