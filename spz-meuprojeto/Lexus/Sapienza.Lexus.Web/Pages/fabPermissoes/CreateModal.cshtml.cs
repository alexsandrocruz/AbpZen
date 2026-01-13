using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabPermissoes;
using Sapienza.Lexus.fabPermissoes.Dtos;
using Sapienza.Lexus.Web.Pages.fabPermissoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabPermissoes;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefabPermissoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabPermissoesAppService _fabPermissoesAppService;

    public CreateModalModel(
        IfabPermissoesAppService fabPermissoesAppService
    )
    {
        _fabPermissoesAppService = fabPermissoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefabPermissoesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefabPermissoesViewModel, CreateUpdatefabPermissoesDto>(ViewModel);
        await _fabPermissoesAppService.CreateAsync(dto);
        return NoContent();
    }
}
