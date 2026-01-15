using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.TransactionAttachment;

public interface ITransactionAttachmentRepository : IRepository<Dominus.TransactionAttachment.TransactionAttachment, Guid>
{
}
