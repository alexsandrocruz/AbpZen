using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advCliLog;
using Sapienza.Lexus.advCliLog.Dtos;
using Sapienza.Lexus.Web.Pages.advCliLog.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advCliLog;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvCliLogViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliLogAppService _advCliLogAppService;

    public CreateModalModel(
        IadvCliLogAppService advCliLogAppService
    )
    {
        _advCliLogAppService = advCliLogAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvCliLogViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvCliLogViewModel, CreateUpdateadvCliLogDto>(ViewModel);
        await _advCliLogAppService.CreateAsync(dto);
        return NoContent();
    }
}
