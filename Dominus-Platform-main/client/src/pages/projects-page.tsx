import { useState } from "react";
import { Link } from "wouter";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Progress } from "@/components/ui/progress";
import { Skeleton } from "@/components/ui/skeleton";
import { Plus, Calendar, DollarSign, MoreVertical, Pencil, Trash2, GripVertical, ExternalLink } from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import { ProjectModal } from "@/components/modals/project-modal";
import { DeleteModal } from "@/components/modals/delete-modal";
import { useToast } from "@/hooks/use-toast";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
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
  arrayMove,
} from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";

export default function ProjectsPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [modalOpen, setModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [selectedProject, setSelectedProject] = useState<any>(null);
  const [activeProject, setActiveProject] = useState<any>(null);

  const { data: projects = [], isLoading } = useQuery({
    queryKey: ['projects', currentWorkspace?.id],
    queryFn: () => api.getProjects(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: clients = [] } = useQuery({
    queryKey: ['clients', currentWorkspace?.id],
    queryFn: () => api.getClients(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const deleteMutation = useMutation({
    mutationFn: (projectId: string) => api.deleteProject(currentWorkspace!.id, projectId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['projects', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Projeto excluído com sucesso!" });
      setDeleteModalOpen(false);
      setSelectedProject(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const reorderMutation = useMutation({
    mutationFn: ({ projectIds, status }: { projectIds: string[], status: string }) => 
      api.reorderProjects(currentWorkspace!.id, projectIds, status),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['projects', currentWorkspace?.id] });
    },
  });

  const getClientName = (id: string) => clients.find((c: any) => c.id === id)?.companyName || 'Cliente Desconhecido';

  const statusMap: Record<string, string> = {
    'IN_PROGRESS': 'Em Andamento',
    'COMPLETED': 'Concluído',
    'PENDING': 'Pendente',
  };

  const handleEdit = (project: any) => {
    setSelectedProject(project);
    setModalOpen(true);
  };

  const handleDelete = (project: any) => {
    setSelectedProject(project);
    setDeleteModalOpen(true);
  };

  const handleNewProject = () => {
    setSelectedProject(null);
    setModalOpen(true);
  };

  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: {
        distance: 8,
      },
    })
  );

  const pendingProjects = projects.filter((p: any) => p.status === 'PENDING');
  const inProgressProjects = projects.filter((p: any) => p.status === 'IN_PROGRESS');
  const completedProjects = projects.filter((p: any) => p.status === 'COMPLETED');

  const handleDragStart = (event: DragStartEvent) => {
    const project = projects.find((p: any) => p.id === event.active.id);
    setActiveProject(project);
  };

  const handleDragEnd = (event: DragEndEvent) => {
    const { active, over } = event;
    setActiveProject(null);

    if (!over) return;

    const activeProjectData = projects.find((p: any) => p.id === active.id);
    if (!activeProjectData) return;

    let targetList: any[];
    let targetStatus: string;

    const isOverPending = over.id === 'pending-droppable' || pendingProjects.some((p: any) => p.id === over.id);
    const isOverInProgress = over.id === 'in-progress-droppable' || inProgressProjects.some((p: any) => p.id === over.id);
    const isOverCompleted = over.id === 'completed-droppable' || completedProjects.some((p: any) => p.id === over.id);

    if (isOverPending) {
      targetList = [...pendingProjects];
      targetStatus = 'PENDING';
    } else if (isOverInProgress) {
      targetList = [...inProgressProjects];
      targetStatus = 'IN_PROGRESS';
    } else if (isOverCompleted) {
      targetList = [...completedProjects];
      targetStatus = 'COMPLETED';
    } else {
      return;
    }

    const isMovingBetweenColumns = activeProjectData.status !== targetStatus;

    if (isMovingBetweenColumns) {
      const overProject = targetList.find(p => p.id === over.id);
      const insertIndex = overProject ? targetList.findIndex(p => p.id === over.id) : targetList.length;
      const newList = [...targetList];
      newList.splice(insertIndex, 0, activeProjectData);
      reorderMutation.mutate({ 
        projectIds: newList.map(p => p.id), 
        status: targetStatus 
      });
    } else {
      const oldIndex = targetList.findIndex(p => p.id === active.id);
      const overProject = targetList.find(p => p.id === over.id);
      const newIndex = overProject ? targetList.findIndex(p => p.id === over.id) : targetList.length;
      
      if (oldIndex !== -1 && oldIndex !== newIndex) {
        const reordered = arrayMove(targetList, oldIndex, newIndex);
        reorderMutation.mutate({ 
          projectIds: reordered.map(p => p.id), 
          status: targetStatus 
        });
      }
    }
  };

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight">Projetos</h1>
            <p className="text-muted-foreground">Arraste projetos entre colunas para alterar o status.</p>
          </div>
          <Button className="gap-2" onClick={handleNewProject} data-testid="button-add-project">
            <Plus className="size-4" />
            Novo Projeto
          </Button>
        </div>

        {isLoading ? (
          <div className="grid md:grid-cols-3 gap-6">
            {[...Array(3)].map((_, i) => (
              <div key={i} className="space-y-4">
                <Skeleton className="h-8 w-32" />
                {[...Array(2)].map((_, j) => (
                  <Skeleton key={j} className="h-32 rounded-xl" />
                ))}
              </div>
            ))}
          </div>
        ) : (
          <DndContext
            sensors={sensors}
            collisionDetection={closestCenter}
            onDragStart={handleDragStart}
            onDragEnd={handleDragEnd}
          >
            <div className="grid md:grid-cols-3 gap-6">
              <ProjectColumn
                id="pending-droppable"
                title="Pendente"
                color="bg-orange-500"
                projects={pendingProjects}
                clients={clients}
                workspaceSlug={currentWorkspace?.slug || ''}
                onEdit={handleEdit}
                onDelete={handleDelete}
              />
              <ProjectColumn
                id="in-progress-droppable"
                title="Em Andamento"
                color="bg-blue-500"
                projects={inProgressProjects}
                clients={clients}
                workspaceSlug={currentWorkspace?.slug || ''}
                onEdit={handleEdit}
                onDelete={handleDelete}
              />
              <ProjectColumn
                id="completed-droppable"
                title="Concluído"
                color="bg-green-500"
                projects={completedProjects}
                clients={clients}
                workspaceSlug={currentWorkspace?.slug || ''}
                onEdit={handleEdit}
                onDelete={handleDelete}
              />
            </div>
            <DragOverlay>
              {activeProject ? (
                <ProjectCardOverlay project={activeProject} clientName={getClientName(activeProject.clientId)} />
              ) : null}
            </DragOverlay>
          </DndContext>
        )}
      </div>

      <ProjectModal
        open={modalOpen}
        onOpenChange={setModalOpen}
        project={selectedProject}
      />

      <DeleteModal
        open={deleteModalOpen}
        onOpenChange={setDeleteModalOpen}
        onConfirm={() => deleteMutation.mutate(selectedProject?.id)}
        title="Excluir Projeto"
        description={`Tem certeza que deseja excluir "${selectedProject?.title}"? Todas as tarefas relacionadas também serão excluídas.`}
        isLoading={deleteMutation.isPending}
      />
    </AppShell>
  );
}

interface ProjectColumnProps {
  id: string;
  title: string;
  color: string;
  projects: any[];
  clients: any[];
  workspaceSlug: string;
  onEdit: (project: any) => void;
  onDelete: (project: any) => void;
}

function ProjectColumn({ id, title, color, projects, clients, workspaceSlug, onEdit, onDelete }: ProjectColumnProps) {
  const { setNodeRef, isOver } = useDroppable({ id });
  const getClientName = (clientId: string) => clients.find((c: any) => c.id === clientId)?.companyName || 'Desconhecido';

  return (
    <div 
      ref={setNodeRef} 
      className={`space-y-4 min-h-[300px] p-4 rounded-xl transition-colors ${
        isOver ? 'bg-primary/5 ring-2 ring-primary/20' : ''
      }`}
    >
      <h2 className="text-lg font-semibold flex items-center gap-2">
        <div className={`size-2 rounded-full ${color}`} />
        {title} ({projects.length})
      </h2>
      <SortableContext items={projects.map(p => p.id)} strategy={verticalListSortingStrategy}>
        <div className="space-y-4">
          {projects.map((project: any) => (
            <SortableProjectCard
              key={project.id}
              project={project}
              clientName={getClientName(project.clientId)}
              workspaceSlug={workspaceSlug}
              onEdit={() => onEdit(project)}
              onDelete={() => onDelete(project)}
            />
          ))}
          {projects.length === 0 && (
            <div className="py-12 text-center text-muted-foreground border-2 border-dashed rounded-xl text-sm">
              Arraste projetos aqui
            </div>
          )}
        </div>
      </SortableContext>
    </div>
  );
}

interface SortableProjectCardProps {
  project: any;
  clientName: string;
  workspaceSlug: string;
  onEdit: () => void;
  onDelete: () => void;
}

function SortableProjectCard({ project, clientName, workspaceSlug, onEdit, onDelete }: SortableProjectCardProps) {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({ id: project.id });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
  };

  return (
    <div ref={setNodeRef} style={style}>
      <TactileCard className="p-4 space-y-3 group" data-testid={`card-project-${project.id}`}>
        <div className="flex items-start gap-2">
          <button
            {...attributes}
            {...listeners}
            className="mt-1 cursor-grab active:cursor-grabbing text-muted-foreground hover:text-foreground touch-none"
          >
            <GripVertical className="size-4" />
          </button>
          <div className="flex-1">
            <div className="flex items-center gap-2">
              <Link href={`/${workspaceSlug}/projects/${project.id}`}>
                <h3 className="font-semibold text-sm hover:text-primary hover:underline cursor-pointer">{project.title}</h3>
              </Link>
            </div>
            <p className="text-xs text-muted-foreground mt-1">
              {clientName}
            </p>
          </div>
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <Button variant="ghost" size="icon" className="size-8 opacity-0 group-hover:opacity-100 transition-opacity">
                <MoreVertical className="size-4" />
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent align="end">
              <Link href={`/${workspaceSlug}/projects/${project.id}`}>
                <DropdownMenuItem>
                  <ExternalLink className="size-4 mr-2" />
                  Ver Detalhes
                </DropdownMenuItem>
              </Link>
              <DropdownMenuItem onClick={onEdit}>
                <Pencil className="size-4 mr-2" />
                Editar
              </DropdownMenuItem>
              <DropdownMenuItem onClick={onDelete} className="text-destructive">
                <Trash2 className="size-4 mr-2" />
                Excluir
              </DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
        </div>

        <div className="flex items-center gap-4 text-xs text-muted-foreground">
          {project.dueDate && (
            <div className="flex items-center gap-1">
              <Calendar className="size-3" />
              {format(new Date(project.dueDate), "d MMM", { locale: ptBR })}
            </div>
          )}
          {project.budget && (
            <div className="flex items-center gap-1">
              <DollarSign className="size-3" />
              R$ {parseFloat(project.budget).toLocaleString('pt-BR')}
            </div>
          )}
        </div>

        <div className="space-y-1">
          <div className="flex justify-between text-xs text-muted-foreground">
            <span>Progresso</span>
            <span>45%</span>
          </div>
          <Progress value={45} className="h-1.5" />
        </div>
      </TactileCard>
    </div>
  );
}

function ProjectCardOverlay({ project, clientName }: { project: any; clientName: string }) {
  return (
    <TactileCard className="p-4 space-y-3 shadow-xl rotate-2 border-primary">
      <div className="flex items-start gap-2">
        <GripVertical className="size-4 mt-1 text-muted-foreground" />
        <div className="flex-1">
          <h3 className="font-semibold text-sm">{project.title}</h3>
          <p className="text-xs text-muted-foreground mt-1">{clientName}</p>
        </div>
      </div>
    </TactileCard>
  );
}
