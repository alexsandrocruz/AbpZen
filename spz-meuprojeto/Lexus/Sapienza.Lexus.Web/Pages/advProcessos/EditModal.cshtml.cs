using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProcessos;
using Sapienza.Lexus.advProcessos.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProcessos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProcessosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProcessosAppService _advProcessosAppService;

    public EditModalModel(
        IadvProcessosAppService advProcessosAppService
    )
    {
        _advProcessosAppService = advProcessosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProcessosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProcessosDto, EditadvProcessosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProcessosViewModel, CreateUpdateadvProcessosDto>(ViewModel);
        await _advProcessosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
