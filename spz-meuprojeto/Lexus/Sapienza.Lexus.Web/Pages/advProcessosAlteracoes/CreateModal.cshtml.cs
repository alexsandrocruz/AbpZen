using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProcessosAlteracoes;
using Sapienza.Lexus.advProcessosAlteracoes.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessosAlteracoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProcessosAlteracoes;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProcessosAlteracoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProcessosAlteracoesAppService _advProcessosAlteracoesAppService;

    public CreateModalModel(
        IadvProcessosAlteracoesAppService advProcessosAlteracoesAppService
    )
    {
        _advProcessosAlteracoesAppService = advProcessosAlteracoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProcessosAlteracoesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProcessosAlteracoesViewModel, CreateUpdateadvProcessosAlteracoesDto>(ViewModel);
        await _advProcessosAlteracoesAppService.CreateAsync(dto);
        return NoContent();
    }
}
