using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPreStatus;
using Sapienza.Lexus.advPreStatus.Dtos;
using Sapienza.Lexus.Web.Pages.advPreStatus.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPreStatus;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvPreStatusViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreStatusAppService _advPreStatusAppService;

    public CreateModalModel(
        IadvPreStatusAppService advPreStatusAppService
    )
    {
        _advPreStatusAppService = advPreStatusAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvPreStatusViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvPreStatusViewModel, CreateUpdateadvPreStatusDto>(ViewModel);
        await _advPreStatusAppService.CreateAsync(dto);
        return NoContent();
    }
}
