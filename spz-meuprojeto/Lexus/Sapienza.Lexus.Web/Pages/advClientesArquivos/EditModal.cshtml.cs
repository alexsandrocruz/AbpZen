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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvClientesArquivosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientesArquivosAppService _advClientesArquivosAppService;

    public EditModalModel(
        IadvClientesArquivosAppService advClientesArquivosAppService
    )
    {
        _advClientesArquivosAppService = advClientesArquivosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advClientesArquivosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advClientesArquivosDto, EditadvClientesArquivosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvClientesArquivosViewModel, CreateUpdateadvClientesArquivosDto>(ViewModel);
        await _advClientesArquivosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
