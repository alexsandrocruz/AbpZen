import { useState } from "react";
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
import { ScrollArea } from "@/components/ui/scroll-area";
import { useToast } from "@/hooks/use-toast";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";
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
  Users,
  GripVertical,
  User,
  Mail,
  Phone,
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
  useDroppable,
} from "@dnd-kit/core";
import {
  SortableContext,
  useSortable,
  verticalListSortingStrategy,
  horizontalListSortingStrategy,
  arrayMove,
} from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";

const STAGE_COLORS = [
  "#6366f1", "#8b5cf6", "#a855f7", "#d946ef", "#ec4899",
  "#f43f5e", "#ef4444", "#f97316", "#f59e0b", "#eab308",
  "#84cc16", "#22c55e", "#10b981", "#14b8a6", "#06b6d4",
  "#0ea5e9", "#3b82f6", "#6366f1", "#64748b", "#1e293b",
];

export default function LeadWorkflowsPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [selectedWorkflow, setSelectedWorkflow] = useState<any>(null);
  const [workflowModalOpen, setWorkflowModalOpen] = useState(false);
  const [editingWorkflow, setEditingWorkflow] = useState<any>(null);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [workflowToDelete, setWorkflowToDelete] = useState<any>(null);
  const [stagesModalOpen, setStagesModalOpen] = useState(false);
  const [leadModalOpen, setLeadModalOpen] = useState(false);
  const [editingLead, setEditingLead] = useState<any>(null);

  const { data: workflows = [], isLoading } = useQuery({
    queryKey: ['lead-workflows', currentWorkspace?.id],
    queryFn: () => api.getLeadWorkflows(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: stages = [], isLoading: stagesLoading } = useQuery({
    queryKey: ['lead-workflow-stages', currentWorkspace?.id, selectedWorkflow?.id],
    queryFn: () => api.getLeadWorkflowStages(currentWorkspace!.id, selectedWorkflow!.id),
    enabled: !!currentWorkspace && !!selectedWorkflow,
  });

  const { data: leads = [], isLoading: leadsLoading } = useQuery({
    queryKey: ['leads', currentWorkspace?.id, selectedWorkflow?.id],
    queryFn: () => api.getLeads(currentWorkspace!.id, { workflowId: selectedWorkflow!.id }),
    enabled: !!currentWorkspace && !!selectedWorkflow,
  });

  const createWorkflowMutation = useMutation({
    mutationFn: (data: any) => api.createLeadWorkflow(currentWorkspace!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['lead-workflows', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Funil criado com sucesso!" });
      setWorkflowModalOpen(false);
      setEditingWorkflow(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateWorkflowMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: any }) =>
      api.updateLeadWorkflow(currentWorkspace!.id, id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['lead-workflows', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Funil atualizado com sucesso!" });
      setWorkflowModalOpen(false);
      setEditingWorkflow(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteWorkflowMutation = useMutation({
    mutationFn: (id: string) => api.deleteLeadWorkflow(currentWorkspace!.id, id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['lead-workflows', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Funil excluído com sucesso!" });
      setDeleteModalOpen(false);
      setWorkflowToDelete(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const createStageMutation = useMutation({
    mutationFn: (data: any) =>
      api.createLeadWorkflowStage(currentWorkspace!.id, selectedWorkflow!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['lead-workflow-stages', currentWorkspace?.id, selectedWorkflow?.id] });
      toast({ title: "Sucesso", description: "Etapa criada com sucesso!" });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateStageMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: any }) =>
      api.updateLeadWorkflowStage(currentWorkspace!.id, selectedWorkflow!.id, id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['lead-workflow-stages', currentWorkspace?.id, selectedWorkflow?.id] });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteStageMutation = useMutation({
    mutationFn: (id: string) =>
      api.deleteLeadWorkflowStage(currentWorkspace!.id, selectedWorkflow!.id, id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['lead-workflow-stages', currentWorkspace?.id, selectedWorkflow?.id] });
      toast({ title: "Sucesso", description: "Etapa excluída com sucesso!" });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const reorderStagesMutation = useMutation({
    mutationFn: (stageIds: string[]) =>
      api.reorderLeadWorkflowStages(currentWorkspace!.id, selectedWorkflow!.id, stageIds),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['lead-workflow-stages', currentWorkspace?.id, selectedWorkflow?.id] });
    },
  });

  const createLeadMutation = useMutation({
    mutationFn: (data: any) => api.createLead(currentWorkspace!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['leads', currentWorkspace?.id, selectedWorkflow?.id] });
      toast({ title: "Sucesso", description: "Lead adicionado com sucesso!" });
      setLeadModalOpen(false);
      setEditingLead(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const moveLeadMutation = useMutation({
    mutationFn: ({ leadId, toStageId }: { leadId: string; toStageId: string }) =>
      api.moveLeadStage(currentWorkspace!.id, leadId, toStageId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['leads', currentWorkspace?.id, selectedWorkflow?.id] });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteLeadMutation = useMutation({
    mutationFn: (id: string) => api.deleteLead(currentWorkspace!.id, id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['leads', currentWorkspace?.id, selectedWorkflow?.id] });
      toast({ title: "Sucesso", description: "Lead excluído com sucesso!" });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleNewWorkflow = () => {
    setEditingWorkflow(null);
    setWorkflowModalOpen(true);
  };

  const handleEditWorkflow = (workflow: any) => {
    setEditingWorkflow(workflow);
    setWorkflowModalOpen(true);
  };

  const handleDeleteWorkflow = (workflow: any) => {
    setWorkflowToDelete(workflow);
    setDeleteModalOpen(true);
  };

  const handleAddLead = () => {
    setEditingLead(null);
    setLeadModalOpen(true);
  };

  const getLeadCount = (workflowId: string) => {
    return 0;
  };

  if (selectedWorkflow) {
    return (
      <AppShell>
        <KanbanView
          workflow={selectedWorkflow}
          stages={stages}
          leads={leads}
          stagesLoading={stagesLoading}
          leadsLoading={leadsLoading}
          onBack={() => setSelectedWorkflow(null)}
          onManageStages={() => setStagesModalOpen(true)}
          onAddLead={handleAddLead}
          onMoveLead={(leadId, toStageId) => moveLeadMutation.mutate({ leadId, toStageId })}
          onDeleteLead={(id) => deleteLeadMutation.mutate(id)}
        />

        <StagesModal
          open={stagesModalOpen}
          onOpenChange={setStagesModalOpen}
          stages={stages}
          onCreateStage={(data) => createStageMutation.mutate(data)}
          onUpdateStage={(id, data) => updateStageMutation.mutate({ id, data })}
          onDeleteStage={(id) => deleteStageMutation.mutate(id)}
          onReorderStages={(stageIds) => reorderStagesMutation.mutate(stageIds)}
        />

        <LeadModal
          open={leadModalOpen}
          onOpenChange={setLeadModalOpen}
          lead={editingLead}
          workflowId={selectedWorkflow.id}
          defaultStageId={stages.find((s: any) => s.isDefault)?.id || stages[0]?.id}
          onSubmit={(data) => createLeadMutation.mutate(data)}
          isLoading={createLeadMutation.isPending}
        />
      </AppShell>
    );
  }

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight">Funis de Leads</h1>
            <p className="text-muted-foreground">Gerencie seus funis de captação e acompanhe seus leads.</p>
          </div>
          <Button className="gap-2" onClick={handleNewWorkflow} data-testid="button-add-workflow">
            <Plus className="size-4" />
            Novo Funil
          </Button>
        </div>

        {isLoading ? (
          <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
            {[...Array(3)].map((_, i) => (
              <Skeleton key={i} className="h-40 rounded-xl" />
            ))}
          </div>
        ) : workflows.length === 0 ? (
          <div className="flex flex-col items-center justify-center py-16 text-center">
            <div className="size-16 rounded-full bg-muted flex items-center justify-center mb-4">
              <Users className="size-8 text-muted-foreground" />
            </div>
            <h3 className="text-lg font-semibold mb-2">Nenhum funil encontrado</h3>
            <p className="text-muted-foreground mb-4">Crie seu primeiro funil para começar a captar leads.</p>
            <Button onClick={handleNewWorkflow}>
              <Plus className="size-4 mr-2" />
              Criar Funil
            </Button>
          </div>
        ) : (
          <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
            {workflows.map((workflow: any) => (
              <TactileCard
                key={workflow.id}
                className="cursor-pointer hover:border-primary/50 transition-colors"
                onClick={() => setSelectedWorkflow(workflow)}
                data-testid={`card-workflow-${workflow.id}`}
              >
                <div className="p-6">
                  <div className="flex items-start justify-between mb-4">
                    <div className="flex-1">
                      <h3 className="font-semibold text-lg" data-testid={`text-workflow-name-${workflow.id}`}>
                        {workflow.name}
                      </h3>
                      {workflow.description && (
                        <p className="text-sm text-muted-foreground mt-1 line-clamp-2">
                          {workflow.description}
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
                            handleEditWorkflow(workflow);
                          }}
                        >
                          <Pencil className="size-4 mr-2" />
                          Editar
                        </DropdownMenuItem>
                        <DropdownMenuItem
                          className="text-destructive"
                          onClick={(e) => {
                            e.stopPropagation();
                            handleDeleteWorkflow(workflow);
                          }}
                        >
                          <Trash2 className="size-4 mr-2" />
                          Excluir
                        </DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </div>
                  <div className="flex items-center gap-3">
                    <Badge variant={workflow.isActive ? "default" : "secondary"}>
                      {workflow.isActive ? "Ativo" : "Inativo"}
                    </Badge>
                    <span className="text-sm text-muted-foreground flex items-center gap-1">
                      <Users className="size-4" />
                      {getLeadCount(workflow.id)} leads
                    </span>
                  </div>
                </div>
              </TactileCard>
            ))}
          </div>
        )}
      </div>

      <WorkflowModal
        open={workflowModalOpen}
        onOpenChange={setWorkflowModalOpen}
        workflow={editingWorkflow}
        onSubmit={(data) => {
          if (editingWorkflow) {
            updateWorkflowMutation.mutate({ id: editingWorkflow.id, data });
          } else {
            createWorkflowMutation.mutate(data);
          }
        }}
        isLoading={createWorkflowMutation.isPending || updateWorkflowMutation.isPending}
      />

      <DeleteModal
        open={deleteModalOpen}
        onOpenChange={setDeleteModalOpen}
        onConfirm={() => deleteWorkflowMutation.mutate(workflowToDelete?.id)}
        title="Excluir Funil"
        description={`Tem certeza que deseja excluir o funil "${workflowToDelete?.name}"? Todos os leads e etapas serão excluídos permanentemente.`}
        isLoading={deleteWorkflowMutation.isPending}
      />
    </AppShell>
  );
}

function WorkflowModal({
  open,
  onOpenChange,
  workflow,
  onSubmit,
  isLoading,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  workflow: any;
  onSubmit: (data: any) => void;
  isLoading: boolean;
}) {
  const [name, setName] = useState(workflow?.name || "");
  const [description, setDescription] = useState(workflow?.description || "");
  const [isActive, setIsActive] = useState(workflow?.isActive ?? true);

  const handleSubmit = () => {
    if (!name.trim()) return;
    onSubmit({ name, description, isActive });
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{workflow ? "Editar Funil" : "Novo Funil"}</DialogTitle>
        </DialogHeader>
        <div className="space-y-4 py-4">
          <div className="space-y-2">
            <Label htmlFor="name">Nome</Label>
            <Input
              id="name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              placeholder="Ex: Funil de Vendas"
              data-testid="input-workflow-name"
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="description">Descrição</Label>
            <Textarea
              id="description"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              placeholder="Descrição do funil..."
              rows={3}
              data-testid="input-workflow-description"
            />
          </div>
          <div className="flex items-center justify-between">
            <Label htmlFor="isActive">Ativo</Label>
            <Switch
              id="isActive"
              checked={isActive}
              onCheckedChange={setIsActive}
              data-testid="switch-workflow-active"
            />
          </div>
        </div>
        <DialogFooter>
          <Button variant="outline" onClick={() => onOpenChange(false)}>
            Cancelar
          </Button>
          <Button onClick={handleSubmit} disabled={isLoading || !name.trim()} data-testid="button-save-workflow">
            {isLoading ? "Salvando..." : "Salvar"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}

function KanbanView({
  workflow,
  stages,
  leads,
  stagesLoading,
  leadsLoading,
  onBack,
  onManageStages,
  onAddLead,
  onMoveLead,
  onDeleteLead,
}: {
  workflow: any;
  stages: any[];
  leads: any[];
  stagesLoading: boolean;
  leadsLoading: boolean;
  onBack: () => void;
  onManageStages: () => void;
  onAddLead: () => void;
  onMoveLead: (leadId: string, toStageId: string) => void;
  onDeleteLead: (leadId: string) => void;
}) {
  const [activeLeadId, setActiveLeadId] = useState<string | null>(null);

  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: {
        distance: 8,
      },
    })
  );

  const handleDragStart = (event: DragStartEvent) => {
    setActiveLeadId(event.active.id as string);
  };

  const handleDragEnd = (event: DragEndEvent) => {
    const { active, over } = event;
    setActiveLeadId(null);

    if (!over) return;

    const leadId = active.id as string;
    const lead = leads.find((l: any) => l.id === leadId);
    if (!lead) return;

    let targetStageId: string | null = null;

    for (const stage of stages) {
      if (over.id === `stage-${stage.id}` || over.id === stage.id) {
        targetStageId = stage.id;
        break;
      }
      const stageLeads = leads.filter((l: any) => l.stageId === stage.id);
      if (stageLeads.some((l: any) => l.id === over.id)) {
        targetStageId = stage.id;
        break;
      }
    }

    if (targetStageId && targetStageId !== lead.stageId) {
      onMoveLead(leadId, targetStageId);
    }
  };

  const activeLead = leads.find((l: any) => l.id === activeLeadId);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <Button variant="ghost" size="icon" onClick={onBack}>
            <ArrowLeft className="size-5" />
          </Button>
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight">{workflow.name}</h1>
            {workflow.description && (
              <p className="text-muted-foreground">{workflow.description}</p>
            )}
          </div>
        </div>
        <div className="flex items-center gap-2">
          <Button variant="outline" onClick={onManageStages} data-testid="button-manage-stages">
            <Settings className="size-4 mr-2" />
            Etapas
          </Button>
          <Button onClick={onAddLead} data-testid="button-add-lead">
            <Plus className="size-4 mr-2" />
            Novo Lead
          </Button>
        </div>
      </div>

      {stagesLoading || leadsLoading ? (
        <div className="flex gap-4 overflow-x-auto pb-4">
          {[...Array(4)].map((_, i) => (
            <Skeleton key={i} className="h-96 w-80 flex-shrink-0 rounded-xl" />
          ))}
        </div>
      ) : stages.length === 0 ? (
        <div className="flex flex-col items-center justify-center py-16 text-center">
          <div className="size-16 rounded-full bg-muted flex items-center justify-center mb-4">
            <Settings className="size-8 text-muted-foreground" />
          </div>
          <h3 className="text-lg font-semibold mb-2">Nenhuma etapa configurada</h3>
          <p className="text-muted-foreground mb-4">Crie etapas para organizar seus leads no funil.</p>
          <Button onClick={onManageStages}>
            <Plus className="size-4 mr-2" />
            Configurar Etapas
          </Button>
        </div>
      ) : (
        <DndContext
          sensors={sensors}
          collisionDetection={closestCenter}
          onDragStart={handleDragStart}
          onDragEnd={handleDragEnd}
        >
          <div className="flex gap-4 overflow-x-auto pb-4">
            {stages
              .sort((a: any, b: any) => a.position - b.position)
              .map((stage: any) => (
                <StageColumn
                  key={stage.id}
                  stage={stage}
                  leads={leads.filter((l: any) => l.stageId === stage.id)}
                  onDeleteLead={onDeleteLead}
                />
              ))}
          </div>
          <DragOverlay>
            {activeLead ? <LeadCardOverlay lead={activeLead} /> : null}
          </DragOverlay>
        </DndContext>
      )}
    </div>
  );
}

function StageColumn({
  stage,
  leads,
  onDeleteLead,
}: {
  stage: any;
  leads: any[];
  onDeleteLead: (id: string) => void;
}) {
  const { setNodeRef, isOver } = useDroppable({
    id: `stage-${stage.id}`,
  });

  return (
    <div
      ref={setNodeRef}
      className={`w-80 flex-shrink-0 rounded-xl bg-muted/50 border transition-colors ${
        isOver ? "border-primary bg-primary/5" : "border-transparent"
      }`}
    >
      <div className="p-4 border-b" style={{ borderColor: stage.color }}>
        <div className="flex items-center gap-2">
          <div className="size-3 rounded-full" style={{ backgroundColor: stage.color }} />
          <h3 className="font-semibold">{stage.name}</h3>
          <Badge variant="secondary" className="ml-auto">
            {leads.length}
          </Badge>
        </div>
      </div>
      <ScrollArea className="h-[calc(100vh-320px)]">
        <div className="p-2 space-y-2">
          <SortableContext items={leads.map((l: any) => l.id)} strategy={verticalListSortingStrategy}>
            {leads.map((lead: any) => (
              <LeadCard key={lead.id} lead={lead} onDelete={() => onDeleteLead(lead.id)} />
            ))}
          </SortableContext>
        </div>
      </ScrollArea>
    </div>
  );
}

function LeadCard({ lead, onDelete }: { lead: any; onDelete: () => void }) {
  const { attributes, listeners, setNodeRef, transform, transition, isDragging } = useSortable({
    id: lead.id,
  });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
  };

  return (
    <div
      ref={setNodeRef}
      style={style}
      {...attributes}
      {...listeners}
      className="bg-card rounded-lg border p-3 cursor-grab active:cursor-grabbing hover:border-primary/50 transition-colors"
      data-testid={`card-lead-${lead.id}`}
    >
      <div className="flex items-start justify-between gap-2">
        <div className="flex-1 min-w-0">
          <div className="flex items-center gap-2 mb-1">
            <User className="size-4 text-muted-foreground flex-shrink-0" />
            <span className="font-medium truncate">{lead.name}</span>
          </div>
          {lead.email && (
            <div className="flex items-center gap-2 text-sm text-muted-foreground">
              <Mail className="size-3 flex-shrink-0" />
              <span className="truncate">{lead.email}</span>
            </div>
          )}
          {lead.phone && (
            <div className="flex items-center gap-2 text-sm text-muted-foreground">
              <Phone className="size-3 flex-shrink-0" />
              <span className="truncate">{lead.phone}</span>
            </div>
          )}
        </div>
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="ghost" size="icon" className="size-6 flex-shrink-0">
              <MoreVertical className="size-3" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuItem className="text-destructive" onClick={onDelete}>
              <Trash2 className="size-4 mr-2" />
              Excluir
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
    </div>
  );
}

function LeadCardOverlay({ lead }: { lead: any }) {
  return (
    <div className="bg-card rounded-lg border p-3 shadow-lg w-72">
      <div className="flex items-center gap-2 mb-1">
        <User className="size-4 text-muted-foreground" />
        <span className="font-medium">{lead.name}</span>
      </div>
      {lead.email && (
        <div className="flex items-center gap-2 text-sm text-muted-foreground">
          <Mail className="size-3" />
          <span>{lead.email}</span>
        </div>
      )}
    </div>
  );
}

function StagesModal({
  open,
  onOpenChange,
  stages,
  onCreateStage,
  onUpdateStage,
  onDeleteStage,
  onReorderStages,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  stages: any[];
  onCreateStage: (data: any) => void;
  onUpdateStage: (id: string, data: any) => void;
  onDeleteStage: (id: string) => void;
  onReorderStages: (stageIds: string[]) => void;
}) {
  const [newStageName, setNewStageName] = useState("");
  const [newStageColor, setNewStageColor] = useState(STAGE_COLORS[0]);
  const [localStages, setLocalStages] = useState<any[]>([]);

  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: {
        distance: 8,
      },
    })
  );

  useState(() => {
    setLocalStages(stages.sort((a, b) => a.position - b.position));
  });

  const handleAddStage = () => {
    if (!newStageName.trim()) return;
    onCreateStage({ name: newStageName, color: newStageColor, position: stages.length });
    setNewStageName("");
    setNewStageColor(STAGE_COLORS[(stages.length + 1) % STAGE_COLORS.length]);
  };

  const handleDragEnd = (event: DragEndEvent) => {
    const { active, over } = event;
    if (!over || active.id === over.id) return;

    const oldIndex = localStages.findIndex((s) => s.id === active.id);
    const newIndex = localStages.findIndex((s) => s.id === over.id);
    const reordered = arrayMove(localStages, oldIndex, newIndex);
    setLocalStages(reordered);
    onReorderStages(reordered.map((s) => s.id));
  };

  const sortedStages = stages.sort((a, b) => a.position - b.position);

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-lg">
        <DialogHeader>
          <DialogTitle>Gerenciar Etapas</DialogTitle>
        </DialogHeader>
        <div className="space-y-4 py-4">
          <div className="flex gap-2">
            <Input
              value={newStageName}
              onChange={(e) => setNewStageName(e.target.value)}
              placeholder="Nome da etapa..."
              className="flex-1"
              data-testid="input-stage-name"
            />
            <div className="relative">
              <input
                type="color"
                value={newStageColor}
                onChange={(e) => setNewStageColor(e.target.value)}
                className="absolute inset-0 opacity-0 cursor-pointer"
              />
              <div
                className="size-10 rounded-md border cursor-pointer"
                style={{ backgroundColor: newStageColor }}
              />
            </div>
            <Button onClick={handleAddStage} disabled={!newStageName.trim()} data-testid="button-add-stage">
              <Plus className="size-4" />
            </Button>
          </div>

          <DndContext sensors={sensors} collisionDetection={closestCenter} onDragEnd={handleDragEnd}>
            <SortableContext items={sortedStages.map((s: any) => s.id)} strategy={verticalListSortingStrategy}>
              <div className="space-y-2">
                {sortedStages.map((stage: any) => (
                  <SortableStageItem
                    key={stage.id}
                    stage={stage}
                    onUpdate={(data) => onUpdateStage(stage.id, data)}
                    onDelete={() => onDeleteStage(stage.id)}
                  />
                ))}
              </div>
            </SortableContext>
          </DndContext>
        </div>
        <DialogFooter>
          <Button onClick={() => onOpenChange(false)}>Fechar</Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}

function SortableStageItem({
  stage,
  onUpdate,
  onDelete,
}: {
  stage: any;
  onUpdate: (data: any) => void;
  onDelete: () => void;
}) {
  const [name, setName] = useState(stage.name);
  const [color, setColor] = useState(stage.color);
  const { attributes, listeners, setNodeRef, transform, transition, isDragging } = useSortable({
    id: stage.id,
  });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
  };

  const handleBlur = () => {
    if (name !== stage.name || color !== stage.color) {
      onUpdate({ name, color });
    }
  };

  return (
    <div
      ref={setNodeRef}
      style={style}
      className="flex items-center gap-2 bg-muted/50 rounded-lg p-2"
    >
      <button {...attributes} {...listeners} className="cursor-grab active:cursor-grabbing">
        <GripVertical className="size-4 text-muted-foreground" />
      </button>
      <div className="relative">
        <input
          type="color"
          value={color}
          onChange={(e) => setColor(e.target.value)}
          onBlur={handleBlur}
          className="absolute inset-0 opacity-0 cursor-pointer"
        />
        <div
          className="size-6 rounded-md border cursor-pointer"
          style={{ backgroundColor: color }}
        />
      </div>
      <Input
        value={name}
        onChange={(e) => setName(e.target.value)}
        onBlur={handleBlur}
        className="flex-1"
      />
      <Button variant="ghost" size="icon" onClick={onDelete}>
        <Trash2 className="size-4 text-destructive" />
      </Button>
    </div>
  );
}

function LeadModal({
  open,
  onOpenChange,
  lead,
  workflowId,
  defaultStageId,
  onSubmit,
  isLoading,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  lead: any;
  workflowId: string;
  defaultStageId: string;
  onSubmit: (data: any) => void;
  isLoading: boolean;
}) {
  const [name, setName] = useState(lead?.name || "");
  const [email, setEmail] = useState(lead?.email || "");
  const [phone, setPhone] = useState(lead?.phone || "");

  const handleSubmit = () => {
    if (!name.trim()) return;
    onSubmit({
      workflowId,
      stageId: defaultStageId,
      name,
      email: email || null,
      phone: phone || null,
      status: "NEW",
      source: "DIRECT",
    });
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{lead ? "Editar Lead" : "Novo Lead"}</DialogTitle>
        </DialogHeader>
        <div className="space-y-4 py-4">
          <div className="space-y-2">
            <Label htmlFor="leadName">Nome *</Label>
            <Input
              id="leadName"
              value={name}
              onChange={(e) => setName(e.target.value)}
              placeholder="Nome do lead"
              data-testid="input-lead-name"
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="leadEmail">E-mail</Label>
            <Input
              id="leadEmail"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="email@exemplo.com"
              data-testid="input-lead-email"
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="leadPhone">Telefone</Label>
            <Input
              id="leadPhone"
              value={phone}
              onChange={(e) => setPhone(e.target.value)}
              placeholder="(11) 99999-9999"
              data-testid="input-lead-phone"
            />
          </div>
        </div>
        <DialogFooter>
          <Button variant="outline" onClick={() => onOpenChange(false)}>
            Cancelar
          </Button>
          <Button onClick={handleSubmit} disabled={isLoading || !name.trim()} data-testid="button-save-lead">
            {isLoading ? "Salvando..." : "Salvar"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
