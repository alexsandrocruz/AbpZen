using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Public.Faqs;

namespace Volo.CmsKit.Pro.Web.Pages.Public.Shared.Components.Faqs;

[Widget(
    ScriptTypes = new[] { typeof(FaqScriptBundleContributor) },
    StyleTypes = new[] { typeof(FaqStyleBundleContributor) },
    AutoInitialize = true
)]
[ViewComponent(Name = "CmsFaq")]
public class FaqViewComponent : AbpViewComponent
{
    protected IFaqSectionPublicAppService FaqPublicAppService;
    
    public FaqViewComponent(IFaqSectionPublicAppService faqPublicAppService)
    {
        FaqPublicAppService = faqPublicAppService;
    }
    
    public virtual async Task<IViewComponentResult> InvokeAsync(string groupName, string sectionName)
    { 
        var section = await FaqPublicAppService.GetListSectionWithQuestionsAsync(new FaqSectionListFilterInput
        {
            GroupName = groupName,
            SectionName = sectionName
        });

        var faqViewModel = new FaqViewModel();
        faqViewModel.ShowSectionName = sectionName.IsNullOrEmpty();
        faqViewModel.SectionWithQuestions = section;

        return View("~/Pages/Public/Shared/Components/Faqs/Faq.cshtml", faqViewModel);
    }
}
