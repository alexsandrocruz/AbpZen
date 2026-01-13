using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advClientes_bkp;
using Sapienza.Lexus.advClientes_bkp.Dtos;
using Sapienza.Lexus.Web.Pages.advClientes_bkp.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advClientes_bkp;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvClientes_bkpViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvClientes_bkpAppService _advClientes_bkpAppService;

    public EditModalModel(
        IadvClientes_bkpAppService advClientes_bkpAppService
    )
    {
        _advClientes_bkpAppService = advClientes_bkpAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advClientes_bkpAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advClientes_bkpDto, EditadvClientes_bkpViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvClientes_bkpViewModel, CreateUpdateadvClientes_bkpDto>(ViewModel);
        await _advClientes_bkpAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
