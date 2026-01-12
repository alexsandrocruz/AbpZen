using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.OrderItem;
using LeptonXDemoApp.OrderItem.Dtos;
using LeptonXDemoApp.Web.Pages.OrderItem.ViewModels;

namespace LeptonXDemoApp.Web.Pages.OrderItem;

public class CreateModel : LeptonXDemoAppPageModel
{
    [BindProperty]
    public CreateOrderItemViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IOrderItemAppService _orderItemAppService;

    public CreateModel(
        IOrderItemAppService orderItemAppService
    )
    {
        _orderItemAppService = orderItemAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateOrderItemViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateOrderItemViewModel, CreateUpdateOrderItemDto>(ViewModel);
        await _orderItemAppService.CreateAsync(dto);
        return RedirectToPage("Index");
    }
}
