using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.PageFeedbacks;
using Volo.CmsKit.Admin.Polls;
using Volo.CmsKit.PageFeedbacks;
using Volo.CmsKit.Polls;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.PageFeedbacks;

public class EditModalModel : AdminPageModel
{
    protected IPageFeedbackAdminAppService PageFeedbackAdminAppService { get; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public PageFeedbackEditViewModel ViewModel { get; set; }

    public EditModalModel(IPageFeedbackAdminAppService pageFeedbackAdminAppService)
    {
        PageFeedbackAdminAppService = pageFeedbackAdminAppService;
    }

    public async Task OnGetAsync()
    {
        var dto = await PageFeedbackAdminAppService.GetAsync(Id);
        
        ViewModel = ObjectMapper.Map<PageFeedbackDto, PageFeedbackEditViewModel>(dto);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var pageFeedbackDto = ObjectMapper.Map<PageFeedbackEditViewModel, UpdatePageFeedbackDto>(ViewModel);
        await PageFeedbackAdminAppService.UpdateAsync(Id, pageFeedbackDto);
        return NoContent();
    }

    public class PageFeedbackEditViewModel
    {
        public string EntityType { get; set; }
        
        public string EntityId { get; set; }
        
        public string Url { get; set; }
        
        public bool IsUseful { get; set; }
        
        [TextArea(Rows = 4)]
        public string UserNote { get; set; }
        
        public DateTime CreationTime { get; set; }
        
        public bool IsHandled { get; set; }
        
        [DynamicMaxLength(typeof(PageFeedbackConst), nameof(PageFeedbackConst.MaxAdminNoteLength))]
        [TextArea(Rows = 4)]
        public string AdminNote { get; set; }
    }
}
