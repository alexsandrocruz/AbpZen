using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.logAcoes;
using Sapienza.Lexus.logAcoes.Dtos;
using Sapienza.Lexus.Web.Pages.logAcoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.logAcoes;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatelogAcoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IlogAcoesAppService _logAcoesAppService;

    public CreateModalModel(
        IlogAcoesAppService logAcoesAppService
    )
    {
        _logAcoesAppService = logAcoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatelogAcoesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatelogAcoesViewModel, CreateUpdatelogAcoesDto>(ViewModel);
        await _logAcoesAppService.CreateAsync(dto);
        return NoContent();
    }
}
