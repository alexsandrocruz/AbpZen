using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeptonXDemoApp.Permissions;
using LeptonXDemoApp.LeadContact.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.LeadContact;

/// <summary>
/// Application service for LeadContact entity
/// </summary>
[Authorize(LeptonXDemoAppPermissions.LeadContact.Default)]
public class LeadContactAppService :
    LeptonXDemoAppAppService,
    ILeadContactAppService
{
    private readonly IRepository<LeptonXDemoApp.LeadContact.LeadContact, Guid> _repository;
    private readonly IRepository<LeptonXDemoApp.Lead.Lead, Guid> _leadRepository;

    public LeadContactAppService(
        IRepository<LeptonXDemoApp.LeadContact.LeadContact, Guid> repository,
        IRepository<LeptonXDemoApp.Lead.Lead, Guid> leadRepository
    )
    {
        _repository = repository;
        _leadRepository = leadRepository;
    }

    /// <summary>
    /// Gets a single LeadContact by Id
    /// </summary>
    public virtual async Task<LeadContactDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<LeptonXDemoApp.LeadContact.LeadContact, LeadContactDto>(entity);
        if (entity.LeadId != null)
        {
            var parent = await _leadRepository.FindAsync(entity.LeadId.Value);
            dto.LeadDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of LeadContacts
    /// </summary>
    public virtual async Task<PagedResultDto<LeadContactDto>> GetListAsync(LeadContactGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<LeptonXDemoApp.LeadContact.LeadContact>, List<LeadContactDto>>(entities);
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

        return new PagedResultDto<LeadContactDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new LeadContact
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.LeadContact.Create)]
    public virtual async Task<LeadContactDto> CreateAsync(CreateUpdateLeadContactDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLeadContactDto, LeptonXDemoApp.LeadContact.LeadContact>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.LeadContact.LeadContact, LeadContactDto>(entity);
    }

    /// <summary>
    /// Updates an existing LeadContact
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.LeadContact.Update)]
    public virtual async Task<LeadContactDto> UpdateAsync(Guid id, CreateUpdateLeadContactDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(LeptonXDemoApp.LeadContact.LeadContact), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.LeadContact.LeadContact, LeadContactDto>(entity);
    }

    /// <summary>
    /// Deletes a LeadContact
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.LeadContact.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLeadContactLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Name
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<LeptonXDemoApp.LeadContact.LeadContact> ApplyFilters(IQueryable<LeptonXDemoApp.LeadContact.LeadContact> queryable, LeadContactGetListInput input)
    {
        return queryable
            .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name))
            .WhereIf(!input.Position.IsNullOrWhiteSpace(), x => x.Position.Contains(input.Position))
            .WhereIf(!input.Email.IsNullOrWhiteSpace(), x => x.Email.Contains(input.Email))
            .WhereIf(!input.Phone.IsNullOrWhiteSpace(), x => x.Phone.Contains(input.Phone))
            .WhereIf(input.LeadId != null, x => x.LeadId == input.LeadId)
            // ========== FK Filters ==========
            .WhereIf(input.LeadId != null, x => x.LeadId == input.LeadId)
            ;
    }
}
