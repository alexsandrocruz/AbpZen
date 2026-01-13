using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finRecibos;
using Sapienza.Lexus.finRecibos.Dtos;
using Sapienza.Lexus.Web.Pages.finRecibos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finRecibos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinRecibosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinRecibosAppService _finRecibosAppService;

    public EditModalModel(
        IfinRecibosAppService finRecibosAppService
    )
    {
        _finRecibosAppService = finRecibosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finRecibosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finRecibosDto, EditfinRecibosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinRecibosViewModel, CreateUpdatefinRecibosDto>(ViewModel);
        await _finRecibosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
