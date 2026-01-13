using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProFases.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProFases;

/// <summary>
/// Application service for advProFases entity
/// </summary>
[Authorize(advProFasesPermissions.Default)]
public class advProFasesAppService :
    LexusAppService,
    IadvProFasesAppService
{
    private readonly IRepository<Sapienza.Lexus.advProFases.advProFases, Guid> _repository;

    public advProFasesAppService(
        IRepository<Sapienza.Lexus.advProFases.advProFases, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProFases by Id
    /// </summary>
    public virtual async Task<advProFasesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProFases.advProFases, advProFasesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProFaseses
    /// </summary>
    public virtual async Task<PagedResultDto<advProFasesDto>> GetListAsync(advProFasesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProFases.advProFases>, List<advProFasesDto>>(entities);

        return new PagedResultDto<advProFasesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProFases
    /// </summary>
    [Authorize(advProFasesPermissions.Create)]
    public virtual async Task<advProFasesDto> CreateAsync(CreateUpdateadvProFasesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProFasesDto, Sapienza.Lexus.advProFases.advProFases>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProFases.advProFases, advProFasesDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProFases
    /// </summary>
    [Authorize(advProFasesPermissions.Update)]
    public virtual async Task<advProFasesDto> UpdateAsync(Guid id, CreateUpdateadvProFasesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProFases.advProFases), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProFases.advProFases, advProFasesDto>(entity);
    }

    /// <summary>
    /// Deletes a advProFases
    /// </summary>
    [Authorize(advProFasesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProFasesLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advProFases.advProFases> ApplyFilters(IQueryable<Sapienza.Lexus.advProFases.advProFases> queryable, advProFasesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idFase != null, x => x.idFase == input.idFase)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
