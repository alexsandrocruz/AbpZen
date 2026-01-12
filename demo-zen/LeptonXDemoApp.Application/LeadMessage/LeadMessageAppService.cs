using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeptonXDemoApp.Permissions;
using LeptonXDemoApp.LeadMessage.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.LeadMessage;

/// <summary>
/// Application service for LeadMessage entity
/// </summary>
[Authorize(LeptonXDemoAppPermissions.LeadMessage.Default)]
public class LeadMessageAppService :
    LeptonXDemoAppAppService,
    ILeadMessageAppService
{
    private readonly IRepository<LeptonXDemoApp.LeadMessage.LeadMessage, Guid> _repository;
    private readonly IRepository<LeptonXDemoApp.MessageTemplate.MessageTemplate, Guid> _messageTemplateRepository;
    private readonly IRepository<LeptonXDemoApp.Lead.Lead, Guid> _leadRepository;

    public LeadMessageAppService(
        IRepository<LeptonXDemoApp.LeadMessage.LeadMessage, Guid> repository,
        IRepository<LeptonXDemoApp.MessageTemplate.MessageTemplate, Guid> messageTemplateRepository,
        IRepository<LeptonXDemoApp.Lead.Lead, Guid> leadRepository
    )
    {
        _repository = repository;
        _messageTemplateRepository = messageTemplateRepository;
        _leadRepository = leadRepository;
    }

    /// <summary>
    /// Gets a single LeadMessage by Id
    /// </summary>
    public virtual async Task<LeadMessageDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<LeptonXDemoApp.LeadMessage.LeadMessage, LeadMessageDto>(entity);
        if (entity.MessageTemplateId != null)
        {
            var parent = await _messageTemplateRepository.FindAsync(entity.MessageTemplateId.Value);
            dto.MessageTemplateDisplayName = parent?.Title;
        }
        if (entity.LeadId != null)
        {
            var parent = await _leadRepository.FindAsync(entity.LeadId.Value);
            dto.LeadDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of LeadMessages
    /// </summary>
    public virtual async Task<PagedResultDto<LeadMessageDto>> GetListAsync(LeadMessageGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<LeptonXDemoApp.LeadMessage.LeadMessage>, List<LeadMessageDto>>(entities);
        var messageTemplateIds = entities
            .Where(x => x.MessageTemplateId != null)
            .Select(x => x.MessageTemplateId.Value)
            .Distinct()
            .ToList();

        if (messageTemplateIds.Any())
        {
            var parents = await _messageTemplateRepository.GetListAsync(x => messageTemplateIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Title);

            foreach (var dto in dtoList.Where(x => x.MessageTemplateId != null))
            {
                if (parentMap.TryGetValue(dto.MessageTemplateId.Value, out var displayName))
                {
                    dto.MessageTemplateDisplayName = displayName;
                }
            }
        }
        var leadIds = entities
            .Where(x => x.LeadId != null)
            .Select(x => x.LeadId.Value)
            .Distinct()
            .ToList();

        if (leadIds.Any())
        {
            var parents = await _leadRepository.GetListAsync(x => leadIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.LeadId != null))
            {
                if (parentMap.TryGetValue(dto.LeadId.Value, out var displayName))
                {
                    dto.LeadDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<LeadMessageDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new LeadMessage
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.LeadMessage.Create)]
    public virtual async Task<LeadMessageDto> CreateAsync(CreateUpdateLeadMessageDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLeadMessageDto, LeptonXDemoApp.LeadMessage.LeadMessage>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.LeadMessage.LeadMessage, LeadMessageDto>(entity);
    }

    /// <summary>
    /// Updates an existing LeadMessage
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.LeadMessage.Update)]
    public virtual async Task<LeadMessageDto> UpdateAsync(Guid id, CreateUpdateLeadMessageDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(LeptonXDemoApp.LeadMessage.LeadMessage), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.LeadMessage.LeadMessage, LeadMessageDto>(entity);
    }

    /// <summary>
    /// Deletes a LeadMessage
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.LeadMessage.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLeadMessageLookupAsync()
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
    protected virtual IQueryable<LeptonXDemoApp.LeadMessage.LeadMessage> ApplyFilters(IQueryable<LeptonXDemoApp.LeadMessage.LeadMessage> queryable, LeadMessageGetListInput input)
    {
        return queryable
            .WhereIf(!input.Title.IsNullOrWhiteSpace(), x => x.Title.Contains(input.Title))
            .WhereIf(input.MessageTemplateId != null, x => x.MessageTemplateId == input.MessageTemplateId)
            .WhereIf(input.Date != null, x => x.Date == input.Date)
            .WhereIf(input.LeadId != null, x => x.LeadId == input.LeadId)
            .WhereIf(!input.Body.IsNullOrWhiteSpace(), x => x.Body.Contains(input.Body))
            .WhereIf(input.Success != null, x => x.Success == input.Success)
            // ========== FK Filters ==========
            .WhereIf(input.MessageTemplateId != null, x => x.MessageTemplateId == input.MessageTemplateId)
            .WhereIf(input.LeadId != null, x => x.LeadId == input.LeadId)
            ;
    }
}
