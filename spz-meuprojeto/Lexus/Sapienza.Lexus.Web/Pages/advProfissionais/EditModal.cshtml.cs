using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProfissionais;
using Sapienza.Lexus.advProfissionais.Dtos;
using Sapienza.Lexus.Web.Pages.advProfissionais.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProfissionais;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProfissionaisViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProfissionaisAppService _advProfissionaisAppService;

    public EditModalModel(
        IadvProfissionaisAppService advProfissionaisAppService
    )
    {
        _advProfissionaisAppService = advProfissionaisAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProfissionaisAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProfissionaisDto, EditadvProfissionaisViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProfissionaisViewModel, CreateUpdateadvProfissionaisDto>(ViewModel);
        await _advProfissionaisAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
