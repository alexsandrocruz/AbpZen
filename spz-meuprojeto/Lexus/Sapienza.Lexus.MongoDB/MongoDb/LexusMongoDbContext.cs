using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Chat.MongoDB;
using Volo.FileManagement.MongoDB;
using Sapienza.Lexus.Lawyer;
using MongoDB.Driver;

namespace Sapienza.Lexus.MongoDB
{
    [ConnectionStringName("Default")]
    public class LexusMongoDbContext : AbpMongoDbContext
    {

        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */
        public IMongoCollection<Sapienza.Lexus.Lawyer.Lawyer> Lawyers => Collection<Sapienza.Lexus.Lawyer.Lawyer>();
        public IMongoCollection<Sapienza.Lexus.Client.Client> Clients => Collection<Sapienza.Lexus.Client.Client>();
        public IMongoCollection<Sapienza.Lexus.Specialization.Specialization> Specializations => Collection<Sapienza.Lexus.Specialization.Specialization>();
        public IMongoCollection<Sapienza.Lexus.LegalProcess.LegalProcess> LegalProcesses => Collection<Sapienza.Lexus.LegalProcess.LegalProcess>();
        public IMongoCollection<Sapienza.Lexus.LawyerSpecialization.LawyerSpecialization> LawyerSpecializations => Collection<Sapienza.Lexus.LawyerSpecialization.LawyerSpecialization>();
        public IMongoCollection<Sapienza.Lexus.Proposal.Proposal> Proposals => Collection<Sapienza.Lexus.Proposal.Proposal>();
                public IMongoCollection<Sapienza.Lexus.PropostalItem.PropostalItem> PropostalItems => Collection<Sapienza.Lexus.PropostalItem.PropostalItem>();
              public IMongoCollection<Sapienza.Lexus._versaoBD._versaoBD> _versaoBDs => Collection<Sapienza.Lexus._versaoBD._versaoBD>();
              public IMongoCollection<Sapienza.Lexus.advCliBairros.advCliBairros> advCliBairroses => Collection<Sapienza.Lexus.advCliBairros.advCliBairros>();
              public IMongoCollection<Sapienza.Lexus.advCliComoChegou.advCliComoChegou> advCliComoChegous => Collection<Sapienza.Lexus.advCliComoChegou.advCliComoChegou>();
              public IMongoCollection<Sapienza.Lexus.advCliLocaisAtendido.advCliLocaisAtendido> advCliLocaisAtendidos => Collection<Sapienza.Lexus.advCliLocaisAtendido.advCliLocaisAtendido>();
              public IMongoCollection<Sapienza.Lexus.advCliLog.advCliLog> advCliLogs => Collection<Sapienza.Lexus.advCliLog.advCliLog>();
              public IMongoCollection<Sapienza.Lexus.advCliPrioridades.advCliPrioridades> advCliPrioridadeses => Collection<Sapienza.Lexus.advCliPrioridades.advCliPrioridades>();
              public IMongoCollection<Sapienza.Lexus.advClientesConvertidos.advClientesConvertidos> advClientesConvertidoses => Collection<Sapienza.Lexus.advClientesConvertidos.advClientesConvertidos>();
              public IMongoCollection<Sapienza.Lexus.advClientesModelos.advClientesModelos> advClientesModeloses => Collection<Sapienza.Lexus.advClientesModelos.advClientesModelos>();
              public IMongoCollection<Sapienza.Lexus.advClientes_bkp.advClientes_bkp> advClientes_bkps => Collection<Sapienza.Lexus.advClientes_bkp.advClientes_bkp>();
              public IMongoCollection<Sapienza.Lexus.advPautaObs.advPautaObs> advPautaObses => Collection<Sapienza.Lexus.advPautaObs.advPautaObs>();
              public IMongoCollection<Sapienza.Lexus.advPreArquivosStatus.advPreArquivosStatus> advPreArquivosStatuses => Collection<Sapienza.Lexus.advPreArquivosStatus.advPreArquivosStatus>();
              public IMongoCollection<Sapienza.Lexus.advPreLogStatus.advPreLogStatus> advPreLogStatuses => Collection<Sapienza.Lexus.advPreLogStatus.advPreLogStatus>();
              public IMongoCollection<Sapienza.Lexus.advPreMetas.advPreMetas> advPreMetases => Collection<Sapienza.Lexus.advPreMetas.advPreMetas>();
              public IMongoCollection<Sapienza.Lexus.advPreMotivosPerda.advPreMotivosPerda> advPreMotivosPerdas => Collection<Sapienza.Lexus.advPreMotivosPerda.advPreMotivosPerda>();
              public IMongoCollection<Sapienza.Lexus.advPreOrigens.advPreOrigens> advPreOrigenses => Collection<Sapienza.Lexus.advPreOrigens.advPreOrigens>();
              public IMongoCollection<Sapienza.Lexus.advPreProcessosCheckLists.advPreProcessosCheckLists> advPreProcessosCheckListses => Collection<Sapienza.Lexus.advPreProcessosCheckLists.advPreProcessosCheckLists>();
              public IMongoCollection<Sapienza.Lexus.advProEscritorios.advProEscritorios> advProEscritorioses => Collection<Sapienza.Lexus.advProEscritorios.advProEscritorios>();
              public IMongoCollection<Sapienza.Lexus.advProFases.advProFases> advProFaseses => Collection<Sapienza.Lexus.advProFases.advProFases>();
              public IMongoCollection<Sapienza.Lexus.advProInstancias.advProInstancias> advProInstanciases => Collection<Sapienza.Lexus.advProInstancias.advProInstancias>();
              public IMongoCollection<Sapienza.Lexus.advProOrgaos.advProOrgaos> advProOrgaoses => Collection<Sapienza.Lexus.advProOrgaos.advProOrgaos>();
              public IMongoCollection<Sapienza.Lexus.advProProbabilidades.advProProbabilidades> advProProbabilidadeses => Collection<Sapienza.Lexus.advProProbabilidades.advProProbabilidades>();
              public IMongoCollection<Sapienza.Lexus.advProRelevancias.advProRelevancias> advProRelevanciases => Collection<Sapienza.Lexus.advProRelevancias.advProRelevancias>();
              public IMongoCollection<Sapienza.Lexus.advProSentencas.advProSentencas> advProSentencases => Collection<Sapienza.Lexus.advProSentencas.advProSentencas>();
              public IMongoCollection<Sapienza.Lexus.advProStatus.advProStatus> advProStatuses => Collection<Sapienza.Lexus.advProStatus.advProStatus>();
              public IMongoCollection<Sapienza.Lexus.advProTipos.advProTipos> advProTiposes => Collection<Sapienza.Lexus.advProTipos.advProTipos>();
              public IMongoCollection<Sapienza.Lexus.advProVaras.advProVaras> advProVarases => Collection<Sapienza.Lexus.advProVaras.advProVaras>();
              public IMongoCollection<Sapienza.Lexus.advTarefasAtualizacoes.advTarefasAtualizacoes> advTarefasAtualizacoeses => Collection<Sapienza.Lexus.advTarefasAtualizacoes.advTarefasAtualizacoes>();
              public IMongoCollection<Sapienza.Lexus.autoFTP.autoFTP> autoFTPs => Collection<Sapienza.Lexus.autoFTP.autoFTP>();
              public IMongoCollection<Sapienza.Lexus.fabConfig.fabConfig> fabConfigs => Collection<Sapienza.Lexus.fabConfig.fabConfig>();
              public IMongoCollection<Sapienza.Lexus.fabDatasEFeriados.fabDatasEFeriados> fabDatasEFeriadoses => Collection<Sapienza.Lexus.fabDatasEFeriados.fabDatasEFeriados>();
              public IMongoCollection<Sapienza.Lexus.fabFormasPagamento.fabFormasPagamento> fabFormasPagamentos => Collection<Sapienza.Lexus.fabFormasPagamento.fabFormasPagamento>();
              public IMongoCollection<Sapienza.Lexus.fabFormasRecebimento.fabFormasRecebimento> fabFormasRecebimentos => Collection<Sapienza.Lexus.fabFormasRecebimento.fabFormasRecebimento>();
              public IMongoCollection<Sapienza.Lexus.fabMotivosAproveitamento.fabMotivosAproveitamento> fabMotivosAproveitamentos => Collection<Sapienza.Lexus.fabMotivosAproveitamento.fabMotivosAproveitamento>();
              public IMongoCollection<Sapienza.Lexus.fabMotivosPerda.fabMotivosPerda> fabMotivosPerdas => Collection<Sapienza.Lexus.fabMotivosPerda.fabMotivosPerda>();
              public IMongoCollection<Sapienza.Lexus.fabRegioes.fabRegioes> fabRegioeses => Collection<Sapienza.Lexus.fabRegioes.fabRegioes>();
              public IMongoCollection<Sapienza.Lexus.fdtDevs.fdtDevs> fdtDevses => Collection<Sapienza.Lexus.fdtDevs.fdtDevs>();
              public IMongoCollection<Sapienza.Lexus.finCentrosResultado.finCentrosResultado> finCentrosResultados => Collection<Sapienza.Lexus.finCentrosResultado.finCentrosResultado>();
              public IMongoCollection<Sapienza.Lexus.finContasClientes.finContasClientes> finContasClienteses => Collection<Sapienza.Lexus.finContasClientes.finContasClientes>();
              public IMongoCollection<Sapienza.Lexus.finLancamentos_BKP.finLancamentos_BKP> finLancamentos_BKPs => Collection<Sapienza.Lexus.finLancamentos_BKP.finLancamentos_BKP>();
              public IMongoCollection<Sapienza.Lexus.finPlanoContasDet.finPlanoContasDet> finPlanoContasDets => Collection<Sapienza.Lexus.finPlanoContasDet.finPlanoContasDet>();
              public IMongoCollection<Sapienza.Lexus.finProcuracoesRPV.finProcuracoesRPV> finProcuracoesRPVs => Collection<Sapienza.Lexus.finProcuracoesRPV.finProcuracoesRPV>();
              public IMongoCollection<Sapienza.Lexus.finRateios.finRateios> finRateioses => Collection<Sapienza.Lexus.finRateios.finRateios>();
              public IMongoCollection<Sapienza.Lexus.finRateiosPadrao.finRateiosPadrao> finRateiosPadraos => Collection<Sapienza.Lexus.finRateiosPadrao.finRateiosPadrao>();
              public IMongoCollection<Sapienza.Lexus.finRecibos.finRecibos> finReciboses => Collection<Sapienza.Lexus.finRecibos.finRecibos>();
              public IMongoCollection<Sapienza.Lexus.finUnidades.finUnidades> finUnidadeses => Collection<Sapienza.Lexus.finUnidades.finUnidades>();
              public IMongoCollection<Sapienza.Lexus.flwConfig.flwConfig> flwConfigs => Collection<Sapienza.Lexus.flwConfig.flwConfig>();
              public IMongoCollection<Sapienza.Lexus.advAgeTiposCompromissos.advAgeTiposCompromissos> advAgeTiposCompromissoses => Collection<Sapienza.Lexus.advAgeTiposCompromissos.advAgeTiposCompromissos>();
              public IMongoCollection<Sapienza.Lexus.advAgeTiposTarefas.advAgeTiposTarefas> advAgeTiposTarefases => Collection<Sapienza.Lexus.advAgeTiposTarefas.advAgeTiposTarefas>();
              public IMongoCollection<Sapienza.Lexus.advCliCargos.advCliCargos> advCliCargoses => Collection<Sapienza.Lexus.advCliCargos.advCliCargos>();
              public IMongoCollection<Sapienza.Lexus.advCliGrupos.advCliGrupos> advCliGruposes => Collection<Sapienza.Lexus.advCliGrupos.advCliGrupos>();
              public IMongoCollection<Sapienza.Lexus.advCliSituacoes.advCliSituacoes> advCliSituacoeses => Collection<Sapienza.Lexus.advCliSituacoes.advCliSituacoes>();
              public IMongoCollection<Sapienza.Lexus.advCliTiposArquivos.advCliTiposArquivos> advCliTiposArquivoses => Collection<Sapienza.Lexus.advCliTiposArquivos.advCliTiposArquivos>();
              public IMongoCollection<Sapienza.Lexus.advCliTiposHistoricos.advCliTiposHistoricos> advCliTiposHistoricoses => Collection<Sapienza.Lexus.advCliTiposHistoricos.advCliTiposHistoricos>();
              public IMongoCollection<Sapienza.Lexus.advClientesINSSStatus.advClientesINSSStatus> advClientesINSSStatuses => Collection<Sapienza.Lexus.advClientesINSSStatus.advClientesINSSStatus>();
              public IMongoCollection<Sapienza.Lexus.advFornecedores.advFornecedores> advFornecedoreses => Collection<Sapienza.Lexus.advFornecedores.advFornecedores>();
              public IMongoCollection<Sapienza.Lexus.advPostosINSS.advPostosINSS> advPostosINSSes => Collection<Sapienza.Lexus.advPostosINSS.advPostosINSS>();
              public IMongoCollection<Sapienza.Lexus.advPreCheckListsGrupos.advPreCheckListsGrupos> advPreCheckListsGruposes => Collection<Sapienza.Lexus.advPreCheckListsGrupos.advPreCheckListsGrupos>();
              public IMongoCollection<Sapienza.Lexus.advPreStatusTipos.advPreStatusTipos> advPreStatusTiposes => Collection<Sapienza.Lexus.advPreStatusTipos.advPreStatusTipos>();
              public IMongoCollection<Sapienza.Lexus.advProMeritos.advProMeritos> advProMeritoses => Collection<Sapienza.Lexus.advProMeritos.advProMeritos>();
              public IMongoCollection<Sapienza.Lexus.advProNaturezas.advProNaturezas> advProNaturezases => Collection<Sapienza.Lexus.advProNaturezas.advProNaturezas>();
              public IMongoCollection<Sapienza.Lexus.advVerTipos.advVerTipos> advVerTiposes => Collection<Sapienza.Lexus.advVerTipos.advVerTipos>();
              public IMongoCollection<Sapienza.Lexus.fabCondicoesPagamento.fabCondicoesPagamento> fabCondicoesPagamentos => Collection<Sapienza.Lexus.fabCondicoesPagamento.fabCondicoesPagamento>();
              public IMongoCollection<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos> fabHistoricoTiposes => Collection<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos>();
              public IMongoCollection<Sapienza.Lexus.fabPaises.fabPaises> fabPaiseses => Collection<Sapienza.Lexus.fabPaises.fabPaises>();
              public IMongoCollection<Sapienza.Lexus.fabPermissoesTipos.fabPermissoesTipos> fabPermissoesTiposes => Collection<Sapienza.Lexus.fabPermissoesTipos.fabPermissoesTipos>();
              public IMongoCollection<Sapienza.Lexus.finAreas.finAreas> finAreases => Collection<Sapienza.Lexus.finAreas.finAreas>();
              public IMongoCollection<Sapienza.Lexus.finCentrosCusto.finCentrosCusto> finCentrosCustos => Collection<Sapienza.Lexus.finCentrosCusto.finCentrosCusto>();
              public IMongoCollection<Sapienza.Lexus.finContas.finContas> finContases => Collection<Sapienza.Lexus.finContas.finContas>();
              public IMongoCollection<Sapienza.Lexus.finGruposDRE.finGruposDRE> finGruposDREs => Collection<Sapienza.Lexus.finGruposDRE.finGruposDRE>();
              public IMongoCollection<Sapienza.Lexus.flwAcoes.flwAcoes> flwAcoeses => Collection<Sapienza.Lexus.flwAcoes.flwAcoes>();
              public IMongoCollection<Sapienza.Lexus.logAcoes.logAcoes> logAcoeses => Collection<Sapienza.Lexus.logAcoes.logAcoes>();
              public IMongoCollection<Sapienza.Lexus.opoSituacoes.opoSituacoes> opoSituacoeses => Collection<Sapienza.Lexus.opoSituacoes.opoSituacoes>();
              public IMongoCollection<Sapienza.Lexus.opoTipos.opoTipos> opoTiposes => Collection<Sapienza.Lexus.opoTipos.opoTipos>();
              public IMongoCollection<Sapienza.Lexus.usuAreas.usuAreas> usuAreases => Collection<Sapienza.Lexus.usuAreas.usuAreas>();
              public IMongoCollection<Sapienza.Lexus.advClientes.advClientes> advClienteses => Collection<Sapienza.Lexus.advClientes.advClientes>();
              public IMongoCollection<Sapienza.Lexus.advClientesArquivos.advClientesArquivos> advClientesArquivoses => Collection<Sapienza.Lexus.advClientesArquivos.advClientesArquivos>();
              public IMongoCollection<Sapienza.Lexus.advClientesAtualizacoes.advClientesAtualizacoes> advClientesAtualizacoeses => Collection<Sapienza.Lexus.advClientesAtualizacoes.advClientesAtualizacoes>();
              public IMongoCollection<Sapienza.Lexus.advClientesChecklist.advClientesChecklist> advClientesChecklists => Collection<Sapienza.Lexus.advClientesChecklist.advClientesChecklist>();
              public IMongoCollection<Sapienza.Lexus.advClientesINSS.advClientesINSS> advClientesINSSes => Collection<Sapienza.Lexus.advClientesINSS.advClientesINSS>();
              public IMongoCollection<Sapienza.Lexus.advPreCheckLists.advPreCheckLists> advPreCheckListses => Collection<Sapienza.Lexus.advPreCheckLists.advPreCheckLists>();
              public IMongoCollection<Sapienza.Lexus.advPreStatus.advPreStatus> advPreStatuses => Collection<Sapienza.Lexus.advPreStatus.advPreStatus>();
              public IMongoCollection<Sapienza.Lexus.advProcessos.advProcessos> advProcessoses => Collection<Sapienza.Lexus.advProcessos.advProcessos>();
              public IMongoCollection<Sapienza.Lexus.advProcessosClientes.advProcessosClientes> advProcessosClienteses => Collection<Sapienza.Lexus.advProcessosClientes.advProcessosClientes>();
              public IMongoCollection<Sapienza.Lexus.advProcessosHonorarios.advProcessosHonorarios> advProcessosHonorarioses => Collection<Sapienza.Lexus.advProcessosHonorarios.advProcessosHonorarios>();
              public IMongoCollection<Sapienza.Lexus.advProcessosMeritos.advProcessosMeritos> advProcessosMeritoses => Collection<Sapienza.Lexus.advProcessosMeritos.advProcessosMeritos>();
              public IMongoCollection<Sapienza.Lexus.fabEstados.fabEstados> fabEstadoses => Collection<Sapienza.Lexus.fabEstados.fabEstados>();
              public IMongoCollection<Sapienza.Lexus.fabPermissoes.fabPermissoes> fabPermissoeses => Collection<Sapienza.Lexus.fabPermissoes.fabPermissoes>();
              public IMongoCollection<Sapienza.Lexus.finExtrato.finExtrato> finExtratos => Collection<Sapienza.Lexus.finExtrato.finExtrato>();
              public IMongoCollection<Sapienza.Lexus.finPlanoContasGrupos.finPlanoContasGrupos> finPlanoContasGruposes => Collection<Sapienza.Lexus.finPlanoContasGrupos.finPlanoContasGrupos>();
              public IMongoCollection<Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes> flwConfigExcecoeses => Collection<Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes>();
              public IMongoCollection<Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios> flwGradeHorarioses => Collection<Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios>();
              public IMongoCollection<Sapienza.Lexus.logCampos.logCampos> logCamposes => Collection<Sapienza.Lexus.logCampos.logCampos>();
              public IMongoCollection<Sapienza.Lexus.usuCargos.usuCargos> usuCargoses => Collection<Sapienza.Lexus.usuCargos.usuCargos>();
              public IMongoCollection<Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos> advClientesHistoricoses => Collection<Sapienza.Lexus.advClientesHistoricos.advClientesHistoricos>();
              public IMongoCollection<Sapienza.Lexus.advCompromissos.advCompromissos> advCompromissoses => Collection<Sapienza.Lexus.advCompromissos.advCompromissos>();
              public IMongoCollection<Sapienza.Lexus.advProcessosAlteracoes.advProcessosAlteracoes> advProcessosAlteracoeses => Collection<Sapienza.Lexus.advProcessosAlteracoes.advProcessosAlteracoes>();
              public IMongoCollection<Sapienza.Lexus.advProcessosDadosHerdeiros.advProcessosDadosHerdeiros> advProcessosDadosHerdeiroses => Collection<Sapienza.Lexus.advProcessosDadosHerdeiros.advProcessosDadosHerdeiros>();
              public IMongoCollection<Sapienza.Lexus.advProfissionais.advProfissionais> advProfissionaises => Collection<Sapienza.Lexus.advProfissionais.advProfissionais>();
              public IMongoCollection<Sapienza.Lexus.advProfissionaisEstados.advProfissionaisEstados> advProfissionaisEstadoses => Collection<Sapienza.Lexus.advProfissionaisEstados.advProfissionaisEstados>();
              public IMongoCollection<Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas> advProfissionaisNaturezases => Collection<Sapienza.Lexus.advProfissionaisNaturezas.advProfissionaisNaturezas>();
              public IMongoCollection<Sapienza.Lexus.advRevisaoDocumentos.advRevisaoDocumentos> advRevisaoDocumentoses => Collection<Sapienza.Lexus.advRevisaoDocumentos.advRevisaoDocumentos>();
              public IMongoCollection<Sapienza.Lexus.advTarefas.advTarefas> advTarefases => Collection<Sapienza.Lexus.advTarefas.advTarefas>();
              public IMongoCollection<Sapienza.Lexus.advVerbas.advVerbas> advVerbases => Collection<Sapienza.Lexus.advVerbas.advVerbas>();
              public IMongoCollection<Sapienza.Lexus.fabCidades.fabCidades> fabCidadeses => Collection<Sapienza.Lexus.fabCidades.fabCidades>();
              public IMongoCollection<Sapienza.Lexus.fabLembretes.fabLembretes> fabLembreteses => Collection<Sapienza.Lexus.fabLembretes.fabLembretes>();
              public IMongoCollection<Sapienza.Lexus.finPlanoContas.finPlanoContas> finPlanoContases => Collection<Sapienza.Lexus.finPlanoContas.finPlanoContas>();
              public IMongoCollection<Sapienza.Lexus.opoOportunidades.opoOportunidades> opoOportunidadeses => Collection<Sapienza.Lexus.opoOportunidades.opoOportunidades>();
              public IMongoCollection<Sapienza.Lexus.opoOrcamentos.opoOrcamentos> opoOrcamentoses => Collection<Sapienza.Lexus.opoOrcamentos.opoOrcamentos>();
              public IMongoCollection<Sapienza.Lexus.usuAcessos.usuAcessos> usuAcessoses => Collection<Sapienza.Lexus.usuAcessos.usuAcessos>();
              public IMongoCollection<Sapienza.Lexus.usuDistancias.usuDistancias> usuDistanciases => Collection<Sapienza.Lexus.usuDistancias.usuDistancias>();
              public IMongoCollection<Sapienza.Lexus.finLancamentos.finLancamentos> finLancamentoses => Collection<Sapienza.Lexus.finLancamentos.finLancamentos>();
              public IMongoCollection<Sapienza.Lexus.finPrestacaoContas.finPrestacaoContas> finPrestacaoContases => Collection<Sapienza.Lexus.finPrestacaoContas.finPrestacaoContas>();
              public IMongoCollection<Sapienza.Lexus.flwFollows.flwFollows> flwFollowses => Collection<Sapienza.Lexus.flwFollows.flwFollows>();
      // <GEN-MONGODB-COLLECTIONS>

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureChat();
            modelBuilder.ConfigureFileManagement();
            //builder.Entity<YourEntity>(b =>
            //{
            //    //...
            //});
        }
    }
}
