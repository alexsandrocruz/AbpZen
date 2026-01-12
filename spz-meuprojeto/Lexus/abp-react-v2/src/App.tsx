import { Route, Switch, Redirect } from "wouter";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ThemeProvider } from "@/providers/theme-provider";
import { AbpProvider } from "@/providers/abp-provider";
import { Toaster } from "@/components/ui/toaster";
import { routes } from "@/config/navigation";
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

            {/* Dynamic routes from config */}
            {routes.map((route) => (
              <Route
                key={route.path}
                path={route.path}
                component={route.component}
              />
            ))}

            {/* Default redirect */}
            <Route path="/">
              <Redirect to="/dashboard" />
            </Route>

            {/* 404 */}
            <Route>
              <div className="flex items-center justify-center min-h-screen text-center">
                <div>
                  <h1 className="text-4xl font-bold">404</h1>
                  <p className="text-muted-foreground">Page not found</p>
                </div>
              </div>
            </Route>
          </Switch>
          <Toaster />
        </AbpProvider>
      </QueryClientProvider>
    </ThemeProvider>
  );
}

export default App;

