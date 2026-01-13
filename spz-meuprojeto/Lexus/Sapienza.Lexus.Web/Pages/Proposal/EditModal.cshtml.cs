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

public class EditModalModel : Sapienza.LexusPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditProposalViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========
    public List<SelectListItem> ClientList { get; set; } = new();

    private readonly IProposalAppService _proposalAppService;
    private readonly IClientAppService _clientAppService;

    public EditModalModel(
        IProposalAppService proposalAppService,
        IClientAppService clientAppService
    )
    {
        _proposalAppService = proposalAppService;
        _clientAppService = clientAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _proposalAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<ProposalDto, EditProposalViewModel>(dto);

        // Load lookup data for FK dropdowns
        if (ViewModel.ClientId != null)
        {
            var client = await _clientAppService.GetAsync(ViewModel.ClientId.Value);
            ViewModel.ClientDisplayName = client.Name;
        }
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditProposalViewModel, CreateUpdateProposalDto>(ViewModel);
        await _proposalAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
