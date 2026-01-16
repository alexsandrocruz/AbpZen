using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advClientes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advClientes;

/// <summary>
/// Application service for advClientes entity
/// </summary>
[Authorize(advClientesPermissions.Default)]
public class advClientesAppService :
    LexusAppService,
    IadvClientesAppService
{
    private readonly IRepository<Sapienza.Lexus.advClientes.advClientes, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.advClientesArquivos.advClientesArquivos, Guid> _advClientesArquivosRepository;
    private readonly IRepository<Sapienza.Lexus.advClientesAtualizacoes.advClientesAtualizacoes, Guid> _advClientesAtualizacoesRepository;
    private readonly IRepository<Sapienza.Lexus.advClientesChecklist.advClientesChecklist, Guid> _advClientesChecklistRepository;
    private readonly IRepository<Sapienza.Lexus.advProcessos.advProcessos, Guid> _advProcessosRepository;
    private readonly IRepository<Sapienza.Lexus.advProcessosClientes.advProcessosClientes, Guid> _advProcessosClientesRepository;
    private readonly IRepository<Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos, Guid> _advClientesHistoricosRepository;
    private readonly IRepository<Sapienza.Lexus.opoOportunidades.opoOportunidades, Guid> _opoOportunidadesRepository;
    private readonly IRepository<Sapienza.Lexus.flwFollows.flwFollows, Guid> _flwFollowsRepository;

    public advClientesAppService(
        IRepository<Sapienza.Lexus.advClientes.advClientes, Guid> repository,
        IRepository<Sapienza.Lexus.advClientesArquivos.advClientesArquivos, Guid> advClientesArquivosRepository,
        IRepository<Sapienza.Lexus.advClientesAtualizacoes.advClientesAtualizacoes, Guid> advClientesAtualizacoesRepository,
        IRepository<Sapienza.Lexus.advClientesChecklist.advClientesChecklist, Guid> advClientesChecklistRepository,
        IRepository<Sapienza.Lexus.advProcessos.advProcessos, Guid> advProcessosRepository,
        IRepository<Sapienza.Lexus.advProcessosClientes.advProcessosClientes, Guid> advProcessosClientesRepository,
        IRepository<Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos, Guid> advClientesHistoricosRepository,
        IRepository<Sapienza.Lexus.opoOportunidades.opoOportunidades, Guid> opoOportunidadesRepository,
        IRepository<Sapienza.Lexus.flwFollows.flwFollows, Guid> flwFollowsRepository
    )
    {
        _repository = repository;
        _advClientesArquivosRepository = advClientesArquivosRepository;
        _advClientesAtualizacoesRepository = advClientesAtualizacoesRepository;
        _advClientesChecklistRepository = advClientesChecklistRepository;
        _advProcessosRepository = advProcessosRepository;
        _advProcessosClientesRepository = advProcessosClientesRepository;
        _advClientesHistoricosRepository = advClientesHistoricosRepository;
        _opoOportunidadesRepository = opoOportunidadesRepository;
        _flwFollowsRepository = flwFollowsRepository;
    }

    /// <summary>
    /// Gets a single advClientes by Id
    /// </summary>
    public virtual async Task<advClientesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advClientes.advClientes, advClientesDto>(entity);
        if (entity.advClientesArquivosId != null)
        {
            var parent = await _advClientesArquivosRepository.FindAsync(entity.advClientesArquivosId.Value);
            dto.advClientesArquivosDisplayName = parent?.descricao;
        }
        if (entity.advClientesAtualizacoesId != null)
        {
            var parent = await _advClientesAtualizacoesRepository.FindAsync(entity.advClientesAtualizacoesId.Value);
            dto.advClientesAtualizacoesDisplayName = parent?.campo;
        }
        if (entity.advClientesChecklistId != null)
        {
            var parent = await _advClientesChecklistRepository.FindAsync(entity.advClientesChecklistId.Value);
            dto.advClientesChecklistDisplayName = parent?.Id.ToString();
        }
        if (entity.advProcessosId != null)
        {
            var parent = await _advProcessosRepository.FindAsync(entity.advProcessosId.Value);
            dto.advProcessosDisplayName = parent?.sintese;
        }
        var advProcessosClientes = await _advProcessosClientesRepository.FindAsync(entity.advProcessosClientesId);
        dto.advProcessosClientesDisplayName = advProcessosClientes?.Id.ToString();
        if (entity.advClientesHistoricosId != null)
        {
            var parent = await _advClientesHistoricosRepository.FindAsync(entity.advClientesHistoricosId.Value);
            dto.advClientesHistoricosDisplayName = parent?.data;
        }
        if (entity.opoOportunidadesId != null)
        {
            var parent = await _opoOportunidadesRepository.FindAsync(entity.opoOportunidadesId.Value);
            dto.opoOportunidadesDisplayName = parent?.titulo;
        }
        if (entity.flwFollowsId != null)
        {
            var parent = await _flwFollowsRepository.FindAsync(entity.flwFollowsId.Value);
            dto.flwFollowsDisplayName = parent?.data;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advClienteses
    /// </summary>
    public virtual async Task<PagedResultDto<advClientesDto>> GetListAsync(advClientesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advClientes.advClientes>, List<advClientesDto>>(entities);
        var advClientesArquivosIds = entities
            .Where(x => x.advClientesArquivosId != null)
            .Select(x => x.advClientesArquivosId.Value)
            .Distinct()
            .ToList();

        if (advClientesArquivosIds.Any())
        {
            var parents = await _advClientesArquivosRepository.GetListAsync(x => advClientesArquivosIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.descricao);

            foreach (var dto in dtoList.Where(x => x.advClientesArquivosId != null))
            {
                if (parentMap.TryGetValue(dto.advClientesArquivosId.Value, out var displayName))
                {
                    dto.advClientesArquivosDisplayName = displayName;
                }
            }
        }
        var advClientesAtualizacoesIds = entities
            .Where(x => x.advClientesAtualizacoesId != null)
            .Select(x => x.advClientesAtualizacoesId.Value)
            .Distinct()
            .ToList();

        if (advClientesAtualizacoesIds.Any())
        {
            var parents = await _advClientesAtualizacoesRepository.GetListAsync(x => advClientesAtualizacoesIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.campo);

            foreach (var dto in dtoList.Where(x => x.advClientesAtualizacoesId != null))
            {
                if (parentMap.TryGetValue(dto.advClientesAtualizacoesId.Value, out var displayName))
                {
                    dto.advClientesAtualizacoesDisplayName = displayName;
                }
            }
        }
        var advClientesChecklistIds = entities
            .Where(x => x.advClientesChecklistId != null)
            .Select(x => x.advClientesChecklistId.Value)
            .Distinct()
            .ToList();

        if (advClientesChecklistIds.Any())
        {
            var parents = await _advClientesChecklistRepository.GetListAsync(x => advClientesChecklistIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Id.ToString());

            foreach (var dto in dtoList.Where(x => x.advClientesChecklistId != null))
            {
                if (parentMap.TryGetValue(dto.advClientesChecklistId.Value, out var displayName))
                {
                    dto.advClientesChecklistDisplayName = displayName;
                }
            }
        }
        var advProcessosIds = entities
            .Where(x => x.advProcessosId != null)
            .Select(x => x.advProcessosId.Value)
            .Distinct()
            .ToList();

        if (advProcessosIds.Any())
        {
            var parents = await _advProcessosRepository.GetListAsync(x => advProcessosIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.sintese);

            foreach (var dto in dtoList.Where(x => x.advProcessosId != null))
            {
                if (parentMap.TryGetValue(dto.advProcessosId.Value, out var displayName))
                {
                    dto.advProcessosDisplayName = displayName;
                }
            }
        }
        var advProcessosClientesIds = entities
            .Select(x => x.advProcessosClientesId)
            .Distinct()
            .ToList();

        if (advProcessosClientesIds.Any())
        {
            var parents = await _advProcessosClientesRepository.GetListAsync(x => advProcessosClientesIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Id.ToString());

            foreach (var dto in dtoList)
            {
                if (parentMap.TryGetValue(dto.advProcessosClientesId, out var displayName))
                {
                    dto.advProcessosClientesDisplayName = displayName;
                }
            }
        }
        var advClientesHistoricosIds = entities
            .Where(x => x.advClientesHistoricosId != null)
            .Select(x => x.advClientesHistoricosId.Value)
            .Distinct()
            .ToList();

        if (advClientesHistoricosIds.Any())
        {
            var parents = await _advClientesHistoricosRepository.GetListAsync(x => advClientesHistoricosIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.data);

            foreach (var dto in dtoList.Where(x => x.advClientesHistoricosId != null))
            {
                if (parentMap.TryGetValue(dto.advClientesHistoricosId.Value, out var displayName))
                {
                    dto.advClientesHistoricosDisplayName = displayName;
                }
            }
        }
        var opoOportunidadesIds = entities
            .Where(x => x.opoOportunidadesId != null)
            .Select(x => x.opoOportunidadesId.Value)
            .Distinct()
            .ToList();

        if (opoOportunidadesIds.Any())
        {
            var parents = await _opoOportunidadesRepository.GetListAsync(x => opoOportunidadesIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.titulo);

            foreach (var dto in dtoList.Where(x => x.opoOportunidadesId != null))
            {
                if (parentMap.TryGetValue(dto.opoOportunidadesId.Value, out var displayName))
                {
                    dto.opoOportunidadesDisplayName = displayName;
                }
            }
        }
        var flwFollowsIds = entities
            .Where(x => x.flwFollowsId != null)
            .Select(x => x.flwFollowsId.Value)
            .Distinct()
            .ToList();

        if (flwFollowsIds.Any())
        {
            var parents = await _flwFollowsRepository.GetListAsync(x => flwFollowsIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.data);

            foreach (var dto in dtoList.Where(x => x.flwFollowsId != null))
            {
                if (parentMap.TryGetValue(dto.flwFollowsId.Value, out var displayName))
                {
                    dto.flwFollowsDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<advClientesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advClientes
    /// </summary>
    [Authorize(advClientesPermissions.Create)]
    public virtual async Task<advClientesDto> CreateAsync(CreateUpdateadvClientesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvClientesDto, Sapienza.Lexus.advClientes.advClientes>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientes.advClientes, advClientesDto>(entity);
    }

    /// <summary>
    /// Updates an existing advClientes
    /// </summary>
    [Authorize(advClientesPermissions.Update)]
    public virtual async Task<advClientesDto> UpdateAsync(Guid id, CreateUpdateadvClientesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advClientes.advClientes), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientes.advClientes, advClientesDto>(entity);
    }

    /// <summary>
    /// Deletes a advClientes
    /// </summary>
    [Authorize(advClientesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvClientesLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advClientes.advClientes> ApplyFilters(IQueryable<Sapienza.Lexus.advClientes.advClientes> queryable, advClientesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.apelido.Contains(input.Filter) || x.nome.Contains(input.Filter) || x.email.Contains(input.Filter) || x.telCelular.Contains(input.Filter) || x.telCelularObs.Contains(input.Filter) || x.telFixo.Contains(input.Filter) || x.telFixoObs.Contains(input.Filter) || x.dataNascimento.Contains(input.Filter) || x.cpf.Contains(input.Filter) || x.rg.Contains(input.Filter) || x.ctps.Contains(input.Filter) || x.endereco.Contains(input.Filter) || x.numero.Contains(input.Filter) || x.complemento.Contains(input.Filter) || x.bairro.Contains(input.Filter) || x.cep.Contains(input.Filter) || x.estado.Contains(input.Filter) || x.cidade.Contains(input.Filter) || x.dataIngresso.Contains(input.Filter) || x.observacoes.Contains(input.Filter) || x.naturalEstado.Contains(input.Filter) || x.naturalCidade.Contains(input.Filter) || x.nomeDaMae.Contains(input.Filter) || x.dibData.Contains(input.Filter) || x.telCelular2.Contains(input.Filter) || x.telCelular2Obs.Contains(input.Filter) || x.telFixo2.Contains(input.Filter) || x.telFixo2Obs.Contains(input.Filter) || x.cnpj.Contains(input.Filter) || x.ie.Contains(input.Filter) || x.incluidoPor.Contains(input.Filter) || x.inssData.Contains(input.Filter) || x.inssResultado.Contains(input.Filter) || x.pastaFTP.Contains(input.Filter) || x.inssProtocolo.Contains(input.Filter) || x.foto.Contains(input.Filter) || x.followBloqueadoAte.Contains(input.Filter) || x.senhaINSSDigital.Contains(input.Filter) || x.instagram.Contains(input.Filter) || x.rgOrgaoExp.Contains(input.Filter) || x.nacionalidade.Contains(input.Filter) || x.estadocivil.Contains(input.Filter) || x.dcbData.Contains(input.Filter))
            .WhereIf(input.idCliente != null, x => x.idCliente == input.idCliente)
            .WhereIf(!input.apelido.IsNullOrWhiteSpace(), x => x.apelido.Contains(input.apelido))
            .WhereIf(input.idGrupo != null, x => x.idGrupo == input.idGrupo)
            .WhereIf(input.idSituacao != null, x => x.idSituacao == input.idSituacao)
            .WhereIf(!input.nome.IsNullOrWhiteSpace(), x => x.nome.Contains(input.nome))
            .WhereIf(!input.email.IsNullOrWhiteSpace(), x => x.email.Contains(input.email))
            .WhereIf(!input.telCelular.IsNullOrWhiteSpace(), x => x.telCelular.Contains(input.telCelular))
            .WhereIf(!input.telCelularObs.IsNullOrWhiteSpace(), x => x.telCelularObs.Contains(input.telCelularObs))
            .WhereIf(!input.telFixo.IsNullOrWhiteSpace(), x => x.telFixo.Contains(input.telFixo))
            .WhereIf(!input.telFixoObs.IsNullOrWhiteSpace(), x => x.telFixoObs.Contains(input.telFixoObs))
            .WhereIf(!input.dataNascimento.IsNullOrWhiteSpace(), x => x.dataNascimento.Contains(input.dataNascimento))
            .WhereIf(!input.cpf.IsNullOrWhiteSpace(), x => x.cpf.Contains(input.cpf))
            .WhereIf(!input.rg.IsNullOrWhiteSpace(), x => x.rg.Contains(input.rg))
            .WhereIf(!input.ctps.IsNullOrWhiteSpace(), x => x.ctps.Contains(input.ctps))
            .WhereIf(!input.endereco.IsNullOrWhiteSpace(), x => x.endereco.Contains(input.endereco))
            .WhereIf(!input.numero.IsNullOrWhiteSpace(), x => x.numero.Contains(input.numero))
            .WhereIf(!input.complemento.IsNullOrWhiteSpace(), x => x.complemento.Contains(input.complemento))
            .WhereIf(!input.bairro.IsNullOrWhiteSpace(), x => x.bairro.Contains(input.bairro))
            .WhereIf(!input.cep.IsNullOrWhiteSpace(), x => x.cep.Contains(input.cep))
            .WhereIf(!input.estado.IsNullOrWhiteSpace(), x => x.estado.Contains(input.estado))
            .WhereIf(!input.cidade.IsNullOrWhiteSpace(), x => x.cidade.Contains(input.cidade))
            .WhereIf(!input.dataIngresso.IsNullOrWhiteSpace(), x => x.dataIngresso.Contains(input.dataIngresso))
            .WhereIf(!input.observacoes.IsNullOrWhiteSpace(), x => x.observacoes.Contains(input.observacoes))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(!input.naturalEstado.IsNullOrWhiteSpace(), x => x.naturalEstado.Contains(input.naturalEstado))
            .WhereIf(!input.naturalCidade.IsNullOrWhiteSpace(), x => x.naturalCidade.Contains(input.naturalCidade))
            .WhereIf(!input.nomeDaMae.IsNullOrWhiteSpace(), x => x.nomeDaMae.Contains(input.nomeDaMae))
            .WhereIf(input.dib != null, x => x.dib == input.dib)
            .WhereIf(!input.dibData.IsNullOrWhiteSpace(), x => x.dibData.Contains(input.dibData))
            .WhereIf(input.dibIdTipoBeneficio != null, x => x.dibIdTipoBeneficio == input.dibIdTipoBeneficio)
            .WhereIf(input.idCargo != null, x => x.idCargo == input.idCargo)
            .WhereIf(!input.telCelular2.IsNullOrWhiteSpace(), x => x.telCelular2.Contains(input.telCelular2))
            .WhereIf(!input.telCelular2Obs.IsNullOrWhiteSpace(), x => x.telCelular2Obs.Contains(input.telCelular2Obs))
            .WhereIf(!input.telFixo2.IsNullOrWhiteSpace(), x => x.telFixo2.Contains(input.telFixo2))
            .WhereIf(!input.telFixo2Obs.IsNullOrWhiteSpace(), x => x.telFixo2Obs.Contains(input.telFixo2Obs))
            .WhereIf(!input.cnpj.IsNullOrWhiteSpace(), x => x.cnpj.Contains(input.cnpj))
            .WhereIf(!input.ie.IsNullOrWhiteSpace(), x => x.ie.Contains(input.ie))
            .WhereIf(input.idFornecedor != null, x => x.idFornecedor == input.idFornecedor)
            .WhereIf(!input.incluidoPor.IsNullOrWhiteSpace(), x => x.incluidoPor.Contains(input.incluidoPor))
            .WhereIf(input.inssAgendado != null, x => x.inssAgendado == input.inssAgendado)
            .WhereIf(!input.inssData.IsNullOrWhiteSpace(), x => x.inssData.Contains(input.inssData))
            .WhereIf(input.inssIdTipoBeneficio != null, x => x.inssIdTipoBeneficio == input.inssIdTipoBeneficio)
            .WhereIf(input.inssIdPosto != null, x => x.inssIdPosto == input.inssIdPosto)
            .WhereIf(!input.inssResultado.IsNullOrWhiteSpace(), x => x.inssResultado.Contains(input.inssResultado))
            .WhereIf(input.prospect != null, x => x.prospect == input.prospect)
            .WhereIf(input.idLocalAtendido != null, x => x.idLocalAtendido == input.idLocalAtendido)
            .WhereIf(input.whatsapp != null, x => x.whatsapp == input.whatsapp)
            .WhereIf(!input.pastaFTP.IsNullOrWhiteSpace(), x => x.pastaFTP.Contains(input.pastaFTP))
            .WhereIf(input.inssResponsavel != null, x => x.inssResponsavel == input.inssResponsavel)
            .WhereIf(input.responsavelPendencia != null, x => x.responsavelPendencia == input.responsavelPendencia)
            .WhereIf(input.comoChegou != null, x => x.comoChegou == input.comoChegou)
            .WhereIf(!input.inssProtocolo.IsNullOrWhiteSpace(), x => x.inssProtocolo.Contains(input.inssProtocolo))
            .WhereIf(input.inssTsInclusao != null, x => x.inssTsInclusao == input.inssTsInclusao)
            .WhereIf(input.inssIdUsuarioInclusao != null, x => x.inssIdUsuarioInclusao == input.inssIdUsuarioInclusao)
            .WhereIf(!input.foto.IsNullOrWhiteSpace(), x => x.foto.Contains(input.foto))
            .WhereIf(!input.followBloqueadoAte.IsNullOrWhiteSpace(), x => x.followBloqueadoAte.Contains(input.followBloqueadoAte))
            .WhereIf(input.falecido != null, x => x.falecido == input.falecido)
            .WhereIf(!input.senhaINSSDigital.IsNullOrWhiteSpace(), x => x.senhaINSSDigital.Contains(input.senhaINSSDigital))
            .WhereIf(input.idPrioridade != null, x => x.idPrioridade == input.idPrioridade)
            .WhereIf(!input.instagram.IsNullOrWhiteSpace(), x => x.instagram.Contains(input.instagram))
            .WhereIf(!input.rgOrgaoExp.IsNullOrWhiteSpace(), x => x.rgOrgaoExp.Contains(input.rgOrgaoExp))
            .WhereIf(!input.nacionalidade.IsNullOrWhiteSpace(), x => x.nacionalidade.Contains(input.nacionalidade))
            .WhereIf(!input.estadocivil.IsNullOrWhiteSpace(), x => x.estadocivil.Contains(input.estadocivil))
            .WhereIf(input.dcb != null, x => x.dcb == input.dcb)
            .WhereIf(!input.dcbData.IsNullOrWhiteSpace(), x => x.dcbData.Contains(input.dcbData))
            .WhereIf(input.finIdUnidade != null, x => x.finIdUnidade == input.finIdUnidade)
            .WhereIf(input.finIdCentroCusto != null, x => x.finIdCentroCusto == input.finIdCentroCusto)
            // ========== FK Filters ==========
            .WhereIf(input.advClientesArquivosId != null, x => x.advClientesArquivosId == input.advClientesArquivosId)
            .WhereIf(input.advClientesAtualizacoesId != null, x => x.advClientesAtualizacoesId == input.advClientesAtualizacoesId)
            .WhereIf(input.advClientesChecklistId != null, x => x.advClientesChecklistId == input.advClientesChecklistId)
            .WhereIf(input.advProcessosId != null, x => x.advProcessosId == input.advProcessosId)
            .WhereIf(input.advProcessosClientesId != null, x => x.advProcessosClientesId == input.advProcessosClientesId)
            .WhereIf(input.advClientesHistoricosId != null, x => x.advClientesHistoricosId == input.advClientesHistoricosId)
            .WhereIf(input.opoOportunidadesId != null, x => x.opoOportunidadesId == input.opoOportunidadesId)
            .WhereIf(input.flwFollowsId != null, x => x.flwFollowsId == input.flwFollowsId)
            ;
    }
}
