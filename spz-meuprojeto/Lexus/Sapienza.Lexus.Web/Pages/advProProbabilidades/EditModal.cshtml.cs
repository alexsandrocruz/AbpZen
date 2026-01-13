using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProProbabilidades;
using Sapienza.Lexus.advProProbabilidades.Dtos;
using Sapienza.Lexus.Web.Pages.advProProbabilidades.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProProbabilidades;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProProbabilidadesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProProbabilidadesAppService _advProProbabilidadesAppService;

    public EditModalModel(
        IadvProProbabilidadesAppService advProProbabilidadesAppService
    )
    {
        _advProProbabilidadesAppService = advProProbabilidadesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProProbabilidadesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProProbabilidadesDto, EditadvProProbabilidadesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProProbabilidadesViewModel, CreateUpdateadvProProbabilidadesDto>(ViewModel);
        await _advProProbabilidadesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
