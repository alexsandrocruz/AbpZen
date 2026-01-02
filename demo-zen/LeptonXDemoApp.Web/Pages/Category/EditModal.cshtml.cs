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

public class EditModalModel : LeptonXDemoAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditCategoryViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly ICategoryAppService _categoryAppService;

    public EditModalModel(
        ICategoryAppService categoryAppService
    )
    {
        _categoryAppService = categoryAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _categoryAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<CategoryDto, EditCategoryViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditCategoryViewModel, CreateUpdateCategoryDto>(ViewModel);
        await _categoryAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
