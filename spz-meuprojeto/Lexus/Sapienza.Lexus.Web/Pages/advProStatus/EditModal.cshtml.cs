using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProStatus;
using Sapienza.Lexus.advProStatus.Dtos;
using Sapienza.Lexus.Web.Pages.advProStatus.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProStatus;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProStatusViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProStatusAppService _advProStatusAppService;

    public EditModalModel(
        IadvProStatusAppService advProStatusAppService
    )
    {
        _advProStatusAppService = advProStatusAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProStatusAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProStatusDto, EditadvProStatusViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProStatusViewModel, CreateUpdateadvProStatusDto>(ViewModel);
        await _advProStatusAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
