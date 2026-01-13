using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPreLogStatus;
using Sapienza.Lexus.advPreLogStatus.Dtos;
using Sapienza.Lexus.Web.Pages.advPreLogStatus.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPreLogStatus;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvPreLogStatusViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreLogStatusAppService _advPreLogStatusAppService;

    public EditModalModel(
        IadvPreLogStatusAppService advPreLogStatusAppService
    )
    {
        _advPreLogStatusAppService = advPreLogStatusAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advPreLogStatusAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advPreLogStatusDto, EditadvPreLogStatusViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvPreLogStatusViewModel, CreateUpdateadvPreLogStatusDto>(ViewModel);
        await _advPreLogStatusAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
