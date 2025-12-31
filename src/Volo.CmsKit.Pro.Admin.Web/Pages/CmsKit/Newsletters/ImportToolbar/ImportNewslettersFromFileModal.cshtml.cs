using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Newsletters.ImportToolbar;

public class ImportNewslettersFromFileModal : PageModel
{
    
    public string AllowedFileType { get; set; }
    
    public void OnGet()
    {
        AllowedFileType = ".csv";
    }
}