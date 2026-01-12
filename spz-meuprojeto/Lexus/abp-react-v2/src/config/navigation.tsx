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
    { path: "/admin/lawyer/edit/:id", component: LawyerFormPage },
    { path: "/admin/case", component: CasePage },
    { path: "/admin/client", component: ClientPage },
    { path: "/admin/specialization", component: SpecializationPage },
    { path: "/admin/legal-process", component: LegalProcessPage },
    { path: "/admin/lawyer-specialization", component: LawyerSpecializationPage },
    { path: "/admin/proposal/new", component: ProposalFormPage },
    { path: "/admin/proposal/:id/edit", component: ProposalFormPage },
    { path: "/admin/proposal", component: ProposalPage },
    { path: "/admin/propostal-item", component: PropostalItemPage },
    { path: "/admin/propostal-item/new", component: PropostalItemFormPage },
    { path: "/admin/propostal-item/:id/edit", component: PropostalItemFormPage },
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

    // <GEN-MENU>
];
