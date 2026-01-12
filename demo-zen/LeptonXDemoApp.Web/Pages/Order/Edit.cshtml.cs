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

public class EditModel : LeptonXDemoAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditOrderViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IOrderAppService _orderAppService;

    public EditModel(
        IOrderAppService orderAppService
    )
    {
        _orderAppService = orderAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _orderAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<OrderDto, EditOrderViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditOrderViewModel, CreateUpdateOrderDto>(ViewModel);
        await _orderAppService.UpdateAsync(Id, dto);
        return RedirectToPage("Index");
    }
}
