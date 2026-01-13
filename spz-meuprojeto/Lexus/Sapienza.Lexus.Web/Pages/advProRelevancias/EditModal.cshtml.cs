using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProRelevancias;
using Sapienza.Lexus.advProRelevancias.Dtos;
using Sapienza.Lexus.Web.Pages.advProRelevancias.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProRelevancias;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProRelevanciasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProRelevanciasAppService _advProRelevanciasAppService;

    public EditModalModel(
        IadvProRelevanciasAppService advProRelevanciasAppService
    )
    {
        _advProRelevanciasAppService = advProRelevanciasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProRelevanciasAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProRelevanciasDto, EditadvProRelevanciasViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProRelevanciasViewModel, CreateUpdateadvProRelevanciasDto>(ViewModel);
        await _advProRelevanciasAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
