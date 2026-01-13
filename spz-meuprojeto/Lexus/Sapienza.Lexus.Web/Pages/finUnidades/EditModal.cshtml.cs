using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finUnidades;
using Sapienza.Lexus.finUnidades.Dtos;
using Sapienza.Lexus.Web.Pages.finUnidades.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finUnidades;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinUnidadesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinUnidadesAppService _finUnidadesAppService;

    public EditModalModel(
        IfinUnidadesAppService finUnidadesAppService
    )
    {
        _finUnidadesAppService = finUnidadesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finUnidadesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finUnidadesDto, EditfinUnidadesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinUnidadesViewModel, CreateUpdatefinUnidadesDto>(ViewModel);
        await _finUnidadesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
