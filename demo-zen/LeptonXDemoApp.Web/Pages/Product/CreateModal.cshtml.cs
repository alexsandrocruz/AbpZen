using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.Product;
using LeptonXDemoApp.Product.Dtos;
using LeptonXDemoApp.Web.Pages.Product.ViewModels;
using LeptonXDemoApp.Category;
using LeptonXDemoApp.Category.Dtos;

namespace LeptonXDemoApp.Web.Pages.Product;

public class CreateModalModel : LeptonXDemoAppPageModel
{
    [BindProperty]
    public CreateProductViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========
    public List<SelectListItem> CategoryList { get; set; } = new();

    private readonly IProductAppService _productAppService;
    private readonly ICategoryAppService _categoryAppService;

    public CreateModalModel(
        IProductAppService productAppService,
        ICategoryAppService categoryAppService
    )
    {
        _productAppService = productAppService;
        _categoryAppService = categoryAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateProductViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateProductViewModel, CreateUpdateProductDto>(ViewModel);
        await _productAppService.CreateAsync(dto);
        return NoContent();
    }
}
