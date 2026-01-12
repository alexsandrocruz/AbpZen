using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace LeptonXDemoApp.Web.Pages.MessageTemplate.ViewModels;

public class CreateMessageTemplateViewModel
{
    [Display(Name = "MessageTemplate:Title")]
    public string? Title { get; set; }
    [Display(Name = "MessageTemplate:Body")]
    [TextArea(Rows = 3)]
    public string? Body { get; set; }
    [Display(Name = "MessageTemplate:MessageType")]
    public MessageTypeEnum? MessageType { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
