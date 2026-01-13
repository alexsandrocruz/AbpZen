using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.opoSituacoes;
using Sapienza.Lexus.opoSituacoes.Dtos;
using Sapienza.Lexus.Web.Pages.opoSituacoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.opoSituacoes;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateopoSituacoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IopoSituacoesAppService _opoSituacoesAppService;

    public CreateModalModel(
        IopoSituacoesAppService opoSituacoesAppService
    )
    {
        _opoSituacoesAppService = opoSituacoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateopoSituacoesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateopoSituacoesViewModel, CreateUpdateopoSituacoesDto>(ViewModel);
        await _opoSituacoesAppService.CreateAsync(dto);
        return NoContent();
    }
}
