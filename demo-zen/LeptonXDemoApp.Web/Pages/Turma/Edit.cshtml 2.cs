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

public class EditModel : LeptonXDemoAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditTurmaViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly ITurmaAppService _turmaAppService;

    public EditModel(
        ITurmaAppService turmaAppService
    )
    {
        _turmaAppService = turmaAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _turmaAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<TurmaDto, EditTurmaViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditTurmaViewModel, CreateUpdateTurmaDto>(ViewModel);
        await _turmaAppService.UpdateAsync(Id, dto);
        return RedirectToPage("Index");
    }
}
