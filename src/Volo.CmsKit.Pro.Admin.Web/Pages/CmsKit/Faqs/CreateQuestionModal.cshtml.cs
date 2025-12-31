using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Faqs;
using Volo.CmsKit.Faqs;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Faqs;

public class CreateQuestionModalModel : AdminPageModel
{
    [BindProperty]
    public FaqQuestionViewModel ViewModel { get; set; }
    
    protected IFaqQuestionAdminAppService FaqQuestionAdminAppService { get; }

    public CreateQuestionModalModel(IFaqQuestionAdminAppService faqQuestionAdminAppService)
    {
        FaqQuestionAdminAppService = faqQuestionAdminAppService;
    }

    public virtual void OnGet(Guid sectionId)
    {
       ViewModel = new FaqQuestionViewModel
       {
           SectionId = sectionId
       }; 
    }
   
    public virtual async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();
        
        await FaqQuestionAdminAppService.CreateAsync(ObjectMapper.Map<FaqQuestionViewModel, CreateFaqQuestionDto>(ViewModel));

        return NoContent();
    }

    public class FaqQuestionViewModel
    {
        [Required]
        [HiddenInput]
        public Guid SectionId { get; set; }
        
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
