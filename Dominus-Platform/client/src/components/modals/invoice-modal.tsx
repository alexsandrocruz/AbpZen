import { useState, useEffect } from "react";
import { useMutation, useQueryClient, useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { useAuth } from "@/lib/auth-store";
import { useToast } from "@/hooks/use-toast";
import {
  Dialog,
  DialogContent,
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
import { Loader2, Plus, Trash2 } from "lucide-react";
import { CustomFieldsSection } from "@/components/custom-fields-section";
import { ClientCombobox } from "@/components/ui/client-combobox";

interface InvoiceModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  invoice?: any;
}

interface InvoiceItem {
  description: string;
  quantity: number;
  price: string;
}

export function InvoiceModal({ open, onOpenChange, invoice }: InvoiceModalProps) {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const isEditing = !!invoice;

  const { data: clients = [] } = useQuery({
    queryKey: ['clients', currentWorkspace?.id],
    queryFn: () => api.getClients(currentWorkspace!.id),
    enabled: !!currentWorkspace && open,
  });

  const { data: projects = [] } = useQuery({
    queryKey: ['projects', currentWorkspace?.id],
    queryFn: () => api.getProjects(currentWorkspace!.id),
    enabled: !!currentWorkspace && open,
  });

  const [formData, setFormData] = useState({
    number: "",
    clientId: "",
    projectId: "",
    issueDate: "",
    dueDate: "",
    status: "DRAFT",
    notes: "",
  });

  const [items, setItems] = useState<InvoiceItem[]>([
    { description: "", quantity: 1, price: "" }
  ]);

  const [customFieldValues, setCustomFieldValues] = useState<Record<string, any>>({});

  const { data: customFieldDefinitions = [] } = useQuery({
    queryKey: ["customFieldDefinitions", currentWorkspace?.id, "INVOICE"],
    queryFn: () => api.getCustomFields(currentWorkspace!.id, "INVOICE"),
    enabled: !!currentWorkspace,
  });

  useEffect(() => {
    if (invoice) {
      setFormData({
        number: invoice.number || "",
        clientId: invoice.clientId || "",
        projectId: invoice.projectId || "",
        issueDate: invoice.issueDate ? new Date(invoice.issueDate).toISOString().split('T')[0] : "",
        dueDate: invoice.dueDate ? new Date(invoice.dueDate).toISOString().split('T')[0] : "",
        status: invoice.status || "DRAFT",
        notes: invoice.notes || "",
      });
      setCustomFieldValues({});
    } else {
      const today = new Date();
      const due = new Date(today);
      due.setDate(due.getDate() + 30);
      setFormData({
        number: `FAT-${Date.now().toString().slice(-6)}`,
        clientId: "",
        projectId: "",
        issueDate: today.toISOString().split('T')[0],
        dueDate: due.toISOString().split('T')[0],
        status: "DRAFT",
        notes: "",
      });
      setItems([{ description: "", quantity: 1, price: "" }]);
      setCustomFieldValues({});
    }
  }, [invoice, open]);

  const saveCustomFields = async (entityId: string) => {
    if (customFieldDefinitions.length === 0) return;
    
    const valuesToSave = customFieldDefinitions.map((def: any) => {
      const rawValue = customFieldValues[def.fieldKey];
      let value: any = null;

      if (def.fieldType === "CHECKBOX") {
        value = rawValue === true || rawValue === false ? rawValue : false;
      } else if (def.fieldType === "NUMBER" || def.fieldType === "CURRENCY") {
        const num = parseFloat(rawValue);
        value = !isNaN(num) ? num : null;
      } else if (def.fieldType === "DATE") {
        value = rawValue || null;
      } else if (def.fieldType === "MULTISELECT") {
        value = Array.isArray(rawValue) ? rawValue : [];
      } else {
        value = rawValue !== undefined && rawValue !== null && rawValue !== "" ? String(rawValue) : null;
      }

      return { definitionId: def.id, value };
    });

    await api.saveCustomFieldValues(currentWorkspace!.id, "INVOICE", entityId, valuesToSave);
  };

  const createMutation = useMutation({
    mutationFn: (data: any) => api.createInvoice(currentWorkspace!.id, data),
    onSuccess: async (newInvoice: any) => {
      await saveCustomFields(newInvoice.id);
      queryClient.invalidateQueries({ queryKey: ['invoices', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Fatura criada com sucesso!" });
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: (data: any) => api.updateInvoice(currentWorkspace!.id, invoice.id, data),
    onSuccess: async () => {
      await saveCustomFields(invoice.id);
      queryClient.invalidateQueries({ queryKey: ['invoices', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Fatura atualizada com sucesso!" });
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const validItems = items.filter(item => item.description && item.price);
    
    const parseDate = (dateStr: string) => {
      if (!dateStr) return new Date().toISOString();
      const date = new Date(dateStr);
      return isNaN(date.getTime()) ? new Date().toISOString() : date.toISOString();
    };
    
    const dataToSend = {
      ...formData,
      issueDate: parseDate(formData.issueDate),
      dueDate: parseDate(formData.dueDate),
      projectId: formData.projectId || null,
      items: validItems,
    };
    if (isEditing) {
      updateMutation.mutate(dataToSend);
    } else {
      createMutation.mutate(dataToSend);
    }
  };

  const addItem = () => {
    setItems([...items, { description: "", quantity: 1, price: "" }]);
  };

  const removeItem = (index: number) => {
    setItems(items.filter((_, i) => i !== index));
  };

  const updateItem = (index: number, field: keyof InvoiceItem, value: any) => {
    const newItems = [...items];
    newItems[index] = { ...newItems[index], [field]: value };
    setItems(newItems);
  };

  const calculateTotal = () => {
    return items.reduce((total, item) => {
      return total + (item.quantity * parseFloat(item.price || "0"));
    }, 0);
  };

  const isLoading = createMutation.isPending || updateMutation.isPending;

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[600px] max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>{isEditing ? "Editar Fatura" : "Nova Fatura"}</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="number">Número</Label>
              <Input
                id="number"
                value={formData.number}
                onChange={(e) => setFormData({ ...formData, number: e.target.value })}
                required
                data-testid="input-invoice-number"
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="status">Status</Label>
              <Select
                value={formData.status}
                onValueChange={(value) => setFormData({ ...formData, status: value })}
              >
                <SelectTrigger data-testid="select-invoice-status">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="DRAFT">Rascunho</SelectItem>
                  <SelectItem value="SENT">Enviada</SelectItem>
                  <SelectItem value="PAID">Paga</SelectItem>
                  <SelectItem value="OVERDUE">Atrasada</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="clientId">Cliente</Label>
              <ClientCombobox
                clients={clients}
                value={formData.clientId}
                onChange={(value) => setFormData({ ...formData, clientId: value })}
                workspaceId={currentWorkspace?.id || ""}
                placeholder="Buscar ou criar cliente..."
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="projectId">Projeto (opcional)</Label>
              <Select
                value={formData.projectId}
                onValueChange={(value) => setFormData({ ...formData, projectId: value })}
              >
                <SelectTrigger data-testid="select-invoice-project">
                  <SelectValue placeholder="Selecione..." />
                </SelectTrigger>
                <SelectContent>
                  {projects.map((project: any) => (
                    <SelectItem key={project.id} value={project.id}>
                      {project.title}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="issueDate">Data de Emissão</Label>
              <Input
                id="issueDate"
                type="date"
                value={formData.issueDate}
                onChange={(e) => setFormData({ ...formData, issueDate: e.target.value })}
                required
                data-testid="input-invoice-issuedate"
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="dueDate">Vencimento</Label>
              <Input
                id="dueDate"
                type="date"
                value={formData.dueDate}
                onChange={(e) => setFormData({ ...formData, dueDate: e.target.value })}
                required
                data-testid="input-invoice-duedate"
              />
            </div>
          </div>

          {!isEditing && (
            <div className="space-y-3">
              <div className="flex items-center justify-between">
                <Label>Itens</Label>
                <Button type="button" variant="outline" size="sm" onClick={addItem}>
                  <Plus className="size-4 mr-1" /> Adicionar Item
                </Button>
              </div>
              {items.map((item, index) => (
                <div key={index} className="flex gap-2 items-end">
                  <div className="flex-1">
                    <Input
                      placeholder="Descrição"
                      value={item.description}
                      onChange={(e) => updateItem(index, 'description', e.target.value)}
                      data-testid={`input-item-description-${index}`}
                    />
                  </div>
                  <div className="w-20">
                    <Input
                      type="number"
                      placeholder="Qtd"
                      min="1"
                      value={item.quantity}
                      onChange={(e) => updateItem(index, 'quantity', parseInt(e.target.value) || 1)}
                      data-testid={`input-item-quantity-${index}`}
                    />
                  </div>
                  <div className="w-28">
                    <Input
                      type="number"
                      step="0.01"
                      placeholder="Preço"
                      value={item.price}
                      onChange={(e) => updateItem(index, 'price', e.target.value)}
                      data-testid={`input-item-price-${index}`}
                    />
                  </div>
                  {items.length > 1 && (
                    <Button type="button" variant="ghost" size="icon" onClick={() => removeItem(index)}>
                      <Trash2 className="size-4 text-destructive" />
                    </Button>
                  )}
                </div>
              ))}
              <div className="text-right font-semibold">
                Total: R$ {calculateTotal().toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
              </div>
            </div>
          )}

          <div className="space-y-2">
            <Label htmlFor="notes">Observações</Label>
            <Textarea
              id="notes"
              value={formData.notes}
              onChange={(e) => setFormData({ ...formData, notes: e.target.value })}
              placeholder="Notas adicionais..."
              data-testid="input-invoice-notes"
            />
          </div>
          {customFieldDefinitions.length > 0 && (
            <div className="border-t pt-4 mt-4">
              <CustomFieldsSection
                entityType="INVOICE"
                entityId={invoice?.id}
                values={customFieldValues}
                onChange={setCustomFieldValues}
              />
            </div>
          )}
          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Cancelar
            </Button>
            <Button type="submit" disabled={isLoading || !formData.clientId} data-testid="button-save-invoice">
              {isLoading && <Loader2 className="mr-2 size-4 animate-spin" />}
              {isEditing ? "Salvar" : "Criar"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
