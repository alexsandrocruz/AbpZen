using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advClientesChecklist;
using Sapienza.Lexus.advClientesChecklist.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesChecklist.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advClientesChecklist;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvClientesChecklistViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesChecklistAppService _advClientesChecklistAppService;

    public CreateModalModel(
        IadvClientesChecklistAppService advClientesChecklistAppService
    )
    {
        _advClientesChecklistAppService = advClientesChecklistAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvClientesChecklistViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvClientesChecklistViewModel, CreateUpdateadvClientesChecklistDto>(ViewModel);
        await _advClientesChecklistAppService.CreateAsync(dto);
        return NoContent();
    }
}
