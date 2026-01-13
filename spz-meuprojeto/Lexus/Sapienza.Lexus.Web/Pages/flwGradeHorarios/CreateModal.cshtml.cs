using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.flwGradeHorarios;
using Sapienza.Lexus.flwGradeHorarios.Dtos;
using Sapienza.Lexus.Web.Pages.flwGradeHorarios.ViewModels;

namespace Sapienza.Lexus.Web.Pages.flwGradeHorarios;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateflwGradeHorariosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IflwGradeHorariosAppService _flwGradeHorariosAppService;

    public CreateModalModel(
        IflwGradeHorariosAppService flwGradeHorariosAppService
    )
    {
        _flwGradeHorariosAppService = flwGradeHorariosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateflwGradeHorariosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateflwGradeHorariosViewModel, CreateUpdateflwGradeHorariosDto>(ViewModel);
        await _flwGradeHorariosAppService.CreateAsync(dto);
        return NoContent();
    }
}
