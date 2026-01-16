using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.fabPermissoes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fabPermissoes;

/// <summary>
/// Application service for fabPermissoes entity
/// </summary>
[Authorize(fabPermissoesPermissions.Default)]
public class fabPermissoesAppService :
    LexusAppService,
    IfabPermissoesAppService
{
    private readonly IRepository<Sapienza.Lexus.fabPermissoes.fabPermissoes, Guid> _repository;

    public fabPermissoesAppService(
        IRepository<Sapienza.Lexus.fabPermissoes.fabPermissoes, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single fabPermissoes by Id
    /// </summary>
    public virtual async Task<fabPermissoesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.fabPermissoes.fabPermissoes, fabPermissoesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of fabPermissoeses
    /// </summary>
    public virtual async Task<PagedResultDto<fabPermissoesDto>> GetListAsync(fabPermissoesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.fabPermissoes.fabPermissoes>, List<fabPermissoesDto>>(entities);

        return new PagedResultDto<fabPermissoesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new fabPermissoes
    /// </summary>
    [Authorize(fabPermissoesPermissions.Create)]
    public virtual async Task<fabPermissoesDto> CreateAsync(CreateUpdatefabPermissoesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefabPermissoesDto, Sapienza.Lexus.fabPermissoes.fabPermissoes>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabPermissoes.fabPermissoes, fabPermissoesDto>(entity);
    }

    /// <summary>
    /// Updates an existing fabPermissoes
    /// </summary>
    [Authorize(fabPermissoesPermissions.Update)]
    public virtual async Task<fabPermissoesDto> UpdateAsync(Guid id, CreateUpdatefabPermissoesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.fabPermissoes.fabPermissoes), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabPermissoes.fabPermissoes, fabPermissoesDto>(entity);
    }

    /// <summary>
    /// Deletes a fabPermissoes
    /// </summary>
    [Authorize(fabPermissoesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfabPermissoesLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.descricao
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.fabPermissoes.fabPermissoes> ApplyFilters(IQueryable<Sapienza.Lexus.fabPermissoes.fabPermissoes> queryable, fabPermissoesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.descricao.Contains(input.Filter) || x.varSession.Contains(input.Filter))
            .WhereIf(input.PermissionId != null, x => x.PermissionId == input.PermissionId)
            .WhereIf(input.idPermissaoTipo != null, x => x.idPermissaoTipo == input.idPermissaoTipo)
            .WhereIf(input.modulo != null, x => x.modulo == input.modulo)
            .WhereIf(!input.descricao.IsNullOrWhiteSpace(), x => x.descricao.Contains(input.descricao))
            .WhereIf(!input.varSession.IsNullOrWhiteSpace(), x => x.varSession.Contains(input.varSession))
            // ========== FK Filters ==========
            ;
    }
}
