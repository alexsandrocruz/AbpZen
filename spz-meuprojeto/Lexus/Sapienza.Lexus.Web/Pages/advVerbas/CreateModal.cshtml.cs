using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advVerbas;
using Sapienza.Lexus.advVerbas.Dtos;
using Sapienza.Lexus.Web.Pages.advVerbas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advVerbas;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvVerbasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvVerbasAppService _advVerbasAppService;

    public CreateModalModel(
        IadvVerbasAppService advVerbasAppService
    )
    {
        _advVerbasAppService = advVerbasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvVerbasViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvVerbasViewModel, CreateUpdateadvVerbasDto>(ViewModel);
        await _advVerbasAppService.CreateAsync(dto);
        return NoContent();
    }
}
