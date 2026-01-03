using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.OrderItem;
using LeptonXDemoApp.OrderItem.Dtos;
using LeptonXDemoApp.Web.Pages.OrderItem.ViewModels;
using LeptonXDemoApp.Product;
using LeptonXDemoApp.Product.Dtos;
using LeptonXDemoApp.Order;
using LeptonXDemoApp.Order.Dtos;

namespace LeptonXDemoApp.Web.Pages.OrderItem;

public class EditModalModel : LeptonXDemoAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditOrderItemViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========
    public List<SelectListItem> ProductList { get; set; } = new();
    public List<SelectListItem> OrderList { get; set; } = new();

    private readonly IOrderItemAppService _orderItemAppService;
    private readonly IProductAppService _productAppService;
    private readonly IOrderAppService _orderAppService;

    public EditModalModel(
        IOrderItemAppService orderItemAppService,
        IProductAppService productAppService,
        IOrderAppService orderAppService
    )
    {
        _orderItemAppService = orderItemAppService;
        _productAppService = productAppService;
        _orderAppService = orderAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _orderItemAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<OrderItemDto, EditOrderItemViewModel>(dto);

        // Load lookup data for FK dropdowns
        var productList = await _productAppService.GetListAsync(new ProductGetListInput { MaxResultCount = 1000 });
        ProductList = productList.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
        ViewModel.ProductList = ProductList;
        var orderList = await _orderAppService.GetListAsync(new OrderGetListInput { MaxResultCount = 1000 });
        OrderList = orderList.Items
            .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
            .ToList();
        ViewModel.OrderList = OrderList;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditOrderItemViewModel, CreateUpdateOrderItemDto>(ViewModel);
        await _orderItemAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
