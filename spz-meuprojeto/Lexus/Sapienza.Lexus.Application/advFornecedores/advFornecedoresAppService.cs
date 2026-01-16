using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advFornecedores.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advFornecedores;

/// <summary>
/// Application service for advFornecedores entity
/// </summary>
[Authorize(advFornecedoresPermissions.Default)]
public class advFornecedoresAppService :
    LexusAppService,
    IadvFornecedoresAppService
{
    private readonly IRepository<Sapienza.Lexus.advFornecedores.advFornecedores, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.advClientes.advClientes, Guid> _advClientesRepository;

    public advFornecedoresAppService(
        IRepository<Sapienza.Lexus.advFornecedores.advFornecedores, Guid> repository,
        IRepository<Sapienza.Lexus.advClientes.advClientes, Guid> advClientesRepository
    )
    {
        _repository = repository;
        _advClientesRepository = advClientesRepository;
    }

    /// <summary>
    /// Gets a single advFornecedores by Id
    /// </summary>
    public virtual async Task<advFornecedoresDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advFornecedores.advFornecedores, advFornecedoresDto>(entity);
        var advClientes = await _advClientesRepository.FindAsync(entity.advClientesId);
        dto.advClientesDisplayName = advClientes?.apelido;

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advFornecedoreses
    /// </summary>
    public virtual async Task<PagedResultDto<advFornecedoresDto>> GetListAsync(advFornecedoresGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advFornecedores.advFornecedores>, List<advFornecedoresDto>>(entities);
        var advClientesIds = entities
            .Select(x => x.advClientesId)
            .Distinct()
            .ToList();

        if (advClientesIds.Any())
        {
            var parents = await _advClientesRepository.GetListAsync(x => advClientesIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.apelido);

            foreach (var dto in dtoList)
            {
                if (parentMap.TryGetValue(dto.advClientesId, out var displayName))
                {
                    dto.advClientesDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<advFornecedoresDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advFornecedores
    /// </summary>
    [Authorize(advFornecedoresPermissions.Create)]
    public virtual async Task<advFornecedoresDto> CreateAsync(CreateUpdateadvFornecedoresDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvFornecedoresDto, Sapienza.Lexus.advFornecedores.advFornecedores>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advFornecedores.advFornecedores, advFornecedoresDto>(entity);
    }

    /// <summary>
    /// Updates an existing advFornecedores
    /// </summary>
    [Authorize(advFornecedoresPermissions.Update)]
    public virtual async Task<advFornecedoresDto> UpdateAsync(Guid id, CreateUpdateadvFornecedoresDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advFornecedores.advFornecedores), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advFornecedores.advFornecedores, advFornecedoresDto>(entity);
    }

    /// <summary>
    /// Deletes a advFornecedores
    /// </summary>
    [Authorize(advFornecedoresPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvFornecedoresLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.apelido
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advFornecedores.advFornecedores> ApplyFilters(IQueryable<Sapienza.Lexus.advFornecedores.advFornecedores> queryable, advFornecedoresGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.apelido.Contains(input.Filter) || x.nome.Contains(input.Filter) || x.email.Contains(input.Filter) || x.telCelular.Contains(input.Filter) || x.telCelularObs.Contains(input.Filter) || x.telFixo.Contains(input.Filter) || x.telFixoObs.Contains(input.Filter) || x.endereco.Contains(input.Filter) || x.numero.Contains(input.Filter) || x.complemento.Contains(input.Filter) || x.bairro.Contains(input.Filter) || x.cep.Contains(input.Filter) || x.estado.Contains(input.Filter) || x.cidade.Contains(input.Filter) || x.observacoes.Contains(input.Filter) || x.foto.Contains(input.Filter))
            .WhereIf(input.idFornecedor != null, x => x.idFornecedor == input.idFornecedor)
            .WhereIf(!input.apelido.IsNullOrWhiteSpace(), x => x.apelido.Contains(input.apelido))
            .WhereIf(!input.nome.IsNullOrWhiteSpace(), x => x.nome.Contains(input.nome))
            .WhereIf(!input.email.IsNullOrWhiteSpace(), x => x.email.Contains(input.email))
            .WhereIf(!input.telCelular.IsNullOrWhiteSpace(), x => x.telCelular.Contains(input.telCelular))
            .WhereIf(!input.telCelularObs.IsNullOrWhiteSpace(), x => x.telCelularObs.Contains(input.telCelularObs))
            .WhereIf(!input.telFixo.IsNullOrWhiteSpace(), x => x.telFixo.Contains(input.telFixo))
            .WhereIf(!input.telFixoObs.IsNullOrWhiteSpace(), x => x.telFixoObs.Contains(input.telFixoObs))
            .WhereIf(!input.endereco.IsNullOrWhiteSpace(), x => x.endereco.Contains(input.endereco))
            .WhereIf(!input.numero.IsNullOrWhiteSpace(), x => x.numero.Contains(input.numero))
            .WhereIf(!input.complemento.IsNullOrWhiteSpace(), x => x.complemento.Contains(input.complemento))
            .WhereIf(!input.bairro.IsNullOrWhiteSpace(), x => x.bairro.Contains(input.bairro))
            .WhereIf(!input.cep.IsNullOrWhiteSpace(), x => x.cep.Contains(input.cep))
            .WhereIf(!input.estado.IsNullOrWhiteSpace(), x => x.estado.Contains(input.estado))
            .WhereIf(!input.cidade.IsNullOrWhiteSpace(), x => x.cidade.Contains(input.cidade))
            .WhereIf(!input.observacoes.IsNullOrWhiteSpace(), x => x.observacoes.Contains(input.observacoes))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.parceiroEmProcesso != null, x => x.parceiroEmProcesso == input.parceiroEmProcesso)
            .WhereIf(input.parceiroEmProcessoPerc != null, x => x.parceiroEmProcessoPerc == input.parceiroEmProcessoPerc)
            .WhereIf(input.idProfissional != null, x => x.idProfissional == input.idProfissional)
            .WhereIf(!input.foto.IsNullOrWhiteSpace(), x => x.foto.Contains(input.foto))
            // ========== FK Filters ==========
            .WhereIf(input.advClientesId != null, x => x.advClientesId == input.advClientesId)
            ;
    }
}
