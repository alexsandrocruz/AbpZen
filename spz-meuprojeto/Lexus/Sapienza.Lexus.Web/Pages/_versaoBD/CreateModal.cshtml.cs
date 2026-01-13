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

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public Create_versaoBDViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly I_versaoBDAppService __versaoBDAppService;

    public CreateModalModel(
        I_versaoBDAppService _versaoBDAppService
    )
    {
        __versaoBDAppService = _versaoBDAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new Create_versaoBDViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<Create_versaoBDViewModel, CreateUpdate_versaoBDDto>(ViewModel);
        await __versaoBDAppService.CreateAsync(dto);
        return NoContent();
    }
}
