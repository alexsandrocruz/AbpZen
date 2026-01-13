using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProStatus.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProStatus;

/// <summary>
/// Application service for advProStatus entity
/// </summary>
[Authorize(advProStatusPermissions.Default)]
public class advProStatusAppService :
    LexusAppService,
    IadvProStatusAppService
{
    private readonly IRepository<Sapienza.Lexus.advProStatus.advProStatus, Guid> _repository;

    public advProStatusAppService(
        IRepository<Sapienza.Lexus.advProStatus.advProStatus, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProStatus by Id
    /// </summary>
    public virtual async Task<advProStatusDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProStatus.advProStatus, advProStatusDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProStatuses
    /// </summary>
    public virtual async Task<PagedResultDto<advProStatusDto>> GetListAsync(advProStatusGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProStatus.advProStatus>, List<advProStatusDto>>(entities);

        return new PagedResultDto<advProStatusDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProStatus
    /// </summary>
    [Authorize(advProStatusPermissions.Create)]
    public virtual async Task<advProStatusDto> CreateAsync(CreateUpdateadvProStatusDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProStatusDto, Sapienza.Lexus.advProStatus.advProStatus>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProStatus.advProStatus, advProStatusDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProStatus
    /// </summary>
    [Authorize(advProStatusPermissions.Update)]
    public virtual async Task<advProStatusDto> UpdateAsync(Guid id, CreateUpdateadvProStatusDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProStatus.advProStatus), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProStatus.advProStatus, advProStatusDto>(entity);
    }

    /// <summary>
    /// Deletes a advProStatus
    /// </summary>
    [Authorize(advProStatusPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProStatusLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.titulo
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advProStatus.advProStatus> ApplyFilters(IQueryable<Sapienza.Lexus.advProStatus.advProStatus> queryable, advProStatusGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idStatus != null, x => x.idStatus == input.idStatus)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
