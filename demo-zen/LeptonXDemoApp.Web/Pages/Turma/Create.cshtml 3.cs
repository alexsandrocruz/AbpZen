using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.Turma;
using LeptonXDemoApp.Turma.Dtos;
using LeptonXDemoApp.Web.Pages.Turma.ViewModels;

namespace LeptonXDemoApp.Web.Pages.Turma;

public class CreateModel : LeptonXDemoAppPageModel
{
    [BindProperty]
    public CreateTurmaViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly ITurmaAppService _turmaAppService;

    public CreateModel(
        ITurmaAppService turmaAppService
    )
    {
        _turmaAppService = turmaAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateTurmaViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateTurmaViewModel, CreateUpdateTurmaDto>(ViewModel);
        await _turmaAppService.CreateAsync(dto);
        return RedirectToPage("Index");
    }
}
