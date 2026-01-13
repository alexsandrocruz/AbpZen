using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advCliComoChegou;
using Sapienza.Lexus.advCliComoChegou.Dtos;
using Sapienza.Lexus.Web.Pages.advCliComoChegou.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advCliComoChegou;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvCliComoChegouViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvCliComoChegouAppService _advCliComoChegouAppService;

    public EditModalModel(
        IadvCliComoChegouAppService advCliComoChegouAppService
    )
    {
        _advCliComoChegouAppService = advCliComoChegouAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advCliComoChegouAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advCliComoChegouDto, EditadvCliComoChegouViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvCliComoChegouViewModel, CreateUpdateadvCliComoChegouDto>(ViewModel);
        await _advCliComoChegouAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
