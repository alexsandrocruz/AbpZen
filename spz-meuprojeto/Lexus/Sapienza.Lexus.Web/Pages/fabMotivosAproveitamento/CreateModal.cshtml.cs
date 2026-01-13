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

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefabMotivosAproveitamentoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabMotivosAproveitamentoAppService _fabMotivosAproveitamentoAppService;

    public CreateModalModel(
        IfabMotivosAproveitamentoAppService fabMotivosAproveitamentoAppService
    )
    {
        _fabMotivosAproveitamentoAppService = fabMotivosAproveitamentoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefabMotivosAproveitamentoViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefabMotivosAproveitamentoViewModel, CreateUpdatefabMotivosAproveitamentoDto>(ViewModel);
        await _fabMotivosAproveitamentoAppService.CreateAsync(dto);
        return NoContent();
    }
}
