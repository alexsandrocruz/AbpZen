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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvCliPrioridadesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliPrioridadesAppService _advCliPrioridadesAppService;

    public EditModalModel(
        IadvCliPrioridadesAppService advCliPrioridadesAppService
    )
    {
        _advCliPrioridadesAppService = advCliPrioridadesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advCliPrioridadesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advCliPrioridadesDto, EditadvCliPrioridadesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvCliPrioridadesViewModel, CreateUpdateadvCliPrioridadesDto>(ViewModel);
        await _advCliPrioridadesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
