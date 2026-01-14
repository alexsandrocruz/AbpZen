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
import { Loader2 } from "lucide-react";
import { CustomFieldsSection } from "@/components/custom-fields-section";
import { ClientCombobox } from "@/components/ui/client-combobox";

interface ProjectModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  project?: any;
}

export function ProjectModal({ open, onOpenChange, project }: ProjectModalProps) {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const isEditing = !!project;

  const { data: clients = [] } = useQuery({
    queryKey: ['clients', currentWorkspace?.id],
    queryFn: () => api.getClients(currentWorkspace!.id),
    enabled: !!currentWorkspace && open,
  });

  const [formData, setFormData] = useState({
    title: "",
    description: "",
    clientId: "",
    status: "PENDING",
    budget: "",
    dueDate: "",
  });

  const [customFieldValues, setCustomFieldValues] = useState<Record<string, any>>({});

  const { data: customFieldDefinitions = [] } = useQuery({
    queryKey: ["customFieldDefinitions", currentWorkspace?.id, "PROJECT"],
    queryFn: () => api.getCustomFields(currentWorkspace!.id, "PROJECT"),
    enabled: !!currentWorkspace,
  });

  useEffect(() => {
    if (project) {
      setFormData({
        title: project.title || "",
        description: project.description || "",
        clientId: project.clientId || "",
        status: project.status || "PENDING",
        budget: project.budget || "",
        dueDate: project.dueDate ? new Date(project.dueDate).toISOString().split('T')[0] : "",
      });
      setCustomFieldValues({});
    } else {
      setFormData({ title: "", description: "", clientId: "", status: "PENDING", budget: "", dueDate: "" });
      setCustomFieldValues({});
    }
  }, [project, open]);

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

    await api.saveCustomFieldValues(currentWorkspace!.id, "PROJECT", entityId, valuesToSave);
  };

  const createMutation = useMutation({
    mutationFn: (data: any) => api.createProject(currentWorkspace!.id, data),
    onSuccess: async (newProject: any) => {
      await saveCustomFields(newProject.id);
      queryClient.invalidateQueries({ queryKey: ['projects', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Projeto criado com sucesso!" });
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: (data: any) => api.updateProject(currentWorkspace!.id, project.id, data),
    onSuccess: async () => {
      await saveCustomFields(project.id);
      queryClient.invalidateQueries({ queryKey: ['projects', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Projeto atualizado com sucesso!" });
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    let dueDateValue = null;
    if (formData.dueDate) {
      try {
        const date = new Date(formData.dueDate);
        if (!isNaN(date.getTime())) {
          dueDateValue = date.toISOString();
        }
      } catch {
        dueDateValue = null;
      }
    }
    const dataToSend = {
      ...formData,
      budget: formData.budget || null,
      dueDate: dueDateValue,
    };
    if (isEditing) {
      updateMutation.mutate(dataToSend);
    } else {
      createMutation.mutate(dataToSend);
    }
  };

  const isLoading = createMutation.isPending || updateMutation.isPending;

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle>{isEditing ? "Editar Projeto" : "Novo Projeto"}</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="title">Título</Label>
            <Input
              id="title"
              value={formData.title}
              onChange={(e) => setFormData({ ...formData, title: e.target.value })}
              placeholder="Nome do projeto"
              required
              data-testid="input-project-title"
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="description">Descrição</Label>
            <Textarea
              id="description"
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              placeholder="Descreva o projeto..."
              data-testid="input-project-description"
            />
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
              <Label htmlFor="status">Status</Label>
              <Select
                value={formData.status}
                onValueChange={(value) => setFormData({ ...formData, status: value })}
              >
                <SelectTrigger data-testid="select-project-status">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="PENDING">Pendente</SelectItem>
                  <SelectItem value="IN_PROGRESS">Em Andamento</SelectItem>
                  <SelectItem value="COMPLETED">Concluído</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="budget">Orçamento (R$)</Label>
              <Input
                id="budget"
                type="number"
                step="0.01"
                value={formData.budget}
                onChange={(e) => setFormData({ ...formData, budget: e.target.value })}
                placeholder="0,00"
                data-testid="input-project-budget"
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="dueDate">Data de Entrega</Label>
              <Input
                id="dueDate"
                type="date"
                value={formData.dueDate}
                onChange={(e) => setFormData({ ...formData, dueDate: e.target.value })}
                data-testid="input-project-duedate"
              />
            </div>
          </div>
          {customFieldDefinitions.length > 0 && (
            <div className="border-t pt-4 mt-4">
              <CustomFieldsSection
                entityType="PROJECT"
                entityId={project?.id}
                values={customFieldValues}
                onChange={setCustomFieldValues}
              />
            </div>
          )}
          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Cancelar
            </Button>
            <Button type="submit" disabled={isLoading || !formData.clientId} data-testid="button-save-project">
              {isLoading && <Loader2 className="mr-2 size-4 animate-spin" />}
              {isEditing ? "Salvar" : "Criar"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
