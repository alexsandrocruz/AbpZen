using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.Customer;
using LeptonXDemoApp.Customer.Dtos;
using LeptonXDemoApp.Web.Pages.Customer.ViewModels;

namespace LeptonXDemoApp.Web.Pages.Customer;

public class EditModalModel : LeptonXDemoAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditCustomerViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly ICustomerAppService _customerAppService;

    public EditModalModel(
        ICustomerAppService customerAppService
    )
    {
        _customerAppService = customerAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _customerAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<CustomerDto, EditCustomerViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditCustomerViewModel, CreateUpdateCustomerDto>(ViewModel);
        await _customerAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
