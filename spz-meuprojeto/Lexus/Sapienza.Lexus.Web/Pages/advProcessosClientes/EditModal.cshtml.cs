using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProcessosClientes;
using Sapienza.Lexus.advProcessosClientes.Dtos;
using Sapienza.Lexus.Web.Pages.advProcessosClientes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProcessosClientes;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProcessosClientesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProcessosClientesAppService _advProcessosClientesAppService;

    public EditModalModel(
        IadvProcessosClientesAppService advProcessosClientesAppService
    )
    {
        _advProcessosClientesAppService = advProcessosClientesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProcessosClientesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProcessosClientesDto, EditadvProcessosClientesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProcessosClientesViewModel, CreateUpdateadvProcessosClientesDto>(ViewModel);
        await _advProcessosClientesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
