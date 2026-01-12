using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeptonXDemoApp.Edital;
using LeptonXDemoApp.Edital.Dtos;
using LeptonXDemoApp.Web.Pages.Edital.ViewModels;

namespace LeptonXDemoApp.Web.Pages.Edital;

public class CreateModalModel : LeptonXDemoAppPageModel
{
    [BindProperty]
    public CreateEditalViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IEditalAppService _editalAppService;

    public CreateModalModel(
        IEditalAppService editalAppService
    )
    {
        _editalAppService = editalAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateEditalViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateEditalViewModel, CreateUpdateEditalDto>(ViewModel);
        await _editalAppService.CreateAsync(dto);
        return NoContent();
    }
}
