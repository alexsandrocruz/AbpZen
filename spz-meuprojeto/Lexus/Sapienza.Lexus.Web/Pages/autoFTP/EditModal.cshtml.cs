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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditautoFTPViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IautoFTPAppService _autoFTPAppService;

    public EditModalModel(
        IautoFTPAppService autoFTPAppService
    )
    {
        _autoFTPAppService = autoFTPAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _autoFTPAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<autoFTPDto, EditautoFTPViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditautoFTPViewModel, CreateUpdateautoFTPDto>(ViewModel);
        await _autoFTPAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
