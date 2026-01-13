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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvPreCheckListsViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreCheckListsAppService _advPreCheckListsAppService;

    public EditModalModel(
        IadvPreCheckListsAppService advPreCheckListsAppService
    )
    {
        _advPreCheckListsAppService = advPreCheckListsAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advPreCheckListsAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advPreCheckListsDto, EditadvPreCheckListsViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvPreCheckListsViewModel, CreateUpdateadvPreCheckListsDto>(ViewModel);
        await _advPreCheckListsAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
