using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Faqs;
using Volo.CmsKit.Faqs;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Faqs;

public class EditQuestionModalModel : AdminPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty] 
    public FaqQuestionEditViewModel EditViewModel { get; set; }

    protected IFaqQuestionAdminAppService FaqQuestionAdminAppService { get; set; }

    public EditQuestionModalModel(IFaqQuestionAdminAppService faqQuestionAdminAppService)
    {
        FaqQuestionAdminAppService = faqQuestionAdminAppService;
    }

    public virtual async Task OnGet()
    {
        EditViewModel = ObjectMapper.Map<FaqQuestionDto, FaqQuestionEditViewModel>(await FaqQuestionAdminAppService.GetAsync(Id));
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();
        await FaqQuestionAdminAppService.UpdateAsync(Id, ObjectMapper.Map<FaqQuestionEditViewModel, UpdateFaqQuestionDto>(EditViewModel));
        
        return NoContent();
    }

    public class FaqQuestionEditViewModel
    {
        [Required]
        [DynamicMaxLength(typeof(FaqQuestionConst), nameof(FaqQuestionConst.MaxTitleLength))]
        public string Title { get; set; }

        [Required]
        [DynamicMaxLength(typeof(FaqQuestionConst), nameof(FaqQuestionConst.MaxTextLength))]
        [TextArea(Rows = 12)]
        public string Text { get; set; }

        [Required] 
        public int Order { get; set; }
    }
}