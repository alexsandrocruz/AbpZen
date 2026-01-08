import { useState } from "react";
import { useMutation, useQueryClient, useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { useToast } from "@/hooks/use-toast";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { ClientCombobox } from "@/components/ui/client-combobox";
import { CategoryCombobox } from "@/components/ui/category-combobox";
import { Calendar } from "@/components/ui/calendar";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";
import { CalendarIcon, Repeat, Loader2 } from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";

interface RecurringTransactionModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  workspaceId: string;
}

export function RecurringTransactionModal({
  open,
  onOpenChange,
  workspaceId,
}: RecurringTransactionModalProps) {
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [formData, setFormData] = useState({
    type: "INCOME" as "INCOME" | "EXPENSE",
    description: "",
    amount: "",
    categoryId: "",
    clientId: "",
    projectId: "",
    supplier: "",
    notes: "",
    startDate: new Date(),
    dueDay: 1,
    recurrenceCount: 12,
    recurrenceType: "MONTHLY" as "MONTHLY" | "WEEKLY" | "BIWEEKLY" | "YEARLY",
  });

  const { data: clients = [] } = useQuery({
    queryKey: ["clients", workspaceId],
    queryFn: () => api.getClients(workspaceId),
    enabled: !!workspaceId,
  });

  const { data: categories = [] } = useQuery({
    queryKey: ["categories", workspaceId],
    queryFn: () => api.getFinancialCategories(workspaceId),
    enabled: !!workspaceId,
  });

  const { data: projects = [] } = useQuery({
    queryKey: ["projects", workspaceId],
    queryFn: () => api.getProjects(workspaceId),
    enabled: !!workspaceId,
  });

  const createMutation = useMutation({
    mutationFn: (data: any) => api.createRecurringTransactions(workspaceId, data),
    onSuccess: (result) => {
      queryClient.invalidateQueries({ queryKey: ["transactions", workspaceId] });
      toast({
        title: "Transações criadas",
        description: result.message,
      });
      onOpenChange(false);
      resetForm();
    },
    onError: (error: any) => {
      toast({
        title: "Erro",
        description: error.message || "Erro ao criar transações recorrentes",
        variant: "destructive",
      });
    },
  });

  const resetForm = () => {
    setFormData({
      type: "INCOME",
      description: "",
      amount: "",
      categoryId: "",
      clientId: "",
      projectId: "",
      supplier: "",
      notes: "",
      startDate: new Date(),
      dueDay: 1,
      recurrenceCount: 12,
      recurrenceType: "MONTHLY" as "MONTHLY" | "WEEKLY" | "BIWEEKLY" | "YEARLY",
    });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.description || !formData.amount || !formData.recurrenceCount) {
      toast({
        title: "Campos obrigatórios",
        description: "Preencha todos os campos obrigatórios",
        variant: "destructive",
      });
      return;
    }

    const parsedAmount = parseFloat(formData.amount.replace(",", "."));
    if (isNaN(parsedAmount) || parsedAmount <= 0) {
      toast({
        title: "Valor inválido",
        description: "O valor deve ser um número positivo",
        variant: "destructive",
      });
      return;
    }

    const categoryId = formData.categoryId && formData.categoryId !== "none" ? formData.categoryId : null;
    const clientId = formData.type === "INCOME" && formData.clientId && formData.clientId !== "none" ? formData.clientId : null;
    const projectId = formData.projectId && formData.projectId !== "none" ? formData.projectId : null;

    createMutation.mutate({
      type: formData.type,
      description: formData.description,
      amount: parsedAmount,
      categoryId,
      clientId,
      projectId,
      supplier: formData.type === "EXPENSE" ? (formData.supplier || null) : null,
      notes: formData.notes || null,
      startDate: formData.startDate.toISOString(),
      dueDay: formData.dueDay,
      recurrenceCount: formData.recurrenceCount,
      recurrenceType: formData.recurrenceType,
    });
  };

  const recurrenceLabel = formData.recurrenceType === "MONTHLY" 
    ? "meses" 
    : formData.recurrenceType === "WEEKLY" 
      ? "semanas" 
      : formData.recurrenceType === "BIWEEKLY"
        ? "quinzenas"
        : "anos";

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-lg max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle className="flex items-center gap-2">
            <Repeat className="h-5 w-5" />
            Nova Transação Recorrente
          </DialogTitle>
          <DialogDescription>
            Crie múltiplas transações de uma vez. Ideal para contratos, assinaturas, aluguéis e salários.
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label>Tipo</Label>
              <Select
                value={formData.type}
                onValueChange={(v: "INCOME" | "EXPENSE") => setFormData({ ...formData, type: v, categoryId: "", clientId: "" })}
              >
                <SelectTrigger data-testid="input-recurring-type">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="INCOME">Receita (A Receber)</SelectItem>
                  <SelectItem value="EXPENSE">Despesa (A Pagar)</SelectItem>
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label>Frequência</Label>
              <Select
                value={formData.recurrenceType}
                onValueChange={(v: "MONTHLY" | "WEEKLY" | "YEARLY") => setFormData({ ...formData, recurrenceType: v })}
              >
                <SelectTrigger data-testid="input-recurrence-type">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="MONTHLY">Mensal</SelectItem>
                  <SelectItem value="BIWEEKLY">Quinzenal</SelectItem>
                  <SelectItem value="WEEKLY">Semanal</SelectItem>
                  <SelectItem value="YEARLY">Anual</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>

          <div className="space-y-2">
            <Label>Descrição *</Label>
            <Input
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              placeholder="Ex: Contrato de Manutenção TI - Escritório Fabio Ribeiro"
              data-testid="input-recurring-description"
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label>Valor (R$) *</Label>
              <Input
                value={formData.amount}
                onChange={(e) => setFormData({ ...formData, amount: e.target.value })}
                placeholder="1300,00"
                data-testid="input-recurring-amount"
              />
            </div>

            <div className="space-y-2">
              <Label>Quantidade de {recurrenceLabel} *</Label>
              <Input
                type="number"
                min="1"
                max="60"
                value={formData.recurrenceCount}
                onChange={(e) => setFormData({ ...formData, recurrenceCount: parseInt(e.target.value) || 1 })}
                data-testid="input-recurrence-count"
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label>Data de Início</Label>
              <Popover>
                <PopoverTrigger asChild>
                  <Button variant="outline" className="w-full justify-start text-left font-normal" data-testid="input-start-date">
                    <CalendarIcon className="mr-2 h-4 w-4" />
                    {format(formData.startDate, "dd/MM/yyyy", { locale: ptBR })}
                  </Button>
                </PopoverTrigger>
                <PopoverContent className="w-auto p-0">
                  <Calendar
                    mode="single"
                    selected={formData.startDate}
                    onSelect={(date) => date && setFormData({ ...formData, startDate: date, dueDay: date.getDate() })}
                    locale={ptBR}
                  />
                </PopoverContent>
              </Popover>
            </div>

            <div className="space-y-2">
              <Label>Dia de Vencimento</Label>
              <Input
                type="number"
                min="1"
                max="31"
                value={formData.dueDay}
                onChange={(e) => setFormData({ ...formData, dueDay: parseInt(e.target.value) || 1 })}
                placeholder="12"
                data-testid="input-due-day"
              />
            </div>
          </div>

          <div className="space-y-2">
            <Label>Categoria</Label>
            <CategoryCombobox
              categories={categories}
              value={formData.categoryId}
              onChange={(v) => setFormData({ ...formData, categoryId: v })}
              workspaceId={workspaceId}
              type={formData.type}
              placeholder="Buscar ou criar categoria..."
            />
          </div>

          <div className="space-y-2">
            <Label>{formData.type === "INCOME" ? "Cliente" : "Fornecedor"}</Label>
            {formData.type === "INCOME" ? (
              <ClientCombobox
                clients={clients}
                value={formData.clientId}
                onChange={(v) => setFormData({ ...formData, clientId: v })}
                workspaceId={workspaceId}
                placeholder="Buscar ou criar cliente..."
              />
            ) : (
              <Input
                value={formData.supplier}
                onChange={(e) => setFormData({ ...formData, supplier: e.target.value })}
                placeholder="Nome do fornecedor"
                data-testid="input-recurring-supplier"
              />
            )}
          </div>

          <div className="space-y-2">
            <Label>Projeto (opcional)</Label>
            <Select
              value={formData.projectId}
              onValueChange={(v) => setFormData({ ...formData, projectId: v })}
            >
              <SelectTrigger data-testid="input-recurring-project">
                <SelectValue placeholder="Selecionar projeto" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="none">Nenhum</SelectItem>
                {projects.map((project: any) => (
                  <SelectItem key={project.id} value={project.id}>
                    {project.name}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          <div className="space-y-2">
            <Label>Observações</Label>
            <Textarea
              value={formData.notes}
              onChange={(e) => setFormData({ ...formData, notes: e.target.value })}
              placeholder="Informações adicionais sobre esta transação recorrente"
              rows={2}
              data-testid="input-recurring-notes"
            />
          </div>

          <div className="bg-muted/50 rounded-lg p-3 text-sm">
            <p className="font-medium mb-1">Resumo:</p>
            <p className="text-muted-foreground">
              Serão criadas <strong>{formData.recurrenceCount}</strong> transações de{" "}
              <strong>R$ {formData.amount || "0,00"}</strong>{" "}
              {formData.recurrenceType === "MONTHLY" && <>com vencimento no dia <strong>{formData.dueDay}</strong> de cada mês</>}
              {formData.recurrenceType === "BIWEEKLY" && <>a cada quinzena (15 dias)</>}
              {formData.recurrenceType === "WEEKLY" && <>semanalmente</>}
              {formData.recurrenceType === "YEARLY" && <>anualmente no dia <strong>{formData.dueDay}</strong></>}
              , a partir de <strong>{format(formData.startDate, "MMMM/yyyy", { locale: ptBR })}</strong>.
            </p>
          </div>

          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Cancelar
            </Button>
            <Button type="submit" disabled={createMutation.isPending} data-testid="button-create-recurring">
              {createMutation.isPending ? (
                <>
                  <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                  Criando...
                </>
              ) : (
                <>
                  <Repeat className="mr-2 h-4 w-4" />
                  Criar {formData.recurrenceCount} Transações
                </>
              )}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
