using System.Collections.Generic;

namespace Volo.CmsKit.Polls;

public class CmsKitPollingOptions
{
    public List<string> WidgetNames { get; set; }

    public CmsKitPollingOptions()
    {
        WidgetNames = new List<string>();
    }

    public void AddWidget(string name)
    {
        if (WidgetNames.Contains(name))
        {
            throw new PollOptionWidgetNameCannotBeSameException(name);
        }
        
        WidgetNames.Add(name);
    }
}