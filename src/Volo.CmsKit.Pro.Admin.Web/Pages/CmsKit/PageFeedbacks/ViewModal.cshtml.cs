using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.CmsKit.Admin.PageFeedbacks;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.PageFeedbacks;

public class ViewModal : AdminPageModel
{
    protected IPageFeedbackAdminAppService PageFeedbackAdminAppService { get; }
    
    public PageFeedbackViewModel PageFeedback { get; set; }
    
    public ViewModal(IPageFeedbackAdminAppService pageFeedbackAdminAppService)
    {
        PageFeedbackAdminAppService = pageFeedbackAdminAppService;
    }

    
    public async Task OnGetAsync(Guid id)
    {
        var pageFeedback = await PageFeedbackAdminAppService.GetAsync(id);
        PageFeedback = ObjectMapper.Map<PageFeedbackDto, PageFeedbackViewModel>(pageFeedback);
    }
    
    public class PageFeedbackViewModel
    {
        public string EntityType { get; set; }
        
        public string EntityId { get; set; }
        
        public string Url { get; set; }
        public bool IsUseful { get; set; }
        
        [TextArea(Rows = 4)]
        public string UserNote { get; set; }
        
        public DateTime CreationTime { get; set; }
        
        public bool IsHandled { get; set; }
        
        [TextArea(Rows = 4)]
        public string AdminNote { get; set; }
    }
}