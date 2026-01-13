using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.advFornecedores;
using Sapienza.Lexus.advFornecedores.Dtos;
using Sapienza.Lexus.Web.Pages.advFornecedores.ViewModels;

namespace Sapienza.Lexus.Web.Pages.advFornecedores;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditadvFornecedoresViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IadvFornecedoresAppService _advFornecedoresAppService;

    public EditModalModel(
        IadvFornecedoresAppService advFornecedoresAppService
    )
    {
        _advFornecedoresAppService = advFornecedoresAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _advFornecedoresAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<advFornecedoresDto, EditadvFornecedoresViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditadvFornecedoresViewModel, CreateUpdateadvFornecedoresDto>(ViewModel);
        await _advFornecedoresAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
