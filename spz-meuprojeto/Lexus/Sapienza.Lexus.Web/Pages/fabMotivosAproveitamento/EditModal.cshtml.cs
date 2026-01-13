using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabMotivosAproveitamento;
using Sapienza.Lexus.fabMotivosAproveitamento.Dtos;
using Sapienza.Lexus.Web.Pages.fabMotivosAproveitamento.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabMotivosAproveitamento;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfabMotivosAproveitamentoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabMotivosAproveitamentoAppService _fabMotivosAproveitamentoAppService;

    public EditModalModel(
        IfabMotivosAproveitamentoAppService fabMotivosAproveitamentoAppService
    )
    {
        _fabMotivosAproveitamentoAppService = fabMotivosAproveitamentoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _fabMotivosAproveitamentoAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<fabMotivosAproveitamentoDto, EditfabMotivosAproveitamentoViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfabMotivosAproveitamentoViewModel, CreateUpdatefabMotivosAproveitamentoDto>(ViewModel);
        await _fabMotivosAproveitamentoAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
