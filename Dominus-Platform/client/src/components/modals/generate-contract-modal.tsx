import { useState, useEffect } from "react";
import { useAuth } from "@/lib/auth-store";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { useToast } from "@/hooks/use-toast";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";
import { CategoryCombobox } from "@/components/ui/category-combobox";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { format, addMonths, addWeeks, addDays, setDate } from "date-fns";
import { Calendar, FileText, CreditCard, Check, ArrowRight, ArrowLeft, Loader2 } from "lucide-react";

interface GenerateContractModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  proposal: any;
}

interface Installment {
  sequence: number;
  amount: number;
  percentage?: number;
  dueDate: string;
  offsetDays?: number;
  description?: string;
}

export function GenerateContractModal({ open, onOpenChange, proposal }: GenerateContractModalProps) {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [step, setStep] = useState(1);
  const [title, setTitle] = useState("");
  const [totalValue, setTotalValue] = useState("");
  const [startsOn, setStartsOn] = useState("");
  const [endsOn, setEndsOn] = useState("");
  const [scheduleType, setScheduleType] = useState<"EQUAL" | "PERCENTAGE" | "MANUAL">("EQUAL");
  const [installmentCount, setInstallmentCount] = useState("1");
  const [installmentFrequency, setInstallmentFrequency] = useState<"MONTHLY" | "WEEKLY" | "BIWEEKLY">("MONTHLY");
  const [installmentDueDay, setInstallmentDueDay] = useState("10");
  const [installments, setInstallments] = useState<Installment[]>([]);
  const [generateTransactions, setGenerateTransactions] = useState(false);
  const [categoryId, setCategoryId] = useState<string | null>(null);
  const [projectId, setProjectId] = useState<string | null>(null);

  const { data: projects = [] } = useQuery({
    queryKey: ['projects', currentWorkspace?.id],
    queryFn: () => api.getProjects(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: categories = [] } = useQuery({
    queryKey: ['categories', currentWorkspace?.id],
    queryFn: () => api.getFinancialCategories(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: proposalItems = [] } = useQuery({
    queryKey: ['proposalItems', currentWorkspace?.id, proposal?.id],
    queryFn: () => api.getProposalItems(currentWorkspace!.id, proposal!.id),
    enabled: !!currentWorkspace?.id && !!proposal?.id && open,
  });

  const calculateProposalTotal = () => {
    const subtotal = proposalItems.reduce((total: number, item: any) => {
      const itemSubtotal = item.quantity * parseFloat(item.price || "0");
      const discountValue = parseFloat(item.discount || "0");
      let itemTotal: number;
      if (item.discountType === "PERCENT") {
        const clampedDiscount = Math.min(Math.max(discountValue, 0), 100);
        itemTotal = itemSubtotal * (1 - clampedDiscount / 100);
      } else {
        itemTotal = itemSubtotal - Math.min(discountValue, itemSubtotal);
      }
      return total + Math.max(itemTotal, 0);
    }, 0);

    const globalDiscount = parseFloat(proposal?.globalDiscount || "0");
    if (proposal?.globalDiscountType === "PERCENT") {
      const clampedDiscount = Math.min(Math.max(globalDiscount, 0), 100);
      return Math.max(subtotal * (1 - clampedDiscount / 100), 0);
    }
    return Math.max(subtotal - Math.min(globalDiscount, subtotal), 0);
  };

  const generateMutation = useMutation({
    mutationFn: (data: any) => api.generateContractFromProposal(currentWorkspace!.id, proposal?.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['contracts', currentWorkspace?.id] });
      queryClient.invalidateQueries({ queryKey: ['proposals', currentWorkspace?.id] });
      if (generateTransactions) {
        queryClient.invalidateQueries({ queryKey: ['transactions', currentWorkspace?.id] });
      }
      toast({ title: "Sucesso", description: "Contrato gerado com sucesso!" });
      onOpenChange(false);
      resetForm();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  useEffect(() => {
    if (proposal && open) {
      setTitle(proposal.title || "");
      const today = new Date();
      setStartsOn(format(today, "yyyy-MM-dd"));
      setEndsOn(format(addMonths(today, 1), "yyyy-MM-dd"));
    }
  }, [proposal, open]);

  useEffect(() => {
    if (proposalItems.length > 0 && open) {
      const calculatedTotal = calculateProposalTotal();
      setTotalValue(calculatedTotal.toFixed(2));
    }
  }, [proposalItems, open]);

  const resetForm = () => {
    setStep(1);
    setTitle("");
    setTotalValue("");
    setStartsOn("");
    setEndsOn("");
    setScheduleType("EQUAL");
    setInstallmentCount("1");
    setInstallmentFrequency("MONTHLY");
    setInstallmentDueDay("10");
    setInstallments([]);
    setGenerateTransactions(false);
    setCategoryId(null);
    setProjectId(null);
  };

  const calculateInstallments = () => {
    const total = parseFloat(totalValue);
    const count = parseInt(installmentCount);
    const dueDay = parseInt(installmentDueDay);
    
    if (!total || !count || count < 1) return;

    const newInstallments: Installment[] = [];
    let baseDate = startsOn ? new Date(startsOn) : new Date();
    
    if (dueDay) {
      baseDate = setDate(baseDate, dueDay);
      if (baseDate < new Date(startsOn)) {
        baseDate = addMonths(baseDate, 1);
      }
    }

    if (scheduleType === "EQUAL") {
      const amount = total / count;
      for (let i = 0; i < count; i++) {
        let dueDate = baseDate;
        if (installmentFrequency === "MONTHLY") {
          dueDate = addMonths(baseDate, i);
        } else if (installmentFrequency === "WEEKLY") {
          dueDate = addWeeks(baseDate, i);
        } else if (installmentFrequency === "BIWEEKLY") {
          dueDate = addWeeks(baseDate, i * 2);
        }
        newInstallments.push({
          sequence: i + 1,
          amount: Math.round(amount * 100) / 100,
          dueDate: format(dueDate, "yyyy-MM-dd"),
          description: `Parcela ${i + 1}/${count}`,
        });
      }
      const diff = total - newInstallments.reduce((sum, inst) => sum + inst.amount, 0);
      if (diff !== 0 && newInstallments.length > 0) {
        newInstallments[newInstallments.length - 1].amount += diff;
      }
    } else if (scheduleType === "PERCENTAGE") {
      for (let i = 0; i < count; i++) {
        const percentage = 100 / count;
        const amount = (total * percentage) / 100;
        let dueDate = baseDate;
        if (installmentFrequency === "MONTHLY") {
          dueDate = addMonths(baseDate, i);
        } else if (installmentFrequency === "WEEKLY") {
          dueDate = addWeeks(baseDate, i);
        } else if (installmentFrequency === "BIWEEKLY") {
          dueDate = addWeeks(baseDate, i * 2);
        }
        newInstallments.push({
          sequence: i + 1,
          amount: Math.round(amount * 100) / 100,
          percentage: Math.round(percentage * 100) / 100,
          dueDate: format(dueDate, "yyyy-MM-dd"),
          description: `Parcela ${i + 1}/${count} (${percentage.toFixed(1)}%)`,
        });
      }
    }

    setInstallments(newInstallments);
  };

  useEffect(() => {
    if (scheduleType !== "MANUAL" && totalValue && installmentCount) {
      calculateInstallments();
    }
  }, [scheduleType, totalValue, installmentCount, installmentFrequency, installmentDueDay, startsOn]);

  const updateInstallment = (index: number, field: keyof Installment, value: any) => {
    const updated = [...installments];
    updated[index] = { ...updated[index], [field]: value };
    
    if (field === 'percentage' && scheduleType === "PERCENTAGE") {
      const total = parseFloat(totalValue);
      const newPercentage = parseFloat(value) || 0;
      updated[index].amount = Math.round((total * newPercentage / 100) * 100) / 100;
      updated[index].description = `Parcela ${updated[index].sequence}/${updated.length} (${newPercentage}%)`;
    }
    
    setInstallments(updated);
  };

  const addManualInstallment = () => {
    const newSequence = installments.length + 1;
    setInstallments([...installments, {
      sequence: newSequence,
      amount: 0,
      dueDate: format(new Date(), "yyyy-MM-dd"),
      description: `Parcela ${newSequence}`,
    }]);
  };

  const removeInstallment = (index: number) => {
    const updated = installments.filter((_, i) => i !== index);
    updated.forEach((inst, i) => inst.sequence = i + 1);
    setInstallments(updated);
  };

  const handleSubmit = () => {
    const data = {
      title,
      totalValue: parseFloat(totalValue),
      startsOn,
      endsOn,
      scheduleType,
      installmentCount: parseInt(installmentCount),
      installmentFrequency,
      installmentDueDay: parseInt(installmentDueDay),
      installments,
      generateTransactions,
      categoryId: generateTransactions ? categoryId : null,
      projectId,
    };
    generateMutation.mutate(data);
  };

  const canProceedStep1 = title && totalValue && parseFloat(totalValue) > 0 && startsOn && endsOn;
  const canProceedStep2 = scheduleType && installments.length > 0;
  const totalInstallmentAmount = installments.reduce((sum, inst) => sum + inst.amount, 0);
  const totalPercentage = installments.reduce((sum, inst) => sum + (inst.percentage || 0), 0);
  const totalMatch = Math.abs(totalInstallmentAmount - parseFloat(totalValue || "0")) < 0.01;
  const percentageMatch = scheduleType !== "PERCENTAGE" || Math.abs(totalPercentage - 100) < 0.01;
  const canSubmit = totalMatch && percentageMatch && (!generateTransactions || categoryId);

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(value);
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle className="flex items-center gap-2">
            <FileText className="size-5" />
            Gerar Contrato a partir da Proposta
          </DialogTitle>
        </DialogHeader>

        <div className="flex items-center justify-center gap-2 py-4 border-b">
          <div className={`flex items-center gap-2 px-3 py-1.5 rounded-full text-sm ${step >= 1 ? 'bg-primary text-white' : 'bg-muted text-muted-foreground'}`}>
            <Calendar className="size-4" />
            Dados
          </div>
          <ArrowRight className="size-4 text-muted-foreground" />
          <div className={`flex items-center gap-2 px-3 py-1.5 rounded-full text-sm ${step >= 2 ? 'bg-primary text-white' : 'bg-muted text-muted-foreground'}`}>
            <CreditCard className="size-4" />
            Parcelas
          </div>
          <ArrowRight className="size-4 text-muted-foreground" />
          <div className={`flex items-center gap-2 px-3 py-1.5 rounded-full text-sm ${step >= 3 ? 'bg-primary text-white' : 'bg-muted text-muted-foreground'}`}>
            <Check className="size-4" />
            Confirmar
          </div>
        </div>

        {step === 1 && (
          <div className="space-y-4 py-4">
            <div className="space-y-2">
              <Label htmlFor="title">Título do Contrato</Label>
              <Input
                id="title"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                placeholder="Ex: Contrato de Serviços"
                data-testid="input-contract-title"
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="totalValue">Valor Total (R$)</Label>
              <Input
                id="totalValue"
                type="number"
                min="0"
                step="0.01"
                value={totalValue}
                onChange={(e) => setTotalValue(e.target.value)}
                placeholder="0,00"
                data-testid="input-contract-total-value"
              />
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="startsOn">Data de Início</Label>
                <Input
                  id="startsOn"
                  type="date"
                  value={startsOn}
                  onChange={(e) => setStartsOn(e.target.value)}
                  data-testid="input-contract-starts-on"
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="endsOn">Data de Término</Label>
                <Input
                  id="endsOn"
                  type="date"
                  value={endsOn}
                  onChange={(e) => setEndsOn(e.target.value)}
                  data-testid="input-contract-ends-on"
                />
              </div>
            </div>

            <div className="space-y-2">
              <Label>Projeto Vinculado (opcional)</Label>
              <Select value={projectId || "none"} onValueChange={(v) => setProjectId(v === "none" ? null : v)}>
                <SelectTrigger data-testid="select-contract-project">
                  <SelectValue placeholder="Nenhum projeto" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="none">Nenhum projeto</SelectItem>
                  {projects.map((project: any) => (
                    <SelectItem key={project.id} value={project.id}>
                      {project.title}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          </div>
        )}

        {step === 2 && (
          <div className="space-y-4 py-4">
            <div className="space-y-2">
              <Label>Tipo de Parcelamento</Label>
              <Select value={scheduleType} onValueChange={(v: any) => setScheduleType(v)}>
                <SelectTrigger data-testid="select-schedule-type">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="EQUAL">Parcelas Iguais</SelectItem>
                  <SelectItem value="PERCENTAGE">Percentual por Parcela</SelectItem>
                  <SelectItem value="MANUAL">Configuração Manual</SelectItem>
                </SelectContent>
              </Select>
            </div>

            {scheduleType !== "MANUAL" && (
              <div className="grid grid-cols-3 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="installmentCount">Número de Parcelas</Label>
                  <Input
                    id="installmentCount"
                    type="number"
                    min="1"
                    value={installmentCount}
                    onChange={(e) => setInstallmentCount(e.target.value)}
                    data-testid="input-installment-count"
                  />
                </div>
                <div className="space-y-2">
                  <Label>Frequência</Label>
                  <Select value={installmentFrequency} onValueChange={(v: any) => setInstallmentFrequency(v)}>
                    <SelectTrigger data-testid="select-installment-frequency">
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="MONTHLY">Mensal</SelectItem>
                      <SelectItem value="WEEKLY">Semanal</SelectItem>
                      <SelectItem value="BIWEEKLY">Quinzenal</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-2">
                  <Label htmlFor="installmentDueDay">Dia Vencimento</Label>
                  <Input
                    id="installmentDueDay"
                    type="number"
                    min="1"
                    max="31"
                    value={installmentDueDay}
                    onChange={(e) => setInstallmentDueDay(e.target.value)}
                    data-testid="input-installment-due-day"
                  />
                </div>
              </div>
            )}

            {scheduleType === "MANUAL" && (
              <Button variant="outline" onClick={addManualInstallment} data-testid="button-add-installment">
                Adicionar Parcela
              </Button>
            )}

            {installments.length > 0 && (
              <div className="space-y-2 border rounded-lg p-4">
                <h4 className="font-medium">Prévia das Parcelas</h4>
                <div className="max-h-64 overflow-y-auto space-y-2">
                  {installments.map((inst, index) => (
                    <div key={inst.sequence} className="flex items-center gap-2 p-2 bg-muted/50 rounded">
                      <span className="text-sm font-medium w-20">#{inst.sequence}</span>
                      {scheduleType === "MANUAL" ? (
                        <>
                          <Input
                            type="number"
                            min="0"
                            step="0.01"
                            value={inst.amount}
                            onChange={(e) => updateInstallment(index, 'amount', parseFloat(e.target.value) || 0)}
                            className="w-32"
                            placeholder="Valor"
                          />
                          <Input
                            type="date"
                            value={inst.dueDate}
                            onChange={(e) => updateInstallment(index, 'dueDate', e.target.value)}
                            className="w-40"
                          />
                          <Button variant="ghost" size="sm" onClick={() => removeInstallment(index)} className="text-destructive">
                            Remover
                          </Button>
                        </>
                      ) : scheduleType === "PERCENTAGE" ? (
                        <>
                          <div className="flex items-center gap-1">
                            <Input
                              type="number"
                              min="0"
                              max="100"
                              step="0.01"
                              value={inst.percentage || 0}
                              onChange={(e) => updateInstallment(index, 'percentage', parseFloat(e.target.value) || 0)}
                              className="w-20"
                            />
                            <span className="text-xs text-muted-foreground">%</span>
                          </div>
                          <span className="text-sm flex-1">{formatCurrency(inst.amount)}</span>
                          <span className="text-sm text-muted-foreground">{format(new Date(inst.dueDate), "dd/MM/yyyy")}</span>
                        </>
                      ) : (
                        <>
                          <span className="text-sm flex-1">{formatCurrency(inst.amount)}</span>
                          <span className="text-sm text-muted-foreground">{format(new Date(inst.dueDate), "dd/MM/yyyy")}</span>
                        </>
                      )}
                    </div>
                  ))}
                </div>
                <div className="flex flex-col gap-1 pt-2 border-t">
                  <div className="flex justify-between">
                    <span className="font-medium">Total das Parcelas:</span>
                    <span className={`font-medium ${totalMatch ? 'text-green-600' : 'text-red-600'}`}>
                      {formatCurrency(totalInstallmentAmount)}
                      {!totalMatch && <span className="text-xs ml-2">(esperado: {formatCurrency(parseFloat(totalValue || "0"))})</span>}
                    </span>
                  </div>
                  {scheduleType === "PERCENTAGE" && (
                    <div className="flex justify-between text-sm">
                      <span>Soma dos Percentuais:</span>
                      <span className={percentageMatch ? 'text-green-600' : 'text-red-600'}>
                        {totalPercentage.toFixed(2)}%
                        {!percentageMatch && <span className="text-xs ml-2">(esperado: 100%)</span>}
                      </span>
                    </div>
                  )}
                </div>
              </div>
            )}
          </div>
        )}

        {step === 3 && (
          <div className="space-y-4 py-4">
            <div className="p-4 bg-muted/50 rounded-lg space-y-3">
              <h4 className="font-medium">Resumo do Contrato</h4>
              <div className="grid grid-cols-2 gap-2 text-sm">
                <span className="text-muted-foreground">Título:</span>
                <span>{title}</span>
                <span className="text-muted-foreground">Valor Total:</span>
                <span>{formatCurrency(parseFloat(totalValue || "0"))}</span>
                <span className="text-muted-foreground">Período:</span>
                <span>{startsOn && format(new Date(startsOn), "dd/MM/yyyy")} até {endsOn && format(new Date(endsOn), "dd/MM/yyyy")}</span>
                <span className="text-muted-foreground">Parcelas:</span>
                <span>{installments.length}x</span>
              </div>
            </div>

            <div className="flex items-center gap-3 p-4 border rounded-lg">
              <Checkbox
                id="generateTransactions"
                checked={generateTransactions}
                onCheckedChange={(checked) => setGenerateTransactions(checked as boolean)}
                data-testid="checkbox-generate-transactions"
              />
              <div>
                <Label htmlFor="generateTransactions" className="cursor-pointer">Gerar Transações Financeiras</Label>
                <p className="text-sm text-muted-foreground">Criar automaticamente receitas a receber para cada parcela.</p>
              </div>
            </div>

            {generateTransactions && currentWorkspace && (
              <div className="space-y-2 pl-7">
                <Label className={!categoryId ? "text-destructive" : ""}>
                  Categoria Financeira {!categoryId && <span className="text-xs">(obrigatória)</span>}
                </Label>
                <CategoryCombobox
                  categories={categories}
                  value={categoryId || ""}
                  onChange={(val) => setCategoryId(val || null)}
                  workspaceId={currentWorkspace.id}
                  type="INCOME"
                  placeholder="Selecionar categoria"
                />
              </div>
            )}
          </div>
        )}

        <DialogFooter className="gap-2">
          {step > 1 && (
            <Button variant="outline" onClick={() => setStep(step - 1)} data-testid="button-previous-step">
              <ArrowLeft className="size-4 mr-2" />
              Anterior
            </Button>
          )}
          <Button variant="outline" onClick={() => onOpenChange(false)} data-testid="button-cancel-contract">
            Cancelar
          </Button>
          {step < 3 ? (
            <Button
              onClick={() => setStep(step + 1)}
              disabled={step === 1 ? !canProceedStep1 : !canProceedStep2}
              data-testid="button-next-step"
            >
              Próximo
              <ArrowRight className="size-4 ml-2" />
            </Button>
          ) : (
            <Button
              onClick={handleSubmit}
              disabled={generateMutation.isPending || !canSubmit}
              data-testid="button-generate-contract"
            >
              {generateMutation.isPending && <Loader2 className="size-4 mr-2 animate-spin" />}
              Gerar Contrato
            </Button>
          )}
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
