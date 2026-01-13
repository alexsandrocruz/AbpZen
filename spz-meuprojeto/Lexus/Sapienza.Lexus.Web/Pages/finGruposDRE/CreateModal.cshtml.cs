using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finGruposDRE;
using Sapienza.Lexus.finGruposDRE.Dtos;
using Sapienza.Lexus.Web.Pages.finGruposDRE.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finGruposDRE;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinGruposDREViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinGruposDREAppService _finGruposDREAppService;

    public CreateModalModel(
        IfinGruposDREAppService finGruposDREAppService
    )
    {
        _finGruposDREAppService = finGruposDREAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinGruposDREViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinGruposDREViewModel, CreateUpdatefinGruposDREDto>(ViewModel);
        await _finGruposDREAppService.CreateAsync(dto);
        return NoContent();
    }
}
