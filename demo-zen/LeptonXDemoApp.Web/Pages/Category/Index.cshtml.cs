using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LeptonXDemoApp.Category;
using LeptonXDemoApp.Category.Dtos;

namespace LeptonXDemoApp.Web.Pages.Category;

public class IndexModel : LeptonXDemoAppPageModel
{
    public CategoryFilterInput CategoryFilter { get; set; }
    
    private readonly ICategoryAppService _categoryAppService;

    public IndexModel(ICategoryAppService categoryAppService)
    {
        _categoryAppService = categoryAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(CategoryGetListInput input)
    {
        var result = await _categoryAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _categoryAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class CategoryFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Category:Name")]
    public string? Name { get; set; }
}
