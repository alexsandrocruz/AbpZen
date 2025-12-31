using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Saas.Tenants;

public class Tenant : FullAuditedAggregateRoot<Guid>, IHasEntityVersion
{
    public virtual string Name { get; protected set; }

    public virtual string NormalizedName { get; protected set; }

    public virtual Guid? EditionId { get; set; }

    public virtual DateTime? EditionEndDateUtc { get; set; }

    public virtual List<TenantConnectionString> ConnectionStrings { get; protected set; }

    public virtual TenantActivationState ActivationState { get; protected set; }

    public virtual DateTime? ActivationEndDate { get; protected set; }

    public virtual int EntityVersion { get; protected set; }

    protected Tenant()
    {

    }

    protected internal Tenant(Guid id, [NotNull] string name, [CanBeNull] string normalizedName, Guid? editionId = null)
    {
        Id = id;
        SetName(name);
        SetNormalizedName(normalizedName);
        EditionId = editionId;

        ConnectionStrings = new List<TenantConnectionString>();
    }

    protected virtual internal void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), TenantConsts.MaxNameLength);
    }

    protected virtual internal void SetNormalizedName([CanBeNull] string normalizedName)
    {
        NormalizedName = Check.NotNullOrWhiteSpace(normalizedName, nameof(normalizedName), TenantConsts.MaxNameLength);;
    }

    [CanBeNull]
    public virtual string FindDefaultConnectionString()
    {
        return FindConnectionString(Abp.Data.ConnectionStrings.DefaultConnectionStringName);
    }

    [CanBeNull]
    public virtual string FindConnectionString(string name)
    {
        return ConnectionStrings.FirstOrDefault(c => c.Name == name)?.Value;
    }

    public virtual void SetDefaultConnectionString(string connectionString)
    {
        SetConnectionString(Abp.Data.ConnectionStrings.DefaultConnectionStringName, connectionString);
    }

    public virtual void SetConnectionString(string name, string connectionString)
    {
        var tenantConnectionString = ConnectionStrings.FirstOrDefault(x => x.Name == name);

        if (tenantConnectionString != null)
        {
            if (tenantConnectionString.Value != connectionString)
            {
                tenantConnectionString.SetValue(connectionString);
            }
        }
        else
        {
            ConnectionStrings.Add(new TenantConnectionString(Id, name, connectionString));
        }
    }

    public virtual void RemoveDefaultConnectionString()
    {
        RemoveConnectionString(Abp.Data.ConnectionStrings.DefaultConnectionStringName);
    }

    public virtual void RemoveConnectionString(string name)
    {
        var tenantConnectionString = ConnectionStrings.FirstOrDefault(x => x.Name == name);

        if (tenantConnectionString != null)
        {
            ConnectionStrings.Remove(tenantConnectionString);
        }
    }

    public virtual void SetActivationState(TenantActivationState activationState)
    {
        ActivationState = activationState;

        //clear ActivationEndDate
        if (ActivationState != TenantActivationState.ActiveWithLimitedTime)
        {
            ActivationEndDate = null;
        }
    }

    public virtual void SetActivationEndDate(DateTime? activationEndDate)
    {
        if (ActivationState == TenantActivationState.ActiveWithLimitedTime)
        {
            ActivationEndDate = activationEndDate;
            return;
        }

        //TODO:
        //throw new BusinessException("");
    }

    public virtual Guid? GetActiveEditionId()
    {
        if (!EditionEndDateUtc.HasValue)
        {
            return EditionId;
        }

        if (EditionEndDateUtc >= DateTime.UtcNow)
        {
            return EditionId;
        }

        return null;
    }
}
