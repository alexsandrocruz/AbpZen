using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPreProcessosCheckLists;
using Sapienza.Lexus.advPreProcessosCheckLists.Dtos;
using Sapienza.Lexus.Web.Pages.advPreProcessosCheckLists.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPreProcessosCheckLists;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvPreProcessosCheckListsViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreProcessosCheckListsAppService _advPreProcessosCheckListsAppService;

    public CreateModalModel(
        IadvPreProcessosCheckListsAppService advPreProcessosCheckListsAppService
    )
    {
        _advPreProcessosCheckListsAppService = advPreProcessosCheckListsAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvPreProcessosCheckListsViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvPreProcessosCheckListsViewModel, CreateUpdateadvPreProcessosCheckListsDto>(ViewModel);
        await _advPreProcessosCheckListsAppService.CreateAsync(dto);
        return NoContent();
    }
}
