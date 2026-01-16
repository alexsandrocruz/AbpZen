using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProNaturezas.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProNaturezas;

/// <summary>
/// Application service for advProNaturezas entity
/// </summary>
[Authorize(advProNaturezasPermissions.Default)]
public class advProNaturezasAppService :
    LexusAppService,
    IadvProNaturezasAppService
{
    private readonly IRepository<Sapienza.Lexus.advProNaturezas.advProNaturezas, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas, Guid> _advProfissionaisNaturezasRepository;

    public advProNaturezasAppService(
        IRepository<Sapienza.Lexus.advProNaturezas.advProNaturezas, Guid> repository,
        IRepository<Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas, Guid> advProfissionaisNaturezasRepository
    )
    {
        _repository = repository;
        _advProfissionaisNaturezasRepository = advProfissionaisNaturezasRepository;
    }

    /// <summary>
    /// Gets a single advProNaturezas by Id
    /// </summary>
    public virtual async Task<advProNaturezasDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProNaturezas.advProNaturezas, advProNaturezasDto>(entity);
        if (entity.advProfissionaisNaturezasId != null)
        {
            var parent = await _advProfissionaisNaturezasRepository.FindAsync(entity.advProfissionaisNaturezasId.Value);
            dto.advProfissionaisNaturezasDisplayName = parent?.Id.ToString();
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProNaturezases
    /// </summary>
    public virtual async Task<PagedResultDto<advProNaturezasDto>> GetListAsync(advProNaturezasGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProNaturezas.advProNaturezas>, List<advProNaturezasDto>>(entities);
        var advProfissionaisNaturezasIds = entities
            .Where(x => x.advProfissionaisNaturezasId != null)
            .Select(x => x.advProfissionaisNaturezasId.Value)
            .Distinct()
            .ToList();

        if (advProfissionaisNaturezasIds.Any())
        {
            var parents = await _advProfissionaisNaturezasRepository.GetListAsync(x => advProfissionaisNaturezasIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Id.ToString());

            foreach (var dto in dtoList.Where(x => x.advProfissionaisNaturezasId != null))
            {
                if (parentMap.TryGetValue(dto.advProfissionaisNaturezasId.Value, out var displayName))
                {
                    dto.advProfissionaisNaturezasDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<advProNaturezasDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProNaturezas
    /// </summary>
    [Authorize(advProNaturezasPermissions.Create)]
    public virtual async Task<advProNaturezasDto> CreateAsync(CreateUpdateadvProNaturezasDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProNaturezasDto, Sapienza.Lexus.advProNaturezas.advProNaturezas>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProNaturezas.advProNaturezas, advProNaturezasDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProNaturezas
    /// </summary>
    [Authorize(advProNaturezasPermissions.Update)]
    public virtual async Task<advProNaturezasDto> UpdateAsync(Guid id, CreateUpdateadvProNaturezasDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProNaturezas.advProNaturezas), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProNaturezas.advProNaturezas, advProNaturezasDto>(entity);
    }

    /// <summary>
    /// Deletes a advProNaturezas
    /// </summary>
    [Authorize(advProNaturezasPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProNaturezasLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advProNaturezas.advProNaturezas> ApplyFilters(IQueryable<Sapienza.Lexus.advProNaturezas.advProNaturezas> queryable, advProNaturezasGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idNatureza != null, x => x.idNatureza == input.idNatureza)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.mostraHistoricoNumeros != null, x => x.mostraHistoricoNumeros == input.mostraHistoricoNumeros)
            .WhereIf(input.recebeAcordo != null, x => x.recebeAcordo == input.recebeAcordo)
            .WhereIf(input.recebeRPV != null, x => x.recebeRPV == input.recebeRPV)
            .WhereIf(input.recebePrecatorio != null, x => x.recebePrecatorio == input.recebePrecatorio)
            .WhereIf(input.recebeAlvara != null, x => x.recebeAlvara == input.recebeAlvara)
            .WhereIf(input.idArea != null, x => x.idArea == input.idArea)
            // ========== FK Filters ==========
            .WhereIf(input.advProfissionaisNaturezasId != null, x => x.advProfissionaisNaturezasId == input.advProfissionaisNaturezasId)
            ;
    }
}
