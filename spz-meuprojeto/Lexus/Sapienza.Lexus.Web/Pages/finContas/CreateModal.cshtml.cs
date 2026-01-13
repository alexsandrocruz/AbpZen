using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finContas;
using Sapienza.Lexus.finContas.Dtos;
using Sapienza.Lexus.Web.Pages.finContas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finContas;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefinContasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinContasAppService _finContasAppService;

    public CreateModalModel(
        IfinContasAppService finContasAppService
    )
    {
        _finContasAppService = finContasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefinContasViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefinContasViewModel, CreateUpdatefinContasDto>(ViewModel);
        await _finContasAppService.CreateAsync(dto);
        return NoContent();
    }
}
