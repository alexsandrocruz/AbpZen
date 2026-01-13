using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finPlanoContas;
using Sapienza.Lexus.finPlanoContas.Dtos;
using Sapienza.Lexus.Web.Pages.finPlanoContas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finPlanoContas;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinPlanoContasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinPlanoContasAppService _finPlanoContasAppService;

    public EditModalModel(
        IfinPlanoContasAppService finPlanoContasAppService
    )
    {
        _finPlanoContasAppService = finPlanoContasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finPlanoContasAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finPlanoContasDto, EditfinPlanoContasViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinPlanoContasViewModel, CreateUpdatefinPlanoContasDto>(ViewModel);
        await _finPlanoContasAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
