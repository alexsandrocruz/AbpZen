using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advVerbas.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advVerbas;

/// <summary>
/// Application service for advVerbas entity
/// </summary>
[Authorize(advVerbasPermissions.Default)]
public class advVerbasAppService :
    LexusAppService,
    IadvVerbasAppService
{
    private readonly IRepository<Sapienza.Lexus.advVerbas.advVerbas, Guid> _repository;

    public advVerbasAppService(
        IRepository<Sapienza.Lexus.advVerbas.advVerbas, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advVerbas by Id
    /// </summary>
    public virtual async Task<advVerbasDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advVerbas.advVerbas, advVerbasDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advVerbases
    /// </summary>
    public virtual async Task<PagedResultDto<advVerbasDto>> GetListAsync(advVerbasGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advVerbas.advVerbas>, List<advVerbasDto>>(entities);

        return new PagedResultDto<advVerbasDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advVerbas
    /// </summary>
    [Authorize(advVerbasPermissions.Create)]
    public virtual async Task<advVerbasDto> CreateAsync(CreateUpdateadvVerbasDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvVerbasDto, Sapienza.Lexus.advVerbas.advVerbas>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advVerbas.advVerbas, advVerbasDto>(entity);
    }

    /// <summary>
    /// Updates an existing advVerbas
    /// </summary>
    [Authorize(advVerbasPermissions.Update)]
    public virtual async Task<advVerbasDto> UpdateAsync(Guid id, CreateUpdateadvVerbasDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advVerbas.advVerbas), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advVerbas.advVerbas, advVerbasDto>(entity);
    }

    /// <summary>
    /// Deletes a advVerbas
    /// </summary>
    [Authorize(advVerbasPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvVerbasLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.dataDe
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advVerbas.advVerbas> ApplyFilters(IQueryable<Sapienza.Lexus.advVerbas.advVerbas> queryable, advVerbasGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.dataDe.Contains(input.Filter) || x.dataAte.Contains(input.Filter) || x.estado.Contains(input.Filter) || x.cidade.Contains(input.Filter) || x.comprovanteArquivo.Contains(input.Filter) || x.incluidoPor.Contains(input.Filter) || x.alteradoPor.Contains(input.Filter))
            .WhereIf(input.idVerba != null, x => x.idVerba == input.idVerba)
            .WhereIf(input.idTipo != null, x => x.idTipo == input.idTipo)
            .WhereIf(input.idProfissional != null, x => x.idProfissional == input.idProfissional)
            .WhereIf(input.idProcesso != null, x => x.idProcesso == input.idProcesso)
            .WhereIf(input.idLancamento != null, x => x.idLancamento == input.idLancamento)
            .WhereIf(input.valor != null, x => x.valor == input.valor)
            .WhereIf(!input.dataDe.IsNullOrWhiteSpace(), x => x.dataDe.Contains(input.dataDe))
            .WhereIf(!input.dataAte.IsNullOrWhiteSpace(), x => x.dataAte.Contains(input.dataAte))
            .WhereIf(!input.estado.IsNullOrWhiteSpace(), x => x.estado.Contains(input.estado))
            .WhereIf(!input.cidade.IsNullOrWhiteSpace(), x => x.cidade.Contains(input.cidade))
            .WhereIf(input.comprovante != null, x => x.comprovante == input.comprovante)
            .WhereIf(!input.comprovanteArquivo.IsNullOrWhiteSpace(), x => x.comprovanteArquivo.Contains(input.comprovanteArquivo))
            .WhereIf(input.solicitacao != null, x => x.solicitacao == input.solicitacao)
            .WhereIf(input.aceito != null, x => x.aceito == input.aceito)
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(!input.incluidoPor.IsNullOrWhiteSpace(), x => x.incluidoPor.Contains(input.incluidoPor))
            .WhereIf(!input.alteradoPor.IsNullOrWhiteSpace(), x => x.alteradoPor.Contains(input.alteradoPor))
            // ========== FK Filters ==========
            ;
    }
}
