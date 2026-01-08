import { useState, useMemo } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
import { Skeleton } from "@/components/ui/skeleton";
import { Badge } from "@/components/ui/badge";
import { Textarea } from "@/components/ui/textarea";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Plus, Calendar, User as UserIcon, MoreHorizontal, Pencil, Trash2, GripVertical, Sparkles, Loader2, ArrowUpDown, Clock } from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import { TaskModal } from "@/components/modals/task-modal";
import { DeleteModal } from "@/components/modals/delete-modal";
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

export default function TasksPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [modalOpen, setModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [selectedTask, setSelectedTask] = useState<any>(null);
  const [activeTask, setActiveTask] = useState<any>(null);
  const [sortOrder, setSortOrder] = useState<"newest" | "oldest">("newest");
  const [showGenerateModal, setShowGenerateModal] = useState(false);
  const [generateText, setGenerateText] = useState("");
  const [parsedTasks, setParsedTasks] = useState<any[]>([]);
  const [selectedGeneratedTasks, setSelectedGeneratedTasks] = useState<Set<number>>(new Set());
  const [isParsing, setIsParsing] = useState(false);
  const [isCreatingBatch, setIsCreatingBatch] = useState(false);

  const { data: tasks = [], isLoading } = useQuery({
    queryKey: ['tasks', currentWorkspace?.id],
    queryFn: () => api.getTasks(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const toggleMutation = useMutation({
    mutationFn: (taskId: string) => api.toggleTask(currentWorkspace!.id, taskId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['tasks', currentWorkspace?.id] });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: (taskId: string) => api.deleteTask(currentWorkspace!.id, taskId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['tasks', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Tarefa excluída com sucesso!" });
      setDeleteModalOpen(false);
      setSelectedTask(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const reorderMutation = useMutation({
    mutationFn: ({ taskIds, isCompleted }: { taskIds: string[], isCompleted: boolean }) => 
      api.reorderTasks(currentWorkspace!.id, taskIds, isCompleted),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['tasks', currentWorkspace?.id] });
    },
  });

  const sortTasks = (taskList: any[]) => {
    return [...taskList].sort((a, b) => {
      const dateA = new Date(a.createdAt).getTime();
      const dateB = new Date(b.createdAt).getTime();
      return sortOrder === "newest" ? dateB - dateA : dateA - dateB;
    });
  };

  const pendingTasks = useMemo(() => sortTasks(tasks.filter((t: any) => !t.isCompleted)), [tasks, sortOrder]);
  const completedTasks = useMemo(() => sortTasks(tasks.filter((t: any) => t.isCompleted)), [tasks, sortOrder]);

  const handleParseText = async () => {
    if (!generateText.trim() || !currentWorkspace) return;
    setIsParsing(true);
    try {
      const result = await api.parseTasksFromText(currentWorkspace.id, generateText);
      setParsedTasks(result.tasks || []);
      setSelectedGeneratedTasks(new Set(result.tasks?.map((_: any, i: number) => i) || []));
    } catch (error: any) {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    } finally {
      setIsParsing(false);
    }
  };

  const handleCreateBatchTasks = async () => {
    if (!currentWorkspace || selectedGeneratedTasks.size === 0) return;
    setIsCreatingBatch(true);
    try {
      const tasksToCreate = parsedTasks
        .filter((_, i) => selectedGeneratedTasks.has(i))
        .map(task => ({
          title: task.title,
          description: task.description,
          priority: task.priority,
        }));
      await api.batchCreateTasks(currentWorkspace.id, tasksToCreate);
      toast({ title: "Sucesso", description: `${tasksToCreate.length} tarefas criadas!` });
      queryClient.invalidateQueries({ queryKey: ['tasks', currentWorkspace.id] });
      setShowGenerateModal(false);
      setParsedTasks([]);
      setSelectedGeneratedTasks(new Set());
      setGenerateText("");
    } catch (error: any) {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    } finally {
      setIsCreatingBatch(false);
    }
  };

  const toggleTaskSelection = (index: number) => {
    const newSelected = new Set(selectedGeneratedTasks);
    if (newSelected.has(index)) {
      newSelected.delete(index);
    } else {
      newSelected.add(index);
    }
    setSelectedGeneratedTasks(newSelected);
  };

  const removeTaskFromList = (index: number) => {
    setParsedTasks(parsedTasks.filter((_, i) => i !== index));
    const newSelected = new Set<number>();
    selectedGeneratedTasks.forEach(i => {
      if (i < index) newSelected.add(i);
      else if (i > index) newSelected.add(i - 1);
    });
    setSelectedGeneratedTasks(newSelected);
  };

  const handleEdit = (task: any) => {
    setSelectedTask(task);
    setModalOpen(true);
  };

  const handleDelete = (task: any) => {
    setSelectedTask(task);
    setDeleteModalOpen(true);
  };

  const handleNewTask = () => {
    setSelectedTask(null);
    setModalOpen(true);
  };

  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: {
        distance: 8,
      },
    })
  );

  const handleDragStart = (event: DragStartEvent) => {
    const task = tasks.find((t: any) => t.id === event.active.id);
    setActiveTask(task);
  };

  const handleDragEnd = (event: DragEndEvent) => {
    const { active, over } = event;
    setActiveTask(null);

    if (!over) return;

    const activeTaskData = tasks.find((t: any) => t.id === active.id);
    if (!activeTaskData) return;

    const isOverPending = over.id === 'pending-droppable' || pendingTasks.some((t: any) => t.id === over.id);
    const isOverCompleted = over.id === 'completed-droppable' || completedTasks.some((t: any) => t.id === over.id);

    let targetList: any[];
    let targetIsCompleted: boolean;

    if (isOverPending) {
      targetList = [...pendingTasks];
      targetIsCompleted = false;
    } else if (isOverCompleted) {
      targetList = [...completedTasks];
      targetIsCompleted = true;
    } else {
      return;
    }

    const isMovingBetweenColumns = activeTaskData.isCompleted !== targetIsCompleted;
    
    if (isMovingBetweenColumns) {
      const overTask = targetList.find(t => t.id === over.id);
      const insertIndex = overTask ? targetList.findIndex(t => t.id === over.id) : targetList.length;
      const newList = [...targetList];
      newList.splice(insertIndex, 0, activeTaskData);
      reorderMutation.mutate({ 
        taskIds: newList.map(t => t.id), 
        isCompleted: targetIsCompleted 
      });
    } else {
      const oldIndex = targetList.findIndex(t => t.id === active.id);
      const overTask = targetList.find(t => t.id === over.id);
      const newIndex = overTask ? targetList.findIndex(t => t.id === over.id) : targetList.length;
      
      if (oldIndex !== -1 && oldIndex !== newIndex) {
        const reordered = arrayMove(targetList, oldIndex, newIndex);
        reorderMutation.mutate({ 
          taskIds: reordered.map(t => t.id), 
          isCompleted: targetIsCompleted 
        });
      }
    }
  };

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight">Tarefas</h1>
            <p className="text-muted-foreground">Arraste tarefas entre colunas para alterar o status.</p>
          </div>
          <div className="flex items-center gap-2">
            <Select value={sortOrder} onValueChange={(v: "newest" | "oldest") => setSortOrder(v)}>
              <SelectTrigger className="w-[160px]" data-testid="select-sort-order">
                <ArrowUpDown className="size-4 mr-2" />
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="newest">Mais recentes</SelectItem>
                <SelectItem value="oldest">Mais antigas</SelectItem>
              </SelectContent>
            </Select>
            <Button variant="outline" className="gap-2" onClick={() => setShowGenerateModal(true)} data-testid="button-generate-tasks">
              <Sparkles className="size-4" />
              Gerar do Texto
            </Button>
            <Button className="gap-2" onClick={handleNewTask} data-testid="button-add-task">
              <Plus className="size-4" />
              Adicionar Tarefa
            </Button>
          </div>
        </div>

        {isLoading ? (
          <div className="grid md:grid-cols-2 gap-8">
            {[...Array(2)].map((_, colIdx) => (
              <div key={colIdx} className="space-y-4">
                <Skeleton className="h-8 w-32" />
                {[...Array(3)].map((_, i) => (
                  <Skeleton key={i} className="h-20 rounded-xl" />
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
            <div className="grid md:grid-cols-2 gap-8">
              <TaskColumn
                id="pending-droppable"
                title="A Fazer"
                count={pendingTasks.length}
                color="bg-orange-500"
                tasks={pendingTasks}
                onToggle={(id) => toggleMutation.mutate(id)}
                onEdit={handleEdit}
                onDelete={handleDelete}
              />
              <TaskColumn
                id="completed-droppable"
                title="Concluídas"
                count={completedTasks.length}
                color="bg-green-500"
                tasks={completedTasks}
                onToggle={(id) => toggleMutation.mutate(id)}
                onEdit={handleEdit}
                onDelete={handleDelete}
                isCompleted
              />
            </div>
            <DragOverlay>
              {activeTask ? (
                <TaskItemOverlay task={activeTask} />
              ) : null}
            </DragOverlay>
          </DndContext>
        )}
      </div>

      <TaskModal
        open={modalOpen}
        onOpenChange={setModalOpen}
        task={selectedTask}
      />

      <DeleteModal
        open={deleteModalOpen}
        onOpenChange={setDeleteModalOpen}
        onConfirm={() => deleteMutation.mutate(selectedTask?.id)}
        title="Excluir Tarefa"
        description={`Tem certeza que deseja excluir "${selectedTask?.title}"?`}
        isLoading={deleteMutation.isPending}
      />

      <Dialog open={showGenerateModal} onOpenChange={(open) => {
        setShowGenerateModal(open);
        if (!open) {
          setParsedTasks([]);
          setSelectedGeneratedTasks(new Set());
          setGenerateText("");
        }
      }}>
        <DialogContent className="max-w-2xl max-h-[80vh] overflow-hidden flex flex-col">
          <DialogHeader>
            <DialogTitle className="flex items-center gap-2">
              <Sparkles className="size-5" />
              Gerar Tarefas do Texto
            </DialogTitle>
          </DialogHeader>
          
          <div className="flex-1 overflow-auto space-y-4">
            {parsedTasks.length === 0 ? (
              <div className="space-y-4">
                <p className="text-sm text-muted-foreground">
                  Cole um texto (de uma conversa com IA, plano de projeto, etc.) e vamos extrair as tarefas automaticamente.
                </p>
                <Textarea
                  value={generateText}
                  onChange={(e) => setGenerateText(e.target.value)}
                  placeholder="Cole aqui o texto do ChatGPT, plano de projeto, ou qualquer texto com passos/tarefas..."
                  rows={12}
                  className="resize-none"
                  data-testid="textarea-generate-tasks-input"
                />
              </div>
            ) : (
              <div className="space-y-4">
                <div className="flex items-center justify-between">
                  <p className="text-sm font-medium">
                    {parsedTasks.length} tarefas encontradas • {selectedGeneratedTasks.size} selecionadas
                  </p>
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => {
                      setParsedTasks([]);
                      setSelectedGeneratedTasks(new Set());
                    }}
                  >
                    Voltar
                  </Button>
                </div>
                <ScrollArea className="h-[300px] pr-4">
                  <div className="space-y-2">
                    {parsedTasks.map((task, index) => (
                      <div
                        key={index}
                        className={`p-3 rounded-lg border ${
                          selectedGeneratedTasks.has(index) ? "border-primary bg-primary/5" : "border-border"
                        }`}
                      >
                        <div className="flex items-start gap-3">
                          <Checkbox
                            checked={selectedGeneratedTasks.has(index)}
                            onCheckedChange={() => toggleTaskSelection(index)}
                            data-testid={`checkbox-generated-task-${index}`}
                          />
                          <div className="flex-1 min-w-0">
                            <p className="font-medium text-sm">{task.title}</p>
                            {task.description && (
                              <p className="text-xs text-muted-foreground mt-1 line-clamp-2">
                                {task.description}
                              </p>
                            )}
                            <div className="flex items-center gap-2 mt-2">
                              {task.priority && (
                                <Badge variant="outline" className="text-xs">
                                  {task.priority === "HIGH" ? "Alta" : task.priority === "MEDIUM" ? "Média" : "Baixa"}
                                </Badge>
                              )}
                              {task.estimatedMinutes && (
                                <span className="text-xs text-muted-foreground flex items-center gap-1">
                                  <Clock className="size-3" />
                                  {task.estimatedMinutes}min
                                </span>
                              )}
                            </div>
                          </div>
                          <Button
                            variant="ghost"
                            size="icon"
                            className="size-6 shrink-0"
                            onClick={() => removeTaskFromList(index)}
                          >
                            <Trash2 className="size-3" />
                          </Button>
                        </div>
                      </div>
                    ))}
                  </div>
                </ScrollArea>
              </div>
            )}
          </div>

          <DialogFooter>
            <Button variant="outline" onClick={() => setShowGenerateModal(false)}>
              Cancelar
            </Button>
            {parsedTasks.length === 0 ? (
              <Button onClick={handleParseText} disabled={!generateText.trim() || isParsing} data-testid="button-analyze-text">
                {isParsing ? (
                  <>
                    <Loader2 className="size-4 mr-2 animate-spin" />
                    Analisando...
                  </>
                ) : (
                  <>
                    <Sparkles className="size-4 mr-2" />
                    Analisar Texto
                  </>
                )}
              </Button>
            ) : (
              <Button onClick={handleCreateBatchTasks} disabled={selectedGeneratedTasks.size === 0 || isCreatingBatch} data-testid="button-create-generated-tasks">
                {isCreatingBatch ? (
                  <>
                    <Loader2 className="size-4 mr-2 animate-spin" />
                    Criando...
                  </>
                ) : (
                  <>
                    <Plus className="size-4 mr-2" />
                    Criar {selectedGeneratedTasks.size} Tarefas
                  </>
                )}
              </Button>
            )}
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </AppShell>
  );
}

interface TaskColumnProps {
  id: string;
  title: string;
  count: number;
  color: string;
  tasks: any[];
  onToggle: (id: string) => void;
  onEdit: (task: any) => void;
  onDelete: (task: any) => void;
  isCompleted?: boolean;
}

function TaskColumn({ id, title, count, color, tasks, onToggle, onEdit, onDelete, isCompleted }: TaskColumnProps) {
  const { setNodeRef, isOver } = useDroppable({ id });

  return (
    <div 
      ref={setNodeRef}
      className={`space-y-4 min-h-[200px] p-4 rounded-xl transition-colors ${
        isOver ? 'bg-primary/5 ring-2 ring-primary/20' : ''
      } ${isCompleted ? 'opacity-70 hover:opacity-100' : ''}`}
    >
      <h2 className="text-lg font-semibold flex items-center gap-2">
        <div className={`size-2 rounded-full ${color}`} />
        {title} ({count})
      </h2>
      <SortableContext items={tasks.map(t => t.id)} strategy={verticalListSortingStrategy}>
        <div className="space-y-3">
          {tasks.map((task: any) => (
            <SortableTaskItem
              key={task.id}
              task={task}
              onToggle={() => onToggle(task.id)}
              onEdit={() => onEdit(task)}
              onDelete={() => onDelete(task)}
            />
          ))}
          {tasks.length === 0 && (
            <div className="py-8 text-center text-muted-foreground border-2 border-dashed rounded-xl">
              {isCompleted ? 'Nenhuma tarefa concluída.' : 'Nenhuma tarefa pendente.'}
            </div>
          )}
        </div>
      </SortableContext>
    </div>
  );
}

interface SortableTaskItemProps {
  task: any;
  onToggle: () => void;
  onEdit: () => void;
  onDelete: () => void;
}

function SortableTaskItem({ task, onToggle, onEdit, onDelete }: SortableTaskItemProps) {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({ id: task.id });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
  };

  return (
    <div ref={setNodeRef} style={style}>
      <TactileCard className="p-4 flex items-start gap-3 transition-all hover:border-primary/30 group" data-testid={`card-task-${task.id}`}>
        <button
          {...attributes}
          {...listeners}
          className="mt-1 cursor-grab active:cursor-grabbing text-muted-foreground hover:text-foreground touch-none"
        >
          <GripVertical className="size-4" />
        </button>
        <Checkbox 
          checked={task.isCompleted} 
          onCheckedChange={onToggle}
          className="mt-1"
        />
        <div className="flex-1 space-y-1">
          <p className={`font-medium text-sm leading-none ${task.isCompleted ? 'line-through text-muted-foreground' : ''}`}>
            {task.title}
          </p>
          {task.description && (
            <p className="text-xs text-muted-foreground line-clamp-2 mt-1">
              {task.description}
            </p>
          )}
          <div className="flex items-center gap-4 pt-1">
            {task.dueDate && (
              <div className={`text-xs flex items-center gap-1 ${task.isCompleted ? 'text-muted-foreground' : 'text-orange-600'}`}>
                <Calendar className="size-3" />
                {format(new Date(task.dueDate), "d 'de' MMM", { locale: ptBR })}
              </div>
            )}
            {task.assignee && (
              <div className="text-xs text-muted-foreground flex items-center gap-1">
                <UserIcon className="size-3" />
                {task.assignee.name || task.assignee.email}
              </div>
            )}
          </div>
        </div>
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="ghost" size="icon" className="opacity-0 group-hover:opacity-100 transition-opacity">
              <MoreHorizontal className="size-4" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
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
      </TactileCard>
    </div>
  );
}

function TaskItemOverlay({ task }: { task: any }) {
  return (
    <TactileCard className="p-4 flex items-start gap-3 shadow-xl rotate-2 border-primary">
      <GripVertical className="size-4 mt-1 text-muted-foreground" />
      <Checkbox checked={task.isCompleted} className="mt-1" />
      <div className="flex-1 space-y-1">
        <p className={`font-medium text-sm leading-none ${task.isCompleted ? 'line-through text-muted-foreground' : ''}`}>
          {task.title}
        </p>
      </div>
    </TactileCard>
  );
}
