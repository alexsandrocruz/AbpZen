using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finRateios;
using Sapienza.Lexus.finRateios.Dtos;
using Sapienza.Lexus.Web.Pages.finRateios.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finRateios;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinRateiosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinRateiosAppService _finRateiosAppService;

    public EditModalModel(
        IfinRateiosAppService finRateiosAppService
    )
    {
        _finRateiosAppService = finRateiosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finRateiosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finRateiosDto, EditfinRateiosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinRateiosViewModel, CreateUpdatefinRateiosDto>(ViewModel);
        await _finRateiosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
