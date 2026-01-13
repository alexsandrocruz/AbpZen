using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finContas;
using Sapienza.Lexus.finContas.Dtos;
using Sapienza.Lexus.Web.Pages.finContas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finContas;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinContasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinContasAppService _finContasAppService;

    public EditModalModel(
        IfinContasAppService finContasAppService
    )
    {
        _finContasAppService = finContasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finContasAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finContasDto, EditfinContasViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinContasViewModel, CreateUpdatefinContasDto>(ViewModel);
        await _finContasAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
