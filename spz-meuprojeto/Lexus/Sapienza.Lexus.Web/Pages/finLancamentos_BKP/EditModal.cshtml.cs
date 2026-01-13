using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finLancamentos_BKP;
using Sapienza.Lexus.finLancamentos_BKP.Dtos;
using Sapienza.Lexus.Web.Pages.finLancamentos_BKP.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finLancamentos_BKP;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinLancamentos_BKPViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinLancamentos_BKPAppService _finLancamentos_BKPAppService;

    public EditModalModel(
        IfinLancamentos_BKPAppService finLancamentos_BKPAppService
    )
    {
        _finLancamentos_BKPAppService = finLancamentos_BKPAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finLancamentos_BKPAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finLancamentos_BKPDto, EditfinLancamentos_BKPViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinLancamentos_BKPViewModel, CreateUpdatefinLancamentos_BKPDto>(ViewModel);
        await _finLancamentos_BKPAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
