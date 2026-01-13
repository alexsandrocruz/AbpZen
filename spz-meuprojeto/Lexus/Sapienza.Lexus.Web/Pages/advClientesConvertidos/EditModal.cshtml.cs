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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvClientesConvertidosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesConvertidosAppService _advClientesConvertidosAppService;

    public EditModalModel(
        IadvClientesConvertidosAppService advClientesConvertidosAppService
    )
    {
        _advClientesConvertidosAppService = advClientesConvertidosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advClientesConvertidosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advClientesConvertidosDto, EditadvClientesConvertidosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvClientesConvertidosViewModel, CreateUpdateadvClientesConvertidosDto>(ViewModel);
        await _advClientesConvertidosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
