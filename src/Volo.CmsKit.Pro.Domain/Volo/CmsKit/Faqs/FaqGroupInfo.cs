using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.Faqs;

public class FaqGroupInfo
{
    [NotNull]
    public virtual string Name { get; set; }
    
    public FaqGroupInfo([NotNull] string name)
    {
        Name =  Check.NotNullOrWhiteSpace(name, nameof(name));
    }
}
