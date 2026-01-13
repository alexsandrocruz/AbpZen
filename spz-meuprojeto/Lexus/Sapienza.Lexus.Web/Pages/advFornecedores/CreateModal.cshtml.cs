using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advFornecedores;
using Sapienza.Lexus.advFornecedores.Dtos;
using Sapienza.Lexus.Web.Pages.advFornecedores.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advFornecedores;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvFornecedoresViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvFornecedoresAppService _advFornecedoresAppService;

    public CreateModalModel(
        IadvFornecedoresAppService advFornecedoresAppService
    )
    {
        _advFornecedoresAppService = advFornecedoresAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvFornecedoresViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvFornecedoresViewModel, CreateUpdateadvFornecedoresDto>(ViewModel);
        await _advFornecedoresAppService.CreateAsync(dto);
        return NoContent();
    }
}
