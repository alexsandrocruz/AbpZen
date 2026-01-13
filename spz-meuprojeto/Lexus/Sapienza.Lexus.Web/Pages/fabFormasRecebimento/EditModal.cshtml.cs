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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfabFormasRecebimentoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabFormasRecebimentoAppService _fabFormasRecebimentoAppService;

    public EditModalModel(
        IfabFormasRecebimentoAppService fabFormasRecebimentoAppService
    )
    {
        _fabFormasRecebimentoAppService = fabFormasRecebimentoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _fabFormasRecebimentoAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<fabFormasRecebimentoDto, EditfabFormasRecebimentoViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfabFormasRecebimentoViewModel, CreateUpdatefabFormasRecebimentoDto>(ViewModel);
        await _fabFormasRecebimentoAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
