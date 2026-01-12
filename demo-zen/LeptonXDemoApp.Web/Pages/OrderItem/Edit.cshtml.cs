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

public class EditModel : LeptonXDemoAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditOrderItemViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IOrderItemAppService _orderItemAppService;

    public EditModel(
        IOrderItemAppService orderItemAppService
    )
    {
        _orderItemAppService = orderItemAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _orderItemAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<OrderItemDto, EditOrderItemViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditOrderItemViewModel, CreateUpdateOrderItemDto>(ViewModel);
        await _orderItemAppService.UpdateAsync(Id, dto);
        return RedirectToPage("Index");
    }
}
