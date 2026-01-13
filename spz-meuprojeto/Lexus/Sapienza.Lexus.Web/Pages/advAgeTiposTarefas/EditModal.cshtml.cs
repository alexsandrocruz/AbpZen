using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advAgeTiposTarefas;
using Sapienza.Lexus.advAgeTiposTarefas.Dtos;
using Sapienza.Lexus.Web.Pages.advAgeTiposTarefas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advAgeTiposTarefas;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvAgeTiposTarefasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvAgeTiposTarefasAppService _advAgeTiposTarefasAppService;

    public EditModalModel(
        IadvAgeTiposTarefasAppService advAgeTiposTarefasAppService
    )
    {
        _advAgeTiposTarefasAppService = advAgeTiposTarefasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advAgeTiposTarefasAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advAgeTiposTarefasDto, EditadvAgeTiposTarefasViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvAgeTiposTarefasViewModel, CreateUpdateadvAgeTiposTarefasDto>(ViewModel);
        await _advAgeTiposTarefasAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
