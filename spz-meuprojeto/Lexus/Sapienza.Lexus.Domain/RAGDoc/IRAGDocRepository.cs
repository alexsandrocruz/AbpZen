using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.RAGDoc;

public interface IRAGDocRepository : IRepository<Sapienza.Lexus.RAGDoc.RAGDoc, Guid>
{
}
