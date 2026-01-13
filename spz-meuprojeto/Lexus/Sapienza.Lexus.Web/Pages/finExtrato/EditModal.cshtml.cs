using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finExtrato;
using Sapienza.Lexus.finExtrato.Dtos;
using Sapienza.Lexus.Web.Pages.finExtrato.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finExtrato;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinExtratoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinExtratoAppService _finExtratoAppService;

    public EditModalModel(
        IfinExtratoAppService finExtratoAppService
    )
    {
        _finExtratoAppService = finExtratoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finExtratoAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finExtratoDto, EditfinExtratoViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinExtratoViewModel, CreateUpdatefinExtratoDto>(ViewModel);
        await _finExtratoAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
