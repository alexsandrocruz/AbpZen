using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.CmsKit.Faqs;

public class FaqQuestionManager : CmsKitProDomainServiceBase
{
    protected IFaqQuestionRepository FaqQuestionRepository { get; }
    protected IFaqSectionRepository FaqSectionRepository { get; }

    public FaqQuestionManager(
        IFaqQuestionRepository faqQuestionRepository,
        IFaqSectionRepository faqSectionRepository)
    {
        FaqQuestionRepository = faqQuestionRepository;
        FaqSectionRepository = faqSectionRepository;
    }

    public virtual async Task<FaqQuestion> CreateAsync(
        Guid sectionId,
        [NotNull] string title,
        [NotNull] string text,
        int order)
    {
        if (await FaqQuestionRepository.AnyAsync(sectionId, title))
        {
            throw new FaqQuestionHasAlreadyException(title);
        }

        if (!await FaqSectionRepository.AnyAsync(sectionId))
        {
            throw new FaqQuestionSectionNotFound();
        }
        
        return new FaqQuestion(
            GuidGenerator.Create(),
            sectionId,
            title,
            text,
            order
        );
    }

    public virtual async Task UpdateTitle(FaqQuestion question, [NotNull] string title)
    {
        if (question.Title != title && await FaqQuestionRepository.AnyAsync(question.SectionId, title))
        {
            throw new FaqQuestionHasAlreadyException(title);
        }

        question.SetTitle(title);
    }
}