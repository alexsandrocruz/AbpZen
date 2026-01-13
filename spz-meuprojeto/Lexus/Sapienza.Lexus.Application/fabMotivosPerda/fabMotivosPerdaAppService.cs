using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.fabMotivosPerda.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fabMotivosPerda;

/// <summary>
/// Application service for fabMotivosPerda entity
/// </summary>
[Authorize(fabMotivosPerdaPermissions.Default)]
public class fabMotivosPerdaAppService :
    LexusAppService,
    IfabMotivosPerdaAppService
{
    private readonly IRepository<Sapienza.Lexus.fabMotivosPerda.fabMotivosPerda, Guid> _repository;

    public fabMotivosPerdaAppService(
        IRepository<Sapienza.Lexus.fabMotivosPerda.fabMotivosPerda, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single fabMotivosPerda by Id
    /// </summary>
    public virtual async Task<fabMotivosPerdaDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.fabMotivosPerda.fabMotivosPerda, fabMotivosPerdaDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of fabMotivosPerdas
    /// </summary>
    public virtual async Task<PagedResultDto<fabMotivosPerdaDto>> GetListAsync(fabMotivosPerdaGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.fabMotivosPerda.fabMotivosPerda>, List<fabMotivosPerdaDto>>(entities);

        return new PagedResultDto<fabMotivosPerdaDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new fabMotivosPerda
    /// </summary>
    [Authorize(fabMotivosPerdaPermissions.Create)]
    public virtual async Task<fabMotivosPerdaDto> CreateAsync(CreateUpdatefabMotivosPerdaDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefabMotivosPerdaDto, Sapienza.Lexus.fabMotivosPerda.fabMotivosPerda>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabMotivosPerda.fabMotivosPerda, fabMotivosPerdaDto>(entity);
    }

    /// <summary>
    /// Updates an existing fabMotivosPerda
    /// </summary>
    [Authorize(fabMotivosPerdaPermissions.Update)]
    public virtual async Task<fabMotivosPerdaDto> UpdateAsync(Guid id, CreateUpdatefabMotivosPerdaDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.fabMotivosPerda.fabMotivosPerda), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabMotivosPerda.fabMotivosPerda, fabMotivosPerdaDto>(entity);
    }

    /// <summary>
    /// Deletes a fabMotivosPerda
    /// </summary>
    [Authorize(fabMotivosPerdaPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfabMotivosPerdaLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.fabMotivosPerda.fabMotivosPerda> ApplyFilters(IQueryable<Sapienza.Lexus.fabMotivosPerda.fabMotivosPerda> queryable, fabMotivosPerdaGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idMotivo != null, x => x.idMotivo == input.idMotivo)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
