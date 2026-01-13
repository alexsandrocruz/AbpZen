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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvCliTiposArquivosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliTiposArquivosAppService _advCliTiposArquivosAppService;

    public EditModalModel(
        IadvCliTiposArquivosAppService advCliTiposArquivosAppService
    )
    {
        _advCliTiposArquivosAppService = advCliTiposArquivosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advCliTiposArquivosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advCliTiposArquivosDto, EditadvCliTiposArquivosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvCliTiposArquivosViewModel, CreateUpdateadvCliTiposArquivosDto>(ViewModel);
        await _advCliTiposArquivosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
