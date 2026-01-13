using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProRelevancias.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProRelevancias;

/// <summary>
/// Application service for advProRelevancias entity
/// </summary>
[Authorize(advProRelevanciasPermissions.Default)]
public class advProRelevanciasAppService :
    LexusAppService,
    IadvProRelevanciasAppService
{
    private readonly IRepository<Sapienza.Lexus.advProRelevancias.advProRelevancias, Guid> _repository;

    public advProRelevanciasAppService(
        IRepository<Sapienza.Lexus.advProRelevancias.advProRelevancias, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProRelevancias by Id
    /// </summary>
    public virtual async Task<advProRelevanciasDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProRelevancias.advProRelevancias, advProRelevanciasDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProRelevanciases
    /// </summary>
    public virtual async Task<PagedResultDto<advProRelevanciasDto>> GetListAsync(advProRelevanciasGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProRelevancias.advProRelevancias>, List<advProRelevanciasDto>>(entities);

        return new PagedResultDto<advProRelevanciasDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProRelevancias
    /// </summary>
    [Authorize(advProRelevanciasPermissions.Create)]
    public virtual async Task<advProRelevanciasDto> CreateAsync(CreateUpdateadvProRelevanciasDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProRelevanciasDto, Sapienza.Lexus.advProRelevancias.advProRelevancias>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProRelevancias.advProRelevancias, advProRelevanciasDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProRelevancias
    /// </summary>
    [Authorize(advProRelevanciasPermissions.Update)]
    public virtual async Task<advProRelevanciasDto> UpdateAsync(Guid id, CreateUpdateadvProRelevanciasDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProRelevancias.advProRelevancias), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProRelevancias.advProRelevancias, advProRelevanciasDto>(entity);
    }

    /// <summary>
    /// Deletes a advProRelevancias
    /// </summary>
    [Authorize(advProRelevanciasPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProRelevanciasLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advProRelevancias.advProRelevancias> ApplyFilters(IQueryable<Sapienza.Lexus.advProRelevancias.advProRelevancias> queryable, advProRelevanciasGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idRelevancia != null, x => x.idRelevancia == input.idRelevancia)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
