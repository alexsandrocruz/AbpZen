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

public class EditModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditSpecializationViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly ISpecializationAppService _specializationAppService;

    public EditModel(
        ISpecializationAppService specializationAppService
    )
    {
        _specializationAppService = specializationAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _specializationAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<SpecializationDto, EditSpecializationViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditSpecializationViewModel, CreateUpdateSpecializationDto>(ViewModel);
        await _specializationAppService.UpdateAsync(Id, dto);
        return RedirectToPage("Index");
    }
}
