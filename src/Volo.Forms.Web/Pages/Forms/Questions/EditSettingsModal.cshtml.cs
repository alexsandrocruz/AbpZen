using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Forms.Forms;

namespace Volo.Forms.Web.Pages.Forms.Questions;

public class EditSettingsModalModel : FormsPageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public UpdateFormSettingInputDto FormSettings { get; set; }

    protected IFormAppService FormAppService { get; }

    public EditSettingsModalModel(IFormAppService formAppService)
    {
        FormAppService = formAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var settings = await FormAppService.GetSettingsAsync(Id);

        FormSettings = ObjectMapper.Map<FormSettingsDto, UpdateFormSettingInputDto>(settings);
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await FormAppService.SetSettingsAsync(Id, FormSettings);

        return NoContent();
    }
}
