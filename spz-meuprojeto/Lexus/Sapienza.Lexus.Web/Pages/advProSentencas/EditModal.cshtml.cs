using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advProSentencas;
using Sapienza.Lexus.advProSentencas.Dtos;
using Sapienza.Lexus.Web.Pages.advProSentencas.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advProSentencas;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvProSentencasViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvProSentencasAppService _advProSentencasAppService;

    public EditModalModel(
        IadvProSentencasAppService advProSentencasAppService
    )
    {
        _advProSentencasAppService = advProSentencasAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advProSentencasAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advProSentencasDto, EditadvProSentencasViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvProSentencasViewModel, CreateUpdateadvProSentencasDto>(ViewModel);
        await _advProSentencasAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
