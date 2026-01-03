using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.OrderItem;
using LeptonXDemoApp.OrderItem.Dtos;
using LeptonXDemoApp.Web.Pages.OrderItem.ViewModels;
using LeptonXDemoApp.Order;
using LeptonXDemoApp.Order.Dtos;

namespace LeptonXDemoApp.Web.Pages.OrderItem;

public class CreateModalModel : LeptonXDemoAppPageModel
{
    [BindProperty]
    public CreateOrderItemViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========
    public List<SelectListItem> OrderList { get; set; } = new();

    private readonly IOrderItemAppService _orderItemAppService;
    private readonly IOrderAppService _orderAppService;

    public CreateModalModel(
        IOrderItemAppService orderItemAppService,
        IOrderAppService orderAppService
    )
    {
        _orderItemAppService = orderItemAppService;
        _orderAppService = orderAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateOrderItemViewModel();

        // Load lookup data for FK dropdowns
        var orderList = await _orderAppService.GetListAsync(new OrderGetListInput { MaxResultCount = 1000 });
        OrderList = orderList.Items
            .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
            .ToList();
        ViewModel.OrderList = OrderList;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateOrderItemViewModel, CreateUpdateOrderItemDto>(ViewModel);
        await _orderItemAppService.CreateAsync(dto);
        return NoContent();
    }
}
