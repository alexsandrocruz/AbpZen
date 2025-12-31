using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.PageFeedbacks;

public class PageFeedbackViewDto
{
    public string EntityType { get; set; }
    public string EntityId { get; set; }
    public string YesButtonText { get; set; }
    public string NoButtonText { get; set; }
    public string UserNotePlaceholder { get; set; }
    public string SubmitButtonText { get; set; }
    public bool ReverseButtons { get; set; }
    public string ThankYouMessageDescription { get; set; }
    public string ThankYouMessageTitle { get; set; }
    public bool HeaderVisible { get; set; } = true;
    public string HeaderText { get; set; }
    public string VeryHelpfulText { get; set; }
    public string NeedsImprovementText { get; set; }
    
    // Data
    public List<SelectListItem> EntityTypes { get; set; }
}