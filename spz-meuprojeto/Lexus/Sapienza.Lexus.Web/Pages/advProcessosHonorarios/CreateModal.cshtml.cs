using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProcessosHonorarios;
using Sapienza.Lexus.advProcessosHonorarios.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessosHonorarios.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProcessosHonorarios;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProcessosHonorariosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProcessosHonorariosAppService _advProcessosHonorariosAppService;

    public CreateModalModel(
        IadvProcessosHonorariosAppService advProcessosHonorariosAppService
    )
    {
        _advProcessosHonorariosAppService = advProcessosHonorariosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProcessosHonorariosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProcessosHonorariosViewModel, CreateUpdateadvProcessosHonorariosDto>(ViewModel);
        await _advProcessosHonorariosAppService.CreateAsync(dto);
        return NoContent();
    }
}
