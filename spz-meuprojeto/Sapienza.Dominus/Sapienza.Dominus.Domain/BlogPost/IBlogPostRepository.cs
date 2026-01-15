using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.BlogPost;

public interface IBlogPostRepository : IRepository<Dominus.BlogPost.BlogPost, Guid>
{
}
