using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finContasClientes;
using Sapienza.Lexus.finContasClientes.Dtos;
using Sapienza.Lexus.Web.Pages.finContasClientes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finContasClientes;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinContasClientesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinContasClientesAppService _finContasClientesAppService;

    public EditModalModel(
        IfinContasClientesAppService finContasClientesAppService
    )
    {
        _finContasClientesAppService = finContasClientesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finContasClientesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finContasClientesDto, EditfinContasClientesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinContasClientesViewModel, CreateUpdatefinContasClientesDto>(ViewModel);
        await _finContasClientesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
