using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finCentrosResultado;
using Sapienza.Lexus.finCentrosResultado.Dtos;
using Sapienza.Lexus.Web.Pages.finCentrosResultado.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finCentrosResultado;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinCentrosResultadoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinCentrosResultadoAppService _finCentrosResultadoAppService;

    public EditModalModel(
        IfinCentrosResultadoAppService finCentrosResultadoAppService
    )
    {
        _finCentrosResultadoAppService = finCentrosResultadoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finCentrosResultadoAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finCentrosResultadoDto, EditfinCentrosResultadoViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinCentrosResultadoViewModel, CreateUpdatefinCentrosResultadoDto>(ViewModel);
        await _finCentrosResultadoAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
