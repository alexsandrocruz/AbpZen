using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.usuPermissoes;
using Sapienza.Lexus.usuPermissoes.Dtos;
using Sapienza.Lexus.Web.Pages.usuPermissoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.usuPermissoes;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateusuPermissoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IusuPermissoesAppService _usuPermissoesAppService;

    public CreateModalModel(
        IusuPermissoesAppService usuPermissoesAppService
    )
    {
        _usuPermissoesAppService = usuPermissoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateusuPermissoesViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateusuPermissoesViewModel, CreateUpdateusuPermissoesDto>(ViewModel);
        await _usuPermissoesAppService.CreateAsync(dto);
        return NoContent();
    }
}
