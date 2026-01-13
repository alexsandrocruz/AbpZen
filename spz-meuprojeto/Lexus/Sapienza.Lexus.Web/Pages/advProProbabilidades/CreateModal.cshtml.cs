using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProProbabilidades;
using Sapienza.Lexus.advProProbabilidades.Dtos;
using Sapienza.Lexus.Web.Pages.advProProbabilidades.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProProbabilidades;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProProbabilidadesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProProbabilidadesAppService _advProProbabilidadesAppService;

    public CreateModalModel(
        IadvProProbabilidadesAppService advProProbabilidadesAppService
    )
    {
        _advProProbabilidadesAppService = advProProbabilidadesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProProbabilidadesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProProbabilidadesViewModel, CreateUpdateadvProProbabilidadesDto>(ViewModel);
        await _advProProbabilidadesAppService.CreateAsync(dto);
        return NoContent();
    }
}
