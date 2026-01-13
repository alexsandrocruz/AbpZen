using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProfissionaisNaturezas;
using Sapienza.Lexus.advProfissionaisNaturezas.Dtos;
using Sapienza.Lexus.Web.Pages.advProfissionaisNaturezas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProfissionaisNaturezas;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProfissionaisNaturezasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProfissionaisNaturezasAppService _advProfissionaisNaturezasAppService;

    public EditModalModel(
        IadvProfissionaisNaturezasAppService advProfissionaisNaturezasAppService
    )
    {
        _advProfissionaisNaturezasAppService = advProfissionaisNaturezasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProfissionaisNaturezasAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProfissionaisNaturezasDto, EditadvProfissionaisNaturezasViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProfissionaisNaturezasViewModel, CreateUpdateadvProfissionaisNaturezasDto>(ViewModel);
        await _advProfissionaisNaturezasAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
