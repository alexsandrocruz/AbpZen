using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finRateios.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finRateios;

/// <summary>
/// Application service for finRateios entity
/// </summary>
[Authorize(finRateiosPermissions.Default)]
public class finRateiosAppService :
    LexusAppService,
    IfinRateiosAppService
{
    private readonly IRepository<Sapienza.Lexus.finRateios.finRateios, Guid> _repository;

    public finRateiosAppService(
        IRepository<Sapienza.Lexus.finRateios.finRateios, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single finRateios by Id
    /// </summary>
    public virtual async Task<finRateiosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finRateios.finRateios, finRateiosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finRateioses
    /// </summary>
    public virtual async Task<PagedResultDto<finRateiosDto>> GetListAsync(finRateiosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finRateios.finRateios>, List<finRateiosDto>>(entities);

        return new PagedResultDto<finRateiosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finRateios
    /// </summary>
    [Authorize(finRateiosPermissions.Create)]
    public virtual async Task<finRateiosDto> CreateAsync(CreateUpdatefinRateiosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinRateiosDto, Sapienza.Lexus.finRateios.finRateios>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finRateios.finRateios, finRateiosDto>(entity);
    }

    /// <summary>
    /// Updates an existing finRateios
    /// </summary>
    [Authorize(finRateiosPermissions.Update)]
    public virtual async Task<finRateiosDto> UpdateAsync(Guid id, CreateUpdatefinRateiosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finRateios.finRateios), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finRateios.finRateios, finRateiosDto>(entity);
    }

    /// <summary>
    /// Deletes a finRateios
    /// </summary>
    [Authorize(finRateiosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinRateiosLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.finRateios.finRateios> ApplyFilters(IQueryable<Sapienza.Lexus.finRateios.finRateios> queryable, finRateiosGetListInput input)
    {
        return queryable
            .WhereIf(input.idRateio != null, x => x.idRateio == input.idRateio)
            .WhereIf(input.idLancamento != null, x => x.idLancamento == input.idLancamento)
            .WhereIf(input.idCentroCusto != null, x => x.idCentroCusto == input.idCentroCusto)
            .WhereIf(input.idCentroResultado != null, x => x.idCentroResultado == input.idCentroResultado)
            .WhereIf(input.percentualCC != null, x => x.percentualCC == input.percentualCC)
            .WhereIf(input.percentualCR != null, x => x.percentualCR == input.percentualCR)
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.idUnidade != null, x => x.idUnidade == input.idUnidade)
            // ========== FK Filters ==========
            ;
    }
}
