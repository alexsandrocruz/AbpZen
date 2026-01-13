using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProcessosAlteracoes;
using Sapienza.Lexus.advProcessosAlteracoes.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessosAlteracoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProcessosAlteracoes;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProcessosAlteracoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProcessosAlteracoesAppService _advProcessosAlteracoesAppService;

    public EditModalModel(
        IadvProcessosAlteracoesAppService advProcessosAlteracoesAppService
    )
    {
        _advProcessosAlteracoesAppService = advProcessosAlteracoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProcessosAlteracoesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProcessosAlteracoesDto, EditadvProcessosAlteracoesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProcessosAlteracoesViewModel, CreateUpdateadvProcessosAlteracoesDto>(ViewModel);
        await _advProcessosAlteracoesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
