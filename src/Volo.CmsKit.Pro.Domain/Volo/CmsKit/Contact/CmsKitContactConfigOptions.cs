using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Volo.Abp;

namespace Volo.CmsKit.Contact;
public class CmsKitContactConfigOptions
{
    public Dictionary<string, ContactConfig> ContactConfigs { get; }

    public CmsKitContactConfigOptions()
    {
        ContactConfigs = new();
    }

    public void AddContact(string name, string receiverEmailAddress)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        
        var config = new ContactConfig(receiverEmailAddress);
        ContactConfigs.Add(name, config);
    }
}
