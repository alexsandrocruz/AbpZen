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
    // <GEN-ROUTES>
];

export const menuItems: NavItem[] = [
    { label: "Dashboard", href: "/dashboard", icon: LayoutDashboard, section: "main" },
    { label: "My Profile", href: "/profile", icon: User, section: "main" },

    // Host Administration
    { label: "Workspaces", href: "/host/workspaces", icon: Building2, section: "host" },
    { label: "Editions", href: "/host/editions", icon: Crown, section: "host" },

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
    // <GEN-MENU>
];
