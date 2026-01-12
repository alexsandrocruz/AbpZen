using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LeptonXDemoApp.MessageTemplate;
using LeptonXDemoApp.MessageTemplate.Dtos;

namespace LeptonXDemoApp.Web.Pages.MessageTemplate;

public class IndexModel : LeptonXDemoAppPageModel
{
    public MessageTemplateFilterInput MessageTemplateFilter { get; set; }
    
    private readonly IMessageTemplateAppService _messageTemplateAppService;

    public IndexModel(IMessageTemplateAppService messageTemplateAppService)
    {
        _messageTemplateAppService = messageTemplateAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(MessageTemplateGetListInput input)
    {
        var result = await _messageTemplateAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _messageTemplateAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class MessageTemplateFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "MessageTemplate:Title")]
    public string? Title { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "MessageTemplate:Body")]
    public string? Body { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "MessageTemplate:MessageType")]
    public MessageTypeEnum? MessageType { get; set; }
}
