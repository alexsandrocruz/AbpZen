using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finRecibos;
using Sapienza.Lexus.finRecibos.Dtos;
using Sapienza.Lexus.Web.Pages.finRecibos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finRecibos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinRecibosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinRecibosAppService _finRecibosAppService;

    public CreateModalModel(
        IfinRecibosAppService finRecibosAppService
    )
    {
        _finRecibosAppService = finRecibosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinRecibosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinRecibosViewModel, CreateUpdatefinRecibosDto>(ViewModel);
        await _finRecibosAppService.CreateAsync(dto);
        return NoContent();
    }
}
