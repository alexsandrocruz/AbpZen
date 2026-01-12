import { useState, useEffect } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Skeleton } from "@/components/ui/skeleton";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { Switch } from "@/components/ui/switch";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";
import { useToast } from "@/hooks/use-toast";
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
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { DeleteModal } from "@/components/modals/delete-modal";
import {
  Plus,
  MoreVertical,
  Pencil,
  Trash2,
  ArrowLeft,
  Settings,
  GripVertical,
  Type,
  Mail,
  Phone,
  CheckSquare,
  ChevronDown,
  AlignLeft,
  Calendar,
  Link as LinkIcon,
  Hash,
  Eye,
  FileInput,
  Code,
  Copy,
  Check,
} from "lucide-react";
import {
  DndContext,
  DragEndEvent,
  DragOverlay,
  DragStartEvent,
  PointerSensor,
  useSensor,
  useSensors,
  closestCenter,
} from "@dnd-kit/core";
import {
  SortableContext,
  useSortable,
  verticalListSortingStrategy,
  arrayMove,
} from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";

const FIELD_TYPES = [
  { value: "TEXT", label: "Texto", icon: Type },
  { value: "EMAIL", label: "E-mail", icon: Mail },
  { value: "PHONE", label: "Telefone", icon: Phone },
  { value: "CHECKBOX", label: "Checkbox", icon: CheckSquare },
  { value: "SELECT", label: "Seleção", icon: ChevronDown },
  { value: "TEXTAREA", label: "Área de Texto", icon: AlignLeft },
  { value: "DATE", label: "Data", icon: Calendar },
  { value: "URL", label: "URL", icon: LinkIcon },
  { value: "NUMBER", label: "Número", icon: Hash },
] as const;

function getFieldIcon(type: string) {
  const fieldType = FIELD_TYPES.find((f) => f.value === type);
  return fieldType?.icon || Type;
}

function getFieldLabel(type: string) {
  const fieldType = FIELD_TYPES.find((f) => f.value === type);
  return fieldType?.label || type;
}

export default function LeadFormsPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [selectedForm, setSelectedForm] = useState<any>(null);
  const [formModalOpen, setFormModalOpen] = useState(false);
  const [editingForm, setEditingForm] = useState<any>(null);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [formToDelete, setFormToDelete] = useState<any>(null);
  const [embedModalOpen, setEmbedModalOpen] = useState(false);
  const [embedForm, setEmbedForm] = useState<any>(null);

  const { data: forms = [], isLoading } = useQuery({
    queryKey: ["lead-forms", currentWorkspace?.id],
    queryFn: () => api.getLeadForms(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: workflows = [] } = useQuery({
    queryKey: ["lead-workflows", currentWorkspace?.id],
    queryFn: () => api.getLeadWorkflows(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const createFormMutation = useMutation({
    mutationFn: (data: any) => api.createLeadForm(currentWorkspace!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lead-forms", currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Formulário criado com sucesso!" });
      setFormModalOpen(false);
      setEditingForm(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateFormMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: any }) =>
      api.updateLeadForm(currentWorkspace!.id, id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lead-forms", currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Formulário atualizado com sucesso!" });
      setFormModalOpen(false);
      setEditingForm(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteFormMutation = useMutation({
    mutationFn: (id: string) => api.deleteLeadForm(currentWorkspace!.id, id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lead-forms", currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Formulário excluído com sucesso!" });
      setDeleteModalOpen(false);
      setFormToDelete(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleNewForm = () => {
    setEditingForm(null);
    setFormModalOpen(true);
  };

  const handleEditForm = (form: any) => {
    setEditingForm(form);
    setFormModalOpen(true);
  };

  const handleDeleteForm = (form: any) => {
    setFormToDelete(form);
    setDeleteModalOpen(true);
  };

  const handleEmbedForm = (form: any) => {
    setEmbedForm(form);
    setEmbedModalOpen(true);
  };

  const getWorkflowName = (workflowId: string) => {
    const workflow = workflows.find((w: any) => w.id === workflowId);
    return workflow?.name || "-";
  };

  if (selectedForm) {
    return (
      <AppShell>
        <FormEditor
          form={selectedForm}
          onBack={() => setSelectedForm(null)}
          onUpdateForm={(data: any) => {
            updateFormMutation.mutate({ id: selectedForm.id, data });
            setSelectedForm({ ...selectedForm, ...data });
          }}
        />
      </AppShell>
    );
  }

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight">Formulários</h1>
            <p className="text-muted-foreground">
              Crie e gerencie formulários para captação de leads.
            </p>
          </div>
          <Button className="gap-2" onClick={handleNewForm} data-testid="button-add-form">
            <Plus className="size-4" />
            Novo Formulário
          </Button>
        </div>

        {isLoading ? (
          <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
            {[...Array(3)].map((_, i) => (
              <Skeleton key={i} className="h-40 rounded-xl" />
            ))}
          </div>
        ) : forms.length === 0 ? (
          <div className="flex flex-col items-center justify-center py-16 text-center">
            <div className="size-16 rounded-full bg-muted flex items-center justify-center mb-4">
              <FileInput className="size-8 text-muted-foreground" />
            </div>
            <h3 className="text-lg font-semibold mb-2">Nenhum formulário encontrado</h3>
            <p className="text-muted-foreground mb-4">
              Crie seu primeiro formulário para começar a captar leads.
            </p>
            <Button onClick={handleNewForm}>
              <Plus className="size-4 mr-2" />
              Criar Formulário
            </Button>
          </div>
        ) : (
          <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
            {forms.map((form: any) => (
              <TactileCard
                key={form.id}
                className="cursor-pointer hover:border-primary/50 transition-colors"
                onClick={() => setSelectedForm(form)}
                data-testid={`card-form-${form.id}`}
              >
                <div className="p-6">
                  <div className="flex items-start justify-between mb-4">
                    <div className="flex-1">
                      <h3
                        className="font-semibold text-lg"
                        data-testid={`text-form-name-${form.id}`}
                      >
                        {form.name}
                      </h3>
                      {form.description && (
                        <p className="text-sm text-muted-foreground mt-1 line-clamp-2">
                          {form.description}
                        </p>
                      )}
                    </div>
                    <DropdownMenu>
                      <DropdownMenuTrigger asChild onClick={(e) => e.stopPropagation()}>
                        <Button variant="ghost" size="icon" className="size-8">
                          <MoreVertical className="size-4" />
                        </Button>
                      </DropdownMenuTrigger>
                      <DropdownMenuContent align="end">
                        <DropdownMenuItem
                          onClick={(e) => {
                            e.stopPropagation();
                            handleEditForm(form);
                          }}
                        >
                          <Pencil className="size-4 mr-2" />
                          Editar
                        </DropdownMenuItem>
                        <DropdownMenuItem
                          onClick={(e) => {
                            e.stopPropagation();
                            handleEmbedForm(form);
                          }}
                        >
                          <Code className="size-4 mr-2" />
                          Código Embed
                        </DropdownMenuItem>
                        <DropdownMenuItem
                          className="text-destructive"
                          onClick={(e) => {
                            e.stopPropagation();
                            handleDeleteForm(form);
                          }}
                        >
                          <Trash2 className="size-4 mr-2" />
                          Excluir
                        </DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </div>
                  <div className="flex items-center gap-3 flex-wrap">
                    <Badge variant={form.isActive ? "default" : "secondary"}>
                      {form.isActive ? "Ativo" : "Inativo"}
                    </Badge>
                    {form.workflowId && (
                      <span className="text-sm text-muted-foreground">
                        Funil: {getWorkflowName(form.workflowId)}
                      </span>
                    )}
                  </div>
                </div>
              </TactileCard>
            ))}
          </div>
        )}
      </div>

      <FormModal
        open={formModalOpen}
        onOpenChange={setFormModalOpen}
        form={editingForm}
        workflows={workflows}
        onSubmit={(data) => {
          if (editingForm) {
            updateFormMutation.mutate({ id: editingForm.id, data });
          } else {
            createFormMutation.mutate(data);
          }
        }}
        isLoading={createFormMutation.isPending || updateFormMutation.isPending}
      />

      <DeleteModal
        open={deleteModalOpen}
        onOpenChange={setDeleteModalOpen}
        onConfirm={() => deleteFormMutation.mutate(formToDelete?.id)}
        title="Excluir Formulário"
        description={`Tem certeza que deseja excluir o formulário "${formToDelete?.name}"? Esta ação não pode ser desfeita.`}
        isLoading={deleteFormMutation.isPending}
      />

      <EmbedModal
        open={embedModalOpen}
        onOpenChange={setEmbedModalOpen}
        form={embedForm}
      />
    </AppShell>
  );
}

function FormModal({
  open,
  onOpenChange,
  form,
  workflows,
  onSubmit,
  isLoading,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  form: any;
  workflows: any[];
  onSubmit: (data: any) => void;
  isLoading: boolean;
}) {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [workflowId, setWorkflowId] = useState<string | null>(null);
  const [isActive, setIsActive] = useState(true);
  const [isLeadGenerator, setIsLeadGenerator] = useState(false);

  useEffect(() => {
    if (form) {
      setName(form.name || "");
      setDescription(form.description || "");
      setWorkflowId(form.workflowId || null);
      setIsActive(form.isActive ?? true);
      setIsLeadGenerator(form.isLeadGenerator ?? false);
    } else {
      setName("");
      setDescription("");
      setWorkflowId(null);
      setIsActive(true);
      setIsLeadGenerator(false);
    }
  }, [form, open]);

  const handleSubmit = () => {
    if (!name.trim()) return;
    onSubmit({ name, description, workflowId, isActive, isLeadGenerator });
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{form ? "Editar Formulário" : "Novo Formulário"}</DialogTitle>
        </DialogHeader>
        <div className="space-y-4 py-4">
          <div className="space-y-2">
            <Label htmlFor="name">Nome</Label>
            <Input
              id="name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              placeholder="Ex: Formulário de Contato"
              data-testid="input-form-name"
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="description">Descrição</Label>
            <Textarea
              id="description"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              placeholder="Descrição do formulário..."
              rows={3}
              data-testid="input-form-description"
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="workflow">Funil Vinculado</Label>
            <Select
              value={workflowId || "none"}
              onValueChange={(val) => setWorkflowId(val === "none" ? null : val)}
            >
              <SelectTrigger data-testid="select-form-workflow">
                <SelectValue placeholder="Selecione um funil (opcional)" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="none">Nenhum</SelectItem>
                {workflows.map((w: any) => (
                  <SelectItem key={w.id} value={w.id}>
                    {w.name}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>
          <div className="flex items-center justify-between">
            <Label htmlFor="isActive">Ativo</Label>
            <Switch
              id="isActive"
              checked={isActive}
              onCheckedChange={setIsActive}
              data-testid="switch-form-active"
            />
          </div>
          <div className="flex items-center justify-between">
            <div className="space-y-0.5">
              <Label htmlFor="isLeadGenerator">Gerador de Lead</Label>
              <p className="text-xs text-muted-foreground">
                Cria automaticamente um lead ao enviar o formulário
              </p>
            </div>
            <Switch
              id="isLeadGenerator"
              checked={isLeadGenerator}
              onCheckedChange={setIsLeadGenerator}
              data-testid="switch-form-lead-generator"
            />
          </div>
        </div>
        <DialogFooter>
          <Button variant="outline" onClick={() => onOpenChange(false)}>
            Cancelar
          </Button>
          <Button
            onClick={handleSubmit}
            disabled={isLoading || !name.trim()}
            data-testid="button-save-form"
          >
            {isLoading ? "Salvando..." : "Salvar"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}

function FormEditor({
  form,
  onBack,
  onUpdateForm,
}: {
  form: any;
  onBack: () => void;
  onUpdateForm: (data: any) => void;
}) {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [fieldModalOpen, setFieldModalOpen] = useState(false);
  const [editingField, setEditingField] = useState<any>(null);
  const [settingsModalOpen, setSettingsModalOpen] = useState(false);
  const [deleteFieldModalOpen, setDeleteFieldModalOpen] = useState(false);
  const [fieldToDelete, setFieldToDelete] = useState<any>(null);
  const [activeFieldId, setActiveFieldId] = useState<string | null>(null);

  const { data: formData, isLoading: formLoading } = useQuery({
    queryKey: ["lead-form", currentWorkspace?.id, form.id],
    queryFn: () => api.getLeadForm(currentWorkspace!.id, form.id),
    enabled: !!currentWorkspace && !!form.id,
  });

  const fields = formData?.fields || [];

  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: {
        distance: 8,
      },
    })
  );

  const createFieldMutation = useMutation({
    mutationFn: (data: any) =>
      api.createLeadFormField(currentWorkspace!.id, form.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lead-form", currentWorkspace?.id, form.id] });
      toast({ title: "Sucesso", description: "Campo adicionado com sucesso!" });
      setFieldModalOpen(false);
      setEditingField(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateFieldMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: any }) =>
      api.updateLeadFormField(currentWorkspace!.id, form.id, id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lead-form", currentWorkspace?.id, form.id] });
      toast({ title: "Sucesso", description: "Campo atualizado com sucesso!" });
      setFieldModalOpen(false);
      setEditingField(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteFieldMutation = useMutation({
    mutationFn: (id: string) =>
      api.deleteLeadFormField(currentWorkspace!.id, form.id, id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lead-form", currentWorkspace?.id, form.id] });
      toast({ title: "Sucesso", description: "Campo excluído com sucesso!" });
      setDeleteFieldModalOpen(false);
      setFieldToDelete(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const reorderFieldsMutation = useMutation({
    mutationFn: (fieldIds: string[]) =>
      api.reorderLeadFormFields(currentWorkspace!.id, form.id, fieldIds),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lead-form", currentWorkspace?.id, form.id] });
    },
  });

  const handleDragStart = (event: DragStartEvent) => {
    setActiveFieldId(event.active.id as string);
  };

  const handleDragEnd = (event: DragEndEvent) => {
    const { active, over } = event;
    setActiveFieldId(null);

    if (!over || active.id === over.id) return;

    const oldIndex = fields.findIndex((f: any) => f.id === active.id);
    const newIndex = fields.findIndex((f: any) => f.id === over.id);

    if (oldIndex !== -1 && newIndex !== -1) {
      const newFields = arrayMove(fields, oldIndex, newIndex);
      reorderFieldsMutation.mutate(newFields.map((f: any) => f.id));
    }
  };

  const handleAddField = () => {
    setEditingField(null);
    setFieldModalOpen(true);
  };

  const handleEditField = (field: any) => {
    setEditingField(field);
    setFieldModalOpen(true);
  };

  const handleDeleteField = (field: any) => {
    setFieldToDelete(field);
    setDeleteFieldModalOpen(true);
  };

  const activeField = fields.find((f: any) => f.id === activeFieldId);

  const sortedFields = [...fields].sort((a: any, b: any) => a.position - b.position);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <Button variant="ghost" size="icon" onClick={onBack}>
            <ArrowLeft className="size-5" />
          </Button>
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight">{form.name}</h1>
            {form.description && <p className="text-muted-foreground">{form.description}</p>}
          </div>
        </div>
        <div className="flex items-center gap-2">
          <Button variant="outline" onClick={() => setSettingsModalOpen(true)} data-testid="button-form-settings">
            <Settings className="size-4 mr-2" />
            Configurações
          </Button>
          <Button onClick={handleAddField} data-testid="button-add-field">
            <Plus className="size-4 mr-2" />
            Adicionar Campo
          </Button>
        </div>
      </div>

      {formLoading ? (
        <div className="grid lg:grid-cols-2 gap-6">
          <Skeleton className="h-96 rounded-xl" />
          <Skeleton className="h-96 rounded-xl" />
        </div>
      ) : (
        <div className="grid lg:grid-cols-2 gap-6">
          <TactileCard>
            <div className="p-6">
              <div className="flex items-center gap-2 mb-4">
                <h2 className="font-semibold text-lg">Campos do Formulário</h2>
                <Badge variant="secondary">{sortedFields.length} campos</Badge>
              </div>

              {sortedFields.length === 0 ? (
                <div className="flex flex-col items-center justify-center py-12 text-center border-2 border-dashed rounded-lg">
                  <Type className="size-8 text-muted-foreground mb-2" />
                  <p className="text-muted-foreground mb-4">Nenhum campo adicionado ainda</p>
                  <Button variant="outline" onClick={handleAddField}>
                    <Plus className="size-4 mr-2" />
                    Adicionar Campo
                  </Button>
                </div>
              ) : (
                <DndContext
                  sensors={sensors}
                  collisionDetection={closestCenter}
                  onDragStart={handleDragStart}
                  onDragEnd={handleDragEnd}
                >
                  <SortableContext
                    items={sortedFields.map((f: any) => f.id)}
                    strategy={verticalListSortingStrategy}
                  >
                    <div className="space-y-2">
                      {sortedFields.map((field: any) => (
                        <SortableFieldItem
                          key={field.id}
                          field={field}
                          onEdit={() => handleEditField(field)}
                          onDelete={() => handleDeleteField(field)}
                        />
                      ))}
                    </div>
                  </SortableContext>
                  <DragOverlay>
                    {activeField && <FieldItemOverlay field={activeField} />}
                  </DragOverlay>
                </DndContext>
              )}
            </div>
          </TactileCard>

          <TactileCard>
            <div className="p-6">
              <div className="flex items-center gap-2 mb-4">
                <Eye className="size-5 text-muted-foreground" />
                <h2 className="font-semibold text-lg">Prévia do Formulário</h2>
              </div>
              <FormPreview
                fields={sortedFields}
                submitButtonText={formData?.submitButtonText || "Enviar"}
                successMessage={formData?.successMessage}
              />
            </div>
          </TactileCard>
        </div>
      )}

      <FieldModal
        open={fieldModalOpen}
        onOpenChange={setFieldModalOpen}
        field={editingField}
        onSubmit={(data) => {
          if (editingField) {
            updateFieldMutation.mutate({ id: editingField.id, data });
          } else {
            createFieldMutation.mutate({ ...data, position: fields.length });
          }
        }}
        isLoading={createFieldMutation.isPending || updateFieldMutation.isPending}
      />

      <SettingsModal
        open={settingsModalOpen}
        onOpenChange={setSettingsModalOpen}
        form={formData || form}
        onSubmit={onUpdateForm}
      />

      <DeleteModal
        open={deleteFieldModalOpen}
        onOpenChange={setDeleteFieldModalOpen}
        onConfirm={() => deleteFieldMutation.mutate(fieldToDelete?.id)}
        title="Excluir Campo"
        description={`Tem certeza que deseja excluir o campo "${fieldToDelete?.label}"?`}
        isLoading={deleteFieldMutation.isPending}
      />
    </div>
  );
}

function SortableFieldItem({
  field,
  onEdit,
  onDelete,
}: {
  field: any;
  onEdit: () => void;
  onDelete: () => void;
}) {
  const { attributes, listeners, setNodeRef, transform, transition, isDragging } = useSortable({
    id: field.id,
  });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
  };

  const Icon = getFieldIcon(field.type);

  return (
    <div
      ref={setNodeRef}
      style={style}
      className="flex items-center gap-3 p-3 bg-muted/50 rounded-lg border hover:border-primary/50 transition-colors"
      data-testid={`field-item-${field.id}`}
    >
      <div {...attributes} {...listeners} className="cursor-grab active:cursor-grabbing">
        <GripVertical className="size-4 text-muted-foreground" />
      </div>
      <div className="size-8 rounded bg-background flex items-center justify-center">
        <Icon className="size-4 text-muted-foreground" />
      </div>
      <div className="flex-1 min-w-0">
        <div className="flex items-center gap-2">
          <span className="font-medium truncate">{field.label}</span>
          {field.required && (
            <Badge variant="destructive" className="text-xs">
              Obrigatório
            </Badge>
          )}
        </div>
        <span className="text-xs text-muted-foreground">{getFieldLabel(field.type)}</span>
      </div>
      <div className="flex items-center gap-1">
        <Button variant="ghost" size="icon" className="size-8" onClick={onEdit}>
          <Pencil className="size-4" />
        </Button>
        <Button variant="ghost" size="icon" className="size-8 text-destructive" onClick={onDelete}>
          <Trash2 className="size-4" />
        </Button>
      </div>
    </div>
  );
}

function FieldItemOverlay({ field }: { field: any }) {
  const Icon = getFieldIcon(field.type);

  return (
    <div className="flex items-center gap-3 p-3 bg-background rounded-lg border-2 border-primary shadow-lg">
      <GripVertical className="size-4 text-muted-foreground" />
      <div className="size-8 rounded bg-muted flex items-center justify-center">
        <Icon className="size-4 text-muted-foreground" />
      </div>
      <div className="flex-1 min-w-0">
        <div className="flex items-center gap-2">
          <span className="font-medium truncate">{field.label}</span>
          {field.required && (
            <Badge variant="destructive" className="text-xs">
              Obrigatório
            </Badge>
          )}
        </div>
        <span className="text-xs text-muted-foreground">{getFieldLabel(field.type)}</span>
      </div>
    </div>
  );
}

function FieldModal({
  open,
  onOpenChange,
  field,
  onSubmit,
  isLoading,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  field: any;
  onSubmit: (data: any) => void;
  isLoading: boolean;
}) {
  const [type, setType] = useState("TEXT");
  const [label, setLabel] = useState("");
  const [placeholder, setPlaceholder] = useState("");
  const [required, setRequired] = useState(false);
  const [options, setOptions] = useState("");

  useEffect(() => {
    if (field) {
      setType(field.type || "TEXT");
      setLabel(field.label || "");
      setPlaceholder(field.placeholder || "");
      setRequired(field.required ?? false);
      setOptions(field.options || "");
    } else {
      setType("TEXT");
      setLabel("");
      setPlaceholder("");
      setRequired(false);
      setOptions("");
    }
  }, [field, open]);

  const handleSubmit = () => {
    if (!label.trim()) return;
    onSubmit({
      type,
      label,
      placeholder: placeholder || null,
      required,
      options: type === "SELECT" ? options : null,
    });
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-md">
        <DialogHeader>
          <DialogTitle>{field ? "Editar Campo" : "Adicionar Campo"}</DialogTitle>
        </DialogHeader>
        <div className="space-y-4 py-4">
          <div className="space-y-2">
            <Label htmlFor="type">Tipo do Campo</Label>
            <Select value={type} onValueChange={setType}>
              <SelectTrigger data-testid="select-field-type">
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                {FIELD_TYPES.map((ft) => {
                  const Icon = ft.icon;
                  return (
                    <SelectItem key={ft.value} value={ft.value}>
                      <div className="flex items-center gap-2">
                        <Icon className="size-4" />
                        {ft.label}
                      </div>
                    </SelectItem>
                  );
                })}
              </SelectContent>
            </Select>
          </div>
          <div className="space-y-2">
            <Label htmlFor="label">Label</Label>
            <Input
              id="label"
              value={label}
              onChange={(e) => setLabel(e.target.value)}
              placeholder="Ex: Nome completo"
              data-testid="input-field-label"
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="placeholder">Placeholder (opcional)</Label>
            <Input
              id="placeholder"
              value={placeholder}
              onChange={(e) => setPlaceholder(e.target.value)}
              placeholder="Ex: Digite seu nome..."
              data-testid="input-field-placeholder"
            />
          </div>
          {type === "SELECT" && (
            <div className="space-y-2">
              <Label htmlFor="options">Opções (uma por linha)</Label>
              <Textarea
                id="options"
                value={options}
                onChange={(e) => setOptions(e.target.value)}
                placeholder="Opção 1&#10;Opção 2&#10;Opção 3"
                rows={4}
                data-testid="input-field-options"
              />
            </div>
          )}
          <div className="flex items-center space-x-2">
            <Checkbox
              id="required"
              checked={required}
              onCheckedChange={(checked) => setRequired(!!checked)}
              data-testid="checkbox-field-required"
            />
            <Label htmlFor="required" className="cursor-pointer">
              Campo obrigatório
            </Label>
          </div>
        </div>
        <DialogFooter>
          <Button variant="outline" onClick={() => onOpenChange(false)}>
            Cancelar
          </Button>
          <Button
            onClick={handleSubmit}
            disabled={isLoading || !label.trim()}
            data-testid="button-save-field"
          >
            {isLoading ? "Salvando..." : "Salvar"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}

function SettingsModal({
  open,
  onOpenChange,
  form,
  onSubmit,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  form: any;
  onSubmit: (data: any) => void;
}) {
  const [submitButtonText, setSubmitButtonText] = useState(form?.submitButtonText || "Enviar");
  const [successMessage, setSuccessMessage] = useState(
    form?.successMessage || "Obrigado! Entraremos em contato em breve."
  );

  useEffect(() => {
    if (form) {
      setSubmitButtonText(form.submitButtonText || "Enviar");
      setSuccessMessage(form.successMessage || "Obrigado! Entraremos em contato em breve.");
    }
  }, [form, open]);

  const handleSubmit = () => {
    onSubmit({ submitButtonText, successMessage });
    onOpenChange(false);
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Configurações do Formulário</DialogTitle>
        </DialogHeader>
        <div className="space-y-4 py-4">
          <div className="space-y-2">
            <Label htmlFor="submitButtonText">Texto do Botão de Envio</Label>
            <Input
              id="submitButtonText"
              value={submitButtonText}
              onChange={(e) => setSubmitButtonText(e.target.value)}
              placeholder="Ex: Enviar, Cadastrar, Solicitar..."
              data-testid="input-submit-button-text"
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="successMessage">Mensagem de Sucesso</Label>
            <Textarea
              id="successMessage"
              value={successMessage}
              onChange={(e) => setSuccessMessage(e.target.value)}
              placeholder="Mensagem exibida após envio do formulário..."
              rows={3}
              data-testid="input-success-message"
            />
          </div>
        </div>
        <DialogFooter>
          <Button variant="outline" onClick={() => onOpenChange(false)}>
            Cancelar
          </Button>
          <Button onClick={handleSubmit} data-testid="button-save-settings">
            Salvar Configurações
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}

function FormPreview({
  fields,
  submitButtonText,
  successMessage,
}: {
  fields: any[];
  submitButtonText: string;
  successMessage?: string;
}) {
  if (fields.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center py-12 text-center border-2 border-dashed rounded-lg">
        <Eye className="size-8 text-muted-foreground mb-2" />
        <p className="text-muted-foreground">
          Adicione campos para visualizar a prévia do formulário
        </p>
      </div>
    );
  }

  return (
    <div className="space-y-4 p-4 bg-muted/30 rounded-lg border">
      {fields.map((field: any) => (
        <div key={field.id} className="space-y-2">
          <Label className="flex items-center gap-1">
            {field.label}
            {field.required && <span className="text-destructive">*</span>}
          </Label>
          {renderPreviewField(field)}
        </div>
      ))}
      <Button className="w-full" disabled>
        {submitButtonText}
      </Button>
      {successMessage && (
        <p className="text-xs text-muted-foreground text-center mt-2">
          Após enviar: "{successMessage}"
        </p>
      )}
    </div>
  );
}

function renderPreviewField(field: any) {
  switch (field.type) {
    case "TEXT":
    case "EMAIL":
    case "PHONE":
    case "URL":
    case "NUMBER":
      return (
        <Input
          type={field.type === "NUMBER" ? "number" : field.type === "EMAIL" ? "email" : "text"}
          placeholder={field.placeholder || ""}
          disabled
          className="bg-background"
        />
      );
    case "TEXTAREA":
      return (
        <Textarea
          placeholder={field.placeholder || ""}
          disabled
          rows={3}
          className="bg-background"
        />
      );
    case "DATE":
      return <Input type="date" disabled className="bg-background" />;
    case "CHECKBOX":
      return (
        <div className="flex items-center space-x-2">
          <Checkbox disabled />
          <span className="text-sm text-muted-foreground">
            {field.placeholder || "Marcar opção"}
          </span>
        </div>
      );
    case "SELECT":
      const options = field.options
        ? field.options.split("\n").filter((o: string) => o.trim())
        : [];
      return (
        <Select disabled>
          <SelectTrigger className="bg-background">
            <SelectValue placeholder={field.placeholder || "Selecione..."} />
          </SelectTrigger>
          <SelectContent>
            {options.map((opt: string, idx: number) => (
              <SelectItem key={idx} value={opt}>
                {opt}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
      );
    default:
      return <Input disabled className="bg-background" />;
  }
}

function EmbedModal({
  open,
  onOpenChange,
  form,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  form: any;
}) {
  const [copied, setCopied] = useState(false);
  const [selectedTab, setSelectedTab] = useState<'script' | 'iframe'>('script');

  if (!form) return null;

  const baseUrl = window.location.origin;
  const formId = form.id;

  const scriptCode = `<!-- Dominus Form Embed -->
<div id="dominus-form-container"></div>
<script src="${baseUrl}/api/embed/forms/${formId}/script.js"></script>
<script>
  // Renderizar o formulário
  DominusForm['${formId}'].render('dominus-form-container', {
    theme: 'light',          // 'light' ou 'dark'
    primaryColor: '#6366f1', // Cor do botão
    showDescription: true    // Exibir descrição do formulário
  });
</script>`;

  const iframeCode = `<!-- Dominus Form Embed (iframe) -->
<iframe 
  src="${baseUrl}/embed/form/${formId}" 
  width="100%" 
  height="500" 
  frameborder="0"
  style="border: none; max-width: 600px;"
></iframe>`;

  const autoRenderCode = `<!-- Dominus Form - Auto Render -->
<div id="my-form-container" data-dominus-form="${formId}" data-theme="light" data-primary-color="#6366f1"></div>
<script src="${baseUrl}/api/embed/forms/${formId}/script.js"></script>`;

  const handleCopy = async (code: string) => {
    await navigator.clipboard.writeText(code);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-2xl">
        <DialogHeader>
          <DialogTitle className="flex items-center gap-2">
            <Code className="size-5" />
            Código Embed - {form.name}
          </DialogTitle>
        </DialogHeader>

        <div className="space-y-4">
          <p className="text-sm text-muted-foreground">
            Use o código abaixo para incorporar este formulário em qualquer site externo. 
            Os leads capturados aparecerão automaticamente no seu painel do Dominus.
          </p>

          <div className="flex gap-2 border-b">
            <button
              className={`px-4 py-2 text-sm font-medium border-b-2 transition-colors ${
                selectedTab === 'script'
                  ? 'border-primary text-primary'
                  : 'border-transparent text-muted-foreground hover:text-foreground'
              }`}
              onClick={() => setSelectedTab('script')}
            >
              JavaScript
            </button>
            <button
              className={`px-4 py-2 text-sm font-medium border-b-2 transition-colors ${
                selectedTab === 'iframe'
                  ? 'border-primary text-primary'
                  : 'border-transparent text-muted-foreground hover:text-foreground'
              }`}
              onClick={() => setSelectedTab('iframe')}
            >
              iFrame
            </button>
          </div>

          {selectedTab === 'script' && (
            <div className="space-y-4">
              <div>
                <div className="flex items-center justify-between mb-2">
                  <Label className="text-sm font-medium">Método 1: Renderização Manual</Label>
                  <Button
                    variant="outline"
                    size="sm"
                    onClick={() => handleCopy(scriptCode)}
                    className="gap-2"
                  >
                    {copied ? <Check className="size-3" /> : <Copy className="size-3" />}
                    {copied ? 'Copiado!' : 'Copiar'}
                  </Button>
                </div>
                <pre className="bg-muted p-4 rounded-lg text-xs overflow-x-auto max-h-48 whitespace-pre-wrap break-all">
                  <code className="break-all">{scriptCode}</code>
                </pre>
              </div>

              <div>
                <div className="flex items-center justify-between mb-2">
                  <Label className="text-sm font-medium">Método 2: Auto-renderização</Label>
                  <Button
                    variant="outline"
                    size="sm"
                    onClick={() => handleCopy(autoRenderCode)}
                    className="gap-2"
                  >
                    {copied ? <Check className="size-3" /> : <Copy className="size-3" />}
                    {copied ? 'Copiado!' : 'Copiar'}
                  </Button>
                </div>
                <pre className="bg-muted p-4 rounded-lg text-xs overflow-x-auto max-h-32 whitespace-pre-wrap break-all">
                  <code className="break-all">{autoRenderCode}</code>
                </pre>
              </div>
            </div>
          )}

          {selectedTab === 'iframe' && (
            <div>
              <div className="flex items-center justify-between mb-2">
                <Label className="text-sm font-medium">Código iFrame</Label>
                <Button
                  variant="outline"
                  size="sm"
                  onClick={() => handleCopy(iframeCode)}
                  className="gap-2"
                >
                  {copied ? <Check className="size-3" /> : <Copy className="size-3" />}
                  {copied ? 'Copiado!' : 'Copiar'}
                </Button>
              </div>
              <pre className="bg-muted p-4 rounded-lg text-xs overflow-x-auto whitespace-pre-wrap break-all">
                <code className="break-all">{iframeCode}</code>
              </pre>
              <p className="text-xs text-muted-foreground mt-2">
                O iFrame é mais simples de usar mas oferece menos personalização.
              </p>
            </div>
          )}

          <div className="bg-blue-50 dark:bg-blue-950/30 border border-blue-200 dark:border-blue-900 rounded-lg p-4">
            <h4 className="text-sm font-semibold text-blue-800 dark:text-blue-200 mb-2">
              Opções de Personalização
            </h4>
            <ul className="text-xs text-blue-700 dark:text-blue-300 space-y-1">
              <li><code>theme</code>: 'light' ou 'dark' para ajustar ao tema do site</li>
              <li><code>primaryColor</code>: Cor hexadecimal para o botão de envio</li>
              <li><code>showDescription</code>: true/false para exibir a descrição do formulário</li>
              <li><code>onSuccess</code>: Callback function executada após sucesso</li>
              <li><code>onError</code>: Callback function executada em caso de erro</li>
            </ul>
          </div>
        </div>

        <DialogFooter>
          <Button variant="outline" onClick={() => onOpenChange(false)}>
            Fechar
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
