using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.Client;
using Sapienza.Lexus.Client.Dtos;
using Sapienza.Lexus.Web.Pages.Client.ViewModels;

namespace Sapienza.Lexus.Web.Pages.Client;

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditClientViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IClientAppService _clientAppService;

    public EditModalModel(
        IClientAppService clientAppService
    )
    {
        _clientAppService = clientAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _clientAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<ClientDto, EditClientViewModel>(dto);

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditClientViewModel, CreateUpdateClientDto>(ViewModel);
        await _clientAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
