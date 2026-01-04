using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeptonXDemoApp.Permissions;
using LeptonXDemoApp.MessageTemplate.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.MessageTemplate;

/// <summary>
/// Application service for MessageTemplate entity
/// </summary>
[Authorize(LeptonXDemoAppPermissions.MessageTemplate.Default)]
public class MessageTemplateAppService :
    LeptonXDemoAppAppService,
    IMessageTemplateAppService
{
    private readonly IRepository<LeptonXDemoApp.MessageTemplate.MessageTemplate, Guid> _repository;

    public MessageTemplateAppService(
        IRepository<LeptonXDemoApp.MessageTemplate.MessageTemplate, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single MessageTemplate by Id
    /// </summary>
    public virtual async Task<MessageTemplateDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<LeptonXDemoApp.MessageTemplate.MessageTemplate, MessageTemplateDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of MessageTemplates
    /// </summary>
    public virtual async Task<PagedResultDto<MessageTemplateDto>> GetListAsync(MessageTemplateGetListInput input)
    {
        var queryable = await _repository.GetQueryableAsync();

        // Apply filters
        queryable = ApplyFilters(queryable, input);

        // Apply default sorting (by CreationTime descending)
        queryable = queryable.OrderByDescending(e => e.CreationTime);

        // Get total count
        var totalCount = await AsyncExecuter.CountAsync(queryable);

        // Apply paging
        queryable = queryable.PageBy(input.SkipCount, input.MaxResultCount);

        var entities = await AsyncExecuter.ToListAsync(queryable);
        var dtoList = ObjectMapper.Map<List<LeptonXDemoApp.MessageTemplate.MessageTemplate>, List<MessageTemplateDto>>(entities);

        return new PagedResultDto<MessageTemplateDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new MessageTemplate
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.MessageTemplate.Create)]
    public virtual async Task<MessageTemplateDto> CreateAsync(CreateUpdateMessageTemplateDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateMessageTemplateDto, LeptonXDemoApp.MessageTemplate.MessageTemplate>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.MessageTemplate.MessageTemplate, MessageTemplateDto>(entity);
    }

    /// <summary>
    /// Updates an existing MessageTemplate
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.MessageTemplate.Update)]
    public virtual async Task<MessageTemplateDto> UpdateAsync(Guid id, CreateUpdateMessageTemplateDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(LeptonXDemoApp.MessageTemplate.MessageTemplate), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.MessageTemplate.MessageTemplate, MessageTemplateDto>(entity);
    }

    /// <summary>
    /// Deletes a MessageTemplate
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.MessageTemplate.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetMessageTemplateLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Title
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<LeptonXDemoApp.MessageTemplate.MessageTemplate> ApplyFilters(IQueryable<LeptonXDemoApp.MessageTemplate.MessageTemplate> queryable, MessageTemplateGetListInput input)
    {
        return queryable
            .WhereIf(!input.Title.IsNullOrWhiteSpace(), x => x.Title.Contains(input.Title))
            .WhereIf(!input.Body.IsNullOrWhiteSpace(), x => x.Body.Contains(input.Body))
            .WhereIf(input.MessageType != null, x => x.MessageType == input.MessageType)
            // ========== FK Filters ==========
            ;
    }
}
