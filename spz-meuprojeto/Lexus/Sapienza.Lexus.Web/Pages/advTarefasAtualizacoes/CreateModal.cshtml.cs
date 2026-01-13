using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advTarefasAtualizacoes;
using Sapienza.Lexus.advTarefasAtualizacoes.Dtos;
using Sapienza.Lexus.Web.Pages.advTarefasAtualizacoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advTarefasAtualizacoes;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvTarefasAtualizacoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvTarefasAtualizacoesAppService _advTarefasAtualizacoesAppService;

    public CreateModalModel(
        IadvTarefasAtualizacoesAppService advTarefasAtualizacoesAppService
    )
    {
        _advTarefasAtualizacoesAppService = advTarefasAtualizacoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvTarefasAtualizacoesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvTarefasAtualizacoesViewModel, CreateUpdateadvTarefasAtualizacoesDto>(ViewModel);
        await _advTarefasAtualizacoesAppService.CreateAsync(dto);
        return NoContent();
    }
}
