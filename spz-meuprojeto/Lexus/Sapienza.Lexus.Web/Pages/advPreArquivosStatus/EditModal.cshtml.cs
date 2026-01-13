using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advPreArquivosStatus;
using Sapienza.Lexus.advPreArquivosStatus.Dtos;
using Sapienza.Lexus.Web.Pages.advPreArquivosStatus.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advPreArquivosStatus;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvPreArquivosStatusViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreArquivosStatusAppService _advPreArquivosStatusAppService;

    public EditModalModel(
        IadvPreArquivosStatusAppService advPreArquivosStatusAppService
    )
    {
        _advPreArquivosStatusAppService = advPreArquivosStatusAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advPreArquivosStatusAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advPreArquivosStatusDto, EditadvPreArquivosStatusViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvPreArquivosStatusViewModel, CreateUpdateadvPreArquivosStatusDto>(ViewModel);
        await _advPreArquivosStatusAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
