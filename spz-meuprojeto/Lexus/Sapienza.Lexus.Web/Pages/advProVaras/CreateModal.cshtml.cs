using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProVaras;
using Sapienza.Lexus.advProVaras.Dtos;
using Sapienza.Lexus.Web.Pages.advProVaras.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProVaras;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProVarasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProVarasAppService _advProVarasAppService;

    public CreateModalModel(
        IadvProVarasAppService advProVarasAppService
    )
    {
        _advProVarasAppService = advProVarasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProVarasViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProVarasViewModel, CreateUpdateadvProVarasDto>(ViewModel);
        await _advProVarasAppService.CreateAsync(dto);
        return NoContent();
    }
}
