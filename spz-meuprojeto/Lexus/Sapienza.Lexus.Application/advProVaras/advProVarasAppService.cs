using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProVaras.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProVaras;

/// <summary>
/// Application service for advProVaras entity
/// </summary>
[Authorize(advProVarasPermissions.Default)]
public class advProVarasAppService :
    LexusAppService,
    IadvProVarasAppService
{
    private readonly IRepository<Sapienza.Lexus.advProVaras.advProVaras, Guid> _repository;

    public advProVarasAppService(
        IRepository<Sapienza.Lexus.advProVaras.advProVaras, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProVaras by Id
    /// </summary>
    public virtual async Task<advProVarasDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProVaras.advProVaras, advProVarasDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProVarases
    /// </summary>
    public virtual async Task<PagedResultDto<advProVarasDto>> GetListAsync(advProVarasGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProVaras.advProVaras>, List<advProVarasDto>>(entities);

        return new PagedResultDto<advProVarasDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProVaras
    /// </summary>
    [Authorize(advProVarasPermissions.Create)]
    public virtual async Task<advProVarasDto> CreateAsync(CreateUpdateadvProVarasDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProVarasDto, Sapienza.Lexus.advProVaras.advProVaras>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProVaras.advProVaras, advProVarasDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProVaras
    /// </summary>
    [Authorize(advProVarasPermissions.Update)]
    public virtual async Task<advProVarasDto> UpdateAsync(Guid id, CreateUpdateadvProVarasDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProVaras.advProVaras), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProVaras.advProVaras, advProVarasDto>(entity);
    }

    /// <summary>
    /// Deletes a advProVaras
    /// </summary>
    [Authorize(advProVarasPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProVarasLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advProVaras.advProVaras> ApplyFilters(IQueryable<Sapienza.Lexus.advProVaras.advProVaras> queryable, advProVarasGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idVara != null, x => x.idVara == input.idVara)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
