using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProfissionais.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProfissionais;

/// <summary>
/// Application service for advProfissionais entity
/// </summary>
[Authorize(advProfissionaisPermissions.Default)]
public class advProfissionaisAppService :
    LexusAppService,
    IadvProfissionaisAppService
{
    private readonly IRepository<Sapienza.Lexus.advProfissionais.advProfissionais, Guid> _repository;

    public advProfissionaisAppService(
        IRepository<Sapienza.Lexus.advProfissionais.advProfissionais, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProfissionais by Id
    /// </summary>
    public virtual async Task<advProfissionaisDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProfissionais.advProfissionais, advProfissionaisDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProfissionaises
    /// </summary>
    public virtual async Task<PagedResultDto<advProfissionaisDto>> GetListAsync(advProfissionaisGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProfissionais.advProfissionais>, List<advProfissionaisDto>>(entities);

        return new PagedResultDto<advProfissionaisDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProfissionais
    /// </summary>
    [Authorize(advProfissionaisPermissions.Create)]
    public virtual async Task<advProfissionaisDto> CreateAsync(CreateUpdateadvProfissionaisDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProfissionaisDto, Sapienza.Lexus.advProfissionais.advProfissionais>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProfissionais.advProfissionais, advProfissionaisDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProfissionais
    /// </summary>
    [Authorize(advProfissionaisPermissions.Update)]
    public virtual async Task<advProfissionaisDto> UpdateAsync(Guid id, CreateUpdateadvProfissionaisDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProfissionais.advProfissionais), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProfissionais.advProfissionais, advProfissionaisDto>(entity);
    }

    /// <summary>
    /// Deletes a advProfissionais
    /// </summary>
    [Authorize(advProfissionaisPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProfissionaisLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.nome
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advProfissionais.advProfissionais> ApplyFilters(IQueryable<Sapienza.Lexus.advProfissionais.advProfissionais> queryable, advProfissionaisGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.nome.Contains(input.Filter) || x.email.Contains(input.Filter))
            .WhereIf(input.idProfissional != null, x => x.idProfissional == input.idProfissional)
            .WhereIf(input.idUsuario != null, x => x.idUsuario == input.idUsuario)
            .WhereIf(!input.nome.IsNullOrWhiteSpace(), x => x.nome.Contains(input.nome))
            .WhereIf(!input.email.IsNullOrWhiteSpace(), x => x.email.Contains(input.email))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
