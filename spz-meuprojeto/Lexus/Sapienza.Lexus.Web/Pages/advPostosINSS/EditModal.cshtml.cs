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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvPostosINSSViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPostosINSSAppService _advPostosINSSAppService;

    public EditModalModel(
        IadvPostosINSSAppService advPostosINSSAppService
    )
    {
        _advPostosINSSAppService = advPostosINSSAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advPostosINSSAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advPostosINSSDto, EditadvPostosINSSViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvPostosINSSViewModel, CreateUpdateadvPostosINSSDto>(ViewModel);
        await _advPostosINSSAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
