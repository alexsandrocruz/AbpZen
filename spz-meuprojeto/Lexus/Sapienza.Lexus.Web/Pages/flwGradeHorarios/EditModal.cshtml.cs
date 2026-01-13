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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditflwGradeHorariosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IflwGradeHorariosAppService _flwGradeHorariosAppService;

    public EditModalModel(
        IflwGradeHorariosAppService flwGradeHorariosAppService
    )
    {
        _flwGradeHorariosAppService = flwGradeHorariosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _flwGradeHorariosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<flwGradeHorariosDto, EditflwGradeHorariosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditflwGradeHorariosViewModel, CreateUpdateflwGradeHorariosDto>(ViewModel);
        await _flwGradeHorariosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
