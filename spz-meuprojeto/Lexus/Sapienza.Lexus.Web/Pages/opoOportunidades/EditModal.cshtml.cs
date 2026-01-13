using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.opoOportunidades;
using Sapienza.Lexus.opoOportunidades.Dtos;
using Sapienza.Lexus.Web.Pages.opoOportunidades.ViewModels;

namespace Sapienza.Lexus.Web.Pages.opoOportunidades;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditopoOportunidadesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IopoOportunidadesAppService _opoOportunidadesAppService;

    public EditModalModel(
        IopoOportunidadesAppService opoOportunidadesAppService
    )
    {
        _opoOportunidadesAppService = opoOportunidadesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _opoOportunidadesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<opoOportunidadesDto, EditopoOportunidadesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditopoOportunidadesViewModel, CreateUpdateopoOportunidadesDto>(ViewModel);
        await _opoOportunidadesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
