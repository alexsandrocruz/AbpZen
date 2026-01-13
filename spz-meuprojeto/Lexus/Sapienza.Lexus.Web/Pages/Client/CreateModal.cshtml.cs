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

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateClientViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========

    private readonly IClientAppService _clientAppService;

    public CreateModalModel(
        IClientAppService clientAppService
    )
    {
        _clientAppService = clientAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateClientViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateClientViewModel, CreateUpdateClientDto>(ViewModel);
        await _clientAppService.CreateAsync(dto);
        return NoContent();
    }
}
