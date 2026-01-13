using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finGruposDRE;
using Sapienza.Lexus.finGruposDRE.Dtos;
using Sapienza.Lexus.Web.Pages.finGruposDRE.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finGruposDRE;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinGruposDREViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinGruposDREAppService _finGruposDREAppService;

    public EditModalModel(
        IfinGruposDREAppService finGruposDREAppService
    )
    {
        _finGruposDREAppService = finGruposDREAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finGruposDREAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finGruposDREDto, EditfinGruposDREViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinGruposDREViewModel, CreateUpdatefinGruposDREDto>(ViewModel);
        await _finGruposDREAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
