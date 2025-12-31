using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Volo.CmsKit.Faqs;

public class FaqSectionManager : CmsKitProDomainServiceBase
{
    protected IFaqSectionRepository FaqSectionRepository { get; }
    protected FaqOptions FaqOptions { get; }

    public FaqSectionManager(
        IFaqSectionRepository faqSectionRepository,
        IOptions<FaqOptions> options)
    {
        FaqSectionRepository = faqSectionRepository;
        FaqOptions = options.Value;
    }

    public virtual async Task<FaqSection> CreateAsync([NotNull] string groupName, [NotNull] string name)
    {
        if (!FaqOptions.HasGroup(groupName))
        {
            throw new FaqSectionInvalidGroupNameException(groupName);
        }

        if (await FaqSectionRepository.AnyAsync(groupName, name))
        {
            throw new FaqSectionHasAlreadyException(groupName,name);
        }
        
        return new FaqSection(
            GuidGenerator.Create(),
            groupName,
            name 
        );
    }

    public virtual async Task UpdateAsync(FaqSection section, [NotNull] string groupName, [NotNull] string name)
    {
        if (!FaqOptions.HasGroup(groupName))
        {
            throw new FaqSectionInvalidGroupNameException(groupName);
        }

        if ((section.GroupName != groupName || section.Name != name) && await FaqSectionRepository.AnyAsync(groupName, name))
        {
            throw new FaqSectionHasAlreadyException(groupName, name);
        }
        
        section
            .SetGroupName(groupName)
            .SetName(name);
    }
}