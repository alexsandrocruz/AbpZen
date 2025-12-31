namespace Volo.CmsKit.Admin.Newsletters;

public class ImportNewslettersFromFileOutput
{
    public int AllCount { get; set; }

    public int SucceededCount { get; set; }

    public int FailedCount { get; set; }
    
    public string InvalidNewslettersDownloadToken { get; set; }
    
    public bool IsAllSucceeded => AllCount == SucceededCount;
}