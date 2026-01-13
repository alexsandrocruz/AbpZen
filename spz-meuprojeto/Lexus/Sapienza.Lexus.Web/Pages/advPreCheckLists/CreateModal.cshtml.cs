using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPreCheckLists;
using Sapienza.Lexus.advPreCheckLists.Dtos;
using Sapienza.Lexus.Web.Pages.advPreCheckLists.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPreCheckLists;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvPreCheckListsViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreCheckListsAppService _advPreCheckListsAppService;

    public CreateModalModel(
        IadvPreCheckListsAppService advPreCheckListsAppService
    )
    {
        _advPreCheckListsAppService = advPreCheckListsAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvPreCheckListsViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvPreCheckListsViewModel, CreateUpdateadvPreCheckListsDto>(ViewModel);
        await _advPreCheckListsAppService.CreateAsync(dto);
        return NoContent();
    }
}
