using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.BlogPost;

public interface IBlogPostRepository : IRepository<Sapienza.Dominus.BlogPost.BlogPost, Guid>
{
}
