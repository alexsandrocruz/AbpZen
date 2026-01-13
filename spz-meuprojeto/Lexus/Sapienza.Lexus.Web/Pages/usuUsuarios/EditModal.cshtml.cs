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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditusuUsuariosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IusuUsuariosAppService _usuUsuariosAppService;

    public EditModalModel(
        IusuUsuariosAppService usuUsuariosAppService
    )
    {
        _usuUsuariosAppService = usuUsuariosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _usuUsuariosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<usuUsuariosDto, EditusuUsuariosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditusuUsuariosViewModel, CreateUpdateusuUsuariosDto>(ViewModel);
        await _usuUsuariosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
