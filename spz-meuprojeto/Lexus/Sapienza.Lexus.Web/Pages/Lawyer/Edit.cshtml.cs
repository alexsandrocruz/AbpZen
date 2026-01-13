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

public class EditModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditLawyerViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly ILawyerAppService _lawyerAppService;

    public EditModel(
        ILawyerAppService lawyerAppService
    )
    {
        _lawyerAppService = lawyerAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _lawyerAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<LawyerDto, EditLawyerViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditLawyerViewModel, CreateUpdateLawyerDto>(ViewModel);
        await _lawyerAppService.UpdateAsync(Id, dto);
        return RedirectToPage("Index");
    }
}
