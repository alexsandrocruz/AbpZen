using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Forms.Forms;

namespace Volo.Forms.Web.Pages.Forms;

public class CreateModalModel : FormsPageModel
{
    [BindProperty]
    public CreateFormDto Form { get; set; }

    protected IFormAppService FormAppService { get; }

    public CreateModalModel(IFormAppService formAppService)
    {
        FormAppService = formAppService;
    }

    public virtual Task<IActionResult> OnGetAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();

        var createdForm = await FormAppService.CreateAsync(Form);

        var createdUrl = $"Forms/{createdForm.Id}/Questions";
        return Content(createdUrl);
    }
}
