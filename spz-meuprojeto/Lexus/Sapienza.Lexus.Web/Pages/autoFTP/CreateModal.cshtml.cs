using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.autoFTP;
using Sapienza.Lexus.autoFTP.Dtos;
using Sapienza.Lexus.Web.Pages.autoFTP.ViewModels;

namespace Sapienza.Lexus.Web.Pages.autoFTP;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateautoFTPViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IautoFTPAppService _autoFTPAppService;

    public CreateModalModel(
        IautoFTPAppService autoFTPAppService
    )
    {
        _autoFTPAppService = autoFTPAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateautoFTPViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateautoFTPViewModel, CreateUpdateautoFTPDto>(ViewModel);
        await _autoFTPAppService.CreateAsync(dto);
        return NoContent();
    }
}
