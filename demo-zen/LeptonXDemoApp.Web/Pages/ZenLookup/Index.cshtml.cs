using System;
using System.Threading.Tasks;
using LeptonXDemoApp.Category;
using LeptonXDemoApp.Category.Dtos;
using LeptonXDemoApp.Customer;
using LeptonXDemoApp.Customer.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace LeptonXDemoApp.Web.Pages.ZenLookup;

public class IndexModel : AbpPageModel
{
    private readonly ICategoryAppService _categoryAppService;
    private readonly ICustomerAppService _customerAppService;

    public IndexModel(
        ICategoryAppService categoryAppService,
        ICustomerAppService customerAppService)
    {
        _categoryAppService = categoryAppService;
        _customerAppService = customerAppService;
    }

    public virtual async Task<IActionResult> OnGetCategoryAsync()
    {
        var result = await _categoryAppService.GetCategoryLookupAsync();
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostCategoryAsync([FromBody] CreateUpdateCategoryDto input)
    {
        var result = await _categoryAppService.CreateAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnGetCustomerAsync()
    {
        var result = await _customerAppService.GetCustomerLookupAsync();
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostCustomerAsync([FromBody] CreateUpdateCustomerDto input)
    {
        var result = await _customerAppService.CreateAsync(input);
        return new JsonResult(result);
    }
}
