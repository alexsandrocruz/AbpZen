using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.usuPermissoes;
using Sapienza.Lexus.usuPermissoes.Dtos;
using Sapienza.Lexus.Web.Pages.usuPermissoes.ViewModels;

namespace Sapienza.Lexus.Web.Pages.usuPermissoes;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditusuPermissoesViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IusuPermissoesAppService _usuPermissoesAppService;

    public EditModalModel(
        IusuPermissoesAppService usuPermissoesAppService
    )
    {
        _usuPermissoesAppService = usuPermissoesAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _usuPermissoesAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<usuPermissoesDto, EditusuPermissoesViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditusuPermissoesViewModel, CreateUpdateusuPermissoesDto>(ViewModel);
        await _usuPermissoesAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
