using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advClientesINSS;
using Sapienza.Lexus.advClientesINSS.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesINSS.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advClientesINSS;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvClientesINSSViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesINSSAppService _advClientesINSSAppService;

    public CreateModalModel(
        IadvClientesINSSAppService advClientesINSSAppService
    )
    {
        _advClientesINSSAppService = advClientesINSSAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvClientesINSSViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvClientesINSSViewModel, CreateUpdateadvClientesINSSDto>(ViewModel);
        await _advClientesINSSAppService.CreateAsync(dto);
        return NoContent();
    }
}
