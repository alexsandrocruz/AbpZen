using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.flwAcoes;
using Sapienza.Lexus.flwAcoes.Dtos;
using Sapienza.Lexus.Web.Pages.flwAcoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.flwAcoes;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateflwAcoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IflwAcoesAppService _flwAcoesAppService;

    public CreateModalModel(
        IflwAcoesAppService flwAcoesAppService
    )
    {
        _flwAcoesAppService = flwAcoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateflwAcoesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateflwAcoesViewModel, CreateUpdateflwAcoesDto>(ViewModel);
        await _flwAcoesAppService.CreateAsync(dto);
        return NoContent();
    }
}
