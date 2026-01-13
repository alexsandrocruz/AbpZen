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

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateadvPreArquivosStatusViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvPreArquivosStatusAppService _advPreArquivosStatusAppService;

    public CreateModalModel(
        IadvPreArquivosStatusAppService advPreArquivosStatusAppService
    )
    {
        _advPreArquivosStatusAppService = advPreArquivosStatusAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateadvPreArquivosStatusViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateadvPreArquivosStatusViewModel, CreateUpdateadvPreArquivosStatusDto>(ViewModel);
        await _advPreArquivosStatusAppService.CreateAsync(dto);
        return NoContent();
    }
}
