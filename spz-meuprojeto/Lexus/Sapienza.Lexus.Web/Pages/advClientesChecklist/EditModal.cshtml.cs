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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvClientesChecklistViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesChecklistAppService _advClientesChecklistAppService;

    public EditModalModel(
        IadvClientesChecklistAppService advClientesChecklistAppService
    )
    {
        _advClientesChecklistAppService = advClientesChecklistAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advClientesChecklistAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advClientesChecklistDto, EditadvClientesChecklistViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvClientesChecklistViewModel, CreateUpdateadvClientesChecklistDto>(ViewModel);
        await _advClientesChecklistAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
