using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabCondicoesPagamento;
using Sapienza.Lexus.fabCondicoesPagamento.Dtos;
using Sapienza.Lexus.Web.Pages.fabCondicoesPagamento.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabCondicoesPagamento;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefabCondicoesPagamentoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabCondicoesPagamentoAppService _fabCondicoesPagamentoAppService;

    public CreateModalModel(
        IfabCondicoesPagamentoAppService fabCondicoesPagamentoAppService
    )
    {
        _fabCondicoesPagamentoAppService = fabCondicoesPagamentoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefabCondicoesPagamentoViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefabCondicoesPagamentoViewModel, CreateUpdatefabCondicoesPagamentoDto>(ViewModel);
        await _fabCondicoesPagamentoAppService.CreateAsync(dto);
        return NoContent();
    }
}
