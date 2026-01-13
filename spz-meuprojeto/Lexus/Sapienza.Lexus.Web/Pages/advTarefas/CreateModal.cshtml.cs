using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advTarefas;
using Sapienza.Lexus.advTarefas.Dtos;
using Sapienza.Lexus.Web.Pages.advTarefas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advTarefas;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvTarefasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvTarefasAppService _advTarefasAppService;

    public CreateModalModel(
        IadvTarefasAppService advTarefasAppService
    )
    {
        _advTarefasAppService = advTarefasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvTarefasViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvTarefasViewModel, CreateUpdateadvTarefasDto>(ViewModel);
        await _advTarefasAppService.CreateAsync(dto);
        return NoContent();
    }
}
