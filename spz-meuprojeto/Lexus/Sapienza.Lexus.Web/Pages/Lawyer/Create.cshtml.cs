using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.Lawyer;
using Sapienza.Lexus.Lawyer.Dtos;
using Sapienza.Lexus.Web.Pages.Lawyer.ViewModels;

namespace Sapienza.Lexus.Web.Pages.Lawyer;

public class CreateModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateLawyerViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly ILawyerAppService _lawyerAppService;

    public CreateModel(
        ILawyerAppService lawyerAppService
    )
    {
        _lawyerAppService = lawyerAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateLawyerViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateLawyerViewModel, CreateUpdateLawyerDto>(ViewModel);
        await _lawyerAppService.CreateAsync(dto);
        return RedirectToPage("Index");
    }
}
