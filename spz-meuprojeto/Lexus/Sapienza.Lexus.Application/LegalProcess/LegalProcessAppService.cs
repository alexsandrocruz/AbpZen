#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.LegalProcess.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.LegalProcess;

/// <summary>
/// Application service for LegalProcess entity
/// </summary>
[Authorize(LegalProcessPermissions.Default)]
public class LegalProcessAppService :
    LexusAppService,
    ILegalProcessAppService
{
    private readonly IRepository<Sapienza.Lexus.LegalProcess.LegalProcess, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.Lawyer.Lawyer, Guid> _lawyerRepository;
    private readonly IRepository<Sapienza.Lexus.Client.Client, Guid> _clientRepository;

    public LegalProcessAppService(
        IRepository<Sapienza.Lexus.LegalProcess.LegalProcess, Guid> repository,
        IRepository<Sapienza.Lexus.Lawyer.Lawyer, Guid> lawyerRepository,
        IRepository<Sapienza.Lexus.Client.Client, Guid> clientRepository
    )
    {
        _repository = repository;
        _lawyerRepository = lawyerRepository;
        _clientRepository = clientRepository;
    }

    /// <summary>
    /// Gets a single LegalProcess by Id
    /// </summary>
    public virtual async Task<LegalProcessDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.LegalProcess.LegalProcess, LegalProcessDto>(entity);
        var lawyer = await _lawyerRepository.FindAsync(entity.LawyerId);
        dto.LawyerDisplayName = lawyer?.FullName;
        var client = await _clientRepository.FindAsync(entity.ClientId);
        dto.ClientDisplayName = client?.Name;

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of LegalProcesses
    /// </summary>
    public virtual async Task<PagedResultDto<LegalProcessDto>> GetListAsync(LegalProcessGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.LegalProcess.LegalProcess>, List<LegalProcessDto>>(entities);
        var lawyerIds = entities
            .Select(x => x.LawyerId)
            .Distinct()
            .ToList();

        if (lawyerIds.Any())
        {
            var parents = await _lawyerRepository.GetListAsync(x => lawyerIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.FullName);

            foreach (var dto in dtoList)
            {
                if (parentMap.TryGetValue(dto.LawyerId, out var displayName))
                {
                    dto.LawyerDisplayName = displayName;
                }
            }
        }
        var clientIds = entities
            .Select(x => x.ClientId)
            .Distinct()
            .ToList();

        if (clientIds.Any())
        {
            var parents = await _clientRepository.GetListAsync(x => clientIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList)
            {
                if (parentMap.TryGetValue(dto.ClientId, out var displayName))
                {
                    dto.ClientDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<LegalProcessDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new LegalProcess
    /// </summary>
    [Authorize(LegalProcessPermissions.Create)]
    public virtual async Task<LegalProcessDto> CreateAsync(CreateUpdateLegalProcessDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLegalProcessDto, Sapienza.Lexus.LegalProcess.LegalProcess>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.LegalProcess.LegalProcess, LegalProcessDto>(entity);
    }

    /// <summary>
    /// Updates an existing LegalProcess
    /// </summary>
    [Authorize(LegalProcessPermissions.Update)]
    public virtual async Task<LegalProcessDto> UpdateAsync(Guid id, CreateUpdateLegalProcessDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.LegalProcess.LegalProcess), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.LegalProcess.LegalProcess, LegalProcessDto>(entity);
    }

    /// <summary>
    /// Deletes a LegalProcess
    /// </summary>
    [Authorize(LegalProcessPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLegalProcessLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.LegalProcess.LegalProcess> ApplyFilters(IQueryable<Sapienza.Lexus.LegalProcess.LegalProcess> queryable, LegalProcessGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>(x.ProcessNumber != null && x.ProcessNumber.Contains(input.Filter)) || (x.Title != null && x.Title.Contains(input.Filter)))
            .WhereIf(!input.ProcessNumber.IsNullOrWhiteSpace(), x => x.ProcessNumber != null && x.ProcessNumber.Contains(input.ProcessNumber))
            .WhereIf(!input.Title.IsNullOrWhiteSpace(), x => x.Title != null && x.Title.Contains(input.Title))
            .WhereIf(input.DateOpened != null, x => x.DateOpened == input.DateOpened)
            .WhereIf(input.LawyerId != null, x => x.LawyerId == input.LawyerId)
            .WhereIf(input.ClientId != null, x => x.ClientId == input.ClientId)
            // ========== FK Filters ==========
            .WhereIf(input.LawyerId != null, x => x.LawyerId == input.LawyerId)
            .WhereIf(input.ClientId != null, x => x.ClientId == input.ClientId)
            ;
    }
}
