using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advClientesAtualizacoes;
using Sapienza.Lexus.advClientesAtualizacoes.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesAtualizacoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advClientesAtualizacoes;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvClientesAtualizacoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesAtualizacoesAppService _advClientesAtualizacoesAppService;

    public EditModalModel(
        IadvClientesAtualizacoesAppService advClientesAtualizacoesAppService
    )
    {
        _advClientesAtualizacoesAppService = advClientesAtualizacoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advClientesAtualizacoesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advClientesAtualizacoesDto, EditadvClientesAtualizacoesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvClientesAtualizacoesViewModel, CreateUpdateadvClientesAtualizacoesDto>(ViewModel);
        await _advClientesAtualizacoesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
