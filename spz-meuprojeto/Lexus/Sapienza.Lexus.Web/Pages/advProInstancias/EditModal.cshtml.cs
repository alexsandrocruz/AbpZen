using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProInstancias;
using Sapienza.Lexus.advProInstancias.Dtos;
using Sapienza.Lexus.Web.Pages.advProInstancias.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProInstancias;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProInstanciasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProInstanciasAppService _advProInstanciasAppService;

    public EditModalModel(
        IadvProInstanciasAppService advProInstanciasAppService
    )
    {
        _advProInstanciasAppService = advProInstanciasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProInstanciasAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProInstanciasDto, EditadvProInstanciasViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProInstanciasViewModel, CreateUpdateadvProInstanciasDto>(ViewModel);
        await _advProInstanciasAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
