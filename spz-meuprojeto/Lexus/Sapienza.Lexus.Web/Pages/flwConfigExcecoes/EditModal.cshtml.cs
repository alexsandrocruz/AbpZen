using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.flwConfigExcecoes;
using Sapienza.Lexus.flwConfigExcecoes.Dtos;
using Sapienza.Lexus.Web.Pages.flwConfigExcecoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.flwConfigExcecoes;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditflwConfigExcecoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IflwConfigExcecoesAppService _flwConfigExcecoesAppService;

    public EditModalModel(
        IflwConfigExcecoesAppService flwConfigExcecoesAppService
    )
    {
        _flwConfigExcecoesAppService = flwConfigExcecoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _flwConfigExcecoesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<flwConfigExcecoesDto, EditflwConfigExcecoesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditflwConfigExcecoesViewModel, CreateUpdateflwConfigExcecoesDto>(ViewModel);
        await _flwConfigExcecoesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
