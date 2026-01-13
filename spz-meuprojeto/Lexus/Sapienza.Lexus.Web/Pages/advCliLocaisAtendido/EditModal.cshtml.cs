using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advCliLocaisAtendido;
using Sapienza.Lexus.advCliLocaisAtendido.Dtos;
using Sapienza.Lexus.Web.Pages.advCliLocaisAtendido.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advCliLocaisAtendido;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvCliLocaisAtendidoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliLocaisAtendidoAppService _advCliLocaisAtendidoAppService;

    public EditModalModel(
        IadvCliLocaisAtendidoAppService advCliLocaisAtendidoAppService
    )
    {
        _advCliLocaisAtendidoAppService = advCliLocaisAtendidoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advCliLocaisAtendidoAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advCliLocaisAtendidoDto, EditadvCliLocaisAtendidoViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvCliLocaisAtendidoViewModel, CreateUpdateadvCliLocaisAtendidoDto>(ViewModel);
        await _advCliLocaisAtendidoAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
