using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finRateiosPadrao.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finRateiosPadrao;

/// <summary>
/// Application service for finRateiosPadrao entity
/// </summary>
[Authorize(finRateiosPadraoPermissions.Default)]
public class finRateiosPadraoAppService :
    LexusAppService,
    IfinRateiosPadraoAppService
{
    private readonly IRepository<Sapienza.Lexus.finRateiosPadrao.finRateiosPadrao, Guid> _repository;

    public finRateiosPadraoAppService(
        IRepository<Sapienza.Lexus.finRateiosPadrao.finRateiosPadrao, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single finRateiosPadrao by Id
    /// </summary>
    public virtual async Task<finRateiosPadraoDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finRateiosPadrao.finRateiosPadrao, finRateiosPadraoDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finRateiosPadraos
    /// </summary>
    public virtual async Task<PagedResultDto<finRateiosPadraoDto>> GetListAsync(finRateiosPadraoGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finRateiosPadrao.finRateiosPadrao>, List<finRateiosPadraoDto>>(entities);

        return new PagedResultDto<finRateiosPadraoDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finRateiosPadrao
    /// </summary>
    [Authorize(finRateiosPadraoPermissions.Create)]
    public virtual async Task<finRateiosPadraoDto> CreateAsync(CreateUpdatefinRateiosPadraoDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinRateiosPadraoDto, Sapienza.Lexus.finRateiosPadrao.finRateiosPadrao>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finRateiosPadrao.finRateiosPadrao, finRateiosPadraoDto>(entity);
    }

    /// <summary>
    /// Updates an existing finRateiosPadrao
    /// </summary>
    [Authorize(finRateiosPadraoPermissions.Update)]
    public virtual async Task<finRateiosPadraoDto> UpdateAsync(Guid id, CreateUpdatefinRateiosPadraoDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finRateiosPadrao.finRateiosPadrao), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finRateiosPadrao.finRateiosPadrao, finRateiosPadraoDto>(entity);
    }

    /// <summary>
    /// Deletes a finRateiosPadrao
    /// </summary>
    [Authorize(finRateiosPadraoPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinRateiosPadraoLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.finRateiosPadrao.finRateiosPadrao> ApplyFilters(IQueryable<Sapienza.Lexus.finRateiosPadrao.finRateiosPadrao> queryable, finRateiosPadraoGetListInput input)
    {
        return queryable
            .WhereIf(input.idPadrao != null, x => x.idPadrao == input.idPadrao)
            .WhereIf(input.idUnidade != null, x => x.idUnidade == input.idUnidade)
            .WhereIf(input.idCentroResultado != null, x => x.idCentroResultado == input.idCentroResultado)
            .WhereIf(input.porcentagem != null, x => x.porcentagem == input.porcentagem)
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
