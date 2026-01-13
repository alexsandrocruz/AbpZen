using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advRevisaoDocumentos;
using Sapienza.Lexus.advRevisaoDocumentos.Dtos;
using Sapienza.Lexus.Web.Pages.advRevisaoDocumentos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advRevisaoDocumentos;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvRevisaoDocumentosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvRevisaoDocumentosAppService _advRevisaoDocumentosAppService;

    public CreateModalModel(
        IadvRevisaoDocumentosAppService advRevisaoDocumentosAppService
    )
    {
        _advRevisaoDocumentosAppService = advRevisaoDocumentosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvRevisaoDocumentosViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvRevisaoDocumentosViewModel, CreateUpdateadvRevisaoDocumentosDto>(ViewModel);
        await _advRevisaoDocumentosAppService.CreateAsync(dto);
        return NoContent();
    }
}
