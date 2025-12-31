using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Faqs;
using Volo.CmsKit.Faqs;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Faqs;

public class EditSectionModalModel : AdminPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public FaqSectionEditViewModel ViewModel { get; set; } = new FaqSectionEditViewModel();

    public List<string> GroupNames { get; set; }

    protected virtual IFaqSectionAdminAppService FaqSectionAdminAppService { get; set; }

    public EditSectionModalModel(IFaqSectionAdminAppService faqSectionAdminAppService)
    {
        FaqSectionAdminAppService = faqSectionAdminAppService;
    }
    
    public async Task OnGet()
    {
        var groups = await FaqSectionAdminAppService.GetGroupsAsync();
        GroupNames = groups.Keys.ToList();

        var section =  await FaqSectionAdminAppService.GetAsync(Id);
        ViewModel.GroupName = section.GroupName;
        ViewModel.Name = section.Name;
        ViewModel.Order = section.Order;
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        var updateFaqSectionDto = new UpdateFaqSectionDto
        {
            GroupName = ViewModel.GroupName,
            Name = ViewModel.Name,
            Order = ViewModel.Order
        };

        await FaqSectionAdminAppService.UpdateAsync(Id, updateFaqSectionDto);

        return NoContent();
    }

    public class FaqSectionEditViewModel
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
