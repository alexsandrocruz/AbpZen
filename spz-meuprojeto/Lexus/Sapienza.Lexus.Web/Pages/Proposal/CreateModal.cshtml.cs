using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.Proposal;
using Sapienza.Lexus.Proposal.Dtos;
using Sapienza.Lexus.Web.Pages.Proposal.ViewModels;
using Sapienza.Lexus.Client;
using Sapienza.Lexus.Client.Dtos;

namespace Sapienza.Lexus.Web.Pages.Proposal;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateProposalViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========
    public List<SelectListItem> ClientList { get; set; } = new();

    private readonly IProposalAppService _proposalAppService;
    private readonly IClientAppService _clientAppService;

    public CreateModalModel(
        IProposalAppService proposalAppService,
        IClientAppService clientAppService
    )
    {
        _proposalAppService = proposalAppService;
        _clientAppService = clientAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateProposalViewModel();

        // Load lookup data for FK dropdowns
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateProposalViewModel, CreateUpdateProposalDto>(ViewModel);
        await _proposalAppService.CreateAsync(dto);
        return NoContent();
    }
}
