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

public class CreateModalModel : LeptonXDemoAppPageModel
{
    [BindProperty]
    public CreateMessageTemplateViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IMessageTemplateAppService _messageTemplateAppService;

    public CreateModalModel(
        IMessageTemplateAppService messageTemplateAppService
    )
    {
        _messageTemplateAppService = messageTemplateAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateMessageTemplateViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateMessageTemplateViewModel, CreateUpdateMessageTemplateDto>(ViewModel);
        await _messageTemplateAppService.CreateAsync(dto);
        return NoContent();
    }
}
