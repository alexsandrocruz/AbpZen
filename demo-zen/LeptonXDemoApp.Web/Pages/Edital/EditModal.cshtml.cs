using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeptonXDemoApp.Edital;
using LeptonXDemoApp.Edital.Dtos;
using LeptonXDemoApp.Web.Pages.Edital.ViewModels;

namespace LeptonXDemoApp.Web.Pages.Edital;

public class EditModalModel : LeptonXDemoAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public EditEditalViewModel ViewModel { get; set; }

    private readonly IEditalAppService _editalAppService;

    public EditModalModel(IEditalAppService editalAppService)
    {
        _editalAppService = editalAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _editalAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<EditalDto, EditEditalViewModel>(dto);
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<EditEditalViewModel, CreateUpdateEditalDto>(ViewModel);
        await _editalAppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
