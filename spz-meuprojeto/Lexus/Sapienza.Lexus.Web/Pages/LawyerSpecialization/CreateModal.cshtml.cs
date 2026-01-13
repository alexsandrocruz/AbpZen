using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sapienza.Lexus.LawyerSpecialization;
using Sapienza.Lexus.LawyerSpecialization.Dtos;
using Sapienza.Lexus.Web.Pages.LawyerSpecialization.ViewModels;
using Sapienza.Lexus.Lawyer;
using Sapienza.Lexus.Lawyer.Dtos;
using Sapienza.Lexus.Specialization;
using Sapienza.Lexus.Specialization.Dtos;

namespace Sapienza.Lexus.Web.Pages.LawyerSpecialization;

public class CreateModalModel : Sapienza.LexusPageModel
{
    [BindProperty]
    public CreateLawyerSpecializationViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========
    public List<SelectListItem> LawyerList { get; set; } = new();
    public List<SelectListItem> SpecializationList { get; set; } = new();

    private readonly ILawyerSpecializationAppService _lawyerSpecializationAppService;
    private readonly ILawyerAppService _lawyerAppService;
    private readonly ISpecializationAppService _specializationAppService;

    public CreateModalModel(
        ILawyerSpecializationAppService lawyerSpecializationAppService,
        ILawyerAppService lawyerAppService,
        ISpecializationAppService specializationAppService
    )
    {
        _lawyerSpecializationAppService = lawyerSpecializationAppService;
        _lawyerAppService = lawyerAppService;
        _specializationAppService = specializationAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateLawyerSpecializationViewModel();

        // Load lookup data for FK dropdowns
        var lawyerList = await _lawyerAppService.GetListAsync(new LawyerGetListInput { MaxResultCount = 1000 });
        LawyerList = lawyerList.Items
            .Select(x => new SelectListItem(x.FullName, x.Id.ToString()))
            .ToList();
        ViewModel.LawyerList = LawyerList;
        var specializationList = await _specializationAppService.GetListAsync(new SpecializationGetListInput { MaxResultCount = 1000 });
        SpecializationList = specializationList.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
        ViewModel.SpecializationList = SpecializationList;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateLawyerSpecializationViewModel, CreateUpdateLawyerSpecializationDto>(ViewModel);
        await _lawyerSpecializationAppService.CreateAsync(dto);
        return NoContent();
    }
}
