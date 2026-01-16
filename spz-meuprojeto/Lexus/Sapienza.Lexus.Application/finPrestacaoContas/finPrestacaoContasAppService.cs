using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finPrestacaoContas.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finPrestacaoContas;

/// <summary>
/// Application service for finPrestacaoContas entity
/// </summary>
[Authorize(finPrestacaoContasPermissions.Default)]
public class finPrestacaoContasAppService :
    LexusAppService,
    IfinPrestacaoContasAppService
{
    private readonly IRepository<Sapienza.Lexus.finPrestacaoContas.finPrestacaoContas, Guid> _repository;

    public finPrestacaoContasAppService(
        IRepository<Sapienza.Lexus.finPrestacaoContas.finPrestacaoContas, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single finPrestacaoContas by Id
    /// </summary>
    public virtual async Task<finPrestacaoContasDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finPrestacaoContas.finPrestacaoContas, finPrestacaoContasDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finPrestacaoContases
    /// </summary>
    public virtual async Task<PagedResultDto<finPrestacaoContasDto>> GetListAsync(finPrestacaoContasGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finPrestacaoContas.finPrestacaoContas>, List<finPrestacaoContasDto>>(entities);

        return new PagedResultDto<finPrestacaoContasDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finPrestacaoContas
    /// </summary>
    [Authorize(finPrestacaoContasPermissions.Create)]
    public virtual async Task<finPrestacaoContasDto> CreateAsync(CreateUpdatefinPrestacaoContasDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinPrestacaoContasDto, Sapienza.Lexus.finPrestacaoContas.finPrestacaoContas>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finPrestacaoContas.finPrestacaoContas, finPrestacaoContasDto>(entity);
    }

    /// <summary>
    /// Updates an existing finPrestacaoContas
    /// </summary>
    [Authorize(finPrestacaoContasPermissions.Update)]
    public virtual async Task<finPrestacaoContasDto> UpdateAsync(Guid id, CreateUpdatefinPrestacaoContasDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finPrestacaoContas.finPrestacaoContas), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finPrestacaoContas.finPrestacaoContas, finPrestacaoContasDto>(entity);
    }

    /// <summary>
    /// Deletes a finPrestacaoContas
    /// </summary>
    [Authorize(finPrestacaoContasPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinPrestacaoContasLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.finPrestacaoContas.finPrestacaoContas> ApplyFilters(IQueryable<Sapienza.Lexus.finPrestacaoContas.finPrestacaoContas> queryable, finPrestacaoContasGetListInput input)
    {
        return queryable
            .WhereIf(input.idPrestacao != null, x => x.idPrestacao == input.idPrestacao)
            .WhereIf(input.idLancamento != null, x => x.idLancamento == input.idLancamento)
            .WhereIf(input.levantado != null, x => x.levantado == input.levantado)
            .WhereIf(input.irpj != null, x => x.irpj == input.irpj)
            .WhereIf(input.carta != null, x => x.carta == input.carta)
            .WhereIf(input.honorarios != null, x => x.honorarios == input.honorarios)
            .WhereIf(input.tarifa != null, x => x.tarifa == input.tarifa)
            .WhereIf(input.liquidoRecebido != null, x => x.liquidoRecebido == input.liquidoRecebido)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            // ========== FK Filters ==========
            ;
    }
}
