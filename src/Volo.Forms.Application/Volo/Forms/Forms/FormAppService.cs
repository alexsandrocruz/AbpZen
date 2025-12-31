using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Content;
using Volo.Abp.Data;
using Volo.Abp.Emailing;
using Volo.Abp.Features;
using CsvHelper;
using CsvHelper.Configuration;
using Volo.Abp.MultiTenancy;
using Volo.Forms.Permissions;
using Volo.Forms.Questions;
using Volo.Forms.Responses;

namespace Volo.Forms.Forms;

[RequiresFeature(FormsFeatures.Enable)]
[Authorize(FormsPermissions.Forms.Default)]
public class FormAppService : FormsAppServiceBase, IFormAppService
{
    protected IFormRepository FormRepository { get; }
    protected IQuestionRepository QuestionRepository { get; }
    protected IResponseRepository ResponseRepository { get; }
    protected QuestionManager QuestionManager { get; }
    protected IEmailSender EmailSender { get; }

    public FormAppService(
        QuestionManager questionManager,
        IFormRepository formRepository,
        IQuestionRepository questionRepository,
        IResponseRepository responseRepository,
        IEmailSender emailSender)
    {
        FormRepository = formRepository;
        QuestionRepository = questionRepository;
        QuestionManager = questionManager;
        ResponseRepository = responseRepository;
        EmailSender = emailSender;
    }

    public virtual async Task<PagedResultDto<FormDto>> GetListAsync(GetFormListInputDto input)
    {
        var formList = await FormRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);
        var totalFormCount = await FormRepository.GetCountAsync(input.Filter);

        return new PagedResultDto<FormDto>(totalFormCount, ObjectMapper.Map<List<Form>, List<FormDto>>(formList));
    }

    public virtual async Task<PagedResultDto<FormResponseDetailedDto>> GetResponsesAsync(Guid id, GetResponseListInputDto input)
    {
        using (DataFilter.Disable<IMultiTenant>())
        {
            var responses = await ResponseRepository.GetListByFormIdAsync(
                id,
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Filter);

            var totalCount = await ResponseRepository.GetCountByFormIdAsync(id, input.Filter);

            return new PagedResultDto<FormResponseDetailedDto>(totalCount,
                ObjectMapper.Map<List<FormResponse>, List<FormResponseDetailedDto>>(responses));
        }
    }

    public virtual async Task<IRemoteStreamContent> GetCsvResponsesAsync(Guid id, GetResponseListInputDto input)
    {
        if (input.MaxResultCount == 0)
        {
            input.MaxResultCount = int.MaxValue;
        }
        
        FormWithQuestions form;
        List<FormResponse> responses;
        List<QuestionBase> questions;
        List<string> headers;
        using (DataFilter.Disable<IMultiTenant>())
        {
            responses = await ResponseRepository.GetListByFormIdAsync(
                id,
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Filter);

            form = await FormRepository.GetWithQuestionsAsync(id, includeChoices: true);
            questions = form.Questions.OrderBy(q => q.Index).ToList();
            headers = questions.Select(q => q.Title).ToList();
        }

        headers.AddFirst("Date");

        var csvConfiguration =
            new CsvConfiguration(new CultureInfo(CultureInfo.CurrentUICulture.Name)) {
                ShouldQuote = arg => true
            };

        using (var memoryStream = new MemoryStream())
        {
            using (var streamWriter = new StreamWriter(stream: memoryStream, encoding: new UTF8Encoding(true)))
            {
                using (var csvWriter = new CsvWriter(streamWriter, csvConfiguration))
                {
                    foreach (var header in headers)
                    {
                        csvWriter.WriteField(header);
                    }

                    for (var i = 0; i < responses.Count; i++)
                    {
                        var response = responses[i];
                        if (i == 0)
                        {
                            await csvWriter.NextRecordAsync();
                        }

                        var date = response.LastModificationTime ?? response.CreationTime;
                        csvWriter.WriteField(date.ToString("yyyy-MM-dd HH:mm:ss"));

                        foreach (var question in questions)
                        {
                            var questionResponse =
                                response.Answers.FirstOrDefault(x => x.QuestionId == question.Id);

                            csvWriter.WriteField(questionResponse?.Value ?? string.Empty);
                        }

                        await csvWriter.NextRecordAsync();
                    }

                    await streamWriter.FlushAsync();
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    var ms = new MemoryStream();
                    await memoryStream.CopyToAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);

                    return new RemoteStreamContent(ms, $"{form.Form.Title}.csv", "text/csv");
                }
            }
        }
    }

    [AllowAnonymous]
    public virtual async Task<long> GetResponsesCountAsync(Guid id)
    {
        using (DataFilter.Disable<IMultiTenant>())
        {
            return await ResponseRepository.GetCountByFormIdAsync(id);
        }
    }

    public virtual async Task<FormDto> CreateAsync(CreateFormDto input)
    {
        var form = new Form(GuidGenerator.Create(), input.Title, input.Description, tenantId: CurrentTenant.Id);

        await FormRepository.InsertAsync(form);

        return ObjectMapper.Map<Form, FormDto>(form);
    }

    [AllowAnonymous]
    public virtual async Task<FormWithDetailsDto> GetAsync(Guid id)
    {
        FormWithQuestions result;
        using (DataFilter.Disable<IMultiTenant>())
        {
            result = await FormRepository.GetWithQuestionsAsync(id, includeChoices: true);
        }

        return ObjectMapper.Map<FormWithQuestions, FormWithDetailsDto>(result);
    }

    public virtual async Task<FormDto> UpdateAsync(Guid id, UpdateFormDto input)
    {
        var form = await FormRepository.GetAsync(id); //GetFormByIdBasedOnTenantAsync(id);

        form.SetTitle(input.Title);
        form.SetDescription(input.Description);
        form.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        await FormRepository.UpdateAsync(form);

        return ObjectMapper.Map<Form, FormDto>(form);
    }

    public virtual async Task SetSettingsAsync(Guid id, UpdateFormSettingInputDto input)
    {
        var form = await FormRepository.GetAsync(id); //GetFormByIdBasedOnTenantAsync(id);

        form.SetSettings(
            input.CanEditResponse,
            input.IsCollectingEmail,
            input.HasLimitOneResponsePerUser,
            input.IsAcceptingResponses,
            input.IsQuiz,
            input.RequiresLogin);

        await FormRepository.UpdateAsync(form);
    }

    [AllowAnonymous]
    public virtual async Task<FormSettingsDto> GetSettingsAsync(Guid formId)
    {
        var form = await GetFormWithoutTenantFilterAsync(formId);

        return ObjectMapper.Map<Form, FormSettingsDto>(form);
    }

    [AllowAnonymous]
    public virtual async Task<List<QuestionDto>> GetQuestionsAsync(Guid id, GetQuestionListDto input)
    {
        var form = await GetFormWithoutTenantFilterAsync(id);

        if (form.RequiresLogin && !CurrentUser.IsAuthenticated)
        {
            throw new AbpAuthorizationException();
        }

        using (DataFilter.Disable<IMultiTenant>())
        {
            var questionItems = await QuestionRepository.GetListByFormIdAsync(form.Id);
            return ObjectMapper.Map<List<QuestionBase>, List<QuestionDto>>(questionItems);
        }
    }

    public virtual async Task<QuestionDto> CreateQuestionAsync(Guid id, CreateQuestionDto input)
    {
        var form = await FormRepository.GetAsync(id); //GetFormByIdBasedOnTenantAsync(id);

        var choiceList = new List<(string, bool)>();

        foreach (var choice in input.Choices)
        {
            choiceList.Add((choice.Value, choice.IsCorrect));
        }

        return ObjectMapper.Map<QuestionBase, QuestionDto>(
            await QuestionManager.CreateQuestionAsync(
                form,
                input.QuestionType,
                input.Index,
                input.IsRequired,
                input.Title,
                input.Description,
                input.HasOtherOption,
                choiceList)
        );
    }

    [Authorize(FormsPermissions.Forms.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return FormRepository.DeleteAsync(id);
    }

    [Authorize(FormsPermissions.Response.Delete)]
    public virtual async Task DeleteAllResponsesOfFormAsync(Guid id)
    {
        var formResponseIds = (await ResponseRepository.GetListByFormIdAsync(id)).Select(q => q.Id);

        foreach (var formResponseId in formResponseIds)
        {
            await ResponseRepository.DeleteAsync(formResponseId);
        }
    }

    public virtual async Task SendInviteEmailAsync(FormInviteEmailInputDto input)
    {
        await EmailSender.SendAsync(input.To, input.Subject, input.Body);
    }

    private async Task<Form> GetFormByIdBasedOnTenantAsync(Guid id)
    {
        if (CurrentTenant.IsAvailable)
        {
            return await FormRepository.GetAsync(id);
        }

        return await GetFormWithoutTenantFilterAsync(id);
    }

    private Task<Form> GetFormWithoutTenantFilterAsync(Guid id)
    {
        using (DataFilter.Disable<IMultiTenant>())
        {
            return FormRepository.GetAsync(id);
        }
    }
}
