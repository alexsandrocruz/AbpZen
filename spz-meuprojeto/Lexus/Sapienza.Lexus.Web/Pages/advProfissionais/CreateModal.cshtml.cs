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

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProfissionaisViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProfissionaisAppService _advProfissionaisAppService;

    public CreateModalModel(
        IadvProfissionaisAppService advProfissionaisAppService
    )
    {
        _advProfissionaisAppService = advProfissionaisAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProfissionaisViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProfissionaisViewModel, CreateUpdateadvProfissionaisDto>(ViewModel);
        await _advProfissionaisAppService.CreateAsync(dto);
        return NoContent();
    }
}
