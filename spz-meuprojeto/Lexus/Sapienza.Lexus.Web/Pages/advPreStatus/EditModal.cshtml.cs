using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPreStatus;
using Sapienza.Lexus.advPreStatus.Dtos;
using Sapienza.Lexus.Web.Pages.advPreStatus.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPreStatus;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvPreStatusViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreStatusAppService _advPreStatusAppService;

    public EditModalModel(
        IadvPreStatusAppService advPreStatusAppService
    )
    {
        _advPreStatusAppService = advPreStatusAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advPreStatusAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advPreStatusDto, EditadvPreStatusViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvPreStatusViewModel, CreateUpdateadvPreStatusDto>(ViewModel);
        await _advPreStatusAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
