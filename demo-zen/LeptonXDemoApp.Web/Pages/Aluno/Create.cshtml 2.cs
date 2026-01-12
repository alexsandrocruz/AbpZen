using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.Aluno;
using LeptonXDemoApp.Aluno.Dtos;
using LeptonXDemoApp.Web.Pages.Aluno.ViewModels;

namespace LeptonXDemoApp.Web.Pages.Aluno;

public class CreateModel : LeptonXDemoAppPageModel
{
    [BindProperty]
    public CreateAlunoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IAlunoAppService _alunoAppService;

    public CreateModel(
        IAlunoAppService alunoAppService
    )
    {
        _alunoAppService = alunoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateAlunoViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateAlunoViewModel, CreateUpdateAlunoDto>(ViewModel);
        await _alunoAppService.CreateAsync(dto);
        return RedirectToPage("Index");
    }
}
