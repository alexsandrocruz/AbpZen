using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finCentrosResultado;
using Sapienza.Lexus.finCentrosResultado.Dtos;
using Sapienza.Lexus.Web.Pages.finCentrosResultado.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finCentrosResultado;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinCentrosResultadoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinCentrosResultadoAppService _finCentrosResultadoAppService;

    public CreateModalModel(
        IfinCentrosResultadoAppService finCentrosResultadoAppService
    )
    {
        _finCentrosResultadoAppService = finCentrosResultadoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinCentrosResultadoViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinCentrosResultadoViewModel, CreateUpdatefinCentrosResultadoDto>(ViewModel);
        await _finCentrosResultadoAppService.CreateAsync(dto);
        return NoContent();
    }
}
