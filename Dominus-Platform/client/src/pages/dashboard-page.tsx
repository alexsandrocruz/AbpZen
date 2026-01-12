import { useAuth } from "@/lib/auth-store";
import { useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { 
  ChartContainer, 
  ChartTooltip, 
  ChartTooltipContent,
  ChartLegend,
  ChartLegendContent,
  type ChartConfig 
} from "@/components/ui/chart";
import { 
  LineChart, 
  Line, 
  XAxis, 
  YAxis, 
  CartesianGrid,
  PieChart,
  Pie,
  Cell,
  ResponsiveContainer,
} from "recharts";
import { 
  Users, 
  Briefcase, 
  CheckSquare, 
  FileText, 
  TrendingUp, 
  TrendingDown,
  CalendarClock,
  ArrowUpRight,
  Plus,
  ChevronDown,
  BarChart3,
  DollarSign,
  FileWarning,
} from "lucide-react";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
  DropdownMenuSeparator,
} from "@/components/ui/dropdown-menu";
import { useLocation } from "wouter";
import { format, isBefore, isAfter, addDays, differenceInDays } from "date-fns";
import { ptBR } from "date-fns/locale";

interface DashboardStats {
  totalClients: number;
  activeProjects: number;
  pendingTasks: number;
  openInvoices: number;
  monthlyIncome: number;
  monthlyExpense: number;
  balance: number;
}

interface CashflowData {
  month: string;
  income: number;
  expense: number;
  balance: number;
}

interface CategoryData {
  name: string;
  value: number;
  color: string;
}

interface Transaction {
  id: string;
  description: string;
  amount: string;
  dueDate: string;
  type: string;
  status: string;
}

interface ExpiringDocument {
  id: string;
  name: string;
  clientId: string;
  expirationDate: string;
  status: string;
}

interface Client {
  id: string;
  name: string;
}

const cashflowConfig: ChartConfig = {
  income: {
    label: "Receitas",
    color: "#22c55e",
  },
  expense: {
    label: "Despesas",
    color: "#ef4444",
  },
  balance: {
    label: "Saldo",
    color: "#3b82f6",
  },
};

const monthNames: Record<string, string> = {
  '01': 'Jan',
  '02': 'Fev',
  '03': 'Mar',
  '04': 'Abr',
  '05': 'Mai',
  '06': 'Jun',
  '07': 'Jul',
  '08': 'Ago',
  '09': 'Set',
  '10': 'Out',
  '11': 'Nov',
  '12': 'Dez',
};

function formatMonthLabel(month: string): string {
  const [, m] = month.split('-');
  return monthNames[m] || m;
}

function formatCurrency(value: number): string {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
    minimumFractionDigits: 0,
    maximumFractionDigits: 0,
  }).format(value);
}

export default function DashboardPage() {
  const { user, currentWorkspace } = useAuth();
  const [, navigate] = useLocation();

  const { data: stats, isLoading: statsLoading } = useQuery<DashboardStats>({
    queryKey: ['dashboard-stats', currentWorkspace?.id],
    queryFn: () => api.getDashboardStats(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: cashflowData = [], isLoading: cashflowLoading } = useQuery<CashflowData[]>({
    queryKey: ['dashboard-cashflow', currentWorkspace?.id],
    queryFn: () => api.getCashflowData(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: categoryData = [], isLoading: categoryLoading } = useQuery<CategoryData[]>({
    queryKey: ['dashboard-income-by-category', currentWorkspace?.id],
    queryFn: () => api.getIncomeByCategory(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: transactions = [], isLoading: transactionsLoading } = useQuery<Transaction[]>({
    queryKey: ['transactions', currentWorkspace?.id],
    queryFn: () => api.getTransactions(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: forecastData = [], isLoading: forecastLoading } = useQuery<CashflowData[]>({
    queryKey: ['dashboard-cashflow-forecast', currentWorkspace?.id],
    queryFn: () => api.getCashflowForecast(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: expiringDocuments = [], isLoading: expiringDocsLoading } = useQuery<ExpiringDocument[]>({
    queryKey: ['expiring-documents', currentWorkspace?.id],
    queryFn: () => api.getExpiringDocuments(currentWorkspace!.id, 30),
    enabled: !!currentWorkspace,
    refetchInterval: 5 * 60 * 1000,
  });

  const { data: clients = [] } = useQuery<Client[]>({
    queryKey: ['clients', currentWorkspace?.id],
    queryFn: () => api.getClients(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const clientsMap = clients.reduce((acc, client) => {
    acc[client.id] = client.name;
    return acc;
  }, {} as Record<string, string>);

  const upcomingDueDates = transactions
    .filter((t) => {
      const dueDate = new Date(t.dueDate);
      const now = new Date();
      const inTwoWeeks = addDays(now, 14);
      return (t.status === 'PENDING' || t.status === 'OVERDUE') && 
             isAfter(dueDate, addDays(now, -1)) && 
             isBefore(dueDate, inTwoWeeks);
    })
    .sort((a, b) => new Date(a.dueDate).getTime() - new Date(b.dueDate).getTime())
    .slice(0, 5);

  const isLoading = statsLoading || cashflowLoading || categoryLoading || transactionsLoading || forecastLoading;

  const chartData = cashflowData.map(d => ({
    ...d,
    monthLabel: formatMonthLabel(d.month),
  }));

  const forecastChartData = forecastData.map(d => ({
    ...d,
    monthLabel: formatMonthLabel(d.month),
  }));

  const totalCategoryValue = categoryData.reduce((sum, cat) => sum + cat.value, 0);

  return (
    <AppShell>
      <div className="space-y-8">
        <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight" data-testid="dashboard-title">Painel</h1>
            <p className="text-muted-foreground mt-1">
              Bem-vindo de volta, {user?.name.split(' ')[0]}. Veja o resumo financeiro do seu negócio.
            </p>
          </div>
          <div className="flex items-center gap-3">
            <Button 
              variant="outline" 
              onClick={() => navigate(`/${currentWorkspace?.slug}/reports`)}
              data-testid="button-view-reports"
            >
              <BarChart3 className="size-4 mr-2" />
              Ver Relatórios
            </Button>
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button data-testid="button-create-new">
                  <Plus className="size-4 mr-2" />
                  Criar Novo
                  <ChevronDown className="size-4 ml-2" />
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end" className="w-56">
                <DropdownMenuItem 
                  onClick={() => navigate(`/${currentWorkspace?.slug}/proposals/new`)}
                  data-testid="menu-new-proposal"
                >
                  <FileText className="size-4 mr-2" />
                  Nova Proposta
                </DropdownMenuItem>
                <DropdownMenuItem 
                  onClick={() => navigate(`/${currentWorkspace?.slug}/clients`)}
                  data-testid="menu-new-client"
                >
                  <Users className="size-4 mr-2" />
                  Novo Cliente
                </DropdownMenuItem>
                <DropdownMenuItem 
                  onClick={() => navigate(`/${currentWorkspace?.slug}/projects`)}
                  data-testid="menu-new-project"
                >
                  <Briefcase className="size-4 mr-2" />
                  Novo Projeto
                </DropdownMenuItem>
                <DropdownMenuItem 
                  onClick={() => navigate(`/${currentWorkspace?.slug}/tasks`)}
                  data-testid="menu-new-task"
                >
                  <CheckSquare className="size-4 mr-2" />
                  Nova Tarefa
                </DropdownMenuItem>
                <DropdownMenuSeparator />
                <DropdownMenuItem 
                  onClick={() => navigate(`/${currentWorkspace?.slug}/financeiro`)}
                  data-testid="menu-new-transaction"
                >
                  <DollarSign className="size-4 mr-2" />
                  Nova Despesa/Receita
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          </div>
        </div>

        <div className="grid gap-4 grid-cols-2 lg:grid-cols-3 xl:grid-cols-6">
          {isLoading ? (
            <>
              {[...Array(6)].map((_, i) => (
                <Skeleton key={i} className="h-28 rounded-xl" />
              ))}
            </>
          ) : (
            <>
              <MetricCard 
                title="Total de Clientes" 
                value={stats?.totalClients?.toString() || '0'} 
                icon={Users}
                testId="metric-total-clients"
              />
              <MetricCard 
                title="Projetos Ativos" 
                value={stats?.activeProjects?.toString() || '0'} 
                icon={Briefcase}
                testId="metric-active-projects"
              />
              <MetricCard 
                title="Tarefas Pendentes" 
                value={stats?.pendingTasks?.toString() || '0'} 
                icon={CheckSquare}
                alert={stats?.pendingTasks ? stats.pendingTasks > 0 : false}
                testId="metric-pending-tasks"
              />
              <MetricCard 
                title="Faturas em Aberto" 
                value={stats?.openInvoices?.toString() || '0'} 
                icon={FileText}
                alert={stats?.openInvoices ? stats.openInvoices > 0 : false}
                testId="metric-open-invoices"
              />
              <MetricCard 
                title="Receita do Mês" 
                value={formatCurrency(stats?.monthlyIncome || 0)} 
                icon={TrendingUp}
                positive
                testId="metric-monthly-income"
              />
              <MetricCard 
                title="Despesa do Mês" 
                value={formatCurrency(stats?.monthlyExpense || 0)} 
                icon={TrendingDown}
                negative
                testId="metric-monthly-expense"
              />
            </>
          )}
        </div>

        <div className="grid gap-6 lg:grid-cols-2">
          <TactileCard className="p-6 min-w-0 overflow-hidden" data-testid="chart-cashflow">
            <div className="flex items-center justify-between mb-4">
              <h2 className="text-lg font-semibold">Fluxo de Caixa (Últimos 12 meses)</h2>
            </div>
            {cashflowLoading ? (
              <Skeleton className="h-[300px] w-full" />
            ) : chartData.length > 0 ? (
              <ChartContainer config={cashflowConfig} className="h-[300px] w-full min-w-0">
                <LineChart data={chartData} margin={{ top: 5, right: 10, left: 10, bottom: 0 }}>
                  <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                  <XAxis 
                    dataKey="monthLabel" 
                    tickLine={false}
                    axisLine={false}
                    tick={{ fontSize: 12 }}
                  />
                  <YAxis 
                    tickFormatter={(value) => formatCurrency(value)}
                    tickLine={false}
                    axisLine={false}
                    tick={{ fontSize: 11 }}
                    width={80}
                  />
                  <ChartTooltip 
                    content={
                      <ChartTooltipContent 
                        formatter={(value) => (
                          <span className="font-medium">
                            {formatCurrency(Number(value))}
                          </span>
                        )}
                      />
                    } 
                  />
                  <ChartLegend content={<ChartLegendContent />} />
                  <Line 
                    type="monotone" 
                    dataKey="income" 
                    stroke="var(--color-income)" 
                    strokeWidth={2}
                    dot={false}
                  />
                  <Line 
                    type="monotone" 
                    dataKey="expense" 
                    stroke="var(--color-expense)" 
                    strokeWidth={2}
                    dot={false}
                  />
                  <Line 
                    type="monotone" 
                    dataKey="balance" 
                    stroke="var(--color-balance)" 
                    strokeWidth={2}
                    dot={false}
                  />
                </LineChart>
              </ChartContainer>
            ) : (
              <div className="h-[300px] flex items-center justify-center text-muted-foreground border border-dashed rounded-lg">
                Nenhum dado de fluxo de caixa disponível
              </div>
            )}
          </TactileCard>

          <TactileCard className="p-6 min-w-0 overflow-hidden" data-testid="chart-income-by-category">
            <div className="flex items-center justify-between mb-4">
              <h2 className="text-lg font-semibold">Receitas por Categoria</h2>
            </div>
            {categoryLoading ? (
              <Skeleton className="h-[300px] w-full" />
            ) : categoryData.length > 0 ? (
              <div className="h-[300px] flex flex-col min-w-0">
                <div className="flex-1 min-w-0">
                  <ResponsiveContainer width="100%" height="100%">
                    <PieChart>
                      <Pie
                        data={categoryData}
                        cx="50%"
                        cy="50%"
                        innerRadius={60}
                        outerRadius={100}
                        paddingAngle={2}
                        dataKey="value"
                        nameKey="name"
                      >
                        {categoryData.map((entry, index) => (
                          <Cell key={`cell-${index}`} fill={entry.color} />
                        ))}
                      </Pie>
                      <ChartTooltip 
                        content={({ active, payload }) => {
                          if (active && payload && payload.length) {
                            const data = payload[0].payload as CategoryData;
                            const percentage = ((data.value / totalCategoryValue) * 100).toFixed(1);
                            return (
                              <div className="bg-background border rounded-lg shadow-lg p-3">
                                <p className="font-medium">{data.name}</p>
                                <p className="text-sm text-muted-foreground">
                                  {formatCurrency(data.value)} ({percentage}%)
                                </p>
                              </div>
                            );
                          }
                          return null;
                        }}
                      />
                    </PieChart>
                  </ResponsiveContainer>
                </div>
                <div className="flex flex-wrap gap-3 justify-center pt-2">
                  {categoryData.slice(0, 5).map((cat, i) => (
                    <div key={i} className="flex items-center gap-2 text-sm">
                      <div 
                        className="w-3 h-3 rounded-sm" 
                        style={{ backgroundColor: cat.color }}
                      />
                      <span className="text-muted-foreground">{cat.name}</span>
                    </div>
                  ))}
                </div>
              </div>
            ) : (
              <div className="h-[300px] flex items-center justify-center text-muted-foreground border border-dashed rounded-lg">
                Nenhuma receita categorizada disponível
              </div>
            )}
          </TactileCard>
        </div>

        <TactileCard className="p-6 min-w-0 overflow-hidden" data-testid="chart-cashflow-forecast">
          <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-2 mb-4">
            <h2 className="text-lg font-semibold">Previsão de Fluxo de Caixa (Próximos 12 meses)</h2>
            <span className="text-xs text-muted-foreground bg-muted px-2 py-1 rounded whitespace-nowrap">Baseado em contas a pagar/receber</span>
          </div>
          {forecastLoading ? (
            <Skeleton className="h-[250px] w-full" />
          ) : forecastChartData.some(d => d.income > 0 || d.expense > 0) ? (
            <ChartContainer config={cashflowConfig} className="h-[250px] w-full min-w-0">
              <LineChart data={forecastChartData} margin={{ top: 5, right: 10, left: 10, bottom: 0 }}>
                <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                <XAxis 
                  dataKey="monthLabel" 
                  tickLine={false}
                  axisLine={false}
                  tick={{ fontSize: 12 }}
                />
                <YAxis 
                  tickFormatter={(value) => formatCurrency(value)}
                  tickLine={false}
                  axisLine={false}
                  tick={{ fontSize: 11 }}
                  width={80}
                />
                <ChartTooltip 
                  content={
                    <ChartTooltipContent 
                      formatter={(value) => (
                        <span className="font-medium">
                          {formatCurrency(Number(value))}
                        </span>
                      )}
                    />
                  } 
                />
                <ChartLegend content={<ChartLegendContent />} />
                <Line 
                  type="monotone" 
                  dataKey="income" 
                  stroke="var(--color-income)" 
                  strokeWidth={2}
                  dot={false}
                  name="A Receber"
                />
                <Line 
                  type="monotone" 
                  dataKey="expense" 
                  stroke="var(--color-expense)" 
                  strokeWidth={2}
                  dot={false}
                  name="A Pagar"
                />
                <Line 
                  type="monotone" 
                  dataKey="balance" 
                  stroke="var(--color-balance)" 
                  strokeWidth={2}
                  dot={false}
                  name="Saldo Acumulado"
                />
              </LineChart>
            </ChartContainer>
          ) : (
            <div className="h-[250px] flex items-center justify-center text-muted-foreground border border-dashed rounded-lg">
              Nenhuma previsão disponível. Adicione transações a pagar/receber para visualizar.
            </div>
          )}
        </TactileCard>

        <div className="grid gap-6 lg:grid-cols-3">
          <TactileCard className="p-6" data-testid="section-expiring-documents">
            <div className="flex items-center justify-between mb-4">
              <h2 className="text-lg font-semibold flex items-center gap-2">
                <FileWarning className="size-5" />
                Documentos Vencendo
              </h2>
              <Button 
                variant="ghost" 
                size="sm" 
                className="text-muted-foreground"
                onClick={() => navigate(`/${currentWorkspace?.slug}/clients`)}
                data-testid="button-view-all-expiring-docs"
              >
                Ver todos
              </Button>
            </div>
            {expiringDocsLoading ? (
              <div className="space-y-3">
                {[...Array(3)].map((_, i) => (
                  <Skeleton key={i} className="h-14 rounded-lg" />
                ))}
              </div>
            ) : expiringDocuments.length > 0 ? (
              <div className="space-y-3 max-h-[300px] overflow-y-auto">
                {expiringDocuments.map((doc) => {
                  const expDate = new Date(doc.expirationDate);
                  const daysRemaining = differenceInDays(expDate, new Date());
                  const clientName = clientsMap[doc.clientId] || 'Cliente desconhecido';
                  
                  let colorClass = 'text-yellow-600 dark:text-yellow-400 bg-yellow-500/10 border-yellow-500/20';
                  if (daysRemaining < 7) {
                    colorClass = 'text-red-600 dark:text-red-400 bg-red-500/10 border-red-500/20';
                  } else if (daysRemaining < 30) {
                    colorClass = 'text-orange-600 dark:text-orange-400 bg-orange-500/10 border-orange-500/20';
                  }
                  
                  return (
                    <div 
                      key={doc.id} 
                      className={`flex items-center justify-between p-3 rounded-lg border ${colorClass}`}
                      data-testid={`expiring-doc-${doc.id}`}
                    >
                      <div className="flex-1 min-w-0">
                        <p className="font-medium truncate">{doc.name}</p>
                        <p className="text-xs opacity-80 truncate">{clientName}</p>
                      </div>
                      <div className="text-right shrink-0 ml-2">
                        <p className="text-sm font-semibold">
                          {daysRemaining === 0 ? 'Hoje' : daysRemaining === 1 ? '1 dia' : `${daysRemaining} dias`}
                        </p>
                        <p className="text-xs opacity-80">
                          {format(expDate, "dd/MM/yyyy")}
                        </p>
                      </div>
                    </div>
                  );
                })}
              </div>
            ) : (
              <div className="p-8 text-center text-muted-foreground border border-dashed rounded-lg">
                Nenhum documento vencendo em breve
              </div>
            )}
          </TactileCard>

          <TactileCard className="p-6" data-testid="section-upcoming-due-dates">
            <div className="flex items-center justify-between mb-4">
              <h2 className="text-lg font-semibold flex items-center gap-2">
                <CalendarClock className="size-5" />
                Próximos Vencimentos
              </h2>
              <Button variant="ghost" size="sm" className="text-muted-foreground" data-testid="button-view-all-transactions">
                Ver Todas
              </Button>
            </div>
            {transactionsLoading ? (
              <div className="space-y-3">
                {[...Array(3)].map((_, i) => (
                  <Skeleton key={i} className="h-14 rounded-lg" />
                ))}
              </div>
            ) : upcomingDueDates.length > 0 ? (
              <div className="space-y-3">
                {upcomingDueDates.map((transaction) => {
                  const dueDate = new Date(transaction.dueDate);
                  const isOverdue = isBefore(dueDate, new Date());
                  const isExpense = transaction.type === 'EXPENSE';
                  
                  return (
                    <div 
                      key={transaction.id} 
                      className="flex items-center justify-between p-3 rounded-lg border bg-card hover:bg-accent/50 transition-colors"
                      data-testid={`transaction-due-${transaction.id}`}
                    >
                      <div className="flex-1 min-w-0">
                        <p className="font-medium truncate">{transaction.description}</p>
                        <p className={`text-xs ${isOverdue ? 'text-destructive' : 'text-muted-foreground'}`}>
                          {isOverdue ? 'Vencido em ' : 'Vence em '}
                          {format(dueDate, "d 'de' MMMM", { locale: ptBR })}
                        </p>
                      </div>
                      <div className="text-right">
                        <p className={`font-semibold ${isExpense ? 'text-red-500' : 'text-green-500'}`}>
                          {isExpense ? '-' : '+'}{formatCurrency(parseFloat(transaction.amount))}
                        </p>
                        <p className="text-xs text-muted-foreground">
                          {isExpense ? 'A pagar' : 'A receber'}
                        </p>
                      </div>
                    </div>
                  );
                })}
              </div>
            ) : (
              <div className="p-8 text-center text-muted-foreground border border-dashed rounded-lg">
                Nenhum vencimento próximo. Tudo em dia!
              </div>
            )}
          </TactileCard>

          <TactileCard className="p-6" data-testid="section-financial-summary">
            <div className="flex items-center justify-between mb-4">
              <h2 className="text-lg font-semibold">Resumo do Mês</h2>
            </div>
            {statsLoading ? (
              <div className="space-y-4">
                <Skeleton className="h-20 rounded-lg" />
                <Skeleton className="h-20 rounded-lg" />
                <Skeleton className="h-20 rounded-lg" />
              </div>
            ) : (
              <div className="space-y-4">
                <div className="p-4 rounded-lg bg-green-500/10 border border-green-500/20">
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-green-600 dark:text-green-400">Receitas</span>
                    <TrendingUp className="size-4 text-green-500" />
                  </div>
                  <p className="text-2xl font-bold text-green-600 dark:text-green-400 mt-1">
                    {formatCurrency(stats?.monthlyIncome || 0)}
                  </p>
                </div>
                
                <div className="p-4 rounded-lg bg-red-500/10 border border-red-500/20">
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-red-600 dark:text-red-400">Despesas</span>
                    <TrendingDown className="size-4 text-red-500" />
                  </div>
                  <p className="text-2xl font-bold text-red-600 dark:text-red-400 mt-1">
                    {formatCurrency(stats?.monthlyExpense || 0)}
                  </p>
                </div>
                
                <div className={`p-4 rounded-lg ${
                  (stats?.balance || 0) >= 0 
                    ? 'bg-blue-500/10 border border-blue-500/20' 
                    : 'bg-orange-500/10 border border-orange-500/20'
                }`}>
                  <div className="flex items-center justify-between">
                    <span className={`text-sm ${
                      (stats?.balance || 0) >= 0 
                        ? 'text-blue-600 dark:text-blue-400' 
                        : 'text-orange-600 dark:text-orange-400'
                    }`}>Saldo do Mês</span>
                    <ArrowUpRight className={`size-4 ${
                      (stats?.balance || 0) >= 0 ? 'text-blue-500' : 'text-orange-500 rotate-90'
                    }`} />
                  </div>
                  <p className={`text-2xl font-bold mt-1 ${
                    (stats?.balance || 0) >= 0 
                      ? 'text-blue-600 dark:text-blue-400' 
                      : 'text-orange-600 dark:text-orange-400'
                  }`}>
                    {formatCurrency(stats?.balance || 0)}
                  </p>
                </div>
              </div>
            )}
          </TactileCard>
        </div>
      </div>
    </AppShell>
  );
}

function MetricCard({ 
  title, 
  value, 
  icon: Icon, 
  alert,
  positive,
  negative,
  testId,
}: { 
  title: string; 
  value: string; 
  icon: any; 
  alert?: boolean;
  positive?: boolean;
  negative?: boolean;
  testId?: string;
}) {
  return (
    <TactileCard className="p-4 space-y-2" data-testid={testId}>
      <div className="flex items-center justify-between text-muted-foreground">
        <span className="text-xs font-medium">{title}</span>
        <Icon className={`size-4 ${
          positive ? 'text-green-500' : 
          negative ? 'text-red-500' : 
          alert ? 'text-amber-500' : ''
        }`} />
      </div>
      <div className={`text-xl font-bold tracking-tight ${
        positive ? 'text-green-600 dark:text-green-400' : 
        negative ? 'text-red-600 dark:text-red-400' : ''
      }`}>
        {value}
      </div>
    </TactileCard>
  );
}
