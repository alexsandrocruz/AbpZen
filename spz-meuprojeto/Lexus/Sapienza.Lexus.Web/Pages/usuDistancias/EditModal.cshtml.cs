using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.usuDistancias;
using Sapienza.Lexus.usuDistancias.Dtos;
using Sapienza.Lexus.Web.Pages.usuDistancias.ViewModels;

namespace Sapienza.Lexus.Web.Pages.usuDistancias;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditusuDistanciasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IusuDistanciasAppService _usuDistanciasAppService;

    public EditModalModel(
        IusuDistanciasAppService usuDistanciasAppService
    )
    {
        _usuDistanciasAppService = usuDistanciasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _usuDistanciasAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<usuDistanciasDto, EditusuDistanciasViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditusuDistanciasViewModel, CreateUpdateusuDistanciasDto>(ViewModel);
        await _usuDistanciasAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
