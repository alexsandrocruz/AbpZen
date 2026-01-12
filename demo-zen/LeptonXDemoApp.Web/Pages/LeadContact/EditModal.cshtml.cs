using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.LeadContact;
using LeptonXDemoApp.LeadContact.Dtos;
using LeptonXDemoApp.Web.Pages.LeadContact.ViewModels;
using LeptonXDemoApp.Lead;
using LeptonXDemoApp.Lead.Dtos;

namespace LeptonXDemoApp.Web.Pages.LeadContact;

public class EditModalModel : LeptonXDemoAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditLeadContactViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========
    public List<SelectListItem> LeadList { get; set; } = new();

    private readonly ILeadContactAppService _leadContactAppService;
    private readonly ILeadAppService _leadAppService;

    public EditModalModel(
        ILeadContactAppService leadContactAppService,
        ILeadAppService leadAppService
    )
    {
        _leadContactAppService = leadContactAppService;
        _leadAppService = leadAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _leadContactAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<LeadContactDto, EditLeadContactViewModel>(dto);

        // Load lookup data for FK dropdowns
        var leadList = await _leadAppService.GetListAsync(new LeadGetListInput { MaxResultCount = 1000 });
        LeadList = leadList.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
        ViewModel.LeadList = LeadList;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditLeadContactViewModel, CreateUpdateLeadContactDto>(ViewModel);
        await _leadContactAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
