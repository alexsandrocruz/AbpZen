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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfabCondicoesPagamentoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabCondicoesPagamentoAppService _fabCondicoesPagamentoAppService;

    public EditModalModel(
        IfabCondicoesPagamentoAppService fabCondicoesPagamentoAppService
    )
    {
        _fabCondicoesPagamentoAppService = fabCondicoesPagamentoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _fabCondicoesPagamentoAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<fabCondicoesPagamentoDto, EditfabCondicoesPagamentoViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfabCondicoesPagamentoViewModel, CreateUpdatefabCondicoesPagamentoDto>(ViewModel);
        await _fabCondicoesPagamentoAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
