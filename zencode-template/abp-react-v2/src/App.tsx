import { Route, Switch, Redirect } from "wouter";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import DashboardPage from "@/pages/dashboard";
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
    <QueryClientProvider client={queryClient}>
      <Switch>
        <Route path="/dashboard" component={DashboardPage} />

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
    </QueryClientProvider>
  );
}

export default App;
