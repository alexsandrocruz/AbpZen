using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Domain.Entities;
using Volo.Forms.Forms;

namespace Volo.Forms.Web.Pages.Forms;

public class PreviewModel : FormsPageModel
{
    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    private readonly IFormAppService _formAppService;

    public PreviewModel(IFormAppService formAppService)
    {
        _formAppService = formAppService;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        var form = await _formAppService.GetAsync(Id);
        if (form == null)
        {
            throw new EntityNotFoundException();
        }

        return Page();
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}
