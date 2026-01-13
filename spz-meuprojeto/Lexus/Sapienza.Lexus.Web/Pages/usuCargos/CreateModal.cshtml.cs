using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.usuCargos;
using Sapienza.Lexus.usuCargos.Dtos;
using Sapienza.Lexus.Web.Pages.usuCargos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.usuCargos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateusuCargosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IusuCargosAppService _usuCargosAppService;

    public CreateModalModel(
        IusuCargosAppService usuCargosAppService
    )
    {
        _usuCargosAppService = usuCargosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateusuCargosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateusuCargosViewModel, CreateUpdateusuCargosDto>(ViewModel);
        await _usuCargosAppService.CreateAsync(dto);
        return NoContent();
    }
}
