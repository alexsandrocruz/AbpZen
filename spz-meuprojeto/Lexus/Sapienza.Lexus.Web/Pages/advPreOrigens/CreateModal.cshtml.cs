using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPreOrigens;
using Sapienza.Lexus.advPreOrigens.Dtos;
using Sapienza.Lexus.Web.Pages.advPreOrigens.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPreOrigens;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvPreOrigensViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreOrigensAppService _advPreOrigensAppService;

    public CreateModalModel(
        IadvPreOrigensAppService advPreOrigensAppService
    )
    {
        _advPreOrigensAppService = advPreOrigensAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvPreOrigensViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvPreOrigensViewModel, CreateUpdateadvPreOrigensDto>(ViewModel);
        await _advPreOrigensAppService.CreateAsync(dto);
        return NoContent();
    }
}
