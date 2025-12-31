using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.Faqs;

public class FaqOptions
{
    private FrozenDictionary<string, FaqGroupInfo> _groups { get; set; }
    public IReadOnlyDictionary<string, FaqGroupInfo> Groups
        => _groups ??= new Dictionary<string, FaqGroupInfo>().ToFrozenDictionary();

    public FaqOptions()
    {
        _groups = new Dictionary<string, FaqGroupInfo>().ToFrozenDictionary();
    }
    
    public virtual void SetGroups([NotNull] ReadOnlySpan<string> names)
    {
        var groups = new Dictionary<string, FaqGroupInfo>();
        foreach (var name in names)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            if (HasGroup(name))
            {
                throw new InvalidOperationException("Group with the same name already exists: " + name);
            }

            groups.Add(name, new FaqGroupInfo(name));
        }
        
        _groups = groups.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
    }
    
    public virtual bool HasGroup([NotNull] string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        
        return Groups.ContainsKey(name);
    }
}