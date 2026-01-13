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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfabPermissoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfabPermissoesAppService _fabPermissoesAppService;

    public EditModalModel(
        IfabPermissoesAppService fabPermissoesAppService
    )
    {
        _fabPermissoesAppService = fabPermissoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _fabPermissoesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<fabPermissoesDto, EditfabPermissoesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfabPermissoesViewModel, CreateUpdatefabPermissoesDto>(ViewModel);
        await _fabPermissoesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
