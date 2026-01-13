using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPreCheckListsGrupos;
using Sapienza.Lexus.advPreCheckListsGrupos.Dtos;
using Sapienza.Lexus.Web.Pages.advPreCheckListsGrupos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPreCheckListsGrupos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvPreCheckListsGruposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreCheckListsGruposAppService _advPreCheckListsGruposAppService;

    public CreateModalModel(
        IadvPreCheckListsGruposAppService advPreCheckListsGruposAppService
    )
    {
        _advPreCheckListsGruposAppService = advPreCheckListsGruposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvPreCheckListsGruposViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvPreCheckListsGruposViewModel, CreateUpdateadvPreCheckListsGruposDto>(ViewModel);
        await _advPreCheckListsGruposAppService.CreateAsync(dto);
        return NoContent();
    }
}
