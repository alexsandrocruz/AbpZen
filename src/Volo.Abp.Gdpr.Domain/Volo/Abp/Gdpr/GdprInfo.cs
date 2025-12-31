using System;
using System.Collections.Generic;
using System.Text.Json;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Gdpr;

public class GdprInfo : Entity<Guid>
{
    public Guid RequestId { get; protected set; }

    /// <summary>
    /// Used to store GDPR related info as JSON
    /// </summary>
    public string Data { get; protected set; }
   
    public string Provider { get; protected set;  }

    protected GdprInfo()
    {
    }

    public GdprInfo(
        Guid id,
        Guid requestId,
        [NotNull] string data,
        [NotNull] string provider) 
        : base(id)
    {
        RequestId = requestId;
        Data = Check.NotNullOrWhiteSpace(data, nameof(data));
        Provider = Check.NotNullOrWhiteSpace(provider, nameof(provider), GdprInfoConsts.MaxProviderLength);
    }
}