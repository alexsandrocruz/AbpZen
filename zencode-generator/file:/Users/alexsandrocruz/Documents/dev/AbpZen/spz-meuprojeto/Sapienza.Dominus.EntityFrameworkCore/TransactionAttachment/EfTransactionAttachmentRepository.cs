using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.TransactionAttachment;

public class EfTransactionAttachmentRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.TransactionAttachment.TransactionAttachment, Guid>, 
      ITransactionAttachmentRepository
{
    public EfTransactionAttachmentRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
