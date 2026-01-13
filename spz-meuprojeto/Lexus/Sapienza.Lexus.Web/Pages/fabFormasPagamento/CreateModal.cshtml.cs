using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabFormasPagamento;
using Sapienza.Lexus.fabFormasPagamento.Dtos;
using Sapienza.Lexus.Web.Pages.fabFormasPagamento.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabFormasPagamento;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefabFormasPagamentoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabFormasPagamentoAppService _fabFormasPagamentoAppService;

    public CreateModalModel(
        IfabFormasPagamentoAppService fabFormasPagamentoAppService
    )
    {
        _fabFormasPagamentoAppService = fabFormasPagamentoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefabFormasPagamentoViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefabFormasPagamentoViewModel, CreateUpdatefabFormasPagamentoDto>(ViewModel);
        await _fabFormasPagamentoAppService.CreateAsync(dto);
        return NoContent();
    }
}
