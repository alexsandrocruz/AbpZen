using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.fabMotivosPerda;
using Sapienza.Lexus.fabMotivosPerda.Dtos;
using Sapienza.Lexus.Web.Pages.fabMotivosPerda.ViewModels;

namespace Sapienza.Lexus.Web.Pages.fabMotivosPerda;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatefabMotivosPerdaViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabMotivosPerdaAppService _fabMotivosPerdaAppService;

    public CreateModalModel(
        IfabMotivosPerdaAppService fabMotivosPerdaAppService
    )
    {
        _fabMotivosPerdaAppService = fabMotivosPerdaAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatefabMotivosPerdaViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatefabMotivosPerdaViewModel, CreateUpdatefabMotivosPerdaDto>(ViewModel);
        await _fabMotivosPerdaAppService.CreateAsync(dto);
        return NoContent();
    }
}
