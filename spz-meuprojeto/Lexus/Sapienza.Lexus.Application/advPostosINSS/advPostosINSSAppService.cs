using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advPostosINSS.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advPostosINSS;

/// <summary>
/// Application service for advPostosINSS entity
/// </summary>
[Authorize(advPostosINSSPermissions.Default)]
public class advPostosINSSAppService :
    LexusAppService,
    IadvPostosINSSAppService
{
    private readonly IRepository<Sapienza.Lexus.advPostosINSS.advPostosINSS, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.advClientes.advClientes, Guid> _advClientesRepository;

    public advPostosINSSAppService(
        IRepository<Sapienza.Lexus.advPostosINSS.advPostosINSS, Guid> repository,
        IRepository<Sapienza.Lexus.advClientes.advClientes, Guid> advClientesRepository
    )
    {
        _repository = repository;
        _advClientesRepository = advClientesRepository;
    }

    /// <summary>
    /// Gets a single advPostosINSS by Id
    /// </summary>
    public virtual async Task<advPostosINSSDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advPostosINSS.advPostosINSS, advPostosINSSDto>(entity);
        var advClientes = await _advClientesRepository.FindAsync(entity.advClientesId);
        dto.advClientesDisplayName = advClientes?.apelido;

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advPostosINSSes
    /// </summary>
    public virtual async Task<PagedResultDto<advPostosINSSDto>> GetListAsync(advPostosINSSGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advPostosINSS.advPostosINSS>, List<advPostosINSSDto>>(entities);
        var advClientesIds = entities
            .Select(x => x.advClientesId)
            .Distinct()
            .ToList();

        if (advClientesIds.Any())
        {
            var parents = await _advClientesRepository.GetListAsync(x => advClientesIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.apelido);

            foreach (var dto in dtoList)
            {
                if (parentMap.TryGetValue(dto.advClientesId, out var displayName))
                {
                    dto.advClientesDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<advPostosINSSDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advPostosINSS
    /// </summary>
    [Authorize(advPostosINSSPermissions.Create)]
    public virtual async Task<advPostosINSSDto> CreateAsync(CreateUpdateadvPostosINSSDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvPostosINSSDto, Sapienza.Lexus.advPostosINSS.advPostosINSS>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPostosINSS.advPostosINSS, advPostosINSSDto>(entity);
    }

    /// <summary>
    /// Updates an existing advPostosINSS
    /// </summary>
    [Authorize(advPostosINSSPermissions.Update)]
    public virtual async Task<advPostosINSSDto> UpdateAsync(Guid id, CreateUpdateadvPostosINSSDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advPostosINSS.advPostosINSS), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPostosINSS.advPostosINSS, advPostosINSSDto>(entity);
    }

    /// <summary>
    /// Deletes a advPostosINSS
    /// </summary>
    [Authorize(advPostosINSSPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvPostosINSSLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advPostosINSS.advPostosINSS> ApplyFilters(IQueryable<Sapienza.Lexus.advPostosINSS.advPostosINSS> queryable, advPostosINSSGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idPosto != null, x => x.idPosto == input.idPosto)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            .WhereIf(input.advClientesId != null, x => x.advClientesId == input.advClientesId)
            ;
    }
}
