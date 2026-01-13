using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPreMetas;
using Sapienza.Lexus.advPreMetas.Dtos;
using Sapienza.Lexus.Web.Pages.advPreMetas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPreMetas;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvPreMetasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreMetasAppService _advPreMetasAppService;

    public CreateModalModel(
        IadvPreMetasAppService advPreMetasAppService
    )
    {
        _advPreMetasAppService = advPreMetasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvPreMetasViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvPreMetasViewModel, CreateUpdateadvPreMetasDto>(ViewModel);
        await _advPreMetasAppService.CreateAsync(dto);
        return NoContent();
    }
}
