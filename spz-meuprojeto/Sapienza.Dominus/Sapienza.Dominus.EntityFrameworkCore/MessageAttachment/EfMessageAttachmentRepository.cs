using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.MessageAttachment;

public class EfMessageAttachmentRepository 
    : EfCoreRepository<DominusDbContext, Dominus.MessageAttachment.MessageAttachment, Guid>, 
      IMessageAttachmentRepository
{
    public EfMessageAttachmentRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
