using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.usuDistancias;
using Sapienza.Lexus.usuDistancias.Dtos;
using Sapienza.Lexus.Web.Pages.usuDistancias.ViewModels;

namespace Sapienza.Lexus.Web.Pages.usuDistancias;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateusuDistanciasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IusuDistanciasAppService _usuDistanciasAppService;

    public CreateModalModel(
        IusuDistanciasAppService usuDistanciasAppService
    )
    {
        _usuDistanciasAppService = usuDistanciasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateusuDistanciasViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateusuDistanciasViewModel, CreateUpdateusuDistanciasDto>(ViewModel);
        await _usuDistanciasAppService.CreateAsync(dto);
        return NoContent();
    }
}
