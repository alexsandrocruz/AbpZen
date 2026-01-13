using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advCliBairros;
using Sapienza.Lexus.advCliBairros.Dtos;
using Sapienza.Lexus.Web.Pages.advCliBairros.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advCliBairros;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvCliBairrosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliBairrosAppService _advCliBairrosAppService;

    public CreateModalModel(
        IadvCliBairrosAppService advCliBairrosAppService
    )
    {
        _advCliBairrosAppService = advCliBairrosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvCliBairrosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvCliBairrosViewModel, CreateUpdateadvCliBairrosDto>(ViewModel);
        await _advCliBairrosAppService.CreateAsync(dto);
        return NoContent();
    }
}
