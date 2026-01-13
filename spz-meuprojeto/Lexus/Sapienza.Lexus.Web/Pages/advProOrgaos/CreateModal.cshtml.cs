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

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvProOrgaosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProOrgaosAppService _advProOrgaosAppService;

    public CreateModalModel(
        IadvProOrgaosAppService advProOrgaosAppService
    )
    {
        _advProOrgaosAppService = advProOrgaosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvProOrgaosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvProOrgaosViewModel, CreateUpdateadvProOrgaosDto>(ViewModel);
        await _advProOrgaosAppService.CreateAsync(dto);
        return NoContent();
    }
}
