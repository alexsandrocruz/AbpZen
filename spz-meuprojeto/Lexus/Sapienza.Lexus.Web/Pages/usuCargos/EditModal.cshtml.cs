using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.usuCargos;
using Sapienza.Lexus.usuCargos.Dtos;
using Sapienza.Lexus.Web.Pages.usuCargos.ViewModels;

namespace Sapienza.Lexus.Web.Pages.usuCargos;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditusuCargosViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IusuCargosAppService _usuCargosAppService;

    public EditModalModel(
        IusuCargosAppService usuCargosAppService
    )
    {
        _usuCargosAppService = usuCargosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _usuCargosAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<usuCargosDto, EditusuCargosViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditusuCargosViewModel, CreateUpdateusuCargosDto>(ViewModel);
        await _usuCargosAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
