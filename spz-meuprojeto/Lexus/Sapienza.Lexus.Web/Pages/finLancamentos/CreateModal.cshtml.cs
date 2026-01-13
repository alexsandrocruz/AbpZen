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

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinLancamentosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinLancamentosAppService _finLancamentosAppService;

    public CreateModalModel(
        IfinLancamentosAppService finLancamentosAppService
    )
    {
        _finLancamentosAppService = finLancamentosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinLancamentosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinLancamentosViewModel, CreateUpdatefinLancamentosDto>(ViewModel);
        await _finLancamentosAppService.CreateAsync(dto);
        return NoContent();
    }
}
