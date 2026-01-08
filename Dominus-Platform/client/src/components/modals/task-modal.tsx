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

interface TaskModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  task?: any;
}

export function TaskModal({ open, onOpenChange, task }: TaskModalProps) {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const isEditing = !!task;

  const { data: projects = [] } = useQuery({
    queryKey: ['projects', currentWorkspace?.id],
    queryFn: () => api.getProjects(currentWorkspace!.id),
    enabled: !!currentWorkspace && open,
  });

  const { data: teamMembers = [] } = useQuery({
    queryKey: ['team', currentWorkspace?.id],
    queryFn: () => api.getTeamMembers(currentWorkspace!.id),
    enabled: !!currentWorkspace && open,
  });

  const [formData, setFormData] = useState({
    title: "",
    description: "",
    projectId: "",
    dueDate: "",
    assigneeId: "",
  });

  const [customFieldValues, setCustomFieldValues] = useState<Record<string, any>>({});

  const { data: customFieldDefinitions = [] } = useQuery({
    queryKey: ["customFieldDefinitions", currentWorkspace?.id, "TASK"],
    queryFn: () => api.getCustomFields(currentWorkspace!.id, "TASK"),
    enabled: !!currentWorkspace,
  });

  useEffect(() => {
    if (task) {
      setFormData({
        title: task.title || "",
        description: task.description || "",
        projectId: task.projectId || "",
        dueDate: task.dueDate ? new Date(task.dueDate).toISOString().split('T')[0] : "",
        assigneeId: task.assigneeId || "",
      });
      setCustomFieldValues({});
    } else {
      setFormData({ title: "", description: "", projectId: "", dueDate: "", assigneeId: "" });
      setCustomFieldValues({});
    }
  }, [task, open]);

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

    await api.saveCustomFieldValues(currentWorkspace!.id, "TASK", entityId, valuesToSave);
  };

  const createMutation = useMutation({
    mutationFn: (data: any) => api.createTask(currentWorkspace!.id, data),
    onSuccess: async (newTask: any) => {
      await saveCustomFields(newTask.id);
      queryClient.invalidateQueries({ queryKey: ['tasks', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Tarefa criada com sucesso!" });
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: (data: any) => api.updateTask(currentWorkspace!.id, task.id, data),
    onSuccess: async () => {
      await saveCustomFields(task.id);
      queryClient.invalidateQueries({ queryKey: ['tasks', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Tarefa atualizada com sucesso!" });
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const dataToSend = {
      ...formData,
      dueDate: formData.dueDate ? new Date(formData.dueDate).toISOString() : null,
      assigneeId: formData.assigneeId || null,
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
          <DialogTitle>{isEditing ? "Editar Tarefa" : "Nova Tarefa"}</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="title">Título</Label>
            <Input
              id="title"
              value={formData.title}
              onChange={(e) => setFormData({ ...formData, title: e.target.value })}
              placeholder="O que precisa ser feito?"
              required
              data-testid="input-task-title"
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="description">Descrição</Label>
            <Textarea
              id="description"
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              placeholder="Detalhes da tarefa..."
              data-testid="input-task-description"
            />
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="projectId">Projeto</Label>
              <Select
                value={formData.projectId}
                onValueChange={(value) => setFormData({ ...formData, projectId: value })}
              >
                <SelectTrigger data-testid="select-task-project">
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
            <div className="space-y-2">
              <Label htmlFor="assigneeId">Responsável</Label>
              <Select
                value={formData.assigneeId}
                onValueChange={(value) => setFormData({ ...formData, assigneeId: value })}
              >
                <SelectTrigger data-testid="select-task-assignee">
                  <SelectValue placeholder="Selecione..." />
                </SelectTrigger>
                <SelectContent>
                  {teamMembers.map((member: any) => (
                    <SelectItem key={member.userId} value={member.userId}>
                      {member.user?.name || member.user?.email}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          </div>
          <div className="space-y-2">
            <Label htmlFor="dueDate">Data de Entrega</Label>
            <Input
              id="dueDate"
              type="date"
              value={formData.dueDate}
              onChange={(e) => setFormData({ ...formData, dueDate: e.target.value })}
              data-testid="input-task-duedate"
            />
          </div>
          {customFieldDefinitions.length > 0 && (
            <div className="border-t pt-4 mt-4">
              <CustomFieldsSection
                entityType="TASK"
                entityId={task?.id}
                values={customFieldValues}
                onChange={setCustomFieldValues}
              />
            </div>
          )}
          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Cancelar
            </Button>
            <Button type="submit" disabled={isLoading || !formData.title} data-testid="button-save-task">
              {isLoading && <Loader2 className="mr-2 size-4 animate-spin" />}
              {isEditing ? "Salvar" : "Criar"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
