using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.TransactionAttachment;

public class EfTransactionAttachmentRepository 
    : EfCoreRepository<DominusDbContext, Dominus.TransactionAttachment.TransactionAttachment, Guid>, 
      ITransactionAttachmentRepository
{
    public EfTransactionAttachmentRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
