using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.usuPermissoes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.usuPermissoes;

/// <summary>
/// Application service for usuPermissoes entity
/// </summary>
[Authorize(usuPermissoesPermissions.Default)]
public class usuPermissoesAppService :
    LexusAppService,
    IusuPermissoesAppService
{
    private readonly IRepository<Sapienza.Lexus.usuPermissoes.usuPermissoes, Guid> _repository;

    public usuPermissoesAppService(
        IRepository<Sapienza.Lexus.usuPermissoes.usuPermissoes, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single usuPermissoes by Id
    /// </summary>
    public virtual async Task<usuPermissoesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.usuPermissoes.usuPermissoes, usuPermissoesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of usuPermissoeses
    /// </summary>
    public virtual async Task<PagedResultDto<usuPermissoesDto>> GetListAsync(usuPermissoesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.usuPermissoes.usuPermissoes>, List<usuPermissoesDto>>(entities);

        return new PagedResultDto<usuPermissoesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new usuPermissoes
    /// </summary>
    [Authorize(usuPermissoesPermissions.Create)]
    public virtual async Task<usuPermissoesDto> CreateAsync(CreateUpdateusuPermissoesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateusuPermissoesDto, Sapienza.Lexus.usuPermissoes.usuPermissoes>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.usuPermissoes.usuPermissoes, usuPermissoesDto>(entity);
    }

    /// <summary>
    /// Updates an existing usuPermissoes
    /// </summary>
    [Authorize(usuPermissoesPermissions.Update)]
    public virtual async Task<usuPermissoesDto> UpdateAsync(Guid id, CreateUpdateusuPermissoesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.usuPermissoes.usuPermissoes), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.usuPermissoes.usuPermissoes, usuPermissoesDto>(entity);
    }

    /// <summary>
    /// Deletes a usuPermissoes
    /// </summary>
    [Authorize(usuPermissoesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetusuPermissoesLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Id.ToString()
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.usuPermissoes.usuPermissoes> ApplyFilters(IQueryable<Sapienza.Lexus.usuPermissoes.usuPermissoes> queryable, usuPermissoesGetListInput input)
    {
        return queryable
            .WhereIf(input.idUsuarioPermissao != null, x => x.idUsuarioPermissao == input.idUsuarioPermissao)
            .WhereIf(input.idUsuario != null, x => x.idUsuario == input.idUsuario)
            .WhereIf(input.idPermissao != null, x => x.idPermissao == input.idPermissao)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
