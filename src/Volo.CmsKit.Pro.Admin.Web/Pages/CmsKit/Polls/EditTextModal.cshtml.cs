using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Polls;

public class EditTextModal : AbpPageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Text { get; set; }


    public EditTextModal()
    {
    }

    public async Task OnGetAsync()
    {

    }

    public async Task OnPostAsync()
    {

    }
}
