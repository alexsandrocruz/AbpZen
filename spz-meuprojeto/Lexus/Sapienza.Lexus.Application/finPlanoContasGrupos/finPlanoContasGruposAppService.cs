using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finPlanoContasGrupos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finPlanoContasGrupos;

/// <summary>
/// Application service for finPlanoContasGrupos entity
/// </summary>
[Authorize(finPlanoContasGruposPermissions.Default)]
public class finPlanoContasGruposAppService :
    LexusAppService,
    IfinPlanoContasGruposAppService
{
    private readonly IRepository<Sapienza.Lexus.finPlanoContasGrupos.finPlanoContasGrupos, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.finPlanoContas.finPlanoContas, Guid> _finPlanoContasRepository;

    public finPlanoContasGruposAppService(
        IRepository<Sapienza.Lexus.finPlanoContasGrupos.finPlanoContasGrupos, Guid> repository,
        IRepository<Sapienza.Lexus.finPlanoContas.finPlanoContas, Guid> finPlanoContasRepository
    )
    {
        _repository = repository;
        _finPlanoContasRepository = finPlanoContasRepository;
    }

    /// <summary>
    /// Gets a single finPlanoContasGrupos by Id
    /// </summary>
    public virtual async Task<finPlanoContasGruposDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finPlanoContasGrupos.finPlanoContasGrupos, finPlanoContasGruposDto>(entity);
        if (entity.finPlanoContasId != null)
        {
            var parent = await _finPlanoContasRepository.FindAsync(entity.finPlanoContasId.Value);
            dto.finPlanoContasDisplayName = parent?.titulo;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finPlanoContasGruposes
    /// </summary>
    public virtual async Task<PagedResultDto<finPlanoContasGruposDto>> GetListAsync(finPlanoContasGruposGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finPlanoContasGrupos.finPlanoContasGrupos>, List<finPlanoContasGruposDto>>(entities);
        var finPlanoContasIds = entities
            .Where(x => x.finPlanoContasId != null)
            .Select(x => x.finPlanoContasId.Value)
            .Distinct()
            .ToList();

        if (finPlanoContasIds.Any())
        {
            var parents = await _finPlanoContasRepository.GetListAsync(x => finPlanoContasIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.titulo);

            foreach (var dto in dtoList.Where(x => x.finPlanoContasId != null))
            {
                if (parentMap.TryGetValue(dto.finPlanoContasId.Value, out var displayName))
                {
                    dto.finPlanoContasDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<finPlanoContasGruposDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finPlanoContasGrupos
    /// </summary>
    [Authorize(finPlanoContasGruposPermissions.Create)]
    public virtual async Task<finPlanoContasGruposDto> CreateAsync(CreateUpdatefinPlanoContasGruposDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinPlanoContasGruposDto, Sapienza.Lexus.finPlanoContasGrupos.finPlanoContasGrupos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finPlanoContasGrupos.finPlanoContasGrupos, finPlanoContasGruposDto>(entity);
    }

    /// <summary>
    /// Updates an existing finPlanoContasGrupos
    /// </summary>
    [Authorize(finPlanoContasGruposPermissions.Update)]
    public virtual async Task<finPlanoContasGruposDto> UpdateAsync(Guid id, CreateUpdatefinPlanoContasGruposDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finPlanoContasGrupos.finPlanoContasGrupos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finPlanoContasGrupos.finPlanoContasGrupos, finPlanoContasGruposDto>(entity);
    }

    /// <summary>
    /// Deletes a finPlanoContasGrupos
    /// </summary>
    [Authorize(finPlanoContasGruposPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinPlanoContasGruposLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.finPlanoContasGrupos.finPlanoContasGrupos> ApplyFilters(IQueryable<Sapienza.Lexus.finPlanoContasGrupos.finPlanoContasGrupos> queryable, finPlanoContasGruposGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter) || x.tipo.Contains(input.Filter))
            .WhereIf(input.idGrupo != null, x => x.idGrupo == input.idGrupo)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(!input.tipo.IsNullOrWhiteSpace(), x => x.tipo.Contains(input.tipo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.idGrupoDRE != null, x => x.idGrupoDRE == input.idGrupoDRE)
            .WhereIf(input.ordem != null, x => x.ordem == input.ordem)
            // ========== FK Filters ==========
            .WhereIf(input.finPlanoContasId != null, x => x.finPlanoContasId == input.finPlanoContasId)
            ;
    }
}
