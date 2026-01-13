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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvTarefasAtualizacoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvTarefasAtualizacoesAppService _advTarefasAtualizacoesAppService;

    public EditModalModel(
        IadvTarefasAtualizacoesAppService advTarefasAtualizacoesAppService
    )
    {
        _advTarefasAtualizacoesAppService = advTarefasAtualizacoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advTarefasAtualizacoesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advTarefasAtualizacoesDto, EditadvTarefasAtualizacoesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvTarefasAtualizacoesViewModel, CreateUpdateadvTarefasAtualizacoesDto>(ViewModel);
        await _advTarefasAtualizacoesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
