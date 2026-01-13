using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advCliGrupos;
using Sapienza.Lexus.advCliGrupos.Dtos;
using Sapienza.Lexus.Web.Pages.advCliGrupos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advCliGrupos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvCliGruposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliGruposAppService _advCliGruposAppService;

    public CreateModalModel(
        IadvCliGruposAppService advCliGruposAppService
    )
    {
        _advCliGruposAppService = advCliGruposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvCliGruposViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvCliGruposViewModel, CreateUpdateadvCliGruposDto>(ViewModel);
        await _advCliGruposAppService.CreateAsync(dto);
        return NoContent();
    }
}
