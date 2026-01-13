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

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateflwFollowsViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IflwFollowsAppService _flwFollowsAppService;

    public CreateModalModel(
        IflwFollowsAppService flwFollowsAppService
    )
    {
        _flwFollowsAppService = flwFollowsAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateflwFollowsViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateflwFollowsViewModel, CreateUpdateflwFollowsDto>(ViewModel);
        await _flwFollowsAppService.CreateAsync(dto);
        return NoContent();
    }
}
