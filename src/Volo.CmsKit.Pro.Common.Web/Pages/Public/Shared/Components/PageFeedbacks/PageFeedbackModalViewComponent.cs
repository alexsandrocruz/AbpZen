using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.CmsKit.Pro.Web.Pages.Public.Shared.Components.PageFeedbacks;

[Widget(
    ScriptFiles = new[] { "/Pages/Public/Shared/Components/PageFeedbacks/Modal.js" },
    RefreshUrl = "/CmsKitProPublicWidgets/PageFeedbackModal",
    AutoInitialize = true
)]
[ViewComponent(Name = "CmsPageFeedbackModal")]
public class PageFeedbackModalViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke(string entityType, string entityId = null, string yesButtonText = null,
        string noButtonText = null, string userNotePlaceholder = null, string submitButtonText = null,
        bool reverseButtons = false, string thankYouMessageDescription = null, string thankYouMessageTitle = null,
        bool headerVisible = true, string headerText = null, string modalId = "page-feedback-modal", string veryHelpfulText = null, string needsImprovementText = null)
    {
        var pageFeedbackViewDto = new PageFeedbackModalViewDto
        {
            EntityType = entityType, EntityId = entityId, YesButtonText = yesButtonText, NoButtonText = noButtonText,
            UserNotePlaceholder = userNotePlaceholder,
            SubmitButtonText = submitButtonText,
            ThankYouMessageDescription = thankYouMessageDescription,
            ThankYouMessageTitle = thankYouMessageTitle,
            HeaderText = headerText,
            ModalId = modalId,
            VeryHelpfulText = veryHelpfulText,
            NeedsImprovementText = needsImprovementText
        };
    
        return View("~/Pages/Public/Shared/Components/PageFeedbacks/Modal.cshtml", pageFeedbackViewDto);
    }
}