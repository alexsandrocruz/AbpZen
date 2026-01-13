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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvTarefasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvTarefasAppService _advTarefasAppService;

    public EditModalModel(
        IadvTarefasAppService advTarefasAppService
    )
    {
        _advTarefasAppService = advTarefasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advTarefasAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advTarefasDto, EditadvTarefasViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvTarefasViewModel, CreateUpdateadvTarefasDto>(ViewModel);
        await _advTarefasAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
