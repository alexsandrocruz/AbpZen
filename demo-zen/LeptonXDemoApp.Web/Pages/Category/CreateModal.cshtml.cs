using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.Category;
using LeptonXDemoApp.Category.Dtos;
using LeptonXDemoApp.Web.Pages.Category.ViewModels;

namespace LeptonXDemoApp.Web.Pages.Category;

public class CreateModalModel : LeptonXDemoAppPageModel
{
    [BindProperty]
    public CreateCategoryViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly ICategoryAppService _categoryAppService;

    public CreateModalModel(
        ICategoryAppService categoryAppService
    )
    {
        _categoryAppService = categoryAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateCategoryViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateCategoryViewModel, CreateUpdateCategoryDto>(ViewModel);
        await _categoryAppService.CreateAsync(dto);
        return NoContent();
    }
}
