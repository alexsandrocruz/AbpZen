using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finPlanoContasGrupos;
using Sapienza.Lexus.finPlanoContasGrupos.Dtos;
using Sapienza.Lexus.Web.Pages.finPlanoContasGrupos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finPlanoContasGrupos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinPlanoContasGruposViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinPlanoContasGruposAppService _finPlanoContasGruposAppService;

    public EditModalModel(
        IfinPlanoContasGruposAppService finPlanoContasGruposAppService
    )
    {
        _finPlanoContasGruposAppService = finPlanoContasGruposAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finPlanoContasGruposAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finPlanoContasGruposDto, EditfinPlanoContasGruposViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinPlanoContasGruposViewModel, CreateUpdatefinPlanoContasGruposDto>(ViewModel);
        await _finPlanoContasGruposAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
