using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advCliGrupos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advCliGrupos;

/// <summary>
/// Application service for advCliGrupos entity
/// </summary>
[Authorize(advCliGruposPermissions.Default)]
public class advCliGruposAppService :
    LexusAppService,
    IadvCliGruposAppService
{
    private readonly IRepository<Sapienza.Lexus.advCliGrupos.advCliGrupos, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.advClientes.advClientes, Guid> _advClientesRepository;

    public advCliGruposAppService(
        IRepository<Sapienza.Lexus.advCliGrupos.advCliGrupos, Guid> repository,
        IRepository<Sapienza.Lexus.advClientes.advClientes, Guid> advClientesRepository
    )
    {
        _repository = repository;
        _advClientesRepository = advClientesRepository;
    }

    /// <summary>
    /// Gets a single advCliGrupos by Id
    /// </summary>
    public virtual async Task<advCliGruposDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advCliGrupos.advCliGrupos, advCliGruposDto>(entity);
        if (entity.advClientesId != null)
        {
            var parent = await _advClientesRepository.FindAsync(entity.advClientesId.Value);
            dto.advClientesDisplayName = parent?.apelido;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advCliGruposes
    /// </summary>
    public virtual async Task<PagedResultDto<advCliGruposDto>> GetListAsync(advCliGruposGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advCliGrupos.advCliGrupos>, List<advCliGruposDto>>(entities);
        var advClientesIds = entities
            .Where(x => x.advClientesId != null)
            .Select(x => x.advClientesId.Value)
            .Distinct()
            .ToList();

        if (advClientesIds.Any())
        {
            var parents = await _advClientesRepository.GetListAsync(x => advClientesIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.apelido);

            foreach (var dto in dtoList.Where(x => x.advClientesId != null))
            {
                if (parentMap.TryGetValue(dto.advClientesId.Value, out var displayName))
                {
                    dto.advClientesDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<advCliGruposDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advCliGrupos
    /// </summary>
    [Authorize(advCliGruposPermissions.Create)]
    public virtual async Task<advCliGruposDto> CreateAsync(CreateUpdateadvCliGruposDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvCliGruposDto, Sapienza.Lexus.advCliGrupos.advCliGrupos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliGrupos.advCliGrupos, advCliGruposDto>(entity);
    }

    /// <summary>
    /// Updates an existing advCliGrupos
    /// </summary>
    [Authorize(advCliGruposPermissions.Update)]
    public virtual async Task<advCliGruposDto> UpdateAsync(Guid id, CreateUpdateadvCliGruposDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advCliGrupos.advCliGrupos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliGrupos.advCliGrupos, advCliGruposDto>(entity);
    }

    /// <summary>
    /// Deletes a advCliGrupos
    /// </summary>
    [Authorize(advCliGruposPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvCliGruposLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advCliGrupos.advCliGrupos> ApplyFilters(IQueryable<Sapienza.Lexus.advCliGrupos.advCliGrupos> queryable, advCliGruposGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idGrupo != null, x => x.idGrupo == input.idGrupo)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            .WhereIf(input.advClientesId != null, x => x.advClientesId == input.advClientesId)
            ;
    }
}
