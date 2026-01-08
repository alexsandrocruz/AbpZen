import { Switch, Route, Redirect } from "wouter";
import { queryClient } from "./lib/queryClient";
import { QueryClientProvider } from "@tanstack/react-query";
import { Toaster } from "@/components/ui/toaster";
import { TooltipProvider } from "@/components/ui/tooltip";
import { useAuth } from "@/lib/auth-store";
import { useEffect, useState } from "react";
import NotFound from "@/pages/not-found";
import PublicProposalPage from "@/pages/public-proposal-page";
import PublicContractPage from "@/pages/public-contract-page";
import PublicLandingPage from "@/pages/public-landing-page";

import AuthPage from "@/pages/auth-page";
import DashboardPage from "@/pages/dashboard-page";
import ClientsPage from "@/pages/clients-page";
import ProjectsPage from "@/pages/projects-page";
import ProjectDetailPage from "@/pages/project-detail-page";
import TasksPage from "@/pages/tasks-page";
import TimeEntriesPage from "@/pages/time-entries-page";
import ProposalsPage from "@/pages/proposals-page";
import InvoicesPage from "@/pages/invoices-page";
import ProductsPage from "@/pages/products-page";
import TransactionsPage from "@/pages/transactions-page";
import ContractsPage from "@/pages/contracts-page";
import LeadWorkflowsPage from "@/pages/lead-workflows-page";
import LeadFormsPage from "@/pages/lead-forms-page";
import LeadLandingPagesPage from "@/pages/lead-landing-pages-page";
import SettingsPage from "@/pages/settings-page";
import ProfilePage from "@/pages/profile-page";
import BillingPage from "@/pages/billing-page";
import TeamPage from "@/pages/team-page";
import AcceptInvitePage from "@/pages/accept-invite-page";
import ProposalFormPage from "@/pages/proposal-form-page";
import ProposalVisualBuilderPage from "@/pages/proposal-visual-builder-page";
import ProposalTemplatesPage from "@/pages/proposal-templates-page";
import ReportsPage from "@/pages/reports-page";
import ClientDetailPage from "@/pages/client-detail-page";
import ClientEditPage from "@/pages/client-edit-page";
import SchedulersPage from "@/pages/schedulers-page";
import SitesPage from "@/pages/sites-page";
import SiteBuilderPage from "@/pages/site-builder-page";
import BlogPostsPage from "@/pages/blog-posts-page";
import PublicBookingPage from "@/pages/public-booking-page";
import LeadsPage from "@/pages/leads-page";
import PublicSitePage from "@/pages/public-site-page";
import MessagesPage from "@/pages/messages-page";
import FilesPage from "@/pages/files-page";
import WorkflowsPage from "@/pages/workflows-page";
import SuperAiPage from "@/pages/super-ai-page";
import HostDashboardPage from "@/pages/host/host-dashboard-page";
import HostWorkspacesPage from "@/pages/host/host-workspaces-page";
import HostUsersPage from "@/pages/host/host-users-page";
import LandingPage from "@/pages/landing-page";
import RecursosPage from "@/pages/recursos-page";
import ForgotPasswordPage from "@/pages/forgot-password-page";
import ResetPasswordPage from "@/pages/reset-password-page";

function ProtectedRoute({ component: Component }: { component: React.ComponentType }) {
  const { user, checkAuth, isLoading } = useAuth();
  const [isChecking, setIsChecking] = useState(true);

  useEffect(() => {
    const doCheck = async () => {
      await checkAuth();
      setIsChecking(false);
    };
    doCheck();
  }, []);

  if (isChecking || isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary"></div>
      </div>
    );
  }

  if (!user) {
    return <Redirect to="/" />;
  }

  return <Component />;
}

function HostProtectedRoute({ component: Component }: { component: React.ComponentType }) {
  const { user, checkAuth, isLoading } = useAuth();
  const [isChecking, setIsChecking] = useState(true);

  useEffect(() => {
    const doCheck = async () => {
      await checkAuth();
      setIsChecking(false);
    };
    doCheck();
  }, []);

  if (isChecking || isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary"></div>
      </div>
    );
  }

  if (!user) {
    return <Redirect to="/" />;
  }

  if (user.globalRole !== 'HOST') {
    return <Redirect to="/" />;
  }

  return <Component />;
}

function Router() {
  return (
    <Switch>
      {/* Public Landing Page */}
      <Route path="/" component={LandingPage} />
      <Route path="/login" component={AuthPage} />
      <Route path="/forgot-password" component={ForgotPasswordPage} />
      <Route path="/reset-password/:token" component={ResetPasswordPage} />
      <Route path="/recursos" component={RecursosPage} />
      
      {/* Host Panel Routes - MUST come before :slug routes */}
      <Route path="/host/dashboard">
        {() => <HostProtectedRoute component={HostDashboardPage} />}
      </Route>
      <Route path="/host/workspaces">
        {() => <HostProtectedRoute component={HostWorkspacesPage} />}
      </Route>
      <Route path="/host/users">
        {() => <HostProtectedRoute component={HostUsersPage} />}
      </Route>
      
      {/* Workspace Routes */}
      <Route path="/:slug/dashboard">
        {() => <ProtectedRoute component={DashboardPage} />}
      </Route>
      <Route path="/:slug/clients">
        {() => <ProtectedRoute component={ClientsPage} />}
      </Route>
      <Route path="/:slug/clients/:id">
        {() => <ProtectedRoute component={ClientDetailPage} />}
      </Route>
      <Route path="/:slug/clients/:id/edit">
        {() => <ProtectedRoute component={ClientEditPage} />}
      </Route>
      <Route path="/:slug/projects">
        {() => <ProtectedRoute component={ProjectsPage} />}
      </Route>
      <Route path="/:slug/projects/:projectId">
        {() => <ProtectedRoute component={ProjectDetailPage} />}
      </Route>
      <Route path="/:slug/tasks">
        {() => <ProtectedRoute component={TasksPage} />}
      </Route>
      <Route path="/:slug/time">
        {() => <ProtectedRoute component={TimeEntriesPage} />}
      </Route>
      <Route path="/:slug/proposals">
        {() => <ProtectedRoute component={ProposalsPage} />}
      </Route>
      <Route path="/:slug/proposals/new">
        {() => <ProtectedRoute component={ProposalFormPage} />}
      </Route>
      <Route path="/:slug/proposals/:id/edit">
        {() => <ProtectedRoute component={ProposalFormPage} />}
      </Route>
      <Route path="/:slug/proposals/:id/visual-builder">
        {() => <ProtectedRoute component={ProposalVisualBuilderPage} />}
      </Route>
      <Route path="/:slug/proposal-templates">
        {() => <ProtectedRoute component={ProposalTemplatesPage} />}
      </Route>
      <Route path="/:slug/proposal-templates/:id/builder">
        {() => <ProtectedRoute component={ProposalVisualBuilderPage} />}
      </Route>
      <Route path="/:slug/invoices">
        {() => <ProtectedRoute component={InvoicesPage} />}
      </Route>
      <Route path="/:slug/products">
        {() => <ProtectedRoute component={ProductsPage} />}
      </Route>
      <Route path="/:slug/financeiro">
        {() => <ProtectedRoute component={TransactionsPage} />}
      </Route>
      <Route path="/:slug/contracts">
        {() => <ProtectedRoute component={ContractsPage} />}
      </Route>
      <Route path="/:slug/leads/workflows">
        {() => <ProtectedRoute component={LeadWorkflowsPage} />}
      </Route>
      <Route path="/:slug/leads/forms">
        {() => <ProtectedRoute component={LeadFormsPage} />}
      </Route>
      <Route path="/:slug/leads/landing-pages">
        {() => <ProtectedRoute component={LeadLandingPagesPage} />}
      </Route>
      <Route path="/:slug/schedulers">
        {() => <ProtectedRoute component={SchedulersPage} />}
      </Route>
      <Route path="/:slug/leads">
        {() => <ProtectedRoute component={LeadsPage} />}
      </Route>
      
      {/* Sites (Site Builder) Routes */}
      <Route path="/:slug/sites">
        {() => <ProtectedRoute component={SitesPage} />}
      </Route>
      <Route path="/:slug/sites/:siteId/builder">
        {() => <ProtectedRoute component={SiteBuilderPage} />}
      </Route>
      <Route path="/:slug/sites/:siteId/blog">
        {() => <ProtectedRoute component={BlogPostsPage} />}
      </Route>
      
      {/* Reports Route */}
      <Route path="/:slug/reports">
        {() => <ProtectedRoute component={ReportsPage} />}
      </Route>

      {/* Settings Route */}
      <Route path="/:slug/settings">
        {() => <ProtectedRoute component={SettingsPage} />}
      </Route>

      {/* Account Routes */}
      <Route path="/perfil">
        {() => <ProtectedRoute component={ProfilePage} />}
      </Route>
      <Route path="/:slug/cobranca">
        {() => <ProtectedRoute component={BillingPage} />}
      </Route>
      <Route path="/:slug/equipe">
        {() => <ProtectedRoute component={TeamPage} />}
      </Route>
      <Route path="/:slug/messages">
        {() => <ProtectedRoute component={MessagesPage} />}
      </Route>
      <Route path="/:slug/files">
        {() => <ProtectedRoute component={FilesPage} />}
      </Route>

      {/* Automation Routes */}
      <Route path="/:slug/workflows">
        {() => <ProtectedRoute component={WorkflowsPage} />}
      </Route>
      <Route path="/:slug/super-ai">
        {() => <ProtectedRoute component={SuperAiPage} />}
      </Route>

      {/* Public Routes (no auth required) */}
      <Route path="/p/:token" component={PublicProposalPage} />
      <Route path="/c/:token" component={PublicContractPage} />
      <Route path="/l/:slug" component={PublicLandingPage} />
      <Route path="/s/:workspaceSlug/:schedulerSlug" component={PublicBookingPage} />
      <Route path="/invite/:token" component={AcceptInvitePage} />
      
      {/* Public Site Routes */}
      <Route path="/site/:workspaceSlug/:siteSlug/:rest*" component={PublicSitePage} />
      <Route path="/site/:workspaceSlug/:siteSlug" component={PublicSitePage} />

      <Route component={NotFound} />
    </Switch>
  );
}

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <TooltipProvider>
        <Toaster />
        <Router />
      </TooltipProvider>
    </QueryClientProvider>
  );
}

export default App;
