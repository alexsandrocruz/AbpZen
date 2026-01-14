import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { 
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Badge } from "@/components/ui/badge";
import { Skeleton } from "@/components/ui/skeleton";
import { 
  DollarSign, 
  Users, 
  Briefcase, 
  TrendingUp,
  TrendingDown,
  Calendar,
  FileText,
  PieChart,
  BarChart3,
  ArrowUpRight,
  ArrowDownRight,
  Download,
} from "lucide-react";
import { useAuth } from "@/lib/auth-store";
import { api } from "@/lib/api";
import {
  AreaChart,
  Area,
  BarChart,
  Bar,
  LineChart,
  Line,
  PieChart as RechartsPieChart,
  Pie,
  Cell,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";
import { format, subMonths, startOfMonth, endOfMonth, parseISO, isWithinInterval, isAfter, isBefore } from "date-fns";
import { ptBR } from "date-fns/locale";

const COLORS = ['#3B82F6', '#10B981', '#F59E0B', '#EF4444', '#8B5CF6', '#EC4899', '#06B6D4', '#84CC16'];

function useDateFilter(months: number) {
  const now = new Date();
  const startDate = startOfMonth(subMonths(now, months - 1));
  const endDate = endOfMonth(now);
  return { startDate, endDate };
}

function isInPeriod(dateStr: string | null | undefined, startDate: Date, endDate: Date): boolean {
  if (!dateStr) return false;
  try {
    const date = parseISO(dateStr);
    return isWithinInterval(date, { start: startDate, end: endDate });
  } catch {
    return false;
  }
}

interface MetricCardProps {
  title: string;
  value: string | number;
  change?: number;
  icon: React.ReactNode;
  loading?: boolean;
}

function MetricCard({ title, value, change, icon, loading }: MetricCardProps) {
  if (loading) {
    return (
      <TactileCard className="p-6">
        <div className="flex items-center justify-between">
          <Skeleton className="h-10 w-10 rounded-lg" />
          <Skeleton className="h-4 w-16" />
        </div>
        <Skeleton className="h-8 w-24 mt-4" />
        <Skeleton className="h-4 w-32 mt-2" />
      </TactileCard>
    );
  }

  return (
    <TactileCard className="p-6">
      <div className="flex items-center justify-between">
        <div className="p-3 rounded-lg bg-primary/10 text-primary">
          {icon}
        </div>
        {change !== undefined && (
          <Badge 
            variant={change >= 0 ? "default" : "destructive"} 
            className={`${change >= 0 ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'}`}
          >
            {change >= 0 ? <ArrowUpRight className="size-3 mr-1" /> : <ArrowDownRight className="size-3 mr-1" />}
            {Math.abs(change).toFixed(1)}%
          </Badge>
        )}
      </div>
      <div className="mt-4">
        <p className="text-2xl font-bold">{value}</p>
        <p className="text-sm text-muted-foreground mt-1">{title}</p>
      </div>
    </TactileCard>
  );
}

function formatCurrency(value: number): string {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(value);
}

export default function ReportsPage() {
  const { currentWorkspace } = useAuth();
  const [period, setPeriod] = useState("12");
  const [activeTab, setActiveTab] = useState("overview");

  const { data: stats, isLoading: statsLoading } = useQuery({
    queryKey: ["dashboard-stats", currentWorkspace?.id],
    queryFn: () => api.getDashboardStats(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: cashflow, isLoading: cashflowLoading } = useQuery({
    queryKey: ["cashflow", currentWorkspace?.id],
    queryFn: () => api.getCashflowData(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: forecast, isLoading: forecastLoading } = useQuery({
    queryKey: ["cashflow-forecast", currentWorkspace?.id],
    queryFn: () => api.getCashflowForecast(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: clients = [], isLoading: clientsLoading } = useQuery({
    queryKey: ["clients", currentWorkspace?.id],
    queryFn: () => api.getClients(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: projects = [], isLoading: projectsLoading } = useQuery({
    queryKey: ["projects", currentWorkspace?.id],
    queryFn: () => api.getProjects(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: proposals = [], isLoading: proposalsLoading } = useQuery({
    queryKey: ["proposals", currentWorkspace?.id],
    queryFn: () => api.getProposals(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: transactions = [], isLoading: transactionsLoading } = useQuery({
    queryKey: ["transactions", currentWorkspace?.id],
    queryFn: () => api.getTransactions(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const isLoading = statsLoading || cashflowLoading || forecastLoading;

  const { startDate, endDate } = useDateFilter(parseInt(period));

  const filteredTransactions = transactions.filter((t: any) => 
    isInPeriod(t.dueDate || t.date, startDate, endDate)
  );

  const filteredProjects = projects.filter((p: any) => 
    isInPeriod(p.createdAt || p.startDate, startDate, endDate)
  );

  const filteredProposals = proposals.filter((p: any) => 
    isInPeriod(p.createdAt || p.issueDate, startDate, endDate)
  );

  const filteredForecast = (forecast || []).slice(-parseInt(period));

  const projectsByStatus = filteredProjects.reduce((acc: Record<string, number>, p: any) => {
    acc[p.status] = (acc[p.status] || 0) + 1;
    return acc;
  }, {});

  const projectStatusData = Object.entries(projectsByStatus).map(([status, count]) => ({
    name: status === 'ACTIVE' ? 'Ativos' : 
          status === 'COMPLETED' ? 'Concluídos' : 
          status === 'ON_HOLD' ? 'Em Espera' : 
          status === 'CANCELLED' ? 'Cancelados' : status,
    value: count,
  }));

  const proposalsByStatus = filteredProposals.reduce((acc: Record<string, number>, p: any) => {
    acc[p.status] = (acc[p.status] || 0) + 1;
    return acc;
  }, {});

  const proposalStatusData = Object.entries(proposalsByStatus).map(([status, count]) => ({
    name: status === 'DRAFT' ? 'Rascunho' :
          status === 'SENT' ? 'Enviada' :
          status === 'APPROVED' ? 'Aprovada' :
          status === 'REJECTED' ? 'Rejeitada' :
          status === 'EXPIRED' ? 'Expirada' : status,
    value: count,
  }));

  const totalIncome = filteredTransactions
    .filter((t: any) => t.type === 'INCOME' && t.status === 'PAID')
    .reduce((sum: number, t: any) => sum + parseFloat(t.amount || 0), 0);

  const totalExpense = filteredTransactions
    .filter((t: any) => t.type === 'EXPENSE' && t.status === 'PAID')
    .reduce((sum: number, t: any) => sum + parseFloat(t.amount || 0), 0);

  const pendingReceivables = filteredTransactions
    .filter((t: any) => t.type === 'INCOME' && t.status === 'PENDING')
    .reduce((sum: number, t: any) => sum + parseFloat(t.amount || 0), 0);

  const pendingPayables = filteredTransactions
    .filter((t: any) => t.type === 'EXPENSE' && t.status === 'PENDING')
    .reduce((sum: number, t: any) => sum + parseFloat(t.amount || 0), 0);

  const topClients = [...clients]
    .map((c: any) => {
      const clientTransactions = filteredTransactions.filter((t: any) => 
        t.clientId === c.id && t.type === 'INCOME' && t.status === 'PAID'
      );
      const total = clientTransactions.reduce((sum: number, t: any) => sum + parseFloat(t.amount || 0), 0);
      return { ...c, totalRevenue: total };
    })
    .sort((a, b) => b.totalRevenue - a.totalRevenue)
    .slice(0, 5);

  const filteredIncomeByCategory = filteredTransactions
    .filter((t: any) => t.type === 'INCOME' && t.status === 'PAID')
    .reduce((acc: Record<string, number>, t: any) => {
      const cat = t.categoryName || 'Sem categoria';
      acc[cat] = (acc[cat] || 0) + parseFloat(t.amount || 0);
      return acc;
    }, {});

  const incomeByCategData = Object.entries(filteredIncomeByCategory)
    .map(([name, value]) => ({ name, value }))
    .sort((a, b) => b.value - a.value);

  const CustomTooltip = ({ active, payload, label }: any) => {
    if (active && payload && payload.length) {
      return (
        <div className="bg-background border rounded-lg shadow-lg p-3">
          <p className="font-medium">{label}</p>
          {payload.map((entry: any, index: number) => (
            <p key={index} style={{ color: entry.color }} className="text-sm">
              {entry.name}: {formatCurrency(entry.value)}
            </p>
          ))}
        </div>
      );
    }
    return null;
  };

  if (!currentWorkspace) {
    return (
      <AppShell>
        <div className="flex items-center justify-center h-full">
          <p className="text-muted-foreground">Carregando...</p>
        </div>
      </AppShell>
    );
  }

  return (
    <AppShell>
      <div className="container py-6 space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-2xl font-bold">Relatórios e Análises</h1>
            <p className="text-muted-foreground">Visualize os dados do seu negócio</p>
          </div>
          <div className="flex items-center gap-3">
            <Select value={period} onValueChange={setPeriod}>
              <SelectTrigger className="w-40" data-testid="select-period">
                <Calendar className="size-4 mr-2" />
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="3">Últimos 3 meses</SelectItem>
                <SelectItem value="6">Últimos 6 meses</SelectItem>
                <SelectItem value="12">Últimos 12 meses</SelectItem>
                <SelectItem value="24">Últimos 24 meses</SelectItem>
              </SelectContent>
            </Select>
            <Button variant="outline" data-testid="button-export">
              <Download className="size-4 mr-2" />
              Exportar
            </Button>
          </div>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
          <MetricCard
            title="Total de Clientes"
            value={clients.length}
            icon={<Users className="size-5" />}
            loading={clientsLoading}
          />
          <MetricCard
            title="Projetos Ativos"
            value={stats?.activeProjects || 0}
            icon={<Briefcase className="size-5" />}
            loading={statsLoading}
          />
          <MetricCard
            title="Receita Total"
            value={formatCurrency(totalIncome)}
            icon={<TrendingUp className="size-5" />}
            loading={transactionsLoading}
          />
          <MetricCard
            title="Despesa Total"
            value={formatCurrency(totalExpense)}
            icon={<TrendingDown className="size-5" />}
            loading={transactionsLoading}
          />
        </div>

        <Tabs value={activeTab} onValueChange={setActiveTab} className="space-y-4">
          <TabsList>
            <TabsTrigger value="overview" data-testid="tab-overview">
              <BarChart3 className="size-4 mr-2" />
              Visão Geral
            </TabsTrigger>
            <TabsTrigger value="financial" data-testid="tab-financial">
              <DollarSign className="size-4 mr-2" />
              Financeiro
            </TabsTrigger>
            <TabsTrigger value="clients" data-testid="tab-clients">
              <Users className="size-4 mr-2" />
              Clientes
            </TabsTrigger>
            <TabsTrigger value="projects" data-testid="tab-projects">
              <Briefcase className="size-4 mr-2" />
              Projetos
            </TabsTrigger>
          </TabsList>

          <TabsContent value="overview" className="space-y-4">
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">
              <TactileCard className="p-6">
                <h3 className="font-semibold mb-4">Fluxo de Caixa - Previsão</h3>
                {forecastLoading ? (
                  <Skeleton className="h-64 w-full" />
                ) : (
                  <ResponsiveContainer width="100%" height={280}>
                    <AreaChart data={filteredForecast}>
                      <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                      <XAxis 
                        dataKey="month" 
                        tickFormatter={(value) => {
                          const [year, month] = value.split('-');
                          return format(new Date(parseInt(year), parseInt(month) - 1), 'MMM', { locale: ptBR });
                        }}
                        className="text-xs"
                      />
                      <YAxis 
                        tickFormatter={(value) => `R$ ${(value / 1000).toFixed(0)}k`}
                        className="text-xs"
                      />
                      <Tooltip content={<CustomTooltip />} />
                      <Legend />
                      <Area 
                        type="monotone" 
                        dataKey="income" 
                        name="Receitas" 
                        stroke="#10B981" 
                        fill="#10B98130" 
                      />
                      <Area 
                        type="monotone" 
                        dataKey="expense" 
                        name="Despesas" 
                        stroke="#EF4444" 
                        fill="#EF444430" 
                      />
                    </AreaChart>
                  </ResponsiveContainer>
                )}
              </TactileCard>

              <TactileCard className="p-6">
                <h3 className="font-semibold mb-4">Saldo Acumulado</h3>
                {forecastLoading ? (
                  <Skeleton className="h-64 w-full" />
                ) : (
                  <ResponsiveContainer width="100%" height={280}>
                    <LineChart data={filteredForecast}>
                      <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                      <XAxis 
                        dataKey="month" 
                        tickFormatter={(value) => {
                          const [year, month] = value.split('-');
                          return format(new Date(parseInt(year), parseInt(month) - 1), 'MMM', { locale: ptBR });
                        }}
                        className="text-xs"
                      />
                      <YAxis 
                        tickFormatter={(value) => `R$ ${(value / 1000).toFixed(0)}k`}
                        className="text-xs"
                      />
                      <Tooltip content={<CustomTooltip />} />
                      <Line 
                        type="monotone" 
                        dataKey="balance" 
                        name="Saldo" 
                        stroke="#3B82F6" 
                        strokeWidth={2}
                        dot={{ fill: '#3B82F6' }}
                      />
                    </LineChart>
                  </ResponsiveContainer>
                )}
              </TactileCard>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-4">
              <TactileCard className="p-6">
                <h3 className="font-semibold mb-4">Projetos por Status</h3>
                {projectsLoading ? (
                  <Skeleton className="h-48 w-full" />
                ) : projectStatusData.length > 0 ? (
                  <ResponsiveContainer width="100%" height={200}>
                    <RechartsPieChart>
                      <Pie
                        data={projectStatusData}
                        cx="50%"
                        cy="50%"
                        innerRadius={40}
                        outerRadius={70}
                        paddingAngle={5}
                        dataKey="value"
                        label={({ name, value }) => `${name}: ${value}`}
                        labelLine={false}
                      >
                        {projectStatusData.map((_, index) => (
                          <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                        ))}
                      </Pie>
                      <Tooltip />
                    </RechartsPieChart>
                  </ResponsiveContainer>
                ) : (
                  <div className="h-48 flex items-center justify-center text-muted-foreground">
                    Nenhum projeto cadastrado
                  </div>
                )}
              </TactileCard>

              <TactileCard className="p-6">
                <h3 className="font-semibold mb-4">Propostas por Status</h3>
                {proposalsLoading ? (
                  <Skeleton className="h-48 w-full" />
                ) : proposalStatusData.length > 0 ? (
                  <ResponsiveContainer width="100%" height={200}>
                    <RechartsPieChart>
                      <Pie
                        data={proposalStatusData}
                        cx="50%"
                        cy="50%"
                        innerRadius={40}
                        outerRadius={70}
                        paddingAngle={5}
                        dataKey="value"
                        label={({ name, value }) => `${name}: ${value}`}
                        labelLine={false}
                      >
                        {proposalStatusData.map((_, index) => (
                          <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                        ))}
                      </Pie>
                      <Tooltip />
                    </RechartsPieChart>
                  </ResponsiveContainer>
                ) : (
                  <div className="h-48 flex items-center justify-center text-muted-foreground">
                    Nenhuma proposta cadastrada
                  </div>
                )}
              </TactileCard>

              <TactileCard className="p-6">
                <h3 className="font-semibold mb-4">Resumo Financeiro</h3>
                <div className="space-y-4">
                  <div className="flex justify-between items-center p-3 bg-green-50 rounded-lg">
                    <span className="text-sm text-green-700">A Receber</span>
                    <span className="font-semibold text-green-700">{formatCurrency(pendingReceivables)}</span>
                  </div>
                  <div className="flex justify-between items-center p-3 bg-red-50 rounded-lg">
                    <span className="text-sm text-red-700">A Pagar</span>
                    <span className="font-semibold text-red-700">{formatCurrency(pendingPayables)}</span>
                  </div>
                  <div className="flex justify-between items-center p-3 bg-blue-50 rounded-lg">
                    <span className="text-sm text-blue-700">Resultado</span>
                    <span className="font-semibold text-blue-700">{formatCurrency(totalIncome - totalExpense)}</span>
                  </div>
                </div>
              </TactileCard>
            </div>
          </TabsContent>

          <TabsContent value="financial" className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
              <MetricCard
                title="Receitas Pagas"
                value={formatCurrency(totalIncome)}
                icon={<TrendingUp className="size-5" />}
                loading={transactionsLoading}
              />
              <MetricCard
                title="Despesas Pagas"
                value={formatCurrency(totalExpense)}
                icon={<TrendingDown className="size-5" />}
                loading={transactionsLoading}
              />
              <MetricCard
                title="A Receber"
                value={formatCurrency(pendingReceivables)}
                icon={<DollarSign className="size-5" />}
                loading={transactionsLoading}
              />
              <MetricCard
                title="A Pagar"
                value={formatCurrency(pendingPayables)}
                icon={<DollarSign className="size-5" />}
                loading={transactionsLoading}
              />
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">
              <TactileCard className="p-6">
                <h3 className="font-semibold mb-4">Receitas vs Despesas</h3>
                {forecastLoading ? (
                  <Skeleton className="h-64 w-full" />
                ) : (
                  <ResponsiveContainer width="100%" height={280}>
                    <BarChart data={filteredForecast}>
                      <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                      <XAxis 
                        dataKey="month" 
                        tickFormatter={(value) => {
                          const [year, month] = value.split('-');
                          return format(new Date(parseInt(year), parseInt(month) - 1), 'MMM', { locale: ptBR });
                        }}
                        className="text-xs"
                      />
                      <YAxis 
                        tickFormatter={(value) => `R$ ${(value / 1000).toFixed(0)}k`}
                        className="text-xs"
                      />
                      <Tooltip content={<CustomTooltip />} />
                      <Legend />
                      <Bar dataKey="income" name="Receitas" fill="#10B981" radius={[4, 4, 0, 0]} />
                      <Bar dataKey="expense" name="Despesas" fill="#EF4444" radius={[4, 4, 0, 0]} />
                    </BarChart>
                  </ResponsiveContainer>
                )}
              </TactileCard>

              <TactileCard className="p-6">
                <h3 className="font-semibold mb-4">Receita por Categoria</h3>
                {transactionsLoading ? (
                  <Skeleton className="h-64 w-full" />
                ) : incomeByCategData.length > 0 ? (
                  <ResponsiveContainer width="100%" height={280}>
                    <RechartsPieChart>
                      <Pie
                        data={incomeByCategData}
                        cx="50%"
                        cy="50%"
                        outerRadius={100}
                        dataKey="value"
                        label={({ name, percent }) => `${name}: ${(percent * 100).toFixed(0)}%`}
                      >
                        {incomeByCategData.map((_, index) => (
                          <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                        ))}
                      </Pie>
                      <Tooltip formatter={(value: number) => formatCurrency(value)} />
                    </RechartsPieChart>
                  </ResponsiveContainer>
                ) : (
                  <div className="h-64 flex items-center justify-center text-muted-foreground">
                    Nenhuma receita no período selecionado
                  </div>
                )}
              </TactileCard>
            </div>
          </TabsContent>

          <TabsContent value="clients" className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
              <MetricCard
                title="Total de Clientes"
                value={clients.length}
                icon={<Users className="size-5" />}
                loading={clientsLoading}
              />
              <MetricCard
                title="Clientes Ativos"
                value={clients.filter((c: any) => c.status === 'ACTIVE').length}
                icon={<Users className="size-5" />}
                loading={clientsLoading}
              />
              <MetricCard
                title="Receita Média/Cliente"
                value={formatCurrency(clients.length > 0 ? totalIncome / clients.length : 0)}
                icon={<DollarSign className="size-5" />}
                loading={clientsLoading || transactionsLoading}
              />
            </div>

            <TactileCard className="p-6">
              <h3 className="font-semibold mb-4">Top 5 Clientes por Receita</h3>
              {clientsLoading || transactionsLoading ? (
                <Skeleton className="h-64 w-full" />
              ) : topClients.length > 0 ? (
                <ResponsiveContainer width="100%" height={280}>
                  <BarChart data={topClients} layout="vertical">
                    <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                    <XAxis 
                      type="number" 
                      tickFormatter={(value) => formatCurrency(value)}
                      className="text-xs"
                    />
                    <YAxis 
                      type="category" 
                      dataKey="name" 
                      width={120}
                      className="text-xs"
                    />
                    <Tooltip formatter={(value: number) => formatCurrency(value)} />
                    <Bar dataKey="totalRevenue" name="Receita Total" fill="#3B82F6" radius={[0, 4, 4, 0]} />
                  </BarChart>
                </ResponsiveContainer>
              ) : (
                <div className="h-64 flex items-center justify-center text-muted-foreground">
                  Nenhum cliente com receita registrada
                </div>
              )}
            </TactileCard>
          </TabsContent>

          <TabsContent value="projects" className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
              <MetricCard
                title="Total de Projetos"
                value={filteredProjects.length}
                icon={<Briefcase className="size-5" />}
                loading={projectsLoading}
              />
              <MetricCard
                title="Projetos Ativos"
                value={filteredProjects.filter((p: any) => p.status === 'ACTIVE').length}
                icon={<Briefcase className="size-5" />}
                loading={projectsLoading}
              />
              <MetricCard
                title="Projetos Concluídos"
                value={filteredProjects.filter((p: any) => p.status === 'COMPLETED').length}
                icon={<Briefcase className="size-5" />}
                loading={projectsLoading}
              />
              <MetricCard
                title="Total de Propostas"
                value={filteredProposals.length}
                icon={<FileText className="size-5" />}
                loading={proposalsLoading}
              />
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">
              <TactileCard className="p-6">
                <h3 className="font-semibold mb-4">Distribuição de Projetos</h3>
                {projectsLoading ? (
                  <Skeleton className="h-64 w-full" />
                ) : projectStatusData.length > 0 ? (
                  <ResponsiveContainer width="100%" height={280}>
                    <BarChart data={projectStatusData}>
                      <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                      <XAxis dataKey="name" className="text-xs" />
                      <YAxis className="text-xs" />
                      <Tooltip />
                      <Bar dataKey="value" name="Quantidade" fill="#3B82F6" radius={[4, 4, 0, 0]}>
                        {projectStatusData.map((_, index) => (
                          <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                        ))}
                      </Bar>
                    </BarChart>
                  </ResponsiveContainer>
                ) : (
                  <div className="h-64 flex items-center justify-center text-muted-foreground">
                    Nenhum projeto cadastrado
                  </div>
                )}
              </TactileCard>

              <TactileCard className="p-6">
                <h3 className="font-semibold mb-4">Taxa de Conversão de Propostas</h3>
                {proposalsLoading ? (
                  <Skeleton className="h-64 w-full" />
                ) : filteredProposals.length > 0 ? (
                  <div className="space-y-6">
                    <div className="flex items-center justify-center">
                      <div className="text-center">
                        <p className="text-5xl font-bold text-primary">
                          {((filteredProposals.filter((p: any) => p.status === 'APPROVED').length / filteredProposals.length) * 100).toFixed(0)}%
                        </p>
                        <p className="text-muted-foreground mt-2">Taxa de Aprovação</p>
                      </div>
                    </div>
                    <div className="grid grid-cols-2 gap-4 text-center">
                      <div className="p-4 bg-green-50 rounded-lg">
                        <p className="text-2xl font-bold text-green-700">
                          {filteredProposals.filter((p: any) => p.status === 'APPROVED').length}
                        </p>
                        <p className="text-sm text-green-600">Aprovadas</p>
                      </div>
                      <div className="p-4 bg-red-50 rounded-lg">
                        <p className="text-2xl font-bold text-red-700">
                          {filteredProposals.filter((p: any) => p.status === 'REJECTED').length}
                        </p>
                        <p className="text-sm text-red-600">Rejeitadas</p>
                      </div>
                      <div className="p-4 bg-yellow-50 rounded-lg">
                        <p className="text-2xl font-bold text-yellow-700">
                          {filteredProposals.filter((p: any) => p.status === 'SENT').length}
                        </p>
                        <p className="text-sm text-yellow-600">Aguardando</p>
                      </div>
                      <div className="p-4 bg-gray-50 rounded-lg">
                        <p className="text-2xl font-bold text-gray-700">
                          {filteredProposals.filter((p: any) => p.status === 'DRAFT').length}
                        </p>
                        <p className="text-sm text-gray-600">Rascunhos</p>
                      </div>
                    </div>
                  </div>
                ) : (
                  <div className="h-64 flex items-center justify-center text-muted-foreground">
                    Nenhuma proposta no período selecionado
                  </div>
                )}
              </TactileCard>
            </div>
          </TabsContent>
        </Tabs>
      </div>
    </AppShell>
  );
}
