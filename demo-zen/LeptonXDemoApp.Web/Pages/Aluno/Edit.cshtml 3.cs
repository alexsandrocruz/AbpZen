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

public class EditModel : LeptonXDemoAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditAlunoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IAlunoAppService _alunoAppService;

    public EditModel(
        IAlunoAppService alunoAppService
    )
    {
        _alunoAppService = alunoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _alunoAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<AlunoDto, EditAlunoViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditAlunoViewModel, CreateUpdateAlunoDto>(ViewModel);
        await _alunoAppService.UpdateAsync(Id, dto);
        return RedirectToPage("Index");
    }
}
