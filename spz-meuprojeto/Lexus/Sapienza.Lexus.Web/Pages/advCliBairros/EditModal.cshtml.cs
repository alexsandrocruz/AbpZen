using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advCliBairros;
using Sapienza.Lexus.advCliBairros.Dtos;
using Sapienza.Lexus.Web.Pages.advCliBairros.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advCliBairros;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvCliBairrosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliBairrosAppService _advCliBairrosAppService;

    public EditModalModel(
        IadvCliBairrosAppService advCliBairrosAppService
    )
    {
        _advCliBairrosAppService = advCliBairrosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advCliBairrosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advCliBairrosDto, EditadvCliBairrosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvCliBairrosViewModel, CreateUpdateadvCliBairrosDto>(ViewModel);
        await _advCliBairrosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
