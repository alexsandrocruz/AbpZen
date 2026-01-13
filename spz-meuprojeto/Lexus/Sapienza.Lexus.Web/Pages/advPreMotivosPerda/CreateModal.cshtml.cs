using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPreMotivosPerda;
using Sapienza.Lexus.advPreMotivosPerda.Dtos;
using Sapienza.Lexus.Web.Pages.advPreMotivosPerda.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPreMotivosPerda;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvPreMotivosPerdaViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreMotivosPerdaAppService _advPreMotivosPerdaAppService;

    public CreateModalModel(
        IadvPreMotivosPerdaAppService advPreMotivosPerdaAppService
    )
    {
        _advPreMotivosPerdaAppService = advPreMotivosPerdaAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvPreMotivosPerdaViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvPreMotivosPerdaViewModel, CreateUpdateadvPreMotivosPerdaDto>(ViewModel);
        await _advPreMotivosPerdaAppService.CreateAsync(dto);
        return NoContent();
    }
}
