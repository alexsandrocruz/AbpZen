using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPreMetas;
using Sapienza.Lexus.advPreMetas.Dtos;
using Sapienza.Lexus.Web.Pages.advPreMetas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPreMetas;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvPreMetasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreMetasAppService _advPreMetasAppService;

    public EditModalModel(
        IadvPreMetasAppService advPreMetasAppService
    )
    {
        _advPreMetasAppService = advPreMetasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advPreMetasAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advPreMetasDto, EditadvPreMetasViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvPreMetasViewModel, CreateUpdateadvPreMetasDto>(ViewModel);
        await _advPreMetasAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
