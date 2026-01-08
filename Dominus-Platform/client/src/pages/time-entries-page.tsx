import { useState, useMemo } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { Badge } from "@/components/ui/badge";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Checkbox } from "@/components/ui/checkbox";
import { 
  Plus, 
  Clock, 
  Calendar, 
  MoreHorizontal, 
  Pencil, 
  Trash2, 
  Filter,
  Briefcase
} from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import { DeleteModal } from "@/components/modals/delete-modal";
import { useToast } from "@/hooks/use-toast";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
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
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { Calendar as CalendarComponent } from "@/components/ui/calendar";
import { Loader2 } from "lucide-react";

interface TimeEntryFormData {
  date: Date;
  projectId: string;
  taskId: string;
  description: string;
  durationMinutes: number;
  hourlyRate: string;
  billable: boolean;
}

const defaultFormData: TimeEntryFormData = {
  date: new Date(),
  projectId: "",
  taskId: "",
  description: "",
  durationMinutes: 60,
  hourlyRate: "",
  billable: false,
};

function TimeEntryModal({
  open,
  onOpenChange,
  entry,
  projects,
  tasks,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  entry?: any;
  projects: any[];
  tasks: any[];
}) {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [formData, setFormData] = useState<TimeEntryFormData>(defaultFormData);
  const [dateOpen, setDateOpen] = useState(false);

  const filteredTasks = useMemo(() => {
    if (!formData.projectId) return [];
    return tasks.filter((t: any) => t.projectId === formData.projectId);
  }, [formData.projectId, tasks]);

  const calculatedValue = useMemo(() => {
    if (!formData.hourlyRate || !formData.durationMinutes) return null;
    const hours = formData.durationMinutes / 60;
    const rate = parseFloat(formData.hourlyRate);
    if (isNaN(rate)) return null;
    return hours * rate;
  }, [formData.hourlyRate, formData.durationMinutes]);

  useState(() => {
    if (entry) {
      const hours = parseFloat(entry.hours) || 0;
      setFormData({
        date: new Date(entry.date),
        projectId: entry.projectId || "",
        taskId: entry.taskId || "",
        description: entry.description || "",
        durationMinutes: Math.round(hours * 60),
        hourlyRate: entry.hourlyRate || "",
        billable: entry.billable || false,
      });
    } else {
      setFormData(defaultFormData);
    }
  });

  const createMutation = useMutation({
    mutationFn: (data: any) => api.createTimeEntry(currentWorkspace!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["time-entries", currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Lançamento criado com sucesso!" });
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: (data: any) =>
      api.updateTimeEntry(currentWorkspace!.id, entry.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["time-entries", currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Lançamento atualizado com sucesso!" });
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    const hours = formData.durationMinutes / 60;
    const payload = {
      date: formData.date.toISOString(),
      projectId: formData.projectId || null,
      taskId: formData.taskId || null,
      description: formData.description,
      hours: hours.toFixed(2),
    };

    if (entry) {
      updateMutation.mutate(payload);
    } else {
      createMutation.mutate(payload);
    }
  };

  const isSubmitting = createMutation.isPending || updateMutation.isPending;

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle>
            {entry ? "Editar Lançamento de Tempo" : "Novo Lançamento de Tempo"}
          </DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label>Data *</Label>
              <Popover open={dateOpen} onOpenChange={setDateOpen}>
                <PopoverTrigger asChild>
                  <Button
                    variant="outline"
                    className="w-full justify-start text-left font-normal"
                    data-testid="input-date"
                  >
                    <Calendar className="mr-2 h-4 w-4" />
                    {format(formData.date, "dd/MM/yyyy", { locale: ptBR })}
                  </Button>
                </PopoverTrigger>
                <PopoverContent className="w-auto p-0" align="start">
                  <CalendarComponent
                    mode="single"
                    selected={formData.date}
                    onSelect={(date) => {
                      if (date) {
                        setFormData({ ...formData, date });
                        setDateOpen(false);
                      }
                    }}
                    locale={ptBR}
                    initialFocus
                  />
                </PopoverContent>
              </Popover>
            </div>

            <div className="space-y-2">
              <Label>Duração (minutos) *</Label>
              <Input
                type="number"
                min={1}
                value={formData.durationMinutes}
                onChange={(e) =>
                  setFormData({ ...formData, durationMinutes: parseInt(e.target.value) || 0 })
                }
                placeholder="60"
                data-testid="input-duration"
              />
              <p className="text-xs text-muted-foreground">
                {(formData.durationMinutes / 60).toFixed(2)} horas
              </p>
            </div>
          </div>

          <div className="space-y-2">
            <Label>Projeto</Label>
            <Select
              value={formData.projectId}
              onValueChange={(value) =>
                setFormData({ ...formData, projectId: value, taskId: "" })
              }
            >
              <SelectTrigger data-testid="select-project">
                <SelectValue placeholder="Selecione um projeto" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="none">Nenhum</SelectItem>
                {projects.map((project: any) => (
                  <SelectItem key={project.id} value={project.id}>
                    {project.title}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          {formData.projectId && filteredTasks.length > 0 && (
            <div className="space-y-2">
              <Label>Tarefa (opcional)</Label>
              <Select
                value={formData.taskId}
                onValueChange={(value) => setFormData({ ...formData, taskId: value })}
              >
                <SelectTrigger data-testid="select-task">
                  <SelectValue placeholder="Selecione uma tarefa" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="none">Nenhuma</SelectItem>
                  {filteredTasks.map((task: any) => (
                    <SelectItem key={task.id} value={task.id}>
                      {task.title}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          )}

          <div className="space-y-2">
            <Label>Descrição *</Label>
            <Textarea
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              placeholder="Descreva o trabalho realizado..."
              rows={3}
              data-testid="input-description"
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label>Valor por Hora (R$)</Label>
              <Input
                type="number"
                step="0.01"
                min={0}
                value={formData.hourlyRate}
                onChange={(e) => setFormData({ ...formData, hourlyRate: e.target.value })}
                placeholder="0,00"
                data-testid="input-hourly-rate"
              />
            </div>

            <div className="flex items-end pb-2">
              <div className="flex items-center space-x-2">
                <Checkbox
                  id="billable"
                  checked={formData.billable}
                  onCheckedChange={(checked) =>
                    setFormData({ ...formData, billable: checked === true })
                  }
                  data-testid="checkbox-billable"
                />
                <Label htmlFor="billable" className="cursor-pointer">
                  Faturável
                </Label>
              </div>
            </div>
          </div>

          {calculatedValue !== null && (
            <div className="p-3 bg-muted rounded-lg">
              <p className="text-sm text-muted-foreground">Valor Total Estimado:</p>
              <p className="text-lg font-semibold">
                R$ {calculatedValue.toLocaleString("pt-BR", { minimumFractionDigits: 2 })}
              </p>
            </div>
          )}

          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Cancelar
            </Button>
            <Button type="submit" disabled={isSubmitting || !formData.description}>
              {isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
              {entry ? "Salvar" : "Criar"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}

export default function TimeEntriesPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [modalOpen, setModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [selectedEntry, setSelectedEntry] = useState<any>(null);

  const [filterProject, setFilterProject] = useState<string>("");
  const [filterBillable, setFilterBillable] = useState<string>("all");
  const [filterDateFrom, setFilterDateFrom] = useState<Date | undefined>();
  const [filterDateTo, setFilterDateTo] = useState<Date | undefined>();

  const { data: timeEntries = [], isLoading: entriesLoading } = useQuery({
    queryKey: ["time-entries", currentWorkspace?.id],
    queryFn: () => api.getTimeEntries(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: projects = [] } = useQuery({
    queryKey: ["projects", currentWorkspace?.id],
    queryFn: () => api.getProjects(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: tasks = [] } = useQuery({
    queryKey: ["tasks", currentWorkspace?.id],
    queryFn: () => api.getTasks(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const deleteMutation = useMutation({
    mutationFn: (entryId: string) => api.deleteTimeEntry(currentWorkspace!.id, entryId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["time-entries", currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Lançamento excluído com sucesso!" });
      setDeleteModalOpen(false);
      setSelectedEntry(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const filteredEntries = useMemo(() => {
    let filtered = [...timeEntries];

    if (filterProject) {
      filtered = filtered.filter((e: any) => e.projectId === filterProject);
    }

    if (filterDateFrom) {
      filtered = filtered.filter((e: any) => new Date(e.date) >= filterDateFrom);
    }

    if (filterDateTo) {
      filtered = filtered.filter((e: any) => new Date(e.date) <= filterDateTo);
    }

    return filtered;
  }, [timeEntries, filterProject, filterBillable, filterDateFrom, filterDateTo]);

  const projectsMap = useMemo(() => {
    const map: Record<string, any> = {};
    projects.forEach((p: any) => {
      map[p.id] = p;
    });
    return map;
  }, [projects]);

  const tasksMap = useMemo(() => {
    const map: Record<string, any> = {};
    tasks.forEach((t: any) => {
      map[t.id] = t;
    });
    return map;
  }, [tasks]);

  const handleEdit = (entry: any) => {
    setSelectedEntry(entry);
    setModalOpen(true);
  };

  const handleDelete = (entry: any) => {
    setSelectedEntry(entry);
    setDeleteModalOpen(true);
  };

  const handleNew = () => {
    setSelectedEntry(null);
    setModalOpen(true);
  };

  const clearFilters = () => {
    setFilterProject("");
    setFilterBillable("all");
    setFilterDateFrom(undefined);
    setFilterDateTo(undefined);
  };

  const formatDuration = (hours: string | number) => {
    const h = parseFloat(hours as string) || 0;
    const totalMinutes = Math.round(h * 60);
    const hrs = Math.floor(totalMinutes / 60);
    const mins = totalMinutes % 60;
    if (hrs === 0) return `${mins}min`;
    if (mins === 0) return `${hrs}h`;
    return `${hrs}h ${mins}min`;
  };

  const totalHours = useMemo(() => {
    return filteredEntries.reduce((sum: number, e: any) => sum + (parseFloat(e.hours) || 0), 0);
  }, [filteredEntries]);

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight">
              Registro de Tempo
            </h1>
            <p className="text-muted-foreground">
              Gerencie os lançamentos de horas trabalhadas.
            </p>
          </div>
          <Button className="gap-2" onClick={handleNew} data-testid="button-add-time-entry">
            <Plus className="size-4" />
            Novo Lançamento
          </Button>
        </div>

        <div className="flex flex-wrap items-center gap-4 p-4 bg-muted/50 rounded-lg">
          <div className="flex items-center gap-2">
            <Filter className="size-4 text-muted-foreground" />
            <span className="text-sm font-medium">Filtros:</span>
          </div>

          <Select value={filterProject || "all"} onValueChange={(v) => setFilterProject(v === "all" ? "" : v)}>
            <SelectTrigger className="w-[200px]" data-testid="filter-project">
              <SelectValue placeholder="Todos os projetos" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="all">Todos os projetos</SelectItem>
              {projects.map((project: any) => (
                <SelectItem key={project.id} value={project.id}>
                  {project.title}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>

          <Popover>
            <PopoverTrigger asChild>
              <Button variant="outline" className="w-[150px] justify-start">
                <Calendar className="mr-2 h-4 w-4" />
                {filterDateFrom ? format(filterDateFrom, "dd/MM/yyyy") : "Data inicial"}
              </Button>
            </PopoverTrigger>
            <PopoverContent className="w-auto p-0" align="start">
              <CalendarComponent
                mode="single"
                selected={filterDateFrom}
                onSelect={setFilterDateFrom}
                locale={ptBR}
                initialFocus
              />
            </PopoverContent>
          </Popover>

          <Popover>
            <PopoverTrigger asChild>
              <Button variant="outline" className="w-[150px] justify-start">
                <Calendar className="mr-2 h-4 w-4" />
                {filterDateTo ? format(filterDateTo, "dd/MM/yyyy") : "Data final"}
              </Button>
            </PopoverTrigger>
            <PopoverContent className="w-auto p-0" align="start">
              <CalendarComponent
                mode="single"
                selected={filterDateTo}
                onSelect={setFilterDateTo}
                locale={ptBR}
                initialFocus
              />
            </PopoverContent>
          </Popover>

          {(filterProject || filterDateFrom || filterDateTo) && (
            <Button variant="ghost" size="sm" onClick={clearFilters}>
              Limpar filtros
            </Button>
          )}
        </div>

        <div className="flex items-center gap-6 text-sm">
          <div className="flex items-center gap-2">
            <Clock className="size-4 text-muted-foreground" />
            <span className="text-muted-foreground">Total:</span>
            <Badge variant="secondary">{formatDuration(totalHours)}</Badge>
          </div>
          <div className="flex items-center gap-2">
            <span className="text-muted-foreground">Lançamentos:</span>
            <Badge variant="outline">{filteredEntries.length}</Badge>
          </div>
        </div>

        {entriesLoading ? (
          <div className="space-y-3">
            {[...Array(5)].map((_, i) => (
              <Skeleton key={i} className="h-16 rounded-lg" />
            ))}
          </div>
        ) : filteredEntries.length === 0 ? (
          <div className="flex flex-col items-center justify-center py-16 text-center">
            <Clock className="size-16 text-muted-foreground/30 mb-4" />
            <h3 className="text-lg font-medium">Nenhum lançamento encontrado</h3>
            <p className="text-muted-foreground mt-1">
              {timeEntries.length === 0
                ? "Comece registrando seu primeiro lançamento de tempo."
                : "Tente ajustar os filtros para encontrar lançamentos."}
            </p>
            {timeEntries.length === 0 && (
              <Button className="mt-4 gap-2" onClick={handleNew}>
                <Plus className="size-4" />
                Novo Lançamento
              </Button>
            )}
          </div>
        ) : (
          <div className="border rounded-lg overflow-hidden">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead className="w-[120px]">Data</TableHead>
                  <TableHead>Projeto</TableHead>
                  <TableHead>Tarefa</TableHead>
                  <TableHead>Descrição</TableHead>
                  <TableHead className="text-right w-[100px]">Duração</TableHead>
                  <TableHead className="w-[50px]"></TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {filteredEntries.map((entry: any) => {
                  const project = projectsMap[entry.projectId];
                  const task = tasksMap[entry.taskId];

                  return (
                    <TableRow key={entry.id} data-testid={`row-time-entry-${entry.id}`}>
                      <TableCell>
                        <div className="flex items-center gap-2">
                          <Calendar className="size-4 text-muted-foreground" />
                          {format(new Date(entry.date), "dd/MM/yyyy", { locale: ptBR })}
                        </div>
                      </TableCell>
                      <TableCell>
                        {project ? (
                          <div className="flex items-center gap-2">
                            <Briefcase className="size-4 text-muted-foreground" />
                            <span>{project.title}</span>
                          </div>
                        ) : (
                          <span className="text-muted-foreground">—</span>
                        )}
                      </TableCell>
                      <TableCell>
                        {task ? (
                          <span>{task.title}</span>
                        ) : (
                          <span className="text-muted-foreground">—</span>
                        )}
                      </TableCell>
                      <TableCell>
                        <span className="line-clamp-1">{entry.description}</span>
                      </TableCell>
                      <TableCell className="text-right">
                        <Badge variant="secondary">{formatDuration(entry.hours)}</Badge>
                      </TableCell>
                      <TableCell>
                        <DropdownMenu>
                          <DropdownMenuTrigger asChild>
                            <Button variant="ghost" size="icon" className="h-8 w-8">
                              <MoreHorizontal className="size-4" />
                            </Button>
                          </DropdownMenuTrigger>
                          <DropdownMenuContent align="end">
                            <DropdownMenuItem onClick={() => handleEdit(entry)}>
                              <Pencil className="size-4 mr-2" />
                              Editar
                            </DropdownMenuItem>
                            <DropdownMenuItem
                              onClick={() => handleDelete(entry)}
                              className="text-destructive"
                            >
                              <Trash2 className="size-4 mr-2" />
                              Excluir
                            </DropdownMenuItem>
                          </DropdownMenuContent>
                        </DropdownMenu>
                      </TableCell>
                    </TableRow>
                  );
                })}
              </TableBody>
            </Table>
          </div>
        )}
      </div>

      <TimeEntryModal
        open={modalOpen}
        onOpenChange={setModalOpen}
        entry={selectedEntry}
        projects={projects}
        tasks={tasks}
      />

      <DeleteModal
        open={deleteModalOpen}
        onOpenChange={setDeleteModalOpen}
        onConfirm={() => deleteMutation.mutate(selectedEntry?.id)}
        title="Excluir Lançamento"
        description="Tem certeza que deseja excluir este lançamento de tempo? Esta ação não pode ser desfeita."
        isLoading={deleteMutation.isPending}
      />
    </AppShell>
  );
}
