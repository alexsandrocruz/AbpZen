using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabFormasRecebimento;
using Sapienza.Lexus.fabFormasRecebimento.Dtos;
using Sapienza.Lexus.Web.Pages.fabFormasRecebimento.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabFormasRecebimento;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefabFormasRecebimentoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabFormasRecebimentoAppService _fabFormasRecebimentoAppService;

    public CreateModalModel(
        IfabFormasRecebimentoAppService fabFormasRecebimentoAppService
    )
    {
        _fabFormasRecebimentoAppService = fabFormasRecebimentoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefabFormasRecebimentoViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefabFormasRecebimentoViewModel, CreateUpdatefabFormasRecebimentoDto>(ViewModel);
        await _fabFormasRecebimentoAppService.CreateAsync(dto);
        return NoContent();
    }
}
