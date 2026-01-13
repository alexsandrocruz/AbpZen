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

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvAgeTiposTarefasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvAgeTiposTarefasAppService _advAgeTiposTarefasAppService;

    public CreateModalModel(
        IadvAgeTiposTarefasAppService advAgeTiposTarefasAppService
    )
    {
        _advAgeTiposTarefasAppService = advAgeTiposTarefasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvAgeTiposTarefasViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvAgeTiposTarefasViewModel, CreateUpdateadvAgeTiposTarefasDto>(ViewModel);
        await _advAgeTiposTarefasAppService.CreateAsync(dto);
        return NoContent();
    }
}
