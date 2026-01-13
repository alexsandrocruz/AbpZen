using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProNaturezas;
using Sapienza.Lexus.advProNaturezas.Dtos;
using Sapienza.Lexus.Web.Pages.advProNaturezas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProNaturezas;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProNaturezasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProNaturezasAppService _advProNaturezasAppService;

    public EditModalModel(
        IadvProNaturezasAppService advProNaturezasAppService
    )
    {
        _advProNaturezasAppService = advProNaturezasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProNaturezasAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProNaturezasDto, EditadvProNaturezasViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProNaturezasViewModel, CreateUpdateadvProNaturezasDto>(ViewModel);
        await _advProNaturezasAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
