using System;
using JetBrains.Annotations;

namespace Volo.CmsKit.Public.Faqs;

[Serializable]
public class FaqSectionListFilterInput
{
    public string GroupName { get; set; }

    [CanBeNull]
    public string SectionName { get; set; }
}
