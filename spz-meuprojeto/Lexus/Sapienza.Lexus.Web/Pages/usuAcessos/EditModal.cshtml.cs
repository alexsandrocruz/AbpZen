using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.usuAcessos;
using Sapienza.Lexus.usuAcessos.Dtos;
using Sapienza.Lexus.Web.Pages.usuAcessos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.usuAcessos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditusuAcessosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IusuAcessosAppService _usuAcessosAppService;

    public EditModalModel(
        IusuAcessosAppService usuAcessosAppService
    )
    {
        _usuAcessosAppService = usuAcessosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _usuAcessosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<usuAcessosDto, EditusuAcessosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditusuAcessosViewModel, CreateUpdateusuAcessosDto>(ViewModel);
        await _usuAcessosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
