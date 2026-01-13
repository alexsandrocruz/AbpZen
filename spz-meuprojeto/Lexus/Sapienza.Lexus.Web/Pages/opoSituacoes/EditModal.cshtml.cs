using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.opoSituacoes;
using Sapienza.Lexus.opoSituacoes.Dtos;
using Sapienza.Lexus.Web.Pages.opoSituacoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.opoSituacoes;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditopoSituacoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IopoSituacoesAppService _opoSituacoesAppService;

    public EditModalModel(
        IopoSituacoesAppService opoSituacoesAppService
    )
    {
        _opoSituacoesAppService = opoSituacoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _opoSituacoesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<opoSituacoesDto, EditopoSituacoesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditopoSituacoesViewModel, CreateUpdateopoSituacoesDto>(ViewModel);
        await _opoSituacoesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
