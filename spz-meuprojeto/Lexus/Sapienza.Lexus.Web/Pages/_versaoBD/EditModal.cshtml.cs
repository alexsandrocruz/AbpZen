using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus._versaoBD;
using Sapienza.Lexus._versaoBD.Dtos;
using Sapienza.Lexus.Web.Pages._versaoBD.ViewModels;

namespace Sapienza.Lexus.Web.Pages._versaoBD;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public Edit_versaoBDViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly I_versaoBDAppService __versaoBDAppService;

    public EditModalModel(
        I_versaoBDAppService _versaoBDAppService
    )
    {
        __versaoBDAppService = _versaoBDAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await __versaoBDAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<_versaoBDDto, Edit_versaoBDViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<Edit_versaoBDViewModel, CreateUpdate_versaoBDDto>(ViewModel);
        await __versaoBDAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
