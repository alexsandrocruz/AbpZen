using System.ComponentModel;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Polls;

public class IndexModel : AbpPageModel
{
    [DisplayName("Filter")]
    public string Filter { get; set; }

    public IndexModel()
    {

    }

    public async Task OnGetAsync()
    {

    }
}
