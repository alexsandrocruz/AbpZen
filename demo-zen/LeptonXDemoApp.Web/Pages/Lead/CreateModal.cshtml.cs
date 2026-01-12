using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.Lead;
using LeptonXDemoApp.Lead.Dtos;
using LeptonXDemoApp.Web.Pages.Lead.ViewModels;

namespace LeptonXDemoApp.Web.Pages.Lead;

public class CreateModalModel : LeptonXDemoAppPageModel
{
    [BindProperty]
    public CreateLeadViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly ILeadAppService _leadAppService;

    public CreateModalModel(
        ILeadAppService leadAppService
    )
    {
        _leadAppService = leadAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateLeadViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateLeadViewModel, CreateUpdateLeadDto>(ViewModel);
        await _leadAppService.CreateAsync(dto);
        return NoContent();
    }
}
