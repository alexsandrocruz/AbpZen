using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Features;
using Volo.Forms.Forms;

namespace Volo.Forms.Questions;

[RequiresFeature(FormsFeatures.Enable)]
public class QuestionAppService : FormsAppServiceBase, IQuestionAppService
{
    protected QuestionManager QuestionManager { get; }
    protected IFormRepository FormRepository { get; }
    protected IQuestionRepository QuestionRepository { get; }

    public QuestionAppService(
        QuestionManager questionManager,
        IFormRepository formRepository,
        IQuestionRepository questionRepository
    )
    {
        QuestionManager = questionManager;
        FormRepository = formRepository;
        QuestionRepository = questionRepository;
    }

    public virtual async Task<List<QuestionDto>> GetListAsync(GetQuestionListDto input)
    {
        //TODO: filter
        var items = await QuestionRepository.GetListAsync();

        return ObjectMapper.Map<List<QuestionBase>, List<QuestionDto>>(items);
    }

    public virtual async Task<QuestionDto> GetAsync(Guid id)
    {
        var item = await QuestionRepository.GetAsync(id, includeDetails: true);

        return ObjectMapper.Map<QuestionBase, QuestionDto>(item);
    }

    public virtual async Task<QuestionDto> UpdateAsync(Guid id, UpdateQuestionDto input)
    {
        var choiceList = new List<(Guid, string, bool)>();

        foreach (var choice in input.Choices)
        {
            if (choice.Id == Guid.Empty)
            {
                choice.Id = GuidGenerator.Create();
            }

            choiceList.Add((choice.Id, choice.Value, choice.IsCorrect));
        }

        var updatedQuestion = await QuestionManager.UpdateAsync(
            id,
            input.Title,
            input.Index,
            input.IsRequired,
            input.Description,
            input.QuestionType,
            input.HasOtherOption,
            choiceList);

        return ObjectMapper.Map<QuestionBase, QuestionDto>(updatedQuestion);
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var item = await QuestionRepository.GetAsync(id);

        await QuestionManager.DeleteAsync(item);
    }
}
