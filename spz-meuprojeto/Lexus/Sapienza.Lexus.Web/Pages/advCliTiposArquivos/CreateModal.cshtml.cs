using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advCliTiposArquivos;
using Sapienza.Lexus.advCliTiposArquivos.Dtos;
using Sapienza.Lexus.Web.Pages.advCliTiposArquivos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advCliTiposArquivos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvCliTiposArquivosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliTiposArquivosAppService _advCliTiposArquivosAppService;

    public CreateModalModel(
        IadvCliTiposArquivosAppService advCliTiposArquivosAppService
    )
    {
        _advCliTiposArquivosAppService = advCliTiposArquivosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvCliTiposArquivosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvCliTiposArquivosViewModel, CreateUpdateadvCliTiposArquivosDto>(ViewModel);
        await _advCliTiposArquivosAppService.CreateAsync(dto);
        return NoContent();
    }
}
