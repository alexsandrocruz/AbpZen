using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finCentrosResultado.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finCentrosResultado;

/// <summary>
/// Application service for finCentrosResultado entity
/// </summary>
[Authorize(finCentrosResultadoPermissions.Default)]
public class finCentrosResultadoAppService :
    LexusAppService,
    IfinCentrosResultadoAppService
{
    private readonly IRepository<Sapienza.Lexus.finCentrosResultado.finCentrosResultado, Guid> _repository;

    public finCentrosResultadoAppService(
        IRepository<Sapienza.Lexus.finCentrosResultado.finCentrosResultado, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single finCentrosResultado by Id
    /// </summary>
    public virtual async Task<finCentrosResultadoDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finCentrosResultado.finCentrosResultado, finCentrosResultadoDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finCentrosResultados
    /// </summary>
    public virtual async Task<PagedResultDto<finCentrosResultadoDto>> GetListAsync(finCentrosResultadoGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finCentrosResultado.finCentrosResultado>, List<finCentrosResultadoDto>>(entities);

        return new PagedResultDto<finCentrosResultadoDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finCentrosResultado
    /// </summary>
    [Authorize(finCentrosResultadoPermissions.Create)]
    public virtual async Task<finCentrosResultadoDto> CreateAsync(CreateUpdatefinCentrosResultadoDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinCentrosResultadoDto, Sapienza.Lexus.finCentrosResultado.finCentrosResultado>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finCentrosResultado.finCentrosResultado, finCentrosResultadoDto>(entity);
    }

    /// <summary>
    /// Updates an existing finCentrosResultado
    /// </summary>
    [Authorize(finCentrosResultadoPermissions.Update)]
    public virtual async Task<finCentrosResultadoDto> UpdateAsync(Guid id, CreateUpdatefinCentrosResultadoDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finCentrosResultado.finCentrosResultado), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finCentrosResultado.finCentrosResultado, finCentrosResultadoDto>(entity);
    }

    /// <summary>
    /// Deletes a finCentrosResultado
    /// </summary>
    [Authorize(finCentrosResultadoPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinCentrosResultadoLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.finCentrosResultado.finCentrosResultado> ApplyFilters(IQueryable<Sapienza.Lexus.finCentrosResultado.finCentrosResultado> queryable, finCentrosResultadoGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idCentroResultado != null, x => x.idCentroResultado == input.idCentroResultado)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.padrao != null, x => x.padrao == input.padrao)
            // ========== FK Filters ==========
            ;
    }
}
