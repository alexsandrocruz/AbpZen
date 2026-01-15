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
import ClientPage from "@/pages/admin/client";
      import ClientContactPage from "@/pages/admin/client-contact";
      import ClientMessagePage from "@/pages/admin/client-message";
      import ProjectPage from "@/pages/admin/project";
      import TaskPage from "@/pages/admin/task";
      import TaskCommentPage from "@/pages/admin/task-comment";
      import TimeEntryPage from "@/pages/admin/time-entry";
      import FinancialCategoryPage from "@/pages/admin/financial-category";
      import TransactionPage from "@/pages/admin/transaction";
      import TransactionAttachmentPage from "@/pages/admin/transaction-attachment";
      import BudgetPage from "@/pages/admin/budget";
      import ProductPage from "@/pages/admin/product";
      import ProposalPage from "@/pages/admin/proposal";
      import ProposalItemPage from "@/pages/admin/proposal-item";
      import ContractPage from "@/pages/admin/contract";
      import LeadWorkflowPage from "@/pages/admin/lead-workflow";
      import LeadWorkflowStagePage from "@/pages/admin/lead-workflow-stage";
      import LeadPage from "@/pages/admin/lead";
      import LeadTagPage from "@/pages/admin/lead-tag";
      import LeadFormPage from "@/pages/admin/lead-form";
      import LeadLandingPagePage from "@/pages/admin/lead-landing-page";
      import CustomFieldPage from "@/pages/admin/custom-field";
      import CustomFieldValuePage from "@/pages/admin/custom-field-value";
      import SchedulerTypePage from "@/pages/admin/scheduler-type";
      import BookingPage from "@/pages/admin/booking";
      import SitePage from "@/pages/admin/site";
      import SitePagePage from "@/pages/admin/site-page";
      import BlogPostPage from "@/pages/admin/blog-post";
      import ConversationPage from "@/pages/admin/conversation";
      import ChatMessagePage from "@/pages/admin/chat-message";
      import WorkflowPage from "@/pages/admin/workflow";
      import AiChatSessionPage from "@/pages/admin/ai-chat-session";
      import AiChatMessagePage from "@/pages/admin/ai-chat-message";
      import WorkspaceInvitePage from "@/pages/admin/workspace-invite";
      import WorkspaceUsageMetricPage from "@/pages/admin/workspace-usage-metric";
      import EmailLogPage from "@/pages/admin/email-log";
      import SmsLogPage from "@/pages/admin/sms-log";
      import WhatsappLogPage from "@/pages/admin/whatsapp-log";
      import LeadFormFieldPage from "@/pages/admin/lead-form-field";
      import LeadFormSubmissionPage from "@/pages/admin/lead-form-submission";
      import LeadStageHistoryPage from "@/pages/admin/lead-stage-history";
      import LeadMessageTemplatePage from "@/pages/admin/lead-message-template";
      import LeadAutomationPage from "@/pages/admin/lead-automation";
      import LeadScheduledMessagePage from "@/pages/admin/lead-scheduled-message";
      import SchedulerAvailabilityPage from "@/pages/admin/scheduler-availability";
      import SchedulerExceptionPage from "@/pages/admin/scheduler-exception";
      import SitePageVersionPage from "@/pages/admin/site-page-version";
      import BlogCategoryPage from "@/pages/admin/blog-category";
      import BlogPostVersionPage from "@/pages/admin/blog-post-version";
      import SiteVisitEventPage from "@/pages/admin/site-visit-event";
      import SiteVisitDailyStatPage from "@/pages/admin/site-visit-daily-stat";
      import MessageAttachmentPage from "@/pages/admin/message-attachment";
      import CommentPage from "@/pages/admin/comment";
      import FilePage from "@/pages/admin/file";
      import WorkflowExecutionPage from "@/pages/admin/workflow-execution";
      import WorkspaceAccessEventPage from "@/pages/admin/workspace-access-event";
      import LandingLeadPage from "@/pages/admin/landing-lead";
      import ProposalBlockInstancePage from "@/pages/admin/proposal-block-instance";
      import ProposalTemplateBlockPage from "@/pages/admin/proposal-template-block";
      import ProjectResponsiblePage from "@/pages/admin/project-responsible";
      import ProjectFollowerPage from "@/pages/admin/project-follower";
      import ProjectCommunicationPage from "@/pages/admin/project-communication";
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
        { path: "/admin/client", component: ClientPage },
          { path: "/admin/client-contact", component: ClientContactPage },
          { path: "/admin/client-message", component: ClientMessagePage },
          { path: "/admin/project", component: ProjectPage },
          { path: "/admin/task", component: TaskPage },
          { path: "/admin/task-comment", component: TaskCommentPage },
          { path: "/admin/time-entry", component: TimeEntryPage },
          { path: "/admin/financial-category", component: FinancialCategoryPage },
          { path: "/admin/transaction", component: TransactionPage },
          { path: "/admin/transaction-attachment", component: TransactionAttachmentPage },
          { path: "/admin/budget", component: BudgetPage },
          { path: "/admin/product", component: ProductPage },
          { path: "/admin/proposal", component: ProposalPage },
          { path: "/admin/proposal-item", component: ProposalItemPage },
          { path: "/admin/contract", component: ContractPage },
          { path: "/admin/lead-workflow", component: LeadWorkflowPage },
          { path: "/admin/lead-workflow-stage", component: LeadWorkflowStagePage },
          { path: "/admin/lead", component: LeadPage },
          { path: "/admin/lead-tag", component: LeadTagPage },
          { path: "/admin/lead-form", component: LeadFormPage },
          { path: "/admin/lead-landing-page", component: LeadLandingPagePage },
          { path: "/admin/custom-field", component: CustomFieldPage },
          { path: "/admin/custom-field-value", component: CustomFieldValuePage },
          { path: "/admin/scheduler-type", component: SchedulerTypePage },
          { path: "/admin/booking", component: BookingPage },
          { path: "/admin/site", component: SitePage },
          { path: "/admin/site-page", component: SitePagePage },
          { path: "/admin/blog-post", component: BlogPostPage },
          { path: "/admin/conversation", component: ConversationPage },
          { path: "/admin/chat-message", component: ChatMessagePage },
          { path: "/admin/workflow", component: WorkflowPage },
          { path: "/admin/ai-chat-session", component: AiChatSessionPage },
          { path: "/admin/ai-chat-message", component: AiChatMessagePage },
          { path: "/admin/workspace-invite", component: WorkspaceInvitePage },
          { path: "/admin/workspace-usage-metric", component: WorkspaceUsageMetricPage },
          { path: "/admin/email-log", component: EmailLogPage },
          { path: "/admin/sms-log", component: SmsLogPage },
          { path: "/admin/whatsapp-log", component: WhatsappLogPage },
          { path: "/admin/lead-form-field", component: LeadFormFieldPage },
          { path: "/admin/lead-form-submission", component: LeadFormSubmissionPage },
          { path: "/admin/lead-stage-history", component: LeadStageHistoryPage },
          { path: "/admin/lead-message-template", component: LeadMessageTemplatePage },
          { path: "/admin/lead-automation", component: LeadAutomationPage },
          { path: "/admin/lead-scheduled-message", component: LeadScheduledMessagePage },
          { path: "/admin/scheduler-availability", component: SchedulerAvailabilityPage },
          { path: "/admin/scheduler-exception", component: SchedulerExceptionPage },
          { path: "/admin/site-page-version", component: SitePageVersionPage },
          { path: "/admin/blog-category", component: BlogCategoryPage },
          { path: "/admin/blog-post-version", component: BlogPostVersionPage },
          { path: "/admin/site-visit-event", component: SiteVisitEventPage },
          { path: "/admin/site-visit-daily-stat", component: SiteVisitDailyStatPage },
          { path: "/admin/message-attachment", component: MessageAttachmentPage },
          { path: "/admin/comment", component: CommentPage },
          { path: "/admin/file", component: FilePage },
          { path: "/admin/workflow-execution", component: WorkflowExecutionPage },
          { path: "/admin/workspace-access-event", component: WorkspaceAccessEventPage },
          { path: "/admin/landing-lead", component: LandingLeadPage },
          { path: "/admin/proposal-block-instance", component: ProposalBlockInstancePage },
          { path: "/admin/proposal-template-block", component: ProposalTemplateBlockPage },
          { path: "/admin/project-responsible", component: ProjectResponsiblePage },
          { path: "/admin/project-follower", component: ProjectFollowerPage },
          { path: "/admin/project-communication", component: ProjectCommunicationPage },
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
        { label: "Clients", href: "/admin/client", icon: LayoutDashboard, section: "entities" },
          { label: "ClientContacts", href: "/admin/client-contact", icon: LayoutDashboard, section: "entities" },
          { label: "ClientMessages", href: "/admin/client-message", icon: LayoutDashboard, section: "entities" },
          { label: "Projects", href: "/admin/project", icon: LayoutDashboard, section: "entities" },
          { label: "Tasks", href: "/admin/task", icon: LayoutDashboard, section: "entities" },
          { label: "TaskComments", href: "/admin/task-comment", icon: LayoutDashboard, section: "entities" },
          { label: "TimeEntries", href: "/admin/time-entry", icon: LayoutDashboard, section: "entities" },
          { label: "FinancialCategories", href: "/admin/financial-category", icon: LayoutDashboard, section: "entities" },
          { label: "Transactions", href: "/admin/transaction", icon: LayoutDashboard, section: "entities" },
          { label: "TransactionAttachments", href: "/admin/transaction-attachment", icon: LayoutDashboard, section: "entities" },
          { label: "Budgets", href: "/admin/budget", icon: LayoutDashboard, section: "entities" },
          { label: "Products", href: "/admin/product", icon: LayoutDashboard, section: "entities" },
          { label: "Proposals", href: "/admin/proposal", icon: LayoutDashboard, section: "entities" },
          { label: "ProposalItems", href: "/admin/proposal-item", icon: LayoutDashboard, section: "entities" },
          { label: "Contracts", href: "/admin/contract", icon: LayoutDashboard, section: "entities" },
          { label: "LeadWorkflows", href: "/admin/lead-workflow", icon: LayoutDashboard, section: "entities" },
          { label: "LeadWorkflowStages", href: "/admin/lead-workflow-stage", icon: LayoutDashboard, section: "entities" },
          { label: "Leads", href: "/admin/lead", icon: LayoutDashboard, section: "entities" },
          { label: "LeadTags", href: "/admin/lead-tag", icon: LayoutDashboard, section: "entities" },
          { label: "LeadForms", href: "/admin/lead-form", icon: LayoutDashboard, section: "entities" },
          { label: "LeadLandingPages", href: "/admin/lead-landing-page", icon: LayoutDashboard, section: "entities" },
          { label: "CustomFields", href: "/admin/custom-field", icon: LayoutDashboard, section: "entities" },
          { label: "CustomFieldValues", href: "/admin/custom-field-value", icon: LayoutDashboard, section: "entities" },
          { label: "SchedulerTypes", href: "/admin/scheduler-type", icon: LayoutDashboard, section: "entities" },
          { label: "Bookings", href: "/admin/booking", icon: LayoutDashboard, section: "entities" },
          { label: "Sites", href: "/admin/site", icon: LayoutDashboard, section: "entities" },
          { label: "SitePages", href: "/admin/site-page", icon: LayoutDashboard, section: "entities" },
          { label: "BlogPosts", href: "/admin/blog-post", icon: LayoutDashboard, section: "entities" },
          { label: "Conversations", href: "/admin/conversation", icon: LayoutDashboard, section: "entities" },
          { label: "ChatMessages", href: "/admin/chat-message", icon: LayoutDashboard, section: "entities" },
          { label: "Workflows", href: "/admin/workflow", icon: LayoutDashboard, section: "entities" },
          { label: "AiChatSessions", href: "/admin/ai-chat-session", icon: LayoutDashboard, section: "entities" },
          { label: "AiChatMessages", href: "/admin/ai-chat-message", icon: LayoutDashboard, section: "entities" },
          { label: "WorkspaceInvites", href: "/admin/workspace-invite", icon: LayoutDashboard, section: "entities" },
          { label: "WorkspaceUsageMetrics", href: "/admin/workspace-usage-metric", icon: LayoutDashboard, section: "entities" },
          { label: "EmailLogs", href: "/admin/email-log", icon: LayoutDashboard, section: "entities" },
          { label: "SmsLogs", href: "/admin/sms-log", icon: LayoutDashboard, section: "entities" },
          { label: "WhatsappLogs", href: "/admin/whatsapp-log", icon: LayoutDashboard, section: "entities" },
          { label: "LeadFormFields", href: "/admin/lead-form-field", icon: LayoutDashboard, section: "entities" },
          { label: "LeadFormSubmissions", href: "/admin/lead-form-submission", icon: LayoutDashboard, section: "entities" },
          { label: "LeadStageHistories", href: "/admin/lead-stage-history", icon: LayoutDashboard, section: "entities" },
          { label: "LeadMessageTemplates", href: "/admin/lead-message-template", icon: LayoutDashboard, section: "entities" },
          { label: "LeadAutomations", href: "/admin/lead-automation", icon: LayoutDashboard, section: "entities" },
          { label: "LeadScheduledMessages", href: "/admin/lead-scheduled-message", icon: LayoutDashboard, section: "entities" },
          { label: "SchedulerAvailabilities", href: "/admin/scheduler-availability", icon: LayoutDashboard, section: "entities" },
          { label: "SchedulerExceptions", href: "/admin/scheduler-exception", icon: LayoutDashboard, section: "entities" },
          { label: "SitePageVersions", href: "/admin/site-page-version", icon: LayoutDashboard, section: "entities" },
          { label: "BlogCategories", href: "/admin/blog-category", icon: LayoutDashboard, section: "entities" },
          { label: "BlogPostVersions", href: "/admin/blog-post-version", icon: LayoutDashboard, section: "entities" },
          { label: "SiteVisitEvents", href: "/admin/site-visit-event", icon: LayoutDashboard, section: "entities" },
          { label: "SiteVisitDailyStats", href: "/admin/site-visit-daily-stat", icon: LayoutDashboard, section: "entities" },
          { label: "MessageAttachments", href: "/admin/message-attachment", icon: LayoutDashboard, section: "entities" },
          { label: "Comments", href: "/admin/comment", icon: LayoutDashboard, section: "entities" },
          { label: "Files", href: "/admin/file", icon: LayoutDashboard, section: "entities" },
          { label: "WorkflowExecutions", href: "/admin/workflow-execution", icon: LayoutDashboard, section: "entities" },
          { label: "WorkspaceAccessEvents", href: "/admin/workspace-access-event", icon: LayoutDashboard, section: "entities" },
          { label: "LandingLeads", href: "/admin/landing-lead", icon: LayoutDashboard, section: "entities" },
          { label: "ProposalBlockInstances", href: "/admin/proposal-block-instance", icon: LayoutDashboard, section: "entities" },
          { label: "ProposalTemplateBlocks", href: "/admin/proposal-template-block", icon: LayoutDashboard, section: "entities" },
          { label: "ProjectResponsibles", href: "/admin/project-responsible", icon: LayoutDashboard, section: "entities" },
          { label: "ProjectFollowers", href: "/admin/project-follower", icon: LayoutDashboard, section: "entities" },
          { label: "ProjectCommunications", href: "/admin/project-communication", icon: LayoutDashboard, section: "entities" },
      // <GEN-MENU>
];
