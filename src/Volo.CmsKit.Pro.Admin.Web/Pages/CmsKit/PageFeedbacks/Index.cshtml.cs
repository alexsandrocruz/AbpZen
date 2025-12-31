namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.PageFeedbacks;

public class IndexModel : AdminPageModel
{
    public bool? IsHandled { get; set; }
    public bool? IsUseful { get; set; }
    public string UrlFilter { get; set; }
    public string EntityType { get; set; }
    public string EntityId { get; set; }
    public bool? HasUserNote { get; set; }
    public bool? HasAdminNote { get; set; }

    public void OnGet()
    {
        
    }
}