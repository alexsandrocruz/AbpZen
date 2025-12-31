using System;
using System.Collections.Generic;

namespace Volo.CmsKit.PageFeedbacks;

public class PageFeedbackEntityTypeDefinition : PolicySpecifiedDefinition, IEquatable<PageFeedbackEntityTypeDefinition>
{
    public PageFeedbackEntityTypeDefinition(string entityType,
        IEnumerable<string> createPolicies = null,
        IEnumerable<string> updatePolicies = null,
        IEnumerable<string> deletePolicies = null)
        : base(entityType, createPolicies, updatePolicies, deletePolicies)
    {
    }

    public bool Equals(PageFeedbackEntityTypeDefinition other)
    {
        return other?.EntityType == EntityType;
    }
}