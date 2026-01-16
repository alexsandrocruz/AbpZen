using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advAgeTiposTarefas.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advAgeTiposTarefas;

/// <summary>
/// Application service for advAgeTiposTarefas entity
/// </summary>
[Authorize(advAgeTiposTarefasPermissions.Default)]
public class advAgeTiposTarefasAppService :
    LexusAppService,
    IadvAgeTiposTarefasAppService
{
    private readonly IRepository<Sapienza.Lexus.advAgeTiposTarefas.advAgeTiposTarefas, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.advTarefas.advTarefas, Guid> _advTarefasRepository;

    public advAgeTiposTarefasAppService(
        IRepository<Sapienza.Lexus.advAgeTiposTarefas.advAgeTiposTarefas, Guid> repository,
        IRepository<Sapienza.Lexus.advTarefas.advTarefas, Guid> advTarefasRepository
    )
    {
        _repository = repository;
        _advTarefasRepository = advTarefasRepository;
    }

    /// <summary>
    /// Gets a single advAgeTiposTarefas by Id
    /// </summary>
    public virtual async Task<advAgeTiposTarefasDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advAgeTiposTarefas.advAgeTiposTarefas, advAgeTiposTarefasDto>(entity);
        if (entity.advTarefasId != null)
        {
            var parent = await _advTarefasRepository.FindAsync(entity.advTarefasId.Value);
            dto.advTarefasDisplayName = parent?.dataCadastro;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advAgeTiposTarefases
    /// </summary>
    public virtual async Task<PagedResultDto<advAgeTiposTarefasDto>> GetListAsync(advAgeTiposTarefasGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advAgeTiposTarefas.advAgeTiposTarefas>, List<advAgeTiposTarefasDto>>(entities);
        var advTarefasIds = entities
            .Where(x => x.advTarefasId != null)
            .Select(x => x.advTarefasId.Value)
            .Distinct()
            .ToList();

        if (advTarefasIds.Any())
        {
            var parents = await _advTarefasRepository.GetListAsync(x => advTarefasIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.dataCadastro);

            foreach (var dto in dtoList.Where(x => x.advTarefasId != null))
            {
                if (parentMap.TryGetValue(dto.advTarefasId.Value, out var displayName))
                {
                    dto.advTarefasDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<advAgeTiposTarefasDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advAgeTiposTarefas
    /// </summary>
    [Authorize(advAgeTiposTarefasPermissions.Create)]
    public virtual async Task<advAgeTiposTarefasDto> CreateAsync(CreateUpdateadvAgeTiposTarefasDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvAgeTiposTarefasDto, Sapienza.Lexus.advAgeTiposTarefas.advAgeTiposTarefas>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advAgeTiposTarefas.advAgeTiposTarefas, advAgeTiposTarefasDto>(entity);
    }

    /// <summary>
    /// Updates an existing advAgeTiposTarefas
    /// </summary>
    [Authorize(advAgeTiposTarefasPermissions.Update)]
    public virtual async Task<advAgeTiposTarefasDto> UpdateAsync(Guid id, CreateUpdateadvAgeTiposTarefasDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advAgeTiposTarefas.advAgeTiposTarefas), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advAgeTiposTarefas.advAgeTiposTarefas, advAgeTiposTarefasDto>(entity);
    }

    /// <summary>
    /// Deletes a advAgeTiposTarefas
    /// </summary>
    [Authorize(advAgeTiposTarefasPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvAgeTiposTarefasLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advAgeTiposTarefas.advAgeTiposTarefas> ApplyFilters(IQueryable<Sapienza.Lexus.advAgeTiposTarefas.advAgeTiposTarefas> queryable, advAgeTiposTarefasGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idTipoTarefa != null, x => x.idTipoTarefa == input.idTipoTarefa)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.agendada != null, x => x.agendada == input.agendada)
            .WhereIf(input.pauta != null, x => x.pauta == input.pauta)
            // ========== FK Filters ==========
            .WhereIf(input.advTarefasId != null, x => x.advTarefasId == input.advTarefasId)
            ;
    }
}
