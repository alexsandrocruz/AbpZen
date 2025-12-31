using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Forms.Forms;

namespace Volo.Forms.Web.Pages.Forms;

public class SendModalModel : FormsPageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public SendFormInfoModel Form { get; set; }

    protected IFormAppService FormAppService { get; }
    protected FormRoutingOptions FormViewRoutingOptions { get; }


    public SendModalModel(IFormAppService formAppService, IOptions<FormRoutingOptions> formRoutingOptions)
    {
        FormAppService = formAppService;
        FormViewRoutingOptions = formRoutingOptions.Value;
    }

    public virtual async Task OnGetAsync()
    {
        var form = await FormAppService.GetAsync(Id);

        var link = GenerateLink(form.Id);
        var message = L["Form:SendFormInvitation"].Value + $"\n<br />\n<a href=\"{link}\">{form.Title}</a>";

        Form = new SendFormInfoModel
        {
            Id = form.Id,
            Link = link,
            Body = message,
            Subject = form.Title
        };
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        return NoContent();
    }

    public class SendFormInfoModel
    {
        [HiddenInput]
        public Guid Id { get; set; }
        public string Link { get; set; }
        [TextArea(Rows = 3)]
        public string Body { get; set; }
        public string Subject { get; set; }
        [Required]
        [EmailAddress]
        public string To { get; set; }
    }

    private string GenerateLink(Guid formId)
    {
        if (FormViewRoutingOptions.HostUrl.IsNullOrWhiteSpace())
        {
            return $"{Request.Scheme}://{Request.Host}/Forms/{formId}/ViewForm";
        }
        
        return $"{FormViewRoutingOptions.HostUrl.EnsureEndsWith('/')}Forms/{formId}/ViewForm";
    }
}
