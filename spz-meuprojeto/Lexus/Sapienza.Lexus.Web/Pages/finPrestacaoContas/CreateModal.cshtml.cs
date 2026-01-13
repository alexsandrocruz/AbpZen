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

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinPrestacaoContasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinPrestacaoContasAppService _finPrestacaoContasAppService;

    public CreateModalModel(
        IfinPrestacaoContasAppService finPrestacaoContasAppService
    )
    {
        _finPrestacaoContasAppService = finPrestacaoContasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinPrestacaoContasViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinPrestacaoContasViewModel, CreateUpdatefinPrestacaoContasDto>(ViewModel);
        await _finPrestacaoContasAppService.CreateAsync(dto);
        return NoContent();
    }
}
