using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finLancamentos;
using Sapienza.Lexus.finLancamentos.Dtos;
using Sapienza.Lexus.Web.Pages.finLancamentos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finLancamentos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinLancamentosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinLancamentosAppService _finLancamentosAppService;

    public EditModalModel(
        IfinLancamentosAppService finLancamentosAppService
    )
    {
        _finLancamentosAppService = finLancamentosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finLancamentosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finLancamentosDto, EditfinLancamentosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinLancamentosViewModel, CreateUpdatefinLancamentosDto>(ViewModel);
        await _finLancamentosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
