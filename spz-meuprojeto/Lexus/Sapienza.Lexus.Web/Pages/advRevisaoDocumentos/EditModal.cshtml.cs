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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvRevisaoDocumentosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvRevisaoDocumentosAppService _advRevisaoDocumentosAppService;

    public EditModalModel(
        IadvRevisaoDocumentosAppService advRevisaoDocumentosAppService
    )
    {
        _advRevisaoDocumentosAppService = advRevisaoDocumentosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advRevisaoDocumentosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advRevisaoDocumentosDto, EditadvRevisaoDocumentosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvRevisaoDocumentosViewModel, CreateUpdateadvRevisaoDocumentosDto>(ViewModel);
        await _advRevisaoDocumentosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
