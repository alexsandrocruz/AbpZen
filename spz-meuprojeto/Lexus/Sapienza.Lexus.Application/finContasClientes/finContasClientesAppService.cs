using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finContasClientes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finContasClientes;

/// <summary>
/// Application service for finContasClientes entity
/// </summary>
[Authorize(finContasClientesPermissions.Default)]
public class finContasClientesAppService :
    LexusAppService,
    IfinContasClientesAppService
{
    private readonly IRepository<Sapienza.Lexus.finContasClientes.finContasClientes, Guid> _repository;

    public finContasClientesAppService(
        IRepository<Sapienza.Lexus.finContasClientes.finContasClientes, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single finContasClientes by Id
    /// </summary>
    public virtual async Task<finContasClientesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finContasClientes.finContasClientes, finContasClientesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finContasClienteses
    /// </summary>
    public virtual async Task<PagedResultDto<finContasClientesDto>> GetListAsync(finContasClientesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finContasClientes.finContasClientes>, List<finContasClientesDto>>(entities);

        return new PagedResultDto<finContasClientesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finContasClientes
    /// </summary>
    [Authorize(finContasClientesPermissions.Create)]
    public virtual async Task<finContasClientesDto> CreateAsync(CreateUpdatefinContasClientesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinContasClientesDto, Sapienza.Lexus.finContasClientes.finContasClientes>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finContasClientes.finContasClientes, finContasClientesDto>(entity);
    }

    /// <summary>
    /// Updates an existing finContasClientes
    /// </summary>
    [Authorize(finContasClientesPermissions.Update)]
    public virtual async Task<finContasClientesDto> UpdateAsync(Guid id, CreateUpdatefinContasClientesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finContasClientes.finContasClientes), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finContasClientes.finContasClientes, finContasClientesDto>(entity);
    }

    /// <summary>
    /// Deletes a finContasClientes
    /// </summary>
    [Authorize(finContasClientesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinContasClientesLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.finContasClientes.finContasClientes> ApplyFilters(IQueryable<Sapienza.Lexus.finContasClientes.finContasClientes> queryable, finContasClientesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter) || x.cor.Contains(input.Filter))
            .WhereIf(input.idContaCliente != null, x => x.idContaCliente == input.idContaCliente)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(!input.cor.IsNullOrWhiteSpace(), x => x.cor.Contains(input.cor))
            // ========== FK Filters ==========
            ;
    }
}
