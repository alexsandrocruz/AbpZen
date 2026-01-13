using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProVaras;
using Sapienza.Lexus.advProVaras.Dtos;
using Sapienza.Lexus.Web.Pages.advProVaras.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProVaras;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProVarasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProVarasAppService _advProVarasAppService;

    public EditModalModel(
        IadvProVarasAppService advProVarasAppService
    )
    {
        _advProVarasAppService = advProVarasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProVarasAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProVarasDto, EditadvProVarasViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProVarasViewModel, CreateUpdateadvProVarasDto>(ViewModel);
        await _advProVarasAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
