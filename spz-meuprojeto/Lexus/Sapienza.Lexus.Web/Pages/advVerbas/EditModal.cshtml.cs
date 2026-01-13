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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvVerbasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvVerbasAppService _advVerbasAppService;

    public EditModalModel(
        IadvVerbasAppService advVerbasAppService
    )
    {
        _advVerbasAppService = advVerbasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advVerbasAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advVerbasDto, EditadvVerbasViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvVerbasViewModel, CreateUpdateadvVerbasDto>(ViewModel);
        await _advVerbasAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
