using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.usuUsuarios;
using Sapienza.Lexus.usuUsuarios.Dtos;
using Sapienza.Lexus.Web.Pages.usuUsuarios.ViewModels;

namespace Sapienza.Lexus.Web.Pages.usuUsuarios;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateusuUsuariosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IusuUsuariosAppService _usuUsuariosAppService;

    public CreateModalModel(
        IusuUsuariosAppService usuUsuariosAppService
    )
    {
        _usuUsuariosAppService = usuUsuariosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateusuUsuariosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateusuUsuariosViewModel, CreateUpdateusuUsuariosDto>(ViewModel);
        await _usuUsuariosAppService.CreateAsync(dto);
        return NoContent();
    }
}
