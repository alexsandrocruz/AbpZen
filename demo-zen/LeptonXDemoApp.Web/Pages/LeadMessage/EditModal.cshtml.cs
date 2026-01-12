using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.LeadMessage;
using LeptonXDemoApp.LeadMessage.Dtos;
using LeptonXDemoApp.Web.Pages.LeadMessage.ViewModels;
using LeptonXDemoApp.MessageTemplate;
using LeptonXDemoApp.MessageTemplate.Dtos;
using LeptonXDemoApp.Lead;
using LeptonXDemoApp.Lead.Dtos;

namespace LeptonXDemoApp.Web.Pages.LeadMessage;

public class EditModalModel : LeptonXDemoAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditLeadMessageViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========
    public List<SelectListItem> MessageTemplateList { get; set; } = new();
    public List<SelectListItem> LeadList { get; set; } = new();

    private readonly ILeadMessageAppService _leadMessageAppService;
    private readonly IMessageTemplateAppService _messageTemplateAppService;
    private readonly ILeadAppService _leadAppService;

    public EditModalModel(
        ILeadMessageAppService leadMessageAppService,
        IMessageTemplateAppService messageTemplateAppService,
        ILeadAppService leadAppService
    )
    {
        _leadMessageAppService = leadMessageAppService;
        _messageTemplateAppService = messageTemplateAppService;
        _leadAppService = leadAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _leadMessageAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<LeadMessageDto, EditLeadMessageViewModel>(dto);

        // Load lookup data for FK dropdowns
        var messageTemplateList = await _messageTemplateAppService.GetListAsync(new MessageTemplateGetListInput { MaxResultCount = 1000 });
        MessageTemplateList = messageTemplateList.Items
            .Select(x => new SelectListItem(x.Title, x.Id.ToString()))
            .ToList();
        ViewModel.MessageTemplateList = MessageTemplateList;
        var leadList = await _leadAppService.GetListAsync(new LeadGetListInput { MaxResultCount = 1000 });
        LeadList = leadList.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
        ViewModel.LeadList = LeadList;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditLeadMessageViewModel, CreateUpdateLeadMessageDto>(ViewModel);
        await _leadMessageAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
