using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.fabConfig.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fabConfig;

/// <summary>
/// Application service for fabConfig entity
/// </summary>
[Authorize(fabConfigPermissions.Default)]
public class fabConfigAppService :
    LexusAppService,
    IfabConfigAppService
{
    private readonly IRepository<Sapienza.Lexus.fabConfig.fabConfig, Guid> _repository;

    public fabConfigAppService(
        IRepository<Sapienza.Lexus.fabConfig.fabConfig, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single fabConfig by Id
    /// </summary>
    public virtual async Task<fabConfigDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.fabConfig.fabConfig, fabConfigDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of fabConfigs
    /// </summary>
    public virtual async Task<PagedResultDto<fabConfigDto>> GetListAsync(fabConfigGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.fabConfig.fabConfig>, List<fabConfigDto>>(entities);

        return new PagedResultDto<fabConfigDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new fabConfig
    /// </summary>
    [Authorize(fabConfigPermissions.Create)]
    public virtual async Task<fabConfigDto> CreateAsync(CreateUpdatefabConfigDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefabConfigDto, Sapienza.Lexus.fabConfig.fabConfig>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabConfig.fabConfig, fabConfigDto>(entity);
    }

    /// <summary>
    /// Updates an existing fabConfig
    /// </summary>
    [Authorize(fabConfigPermissions.Update)]
    public virtual async Task<fabConfigDto> UpdateAsync(Guid id, CreateUpdatefabConfigDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.fabConfig.fabConfig), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabConfig.fabConfig, fabConfigDto>(entity);
    }

    /// <summary>
    /// Deletes a fabConfig
    /// </summary>
    [Authorize(fabConfigPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfabConfigLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.imagemLogin
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.fabConfig.fabConfig> ApplyFilters(IQueryable<Sapienza.Lexus.fabConfig.fabConfig> queryable, fabConfigGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.imagemLogin.Contains(input.Filter) || x.imagemLoginCentral.Contains(input.Filter) || x.imagemLoginTickets.Contains(input.Filter) || x.dataBloqueioFinanceiro.Contains(input.Filter))
            .WhereIf(input.idConfig != null, x => x.idConfig == input.idConfig)
            .WhereIf(!input.imagemLogin.IsNullOrWhiteSpace(), x => x.imagemLogin.Contains(input.imagemLogin))
            .WhereIf(!input.imagemLoginCentral.IsNullOrWhiteSpace(), x => x.imagemLoginCentral.Contains(input.imagemLoginCentral))
            .WhereIf(!input.imagemLoginTickets.IsNullOrWhiteSpace(), x => x.imagemLoginTickets.Contains(input.imagemLoginTickets))
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.precoCombustivel != null, x => x.precoCombustivel == input.precoCombustivel)
            .WhereIf(!input.dataBloqueioFinanceiro.IsNullOrWhiteSpace(), x => x.dataBloqueioFinanceiro.Contains(input.dataBloqueioFinanceiro))
            // ========== FK Filters ==========
            ;
    }
}
