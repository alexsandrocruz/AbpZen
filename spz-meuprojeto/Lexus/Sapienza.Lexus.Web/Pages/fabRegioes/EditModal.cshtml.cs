using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabRegioes;
using Sapienza.Lexus.fabRegioes.Dtos;
using Sapienza.Lexus.Web.Pages.fabRegioes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabRegioes;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfabRegioesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabRegioesAppService _fabRegioesAppService;

    public EditModalModel(
        IfabRegioesAppService fabRegioesAppService
    )
    {
        _fabRegioesAppService = fabRegioesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _fabRegioesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<fabRegioesDto, EditfabRegioesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfabRegioesViewModel, CreateUpdatefabRegioesDto>(ViewModel);
        await _fabRegioesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
