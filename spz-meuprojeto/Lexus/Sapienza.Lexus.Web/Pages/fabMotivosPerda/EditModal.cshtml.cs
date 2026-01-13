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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfabMotivosPerdaViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabMotivosPerdaAppService _fabMotivosPerdaAppService;

    public EditModalModel(
        IfabMotivosPerdaAppService fabMotivosPerdaAppService
    )
    {
        _fabMotivosPerdaAppService = fabMotivosPerdaAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _fabMotivosPerdaAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<fabMotivosPerdaDto, EditfabMotivosPerdaViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfabMotivosPerdaViewModel, CreateUpdatefabMotivosPerdaDto>(ViewModel);
        await _fabMotivosPerdaAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
