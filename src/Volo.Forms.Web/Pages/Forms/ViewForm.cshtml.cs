using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Forms.Forms;

namespace Volo.Forms.Web.Pages.Forms;

public class ViewFormModel : FormsPageModel
{
    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool RequiresLogin { get; set; }
    protected IFormAppService FormAppService { get; }
    
    protected FormRoutingOptions FormViewRoutingOptions { get; }

    public ViewFormModel(IFormAppService formAppService, IOptions<FormRoutingOptions> formRoutingOptions)
    {
        FormAppService = formAppService;
        FormViewRoutingOptions = formRoutingOptions.Value;
    }

    public virtual async Task<IActionResult> OnGet()
    {
        if (FormViewRoutingOptions.OnlyViewInHostProject && !FormViewRoutingOptions.HostUrl.IsNullOrWhiteSpace() && !Request.GetDisplayUrl().StartsWith(FormViewRoutingOptions.HostUrl))
        {
            return NotFound();
        }
        
        var formSettings = await FormAppService.GetSettingsAsync(Id);

        Title = formSettings.Title;
        RequiresLogin = formSettings.RequiresLogin;

        if (!formSettings.RequiresLogin)
        {
            return await Task.FromResult<IActionResult>(Page());
        }

        if (!CurrentUser.IsAuthenticated)
        {
            var returnUrl = $"/Forms/{Id}/ViewForm";
            return Redirect("/Account/Login?ReturnUrl=" + returnUrl);
        }

        return await Task.FromResult<IActionResult>(Page());
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}
