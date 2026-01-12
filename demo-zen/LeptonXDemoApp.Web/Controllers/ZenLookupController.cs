using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeptonXDemoApp.Category;
using LeptonXDemoApp.Category.Dtos;
using LeptonXDemoApp.Customer;
using LeptonXDemoApp.Customer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LeptonXDemoApp.Web.Controllers;

[Route("ZenLookup")]
[Authorize]
public class ZenLookupController : Controller
{
    [HttpGet("ping")]
    [AllowAnonymous]
    public IActionResult Ping()
    {
        return Ok("Pong");
    }

    private readonly ICategoryAppService _categoryAppService;
    private readonly ICustomerAppService _customerAppService;

    public ZenLookupController(
        ICategoryAppService categoryAppService,
        ICustomerAppService customerAppService)
    {
        _categoryAppService = categoryAppService;
        _customerAppService = customerAppService;
    }

    [HttpGet("category")]
    public virtual async Task<ListResultDto<Category.LookupDto<Guid>>> GetCategoryLookupAsync()
    {
        return await _categoryAppService.GetCategoryLookupAsync();
    }

    [HttpPost("category")]
    public virtual async Task<CategoryDto> CreateCategoryAsync(CreateUpdateCategoryDto input)
    {
        return await _categoryAppService.CreateAsync(input);
    }

    [HttpGet("customer")]
    public virtual async Task<ListResultDto<Customer.LookupDto<Guid>>> GetCustomerLookupAsync()
    {
        return await _customerAppService.GetCustomerLookupAsync();
    }

    [HttpPost("customer")]
    public virtual async Task<CustomerDto> CreateCustomerAsync(CreateUpdateCustomerDto input)
    {
        return await _customerAppService.CreateAsync(input);
    }
}
