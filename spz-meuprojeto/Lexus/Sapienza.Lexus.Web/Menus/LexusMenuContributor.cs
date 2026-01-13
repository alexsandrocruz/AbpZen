using System;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.Configuration;
using Sapienza.Lexus.Localization;
using Sapienza.Lexus.Permissions;
using Volo.Abp.Account.Localization;
using Volo.Abp.AuditLogging.Web.Navigation;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.LanguageManagement.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TextTemplateManagement.Web.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;
using Volo.Saas.Host.Navigation;
using Volo.Abp.OpenIddict.Pro.Web.Menus;

namespace Sapienza.Lexus.Web.Menus
{
    public class LexusMenuContributor : IMenuContributor
    {
        private readonly IConfiguration _configuration;

        public LexusMenuContributor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
            else if (context.Menu.Name == StandardMenus.User)
            {
                await ConfigureUserMenuAsync(context);
            }
        }

        private static Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var l = context.GetLocalizer<LexusResource>();
            //Home
            context.Menu.AddItem(
                new ApplicationMenuItem(
                    LexusMenus.Home,
                    l["Menu:Home"],
                    "~/",
                    icon: "fa fa-home",
                    order: 1
                )
            );

            //Host Dashboard
            context.Menu.AddItem(
                new ApplicationMenuItem(
                    LexusMenus.HostDashboard,
                    l["Menu:Dashboard"],
                    "~/HostDashboard",
                    icon: "fa fa-line-chart",
                    order: 2
                ).RequirePermissions(LexusPermissions.Dashboard.Host)
            );

            //Tenant Dashboard
            context.Menu.AddItem(
                new ApplicationMenuItem(
                    LexusMenus.TenantDashboard,
                    l["Menu:Dashboard"],
                    "~/Dashboard",
                    icon: "fa fa-line-chart",
                    order: 2
                ).RequirePermissions(LexusPermissions.Dashboard.Tenant)
            );

            //Saas
            context.Menu.SetSubItemOrder(SaasHostMenuNames.GroupName, 3);


            //Administration
            var administration = context.Menu.GetAdministration();
            administration.Order = 5;

            //Administration->Identity
            administration.SetSubItemOrder(IdentityMenuNames.GroupName, 1);

            //Administration->Identity Server
            administration.SetSubItemOrder(OpenIddictProMenus.GroupName, 2);

            //Administration->Language Management
            administration.SetSubItemOrder(LanguageManagementMenuNames.GroupName, 3);

            //Administration->Text Template Management
            administration.SetSubItemOrder(TextTemplateManagementMainMenuNames.GroupName, 4);

            //Administration->Audit Logs
            administration.SetSubItemOrder(AbpAuditLoggingMainMenuNames.GroupName, 5);

            //Administration->Settings
            administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 6);

                        context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.Lawyer, l["Menu:Lawyers"], "~/Lawyer", icon: "fa fa-folder-open").RequirePermissions(LawyerPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.Case, l["Menu:Cases"], "~/Case", icon: "fa fa-folder-open").RequirePermissions(CasePermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.Client, l["Menu:Clients"], "~/Client", icon: "fa fa-folder-open").RequirePermissions(ClientPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.Specialization, l["Menu:Specializations"], "~/Specialization", icon: "fa fa-folder-open").RequirePermissions(SpecializationPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.LegalProcess, l["Menu:LegalProcesses"], "~/LegalProcess", icon: "fa fa-folder-open").RequirePermissions(LegalProcessPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.LawyerSpecialization, l["Menu:LawyerSpecializations"], "~/LawyerSpecialization", icon: "fa fa-folder-open").RequirePermissions(LawyerSpecializationPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.Proposal, l["Menu:Proposals"], "~/Proposal", icon: "fa fa-folder-open").RequirePermissions(ProposalPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.PropostalItem, l["Menu:PropostalItems"], "~/PropostalItem", icon: "fa fa-folder-open").RequirePermissions(PropostalItemPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus._versaoBD, l["Menu:_versaoBDs"], "~/_versaoBD", icon: "fa fa-folder-open").RequirePermissions(_versaoBDPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advCliBairros, l["Menu:advCliBairroses"], "~/advCliBairros", icon: "fa fa-folder-open").RequirePermissions(advCliBairrosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advCliComoChegou, l["Menu:advCliComoChegous"], "~/advCliComoChegou", icon: "fa fa-folder-open").RequirePermissions(advCliComoChegouPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advCliLocaisAtendido, l["Menu:advCliLocaisAtendidos"], "~/advCliLocaisAtendido", icon: "fa fa-folder-open").RequirePermissions(advCliLocaisAtendidoPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advCliLog, l["Menu:advCliLogs"], "~/advCliLog", icon: "fa fa-folder-open").RequirePermissions(advCliLogPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advCliPrioridades, l["Menu:advCliPrioridadeses"], "~/advCliPrioridades", icon: "fa fa-folder-open").RequirePermissions(advCliPrioridadesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advClientesConvertidos, l["Menu:advClientesConvertidoses"], "~/advClientesConvertidos", icon: "fa fa-folder-open").RequirePermissions(advClientesConvertidosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advClientesModelos, l["Menu:advClientesModeloses"], "~/advClientesModelos", icon: "fa fa-folder-open").RequirePermissions(advClientesModelosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advClientes_bkp, l["Menu:advClientes_bkps"], "~/advClientes_bkp", icon: "fa fa-folder-open").RequirePermissions(advClientes_bkpPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advPautaObs, l["Menu:advPautaObses"], "~/advPautaObs", icon: "fa fa-folder-open").RequirePermissions(advPautaObsPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advPreArquivosStatus, l["Menu:advPreArquivosStatuses"], "~/advPreArquivosStatus", icon: "fa fa-folder-open").RequirePermissions(advPreArquivosStatusPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advPreLogStatus, l["Menu:advPreLogStatuses"], "~/advPreLogStatus", icon: "fa fa-folder-open").RequirePermissions(advPreLogStatusPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advPreMetas, l["Menu:advPreMetases"], "~/advPreMetas", icon: "fa fa-folder-open").RequirePermissions(advPreMetasPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advPreMotivosPerda, l["Menu:advPreMotivosPerdas"], "~/advPreMotivosPerda", icon: "fa fa-folder-open").RequirePermissions(advPreMotivosPerdaPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advPreOrigens, l["Menu:advPreOrigenses"], "~/advPreOrigens", icon: "fa fa-folder-open").RequirePermissions(advPreOrigensPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advPreProcessosCheckLists, l["Menu:advPreProcessosCheckListses"], "~/advPreProcessosCheckLists", icon: "fa fa-folder-open").RequirePermissions(advPreProcessosCheckListsPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProEscritorios, l["Menu:advProEscritorioses"], "~/advProEscritorios", icon: "fa fa-folder-open").RequirePermissions(advProEscritoriosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProFases, l["Menu:advProFaseses"], "~/advProFases", icon: "fa fa-folder-open").RequirePermissions(advProFasesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProInstancias, l["Menu:advProInstanciases"], "~/advProInstancias", icon: "fa fa-folder-open").RequirePermissions(advProInstanciasPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProOrgaos, l["Menu:advProOrgaoses"], "~/advProOrgaos", icon: "fa fa-folder-open").RequirePermissions(advProOrgaosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProProbabilidades, l["Menu:advProProbabilidadeses"], "~/advProProbabilidades", icon: "fa fa-folder-open").RequirePermissions(advProProbabilidadesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProRelevancias, l["Menu:advProRelevanciases"], "~/advProRelevancias", icon: "fa fa-folder-open").RequirePermissions(advProRelevanciasPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProSentencas, l["Menu:advProSentencases"], "~/advProSentencas", icon: "fa fa-folder-open").RequirePermissions(advProSentencasPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProStatus, l["Menu:advProStatuses"], "~/advProStatus", icon: "fa fa-folder-open").RequirePermissions(advProStatusPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProTipos, l["Menu:advProTiposes"], "~/advProTipos", icon: "fa fa-folder-open").RequirePermissions(advProTiposPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProVaras, l["Menu:advProVarases"], "~/advProVaras", icon: "fa fa-folder-open").RequirePermissions(advProVarasPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advTarefasAtualizacoes, l["Menu:advTarefasAtualizacoeses"], "~/advTarefasAtualizacoes", icon: "fa fa-folder-open").RequirePermissions(advTarefasAtualizacoesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.autoFTP, l["Menu:autoFTPs"], "~/autoFTP", icon: "fa fa-folder-open").RequirePermissions(autoFTPPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.fabConfig, l["Menu:fabConfigs"], "~/fabConfig", icon: "fa fa-folder-open").RequirePermissions(fabConfigPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.fabDatasEFeriados, l["Menu:fabDatasEFeriadoses"], "~/fabDatasEFeriados", icon: "fa fa-folder-open").RequirePermissions(fabDatasEFeriadosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.fabFormasPagamento, l["Menu:fabFormasPagamentos"], "~/fabFormasPagamento", icon: "fa fa-folder-open").RequirePermissions(fabFormasPagamentoPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.fabFormasRecebimento, l["Menu:fabFormasRecebimentos"], "~/fabFormasRecebimento", icon: "fa fa-folder-open").RequirePermissions(fabFormasRecebimentoPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.fabMotivosAproveitamento, l["Menu:fabMotivosAproveitamentos"], "~/fabMotivosAproveitamento", icon: "fa fa-folder-open").RequirePermissions(fabMotivosAproveitamentoPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.fabMotivosPerda, l["Menu:fabMotivosPerdas"], "~/fabMotivosPerda", icon: "fa fa-folder-open").RequirePermissions(fabMotivosPerdaPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.fabRegioes, l["Menu:fabRegioeses"], "~/fabRegioes", icon: "fa fa-folder-open").RequirePermissions(fabRegioesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.fdtDevs, l["Menu:fdtDevses"], "~/fdtDevs", icon: "fa fa-folder-open").RequirePermissions(fdtDevsPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finCentrosResultado, l["Menu:finCentrosResultados"], "~/finCentrosResultado", icon: "fa fa-folder-open").RequirePermissions(finCentrosResultadoPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finContasClientes, l["Menu:finContasClienteses"], "~/finContasClientes", icon: "fa fa-folder-open").RequirePermissions(finContasClientesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finLancamentos_BKP, l["Menu:finLancamentos_BKPs"], "~/finLancamentos_BKP", icon: "fa fa-folder-open").RequirePermissions(finLancamentos_BKPPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finPlanoContasDet, l["Menu:finPlanoContasDets"], "~/finPlanoContasDet", icon: "fa fa-folder-open").RequirePermissions(finPlanoContasDetPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finProcuracoesRPV, l["Menu:finProcuracoesRPVs"], "~/finProcuracoesRPV", icon: "fa fa-folder-open").RequirePermissions(finProcuracoesRPVPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finRateios, l["Menu:finRateioses"], "~/finRateios", icon: "fa fa-folder-open").RequirePermissions(finRateiosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finRateiosPadrao, l["Menu:finRateiosPadraos"], "~/finRateiosPadrao", icon: "fa fa-folder-open").RequirePermissions(finRateiosPadraoPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finRecibos, l["Menu:finReciboses"], "~/finRecibos", icon: "fa fa-folder-open").RequirePermissions(finRecibosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finUnidades, l["Menu:finUnidadeses"], "~/finUnidades", icon: "fa fa-folder-open").RequirePermissions(finUnidadesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.flwConfig, l["Menu:flwConfigs"], "~/flwConfig", icon: "fa fa-folder-open").RequirePermissions(flwConfigPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advAgeTiposCompromissos, l["Menu:advAgeTiposCompromissoses"], "~/advAgeTiposCompromissos", icon: "fa fa-folder-open").RequirePermissions(advAgeTiposCompromissosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advAgeTiposTarefas, l["Menu:advAgeTiposTarefases"], "~/advAgeTiposTarefas", icon: "fa fa-folder-open").RequirePermissions(advAgeTiposTarefasPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advCliCargos, l["Menu:advCliCargoses"], "~/advCliCargos", icon: "fa fa-folder-open").RequirePermissions(advCliCargosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advCliGrupos, l["Menu:advCliGruposes"], "~/advCliGrupos", icon: "fa fa-folder-open").RequirePermissions(advCliGruposPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advCliSituacoes, l["Menu:advCliSituacoeses"], "~/advCliSituacoes", icon: "fa fa-folder-open").RequirePermissions(advCliSituacoesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advCliTiposArquivos, l["Menu:advCliTiposArquivoses"], "~/advCliTiposArquivos", icon: "fa fa-folder-open").RequirePermissions(advCliTiposArquivosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advCliTiposHistoricos, l["Menu:advCliTiposHistoricoses"], "~/advCliTiposHistoricos", icon: "fa fa-folder-open").RequirePermissions(advCliTiposHistoricosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advClientesINSSStatus, l["Menu:advClientesINSSStatuses"], "~/advClientesINSSStatus", icon: "fa fa-folder-open").RequirePermissions(advClientesINSSStatusPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advFornecedores, l["Menu:advFornecedoreses"], "~/advFornecedores", icon: "fa fa-folder-open").RequirePermissions(advFornecedoresPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advPostosINSS, l["Menu:advPostosINSSes"], "~/advPostosINSS", icon: "fa fa-folder-open").RequirePermissions(advPostosINSSPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advPreCheckListsGrupos, l["Menu:advPreCheckListsGruposes"], "~/advPreCheckListsGrupos", icon: "fa fa-folder-open").RequirePermissions(advPreCheckListsGruposPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advPreStatusTipos, l["Menu:advPreStatusTiposes"], "~/advPreStatusTipos", icon: "fa fa-folder-open").RequirePermissions(advPreStatusTiposPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProMeritos, l["Menu:advProMeritoses"], "~/advProMeritos", icon: "fa fa-folder-open").RequirePermissions(advProMeritosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProNaturezas, l["Menu:advProNaturezases"], "~/advProNaturezas", icon: "fa fa-folder-open").RequirePermissions(advProNaturezasPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advVerTipos, l["Menu:advVerTiposes"], "~/advVerTipos", icon: "fa fa-folder-open").RequirePermissions(advVerTiposPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.fabCondicoesPagamento, l["Menu:fabCondicoesPagamentos"], "~/fabCondicoesPagamento", icon: "fa fa-folder-open").RequirePermissions(fabCondicoesPagamentoPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.fabHistoricoTipos, l["Menu:fabHistoricoTiposes"], "~/fabHistoricoTipos", icon: "fa fa-folder-open").RequirePermissions(fabHistoricoTiposPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.fabPaises, l["Menu:fabPaiseses"], "~/fabPaises", icon: "fa fa-folder-open").RequirePermissions(fabPaisesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.fabPermissoesTipos, l["Menu:fabPermissoesTiposes"], "~/fabPermissoesTipos", icon: "fa fa-folder-open").RequirePermissions(fabPermissoesTiposPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finAreas, l["Menu:finAreases"], "~/finAreas", icon: "fa fa-folder-open").RequirePermissions(finAreasPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finCentrosCusto, l["Menu:finCentrosCustos"], "~/finCentrosCusto", icon: "fa fa-folder-open").RequirePermissions(finCentrosCustoPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finContas, l["Menu:finContases"], "~/finContas", icon: "fa fa-folder-open").RequirePermissions(finContasPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finGruposDRE, l["Menu:finGruposDREs"], "~/finGruposDRE", icon: "fa fa-folder-open").RequirePermissions(finGruposDREPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.flwAcoes, l["Menu:flwAcoeses"], "~/flwAcoes", icon: "fa fa-folder-open").RequirePermissions(flwAcoesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.logAcoes, l["Menu:logAcoeses"], "~/logAcoes", icon: "fa fa-folder-open").RequirePermissions(logAcoesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.opoSituacoes, l["Menu:opoSituacoeses"], "~/opoSituacoes", icon: "fa fa-folder-open").RequirePermissions(opoSituacoesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.opoTipos, l["Menu:opoTiposes"], "~/opoTipos", icon: "fa fa-folder-open").RequirePermissions(opoTiposPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.usuAreas, l["Menu:usuAreases"], "~/usuAreas", icon: "fa fa-folder-open").RequirePermissions(usuAreasPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advClientes, l["Menu:advClienteses"], "~/advClientes", icon: "fa fa-folder-open").RequirePermissions(advClientesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advClientesArquivos, l["Menu:advClientesArquivoses"], "~/advClientesArquivos", icon: "fa fa-folder-open").RequirePermissions(advClientesArquivosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advClientesAtualizacoes, l["Menu:advClientesAtualizacoeses"], "~/advClientesAtualizacoes", icon: "fa fa-folder-open").RequirePermissions(advClientesAtualizacoesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advClientesChecklist, l["Menu:advClientesChecklists"], "~/advClientesChecklist", icon: "fa fa-folder-open").RequirePermissions(advClientesChecklistPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advClientesINSS, l["Menu:advClientesINSSes"], "~/advClientesINSS", icon: "fa fa-folder-open").RequirePermissions(advClientesINSSPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advPreCheckLists, l["Menu:advPreCheckListses"], "~/advPreCheckLists", icon: "fa fa-folder-open").RequirePermissions(advPreCheckListsPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advPreStatus, l["Menu:advPreStatuses"], "~/advPreStatus", icon: "fa fa-folder-open").RequirePermissions(advPreStatusPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProcessos, l["Menu:advProcessoses"], "~/advProcessos", icon: "fa fa-folder-open").RequirePermissions(advProcessosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProcessosClientes, l["Menu:advProcessosClienteses"], "~/advProcessosClientes", icon: "fa fa-folder-open").RequirePermissions(advProcessosClientesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProcessosHonorarios, l["Menu:advProcessosHonorarioses"], "~/advProcessosHonorarios", icon: "fa fa-folder-open").RequirePermissions(advProcessosHonorariosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProcessosMeritos, l["Menu:advProcessosMeritoses"], "~/advProcessosMeritos", icon: "fa fa-folder-open").RequirePermissions(advProcessosMeritosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.fabEstados, l["Menu:fabEstadoses"], "~/fabEstados", icon: "fa fa-folder-open").RequirePermissions(fabEstadosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.fabPermissoes, l["Menu:fabPermissoeses"], "~/fabPermissoes", icon: "fa fa-folder-open").RequirePermissions(fabPermissoesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finExtrato, l["Menu:finExtratos"], "~/finExtrato", icon: "fa fa-folder-open").RequirePermissions(finExtratoPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finPlanoContasGrupos, l["Menu:finPlanoContasGruposes"], "~/finPlanoContasGrupos", icon: "fa fa-folder-open").RequirePermissions(finPlanoContasGruposPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.flwConfigExcecoes, l["Menu:flwConfigExcecoeses"], "~/flwConfigExcecoes", icon: "fa fa-folder-open").RequirePermissions(flwConfigExcecoesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.flwGradeHorarios, l["Menu:flwGradeHorarioses"], "~/flwGradeHorarios", icon: "fa fa-folder-open").RequirePermissions(flwGradeHorariosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.logCampos, l["Menu:logCamposes"], "~/logCampos", icon: "fa fa-folder-open").RequirePermissions(logCamposPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.usuCargos, l["Menu:usuCargoses"], "~/usuCargos", icon: "fa fa-folder-open").RequirePermissions(usuCargosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advClientesHistoricos, l["Menu:advClientesHistoricoses"], "~/advClientesHistoricos", icon: "fa fa-folder-open").RequirePermissions(advClientesHistoricosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advCompromissos, l["Menu:advCompromissoses"], "~/advCompromissos", icon: "fa fa-folder-open").RequirePermissions(advCompromissosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProcessosAlteracoes, l["Menu:advProcessosAlteracoeses"], "~/advProcessosAlteracoes", icon: "fa fa-folder-open").RequirePermissions(advProcessosAlteracoesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProcessosDadosHerdeiros, l["Menu:advProcessosDadosHerdeiroses"], "~/advProcessosDadosHerdeiros", icon: "fa fa-folder-open").RequirePermissions(advProcessosDadosHerdeirosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProfissionais, l["Menu:advProfissionaises"], "~/advProfissionais", icon: "fa fa-folder-open").RequirePermissions(advProfissionaisPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProfissionaisEstados, l["Menu:advProfissionaisEstadoses"], "~/advProfissionaisEstados", icon: "fa fa-folder-open").RequirePermissions(advProfissionaisEstadosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advProfissionaisNaturezas, l["Menu:advProfissionaisNaturezases"], "~/advProfissionaisNaturezas", icon: "fa fa-folder-open").RequirePermissions(advProfissionaisNaturezasPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advRevisaoDocumentos, l["Menu:advRevisaoDocumentoses"], "~/advRevisaoDocumentos", icon: "fa fa-folder-open").RequirePermissions(advRevisaoDocumentosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advTarefas, l["Menu:advTarefases"], "~/advTarefas", icon: "fa fa-folder-open").RequirePermissions(advTarefasPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.advVerbas, l["Menu:advVerbases"], "~/advVerbas", icon: "fa fa-folder-open").RequirePermissions(advVerbasPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.fabCidades, l["Menu:fabCidadeses"], "~/fabCidades", icon: "fa fa-folder-open").RequirePermissions(fabCidadesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.fabLembretes, l["Menu:fabLembreteses"], "~/fabLembretes", icon: "fa fa-folder-open").RequirePermissions(fabLembretesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finPlanoContas, l["Menu:finPlanoContases"], "~/finPlanoContas", icon: "fa fa-folder-open").RequirePermissions(finPlanoContasPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.opoOportunidades, l["Menu:opoOportunidadeses"], "~/opoOportunidades", icon: "fa fa-folder-open").RequirePermissions(opoOportunidadesPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.opoOrcamentos, l["Menu:opoOrcamentoses"], "~/opoOrcamentos", icon: "fa fa-folder-open").RequirePermissions(opoOrcamentosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.usuAcessos, l["Menu:usuAcessoses"], "~/usuAcessos", icon: "fa fa-folder-open").RequirePermissions(usuAcessosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.usuDistancias, l["Menu:usuDistanciases"], "~/usuDistancias", icon: "fa fa-folder-open").RequirePermissions(usuDistanciasPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finLancamentos, l["Menu:finLancamentoses"], "~/finLancamentos", icon: "fa fa-folder-open").RequirePermissions(finLancamentosPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.finPrestacaoContas, l["Menu:finPrestacaoContases"], "~/finPrestacaoContas", icon: "fa fa-folder-open").RequirePermissions(finPrestacaoContasPermissions.Default));
                  context.Menu.AddItem(new ApplicationMenuItem(LexusMenus.flwFollows, l["Menu:flwFollowses"], "~/flwFollows", icon: "fa fa-folder-open").RequirePermissions(flwFollowsPermissions.Default));
      // <ZenCode-Menu-Marker>

            return Task.CompletedTask;
        }

        private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
        {
            var identityServerUrl = _configuration["AuthServer:Authority"] ?? "~";
            var uiResource = context.GetLocalizer<AbpUiResource>();
            var accountResource = context.GetLocalizer<AccountResource>();
            context.Menu.AddItem(new ApplicationMenuItem("Account.Manage", accountResource["MyAccount"], $"{identityServerUrl.EnsureEndsWith('/')}Account/Manage", icon: "bi bi-gear", order: 1000, null, "_blank").RequireAuthenticated());
            context.Menu.AddItem(new ApplicationMenuItem("Account.SecurityLogs", accountResource["MySecurityLogs"], $"{identityServerUrl.EnsureEndsWith('/')}Account/SecurityLogs", icon: "bi bi-shield", target: "_blank").RequireAuthenticated());
            context.Menu.AddItem(new ApplicationMenuItem("Account.Logout", uiResource["Logout"], url: "~/Account/Logout", icon: "fa fa-power-off", order: int.MaxValue - 1000).RequireAuthenticated());

            return Task.CompletedTask;
        }
    }
}
