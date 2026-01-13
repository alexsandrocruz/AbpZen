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

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvCliLocaisAtendidoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliLocaisAtendidoAppService _advCliLocaisAtendidoAppService;

    public CreateModalModel(
        IadvCliLocaisAtendidoAppService advCliLocaisAtendidoAppService
    )
    {
        _advCliLocaisAtendidoAppService = advCliLocaisAtendidoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvCliLocaisAtendidoViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvCliLocaisAtendidoViewModel, CreateUpdateadvCliLocaisAtendidoDto>(ViewModel);
        await _advCliLocaisAtendidoAppService.CreateAsync(dto);
        return NoContent();
    }
}
