using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProcessosDadosHerdeiros.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProcessosDadosHerdeiros;

/// <summary>
/// Application service for advProcessosDadosHerdeiros entity
/// </summary>
[Authorize(advProcessosDadosHerdeirosPermissions.Default)]
public class advProcessosDadosHerdeirosAppService :
    LexusAppService,
    IadvProcessosDadosHerdeirosAppService
{
    private readonly IRepository<Sapienza.Lexus.advProcessosDadosHerdeiros.advProcessosDadosHerdeiros, Guid> _repository;

    public advProcessosDadosHerdeirosAppService(
        IRepository<Sapienza.Lexus.advProcessosDadosHerdeiros.advProcessosDadosHerdeiros, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProcessosDadosHerdeiros by Id
    /// </summary>
    public virtual async Task<advProcessosDadosHerdeirosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProcessosDadosHerdeiros.advProcessosDadosHerdeiros, advProcessosDadosHerdeirosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProcessosDadosHerdeiroses
    /// </summary>
    public virtual async Task<PagedResultDto<advProcessosDadosHerdeirosDto>> GetListAsync(advProcessosDadosHerdeirosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProcessosDadosHerdeiros.advProcessosDadosHerdeiros>, List<advProcessosDadosHerdeirosDto>>(entities);

        return new PagedResultDto<advProcessosDadosHerdeirosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProcessosDadosHerdeiros
    /// </summary>
    [Authorize(advProcessosDadosHerdeirosPermissions.Create)]
    public virtual async Task<advProcessosDadosHerdeirosDto> CreateAsync(CreateUpdateadvProcessosDadosHerdeirosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProcessosDadosHerdeirosDto, Sapienza.Lexus.advProcessosDadosHerdeiros.advProcessosDadosHerdeiros>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProcessosDadosHerdeiros.advProcessosDadosHerdeiros, advProcessosDadosHerdeirosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProcessosDadosHerdeiros
    /// </summary>
    [Authorize(advProcessosDadosHerdeirosPermissions.Update)]
    public virtual async Task<advProcessosDadosHerdeirosDto> UpdateAsync(Guid id, CreateUpdateadvProcessosDadosHerdeirosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProcessosDadosHerdeiros.advProcessosDadosHerdeiros), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProcessosDadosHerdeiros.advProcessosDadosHerdeiros, advProcessosDadosHerdeirosDto>(entity);
    }

    /// <summary>
    /// Deletes a advProcessosDadosHerdeiros
    /// </summary>
    [Authorize(advProcessosDadosHerdeirosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProcessosDadosHerdeirosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.bancarioTipoConta
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advProcessosDadosHerdeiros.advProcessosDadosHerdeiros> ApplyFilters(IQueryable<Sapienza.Lexus.advProcessosDadosHerdeiros.advProcessosDadosHerdeiros> queryable, advProcessosDadosHerdeirosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.bancarioTipoConta.Contains(input.Filter) || x.bancarioAgencia.Contains(input.Filter) || x.bancarioConta.Contains(input.Filter) || x.bancarioFavorecido.Contains(input.Filter) || x.bancarioCpf.Contains(input.Filter) || x.bancarioTarifaParcelas.Contains(input.Filter))
            .WhereIf(input.idHerdeiro != null, x => x.idHerdeiro == input.idHerdeiro)
            .WhereIf(input.idProcesso != null, x => x.idProcesso == input.idProcesso)
            .WhereIf(input.sequencia != null, x => x.sequencia == input.sequencia)
            .WhereIf(input.bancarioBancoId != null, x => x.bancarioBancoId == input.bancarioBancoId)
            .WhereIf(!input.bancarioTipoConta.IsNullOrWhiteSpace(), x => x.bancarioTipoConta.Contains(input.bancarioTipoConta))
            .WhereIf(!input.bancarioAgencia.IsNullOrWhiteSpace(), x => x.bancarioAgencia.Contains(input.bancarioAgencia))
            .WhereIf(!input.bancarioConta.IsNullOrWhiteSpace(), x => x.bancarioConta.Contains(input.bancarioConta))
            .WhereIf(!input.bancarioFavorecido.IsNullOrWhiteSpace(), x => x.bancarioFavorecido.Contains(input.bancarioFavorecido))
            .WhereIf(!input.bancarioCpf.IsNullOrWhiteSpace(), x => x.bancarioCpf.Contains(input.bancarioCpf))
            .WhereIf(input.bancarioPerc != null, x => x.bancarioPerc == input.bancarioPerc)
            .WhereIf(input.bancarioTarifa != null, x => x.bancarioTarifa == input.bancarioTarifa)
            .WhereIf(!input.bancarioTarifaParcelas.IsNullOrWhiteSpace(), x => x.bancarioTarifaParcelas.Contains(input.bancarioTarifaParcelas))
            .WhereIf(input.idHonorario != null, x => x.idHonorario == input.idHonorario)
            // ========== FK Filters ==========
            ;
    }
}
