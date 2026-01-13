using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.flwAcoes;
using Sapienza.Lexus.flwAcoes.Dtos;
using Sapienza.Lexus.Web.Pages.flwAcoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.flwAcoes;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditflwAcoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IflwAcoesAppService _flwAcoesAppService;

    public EditModalModel(
        IflwAcoesAppService flwAcoesAppService
    )
    {
        _flwAcoesAppService = flwAcoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _flwAcoesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<flwAcoesDto, EditflwAcoesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditflwAcoesViewModel, CreateUpdateflwAcoesDto>(ViewModel);
        await _flwAcoesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
