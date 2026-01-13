using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProcessosDadosHerdeiros;
using Sapienza.Lexus.advProcessosDadosHerdeiros.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessosDadosHerdeiros.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProcessosDadosHerdeiros;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProcessosDadosHerdeirosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProcessosDadosHerdeirosAppService _advProcessosDadosHerdeirosAppService;

    public EditModalModel(
        IadvProcessosDadosHerdeirosAppService advProcessosDadosHerdeirosAppService
    )
    {
        _advProcessosDadosHerdeirosAppService = advProcessosDadosHerdeirosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProcessosDadosHerdeirosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProcessosDadosHerdeirosDto, EditadvProcessosDadosHerdeirosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProcessosDadosHerdeirosViewModel, CreateUpdateadvProcessosDadosHerdeirosDto>(ViewModel);
        await _advProcessosDadosHerdeirosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
