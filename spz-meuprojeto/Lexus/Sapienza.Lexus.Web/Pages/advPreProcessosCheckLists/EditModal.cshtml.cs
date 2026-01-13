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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvPreProcessosCheckListsViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreProcessosCheckListsAppService _advPreProcessosCheckListsAppService;

    public EditModalModel(
        IadvPreProcessosCheckListsAppService advPreProcessosCheckListsAppService
    )
    {
        _advPreProcessosCheckListsAppService = advPreProcessosCheckListsAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advPreProcessosCheckListsAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advPreProcessosCheckListsDto, EditadvPreProcessosCheckListsViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvPreProcessosCheckListsViewModel, CreateUpdateadvPreProcessosCheckListsDto>(ViewModel);
        await _advPreProcessosCheckListsAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
