using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.Specialization;
using Sapienza.Lexus.Specialization.Dtos;
using Sapienza.Lexus.Web.Pages.Specialization.ViewModels;

namespace Sapienza.Lexus.Web.Pages.Specialization;

public class CreateModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateSpecializationViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly ISpecializationAppService _specializationAppService;

    public CreateModel(
        ISpecializationAppService specializationAppService
    )
    {
        _specializationAppService = specializationAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateSpecializationViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateSpecializationViewModel, CreateUpdateSpecializationDto>(ViewModel);
        await _specializationAppService.CreateAsync(dto);
        return RedirectToPage("Index");
    }
}
