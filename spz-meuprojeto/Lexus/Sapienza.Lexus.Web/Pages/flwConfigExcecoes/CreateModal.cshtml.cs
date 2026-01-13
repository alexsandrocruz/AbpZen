using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.flwConfigExcecoes;
using Sapienza.Lexus.flwConfigExcecoes.Dtos;
using Sapienza.Lexus.Web.Pages.flwConfigExcecoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.flwConfigExcecoes;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateflwConfigExcecoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IflwConfigExcecoesAppService _flwConfigExcecoesAppService;

    public CreateModalModel(
        IflwConfigExcecoesAppService flwConfigExcecoesAppService
    )
    {
        _flwConfigExcecoesAppService = flwConfigExcecoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateflwConfigExcecoesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateflwConfigExcecoesViewModel, CreateUpdateflwConfigExcecoesDto>(ViewModel);
        await _flwConfigExcecoesAppService.CreateAsync(dto);
        return NoContent();
    }
}
