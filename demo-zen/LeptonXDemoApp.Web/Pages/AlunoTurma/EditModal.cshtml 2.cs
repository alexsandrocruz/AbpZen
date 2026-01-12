using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.AlunoTurma;
using LeptonXDemoApp.AlunoTurma.Dtos;
using LeptonXDemoApp.Web.Pages.AlunoTurma.ViewModels;

namespace LeptonXDemoApp.Web.Pages.AlunoTurma;

public class EditModalModel : LeptonXDemoAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditAlunoTurmaViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IAlunoTurmaAppService _alunoTurmaAppService;

    public EditModalModel(
        IAlunoTurmaAppService alunoTurmaAppService
    )
    {
        _alunoTurmaAppService = alunoTurmaAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _alunoTurmaAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<AlunoTurmaDto, EditAlunoTurmaViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditAlunoTurmaViewModel, CreateUpdateAlunoTurmaDto>(ViewModel);
        await _alunoTurmaAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
