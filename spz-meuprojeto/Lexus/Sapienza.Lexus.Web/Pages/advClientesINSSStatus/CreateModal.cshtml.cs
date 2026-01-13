using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advClientesINSSStatus;
using Sapienza.Lexus.advClientesINSSStatus.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesINSSStatus.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advClientesINSSStatus;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvClientesINSSStatusViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesINSSStatusAppService _advClientesINSSStatusAppService;

    public CreateModalModel(
        IadvClientesINSSStatusAppService advClientesINSSStatusAppService
    )
    {
        _advClientesINSSStatusAppService = advClientesINSSStatusAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvClientesINSSStatusViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvClientesINSSStatusViewModel, CreateUpdateadvClientesINSSStatusDto>(ViewModel);
        await _advClientesINSSStatusAppService.CreateAsync(dto);
        return NoContent();
    }
}
