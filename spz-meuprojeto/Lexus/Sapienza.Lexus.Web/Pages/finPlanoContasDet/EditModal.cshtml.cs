using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finPlanoContasDet;
using Sapienza.Lexus.finPlanoContasDet.Dtos;
using Sapienza.Lexus.Web.Pages.finPlanoContasDet.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finPlanoContasDet;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinPlanoContasDetViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinPlanoContasDetAppService _finPlanoContasDetAppService;

    public EditModalModel(
        IfinPlanoContasDetAppService finPlanoContasDetAppService
    )
    {
        _finPlanoContasDetAppService = finPlanoContasDetAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finPlanoContasDetAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finPlanoContasDetDto, EditfinPlanoContasDetViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinPlanoContasDetViewModel, CreateUpdatefinPlanoContasDetDto>(ViewModel);
        await _finPlanoContasDetAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
