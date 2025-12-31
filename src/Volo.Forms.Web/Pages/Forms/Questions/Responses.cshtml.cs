using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Forms.Forms;

namespace Volo.Forms.Web.Pages.Forms.Questions;

public class ResponsesModel : FormsPageModel
{
    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    public long ResponseCount { get; set; }

    protected IFormAppService FormAppService { get; }

    public ResponsesModel(IFormAppService formAppService)
    {
        FormAppService = formAppService;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        var form = await FormAppService.GetAsync(Id);
        if (form.Id == Guid.Empty)
        {
            return NotFound();
        }

        ResponseCount = await FormAppService.GetResponsesCountAsync(Id);

        return Page();
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}
