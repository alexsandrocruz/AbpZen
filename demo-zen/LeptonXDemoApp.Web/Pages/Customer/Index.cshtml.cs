using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using LeptonXDemoApp.Customer;
using LeptonXDemoApp.Customer.Dtos;

namespace LeptonXDemoApp.Web.Pages.Customer;

public class IndexModel : LeptonXDemoAppPageModel
{
    public CustomerFilterInput CustomerFilter { get; set; }
    
    private readonly ICustomerAppService _customerAppService;

    public IndexModel(ICustomerAppService customerAppService)
    {
        _customerAppService = customerAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(CustomerGetListInput input)
    {
        var result = await _customerAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _customerAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class CustomerFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "Customer:Name")]
    public string? Name { get; set; }
}
