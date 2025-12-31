using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Faqs;
using Volo.CmsKit.Faqs;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Faqs;

public class CreateSectionModalModel : AdminPageModel
{
    protected IFaqSectionAdminAppService FaqSectionAdminAppService { get; }

    [BindProperty]
    public CreateFaqSectionViewModel ViewModel { get; set; }

    public List<string> GroupNames { get; set; }

    public CreateSectionModalModel(IFaqSectionAdminAppService faqSectionAdminAppService)
    {
        FaqSectionAdminAppService = faqSectionAdminAppService;
    }
    public virtual async Task OnGetAsync()
    {
        var groups =  await FaqSectionAdminAppService.GetGroupsAsync();
        if (!groups.Keys.Any())
        {
            Alerts.Warning(text: L["NoGroupFoundWarn"]);
        }
        GroupNames = groups.Keys.ToList();
    }
    public virtual async Task<IActionResult> OnPostAsync()
    {
        await FaqSectionAdminAppService.CreateAsync(ObjectMapper.Map<CreateFaqSectionViewModel, CreateFaqSectionDto>(ViewModel));

        return NoContent();
    }

    public class CreateFaqSectionViewModel
    {
        [Required]
        [DynamicMaxLength(typeof(FaqSectionConst), nameof(FaqSectionConst.MaxGroupNameLength))]
        public string GroupName { get; set; }

        [Required]
        [DynamicMaxLength(typeof(FaqSectionConst), nameof(FaqSectionConst.MaxNameLength))]
        public string Name { get; set; }

        [Required] 
        public int Order { get; set; }
    }
}
