using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.LeadScheduledMessage.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadScheduledMessage;

/// <summary>
/// Application service for LeadScheduledMessage entity
/// </summary>
[Authorize(LeadScheduledMessagePermissions.Default)]
public class LeadScheduledMessageAppService :
    DominusAppService,
    ILeadScheduledMessageAppService
{
    private readonly IRepository<Dominus.LeadScheduledMessage.LeadScheduledMessage, Guid> _repository;
    private readonly IRepository<Dominus.LeadAutomation.LeadAutomation, Guid> _leadAutomationRepository;

    public LeadScheduledMessageAppService(
        IRepository<Dominus.LeadScheduledMessage.LeadScheduledMessage, Guid> repository,
        IRepository<Dominus.LeadAutomation.LeadAutomation, Guid> leadAutomationRepository
    )
    {
        _repository = repository;
        _leadAutomationRepository = leadAutomationRepository;
    }

    /// <summary>
    /// Gets a single LeadScheduledMessage by Id
    /// </summary>
    public virtual async Task<LeadScheduledMessageDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.LeadScheduledMessage.LeadScheduledMessage, LeadScheduledMessageDto>(entity);
        if (entity.LeadAutomationId != null)
        {
            var parent = await _leadAutomationRepository.FindAsync(entity.LeadAutomationId.Value);
            dto.LeadAutomationDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of LeadScheduledMessages
    /// </summary>
    public virtual async Task<PagedResultDto<LeadScheduledMessageDto>> GetListAsync(LeadScheduledMessageGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.LeadScheduledMessage.LeadScheduledMessage>, List<LeadScheduledMessageDto>>(entities);
        var leadAutomationIds = entities
            .Where(x => x.LeadAutomationId != null)
            .Select(x => x.LeadAutomationId.Value)
            .Distinct()
            .ToList();

        if (leadAutomationIds.Any())
        {
            var parents = await _leadAutomationRepository.GetListAsync(x => leadAutomationIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.LeadAutomationId != null))
            {
                if (parentMap.TryGetValue(dto.LeadAutomationId.Value, out var displayName))
                {
                    dto.LeadAutomationDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<LeadScheduledMessageDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new LeadScheduledMessage
    /// </summary>
    [Authorize(LeadScheduledMessagePermissions.Create)]
    public virtual async Task<LeadScheduledMessageDto> CreateAsync(CreateUpdateLeadScheduledMessageDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLeadScheduledMessageDto, Dominus.LeadScheduledMessage.LeadScheduledMessage>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadScheduledMessage.LeadScheduledMessage, LeadScheduledMessageDto>(entity);
    }

    /// <summary>
    /// Updates an existing LeadScheduledMessage
    /// </summary>
    [Authorize(LeadScheduledMessagePermissions.Update)]
    public virtual async Task<LeadScheduledMessageDto> UpdateAsync(Guid id, CreateUpdateLeadScheduledMessageDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.LeadScheduledMessage.LeadScheduledMessage), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadScheduledMessage.LeadScheduledMessage, LeadScheduledMessageDto>(entity);
    }

    /// <summary>
    /// Deletes a LeadScheduledMessage
    /// </summary>
    [Authorize(LeadScheduledMessagePermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLeadScheduledMessageLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Status
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.LeadScheduledMessage.LeadScheduledMessage> ApplyFilters(IQueryable<Dominus.LeadScheduledMessage.LeadScheduledMessage> queryable, LeadScheduledMessageGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.LeadAutomationId != null, x => x.LeadAutomationId == input.LeadAutomationId)
            ;
    }
}
