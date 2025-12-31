using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.CmsKit.Pro.Web.Pages.Public.Shared.Components.PageFeedbacks;

[Widget(
    StyleFiles = new[] { "/Pages/Public/Shared/Components/PageFeedbacks/Default.css" },
    ScriptTypes = new[] { typeof(PageFeedbackWidgetScriptContributor) },
    RefreshUrl = "/CmsKitProPublicWidgets/PageFeedback",
    AutoInitialize = true
)]
[ViewComponent(Name = "CmsPageFeedback")]
public class PageFeedbackViewComponent : AbpViewComponent
{
   public virtual IViewComponentResult Invoke(string entityType, string entityId = null, string yesButtonText = null,
        string noButtonText = null, string userNotePlaceholder = null, string submitButtonText = null,
        bool reverseButtons = false, string thankYouMessageDescription = null, string thankYouMessageTitle = null,
        bool headerVisible = true, string headerText = null, string veryHelpfulText = null, string needsImprovementText = null)
    {
        var model = new PageFeedbackViewDto {
            EntityType = entityType, EntityId = entityId, YesButtonText = yesButtonText, NoButtonText = noButtonText,
            UserNotePlaceholder = userNotePlaceholder,
            SubmitButtonText = submitButtonText,
            ThankYouMessageDescription = thankYouMessageDescription,
            ThankYouMessageTitle = thankYouMessageTitle,
            HeaderText = headerText,
            HeaderVisible = headerVisible,
            ReverseButtons = reverseButtons,
            VeryHelpfulText = veryHelpfulText,
            NeedsImprovementText = needsImprovementText
        };

        return View("~/Pages/Public/Shared/Components/PageFeedbacks/Default.cshtml", model);
    }
}