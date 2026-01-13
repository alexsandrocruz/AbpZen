using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advCliComoChegou;
using Sapienza.Lexus.advCliComoChegou.Dtos;
using Sapienza.Lexus.Web.Pages.advCliComoChegou.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advCliComoChegou;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvCliComoChegouViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliComoChegouAppService _advCliComoChegouAppService;

    public CreateModalModel(
        IadvCliComoChegouAppService advCliComoChegouAppService
    )
    {
        _advCliComoChegouAppService = advCliComoChegouAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvCliComoChegouViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvCliComoChegouViewModel, CreateUpdateadvCliComoChegouDto>(ViewModel);
        await _advCliComoChegouAppService.CreateAsync(dto);
        return NoContent();
    }
}
