using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProcessosDadosHerdeiros;
using Sapienza.Lexus.advProcessosDadosHerdeiros.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessosDadosHerdeiros.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProcessosDadosHerdeiros;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProcessosDadosHerdeirosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProcessosDadosHerdeirosAppService _advProcessosDadosHerdeirosAppService;

    public CreateModalModel(
        IadvProcessosDadosHerdeirosAppService advProcessosDadosHerdeirosAppService
    )
    {
        _advProcessosDadosHerdeirosAppService = advProcessosDadosHerdeirosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProcessosDadosHerdeirosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProcessosDadosHerdeirosViewModel, CreateUpdateadvProcessosDadosHerdeirosDto>(ViewModel);
        await _advProcessosDadosHerdeirosAppService.CreateAsync(dto);
        return NoContent();
    }
}
