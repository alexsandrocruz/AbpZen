using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.flwConfig;
using Sapienza.Lexus.flwConfig.Dtos;
using Sapienza.Lexus.Web.Pages.flwConfig.ViewModels;

namespace Sapienza.Lexus.Web.Pages.flwConfig;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditflwConfigViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IflwConfigAppService _flwConfigAppService;

    public EditModalModel(
        IflwConfigAppService flwConfigAppService
    )
    {
        _flwConfigAppService = flwConfigAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _flwConfigAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<flwConfigDto, EditflwConfigViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditflwConfigViewModel, CreateUpdateflwConfigDto>(ViewModel);
        await _flwConfigAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
