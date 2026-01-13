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

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateopoOportunidadesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IopoOportunidadesAppService _opoOportunidadesAppService;

    public CreateModalModel(
        IopoOportunidadesAppService opoOportunidadesAppService
    )
    {
        _opoOportunidadesAppService = opoOportunidadesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateopoOportunidadesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateopoOportunidadesViewModel, CreateUpdateopoOportunidadesDto>(ViewModel);
        await _opoOportunidadesAppService.CreateAsync(dto);
        return NoContent();
    }
}
