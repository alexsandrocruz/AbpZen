using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Pro.Web.Pages.Public.Shared.Components.Contact;
using Volo.CmsKit.Pro.Web.Pages.Public.Shared.Components.PageFeedbacks;
using Volo.CmsKit.Pro.Web.Pages.Public.Shared.Components.Poll;

namespace Volo.CmsKit.Pro.Public.Web.Controllers;

public class CmsKitProPublicWidgetsController : AbpController
{
    public Task<IActionResult> Contact(string contactName)
    {
        return Task.FromResult((IActionResult)ViewComponent(typeof(ContactViewComponent), new { contactName }));
    }

    public Task<IActionResult> Poll(string widgetName)
    {
        return Task.FromResult((IActionResult)ViewComponent(typeof(PollViewComponent), new { widgetName }));
    }

    public Task<IActionResult> PollByCode(string code)
    {
        return Task.FromResult((IActionResult)ViewComponent(typeof(PollByCodeViewComponent), new { code }));
    }
    
    public Task<IActionResult> PageFeedback(PageFeedbackViewDto pageFeedbackViewDto)
    {
        return Task.FromResult((IActionResult)ViewComponent(typeof(PageFeedbackViewComponent), pageFeedbackViewDto));
    }
    
    public Task<IActionResult> PageFeedbackModal(PageFeedbackModalViewDto pageFeedbackViewDto)
    {
        return Task.FromResult((IActionResult)ViewComponent(typeof(PageFeedbackModalViewComponent), pageFeedbackViewDto));
    }
}
