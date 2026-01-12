using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.AlunoTurma;
using LeptonXDemoApp.AlunoTurma.Dtos;
using LeptonXDemoApp.Web.Pages.AlunoTurma.ViewModels;
using LeptonXDemoApp.Aluno;
using LeptonXDemoApp.Aluno.Dtos;
using LeptonXDemoApp.Turma;
using LeptonXDemoApp.Turma.Dtos;

namespace LeptonXDemoApp.Web.Pages.AlunoTurma;

public class CreateModalModel : LeptonXDemoAppPageModel
{
    [BindProperty]
    public CreateAlunoTurmaViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========
    public List<SelectListItem> AlunoList { get; set; } = new();
    public List<SelectListItem> TurmaList { get; set; } = new();

    private readonly IAlunoTurmaAppService _alunoTurmaAppService;
    private readonly IAlunoAppService _alunoAppService;
    private readonly ITurmaAppService _turmaAppService;

    public CreateModalModel(
        IAlunoTurmaAppService alunoTurmaAppService,
        IAlunoAppService alunoAppService,
        ITurmaAppService turmaAppService
    )
    {
        _alunoTurmaAppService = alunoTurmaAppService;
        _alunoAppService = alunoAppService;
        _turmaAppService = turmaAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateAlunoTurmaViewModel();

        // Load lookup data for FK dropdowns
        var alunoList = await _alunoAppService.GetListAsync(new AlunoGetListInput { MaxResultCount = 1000 });
        AlunoList = alunoList.Items
            .Select(x => new SelectListItem(x.Nome, x.Id.ToString()))
            .ToList();
        ViewModel.AlunoList = AlunoList;
        var turmaList = await _turmaAppService.GetListAsync(new TurmaGetListInput { MaxResultCount = 1000 });
        TurmaList = turmaList.Items
            .Select(x => new SelectListItem(x.Nome, x.Id.ToString()))
            .ToList();
        ViewModel.TurmaList = TurmaList;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateAlunoTurmaViewModel, CreateUpdateAlunoTurmaDto>(ViewModel);
        await _alunoTurmaAppService.CreateAsync(dto);
        return NoContent();
    }
}
