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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvClientesINSSStatusViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesINSSStatusAppService _advClientesINSSStatusAppService;

    public EditModalModel(
        IadvClientesINSSStatusAppService advClientesINSSStatusAppService
    )
    {
        _advClientesINSSStatusAppService = advClientesINSSStatusAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advClientesINSSStatusAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advClientesINSSStatusDto, EditadvClientesINSSStatusViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvClientesINSSStatusViewModel, CreateUpdateadvClientesINSSStatusDto>(ViewModel);
        await _advClientesINSSStatusAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
