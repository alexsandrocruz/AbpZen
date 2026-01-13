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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfabFormasPagamentoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabFormasPagamentoAppService _fabFormasPagamentoAppService;

    public EditModalModel(
        IfabFormasPagamentoAppService fabFormasPagamentoAppService
    )
    {
        _fabFormasPagamentoAppService = fabFormasPagamentoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _fabFormasPagamentoAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<fabFormasPagamentoDto, EditfabFormasPagamentoViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfabFormasPagamentoViewModel, CreateUpdatefabFormasPagamentoDto>(ViewModel);
        await _fabFormasPagamentoAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
