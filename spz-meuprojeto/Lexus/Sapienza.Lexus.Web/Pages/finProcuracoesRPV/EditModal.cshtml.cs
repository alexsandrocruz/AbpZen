using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finProcuracoesRPV;
using Sapienza.Lexus.finProcuracoesRPV.Dtos;
using Sapienza.Lexus.Web.Pages.finProcuracoesRPV.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finProcuracoesRPV;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinProcuracoesRPVViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinProcuracoesRPVAppService _finProcuracoesRPVAppService;

    public EditModalModel(
        IfinProcuracoesRPVAppService finProcuracoesRPVAppService
    )
    {
        _finProcuracoesRPVAppService = finProcuracoesRPVAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finProcuracoesRPVAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finProcuracoesRPVDto, EditfinProcuracoesRPVViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinProcuracoesRPVViewModel, CreateUpdatefinProcuracoesRPVDto>(ViewModel);
        await _finProcuracoesRPVAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
