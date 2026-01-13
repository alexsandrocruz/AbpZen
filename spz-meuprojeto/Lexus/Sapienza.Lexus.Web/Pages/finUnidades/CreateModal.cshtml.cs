using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finUnidades;
using Sapienza.Lexus.finUnidades.Dtos;
using Sapienza.Lexus.Web.Pages.finUnidades.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finUnidades;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinUnidadesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinUnidadesAppService _finUnidadesAppService;

    public CreateModalModel(
        IfinUnidadesAppService finUnidadesAppService
    )
    {
        _finUnidadesAppService = finUnidadesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinUnidadesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinUnidadesViewModel, CreateUpdatefinUnidadesDto>(ViewModel);
        await _finUnidadesAppService.CreateAsync(dto);
        return NoContent();
    }
}
