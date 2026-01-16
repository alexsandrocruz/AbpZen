using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.fabHistoricoTipos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fabHistoricoTipos;

/// <summary>
/// Application service for fabHistoricoTipos entity
/// </summary>
[Authorize(fabHistoricoTiposPermissions.Default)]
public class fabHistoricoTiposAppService :
    LexusAppService,
    IfabHistoricoTiposAppService
{
    private readonly IRepository<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes, Guid> _flwConfigExcecoesRepository;
    private readonly IRepository<Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios, Guid> _flwGradeHorariosRepository;
    private readonly IRepository<Sapienza.Lexus.flwFollows.flwFollows, Guid> _flwFollowsRepository;

    public fabHistoricoTiposAppService(
        IRepository<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos, Guid> repository,
        IRepository<Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes, Guid> flwConfigExcecoesRepository,
        IRepository<Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios, Guid> flwGradeHorariosRepository,
        IRepository<Sapienza.Lexus.flwFollows.flwFollows, Guid> flwFollowsRepository
    )
    {
        _repository = repository;
        _flwConfigExcecoesRepository = flwConfigExcecoesRepository;
        _flwGradeHorariosRepository = flwGradeHorariosRepository;
        _flwFollowsRepository = flwFollowsRepository;
    }

    /// <summary>
    /// Gets a single fabHistoricoTipos by Id
    /// </summary>
    public virtual async Task<fabHistoricoTiposDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos, fabHistoricoTiposDto>(entity);
        if (entity.flwConfigExcecoesId != null)
        {
            var parent = await _flwConfigExcecoesRepository.FindAsync(entity.flwConfigExcecoesId.Value);
            dto.flwConfigExcecoesDisplayName = parent?.tipoMarcacoes;
        }
        if (entity.flwGradeHorariosId != null)
        {
            var parent = await _flwGradeHorariosRepository.FindAsync(entity.flwGradeHorariosId.Value);
            dto.flwGradeHorariosDisplayName = parent?.Id.ToString();
        }
        if (entity.flwFollowsId != null)
        {
            var parent = await _flwFollowsRepository.FindAsync(entity.flwFollowsId.Value);
            dto.flwFollowsDisplayName = parent?.data;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of fabHistoricoTiposes
    /// </summary>
    public virtual async Task<PagedResultDto<fabHistoricoTiposDto>> GetListAsync(fabHistoricoTiposGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos>, List<fabHistoricoTiposDto>>(entities);
        var flwConfigExcecoesIds = entities
            .Where(x => x.flwConfigExcecoesId != null)
            .Select(x => x.flwConfigExcecoesId.Value)
            .Distinct()
            .ToList();

        if (flwConfigExcecoesIds.Any())
        {
            var parents = await _flwConfigExcecoesRepository.GetListAsync(x => flwConfigExcecoesIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.tipoMarcacoes);

            foreach (var dto in dtoList.Where(x => x.flwConfigExcecoesId != null))
            {
                if (parentMap.TryGetValue(dto.flwConfigExcecoesId.Value, out var displayName))
                {
                    dto.flwConfigExcecoesDisplayName = displayName;
                }
            }
        }
        var flwGradeHorariosIds = entities
            .Where(x => x.flwGradeHorariosId != null)
            .Select(x => x.flwGradeHorariosId.Value)
            .Distinct()
            .ToList();

        if (flwGradeHorariosIds.Any())
        {
            var parents = await _flwGradeHorariosRepository.GetListAsync(x => flwGradeHorariosIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Id.ToString());

            foreach (var dto in dtoList.Where(x => x.flwGradeHorariosId != null))
            {
                if (parentMap.TryGetValue(dto.flwGradeHorariosId.Value, out var displayName))
                {
                    dto.flwGradeHorariosDisplayName = displayName;
                }
            }
        }
        var flwFollowsIds = entities
            .Where(x => x.flwFollowsId != null)
            .Select(x => x.flwFollowsId.Value)
            .Distinct()
            .ToList();

        if (flwFollowsIds.Any())
        {
            var parents = await _flwFollowsRepository.GetListAsync(x => flwFollowsIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.data);

            foreach (var dto in dtoList.Where(x => x.flwFollowsId != null))
            {
                if (parentMap.TryGetValue(dto.flwFollowsId.Value, out var displayName))
                {
                    dto.flwFollowsDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<fabHistoricoTiposDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new fabHistoricoTipos
    /// </summary>
    [Authorize(fabHistoricoTiposPermissions.Create)]
    public virtual async Task<fabHistoricoTiposDto> CreateAsync(CreateUpdatefabHistoricoTiposDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefabHistoricoTiposDto, Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos, fabHistoricoTiposDto>(entity);
    }

    /// <summary>
    /// Updates an existing fabHistoricoTipos
    /// </summary>
    [Authorize(fabHistoricoTiposPermissions.Update)]
    public virtual async Task<fabHistoricoTiposDto> UpdateAsync(Guid id, CreateUpdatefabHistoricoTiposDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos, fabHistoricoTiposDto>(entity);
    }

    /// <summary>
    /// Deletes a fabHistoricoTipos
    /// </summary>
    [Authorize(fabHistoricoTiposPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfabHistoricoTiposLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos> ApplyFilters(IQueryable<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos> queryable, fabHistoricoTiposGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter) || x.tipoMarcacoes.Contains(input.Filter))
            .WhereIf(input.idHistoricoTipo != null, x => x.idHistoricoTipo == input.idHistoricoTipo)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(!input.tipoMarcacoes.IsNullOrWhiteSpace(), x => x.tipoMarcacoes.Contains(input.tipoMarcacoes))
            // ========== FK Filters ==========
            .WhereIf(input.flwConfigExcecoesId != null, x => x.flwConfigExcecoesId == input.flwConfigExcecoesId)
            .WhereIf(input.flwGradeHorariosId != null, x => x.flwGradeHorariosId == input.flwGradeHorariosId)
            .WhereIf(input.flwFollowsId != null, x => x.flwFollowsId == input.flwFollowsId)
            ;
    }
}
