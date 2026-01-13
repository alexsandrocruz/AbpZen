using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.logAcoes;
using Sapienza.Lexus.logAcoes.Dtos;
using Sapienza.Lexus.Web.Pages.logAcoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.logAcoes;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditlogAcoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IlogAcoesAppService _logAcoesAppService;

    public EditModalModel(
        IlogAcoesAppService logAcoesAppService
    )
    {
        _logAcoesAppService = logAcoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _logAcoesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<logAcoesDto, EditlogAcoesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditlogAcoesViewModel, CreateUpdatelogAcoesDto>(ViewModel);
        await _logAcoesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
