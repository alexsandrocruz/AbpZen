using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.usuAcessos;
using Sapienza.Lexus.usuAcessos.Dtos;
using Sapienza.Lexus.Web.Pages.usuAcessos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.usuAcessos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateusuAcessosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IusuAcessosAppService _usuAcessosAppService;

    public CreateModalModel(
        IusuAcessosAppService usuAcessosAppService
    )
    {
        _usuAcessosAppService = usuAcessosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateusuAcessosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateusuAcessosViewModel, CreateUpdateusuAcessosDto>(ViewModel);
        await _usuAcessosAppService.CreateAsync(dto);
        return NoContent();
    }
}
