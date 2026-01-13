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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvPreMotivosPerdaViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreMotivosPerdaAppService _advPreMotivosPerdaAppService;

    public EditModalModel(
        IadvPreMotivosPerdaAppService advPreMotivosPerdaAppService
    )
    {
        _advPreMotivosPerdaAppService = advPreMotivosPerdaAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advPreMotivosPerdaAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advPreMotivosPerdaDto, EditadvPreMotivosPerdaViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvPreMotivosPerdaViewModel, CreateUpdateadvPreMotivosPerdaDto>(ViewModel);
        await _advPreMotivosPerdaAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
