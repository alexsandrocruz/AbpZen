using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advClientesArquivos;
using Sapienza.Lexus.advClientesArquivos.Dtos;
using Sapienza.Lexus.Web.Pages.advClientesArquivos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advClientesArquivos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvClientesArquivosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesArquivosAppService _advClientesArquivosAppService;

    public CreateModalModel(
        IadvClientesArquivosAppService advClientesArquivosAppService
    )
    {
        _advClientesArquivosAppService = advClientesArquivosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvClientesArquivosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvClientesArquivosViewModel, CreateUpdateadvClientesArquivosDto>(ViewModel);
        await _advClientesArquivosAppService.CreateAsync(dto);
        return NoContent();
    }
}
