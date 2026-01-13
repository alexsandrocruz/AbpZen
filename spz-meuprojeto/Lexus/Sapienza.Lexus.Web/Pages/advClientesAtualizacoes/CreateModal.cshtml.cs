using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advClientesAtualizacoes;
using Sapienza.Lexus.advClientesAtualizacoes.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesAtualizacoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advClientesAtualizacoes;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvClientesAtualizacoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesAtualizacoesAppService _advClientesAtualizacoesAppService;

    public CreateModalModel(
        IadvClientesAtualizacoesAppService advClientesAtualizacoesAppService
    )
    {
        _advClientesAtualizacoesAppService = advClientesAtualizacoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvClientesAtualizacoesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvClientesAtualizacoesViewModel, CreateUpdateadvClientesAtualizacoesDto>(ViewModel);
        await _advClientesAtualizacoesAppService.CreateAsync(dto);
        return NoContent();
    }
}
