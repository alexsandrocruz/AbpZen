using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advPreMotivosPerda.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advPreMotivosPerda;

/// <summary>
/// Application service for advPreMotivosPerda entity
/// </summary>
[Authorize(advPreMotivosPerdaPermissions.Default)]
public class advPreMotivosPerdaAppService :
    LexusAppService,
    IadvPreMotivosPerdaAppService
{
    private readonly IRepository<Sapienza.Lexus.advPreMotivosPerda.advPreMotivosPerda, Guid> _repository;

    public advPreMotivosPerdaAppService(
        IRepository<Sapienza.Lexus.advPreMotivosPerda.advPreMotivosPerda, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advPreMotivosPerda by Id
    /// </summary>
    public virtual async Task<advPreMotivosPerdaDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advPreMotivosPerda.advPreMotivosPerda, advPreMotivosPerdaDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advPreMotivosPerdas
    /// </summary>
    public virtual async Task<PagedResultDto<advPreMotivosPerdaDto>> GetListAsync(advPreMotivosPerdaGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advPreMotivosPerda.advPreMotivosPerda>, List<advPreMotivosPerdaDto>>(entities);

        return new PagedResultDto<advPreMotivosPerdaDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advPreMotivosPerda
    /// </summary>
    [Authorize(advPreMotivosPerdaPermissions.Create)]
    public virtual async Task<advPreMotivosPerdaDto> CreateAsync(CreateUpdateadvPreMotivosPerdaDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvPreMotivosPerdaDto, Sapienza.Lexus.advPreMotivosPerda.advPreMotivosPerda>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreMotivosPerda.advPreMotivosPerda, advPreMotivosPerdaDto>(entity);
    }

    /// <summary>
    /// Updates an existing advPreMotivosPerda
    /// </summary>
    [Authorize(advPreMotivosPerdaPermissions.Update)]
    public virtual async Task<advPreMotivosPerdaDto> UpdateAsync(Guid id, CreateUpdateadvPreMotivosPerdaDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advPreMotivosPerda.advPreMotivosPerda), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreMotivosPerda.advPreMotivosPerda, advPreMotivosPerdaDto>(entity);
    }

    /// <summary>
    /// Deletes a advPreMotivosPerda
    /// </summary>
    [Authorize(advPreMotivosPerdaPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvPreMotivosPerdaLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advPreMotivosPerda.advPreMotivosPerda> ApplyFilters(IQueryable<Sapienza.Lexus.advPreMotivosPerda.advPreMotivosPerda> queryable, advPreMotivosPerdaGetListInput input)
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
