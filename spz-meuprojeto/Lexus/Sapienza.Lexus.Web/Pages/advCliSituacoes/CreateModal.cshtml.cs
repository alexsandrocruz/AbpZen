using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advCliSituacoes;
using Sapienza.Lexus.advCliSituacoes.Dtos;
using Sapienza.Lexus.Web.Pages.advCliSituacoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advCliSituacoes;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvCliSituacoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliSituacoesAppService _advCliSituacoesAppService;

    public CreateModalModel(
        IadvCliSituacoesAppService advCliSituacoesAppService
    )
    {
        _advCliSituacoesAppService = advCliSituacoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvCliSituacoesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvCliSituacoesViewModel, CreateUpdateadvCliSituacoesDto>(ViewModel);
        await _advCliSituacoesAppService.CreateAsync(dto);
        return NoContent();
    }
}
