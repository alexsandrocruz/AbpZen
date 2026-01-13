using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finPrestacaoContas;
using Sapienza.Lexus.finPrestacaoContas.Dtos;
using Sapienza.Lexus.Web.Pages.finPrestacaoContas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finPrestacaoContas;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinPrestacaoContasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinPrestacaoContasAppService _finPrestacaoContasAppService;

    public EditModalModel(
        IfinPrestacaoContasAppService finPrestacaoContasAppService
    )
    {
        _finPrestacaoContasAppService = finPrestacaoContasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finPrestacaoContasAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finPrestacaoContasDto, EditfinPrestacaoContasViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinPrestacaoContasViewModel, CreateUpdatefinPrestacaoContasDto>(ViewModel);
        await _finPrestacaoContasAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
