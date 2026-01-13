using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advCliSituacoes;
using Sapienza.Lexus.advCliSituacoes.Dtos;
using Sapienza.Lexus.Web.Pages.advCliSituacoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advCliSituacoes;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvCliSituacoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliSituacoesAppService _advCliSituacoesAppService;

    public EditModalModel(
        IadvCliSituacoesAppService advCliSituacoesAppService
    )
    {
        _advCliSituacoesAppService = advCliSituacoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advCliSituacoesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advCliSituacoesDto, EditadvCliSituacoesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvCliSituacoesViewModel, CreateUpdateadvCliSituacoesDto>(ViewModel);
        await _advCliSituacoesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
