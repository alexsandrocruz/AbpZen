using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPostosINSS;
using Sapienza.Lexus.advPostosINSS.Dtos;
using Sapienza.Lexus.Web.Pages.advPostosINSS.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPostosINSS;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvPostosINSSViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPostosINSSAppService _advPostosINSSAppService;

    public CreateModalModel(
        IadvPostosINSSAppService advPostosINSSAppService
    )
    {
        _advPostosINSSAppService = advPostosINSSAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvPostosINSSViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvPostosINSSViewModel, CreateUpdateadvPostosINSSDto>(ViewModel);
        await _advPostosINSSAppService.CreateAsync(dto);
        return NoContent();
    }
}
