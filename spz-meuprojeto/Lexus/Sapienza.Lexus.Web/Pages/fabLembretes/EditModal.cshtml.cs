using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabLembretes;
using Sapienza.Lexus.fabLembretes.Dtos;
using Sapienza.Lexus.Web.Pages.fabLembretes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabLembretes;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfabLembretesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabLembretesAppService _fabLembretesAppService;

    public EditModalModel(
        IfabLembretesAppService fabLembretesAppService
    )
    {
        _fabLembretesAppService = fabLembretesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _fabLembretesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<fabLembretesDto, EditfabLembretesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfabLembretesViewModel, CreateUpdatefabLembretesDto>(ViewModel);
        await _fabLembretesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
