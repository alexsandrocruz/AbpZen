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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProcessosHonorariosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProcessosHonorariosAppService _advProcessosHonorariosAppService;

    public EditModalModel(
        IadvProcessosHonorariosAppService advProcessosHonorariosAppService
    )
    {
        _advProcessosHonorariosAppService = advProcessosHonorariosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProcessosHonorariosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProcessosHonorariosDto, EditadvProcessosHonorariosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProcessosHonorariosViewModel, CreateUpdateadvProcessosHonorariosDto>(ViewModel);
        await _advProcessosHonorariosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
