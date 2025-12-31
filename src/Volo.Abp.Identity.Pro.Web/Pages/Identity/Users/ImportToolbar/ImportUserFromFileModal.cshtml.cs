using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Volo.Abp.Identity.Web.Pages.Identity.Users.ImportToolbar;

public class ImportUserFromFileModal : IdentityUserModalPageModel
{
    [BindProperty(SupportsGet = true)]
    public ImportUsersFromFileType FileType { get; set; }
    
    public string AllowedFileType { get; set; }
    
    public void OnGet()
    {
        AllowedFileType = FileType == ImportUsersFromFileType.Excel ? ".xlsx, .xls" : ".csv";
    }
}