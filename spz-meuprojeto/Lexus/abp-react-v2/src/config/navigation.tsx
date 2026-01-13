import {
    LayoutDashboard,
    Building2,
    Users,
    Shield,
    Settings,
    Crown,
    User,
    UserPlus,
    FileText,
    ShieldAlert,
    Users2,
    Network,
    ShieldCheck,
    Box,
} from "lucide-react";
import React from "react";

// Page Imports
import DashboardPage from "@/pages/dashboard";
import TenantDashboardPage from "@/pages/dashboard/tenant-dashboard";
import HostWorkspacesPage from "@/pages/host/workspaces";
import HostUsersPage from "@/pages/host/users";
import HostRolesPage from "@/pages/host/roles";
import HostSettingsPage from "@/pages/host/settings";
import HostEditionsPage from "@/pages/host/editions";
import InvitationsPage from "@/pages/host/invitations";
import AuditLogsPage from "@/pages/host/audit-logs";
import SecurityLogsPage from "@/pages/host/security-logs";
import OrgUnitsPage from "@/pages/host/org-units";
import PermissionGroupsPage from "@/pages/host/permission-groups";
import LoginPage from "@/pages/auth/login";
import RegisterPage from "@/pages/auth/register";
import ForgotPasswordPage from "@/pages/auth/forgot-password";
import ProfilePage from "@/pages/profile";
import UserSessionsPage from "@/pages/sessions";
import LgpdPage from "@/pages/profile/lgpd";
import TermsPage from "@/pages/legal/terms";
import PrivacyPage from "@/pages/legal/privacy";
import LawyerPage from "@/pages/admin/lawyer";
import LawyerFormPage from "@/pages/admin/lawyer-form";
import CasePage from "@/pages/admin/case";
import ClientPage from "@/pages/admin/client";
import SpecializationPage from "@/pages/admin/specialization";
import LegalProcessPage from "@/pages/admin/legal-process";
import LawyerSpecializationPage from "@/pages/admin/lawyer-specialization";
import ProposalPage from "@/pages/admin/proposal";
import ProposalFormPage from "@/pages/admin/proposal/form";
import PropostalItemPage from "@/pages/admin/propostal-item";
import PropostalItemFormPage from "@/pages/admin/propostal-item/form";
import _versaoBDPage from "@/pages/admin/-versao-bd";
import advCliBairrosPage from "@/pages/admin/adv-cli-bairros";
import advCliComoChegouPage from "@/pages/admin/adv-cli-como-chegou";
import advCliLocaisAtendidoPage from "@/pages/admin/adv-cli-locais-atendido";
import advCliLogPage from "@/pages/admin/adv-cli-log";
import advCliPrioridadesPage from "@/pages/admin/adv-cli-prioridades";
import advClientesConvertidosPage from "@/pages/admin/adv-clientes-convertidos";
import advClientesModelosPage from "@/pages/admin/adv-clientes-modelos";
import advClientes_bkpPage from "@/pages/admin/adv-clientes-bkp";
import advPautaObsPage from "@/pages/admin/adv-pauta-obs";
import advPreArquivosStatusPage from "@/pages/admin/adv-pre-arquivos-status";
import advPreLogStatusPage from "@/pages/admin/adv-pre-log-status";
import advPreMetasPage from "@/pages/admin/adv-pre-metas";
import advPreMotivosPerdaPage from "@/pages/admin/adv-pre-motivos-perda";
import advPreOrigensPage from "@/pages/admin/adv-pre-origens";
import advPreProcessosCheckListsPage from "@/pages/admin/adv-pre-processos-check-lists";
import advProEscritoriosPage from "@/pages/admin/adv-pro-escritorios";
import advProFasesPage from "@/pages/admin/adv-pro-fases";
import advProInstanciasPage from "@/pages/admin/adv-pro-instancias";
import advProOrgaosPage from "@/pages/admin/adv-pro-orgaos";
import advProProbabilidadesPage from "@/pages/admin/adv-pro-probabilidades";
import advProRelevanciasPage from "@/pages/admin/adv-pro-relevancias";
import advProSentencasPage from "@/pages/admin/adv-pro-sentencas";
import advProStatusPage from "@/pages/admin/adv-pro-status";
import advProTiposPage from "@/pages/admin/adv-pro-tipos";
import advProVarasPage from "@/pages/admin/adv-pro-varas";
import advTarefasAtualizacoesPage from "@/pages/admin/adv-tarefas-atualizacoes";
import autoFTPPage from "@/pages/admin/auto-ftp";
import fabConfigPage from "@/pages/admin/fab-config";
import fabDatasEFeriadosPage from "@/pages/admin/fab-datas-eferiados";
import fabFormasPagamentoPage from "@/pages/admin/fab-formas-pagamento";
import fabFormasRecebimentoPage from "@/pages/admin/fab-formas-recebimento";
import fabMotivosAproveitamentoPage from "@/pages/admin/fab-motivos-aproveitamento";
import fabMotivosPerdaPage from "@/pages/admin/fab-motivos-perda";
import fabRegioesPage from "@/pages/admin/fab-regioes";
import fdtDevsPage from "@/pages/admin/fdt-devs";
import finCentrosResultadoPage from "@/pages/admin/fin-centros-resultado";
import finContasClientesPage from "@/pages/admin/fin-contas-clientes";
import finLancamentos_BKPPage from "@/pages/admin/fin-lancamentos-bkp";
import finPlanoContasDetPage from "@/pages/admin/fin-plano-contas-det";
import finProcuracoesRPVPage from "@/pages/admin/fin-procuracoes-rpv";
import finRateiosPage from "@/pages/admin/fin-rateios";
import finRateiosPadraoPage from "@/pages/admin/fin-rateios-padrao";
import finRecibosPage from "@/pages/admin/fin-recibos";
import finUnidadesPage from "@/pages/admin/fin-unidades";
import flwConfigPage from "@/pages/admin/flw-config";
import advAgeTiposCompromissosPage from "@/pages/admin/adv-age-tipos-compromissos";
import advAgeTiposTarefasPage from "@/pages/admin/adv-age-tipos-tarefas";
import advCliCargosPage from "@/pages/admin/adv-cli-cargos";
import advCliGruposPage from "@/pages/admin/adv-cli-grupos";
import advCliSituacoesPage from "@/pages/admin/adv-cli-situacoes";
import advCliTiposArquivosPage from "@/pages/admin/adv-cli-tipos-arquivos";
import advCliTiposHistoricosPage from "@/pages/admin/adv-cli-tipos-historicos";
import advClientesINSSStatusPage from "@/pages/admin/adv-clientes-inssstatus";
import advFornecedoresPage from "@/pages/admin/adv-fornecedores";
import advPostosINSSPage from "@/pages/admin/adv-postos-inss";
import advPreCheckListsGruposPage from "@/pages/admin/adv-pre-check-lists-grupos";
import advPreStatusTiposPage from "@/pages/admin/adv-pre-status-tipos";
import advProMeritosPage from "@/pages/admin/adv-pro-meritos";
import advProNaturezasPage from "@/pages/admin/adv-pro-naturezas";
import advVerTiposPage from "@/pages/admin/adv-ver-tipos";
import fabCondicoesPagamentoPage from "@/pages/admin/fab-condicoes-pagamento";
import fabHistoricoTiposPage from "@/pages/admin/fab-historico-tipos";
import fabPaisesPage from "@/pages/admin/fab-paises";
import fabPermissoesTiposPage from "@/pages/admin/fab-permissoes-tipos";
import finAreasPage from "@/pages/admin/fin-areas";
import finCentrosCustoPage from "@/pages/admin/fin-centros-custo";
import finContasPage from "@/pages/admin/fin-contas";
import finGruposDREPage from "@/pages/admin/fin-grupos-dre";
import flwAcoesPage from "@/pages/admin/flw-acoes";
import logAcoesPage from "@/pages/admin/log-acoes";
import opoSituacoesPage from "@/pages/admin/opo-situacoes";
import opoTiposPage from "@/pages/admin/opo-tipos";
import usuAreasPage from "@/pages/admin/usu-areas";
import advClientesPage from "@/pages/admin/adv-clientes";
import advClientesArquivosPage from "@/pages/admin/adv-clientes-arquivos";
import advClientesAtualizacoesPage from "@/pages/admin/adv-clientes-atualizacoes";
import advClientesChecklistPage from "@/pages/admin/adv-clientes-checklist";
import advClientesINSSPage from "@/pages/admin/adv-clientes-inss";
import advPreCheckListsPage from "@/pages/admin/adv-pre-check-lists";
import advPreStatusPage from "@/pages/admin/adv-pre-status";
import advProcessosPage from "@/pages/admin/adv-processos";
import advProcessosClientesPage from "@/pages/admin/adv-processos-clientes";
import advProcessosHonorariosPage from "@/pages/admin/adv-processos-honorarios";
import advProcessosMeritosPage from "@/pages/admin/adv-processos-meritos";
import fabEstadosPage from "@/pages/admin/fab-estados";
import fabPermissoesPage from "@/pages/admin/fab-permissoes";
import finExtratoPage from "@/pages/admin/fin-extrato";
import finPlanoContasGruposPage from "@/pages/admin/fin-plano-contas-grupos";
import flwConfigExcecoesPage from "@/pages/admin/flw-config-excecoes";
import flwGradeHorariosPage from "@/pages/admin/flw-grade-horarios";
import logCamposPage from "@/pages/admin/log-campos";
import usuCargosPage from "@/pages/admin/usu-cargos";
import advClientesHistoricosPage from "@/pages/admin/adv-clientes-historicos";
import advCompromissosPage from "@/pages/admin/adv-compromissos";
import advProcessosAlteracoesPage from "@/pages/admin/adv-processos-alteracoes";
import advProcessosDadosHerdeirosPage from "@/pages/admin/adv-processos-dados-herdeiros";
import advProfissionaisPage from "@/pages/admin/adv-profissionais";
import advProfissionaisEstadosPage from "@/pages/admin/adv-profissionais-estados";
import advProfissionaisNaturezasPage from "@/pages/admin/adv-profissionais-naturezas";
import advRevisaoDocumentosPage from "@/pages/admin/adv-revisao-documentos";
import advTarefasPage from "@/pages/admin/adv-tarefas";
import advVerbasPage from "@/pages/admin/adv-verbas";
import fabCidadesPage from "@/pages/admin/fab-cidades";
import fabLembretesPage from "@/pages/admin/fab-lembretes";
import finPlanoContasPage from "@/pages/admin/fin-plano-contas";
import opoOportunidadesPage from "@/pages/admin/opo-oportunidades";
import opoOrcamentosPage from "@/pages/admin/opo-orcamentos";
import usuAcessosPage from "@/pages/admin/usu-acessos";
import usuDistanciasPage from "@/pages/admin/usu-distancias";
import finLancamentosPage from "@/pages/admin/fin-lancamentos";
import finPrestacaoContasPage from "@/pages/admin/fin-prestacao-contas";
import flwFollowsPage from "@/pages/admin/flw-follows";
// <GEN-IMPORTS>

export interface NavItem {
    label: string;
    href?: string;
    icon: any; // LucideIcon
    section?: "main" | "host" | "admin" | "entities";
    permission?: string;
    items?: NavItem[];
}

export interface RouteConfig {
    path: string;
    component: React.ComponentType<any>;
    permission?: string;
}

export const routes: RouteConfig[] = [
    { path: "/dashboard", component: DashboardPage },
    { path: "/tenant-dashboard", component: TenantDashboardPage },
    { path: "/auth/login", component: LoginPage },
    { path: "/auth/register", component: RegisterPage },
    { path: "/auth/forgot-password", component: ForgotPasswordPage },
    { path: "/profile", component: ProfilePage },
    { path: "/host/workspaces", component: HostWorkspacesPage },
    { path: "/host/tenants", component: HostWorkspacesPage },
    { path: "/host/tenant", component: HostWorkspacesPage },
    { path: "/host/users", component: HostUsersPage },
    { path: "/host/roles", component: HostRolesPage },
    { path: "/host/settings", component: HostSettingsPage },
    { path: "/host/editions", component: HostEditionsPage },
    { path: "/host/invitations", component: InvitationsPage },
    { path: "/host/audit-logs", component: AuditLogsPage },
    { path: "/host/security-logs", component: SecurityLogsPage },
    { path: "/host/org-units", component: OrgUnitsPage },
    { path: "/host/permission-groups", component: PermissionGroupsPage },
    { path: "/sessions", component: UserSessionsPage },
    { path: "/profile/lgpd", component: LgpdPage },
    { path: "/legal/terms", component: TermsPage },
    { path: "/legal/privacy", component: PrivacyPage },
    { path: "/admin/lawyer", component: LawyerPage },
    { path: "/admin/lawyer/create", component: LawyerFormPage },
    { path: "/admin/lawyer/:id/edit", component: LawyerFormPage },
    { path: "/admin/case", component: CasePage },
    { path: "/admin/client", component: ClientPage },
    { path: "/admin/specialization", component: SpecializationPage },
    { path: "/admin/legal-process", component: LegalProcessPage },
    { path: "/admin/lawyer-specialization", component: LawyerSpecializationPage },
    { path: "/admin/proposal/create", component: ProposalFormPage },
    { path: "/admin/proposal/:id/edit", component: ProposalFormPage },
    { path: "/admin/proposal", component: ProposalPage },
    { path: "/admin/propostal-item", component: PropostalItemPage },
    { path: "/admin/propostal-item/create", component: PropostalItemFormPage },
    { path: "/admin/propostal-item/:id/edit", component: PropostalItemFormPage },
    { path: "/admin/-versao-bd", component: _versaoBDPage },
    { path: "/admin/adv-cli-bairros", component: advCliBairrosPage },
    { path: "/admin/adv-cli-como-chegou", component: advCliComoChegouPage },
    { path: "/admin/adv-cli-locais-atendido", component: advCliLocaisAtendidoPage },
    { path: "/admin/adv-cli-log", component: advCliLogPage },
    { path: "/admin/adv-cli-prioridades", component: advCliPrioridadesPage },
    { path: "/admin/adv-clientes-convertidos", component: advClientesConvertidosPage },
    { path: "/admin/adv-clientes-modelos", component: advClientesModelosPage },
    { path: "/admin/adv-clientes-bkp", component: advClientes_bkpPage },
    { path: "/admin/adv-pauta-obs", component: advPautaObsPage },
    { path: "/admin/adv-pre-arquivos-status", component: advPreArquivosStatusPage },
    { path: "/admin/adv-pre-log-status", component: advPreLogStatusPage },
    { path: "/admin/adv-pre-metas", component: advPreMetasPage },
    { path: "/admin/adv-pre-motivos-perda", component: advPreMotivosPerdaPage },
    { path: "/admin/adv-pre-origens", component: advPreOrigensPage },
    { path: "/admin/adv-pre-processos-check-lists", component: advPreProcessosCheckListsPage },
    { path: "/admin/adv-pro-escritorios", component: advProEscritoriosPage },
    { path: "/admin/adv-pro-fases", component: advProFasesPage },
    { path: "/admin/adv-pro-instancias", component: advProInstanciasPage },
    { path: "/admin/adv-pro-orgaos", component: advProOrgaosPage },
    { path: "/admin/adv-pro-probabilidades", component: advProProbabilidadesPage },
    { path: "/admin/adv-pro-relevancias", component: advProRelevanciasPage },
    { path: "/admin/adv-pro-sentencas", component: advProSentencasPage },
    { path: "/admin/adv-pro-status", component: advProStatusPage },
    { path: "/admin/adv-pro-tipos", component: advProTiposPage },
    { path: "/admin/adv-pro-varas", component: advProVarasPage },
    { path: "/admin/adv-tarefas-atualizacoes", component: advTarefasAtualizacoesPage },
    { path: "/admin/auto-ftp", component: autoFTPPage },
    { path: "/admin/fab-config", component: fabConfigPage },
    { path: "/admin/fab-datas-eferiados", component: fabDatasEFeriadosPage },
    { path: "/admin/fab-formas-pagamento", component: fabFormasPagamentoPage },
    { path: "/admin/fab-formas-recebimento", component: fabFormasRecebimentoPage },
    { path: "/admin/fab-motivos-aproveitamento", component: fabMotivosAproveitamentoPage },
    { path: "/admin/fab-motivos-perda", component: fabMotivosPerdaPage },
    { path: "/admin/fab-regioes", component: fabRegioesPage },
    { path: "/admin/fdt-devs", component: fdtDevsPage },
    { path: "/admin/fin-centros-resultado", component: finCentrosResultadoPage },
    { path: "/admin/fin-contas-clientes", component: finContasClientesPage },
    { path: "/admin/fin-lancamentos-bkp", component: finLancamentos_BKPPage },
    { path: "/admin/fin-plano-contas-det", component: finPlanoContasDetPage },
    { path: "/admin/fin-procuracoes-rpv", component: finProcuracoesRPVPage },
    { path: "/admin/fin-rateios", component: finRateiosPage },
    { path: "/admin/fin-rateios-padrao", component: finRateiosPadraoPage },
    { path: "/admin/fin-recibos", component: finRecibosPage },
    { path: "/admin/fin-unidades", component: finUnidadesPage },
    { path: "/admin/flw-config", component: flwConfigPage },
    { path: "/admin/adv-age-tipos-compromissos", component: advAgeTiposCompromissosPage },
    { path: "/admin/adv-age-tipos-tarefas", component: advAgeTiposTarefasPage },
    { path: "/admin/adv-cli-cargos", component: advCliCargosPage },
    { path: "/admin/adv-cli-grupos", component: advCliGruposPage },
    { path: "/admin/adv-cli-situacoes", component: advCliSituacoesPage },
    { path: "/admin/adv-cli-tipos-arquivos", component: advCliTiposArquivosPage },
    { path: "/admin/adv-cli-tipos-historicos", component: advCliTiposHistoricosPage },
    { path: "/admin/adv-clientes-inssstatus", component: advClientesINSSStatusPage },
    { path: "/admin/adv-fornecedores", component: advFornecedoresPage },
    { path: "/admin/adv-postos-inss", component: advPostosINSSPage },
    { path: "/admin/adv-pre-check-lists-grupos", component: advPreCheckListsGruposPage },
    { path: "/admin/adv-pre-status-tipos", component: advPreStatusTiposPage },
    { path: "/admin/adv-pro-meritos", component: advProMeritosPage },
    { path: "/admin/adv-pro-naturezas", component: advProNaturezasPage },
    { path: "/admin/adv-ver-tipos", component: advVerTiposPage },
    { path: "/admin/fab-condicoes-pagamento", component: fabCondicoesPagamentoPage },
    { path: "/admin/fab-historico-tipos", component: fabHistoricoTiposPage },
    { path: "/admin/fab-paises", component: fabPaisesPage },
    { path: "/admin/fab-permissoes-tipos", component: fabPermissoesTiposPage },
    { path: "/admin/fin-areas", component: finAreasPage },
    { path: "/admin/fin-centros-custo", component: finCentrosCustoPage },
    { path: "/admin/fin-contas", component: finContasPage },
    { path: "/admin/fin-grupos-dre", component: finGruposDREPage },
    { path: "/admin/flw-acoes", component: flwAcoesPage },
    { path: "/admin/log-acoes", component: logAcoesPage },
    { path: "/admin/opo-situacoes", component: opoSituacoesPage },
    { path: "/admin/opo-tipos", component: opoTiposPage },
    { path: "/admin/usu-areas", component: usuAreasPage },
    { path: "/admin/adv-clientes", component: advClientesPage },
    { path: "/admin/adv-clientes-arquivos", component: advClientesArquivosPage },
    { path: "/admin/adv-clientes-atualizacoes", component: advClientesAtualizacoesPage },
    { path: "/admin/adv-clientes-checklist", component: advClientesChecklistPage },
    { path: "/admin/adv-clientes-inss", component: advClientesINSSPage },
    { path: "/admin/adv-pre-check-lists", component: advPreCheckListsPage },
    { path: "/admin/adv-pre-status", component: advPreStatusPage },
    { path: "/admin/adv-processos", component: advProcessosPage },
    { path: "/admin/adv-processos-clientes", component: advProcessosClientesPage },
    { path: "/admin/adv-processos-honorarios", component: advProcessosHonorariosPage },
    { path: "/admin/adv-processos-meritos", component: advProcessosMeritosPage },
    { path: "/admin/fab-estados", component: fabEstadosPage },
    { path: "/admin/fab-permissoes", component: fabPermissoesPage },
    { path: "/admin/fin-extrato", component: finExtratoPage },
    { path: "/admin/fin-plano-contas-grupos", component: finPlanoContasGruposPage },
    { path: "/admin/flw-config-excecoes", component: flwConfigExcecoesPage },
    { path: "/admin/flw-grade-horarios", component: flwGradeHorariosPage },
    { path: "/admin/log-campos", component: logCamposPage },
    { path: "/admin/usu-cargos", component: usuCargosPage },
    { path: "/admin/adv-clientes-historicos", component: advClientesHistoricosPage },
    { path: "/admin/adv-compromissos", component: advCompromissosPage },
    { path: "/admin/adv-processos-alteracoes", component: advProcessosAlteracoesPage },
    { path: "/admin/adv-processos-dados-herdeiros", component: advProcessosDadosHerdeirosPage },
    { path: "/admin/adv-profissionais", component: advProfissionaisPage },
    { path: "/admin/adv-profissionais-estados", component: advProfissionaisEstadosPage },
    { path: "/admin/adv-profissionais-naturezas", component: advProfissionaisNaturezasPage },
    { path: "/admin/adv-revisao-documentos", component: advRevisaoDocumentosPage },
    { path: "/admin/adv-tarefas", component: advTarefasPage },
    { path: "/admin/adv-verbas", component: advVerbasPage },
    { path: "/admin/fab-cidades", component: fabCidadesPage },
    { path: "/admin/fab-lembretes", component: fabLembretesPage },
    { path: "/admin/fin-plano-contas", component: finPlanoContasPage },
    { path: "/admin/opo-oportunidades", component: opoOportunidadesPage },
    { path: "/admin/opo-orcamentos", component: opoOrcamentosPage },
    { path: "/admin/usu-acessos", component: usuAcessosPage },
    { path: "/admin/usu-distancias", component: usuDistanciasPage },
    { path: "/admin/fin-lancamentos", component: finLancamentosPage },
    { path: "/admin/fin-prestacao-contas", component: finPrestacaoContasPage },
    { path: "/admin/flw-follows", component: flwFollowsPage },
    // <GEN-ROUTES>
];

export const menuItems: NavItem[] = [
    { label: "Dashboard", href: "/dashboard", icon: LayoutDashboard, section: "main" },
    { label: "My Profile", href: "/profile", icon: User, section: "main" },

    // Host Administration
    { label: "Workspaces", href: "/host/workspaces", icon: Building2, section: "host" },
    { label: "Editions", href: "/host/editions", icon: Crown, section: "host" },

    // Entities
    { label: "Lawyers", href: "/admin/lawyer", icon: LayoutDashboard, section: "entities" },
    { label: "Cases", href: "/admin/case", icon: LayoutDashboard, section: "entities" },

    // Administration (Tenant / Shared)
    {
        label: "Identity Management",
        icon: Users2,
        section: "admin",
        items: [
            { label: "Organization Units", href: "/host/org-units", icon: Network },
            { label: "Permission Groups", href: "/host/permission-groups", icon: ShieldCheck },
            { label: "Roles", href: "/host/roles", icon: Shield },
            { label: "Users", href: "/host/users", icon: Users },
            { label: "Security Logs", href: "/host/security-logs", icon: ShieldAlert },
        ]
    },
    { label: "Settings", href: "/host/settings", icon: Settings, section: "admin" },
    { label: "Audit Logs", href: "/host/audit-logs", icon: FileText, section: "admin" },
    { label: "Invitations", href: "/host/invitations", icon: UserPlus, section: "admin" },
    { label: "Clients", href: "/admin/client", icon: LayoutDashboard, section: "entities" },
    { label: "Specializations", href: "/admin/specialization", icon: LayoutDashboard, section: "entities" },
    { label: "LegalProcesses", href: "/admin/legal-process", icon: LayoutDashboard, section: "entities" },
    { label: "LawyerSpecializations", href: "/admin/lawyer-specialization", icon: LayoutDashboard, section: "entities" },
    { label: "Proposals", href: "/admin/proposal", icon: LayoutDashboard, section: "entities" },
    { label: "PropostalItems", href: "/admin/propostal-item", icon: LayoutDashboard, section: "entities" },

    { label: "_versaoBDs", href: "/admin/-versao-bd", icon: LayoutDashboard, section: "entities" },
    { label: "advCliBairroses", href: "/admin/adv-cli-bairros", icon: LayoutDashboard, section: "entities" },
    { label: "advCliComoChegous", href: "/admin/adv-cli-como-chegou", icon: LayoutDashboard, section: "entities" },
    { label: "advCliLocaisAtendidos", href: "/admin/adv-cli-locais-atendido", icon: LayoutDashboard, section: "entities" },
    { label: "advCliLogs", href: "/admin/adv-cli-log", icon: LayoutDashboard, section: "entities" },
    { label: "advCliPrioridadeses", href: "/admin/adv-cli-prioridades", icon: LayoutDashboard, section: "entities" },
    { label: "advClientesConvertidoses", href: "/admin/adv-clientes-convertidos", icon: LayoutDashboard, section: "entities" },
    { label: "advClientesModeloses", href: "/admin/adv-clientes-modelos", icon: LayoutDashboard, section: "entities" },
    { label: "advClientes_bkps", href: "/admin/adv-clientes-bkp", icon: LayoutDashboard, section: "entities" },
    { label: "advPautaObses", href: "/admin/adv-pauta-obs", icon: LayoutDashboard, section: "entities" },
    { label: "advPreArquivosStatuses", href: "/admin/adv-pre-arquivos-status", icon: LayoutDashboard, section: "entities" },
    { label: "advPreLogStatuses", href: "/admin/adv-pre-log-status", icon: LayoutDashboard, section: "entities" },
    { label: "advPreMetases", href: "/admin/adv-pre-metas", icon: LayoutDashboard, section: "entities" },
    { label: "advPreMotivosPerdas", href: "/admin/adv-pre-motivos-perda", icon: LayoutDashboard, section: "entities" },
    { label: "advPreOrigenses", href: "/admin/adv-pre-origens", icon: LayoutDashboard, section: "entities" },
    { label: "advPreProcessosCheckListses", href: "/admin/adv-pre-processos-check-lists", icon: LayoutDashboard, section: "entities" },
    { label: "advProEscritorioses", href: "/admin/adv-pro-escritorios", icon: LayoutDashboard, section: "entities" },
    { label: "advProFaseses", href: "/admin/adv-pro-fases", icon: LayoutDashboard, section: "entities" },
    { label: "advProInstanciases", href: "/admin/adv-pro-instancias", icon: LayoutDashboard, section: "entities" },
    { label: "advProOrgaoses", href: "/admin/adv-pro-orgaos", icon: LayoutDashboard, section: "entities" },
    { label: "advProProbabilidadeses", href: "/admin/adv-pro-probabilidades", icon: LayoutDashboard, section: "entities" },
    { label: "advProRelevanciases", href: "/admin/adv-pro-relevancias", icon: LayoutDashboard, section: "entities" },
    { label: "advProSentencases", href: "/admin/adv-pro-sentencas", icon: LayoutDashboard, section: "entities" },
    { label: "advProStatuses", href: "/admin/adv-pro-status", icon: LayoutDashboard, section: "entities" },
    { label: "advProTiposes", href: "/admin/adv-pro-tipos", icon: LayoutDashboard, section: "entities" },
    { label: "advProVarases", href: "/admin/adv-pro-varas", icon: LayoutDashboard, section: "entities" },
    { label: "advTarefasAtualizacoeses", href: "/admin/adv-tarefas-atualizacoes", icon: LayoutDashboard, section: "entities" },
    { label: "autoFTPs", href: "/admin/auto-ftp", icon: LayoutDashboard, section: "entities" },
    { label: "fabConfigs", href: "/admin/fab-config", icon: LayoutDashboard, section: "entities" },
    { label: "fabDatasEFeriadoses", href: "/admin/fab-datas-eferiados", icon: LayoutDashboard, section: "entities" },
    { label: "fabFormasPagamentos", href: "/admin/fab-formas-pagamento", icon: LayoutDashboard, section: "entities" },
    { label: "fabFormasRecebimentos", href: "/admin/fab-formas-recebimento", icon: LayoutDashboard, section: "entities" },
    { label: "fabMotivosAproveitamentos", href: "/admin/fab-motivos-aproveitamento", icon: LayoutDashboard, section: "entities" },
    { label: "fabMotivosPerdas", href: "/admin/fab-motivos-perda", icon: LayoutDashboard, section: "entities" },
    { label: "fabRegioeses", href: "/admin/fab-regioes", icon: LayoutDashboard, section: "entities" },
    { label: "fdtDevses", href: "/admin/fdt-devs", icon: LayoutDashboard, section: "entities" },
    { label: "finCentrosResultados", href: "/admin/fin-centros-resultado", icon: LayoutDashboard, section: "entities" },
    { label: "finContasClienteses", href: "/admin/fin-contas-clientes", icon: LayoutDashboard, section: "entities" },
    { label: "finLancamentos_BKPs", href: "/admin/fin-lancamentos-bkp", icon: LayoutDashboard, section: "entities" },
    { label: "finPlanoContasDets", href: "/admin/fin-plano-contas-det", icon: LayoutDashboard, section: "entities" },
    { label: "finProcuracoesRPVs", href: "/admin/fin-procuracoes-rpv", icon: LayoutDashboard, section: "entities" },
    { label: "finRateioses", href: "/admin/fin-rateios", icon: LayoutDashboard, section: "entities" },
    { label: "finRateiosPadraos", href: "/admin/fin-rateios-padrao", icon: LayoutDashboard, section: "entities" },
    { label: "finReciboses", href: "/admin/fin-recibos", icon: LayoutDashboard, section: "entities" },
    { label: "finUnidadeses", href: "/admin/fin-unidades", icon: LayoutDashboard, section: "entities" },
    { label: "flwConfigs", href: "/admin/flw-config", icon: LayoutDashboard, section: "entities" },
    { label: "advAgeTiposCompromissoses", href: "/admin/adv-age-tipos-compromissos", icon: LayoutDashboard, section: "entities" },
    { label: "advAgeTiposTarefases", href: "/admin/adv-age-tipos-tarefas", icon: LayoutDashboard, section: "entities" },
    { label: "advCliCargoses", href: "/admin/adv-cli-cargos", icon: LayoutDashboard, section: "entities" },
    { label: "advCliGruposes", href: "/admin/adv-cli-grupos", icon: LayoutDashboard, section: "entities" },
    { label: "advCliSituacoeses", href: "/admin/adv-cli-situacoes", icon: LayoutDashboard, section: "entities" },
    { label: "advCliTiposArquivoses", href: "/admin/adv-cli-tipos-arquivos", icon: LayoutDashboard, section: "entities" },
    { label: "advCliTiposHistoricoses", href: "/admin/adv-cli-tipos-historicos", icon: LayoutDashboard, section: "entities" },
    { label: "advClientesINSSStatuses", href: "/admin/adv-clientes-inssstatus", icon: LayoutDashboard, section: "entities" },
    { label: "advFornecedoreses", href: "/admin/adv-fornecedores", icon: LayoutDashboard, section: "entities" },
    { label: "advPostosINSSes", href: "/admin/adv-postos-inss", icon: LayoutDashboard, section: "entities" },
    { label: "advPreCheckListsGruposes", href: "/admin/adv-pre-check-lists-grupos", icon: LayoutDashboard, section: "entities" },
    { label: "advPreStatusTiposes", href: "/admin/adv-pre-status-tipos", icon: LayoutDashboard, section: "entities" },
    { label: "advProMeritoses", href: "/admin/adv-pro-meritos", icon: LayoutDashboard, section: "entities" },
    { label: "advProNaturezases", href: "/admin/adv-pro-naturezas", icon: LayoutDashboard, section: "entities" },
    { label: "advVerTiposes", href: "/admin/adv-ver-tipos", icon: LayoutDashboard, section: "entities" },
    { label: "fabCondicoesPagamentos", href: "/admin/fab-condicoes-pagamento", icon: LayoutDashboard, section: "entities" },
    { label: "fabHistoricoTiposes", href: "/admin/fab-historico-tipos", icon: LayoutDashboard, section: "entities" },
    { label: "fabPaiseses", href: "/admin/fab-paises", icon: LayoutDashboard, section: "entities" },
    { label: "fabPermissoesTiposes", href: "/admin/fab-permissoes-tipos", icon: LayoutDashboard, section: "entities" },
    { label: "finAreases", href: "/admin/fin-areas", icon: LayoutDashboard, section: "entities" },
    { label: "finCentrosCustos", href: "/admin/fin-centros-custo", icon: LayoutDashboard, section: "entities" },
    { label: "finContases", href: "/admin/fin-contas", icon: LayoutDashboard, section: "entities" },
    { label: "finGruposDREs", href: "/admin/fin-grupos-dre", icon: LayoutDashboard, section: "entities" },
    { label: "flwAcoeses", href: "/admin/flw-acoes", icon: LayoutDashboard, section: "entities" },
    { label: "logAcoeses", href: "/admin/log-acoes", icon: LayoutDashboard, section: "entities" },
    { label: "opoSituacoeses", href: "/admin/opo-situacoes", icon: LayoutDashboard, section: "entities" },
    { label: "opoTiposes", href: "/admin/opo-tipos", icon: LayoutDashboard, section: "entities" },
    { label: "usuAreases", href: "/admin/usu-areas", icon: LayoutDashboard, section: "entities" },
    { label: "advClienteses", href: "/admin/adv-clientes", icon: LayoutDashboard, section: "entities" },
    { label: "advClientesArquivoses", href: "/admin/adv-clientes-arquivos", icon: LayoutDashboard, section: "entities" },
    { label: "advClientesAtualizacoeses", href: "/admin/adv-clientes-atualizacoes", icon: LayoutDashboard, section: "entities" },
    { label: "advClientesChecklists", href: "/admin/adv-clientes-checklist", icon: LayoutDashboard, section: "entities" },
    { label: "advClientesINSSes", href: "/admin/adv-clientes-inss", icon: LayoutDashboard, section: "entities" },
    { label: "advPreCheckListses", href: "/admin/adv-pre-check-lists", icon: LayoutDashboard, section: "entities" },
    { label: "advPreStatuses", href: "/admin/adv-pre-status", icon: LayoutDashboard, section: "entities" },
    { label: "advProcessoses", href: "/admin/adv-processos", icon: LayoutDashboard, section: "entities" },
    { label: "advProcessosClienteses", href: "/admin/adv-processos-clientes", icon: LayoutDashboard, section: "entities" },
    { label: "advProcessosHonorarioses", href: "/admin/adv-processos-honorarios", icon: LayoutDashboard, section: "entities" },
    { label: "advProcessosMeritoses", href: "/admin/adv-processos-meritos", icon: LayoutDashboard, section: "entities" },
    { label: "fabEstadoses", href: "/admin/fab-estados", icon: LayoutDashboard, section: "entities" },
    { label: "fabPermissoeses", href: "/admin/fab-permissoes", icon: LayoutDashboard, section: "entities" },
    { label: "finExtratos", href: "/admin/fin-extrato", icon: LayoutDashboard, section: "entities" },
    { label: "finPlanoContasGruposes", href: "/admin/fin-plano-contas-grupos", icon: LayoutDashboard, section: "entities" },
    { label: "flwConfigExcecoeses", href: "/admin/flw-config-excecoes", icon: LayoutDashboard, section: "entities" },
    { label: "flwGradeHorarioses", href: "/admin/flw-grade-horarios", icon: LayoutDashboard, section: "entities" },
    { label: "logCamposes", href: "/admin/log-campos", icon: LayoutDashboard, section: "entities" },
    { label: "usuCargoses", href: "/admin/usu-cargos", icon: LayoutDashboard, section: "entities" },
    { label: "advClientesHistoricoses", href: "/admin/adv-clientes-historicos", icon: LayoutDashboard, section: "entities" },
    { label: "advCompromissoses", href: "/admin/adv-compromissos", icon: LayoutDashboard, section: "entities" },
    { label: "advProcessosAlteracoeses", href: "/admin/adv-processos-alteracoes", icon: LayoutDashboard, section: "entities" },
    { label: "advProcessosDadosHerdeiroses", href: "/admin/adv-processos-dados-herdeiros", icon: LayoutDashboard, section: "entities" },
    { label: "advProfissionaises", href: "/admin/adv-profissionais", icon: LayoutDashboard, section: "entities" },
    { label: "advProfissionaisEstadoses", href: "/admin/adv-profissionais-estados", icon: LayoutDashboard, section: "entities" },
    { label: "advProfissionaisNaturezases", href: "/admin/adv-profissionais-naturezas", icon: LayoutDashboard, section: "entities" },
    { label: "advRevisaoDocumentoses", href: "/admin/adv-revisao-documentos", icon: LayoutDashboard, section: "entities" },
    { label: "advTarefases", href: "/admin/adv-tarefas", icon: LayoutDashboard, section: "entities" },
    { label: "advVerbases", href: "/admin/adv-verbas", icon: LayoutDashboard, section: "entities" },
    { label: "fabCidadeses", href: "/admin/fab-cidades", icon: LayoutDashboard, section: "entities" },
    { label: "fabLembreteses", href: "/admin/fab-lembretes", icon: LayoutDashboard, section: "entities" },
    { label: "finPlanoContases", href: "/admin/fin-plano-contas", icon: LayoutDashboard, section: "entities" },
    { label: "opoOportunidadeses", href: "/admin/opo-oportunidades", icon: LayoutDashboard, section: "entities" },
    { label: "opoOrcamentoses", href: "/admin/opo-orcamentos", icon: LayoutDashboard, section: "entities" },
    { label: "usuAcessoses", href: "/admin/usu-acessos", icon: LayoutDashboard, section: "entities" },
    { label: "usuDistanciases", href: "/admin/usu-distancias", icon: LayoutDashboard, section: "entities" },
    { label: "finLancamentoses", href: "/admin/fin-lancamentos", icon: LayoutDashboard, section: "entities" },
    { label: "finPrestacaoContases", href: "/admin/fin-prestacao-contas", icon: LayoutDashboard, section: "entities" },
    { label: "flwFollowses", href: "/admin/flw-follows", icon: LayoutDashboard, section: "entities" },
    // <GEN-MENU>
];
