using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advClientesConvertidos;
using Sapienza.Lexus.advClientesConvertidos.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesConvertidos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advClientesConvertidos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvClientesConvertidosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesConvertidosAppService _advClientesConvertidosAppService;

    public CreateModalModel(
        IadvClientesConvertidosAppService advClientesConvertidosAppService
    )
    {
        _advClientesConvertidosAppService = advClientesConvertidosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvClientesConvertidosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvClientesConvertidosViewModel, CreateUpdateadvClientesConvertidosDto>(ViewModel);
        await _advClientesConvertidosAppService.CreateAsync(dto);
        return NoContent();
    }
}
