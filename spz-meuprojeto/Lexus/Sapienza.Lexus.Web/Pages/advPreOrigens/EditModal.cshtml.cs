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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvPreOrigensViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreOrigensAppService _advPreOrigensAppService;

    public EditModalModel(
        IadvPreOrigensAppService advPreOrigensAppService
    )
    {
        _advPreOrigensAppService = advPreOrigensAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advPreOrigensAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advPreOrigensDto, EditadvPreOrigensViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvPreOrigensViewModel, CreateUpdateadvPreOrigensDto>(ViewModel);
        await _advPreOrigensAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
