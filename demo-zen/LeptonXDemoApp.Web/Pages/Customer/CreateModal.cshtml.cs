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

public class CreateModalModel : LeptonXDemoAppPageModel
{
    [BindProperty]
    public CreateCustomerViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly ICustomerAppService _customerAppService;

    public CreateModalModel(
        ICustomerAppService customerAppService
    )
    {
        _customerAppService = customerAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateCustomerViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateCustomerViewModel, CreateUpdateCustomerDto>(ViewModel);
        await _customerAppService.CreateAsync(dto);
        return NoContent();
    }
}
