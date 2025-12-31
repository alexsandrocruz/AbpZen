using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Gdpr;

public class GdprRequest : AggregateRoot<Guid>, IHasCreationTime
{
    public Guid UserId { get; protected set;  }

    public DateTime CreationTime { get; protected set; }

    public DateTime ReadyTime { get; protected set; }
    
    public ICollection<GdprInfo> Infos { get; protected set; }

    protected GdprRequest()
    {
        
    }

    public GdprRequest(
        Guid id, 
        Guid userId, 
        DateTime readyTime)
        : base(id)
    {
        UserId = userId;
        ReadyTime = readyTime;
        Infos = new Collection<GdprInfo>();
    }

    public void AddData(Guid gdprInfoId, [NotNull] string data, [NotNull] string provider)
    {
        Infos.Add(new GdprInfo(gdprInfoId, requestId: Id, data, provider));
    }
}