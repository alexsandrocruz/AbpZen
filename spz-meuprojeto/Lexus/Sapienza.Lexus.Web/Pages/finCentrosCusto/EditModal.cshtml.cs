using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.finCentrosCusto;
using Sapienza.Lexus.finCentrosCusto.Dtos;
using Sapienza.Lexus.Web.Pages.finCentrosCusto.ViewModels;

namespace Sapienza.Lexus.Web.Pages.finCentrosCusto;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditfinCentrosCustoViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IfinCentrosCustoAppService _finCentrosCustoAppService;

    public EditModalModel(
        IfinCentrosCustoAppService finCentrosCustoAppService
    )
    {
        _finCentrosCustoAppService = finCentrosCustoAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _finCentrosCustoAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<finCentrosCustoDto, EditfinCentrosCustoViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditfinCentrosCustoViewModel, CreateUpdatefinCentrosCustoDto>(ViewModel);
        await _finCentrosCustoAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
