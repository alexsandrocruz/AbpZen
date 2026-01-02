using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeptonXDemoApp.Edital;
using LeptonXDemoApp.Edital.Dtos;
using LeptonXDemoApp.Web.Pages.Edital.ViewModels;

namespace LeptonXDemoApp.Web.Pages.Edital;

public class CreateModalModel : LeptonXDemoAppPageModel
{
    [BindProperty]
    public CreateEditalViewModel ViewModel { get; set; }

    private readonly IEditalAppService _editalAppService;

    public CreateModalModel(IEditalAppService editalAppService)
    {
        _editalAppService = editalAppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new CreateEditalViewModel();
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateEditalViewModel, CreateUpdateEditalDto>(ViewModel);
        await _editalAppService.CreateAsync(dto);
        return NoContent();
    }
}
