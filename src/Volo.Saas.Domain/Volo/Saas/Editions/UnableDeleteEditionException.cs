using System;
using Volo.Abp;

namespace Volo.Saas.Editions;

[Serializable]
public class UnableDeleteEditionException : BusinessException
{
    public UnableDeleteEditionException(string editionName)
        : base(code: SaasErrorCodes.Edition.UnableDeleteEdition)
    {
        EditionName = editionName;

        WithData(nameof(EditionName), EditionName);
    }

    public virtual string EditionName { get; protected set; }
}
