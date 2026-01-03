using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LeptonXDemoApp.OrderItem;
using LeptonXDemoApp.OrderItem.Dtos;

namespace LeptonXDemoApp.Web.Pages.OrderItem;

public class IndexModel : LeptonXDemoAppPageModel
{
    public OrderItemFilterInput OrderItemFilter { get; set; }
    
    private readonly IOrderItemAppService _orderItemAppService;

    public IndexModel(IOrderItemAppService orderItemAppService)
    {
        _orderItemAppService = orderItemAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(OrderItemGetListInput input)
    {
        var result = await _orderItemAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _orderItemAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class OrderItemFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "OrderItem:ProductId")]
    public Guid? ProductId { get; set; }
}
