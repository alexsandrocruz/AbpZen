import { useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { HostLayout } from "@/components/layout/host-layout";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Badge } from "@/components/ui/badge";
import { 
  Building2, 
  Users, 
  TrendingUp,
  TrendingDown,
  DollarSign,
  Activity,
  Crown,
  Loader2,
  UserPlus,
  UserMinus,
  BarChart3,
  ArrowUpRight,
  ArrowDownRight,
} from "lucide-react";
import { formatDistanceToNow } from "date-fns";
import { ptBR } from "date-fns/locale";

interface DashboardMetrics {
  totalWorkspaces: number;
  totalUsers: number;
  totalClients: number;
  totalProjects: number;
  mrr: number;
  arr: number;
  newWorkspacesThisMonth: number;
  churnedWorkspacesThisMonth: number;
  churnRate: number;
  activeUsersLast30Days: number;
  avgRevenuePerWorkspace: number;
  planCounts: {
    TRIAL: number;
    STARTER: number;
    PROFESSIONAL: number;
    ENTERPRISE: number;
  };
  recentEvents: any[];
}

const formatCurrency = (value: number) => {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
  }).format(value);
};

const formatPercent = (value: number) => {
  return `${value.toFixed(1)}%`;
};

export default function HostDashboardPage() {
  const { data: metrics, isLoading } = useQuery<DashboardMetrics>({
    queryKey: ["host", "dashboard"],
    queryFn: api.getHostDashboard,
  });

  if (isLoading) {
    return (
      <HostLayout>
        <div className="flex items-center justify-center h-full min-h-[60vh]">
          <Loader2 className="h-8 w-8 animate-spin text-primary" />
        </div>
      </HostLayout>
    );
  }

  const planLabels: Record<string, { label: string; color: string; price: number }> = {
    TRIAL: { label: "Trial", color: "bg-gray-500", price: 0 },
    STARTER: { label: "Starter", color: "bg-blue-500", price: 97 },
    PROFESSIONAL: { label: "Professional", color: "bg-purple-500", price: 197 },
    ENTERPRISE: { label: "Enterprise", color: "bg-amber-500", price: 497 },
  };

  const mrr = metrics?.mrr || 0;
  const arr = metrics?.arr || mrr * 12;
  const newWorkspaces = metrics?.newWorkspacesThisMonth || 0;
  const churnedWorkspaces = metrics?.churnedWorkspacesThisMonth || 0;
  const churnRate = metrics?.churnRate || 0;
  const activeUsers = metrics?.activeUsersLast30Days || metrics?.totalUsers || 0;
  const avgRevenue = metrics?.avgRevenuePerWorkspace || 0;

  return (
    <HostLayout>
      <div className="p-6 space-y-6">
        <div className="flex items-center justify-between mb-8">
          <div className="flex items-center gap-3">
            <Crown className="h-8 w-8 text-amber-500" />
            <div>
              <h1 className="text-3xl font-bold">Painel Host</h1>
              <p className="text-muted-foreground">Métricas e visão geral da plataforma</p>
            </div>
          </div>
          <Badge variant="outline" className="text-amber-500 border-amber-500">
            <Activity className="h-3 w-3 mr-1" />
            Tempo Real
          </Badge>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
          <Card className="bg-gradient-to-br from-green-500/10 to-green-500/5 border-green-500/20" data-testid="card-mrr">
            <CardHeader className="flex flex-row items-center justify-between pb-2">
              <CardTitle className="text-sm font-medium">MRR</CardTitle>
              <DollarSign className="h-4 w-4 text-green-500" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold text-green-600">{formatCurrency(mrr)}</div>
              <p className="text-xs text-muted-foreground">Receita Mensal Recorrente</p>
            </CardContent>
          </Card>

          <Card className="bg-gradient-to-br from-emerald-500/10 to-emerald-500/5 border-emerald-500/20" data-testid="card-arr">
            <CardHeader className="flex flex-row items-center justify-between pb-2">
              <CardTitle className="text-sm font-medium">ARR</CardTitle>
              <BarChart3 className="h-4 w-4 text-emerald-500" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold text-emerald-600">{formatCurrency(arr)}</div>
              <p className="text-xs text-muted-foreground">Receita Anual Recorrente</p>
            </CardContent>
          </Card>

          <Card className="bg-gradient-to-br from-blue-500/10 to-blue-500/5 border-blue-500/20" data-testid="card-new-workspaces">
            <CardHeader className="flex flex-row items-center justify-between pb-2">
              <CardTitle className="text-sm font-medium">Novos Clientes</CardTitle>
              <UserPlus className="h-4 w-4 text-blue-500" />
            </CardHeader>
            <CardContent>
              <div className="flex items-baseline gap-2">
                <span className="text-2xl font-bold text-blue-600">{newWorkspaces}</span>
                <span className="text-xs text-green-500 flex items-center">
                  <ArrowUpRight className="h-3 w-3" />
                  este mês
                </span>
              </div>
              <p className="text-xs text-muted-foreground">Workspaces criados</p>
            </CardContent>
          </Card>

          <Card className="bg-gradient-to-br from-red-500/10 to-red-500/5 border-red-500/20" data-testid="card-churn">
            <CardHeader className="flex flex-row items-center justify-between pb-2">
              <CardTitle className="text-sm font-medium">Churn Rate</CardTitle>
              <UserMinus className="h-4 w-4 text-red-500" />
            </CardHeader>
            <CardContent>
              <div className="flex items-baseline gap-2">
                <span className="text-2xl font-bold text-red-600">{formatPercent(churnRate)}</span>
                {churnRate > 5 && (
                  <span className="text-xs text-red-500 flex items-center">
                    <ArrowDownRight className="h-3 w-3" />
                    alto
                  </span>
                )}
              </div>
              <p className="text-xs text-muted-foreground">{churnedWorkspaces} cancelamentos este mês</p>
            </CardContent>
          </Card>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
          <Card data-testid="card-total-workspaces">
            <CardHeader className="flex flex-row items-center justify-between pb-2">
              <CardTitle className="text-sm font-medium">Total Workspaces</CardTitle>
              <Building2 className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics?.totalWorkspaces || 0}</div>
              <p className="text-xs text-muted-foreground">Organizações ativas</p>
            </CardContent>
          </Card>

          <Card data-testid="card-total-users">
            <CardHeader className="flex flex-row items-center justify-between pb-2">
              <CardTitle className="text-sm font-medium">Total Usuários</CardTitle>
              <Users className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics?.totalUsers || 0}</div>
              <p className="text-xs text-muted-foreground">Usuários cadastrados</p>
            </CardContent>
          </Card>

          <Card data-testid="card-active-users">
            <CardHeader className="flex flex-row items-center justify-between pb-2">
              <CardTitle className="text-sm font-medium">Usuários Ativos</CardTitle>
              <Activity className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{activeUsers}</div>
              <p className="text-xs text-muted-foreground">Últimos 30 dias</p>
            </CardContent>
          </Card>

          <Card data-testid="card-avg-revenue">
            <CardHeader className="flex flex-row items-center justify-between pb-2">
              <CardTitle className="text-sm font-medium">Ticket Médio</CardTitle>
              <TrendingUp className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{formatCurrency(avgRevenue)}</div>
              <p className="text-xs text-muted-foreground">Por workspace</p>
            </CardContent>
          </Card>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
          <Card data-testid="card-plan-distribution">
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <BarChart3 className="h-5 w-5" />
                Distribuição por Plano
              </CardTitle>
            </CardHeader>
            <CardContent>
              <div className="space-y-4">
                {Object.entries(metrics?.planCounts || {}).map(([plan, count]) => {
                  const planInfo = planLabels[plan] || { label: plan, color: "bg-gray-500", price: 0 };
                  const total = Object.values(metrics?.planCounts || {}).reduce((a, b) => a + b, 0);
                  const percentage = total > 0 ? Math.round((count / total) * 100) : 0;
                  const revenue = count * planInfo.price;
                  
                  return (
                    <div key={plan} className="space-y-2" data-testid={`plan-row-${plan.toLowerCase()}`}>
                      <div className="flex items-center justify-between">
                        <div className="flex items-center gap-2">
                          <div className={`w-3 h-3 rounded-full ${planInfo.color}`} />
                          <span className="font-medium">{planInfo.label}</span>
                          <Badge variant="secondary" className="text-xs">
                            {formatCurrency(planInfo.price)}/mês
                          </Badge>
                        </div>
                        <div className="text-right">
                          <span className="text-sm font-medium">{count} clientes</span>
                          <span className="text-xs text-muted-foreground ml-2">
                            ({formatCurrency(revenue)}/mês)
                          </span>
                        </div>
                      </div>
                      <div className="h-2 bg-muted rounded-full overflow-hidden">
                        <div 
                          className={`h-full ${planInfo.color} transition-all`} 
                          style={{ width: `${percentage}%` }}
                        />
                      </div>
                    </div>
                  );
                })}
              </div>
            </CardContent>
          </Card>

          <Card data-testid="card-recent-activity">
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Activity className="h-5 w-5" />
                Atividade Recente
              </CardTitle>
            </CardHeader>
            <CardContent>
              <div className="space-y-4">
                {metrics?.recentEvents && metrics.recentEvents.length > 0 ? (
                  metrics.recentEvents.map((event: any, index: number) => (
                    <div key={event.id || index} className="flex items-center gap-3" data-testid={`activity-row-${index}`}>
                      <Avatar className="h-8 w-8">
                        <AvatarImage src={event.user?.avatar} />
                        <AvatarFallback>
                          {event.user?.name?.charAt(0) || "U"}
                        </AvatarFallback>
                      </Avatar>
                      <div className="flex-1 min-w-0">
                        <p className="text-sm font-medium truncate">
                          {event.user?.name || "Usuário"}
                        </p>
                        <p className="text-xs text-muted-foreground truncate">
                          {event.eventType} - {event.workspace?.name || "Workspace"}
                        </p>
                      </div>
                      <span className="text-xs text-muted-foreground whitespace-nowrap">
                        {event.createdAt && formatDistanceToNow(new Date(event.createdAt), { 
                          addSuffix: true, 
                          locale: ptBR 
                        })}
                      </span>
                    </div>
                  ))
                ) : (
                  <p className="text-sm text-muted-foreground text-center py-4">
                    Nenhuma atividade recente
                  </p>
                )}
              </div>
            </CardContent>
          </Card>
        </div>
      </div>
    </HostLayout>
  );
}
