using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.PropostalItem;
using Sapienza.Lexus.PropostalItem.Dtos;
using Sapienza.Lexus.Web.Pages.PropostalItem.ViewModels;
using Sapienza.Lexus.Proposal;
using Sapienza.Lexus.Proposal.Dtos;

namespace Sapienza.Lexus.Web.Pages.PropostalItem;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreatePropostalItemViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========
    public List<SelectListItem> ProposalList { get; set; } = new();

    private readonly IPropostalItemAppService _propostalItemAppService;
    private readonly IProposalAppService _proposalAppService;

    public CreateModalModel(
        IPropostalItemAppService propostalItemAppService,
        IProposalAppService proposalAppService
    )
    {
        _propostalItemAppService = propostalItemAppService;
        _proposalAppService = proposalAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreatePropostalItemViewModel();

        // Load lookup data for FK dropdowns
        var proposalList = await _proposalAppService.GetListAsync(new ProposalGetListInput { MaxResultCount = 1000 });
        ProposalList = proposalList.Items
            .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
            .ToList();
        ViewModel.ProposalList = ProposalList;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreatePropostalItemViewModel, CreateUpdatePropostalItemDto>(ViewModel);
        await _propostalItemAppService.CreateAsync(dto);
        return NoContent();
    }
}
