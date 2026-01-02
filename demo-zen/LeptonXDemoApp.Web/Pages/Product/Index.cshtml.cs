using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LeptonXDemoApp.Product;
using LeptonXDemoApp.Product.Dtos;

namespace LeptonXDemoApp.Web.Pages.Product;

public class IndexModel : LeptonXDemoAppPageModel
{
    public ProductFilterInput ProductFilter { get; set; }
    
    private readonly IProductAppService _productAppService;

    public IndexModel(IProductAppService productAppService)
    {
        _productAppService = productAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(ProductGetListInput input)
    {
        var result = await _productAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _productAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class ProductFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Product:Name")]
    public string? Name { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Product:Price")]
    public decimal? Price { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Product:CategoryId")]
    public Guid? CategoryId { get; set; }
}
