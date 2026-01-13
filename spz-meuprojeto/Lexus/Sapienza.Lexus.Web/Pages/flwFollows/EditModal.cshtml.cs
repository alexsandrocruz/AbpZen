using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.flwFollows;
using Sapienza.Lexus.flwFollows.Dtos;
using Sapienza.Lexus.Web.Pages.flwFollows.ViewModels;

namespace Sapienza.Lexus.Web.Pages.flwFollows;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditflwFollowsViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IflwFollowsAppService _flwFollowsAppService;

    public EditModalModel(
        IflwFollowsAppService flwFollowsAppService
    )
    {
        _flwFollowsAppService = flwFollowsAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _flwFollowsAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<flwFollowsDto, EditflwFollowsViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditflwFollowsViewModel, CreateUpdateflwFollowsDto>(ViewModel);
        await _flwFollowsAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
