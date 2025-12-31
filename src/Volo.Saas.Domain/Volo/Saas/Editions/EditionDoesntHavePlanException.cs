using System;
using Volo.Abp;

namespace Volo.Saas.Editions;

public class EditionDoesntHavePlanException : BusinessException
{
    public EditionDoesntHavePlanException(Guid editionId) : base(code: SaasErrorCodes.Edition.EditionDoesntHavePlan)
    {
        EditionId = editionId;

        WithData(nameof(EditionId), EditionId);
    }

    public virtual Guid EditionId { get; protected set; }
}
