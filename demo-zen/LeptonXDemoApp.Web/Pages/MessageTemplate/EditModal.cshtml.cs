using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.MessageTemplate;
using LeptonXDemoApp.MessageTemplate.Dtos;
using LeptonXDemoApp.Web.Pages.MessageTemplate.ViewModels;

namespace LeptonXDemoApp.Web.Pages.MessageTemplate;

public class EditModalModel : LeptonXDemoAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditMessageTemplateViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IMessageTemplateAppService _messageTemplateAppService;

    public EditModalModel(
        IMessageTemplateAppService messageTemplateAppService
    )
    {
        _messageTemplateAppService = messageTemplateAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _messageTemplateAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<MessageTemplateDto, EditMessageTemplateViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditMessageTemplateViewModel, CreateUpdateMessageTemplateDto>(ViewModel);
        await _messageTemplateAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
