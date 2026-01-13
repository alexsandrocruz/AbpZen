using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPreCheckListsGrupos;
using Sapienza.Lexus.advPreCheckListsGrupos.Dtos;
using Sapienza.Lexus.Web.Pages.advPreCheckListsGrupos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPreCheckListsGrupos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvPreCheckListsGruposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreCheckListsGruposAppService _advPreCheckListsGruposAppService;

    public EditModalModel(
        IadvPreCheckListsGruposAppService advPreCheckListsGruposAppService
    )
    {
        _advPreCheckListsGruposAppService = advPreCheckListsGruposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advPreCheckListsGruposAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advPreCheckListsGruposDto, EditadvPreCheckListsGruposViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvPreCheckListsGruposViewModel, CreateUpdateadvPreCheckListsGruposDto>(ViewModel);
        await _advPreCheckListsGruposAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
