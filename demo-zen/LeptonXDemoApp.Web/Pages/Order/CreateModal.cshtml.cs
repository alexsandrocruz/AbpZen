using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.Order;
using LeptonXDemoApp.Order.Dtos;
using LeptonXDemoApp.Web.Pages.Order.ViewModels;
using LeptonXDemoApp.Customer;
using LeptonXDemoApp.Customer.Dtos;

namespace LeptonXDemoApp.Web.Pages.Order;

public class CreateModalModel : LeptonXDemoAppPageModel
{
    [BindProperty]
    public CreateOrderViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========
    public List<SelectListItem> CustomerList { get; set; } = new();

    private readonly IOrderAppService _orderAppService;
    private readonly ICustomerAppService _customerAppService;

    public CreateModalModel(
        IOrderAppService orderAppService,
        ICustomerAppService customerAppService
    )
    {
        _orderAppService = orderAppService;
        _customerAppService = customerAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateOrderViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateOrderViewModel, CreateUpdateOrderDto>(ViewModel);
        await _orderAppService.CreateAsync(dto);
        return NoContent();
    }
}
