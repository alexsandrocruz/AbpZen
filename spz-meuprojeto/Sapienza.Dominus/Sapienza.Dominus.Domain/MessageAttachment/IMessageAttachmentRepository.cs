using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.MessageAttachment;

public interface IMessageAttachmentRepository : IRepository<Dominus.MessageAttachment.MessageAttachment, Guid>
{
}
