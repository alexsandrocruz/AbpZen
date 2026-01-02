using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LeptonXDemoApp.Order;
using LeptonXDemoApp.Order.Dtos;

namespace LeptonXDemoApp.Web.Pages.Order;

public class IndexModel : LeptonXDemoAppPageModel
{
    public OrderFilterInput OrderFilter { get; set; }
    
    private readonly IOrderAppService _orderAppService;

    public IndexModel(IOrderAppService orderAppService)
    {
        _orderAppService = orderAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(OrderGetListInput input)
    {
        var result = await _orderAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _orderAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class OrderFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Order:Number")]
    public string? Number { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Order:Date")]
    public DateTime? Date { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Order:CustomerId")]
    public Guid? CustomerId { get; set; }
}
