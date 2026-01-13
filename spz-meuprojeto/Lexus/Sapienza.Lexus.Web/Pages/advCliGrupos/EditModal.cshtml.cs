using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advCliGrupos;
using Sapienza.Lexus.advCliGrupos.Dtos;
using Sapienza.Lexus.Web.Pages.advCliGrupos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advCliGrupos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvCliGruposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliGruposAppService _advCliGruposAppService;

    public EditModalModel(
        IadvCliGruposAppService advCliGruposAppService
    )
    {
        _advCliGruposAppService = advCliGruposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advCliGruposAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advCliGruposDto, EditadvCliGruposViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvCliGruposViewModel, CreateUpdateadvCliGruposDto>(ViewModel);
        await _advCliGruposAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
