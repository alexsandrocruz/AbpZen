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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfdtDevsViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfdtDevsAppService _fdtDevsAppService;

    public EditModalModel(
        IfdtDevsAppService fdtDevsAppService
    )
    {
        _fdtDevsAppService = fdtDevsAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _fdtDevsAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<fdtDevsDto, EditfdtDevsViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfdtDevsViewModel, CreateUpdatefdtDevsDto>(ViewModel);
        await _fdtDevsAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
