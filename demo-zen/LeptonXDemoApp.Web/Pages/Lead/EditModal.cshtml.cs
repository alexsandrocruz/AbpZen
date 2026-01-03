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

public class EditModalModel : LeptonXDemoAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditLeadViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly ILeadAppService _leadAppService;

    public EditModalModel(
        ILeadAppService leadAppService
    )
    {
        _leadAppService = leadAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _leadAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<LeadDto, EditLeadViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditLeadViewModel, CreateUpdateLeadDto>(ViewModel);
        await _leadAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
