using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProOrgaos;
using Sapienza.Lexus.advProOrgaos.Dtos;
using Sapienza.Lexus.Web.Pages.advProOrgaos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProOrgaos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProOrgaosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProOrgaosAppService _advProOrgaosAppService;

    public EditModalModel(
        IadvProOrgaosAppService advProOrgaosAppService
    )
    {
        _advProOrgaosAppService = advProOrgaosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProOrgaosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProOrgaosDto, EditadvProOrgaosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProOrgaosViewModel, CreateUpdateadvProOrgaosDto>(ViewModel);
        await _advProOrgaosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
