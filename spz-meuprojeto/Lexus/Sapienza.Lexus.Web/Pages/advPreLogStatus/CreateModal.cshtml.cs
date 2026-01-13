using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPreLogStatus;
using Sapienza.Lexus.advPreLogStatus.Dtos;
using Sapienza.Lexus.Web.Pages.advPreLogStatus.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPreLogStatus;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvPreLogStatusViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreLogStatusAppService _advPreLogStatusAppService;

    public CreateModalModel(
        IadvPreLogStatusAppService advPreLogStatusAppService
    )
    {
        _advPreLogStatusAppService = advPreLogStatusAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvPreLogStatusViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvPreLogStatusViewModel, CreateUpdateadvPreLogStatusDto>(ViewModel);
        await _advPreLogStatusAppService.CreateAsync(dto);
        return NoContent();
    }
}
