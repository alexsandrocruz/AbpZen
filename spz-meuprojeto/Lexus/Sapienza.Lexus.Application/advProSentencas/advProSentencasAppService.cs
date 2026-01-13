using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProSentencas.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProSentencas;

/// <summary>
/// Application service for advProSentencas entity
/// </summary>
[Authorize(advProSentencasPermissions.Default)]
public class advProSentencasAppService :
    LexusAppService,
    IadvProSentencasAppService
{
    private readonly IRepository<Sapienza.Lexus.advProSentencas.advProSentencas, Guid> _repository;

    public advProSentencasAppService(
        IRepository<Sapienza.Lexus.advProSentencas.advProSentencas, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProSentencas by Id
    /// </summary>
    public virtual async Task<advProSentencasDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProSentencas.advProSentencas, advProSentencasDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProSentencases
    /// </summary>
    public virtual async Task<PagedResultDto<advProSentencasDto>> GetListAsync(advProSentencasGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProSentencas.advProSentencas>, List<advProSentencasDto>>(entities);

        return new PagedResultDto<advProSentencasDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProSentencas
    /// </summary>
    [Authorize(advProSentencasPermissions.Create)]
    public virtual async Task<advProSentencasDto> CreateAsync(CreateUpdateadvProSentencasDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProSentencasDto, Sapienza.Lexus.advProSentencas.advProSentencas>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProSentencas.advProSentencas, advProSentencasDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProSentencas
    /// </summary>
    [Authorize(advProSentencasPermissions.Update)]
    public virtual async Task<advProSentencasDto> UpdateAsync(Guid id, CreateUpdateadvProSentencasDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProSentencas.advProSentencas), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProSentencas.advProSentencas, advProSentencasDto>(entity);
    }

    /// <summary>
    /// Deletes a advProSentencas
    /// </summary>
    [Authorize(advProSentencasPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProSentencasLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advProSentencas.advProSentencas> ApplyFilters(IQueryable<Sapienza.Lexus.advProSentencas.advProSentencas> queryable, advProSentencasGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idSentenca != null, x => x.idSentenca == input.idSentenca)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
