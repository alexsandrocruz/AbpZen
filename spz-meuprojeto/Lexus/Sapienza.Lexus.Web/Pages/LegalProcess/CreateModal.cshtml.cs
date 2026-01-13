using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.LegalProcess;
using Sapienza.Lexus.LegalProcess.Dtos;
using Sapienza.Lexus.Web.Pages.LegalProcess.ViewModels;
using Sapienza.Lexus.Lawyer;
using Sapienza.Lexus.Lawyer.Dtos;
using Sapienza.Lexus.Client;
using Sapienza.Lexus.Client.Dtos;

namespace Sapienza.Lexus.Web.Pages.LegalProcess;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateLegalProcessViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========
    public List<SelectListItem> LawyerList { get; set; } = new();
    public List<SelectListItem> ClientList { get; set; } = new();

    private readonly ILegalProcessAppService _legalProcessAppService;
    private readonly ILawyerAppService _lawyerAppService;
    private readonly IClientAppService _clientAppService;

    public CreateModalModel(
        ILegalProcessAppService legalProcessAppService,
        ILawyerAppService lawyerAppService,
        IClientAppService clientAppService
    )
    {
        _legalProcessAppService = legalProcessAppService;
        _lawyerAppService = lawyerAppService;
        _clientAppService = clientAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateLegalProcessViewModel();

        // Load lookup data for FK dropdowns
        var lawyerList = await _lawyerAppService.GetListAsync(new LawyerGetListInput { MaxResultCount = 1000 });
        LawyerList = lawyerList.Items
            .Select(x => new SelectListItem(x.FullName, x.Id.ToString()))
            .ToList();
        ViewModel.LawyerList = LawyerList;
        var clientList = await _clientAppService.GetListAsync(new ClientGetListInput { MaxResultCount = 1000 });
        ClientList = clientList.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
        ViewModel.ClientList = ClientList;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateLegalProcessViewModel, CreateUpdateLegalProcessDto>(ViewModel);
        await _legalProcessAppService.CreateAsync(dto);
        return NoContent();
    }
}
