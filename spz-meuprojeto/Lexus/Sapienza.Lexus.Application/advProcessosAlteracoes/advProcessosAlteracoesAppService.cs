using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProcessosAlteracoes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProcessosAlteracoes;

/// <summary>
/// Application service for advProcessosAlteracoes entity
/// </summary>
[Authorize(advProcessosAlteracoesPermissions.Default)]
public class advProcessosAlteracoesAppService :
    LexusAppService,
    IadvProcessosAlteracoesAppService
{
    private readonly IRepository<Sapienza.Lexus.advProcessosAlteracoes.advProcessosAlteracoes, Guid> _repository;

    public advProcessosAlteracoesAppService(
        IRepository<Sapienza.Lexus.advProcessosAlteracoes.advProcessosAlteracoes, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProcessosAlteracoes by Id
    /// </summary>
    public virtual async Task<advProcessosAlteracoesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProcessosAlteracoes.advProcessosAlteracoes, advProcessosAlteracoesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProcessosAlteracoeses
    /// </summary>
    public virtual async Task<PagedResultDto<advProcessosAlteracoesDto>> GetListAsync(advProcessosAlteracoesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProcessosAlteracoes.advProcessosAlteracoes>, List<advProcessosAlteracoesDto>>(entities);

        return new PagedResultDto<advProcessosAlteracoesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProcessosAlteracoes
    /// </summary>
    [Authorize(advProcessosAlteracoesPermissions.Create)]
    public virtual async Task<advProcessosAlteracoesDto> CreateAsync(CreateUpdateadvProcessosAlteracoesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProcessosAlteracoesDto, Sapienza.Lexus.advProcessosAlteracoes.advProcessosAlteracoes>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProcessosAlteracoes.advProcessosAlteracoes, advProcessosAlteracoesDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProcessosAlteracoes
    /// </summary>
    [Authorize(advProcessosAlteracoesPermissions.Update)]
    public virtual async Task<advProcessosAlteracoesDto> UpdateAsync(Guid id, CreateUpdateadvProcessosAlteracoesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProcessosAlteracoes.advProcessosAlteracoes), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProcessosAlteracoes.advProcessosAlteracoes, advProcessosAlteracoesDto>(entity);
    }

    /// <summary>
    /// Deletes a advProcessosAlteracoes
    /// </summary>
    [Authorize(advProcessosAlteracoesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProcessosAlteracoesLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.texto
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advProcessosAlteracoes.advProcessosAlteracoes> ApplyFilters(IQueryable<Sapienza.Lexus.advProcessosAlteracoes.advProcessosAlteracoes> queryable, advProcessosAlteracoesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.texto.Contains(input.Filter))
            .WhereIf(input.idProcessoAlteracao != null, x => x.idProcessoAlteracao == input.idProcessoAlteracao)
            .WhereIf(input.idProcesso != null, x => x.idProcesso == input.idProcesso)
            .WhereIf(input.IdentityUserId != null, x => x.IdentityUserId == input.IdentityUserId)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(!input.texto.IsNullOrWhiteSpace(), x => x.texto.Contains(input.texto))
            // ========== FK Filters ==========
            ;
    }
}
