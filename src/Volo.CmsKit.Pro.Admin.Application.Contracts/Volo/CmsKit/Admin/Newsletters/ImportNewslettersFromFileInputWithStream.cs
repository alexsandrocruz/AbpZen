using Volo.Abp.Content;

namespace Volo.CmsKit.Admin.Newsletters;

public class ImportNewslettersFromFileInputWithStream
{
    public IRemoteStreamContent File { get; set; }
}