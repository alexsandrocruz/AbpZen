using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fdtDevs;
using Sapienza.Lexus.fdtDevs.Dtos;
using Sapienza.Lexus.Web.Pages.fdtDevs.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fdtDevs;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefdtDevsViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfdtDevsAppService _fdtDevsAppService;

    public CreateModalModel(
        IfdtDevsAppService fdtDevsAppService
    )
    {
        _fdtDevsAppService = fdtDevsAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefdtDevsViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefdtDevsViewModel, CreateUpdatefdtDevsDto>(ViewModel);
        await _fdtDevsAppService.CreateAsync(dto);
        return NoContent();
    }
}
