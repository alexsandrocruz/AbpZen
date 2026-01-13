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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvClientesINSSViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesINSSAppService _advClientesINSSAppService;

    public EditModalModel(
        IadvClientesINSSAppService advClientesINSSAppService
    )
    {
        _advClientesINSSAppService = advClientesINSSAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advClientesINSSAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advClientesINSSDto, EditadvClientesINSSViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvClientesINSSViewModel, CreateUpdateadvClientesINSSDto>(ViewModel);
        await _advClientesINSSAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
