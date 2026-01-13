using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProStatus;
using Sapienza.Lexus.advProStatus.Dtos;
using Sapienza.Lexus.Web.Pages.advProStatus.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProStatus;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProStatusViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProStatusAppService _advProStatusAppService;

    public CreateModalModel(
        IadvProStatusAppService advProStatusAppService
    )
    {
        _advProStatusAppService = advProStatusAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProStatusViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProStatusViewModel, CreateUpdateadvProStatusDto>(ViewModel);
        await _advProStatusAppService.CreateAsync(dto);
        return NoContent();
    }
}
