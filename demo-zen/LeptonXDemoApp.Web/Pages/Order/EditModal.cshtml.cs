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

public class EditModalModel : LeptonXDemoAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditOrderViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========
    public List<SelectListItem> CustomerList { get; set; } = new();

    private readonly IOrderAppService _orderAppService;
    private readonly ICustomerAppService _customerAppService;

    public EditModalModel(
        IOrderAppService orderAppService,
        ICustomerAppService customerAppService
    )
    {
        _orderAppService = orderAppService;
        _customerAppService = customerAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _orderAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<OrderDto, EditOrderViewModel>(dto);

        // Load lookup data for FK dropdowns
        if (ViewModel.CustomerId != null)
        {
            var customer = await _customerAppService.GetAsync(ViewModel.CustomerId.Value);
            ViewModel.CustomerDisplayName = customer.Name;
        }
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditOrderViewModel, CreateUpdateOrderDto>(ViewModel);
        await _orderAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
