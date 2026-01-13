using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finExtrato.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finExtrato;

/// <summary>
/// Application service for finExtrato entity
/// </summary>
[Authorize(finExtratoPermissions.Default)]
public class finExtratoAppService :
    LexusAppService,
    IfinExtratoAppService
{
    private readonly IRepository<Sapienza.Lexus.finExtrato.finExtrato, Guid> _repository;

    public finExtratoAppService(
        IRepository<Sapienza.Lexus.finExtrato.finExtrato, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single finExtrato by Id
    /// </summary>
    public virtual async Task<finExtratoDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finExtrato.finExtrato, finExtratoDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finExtratos
    /// </summary>
    public virtual async Task<PagedResultDto<finExtratoDto>> GetListAsync(finExtratoGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finExtrato.finExtrato>, List<finExtratoDto>>(entities);

        return new PagedResultDto<finExtratoDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finExtrato
    /// </summary>
    [Authorize(finExtratoPermissions.Create)]
    public virtual async Task<finExtratoDto> CreateAsync(CreateUpdatefinExtratoDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinExtratoDto, Sapienza.Lexus.finExtrato.finExtrato>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finExtrato.finExtrato, finExtratoDto>(entity);
    }

    /// <summary>
    /// Updates an existing finExtrato
    /// </summary>
    [Authorize(finExtratoPermissions.Update)]
    public virtual async Task<finExtratoDto> UpdateAsync(Guid id, CreateUpdatefinExtratoDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finExtrato.finExtrato), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finExtrato.finExtrato, finExtratoDto>(entity);
    }

    /// <summary>
    /// Deletes a finExtrato
    /// </summary>
    [Authorize(finExtratoPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinExtratoLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.data
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.finExtrato.finExtrato> ApplyFilters(IQueryable<Sapienza.Lexus.finExtrato.finExtrato> queryable, finExtratoGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.data.Contains(input.Filter) || x.descricao.Contains(input.Filter))
            .WhereIf(input.idExtrato != null, x => x.idExtrato == input.idExtrato)
            .WhereIf(input.idConta != null, x => x.idConta == input.idConta)
            .WhereIf(input.idLancamento != null, x => x.idLancamento == input.idLancamento)
            .WhereIf(input.transferencia != null, x => x.transferencia == input.transferencia)
            .WhereIf(input.idExtratoRel != null, x => x.idExtratoRel == input.idExtratoRel)
            .WhereIf(!input.data.IsNullOrWhiteSpace(), x => x.data.Contains(input.data))
            .WhereIf(!input.descricao.IsNullOrWhiteSpace(), x => x.descricao.Contains(input.descricao))
            .WhereIf(input.credito != null, x => x.credito == input.credito)
            .WhereIf(input.debito != null, x => x.debito == input.debito)
            .WhereIf(input.conferido != null, x => x.conferido == input.conferido)
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.idUsuarioInclusao != null, x => x.idUsuarioInclusao == input.idUsuarioInclusao)
            .WhereIf(input.idUsuarioAlteracao != null, x => x.idUsuarioAlteracao == input.idUsuarioAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
