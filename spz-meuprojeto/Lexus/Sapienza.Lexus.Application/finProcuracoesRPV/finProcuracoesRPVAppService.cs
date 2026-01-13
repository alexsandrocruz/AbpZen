using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finProcuracoesRPV.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finProcuracoesRPV;

/// <summary>
/// Application service for finProcuracoesRPV entity
/// </summary>
[Authorize(finProcuracoesRPVPermissions.Default)]
public class finProcuracoesRPVAppService :
    LexusAppService,
    IfinProcuracoesRPVAppService
{
    private readonly IRepository<Sapienza.Lexus.finProcuracoesRPV.finProcuracoesRPV, Guid> _repository;

    public finProcuracoesRPVAppService(
        IRepository<Sapienza.Lexus.finProcuracoesRPV.finProcuracoesRPV, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single finProcuracoesRPV by Id
    /// </summary>
    public virtual async Task<finProcuracoesRPVDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finProcuracoesRPV.finProcuracoesRPV, finProcuracoesRPVDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finProcuracoesRPVs
    /// </summary>
    public virtual async Task<PagedResultDto<finProcuracoesRPVDto>> GetListAsync(finProcuracoesRPVGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finProcuracoesRPV.finProcuracoesRPV>, List<finProcuracoesRPVDto>>(entities);

        return new PagedResultDto<finProcuracoesRPVDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finProcuracoesRPV
    /// </summary>
    [Authorize(finProcuracoesRPVPermissions.Create)]
    public virtual async Task<finProcuracoesRPVDto> CreateAsync(CreateUpdatefinProcuracoesRPVDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinProcuracoesRPVDto, Sapienza.Lexus.finProcuracoesRPV.finProcuracoesRPV>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finProcuracoesRPV.finProcuracoesRPV, finProcuracoesRPVDto>(entity);
    }

    /// <summary>
    /// Updates an existing finProcuracoesRPV
    /// </summary>
    [Authorize(finProcuracoesRPVPermissions.Update)]
    public virtual async Task<finProcuracoesRPVDto> UpdateAsync(Guid id, CreateUpdatefinProcuracoesRPVDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finProcuracoesRPV.finProcuracoesRPV), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finProcuracoesRPV.finProcuracoesRPV, finProcuracoesRPVDto>(entity);
    }

    /// <summary>
    /// Deletes a finProcuracoesRPV
    /// </summary>
    [Authorize(finProcuracoesRPVPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinProcuracoesRPVLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.finProcuracoesRPV.finProcuracoesRPV> ApplyFilters(IQueryable<Sapienza.Lexus.finProcuracoesRPV.finProcuracoesRPV> queryable, finProcuracoesRPVGetListInput input)
    {
        return queryable
            .WhereIf(input.idProcuracao != null, x => x.idProcuracao == input.idProcuracao)
            .WhereIf(input.idCliente != null, x => x.idCliente == input.idCliente)
            .WhereIf(input.idProcesso != null, x => x.idProcesso == input.idProcesso)
            .WhereIf(input.impressa != null, x => x.impressa == input.impressa)
            .WhereIf(input.tsImpressa != null, x => x.tsImpressa == input.tsImpressa)
            .WhereIf(input.assinada != null, x => x.assinada == input.assinada)
            .WhereIf(input.tsAssinatura != null, x => x.tsAssinatura == input.tsAssinatura)
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
