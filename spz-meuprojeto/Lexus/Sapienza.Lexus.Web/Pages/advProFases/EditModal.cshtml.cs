using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProFases;
using Sapienza.Lexus.advProFases.Dtos;
using Sapienza.Lexus.Web.Pages.advProFases.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProFases;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProFasesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProFasesAppService _advProFasesAppService;

    public EditModalModel(
        IadvProFasesAppService advProFasesAppService
    )
    {
        _advProFasesAppService = advProFasesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProFasesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProFasesDto, EditadvProFasesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProFasesViewModel, CreateUpdateadvProFasesDto>(ViewModel);
        await _advProFasesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
