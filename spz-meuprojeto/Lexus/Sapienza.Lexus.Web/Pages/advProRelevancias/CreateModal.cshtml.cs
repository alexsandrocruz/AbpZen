using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProRelevancias;
using Sapienza.Lexus.advProRelevancias.Dtos;
using Sapienza.Lexus.Web.Pages.advProRelevancias.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProRelevancias;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProRelevanciasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProRelevanciasAppService _advProRelevanciasAppService;

    public CreateModalModel(
        IadvProRelevanciasAppService advProRelevanciasAppService
    )
    {
        _advProRelevanciasAppService = advProRelevanciasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProRelevanciasViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProRelevanciasViewModel, CreateUpdateadvProRelevanciasDto>(ViewModel);
        await _advProRelevanciasAppService.CreateAsync(dto);
        return NoContent();
    }
}
