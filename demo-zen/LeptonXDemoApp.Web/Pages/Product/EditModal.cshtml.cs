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

public class EditModalModel : LeptonXDemoAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditProductViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========
    public List<SelectListItem> CategoryList { get; set; } = new();

    private readonly IProductAppService _productAppService;
    private readonly ICategoryAppService _categoryAppService;

    public EditModalModel(
        IProductAppService productAppService,
        ICategoryAppService categoryAppService
    )
    {
        _productAppService = productAppService;
        _categoryAppService = categoryAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _productAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<ProductDto, EditProductViewModel>(dto);

        // Load lookup data for FK dropdowns
        var categoryList = await _categoryAppService.GetListAsync(new CategoryGetListInput { MaxResultCount = 1000 });
        CategoryList = categoryList.Items
            .Select(x => new SelectListItem(x.Id.ToString(), x.Id.ToString()))
            .ToList();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditProductViewModel, CreateUpdateProductDto>(ViewModel);
        await _productAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
