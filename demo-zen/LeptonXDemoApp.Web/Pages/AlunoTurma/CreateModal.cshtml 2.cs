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

public class CreateModalModel : LeptonXDemoAppPageModel
{
    [BindProperty]
    public CreateAlunoTurmaViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IAlunoTurmaAppService _alunoTurmaAppService;

    public CreateModalModel(
        IAlunoTurmaAppService alunoTurmaAppService
    )
    {
        _alunoTurmaAppService = alunoTurmaAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateAlunoTurmaViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateAlunoTurmaViewModel, CreateUpdateAlunoTurmaDto>(ViewModel);
        await _alunoTurmaAppService.CreateAsync(dto);
        return NoContent();
    }
}
