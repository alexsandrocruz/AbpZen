using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.Order;
using LeptonXDemoApp.Order.Dtos;
using LeptonXDemoApp.Web.Pages.Order.ViewModels;

namespace LeptonXDemoApp.Web.Pages.Order;

public class CreateModel : LeptonXDemoAppPageModel
{
    [BindProperty]
    public CreateOrderViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IOrderAppService _orderAppService;

    public CreateModel(
        IOrderAppService orderAppService
    )
    {
        _orderAppService = orderAppService;
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
        return RedirectToPage("Index");
    }
}
