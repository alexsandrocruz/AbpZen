import { useState } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { Badge } from "@/components/ui/badge";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Calendar } from "@/components/ui/calendar";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { ClientCombobox } from "@/components/ui/client-combobox";
import { CategoryCombobox } from "@/components/ui/category-combobox";
import { 
  Plus, 
  MoreHorizontal, 
  Pencil, 
  Trash2, 
  CheckCircle2,
  Filter,
  TrendingUp,
  TrendingDown,
  Wallet,
  CalendarIcon,
  Loader2,
  Repeat
} from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import { DeleteModal } from "@/components/modals/delete-modal";
import { RecurringTransactionModal } from "@/components/modals/recurring-transaction-modal";
import { useToast } from "@/hooks/use-toast";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";
import {
  Tabs,
  TabsContent,
  TabsList,
  TabsTrigger,
} from "@/components/ui/tabs";
import { cn } from "@/lib/utils";

const statusMap: Record<string, { label: string; color: string }> = {
  'PENDING': { label: 'Pendente', color: 'bg-yellow-50 text-yellow-700 border-yellow-200' },
  'RECEIVED': { label: 'Recebido', color: 'bg-green-50 text-green-700 border-green-200' },
  'PAID': { label: 'Pago', color: 'bg-blue-50 text-blue-700 border-blue-200' },
  'OVERDUE': { label: 'Atrasado', color: 'bg-red-50 text-red-700 border-red-200' },
  'CANCELLED': { label: 'Cancelado', color: 'bg-gray-50 text-gray-700 border-gray-200' },
};

interface TransactionFormData {
  type: 'INCOME' | 'EXPENSE';
  description: string;
  amount: string;
  dueDate: Date | null;
  paymentDate: Date | null;
  clientId: string;
  projectId: string;
  categoryId: string;
  status: string;
  supplier: string;
  notes: string;
}

const initialFormData: TransactionFormData = {
  type: 'INCOME',
  description: '',
  amount: '',
  dueDate: null,
  paymentDate: null,
  clientId: '',
  projectId: '',
  categoryId: '',
  status: 'PENDING',
  supplier: '',
  notes: '',
};

export default function TransactionsPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [activeTab, setActiveTab] = useState<'INCOME' | 'EXPENSE'>('INCOME');
  const [modalOpen, setModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [recurringModalOpen, setRecurringModalOpen] = useState(false);
  const [selectedTransaction, setSelectedTransaction] = useState<any>(null);
  const [formData, setFormData] = useState<TransactionFormData>(initialFormData);
  
  const [filterStatus, setFilterStatus] = useState<string>('all');
  const [filterCategory, setFilterCategory] = useState<string>('all');

  const { data: transactions = [], isLoading } = useQuery({
    queryKey: ['transactions', currentWorkspace?.id],
    queryFn: () => api.getTransactions(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: clients = [] } = useQuery({
    queryKey: ['clients', currentWorkspace?.id],
    queryFn: () => api.getClients(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: projects = [] } = useQuery({
    queryKey: ['projects', currentWorkspace?.id],
    queryFn: () => api.getProjects(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: categories = [] } = useQuery({
    queryKey: ['financial-categories', currentWorkspace?.id],
    queryFn: () => api.getFinancialCategories(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const createMutation = useMutation({
    mutationFn: (data: any) => api.createTransaction(currentWorkspace!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['transactions', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Transação criada com sucesso!" });
      handleCloseModal();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: any }) => 
      api.updateTransaction(currentWorkspace!.id, id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['transactions', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Transação atualizada com sucesso!" });
      handleCloseModal();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: (transactionId: string) => api.deleteTransaction(currentWorkspace!.id, transactionId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['transactions', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Transação excluída com sucesso!" });
      setDeleteModalOpen(false);
      setSelectedTransaction(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const markAsPaidMutation = useMutation({
    mutationFn: ({ id, status }: { id: string; status: string }) => 
      api.updateTransaction(currentWorkspace!.id, id, { 
        status, 
        paymentDate: new Date().toISOString() 
      }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['transactions', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Status atualizado com sucesso!" });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const getClientName = (id: string) => {
    const client = clients.find((c: any) => c.id === id);
    return client?.companyName || client?.name || '';
  };

  const getCategoryName = (id: string) => {
    const category = categories.find((c: any) => c.id === id);
    return category?.name || '';
  };

  const filteredTransactions = transactions.filter((t: any) => {
    if (t.type !== activeTab) return false;
    if (filterStatus !== 'all' && t.status !== filterStatus) return false;
    if (filterCategory !== 'all' && t.categoryId !== filterCategory) return false;
    return true;
  });

  const incomeTransactions = transactions.filter((t: any) => t.type === 'INCOME');
  const expenseTransactions = transactions.filter((t: any) => t.type === 'EXPENSE');

  const totalIncome = incomeTransactions
    .filter((t: any) => t.status === 'RECEIVED' || t.status === 'PENDING')
    .reduce((sum: number, t: any) => sum + parseFloat(t.amount || '0'), 0);
  
  const totalExpense = expenseTransactions
    .filter((t: any) => t.status === 'PAID' || t.status === 'PENDING')
    .reduce((sum: number, t: any) => sum + parseFloat(t.amount || '0'), 0);
  
  const balance = totalIncome - totalExpense;

  const handleNewTransaction = () => {
    setSelectedTransaction(null);
    setFormData({ ...initialFormData, type: activeTab });
    setModalOpen(true);
  };

  const handleEdit = (transaction: any) => {
    setSelectedTransaction(transaction);
    setFormData({
      type: transaction.type,
      description: transaction.description || '',
      amount: transaction.amount || '',
      dueDate: transaction.dueDate ? new Date(transaction.dueDate) : null,
      paymentDate: transaction.paymentDate ? new Date(transaction.paymentDate) : null,
      clientId: transaction.clientId || '',
      projectId: transaction.projectId || '',
      categoryId: transaction.categoryId || '',
      status: transaction.status || 'PENDING',
      supplier: transaction.supplier || '',
      notes: transaction.notes || '',
    });
    setModalOpen(true);
  };

  const handleDelete = (transaction: any) => {
    setSelectedTransaction(transaction);
    setDeleteModalOpen(true);
  };

  const handleCloseModal = () => {
    setModalOpen(false);
    setSelectedTransaction(null);
    setFormData(initialFormData);
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.description || !formData.amount || !formData.dueDate) {
      toast({ title: "Erro", description: "Preencha os campos obrigatórios", variant: "destructive" });
      return;
    }

    const data = {
      type: formData.type,
      description: formData.description,
      amount: formData.amount,
      dueDate: formData.dueDate.toISOString(),
      paymentDate: formData.paymentDate?.toISOString() || null,
      clientId: formData.clientId || null,
      projectId: formData.projectId || null,
      categoryId: formData.categoryId || null,
      status: formData.status,
      supplier: formData.supplier || null,
      notes: formData.notes || null,
    };

    if (selectedTransaction) {
      updateMutation.mutate({ id: selectedTransaction.id, data });
    } else {
      createMutation.mutate(data);
    }
  };

  const handleMarkAsPaid = (transaction: any) => {
    const newStatus = transaction.type === 'INCOME' ? 'RECEIVED' : 'PAID';
    markAsPaidMutation.mutate({ id: transaction.id, status: newStatus });
  };

  const isSubmitting = createMutation.isPending || updateMutation.isPending;

  const filteredCategories = categories.filter((c: any) => c.type === activeTab);

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight">Financeiro</h1>
            <p className="text-muted-foreground">Gerencie contas a pagar e receber.</p>
          </div>
          <div className="flex gap-2">
            <Button variant="outline" className="gap-2" onClick={() => setRecurringModalOpen(true)} data-testid="button-recurring-transaction">
              <Repeat className="size-4" />
              Recorrência
            </Button>
            <Button className="gap-2" onClick={handleNewTransaction} data-testid="button-add-transaction">
              <Plus className="size-4" />
              Nova Transação
            </Button>
          </div>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <TactileCard className="p-4">
            <div className="flex items-center gap-3">
              <div className="p-2 rounded-lg bg-green-100">
                <TrendingUp className="size-5 text-green-600" />
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Total a Receber</p>
                <p className="text-xl font-bold text-green-600" data-testid="text-total-income">
                  R$ {totalIncome.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                </p>
              </div>
            </div>
          </TactileCard>
          
          <TactileCard className="p-4">
            <div className="flex items-center gap-3">
              <div className="p-2 rounded-lg bg-red-100">
                <TrendingDown className="size-5 text-red-600" />
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Total a Pagar</p>
                <p className="text-xl font-bold text-red-600" data-testid="text-total-expense">
                  R$ {totalExpense.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                </p>
              </div>
            </div>
          </TactileCard>
          
          <TactileCard className="p-4">
            <div className="flex items-center gap-3">
              <div className={cn("p-2 rounded-lg", balance >= 0 ? "bg-blue-100" : "bg-orange-100")}>
                <Wallet className={cn("size-5", balance >= 0 ? "text-blue-600" : "text-orange-600")} />
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Saldo</p>
                <p className={cn("text-xl font-bold", balance >= 0 ? "text-blue-600" : "text-orange-600")} data-testid="text-balance">
                  R$ {balance.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                </p>
              </div>
            </div>
          </TactileCard>
        </div>

        <Tabs value={activeTab} onValueChange={(v) => setActiveTab(v as 'INCOME' | 'EXPENSE')}>
          <div className="flex items-center justify-between gap-4 flex-wrap">
            <TabsList>
              <TabsTrigger value="INCOME" className="gap-2" data-testid="tab-income">
                <TrendingUp className="size-4" />
                Contas a Receber
                <Badge variant="secondary" className="ml-1">{incomeTransactions.length}</Badge>
              </TabsTrigger>
              <TabsTrigger value="EXPENSE" className="gap-2" data-testid="tab-expense">
                <TrendingDown className="size-4" />
                Contas a Pagar
                <Badge variant="secondary" className="ml-1">{expenseTransactions.length}</Badge>
              </TabsTrigger>
            </TabsList>

            <div className="flex items-center gap-2">
              <Filter className="size-4 text-muted-foreground" />
              <Select value={filterStatus} onValueChange={setFilterStatus}>
                <SelectTrigger className="w-[150px]" data-testid="filter-status">
                  <SelectValue placeholder="Status" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">Todos</SelectItem>
                  <SelectItem value="PENDING">Pendente</SelectItem>
                  {activeTab === 'INCOME' && <SelectItem value="RECEIVED">Recebido</SelectItem>}
                  {activeTab === 'EXPENSE' && <SelectItem value="PAID">Pago</SelectItem>}
                  <SelectItem value="OVERDUE">Atrasado</SelectItem>
                  <SelectItem value="CANCELLED">Cancelado</SelectItem>
                </SelectContent>
              </Select>

              <Select value={filterCategory} onValueChange={setFilterCategory}>
                <SelectTrigger className="w-[180px]" data-testid="filter-category">
                  <SelectValue placeholder="Categoria" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">Todas Categorias</SelectItem>
                  {filteredCategories.map((cat: any) => (
                    <SelectItem key={cat.id} value={cat.id}>{cat.name}</SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          </div>

          <TabsContent value="INCOME" className="mt-4">
            <TransactionTable
              transactions={filteredTransactions}
              isLoading={isLoading}
              type="INCOME"
              onEdit={handleEdit}
              onDelete={handleDelete}
              onMarkAsPaid={handleMarkAsPaid}
              getClientName={getClientName}
              getCategoryName={getCategoryName}
            />
          </TabsContent>
          
          <TabsContent value="EXPENSE" className="mt-4">
            <TransactionTable
              transactions={filteredTransactions}
              isLoading={isLoading}
              type="EXPENSE"
              onEdit={handleEdit}
              onDelete={handleDelete}
              onMarkAsPaid={handleMarkAsPaid}
              getClientName={getClientName}
              getCategoryName={getCategoryName}
            />
          </TabsContent>
        </Tabs>
      </div>

      <Dialog open={modalOpen} onOpenChange={setModalOpen}>
        <DialogContent className="sm:max-w-[600px]">
          <DialogHeader>
            <DialogTitle>
              {selectedTransaction ? 'Editar Transação' : 'Nova Transação'}
            </DialogTitle>
          </DialogHeader>
          
          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label>Tipo *</Label>
                <Select 
                  value={formData.type} 
                  onValueChange={(v) => setFormData({ ...formData, type: v as 'INCOME' | 'EXPENSE' })}
                >
                  <SelectTrigger data-testid="input-type">
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="INCOME">Conta a Receber</SelectItem>
                    <SelectItem value="EXPENSE">Conta a Pagar</SelectItem>
                  </SelectContent>
                </Select>
              </div>

              <div className="space-y-2">
                <Label>Status *</Label>
                <Select 
                  value={formData.status} 
                  onValueChange={(v) => setFormData({ ...formData, status: v })}
                >
                  <SelectTrigger data-testid="input-status">
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="PENDING">Pendente</SelectItem>
                    {formData.type === 'INCOME' && <SelectItem value="RECEIVED">Recebido</SelectItem>}
                    {formData.type === 'EXPENSE' && <SelectItem value="PAID">Pago</SelectItem>}
                    <SelectItem value="OVERDUE">Atrasado</SelectItem>
                    <SelectItem value="CANCELLED">Cancelado</SelectItem>
                  </SelectContent>
                </Select>
              </div>
            </div>

            <div className="space-y-2">
              <Label>Descrição *</Label>
              <Input
                value={formData.description}
                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                placeholder="Descrição da transação"
                data-testid="input-description"
              />
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label>Valor *</Label>
                <Input
                  type="number"
                  step="0.01"
                  value={formData.amount}
                  onChange={(e) => setFormData({ ...formData, amount: e.target.value })}
                  placeholder="0,00"
                  data-testid="input-amount"
                />
              </div>

              <div className="space-y-2">
                <Label>Data de Vencimento *</Label>
                <Popover>
                  <PopoverTrigger asChild>
                    <Button
                      variant="outline"
                      className={cn(
                        "w-full justify-start text-left font-normal",
                        !formData.dueDate && "text-muted-foreground"
                      )}
                      data-testid="input-due-date"
                    >
                      <CalendarIcon className="mr-2 h-4 w-4" />
                      {formData.dueDate ? format(formData.dueDate, "dd/MM/yyyy") : "Selecionar"}
                    </Button>
                  </PopoverTrigger>
                  <PopoverContent className="w-auto p-0" align="start">
                    <Calendar
                      mode="single"
                      selected={formData.dueDate || undefined}
                      onSelect={(date) => setFormData({ ...formData, dueDate: date || null })}
                      locale={ptBR}
                    />
                  </PopoverContent>
                </Popover>
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label>Data de Pagamento</Label>
                <Popover>
                  <PopoverTrigger asChild>
                    <Button
                      variant="outline"
                      className={cn(
                        "w-full justify-start text-left font-normal",
                        !formData.paymentDate && "text-muted-foreground"
                      )}
                      data-testid="input-payment-date"
                    >
                      <CalendarIcon className="mr-2 h-4 w-4" />
                      {formData.paymentDate ? format(formData.paymentDate, "dd/MM/yyyy") : "Selecionar"}
                    </Button>
                  </PopoverTrigger>
                  <PopoverContent className="w-auto p-0" align="start">
                    <Calendar
                      mode="single"
                      selected={formData.paymentDate || undefined}
                      onSelect={(date) => setFormData({ ...formData, paymentDate: date || null })}
                      locale={ptBR}
                    />
                  </PopoverContent>
                </Popover>
              </div>

              <div className="space-y-2">
                <Label>Categoria</Label>
                <CategoryCombobox
                  categories={categories}
                  value={formData.categoryId}
                  onChange={(v) => setFormData({ ...formData, categoryId: v })}
                  workspaceId={currentWorkspace?.id || ""}
                  type={formData.type}
                  placeholder="Buscar ou criar categoria..."
                />
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label>{formData.type === 'INCOME' ? 'Cliente' : 'Fornecedor'}</Label>
                {formData.type === 'INCOME' ? (
                  <ClientCombobox
                    clients={clients}
                    value={formData.clientId}
                    onChange={(v) => setFormData({ ...formData, clientId: v })}
                    workspaceId={currentWorkspace?.id || ""}
                    placeholder="Buscar ou criar cliente..."
                  />
                ) : (
                  <Input
                    value={formData.supplier}
                    onChange={(e) => setFormData({ ...formData, supplier: e.target.value })}
                    placeholder="Nome do fornecedor"
                    data-testid="input-supplier"
                  />
                )}
              </div>

              <div className="space-y-2">
                <Label>Projeto</Label>
                <Select 
                  value={formData.projectId} 
                  onValueChange={(v) => setFormData({ ...formData, projectId: v })}
                >
                  <SelectTrigger data-testid="input-project">
                    <SelectValue placeholder="Selecionar projeto" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="none">Nenhum</SelectItem>
                    {projects.map((project: any) => (
                      <SelectItem key={project.id} value={project.id}>{project.title}</SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </div>
            </div>

            <div className="space-y-2">
              <Label>Notas</Label>
              <Textarea
                value={formData.notes}
                onChange={(e) => setFormData({ ...formData, notes: e.target.value })}
                placeholder="Observações adicionais..."
                rows={3}
                data-testid="input-notes"
              />
            </div>

            <DialogFooter>
              <Button type="button" variant="outline" onClick={handleCloseModal}>
                Cancelar
              </Button>
              <Button type="submit" disabled={isSubmitting} data-testid="button-submit-transaction">
                {isSubmitting && <Loader2 className="mr-2 size-4 animate-spin" />}
                {selectedTransaction ? 'Salvar' : 'Criar'}
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>

      <DeleteModal
        open={deleteModalOpen}
        onOpenChange={setDeleteModalOpen}
        onConfirm={() => deleteMutation.mutate(selectedTransaction?.id)}
        title="Excluir Transação"
        description={`Tem certeza que deseja excluir a transação "${selectedTransaction?.description}"?`}
        isLoading={deleteMutation.isPending}
      />

      <RecurringTransactionModal
        open={recurringModalOpen}
        onOpenChange={setRecurringModalOpen}
        workspaceId={currentWorkspace?.id || ""}
      />
    </AppShell>
  );
}

interface TransactionTableProps {
  transactions: any[];
  isLoading: boolean;
  type: 'INCOME' | 'EXPENSE';
  onEdit: (transaction: any) => void;
  onDelete: (transaction: any) => void;
  onMarkAsPaid: (transaction: any) => void;
  getClientName: (id: string) => string;
  getCategoryName: (id: string) => string;
}

function TransactionTable({
  transactions,
  isLoading,
  type,
  onEdit,
  onDelete,
  onMarkAsPaid,
  getClientName,
  getCategoryName,
}: TransactionTableProps) {
  if (isLoading) {
    return <Skeleton className="h-64 rounded-xl" />;
  }

  const isOverdue = (transaction: any) => {
    if (transaction.status !== 'PENDING') return false;
    const dueDate = new Date(transaction.dueDate);
    return dueDate < new Date();
  };

  return (
    <TactileCard className="overflow-hidden">
      <Table>
        <TableHeader className="bg-muted/50">
          <TableRow>
            <TableHead>Data de Vencimento</TableHead>
            <TableHead>Descrição</TableHead>
            <TableHead>{type === 'INCOME' ? 'Cliente' : 'Fornecedor'}</TableHead>
            <TableHead>Categoria</TableHead>
            <TableHead className="text-right">Valor</TableHead>
            <TableHead>Status</TableHead>
            <TableHead className="w-[50px]"></TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {transactions.map((transaction: any) => (
            <TableRow 
              key={transaction.id} 
              className={cn(
                "hover:bg-muted/50",
                isOverdue(transaction) && "bg-red-50/50"
              )} 
              data-testid={`row-transaction-${transaction.id}`}
            >
              <TableCell>
                {format(new Date(transaction.dueDate), "d 'de' MMM, yyyy", { locale: ptBR })}
              </TableCell>
              <TableCell className="font-medium">{transaction.description}</TableCell>
              <TableCell>
                {type === 'INCOME' 
                  ? getClientName(transaction.clientId) 
                  : transaction.supplier || '-'}
              </TableCell>
              <TableCell>{getCategoryName(transaction.categoryId) || '-'}</TableCell>
              <TableCell className={cn(
                "text-right font-medium",
                type === 'INCOME' ? "text-green-600" : "text-red-600"
              )}>
                R$ {parseFloat(transaction.amount || '0').toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
              </TableCell>
              <TableCell>
                <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium border ${
                  isOverdue(transaction) 
                    ? statusMap['OVERDUE'].color 
                    : statusMap[transaction.status]?.color || 'bg-gray-50 text-gray-700 border-gray-200'
                }`}>
                  {isOverdue(transaction) 
                    ? statusMap['OVERDUE'].label 
                    : statusMap[transaction.status]?.label || transaction.status}
                </span>
              </TableCell>
              <TableCell>
                <DropdownMenu>
                  <DropdownMenuTrigger asChild>
                    <Button variant="ghost" size="icon" className="size-8">
                      <MoreHorizontal className="size-4" />
                    </Button>
                  </DropdownMenuTrigger>
                  <DropdownMenuContent align="end">
                    {transaction.status === 'PENDING' && (
                      <DropdownMenuItem onClick={() => onMarkAsPaid(transaction)}>
                        <CheckCircle2 className="size-4 mr-2" />
                        {type === 'INCOME' ? 'Marcar como Recebido' : 'Marcar como Pago'}
                      </DropdownMenuItem>
                    )}
                    <DropdownMenuItem onClick={() => onEdit(transaction)}>
                      <Pencil className="size-4 mr-2" />
                      Editar
                    </DropdownMenuItem>
                    <DropdownMenuItem onClick={() => onDelete(transaction)} className="text-destructive">
                      <Trash2 className="size-4 mr-2" />
                      Excluir
                    </DropdownMenuItem>
                  </DropdownMenuContent>
                </DropdownMenu>
              </TableCell>
            </TableRow>
          ))}
          {transactions.length === 0 && (
            <TableRow>
              <TableCell colSpan={7} className="h-24 text-center text-muted-foreground">
                {type === 'INCOME' 
                  ? 'Nenhuma conta a receber ainda. Crie sua primeira transação!' 
                  : 'Nenhuma conta a pagar ainda. Crie sua primeira transação!'}
              </TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>
    </TactileCard>
  );
}
