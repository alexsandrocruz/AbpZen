using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.PageFeedbacks;

public class PageFeedback : AggregateRoot<Guid>, IMultiTenant, IHasCreationTime
{
    public virtual string EntityType { get; protected set; }
    public virtual string EntityId { get; protected set; }
    public virtual string Url { get; protected set; }
    public virtual bool IsUseful { get; set; }
    public virtual string UserNote { get; protected set; }
    public virtual string AdminNote { get; protected set; }
    public virtual bool IsHandled { get; set; }
    public virtual Guid? TenantId { get; protected set; }
    public virtual DateTime CreationTime { get; protected set;}
    
    public virtual Guid FeedbackUserId { get; protected set; }

    protected PageFeedback()
    {
    }

    internal PageFeedback(
        Guid id,
        [NotNull] string entityType,
        string entityId,
        string url,
        bool isUseful,
        string userNote,
        string adminNote = null,
        bool isHandled = false,
        Guid? tenantId = null,
        Guid feedbackUserId = default
    ) : base(id)
    {
        SetEntityType(entityType);
        SetEntityId(entityId);
        SetUrl(url);
        IsUseful = isUseful;
        SetUserNote(userNote);
        SetAdminNote(adminNote);
        IsHandled = isHandled;
        TenantId = tenantId;
        FeedbackUserId = feedbackUserId;
    }

    protected virtual PageFeedback SetEntityType([NotNull] string entityType)
    {
        EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType), PageFeedbackConst.MaxEntityTypeLength);
        return this;
    }
    
    protected virtual PageFeedback SetEntityId(string entityId)
    {
        EntityId = Check.Length(entityId, nameof(entityId), PageFeedbackConst.MaxEntityIdLength);
        return this;
    }
    
    protected virtual PageFeedback SetUrl(string url)
    {
        Url = Check.Length(url, nameof(url), PageFeedbackConst.MaxUrlLength);
        return this;
    }
    
    protected virtual PageFeedback SetUserNote(string userNote)
    {
        UserNote = Check.Length(userNote, nameof(userNote), PageFeedbackConst.MaxUserNoteLength);
        return this;
    }
    
    public virtual PageFeedback SetAdminNote(string adminNote)
    {
        AdminNote = Check.Length(adminNote, nameof(adminNote), PageFeedbackConst.MaxAdminNoteLength);
        return this;
    }
    
    public virtual PageFeedback InitializeUserNote(string userNote, bool? isUseful = null)
    {
        if (!UserNote.IsNullOrWhiteSpace())
        {
            return this;
        }

        SetUserNote(userNote);
        if (isUseful.HasValue)
        {
            IsUseful = isUseful.Value;
        }

        return this;
    }
}