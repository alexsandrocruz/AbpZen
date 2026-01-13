using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProInstancias;
using Sapienza.Lexus.advProInstancias.Dtos;
using Sapienza.Lexus.Web.Pages.advProInstancias.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProInstancias;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProInstanciasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProInstanciasAppService _advProInstanciasAppService;

    public CreateModalModel(
        IadvProInstanciasAppService advProInstanciasAppService
    )
    {
        _advProInstanciasAppService = advProInstanciasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProInstanciasViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProInstanciasViewModel, CreateUpdateadvProInstanciasDto>(ViewModel);
        await _advProInstanciasAppService.CreateAsync(dto);
        return NoContent();
    }
}
