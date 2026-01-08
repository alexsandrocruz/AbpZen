import { Route, Switch, Redirect } from "wouter";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ThemeProvider } from "@/providers/theme-provider";
import { AbpProvider } from "@/providers/abp-provider";
import DashboardPage from "@/pages/dashboard";
import HostWorkspacesPage from "@/pages/host/workspaces";
import HostUsersPage from "@/pages/host/users";
import HostRolesPage from "@/pages/host/roles";
import HostSettingsPage from "@/pages/host/settings";
import HostEditionsPage from "@/pages/host/editions";
import LoginPage from "@/pages/auth/login";
import "./index.css";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 60 * 1000, // 1 minute
      retry: 1,
    },
  },
});

function App() {
  return (
    <ThemeProvider defaultTheme="system" storageKey="abp-react-theme">
      <QueryClientProvider client={queryClient}>
        <AbpProvider>
          <Switch>
            {/* Auth routes */}
            <Route path="/auth/login" component={LoginPage} />

            {/* Main routes */}
            <Route path="/dashboard" component={DashboardPage} />

            {/* Host Admin routes */}
            <Route path="/host/workspaces" component={HostWorkspacesPage} />
            <Route path="/host/users" component={HostUsersPage} />
            <Route path="/host/roles" component={HostRolesPage} />
            <Route path="/host/settings" component={HostSettingsPage} />
            <Route path="/host/editions" component={HostEditionsPage} />

            {/* Default redirect */}
            <Route path="/">
              <Redirect to="/dashboard" />
            </Route>

            {/* 404 */}
            <Route>
              <div className="flex items-center justify-center min-h-screen">
                <div className="text-center">
                  <h1 className="text-4xl font-bold">404</h1>
                  <p className="text-muted-foreground">Page not found</p>
                </div>
              </div>
            </Route>
          </Switch>
        </AbpProvider>
      </QueryClientProvider>
    </ThemeProvider>
  );
}

export default App;
