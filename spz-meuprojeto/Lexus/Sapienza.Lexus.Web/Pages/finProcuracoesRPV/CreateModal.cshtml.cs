using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finProcuracoesRPV;
using Sapienza.Lexus.finProcuracoesRPV.Dtos;
using Sapienza.Lexus.Web.Pages.finProcuracoesRPV.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finProcuracoesRPV;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinProcuracoesRPVViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinProcuracoesRPVAppService _finProcuracoesRPVAppService;

    public CreateModalModel(
        IfinProcuracoesRPVAppService finProcuracoesRPVAppService
    )
    {
        _finProcuracoesRPVAppService = finProcuracoesRPVAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinProcuracoesRPVViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinProcuracoesRPVViewModel, CreateUpdatefinProcuracoesRPVDto>(ViewModel);
        await _finProcuracoesRPVAppService.CreateAsync(dto);
        return NoContent();
    }
}
