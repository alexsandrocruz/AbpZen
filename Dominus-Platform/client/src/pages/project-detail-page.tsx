import { useState, useMemo } from "react";
import { useRoute, Link } from "wouter";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { TactileCard } from "@/components/ui/tactile-card";
import { Skeleton } from "@/components/ui/skeleton";
import { Textarea } from "@/components/ui/textarea";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Checkbox } from "@/components/ui/checkbox";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Separator } from "@/components/ui/separator";
import { useToast } from "@/hooks/use-toast";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import {
  ArrowLeft,
  Calendar,
  DollarSign,
  Users,
  MessageSquare,
  ListTodo,
  LayoutGrid,
  Plus,
  Mail,
  Phone,
  MessageCircle,
  Send,
  Clock,
  CheckCircle2,
  Circle,
  UserPlus,
  User,
  X,
  Building2,
  Pencil,
  Sparkles,
  Loader2,
  Trash2,
  ArrowUpDown,
} from "lucide-react";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";

export default function ProjectDetailPage() {
  const [, params] = useRoute("/:slug/projects/:projectId");
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const projectId = params?.projectId;

  const [activeTab, setActiveTab] = useState("overview");
  const [showAddMemberModal, setShowAddMemberModal] = useState(false);
  const [memberType, setMemberType] = useState<"responsible" | "follower">("responsible");
  const [showCommunicationModal, setShowCommunicationModal] = useState(false);
  const [showTaskModal, setShowTaskModal] = useState(false);
  const [showGenerateTasksModal, setShowGenerateTasksModal] = useState(false);
  const [generateText, setGenerateText] = useState("");
  const [parsedTasks, setParsedTasks] = useState<any[]>([]);
  const [selectedTasks, setSelectedTasks] = useState<Set<number>>(new Set());
  const [isParsing, setIsParsing] = useState(false);
  const [isCreatingBatch, setIsCreatingBatch] = useState(false);
  const [taskSortOrder, setTaskSortOrder] = useState<"newest" | "oldest">("newest");

  const { data: project, isLoading: projectLoading } = useQuery({
    queryKey: ["project", projectId],
    queryFn: async () => {
      const projects = await api.getProjects(currentWorkspace!.id);
      return projects.find((p: any) => p.id === projectId);
    },
    enabled: !!currentWorkspace && !!projectId,
  });

  const { data: client } = useQuery({
    queryKey: ["client", project?.clientId],
    queryFn: () => api.getClient(currentWorkspace!.id, project!.clientId),
    enabled: !!currentWorkspace && !!project?.clientId,
  });

  const { data: rawTasks = [] } = useQuery({
    queryKey: ["project-tasks", projectId],
    queryFn: async () => {
      const allTasks = await api.getTasks(currentWorkspace!.id);
      return allTasks.filter((t: any) => t.projectId === projectId);
    },
    enabled: !!currentWorkspace && !!projectId,
  });

  const tasks = useMemo(() => {
    const sorted = [...rawTasks].sort((a: any, b: any) => {
      const dateA = new Date(a.createdAt || 0).getTime();
      const dateB = new Date(b.createdAt || 0).getTime();
      return taskSortOrder === "newest" ? dateB - dateA : dateA - dateB;
    });
    return sorted;
  }, [rawTasks, taskSortOrder]);

  const { data: responsibles = [] } = useQuery({
    queryKey: ["project-responsibles", projectId],
    queryFn: () => api.getProjectResponsibles(currentWorkspace!.id, projectId!),
    enabled: !!currentWorkspace && !!projectId,
  });

  const { data: followers = [] } = useQuery({
    queryKey: ["project-followers", projectId],
    queryFn: () => api.getProjectFollowers(currentWorkspace!.id, projectId!),
    enabled: !!currentWorkspace && !!projectId,
  });

  const { data: communications = [] } = useQuery({
    queryKey: ["project-communications", projectId],
    queryFn: () => api.getProjectCommunications(currentWorkspace!.id, projectId!),
    enabled: !!currentWorkspace && !!projectId,
  });

  const { data: members = [] } = useQuery({
    queryKey: ["workspace-members", currentWorkspace?.id],
    queryFn: () => api.getTeamMembers(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const statusMap: Record<string, { label: string; color: string }> = {
    PENDING: { label: "Pendente", color: "bg-orange-500" },
    IN_PROGRESS: { label: "Em Andamento", color: "bg-blue-500" },
    COMPLETED: { label: "Concluído", color: "bg-green-500" },
  };

  const channelMap: Record<string, { label: string; icon: React.ReactNode }> = {
    EMAIL: { label: "E-mail", icon: <Mail className="size-4" /> },
    SMS: { label: "SMS", icon: <MessageSquare className="size-4" /> },
    WHATSAPP: { label: "WhatsApp", icon: <MessageCircle className="size-4" /> },
    PHONE: { label: "Telefone", icon: <Phone className="size-4" /> },
    MEETING: { label: "Reunião", icon: <Users className="size-4" /> },
    NOTE: { label: "Anotação", icon: <Pencil className="size-4" /> },
  };

  const toggleTaskMutation = useMutation({
    mutationFn: (taskId: string) => api.toggleTask(currentWorkspace!.id, taskId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["project-tasks", projectId] });
    },
  });

  const addResponsibleMutation = useMutation({
    mutationFn: (memberId: string) => api.addProjectResponsible(currentWorkspace!.id, projectId!, memberId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["project-responsibles", projectId] });
      toast({ title: "Sucesso", description: "Responsável adicionado!" });
      setShowAddMemberModal(false);
    },
  });

  const removeResponsibleMutation = useMutation({
    mutationFn: (memberId: string) => api.removeProjectResponsible(currentWorkspace!.id, projectId!, memberId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["project-responsibles", projectId] });
      toast({ title: "Sucesso", description: "Responsável removido!" });
    },
  });

  const addFollowerMutation = useMutation({
    mutationFn: (memberId: string) => api.addProjectFollower(currentWorkspace!.id, projectId!, memberId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["project-followers", projectId] });
      toast({ title: "Sucesso", description: "Seguidor adicionado!" });
      setShowAddMemberModal(false);
    },
  });

  const removeFollowerMutation = useMutation({
    mutationFn: (memberId: string) => api.removeProjectFollower(currentWorkspace!.id, projectId!, memberId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["project-followers", projectId] });
      toast({ title: "Sucesso", description: "Seguidor removido!" });
    },
  });

  const createCommunicationMutation = useMutation({
    mutationFn: (data: any) => api.createProjectCommunication(currentWorkspace!.id, projectId!, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["project-communications", projectId] });
      toast({ title: "Sucesso", description: "Comunicação registrada!" });
      setShowCommunicationModal(false);
    },
  });

  const handleParseText = async () => {
    if (!generateText.trim()) {
      toast({ title: "Erro", description: "Cole o texto primeiro", variant: "destructive" });
      return;
    }
    setIsParsing(true);
    try {
      const response = await fetch(`/api/workspaces/${currentWorkspace!.id}/tasks/parse-from-text`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({ text: generateText }),
      });
      if (!response.ok) throw new Error("Erro ao analisar texto");
      const result = await response.json();
      setParsedTasks(result.tasks || []);
      setSelectedTasks(new Set(result.tasks?.map((_: any, i: number) => i) || []));
    } catch (error: any) {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    } finally {
      setIsParsing(false);
    }
  };

  const handleCreateBatchTasks = async () => {
    const tasksToCreate = parsedTasks.filter((_, i) => selectedTasks.has(i));
    if (tasksToCreate.length === 0) {
      toast({ title: "Erro", description: "Selecione pelo menos uma tarefa", variant: "destructive" });
      return;
    }
    setIsCreatingBatch(true);
    try {
      const response = await fetch(`/api/workspaces/${currentWorkspace!.id}/tasks/batch-create`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({ tasks: tasksToCreate, projectId }),
      });
      if (!response.ok) throw new Error("Erro ao criar tarefas");
      const result = await response.json();
      queryClient.invalidateQueries({ queryKey: ["project-tasks", projectId] });
      toast({ title: "Sucesso", description: `${result.created} tarefas criadas!` });
      setShowGenerateTasksModal(false);
      setParsedTasks([]);
      setSelectedTasks(new Set());
      setGenerateText("");
    } catch (error: any) {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    } finally {
      setIsCreatingBatch(false);
    }
  };

  const toggleTaskSelection = (index: number) => {
    const newSelected = new Set(selectedTasks);
    if (newSelected.has(index)) {
      newSelected.delete(index);
    } else {
      newSelected.add(index);
    }
    setSelectedTasks(newSelected);
  };

  const removeTaskFromList = (index: number) => {
    setParsedTasks(prev => prev.filter((_, i) => i !== index));
    const newSelected = new Set<number>();
    selectedTasks.forEach(i => {
      if (i < index) newSelected.add(i);
      else if (i > index) newSelected.add(i - 1);
    });
    setSelectedTasks(newSelected);
  };

  const completedTasks = tasks.filter((t: any) => t.isCompleted).length;
  const progress = tasks.length > 0 ? (completedTasks / tasks.length) * 100 : 0;

  if (projectLoading) {
    return (
      <AppShell>
        <div className="space-y-6">
          <Skeleton className="h-10 w-48" />
          <Skeleton className="h-64 w-full" />
        </div>
      </AppShell>
    );
  }

  if (!project) {
    return (
      <AppShell>
        <div className="flex flex-col items-center justify-center py-16">
          <h2 className="text-xl font-semibold">Projeto não encontrado</h2>
          <Link href={`/${currentWorkspace?.slug}/projects`}>
            <Button variant="link">Voltar para projetos</Button>
          </Link>
        </div>
      </AppShell>
    );
  }

  const handleAddMember = (type: "responsible" | "follower") => {
    setMemberType(type);
    setShowAddMemberModal(true);
  };

  const existingMemberIds = [
    ...responsibles.map((r: any) => r.memberId),
    ...followers.map((f: any) => f.memberId),
  ];

  const availableMembers = members.filter((m: any) => !existingMemberIds.includes(m.id));

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center gap-4">
          <Link href={`/${currentWorkspace?.slug}/projects`}>
            <Button variant="ghost" size="icon" data-testid="button-back-projects">
              <ArrowLeft className="size-5" />
            </Button>
          </Link>
          <div className="flex-1">
            <div className="flex items-center gap-3">
              <h1 className="text-2xl font-display font-bold" data-testid="text-project-title">
                {project.title}
              </h1>
              <Badge className={`${statusMap[project.status]?.color} text-white`}>
                {statusMap[project.status]?.label}
              </Badge>
            </div>
            {client && (
              <Link href={`/${currentWorkspace?.slug}/clients/${client.id}`}>
                <p className="text-muted-foreground hover:underline flex items-center gap-1 mt-1">
                  <Building2 className="size-4" />
                  {client.companyName}
                </p>
              </Link>
            )}
          </div>
        </div>

        <Tabs value={activeTab} onValueChange={setActiveTab} className="space-y-6">
          <TabsList>
            <TabsTrigger value="overview" className="gap-2">
              <LayoutGrid className="size-4" />
              Visão Geral
            </TabsTrigger>
            <TabsTrigger value="tasks" className="gap-2">
              <ListTodo className="size-4" />
              Tarefas ({tasks.length})
            </TabsTrigger>
            <TabsTrigger value="communications" className="gap-2">
              <MessageSquare className="size-4" />
              Comunicações ({communications.length})
            </TabsTrigger>
            <TabsTrigger value="team" className="gap-2">
              <Users className="size-4" />
              Equipe ({responsibles.length + followers.length})
            </TabsTrigger>
          </TabsList>

          <TabsContent value="overview" className="space-y-6">
            <div className="grid md:grid-cols-3 gap-6">
              <TactileCard className="p-6">
                <div className="flex items-center gap-3 text-muted-foreground mb-2">
                  <Calendar className="size-5" />
                  <span>Prazo</span>
                </div>
                <p className="text-xl font-semibold">
                  {project.dueDate
                    ? format(new Date(project.dueDate), "dd/MM/yyyy", { locale: ptBR })
                    : "Não definido"}
                </p>
              </TactileCard>

              <TactileCard className="p-6">
                <div className="flex items-center gap-3 text-muted-foreground mb-2">
                  <DollarSign className="size-5" />
                  <span>Orçamento</span>
                </div>
                <p className="text-xl font-semibold">
                  {project.budget
                    ? new Intl.NumberFormat("pt-BR", {
                        style: "currency",
                        currency: "BRL",
                      }).format(Number(project.budget))
                    : "Não definido"}
                </p>
              </TactileCard>

              <TactileCard className="p-6">
                <div className="flex items-center gap-3 text-muted-foreground mb-2">
                  <ListTodo className="size-5" />
                  <span>Progresso</span>
                </div>
                <p className="text-xl font-semibold">
                  {completedTasks}/{tasks.length} tarefas
                </p>
                <div className="mt-2 h-2 bg-muted rounded-full overflow-hidden">
                  <div
                    className="h-full bg-primary transition-all"
                    style={{ width: `${progress}%` }}
                  />
                </div>
              </TactileCard>
            </div>

            {project.description && (
              <TactileCard className="p-6">
                <h3 className="font-semibold mb-2">Descrição</h3>
                <p className="text-muted-foreground whitespace-pre-wrap">{project.description}</p>
              </TactileCard>
            )}

            <div className="grid md:grid-cols-2 gap-6">
              <TactileCard className="p-6">
                <div className="flex items-center justify-between mb-4">
                  <h3 className="font-semibold">Responsáveis</h3>
                  <Button size="sm" variant="outline" onClick={() => handleAddMember("responsible")}>
                    <UserPlus className="size-4 mr-1" />
                    Adicionar
                  </Button>
                </div>
                <div className="space-y-2">
                  {responsibles.length === 0 ? (
                    <p className="text-sm text-muted-foreground">Nenhum responsável atribuído</p>
                  ) : (
                    responsibles.map((r: any) => (
                      <div key={r.id} className="flex items-center justify-between">
                        <div className="flex items-center gap-2">
                          <Avatar className="size-8">
                            <AvatarFallback className="text-xs">
                              {r.member?.user?.name?.substring(0, 2).toUpperCase()}
                            </AvatarFallback>
                          </Avatar>
                          <span className="text-sm">{r.member?.user?.name}</span>
                        </div>
                        <Button
                          variant="ghost"
                          size="icon"
                          className="size-6"
                          onClick={() => removeResponsibleMutation.mutate(r.memberId)}
                        >
                          <X className="size-4" />
                        </Button>
                      </div>
                    ))
                  )}
                </div>
              </TactileCard>

              <TactileCard className="p-6">
                <div className="flex items-center justify-between mb-4">
                  <h3 className="font-semibold">Seguidores</h3>
                  <Button size="sm" variant="outline" onClick={() => handleAddMember("follower")}>
                    <UserPlus className="size-4 mr-1" />
                    Adicionar
                  </Button>
                </div>
                <div className="space-y-2">
                  {followers.length === 0 ? (
                    <p className="text-sm text-muted-foreground">Nenhum seguidor</p>
                  ) : (
                    followers.map((f: any) => (
                      <div key={f.id} className="flex items-center justify-between">
                        <div className="flex items-center gap-2">
                          <Avatar className="size-8">
                            <AvatarFallback className="text-xs">
                              {f.member?.user?.name?.substring(0, 2).toUpperCase()}
                            </AvatarFallback>
                          </Avatar>
                          <span className="text-sm">{f.member?.user?.name}</span>
                        </div>
                        <Button
                          variant="ghost"
                          size="icon"
                          className="size-6"
                          onClick={() => removeFollowerMutation.mutate(f.memberId)}
                        >
                          <X className="size-4" />
                        </Button>
                      </div>
                    ))
                  )}
                </div>
              </TactileCard>
            </div>
          </TabsContent>

          <TabsContent value="tasks" className="space-y-4">
            <div className="flex justify-between items-center">
              <h3 className="font-semibold">Tarefas do Projeto</h3>
              <div className="flex gap-2 items-center">
                <Select value={taskSortOrder} onValueChange={(v) => setTaskSortOrder(v as "newest" | "oldest")}>
                  <SelectTrigger className="w-[140px] h-8" data-testid="select-task-sort-order">
                    <ArrowUpDown className="size-4 mr-1" />
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="newest">Mais recentes</SelectItem>
                    <SelectItem value="oldest">Mais antigas</SelectItem>
                  </SelectContent>
                </Select>
                <Button size="sm" variant="outline" onClick={() => setShowGenerateTasksModal(true)} data-testid="button-generate-tasks-from-text">
                  <Sparkles className="size-4 mr-1" />
                  Gerar do Texto
                </Button>
                <Button size="sm" onClick={() => setShowTaskModal(true)}>
                  <Plus className="size-4 mr-1" />
                  Nova Tarefa
                </Button>
              </div>
            </div>
            <div className="space-y-2">
              {tasks.length === 0 ? (
                <TactileCard className="p-8 text-center">
                  <ListTodo className="size-12 mx-auto text-muted-foreground mb-4" />
                  <p className="text-muted-foreground">Nenhuma tarefa neste projeto</p>
                  <Button variant="outline" className="mt-4" onClick={() => setShowTaskModal(true)}>
                    <Plus className="size-4 mr-1" />
                    Criar primeira tarefa
                  </Button>
                </TactileCard>
              ) : (
                tasks.map((task: any) => (
                  <TactileCard key={task.id} className="p-4">
                    <div className="flex items-start gap-3">
                      <button
                        onClick={() => toggleTaskMutation.mutate(task.id)}
                        className="mt-0.5"
                        data-testid={`button-toggle-task-${task.id}`}
                      >
                        {task.isCompleted ? (
                          <CheckCircle2 className="size-5 text-green-500" />
                        ) : (
                          <Circle className="size-5 text-muted-foreground" />
                        )}
                      </button>
                      <div className="flex-1">
                        <p
                          className={`font-medium ${
                            task.isCompleted ? "line-through text-muted-foreground" : ""
                          }`}
                        >
                          {task.title}
                        </p>
                        {task.description && (
                          <p className="text-xs text-muted-foreground mt-1 line-clamp-2">
                            {task.description}
                          </p>
                        )}
                        <div className="flex items-center gap-4 mt-2">
                          {task.dueDate && (
                            <p className="text-sm text-muted-foreground flex items-center gap-1">
                              <Clock className="size-3" />
                              {format(new Date(task.dueDate), "dd/MM/yyyy", { locale: ptBR })}
                            </p>
                          )}
                          {task.assignee && (
                            <p className="text-sm text-muted-foreground flex items-center gap-1">
                              <User className="size-3" />
                              {task.assignee.name || task.assignee.email}
                            </p>
                          )}
                        </div>
                      </div>
                    </div>
                  </TactileCard>
                ))
              )}
            </div>
          </TabsContent>

          <TabsContent value="communications" className="space-y-4">
            <div className="flex justify-between items-center">
              <h3 className="font-semibold">Histórico de Comunicações</h3>
              <Button size="sm" onClick={() => setShowCommunicationModal(true)}>
                <Plus className="size-4 mr-1" />
                Nova Comunicação
              </Button>
            </div>
            <div className="space-y-4">
              {communications.length === 0 ? (
                <TactileCard className="p-8 text-center">
                  <MessageSquare className="size-12 mx-auto text-muted-foreground mb-4" />
                  <p className="text-muted-foreground">Nenhuma comunicação registrada</p>
                  <Button
                    variant="outline"
                    className="mt-4"
                    onClick={() => setShowCommunicationModal(true)}
                  >
                    <Plus className="size-4 mr-1" />
                    Registrar comunicação
                  </Button>
                </TactileCard>
              ) : (
                communications.map((comm: any) => (
                  <TactileCard key={comm.id} className="p-4">
                    <div className="flex items-start gap-3">
                      <div
                        className={`p-2 rounded-full ${
                          comm.direction === "OUTBOUND" ? "bg-blue-100" : "bg-green-100"
                        }`}
                      >
                        {channelMap[comm.channel]?.icon}
                      </div>
                      <div className="flex-1">
                        <div className="flex items-center gap-2 mb-1">
                          <Badge variant="outline">{channelMap[comm.channel]?.label}</Badge>
                          <Badge variant={comm.direction === "OUTBOUND" ? "default" : "secondary"}>
                            {comm.direction === "OUTBOUND" ? "Enviado" : "Recebido"}
                          </Badge>
                          {comm.status && (
                            <Badge
                              variant={
                                comm.status === "DELIVERED" || comm.status === "READ"
                                  ? "default"
                                  : comm.status === "FAILED"
                                  ? "destructive"
                                  : "secondary"
                              }
                            >
                              {comm.status}
                            </Badge>
                          )}
                        </div>
                        {comm.subject && <p className="font-medium">{comm.subject}</p>}
                        <p className="text-sm text-muted-foreground mt-1 whitespace-pre-wrap">
                          {comm.content}
                        </p>
                        <p className="text-xs text-muted-foreground mt-2">
                          {format(new Date(comm.createdAt), "dd/MM/yyyy 'às' HH:mm", { locale: ptBR })}
                        </p>
                      </div>
                    </div>
                  </TactileCard>
                ))
              )}
            </div>
          </TabsContent>

          <TabsContent value="team" className="space-y-6">
            <div className="grid md:grid-cols-2 gap-6">
              <TactileCard className="p-6">
                <div className="flex items-center justify-between mb-4">
                  <h3 className="font-semibold text-lg">Responsáveis</h3>
                  <Button size="sm" onClick={() => handleAddMember("responsible")}>
                    <UserPlus className="size-4 mr-1" />
                    Adicionar
                  </Button>
                </div>
                <p className="text-sm text-muted-foreground mb-4">
                  Membros da equipe diretamente responsáveis pela execução do projeto.
                </p>
                <div className="space-y-3">
                  {responsibles.length === 0 ? (
                    <p className="text-sm text-muted-foreground py-4 text-center">
                      Nenhum responsável atribuído
                    </p>
                  ) : (
                    responsibles.map((r: any) => (
                      <div
                        key={r.id}
                        className="flex items-center justify-between p-3 bg-muted/50 rounded-lg"
                      >
                        <div className="flex items-center gap-3">
                          <Avatar>
                            <AvatarFallback>
                              {r.member?.user?.name?.substring(0, 2).toUpperCase()}
                            </AvatarFallback>
                          </Avatar>
                          <div>
                            <p className="font-medium">{r.member?.user?.name}</p>
                            <p className="text-sm text-muted-foreground">{r.member?.user?.email}</p>
                          </div>
                        </div>
                        <Button
                          variant="ghost"
                          size="icon"
                          onClick={() => removeResponsibleMutation.mutate(r.memberId)}
                        >
                          <X className="size-4" />
                        </Button>
                      </div>
                    ))
                  )}
                </div>
              </TactileCard>

              <TactileCard className="p-6">
                <div className="flex items-center justify-between mb-4">
                  <h3 className="font-semibold text-lg">Seguidores</h3>
                  <Button size="sm" onClick={() => handleAddMember("follower")}>
                    <UserPlus className="size-4 mr-1" />
                    Adicionar
                  </Button>
                </div>
                <p className="text-sm text-muted-foreground mb-4">
                  Membros que acompanham o projeto e recebem notificações.
                </p>
                <div className="space-y-3">
                  {followers.length === 0 ? (
                    <p className="text-sm text-muted-foreground py-4 text-center">
                      Nenhum seguidor
                    </p>
                  ) : (
                    followers.map((f: any) => (
                      <div
                        key={f.id}
                        className="flex items-center justify-between p-3 bg-muted/50 rounded-lg"
                      >
                        <div className="flex items-center gap-3">
                          <Avatar>
                            <AvatarFallback>
                              {f.member?.user?.name?.substring(0, 2).toUpperCase()}
                            </AvatarFallback>
                          </Avatar>
                          <div>
                            <p className="font-medium">{f.member?.user?.name}</p>
                            <p className="text-sm text-muted-foreground">{f.member?.user?.email}</p>
                          </div>
                        </div>
                        <Button
                          variant="ghost"
                          size="icon"
                          onClick={() => removeFollowerMutation.mutate(f.memberId)}
                        >
                          <X className="size-4" />
                        </Button>
                      </div>
                    ))
                  )}
                </div>
              </TactileCard>
            </div>
          </TabsContent>
        </Tabs>
      </div>

      <AddMemberModal
        open={showAddMemberModal}
        onOpenChange={setShowAddMemberModal}
        type={memberType}
        members={availableMembers}
        onAdd={(memberId) => {
          if (memberType === "responsible") {
            addResponsibleMutation.mutate(memberId);
          } else {
            addFollowerMutation.mutate(memberId);
          }
        }}
        isLoading={addResponsibleMutation.isPending || addFollowerMutation.isPending}
      />

      <CommunicationModal
        open={showCommunicationModal}
        onOpenChange={setShowCommunicationModal}
        client={client}
        onSubmit={(data) => createCommunicationMutation.mutate(data)}
        isLoading={createCommunicationMutation.isPending}
      />

      <TaskModal
        open={showTaskModal}
        onOpenChange={setShowTaskModal}
        projectId={projectId!}
        workspaceId={currentWorkspace?.id!}
        onSuccess={() => {
          queryClient.invalidateQueries({ queryKey: ["project-tasks", projectId] });
          setShowTaskModal(false);
        }}
      />

      <Dialog open={showGenerateTasksModal} onOpenChange={(open) => {
        setShowGenerateTasksModal(open);
        if (!open) {
          setParsedTasks([]);
          setSelectedTasks(new Set());
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
                    {parsedTasks.length} tarefas encontradas • {selectedTasks.size} selecionadas
                  </p>
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => {
                      setParsedTasks([]);
                      setSelectedTasks(new Set());
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
                          selectedTasks.has(index) ? "border-primary bg-primary/5" : "border-border"
                        }`}
                      >
                        <div className="flex items-start gap-3">
                          <Checkbox
                            checked={selectedTasks.has(index)}
                            onCheckedChange={() => toggleTaskSelection(index)}
                            data-testid={`checkbox-task-${index}`}
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
            <Button variant="outline" onClick={() => setShowGenerateTasksModal(false)}>
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
              <Button onClick={handleCreateBatchTasks} disabled={selectedTasks.size === 0 || isCreatingBatch} data-testid="button-create-tasks">
                {isCreatingBatch ? (
                  <>
                    <Loader2 className="size-4 mr-2 animate-spin" />
                    Criando...
                  </>
                ) : (
                  <>
                    <Plus className="size-4 mr-2" />
                    Criar {selectedTasks.size} Tarefas
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

function AddMemberModal({
  open,
  onOpenChange,
  type,
  members,
  onAdd,
  isLoading,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  type: "responsible" | "follower";
  members: any[];
  onAdd: (memberId: string) => void;
  isLoading: boolean;
}) {
  const [selectedMemberId, setSelectedMemberId] = useState<string>("");

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>
            Adicionar {type === "responsible" ? "Responsável" : "Seguidor"}
          </DialogTitle>
        </DialogHeader>
        <div className="space-y-4">
          <div>
            <Label>Selecione um membro</Label>
            <Select value={selectedMemberId} onValueChange={setSelectedMemberId}>
              <SelectTrigger>
                <SelectValue placeholder="Selecione..." />
              </SelectTrigger>
              <SelectContent>
                {members.map((m: any) => (
                  <SelectItem key={m.id} value={m.id}>
                    {m.user?.name || m.user?.email}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>
        </div>
        <DialogFooter>
          <Button variant="outline" onClick={() => onOpenChange(false)}>
            Cancelar
          </Button>
          <Button
            onClick={() => onAdd(selectedMemberId)}
            disabled={!selectedMemberId || isLoading}
          >
            {isLoading ? "Adicionando..." : "Adicionar"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}

function CommunicationModal({
  open,
  onOpenChange,
  client,
  onSubmit,
  isLoading,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  client: any;
  onSubmit: (data: any) => void;
  isLoading: boolean;
}) {
  const [channel, setChannel] = useState("NOTE");
  const [direction, setDirection] = useState("OUTBOUND");
  const [subject, setSubject] = useState("");
  const [content, setContent] = useState("");

  const handleSubmit = () => {
    onSubmit({
      channel,
      direction,
      subject: subject || null,
      content,
      recipientName: client?.companyName,
      recipientEmail: client?.email,
      recipientPhone: client?.phone,
    });
    setSubject("");
    setContent("");
    setChannel("NOTE");
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-lg">
        <DialogHeader>
          <DialogTitle>Nova Comunicação</DialogTitle>
        </DialogHeader>
        <div className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div>
              <Label>Canal</Label>
              <Select value={channel} onValueChange={setChannel}>
                <SelectTrigger>
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="EMAIL">E-mail</SelectItem>
                  <SelectItem value="SMS">SMS</SelectItem>
                  <SelectItem value="WHATSAPP">WhatsApp</SelectItem>
                  <SelectItem value="PHONE">Telefone</SelectItem>
                  <SelectItem value="MEETING">Reunião</SelectItem>
                  <SelectItem value="NOTE">Anotação</SelectItem>
                </SelectContent>
              </Select>
            </div>
            <div>
              <Label>Direção</Label>
              <Select value={direction} onValueChange={setDirection}>
                <SelectTrigger>
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="OUTBOUND">Enviado</SelectItem>
                  <SelectItem value="INBOUND">Recebido</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>
          <div>
            <Label>Assunto (opcional)</Label>
            <Input
              value={subject}
              onChange={(e) => setSubject(e.target.value)}
              placeholder="Assunto da comunicação"
            />
          </div>
          <div>
            <Label>Conteúdo</Label>
            <Textarea
              value={content}
              onChange={(e) => setContent(e.target.value)}
              placeholder="Descreva a comunicação..."
              rows={4}
            />
          </div>
        </div>
        <DialogFooter>
          <Button variant="outline" onClick={() => onOpenChange(false)}>
            Cancelar
          </Button>
          <Button onClick={handleSubmit} disabled={!content || isLoading}>
            {isLoading ? "Salvando..." : "Salvar"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}

function TaskModal({
  open,
  onOpenChange,
  projectId,
  workspaceId,
  onSuccess,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  projectId: string;
  workspaceId: string;
  onSuccess: () => void;
}) {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [dueDate, setDueDate] = useState("");
  const [assigneeId, setAssigneeId] = useState("");
  const { toast } = useToast();

  const { data: teamMembers = [] } = useQuery({
    queryKey: ['team', workspaceId],
    queryFn: () => api.getTeamMembers(workspaceId),
    enabled: !!workspaceId && open,
  });

  const createTaskMutation = useMutation({
    mutationFn: (data: any) => api.createTask(workspaceId, data),
    onSuccess: () => {
      toast({ title: "Sucesso", description: "Tarefa criada!" });
      setTitle("");
      setDescription("");
      setDueDate("");
      setAssigneeId("");
      onSuccess();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleSubmit = () => {
    createTaskMutation.mutate({
      title,
      description: description || null,
      projectId,
      dueDate: dueDate || null,
      assigneeId: assigneeId || null,
    });
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle>Nova Tarefa</DialogTitle>
        </DialogHeader>
        <div className="space-y-4">
          <div>
            <Label>Título</Label>
            <Input
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              placeholder="Título da tarefa"
              data-testid="input-project-task-title"
            />
          </div>
          <div>
            <Label>Descrição (opcional)</Label>
            <Textarea
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              placeholder="Detalhes da tarefa..."
              rows={3}
              data-testid="input-project-task-description"
            />
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <Label>Responsável</Label>
              <Select value={assigneeId} onValueChange={setAssigneeId}>
                <SelectTrigger data-testid="select-project-task-assignee">
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
            <div>
              <Label>Data de Entrega</Label>
              <Input
                type="date"
                value={dueDate}
                onChange={(e) => setDueDate(e.target.value)}
                data-testid="input-project-task-duedate"
              />
            </div>
          </div>
        </div>
        <DialogFooter>
          <Button variant="outline" onClick={() => onOpenChange(false)}>
            Cancelar
          </Button>
          <Button
            onClick={handleSubmit}
            disabled={!title || createTaskMutation.isPending}
            data-testid="button-create-project-task"
          >
            {createTaskMutation.isPending ? "Criando..." : "Criar"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
