import { useState, useEffect } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Badge } from "@/components/ui/badge";
import { Switch } from "@/components/ui/switch";
import { Skeleton } from "@/components/ui/skeleton";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Alert, AlertDescription } from "@/components/ui/alert";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
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
import { useToast } from "@/hooks/use-toast";
import { 
  Plus, Calendar, Clock, MoreHorizontal, Pencil, Trash2, Copy, 
  ExternalLink, Video, Check, X, Users, CalendarDays, Link2, Link2Off, Loader2, AlertCircle
} from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import { DeleteModal } from "@/components/modals/delete-modal";

const DAY_NAMES = ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'];
const DURATION_OPTIONS = [15, 30, 45, 60, 90, 120];
const COLOR_OPTIONS = ['#2563eb', '#7c3aed', '#db2777', '#ea580c', '#16a34a', '#0891b2', '#4f46e5', '#dc2626'];

interface SchedulerType {
  id: string;
  name: string;
  slug: string;
  description?: string;
  color: string;
  durationMinutes: number;
  bufferBeforeMinutes: number;
  bufferAfterMinutes: number;
  isActive: boolean;
  createMeetLink: boolean;
  availability?: Array<{ dayOfWeek: number; startTime: string; endTime: string }>;
  bookings?: Booking[];
}

interface Booking {
  id: string;
  clientName: string;
  clientEmail: string;
  clientPhone?: string;
  notes?: string;
  startTime: string;
  endTime: string;
  status: string;
  meetingLink?: string;
  schedulerTypeId: string;
}

export default function SchedulersPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  
  const [modalOpen, setModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [selectedScheduler, setSelectedScheduler] = useState<SchedulerType | null>(null);
  const [activeTab, setActiveTab] = useState<'types' | 'bookings'>('types');
  
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    durationMinutes: 30,
    color: '#2563eb',
    bufferBeforeMinutes: 0,
    bufferAfterMinutes: 0,
    createMeetLink: true,
    availability: [
      { dayOfWeek: 1, startTime: '09:00', endTime: '18:00' },
      { dayOfWeek: 2, startTime: '09:00', endTime: '18:00' },
      { dayOfWeek: 3, startTime: '09:00', endTime: '18:00' },
      { dayOfWeek: 4, startTime: '09:00', endTime: '18:00' },
      { dayOfWeek: 5, startTime: '09:00', endTime: '18:00' },
    ],
  });

  const { data: schedulers = [], isLoading } = useQuery({
    queryKey: ['schedulers', currentWorkspace?.id],
    queryFn: () => api.getSchedulers(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: bookings = [] } = useQuery({
    queryKey: ['bookings', currentWorkspace?.id],
    queryFn: () => api.getBookings(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: googleCalendarStatus } = useQuery({
    queryKey: ['google-calendar-status', currentWorkspace?.id],
    queryFn: () => api.getGoogleCalendarStatus(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  // Check for google_calendar_connected query param
  useEffect(() => {
    const params = new URLSearchParams(window.location.search);
    if (params.get('google_calendar_connected') === 'true') {
      toast({ title: "Sucesso", description: "Google Calendar conectado com sucesso!" });
      window.history.replaceState({}, '', window.location.pathname);
      queryClient.invalidateQueries({ queryKey: ['google-calendar-status'] });
    }
  }, []);

  const connectGoogleCalendarMutation = useMutation({
    mutationFn: () => api.getGoogleCalendarAuthUrl(currentWorkspace!.id),
    onSuccess: ({ authUrl }) => {
      window.location.href = authUrl;
    },
    onError: (error: Error) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const disconnectGoogleCalendarMutation = useMutation({
    mutationFn: () => api.disconnectGoogleCalendar(currentWorkspace!.id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['google-calendar-status'] });
      toast({ title: "Sucesso", description: "Google Calendar desconectado." });
    },
    onError: (error: Error) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const createMutation = useMutation({
    mutationFn: async (data: typeof formData) => {
      const scheduler = await api.createScheduler(currentWorkspace!.id, data);
      if (data.availability.length > 0) {
        await api.updateSchedulerAvailability(currentWorkspace!.id, scheduler.id, data.availability);
      }
      return scheduler;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['schedulers'] });
      setModalOpen(false);
      resetForm();
      toast({ title: "Tipo de reunião criado com sucesso!" });
    },
    onError: (error: Error) => {
      toast({ title: "Erro ao criar", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: async (data: typeof formData) => {
      const scheduler = await api.updateScheduler(currentWorkspace!.id, selectedScheduler!.id, data);
      await api.updateSchedulerAvailability(currentWorkspace!.id, selectedScheduler!.id, data.availability);
      return scheduler;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['schedulers'] });
      setModalOpen(false);
      setSelectedScheduler(null);
      resetForm();
      toast({ title: "Tipo de reunião atualizado!" });
    },
    onError: (error: Error) => {
      toast({ title: "Erro ao atualizar", description: error.message, variant: "destructive" });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => api.deleteScheduler(currentWorkspace!.id, id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['schedulers'] });
      setDeleteModalOpen(false);
      setSelectedScheduler(null);
      toast({ title: "Tipo de reunião excluído!" });
    },
    onError: (error: Error) => {
      toast({ title: "Erro ao excluir", description: error.message, variant: "destructive" });
    },
  });

  const updateBookingMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: any }) => 
      api.updateBooking(currentWorkspace!.id, id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['bookings'] });
      toast({ title: "Agendamento atualizado!" });
    },
  });

  const cancelBookingMutation = useMutation({
    mutationFn: (id: string) => api.deleteBooking(currentWorkspace!.id, id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['bookings'] });
      toast({ title: "Agendamento cancelado!" });
    },
  });

  const resetForm = () => {
    setFormData({
      name: '',
      description: '',
      durationMinutes: 30,
      color: '#2563eb',
      bufferBeforeMinutes: 0,
      bufferAfterMinutes: 0,
      createMeetLink: true,
      availability: [
        { dayOfWeek: 1, startTime: '09:00', endTime: '18:00' },
        { dayOfWeek: 2, startTime: '09:00', endTime: '18:00' },
        { dayOfWeek: 3, startTime: '09:00', endTime: '18:00' },
        { dayOfWeek: 4, startTime: '09:00', endTime: '18:00' },
        { dayOfWeek: 5, startTime: '09:00', endTime: '18:00' },
      ],
    });
  };

  const handleEdit = async (scheduler: SchedulerType) => {
    const details = await api.getScheduler(currentWorkspace!.id, scheduler.id);
    setSelectedScheduler(details);
    setFormData({
      name: details.name,
      description: details.description || '',
      durationMinutes: details.durationMinutes,
      color: details.color || '#2563eb',
      bufferBeforeMinutes: details.bufferBeforeMinutes || 0,
      bufferAfterMinutes: details.bufferAfterMinutes || 0,
      createMeetLink: details.createMeetLink !== false,
      availability: details.availability || [],
    });
    setModalOpen(true);
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (selectedScheduler) {
      updateMutation.mutate(formData);
    } else {
      createMutation.mutate(formData);
    }
  };

  const toggleDayAvailability = (dayOfWeek: number) => {
    const exists = formData.availability.find(a => a.dayOfWeek === dayOfWeek);
    if (exists) {
      setFormData({
        ...formData,
        availability: formData.availability.filter(a => a.dayOfWeek !== dayOfWeek),
      });
    } else {
      setFormData({
        ...formData,
        availability: [...formData.availability, { dayOfWeek, startTime: '09:00', endTime: '18:00' }].sort((a, b) => a.dayOfWeek - b.dayOfWeek),
      });
    }
  };

  const updateDayTime = (dayOfWeek: number, field: 'startTime' | 'endTime', value: string) => {
    setFormData({
      ...formData,
      availability: formData.availability.map(a => 
        a.dayOfWeek === dayOfWeek ? { ...a, [field]: value } : a
      ),
    });
  };

  const copyLink = (scheduler: SchedulerType) => {
    const url = `${window.location.origin}/s/${currentWorkspace?.slug}/${scheduler.slug}`;
    navigator.clipboard.writeText(url);
    toast({ title: "Link copiado!" });
  };

  const getSchedulerName = (schedulerTypeId: string) => {
    const scheduler = schedulers.find((s: SchedulerType) => s.id === schedulerTypeId);
    return scheduler?.name || 'Tipo desconhecido';
  };

  return (
    <AppShell>
      <div className="p-6 space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-2xl font-semibold">Agendamentos</h1>
            <p className="text-muted-foreground">
              Configure tipos de reunião e gerencie agendamentos
            </p>
          </div>
        </div>

        {/* Google Calendar Integration Status */}
        <TactileCard className="p-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-3">
              <div className={`p-2 rounded-lg ${googleCalendarStatus?.connected ? 'bg-green-100 text-green-700' : 'bg-amber-100 text-amber-700'}`}>
                <Calendar className="size-5" />
              </div>
              <div>
                <h3 className="font-medium">Google Calendar</h3>
                {googleCalendarStatus?.connected ? (
                  <p className="text-sm text-muted-foreground">
                    Conectado como {googleCalendarStatus.email}
                  </p>
                ) : (
                  <p className="text-sm text-muted-foreground">
                    Conecte para sincronizar eventos e verificar disponibilidade
                  </p>
                )}
              </div>
            </div>
            <div>
              {googleCalendarStatus?.connected ? (
                <Button
                  variant="outline"
                  size="sm"
                  onClick={() => disconnectGoogleCalendarMutation.mutate()}
                  disabled={disconnectGoogleCalendarMutation.isPending}
                  className="gap-2"
                >
                  {disconnectGoogleCalendarMutation.isPending ? (
                    <Loader2 className="size-4 animate-spin" />
                  ) : (
                    <Link2Off className="size-4" />
                  )}
                  Desconectar
                </Button>
              ) : (
                <Button
                  size="sm"
                  onClick={() => connectGoogleCalendarMutation.mutate()}
                  disabled={connectGoogleCalendarMutation.isPending}
                  className="gap-2"
                >
                  {connectGoogleCalendarMutation.isPending ? (
                    <Loader2 className="size-4 animate-spin" />
                  ) : (
                    <Link2 className="size-4" />
                  )}
                  Conectar Google Calendar
                </Button>
              )}
            </div>
          </div>
        </TactileCard>

        <Tabs value={activeTab} onValueChange={(v) => setActiveTab(v as 'types' | 'bookings')}>
          <TabsList>
            <TabsTrigger value="types" className="gap-2">
              <CalendarDays className="size-4" />
              Tipos de Reunião
            </TabsTrigger>
            <TabsTrigger value="bookings" className="gap-2">
              <Users className="size-4" />
              Agendamentos ({bookings.length})
            </TabsTrigger>
          </TabsList>

          <TabsContent value="types" className="mt-6">
            <div className="flex justify-end mb-4">
              <Button onClick={() => { resetForm(); setSelectedScheduler(null); setModalOpen(true); }} className="gap-2" data-testid="button-add-scheduler">
                <Plus className="size-4" />
                Novo Tipo de Reunião
              </Button>
            </div>

            {isLoading ? (
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {[1, 2, 3].map(i => (
                  <Skeleton key={i} className="h-48 rounded-xl" />
                ))}
              </div>
            ) : schedulers.length === 0 ? (
              <TactileCard className="p-12 text-center">
                <Calendar className="size-12 mx-auto text-muted-foreground mb-4" />
                <h3 className="text-lg font-medium mb-2">Nenhum tipo de reunião</h3>
                <p className="text-muted-foreground mb-4">
                  Crie tipos de reunião para permitir que clientes agendem horários com você
                </p>
                <Button onClick={() => setModalOpen(true)} className="gap-2">
                  <Plus className="size-4" />
                  Criar primeiro tipo
                </Button>
              </TactileCard>
            ) : (
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {schedulers.map((scheduler: SchedulerType) => (
                  <TactileCard key={scheduler.id} className="p-6 relative group" data-testid={`scheduler-card-${scheduler.id}`}>
                    <div className="absolute top-4 right-4 flex items-center gap-2">
                      <Badge variant={scheduler.isActive ? "default" : "secondary"}>
                        {scheduler.isActive ? 'Ativo' : 'Inativo'}
                      </Badge>
                      <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                          <Button variant="ghost" size="icon" className="h-8 w-8">
                            <MoreHorizontal className="size-4" />
                          </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent align="end">
                          <DropdownMenuItem onClick={() => handleEdit(scheduler)}>
                            <Pencil className="size-4 mr-2" />
                            Editar
                          </DropdownMenuItem>
                          <DropdownMenuItem onClick={() => copyLink(scheduler)}>
                            <Copy className="size-4 mr-2" />
                            Copiar Link
                          </DropdownMenuItem>
                          <DropdownMenuItem onClick={() => window.open(`/s/${currentWorkspace?.slug}/${scheduler.slug}`, '_blank')}>
                            <ExternalLink className="size-4 mr-2" />
                            Abrir Página
                          </DropdownMenuItem>
                          <DropdownMenuItem 
                            onClick={() => { setSelectedScheduler(scheduler); setDeleteModalOpen(true); }}
                            className="text-destructive"
                          >
                            <Trash2 className="size-4 mr-2" />
                            Excluir
                          </DropdownMenuItem>
                        </DropdownMenuContent>
                      </DropdownMenu>
                    </div>

                    <div 
                      className="size-12 rounded-xl flex items-center justify-center text-white mb-4"
                      style={{ backgroundColor: scheduler.color }}
                    >
                      <Calendar className="size-6" />
                    </div>

                    <h3 className="text-lg font-semibold mb-1">{scheduler.name}</h3>
                    {scheduler.description && (
                      <p className="text-sm text-muted-foreground mb-3 line-clamp-2">{scheduler.description}</p>
                    )}

                    <div className="flex items-center gap-4 text-sm text-muted-foreground">
                      <div className="flex items-center gap-1">
                        <Clock className="size-4" />
                        {scheduler.durationMinutes} min
                      </div>
                      {scheduler.createMeetLink && (
                        <div className="flex items-center gap-1">
                          <Video className="size-4" />
                          Google Meet
                        </div>
                      )}
                    </div>
                  </TactileCard>
                ))}
              </div>
            )}
          </TabsContent>

          <TabsContent value="bookings" className="mt-6">
            {bookings.length === 0 ? (
              <TactileCard className="p-12 text-center">
                <Users className="size-12 mx-auto text-muted-foreground mb-4" />
                <h3 className="text-lg font-medium mb-2">Nenhum agendamento</h3>
                <p className="text-muted-foreground">
                  Quando clientes agendarem reuniões, elas aparecerão aqui
                </p>
              </TactileCard>
            ) : (
              <div className="space-y-3">
                {bookings.map((booking: Booking) => (
                  <TactileCard key={booking.id} className="p-4" data-testid={`booking-card-${booking.id}`}>
                    <div className="flex items-center justify-between">
                      <div className="flex items-center gap-4">
                        <div className="size-12 rounded-full bg-primary/10 flex items-center justify-center">
                          <span className="text-lg font-semibold text-primary">
                            {booking.clientName.charAt(0).toUpperCase()}
                          </span>
                        </div>
                        <div>
                          <h4 className="font-medium">{booking.clientName}</h4>
                          <p className="text-sm text-muted-foreground">{booking.clientEmail}</p>
                          <div className="flex items-center gap-2 mt-1 text-sm">
                            <Badge variant="outline">{getSchedulerName(booking.schedulerTypeId)}</Badge>
                            <span className="text-muted-foreground">
                              {format(new Date(booking.startTime), "dd/MM/yyyy 'às' HH:mm", { locale: ptBR })}
                            </span>
                          </div>
                        </div>
                      </div>
                      <div className="flex items-center gap-2">
                        <Badge variant={
                          booking.status === 'CONFIRMED' ? 'default' :
                          booking.status === 'COMPLETED' ? 'secondary' :
                          booking.status === 'CANCELLED' ? 'destructive' : 'outline'
                        }>
                          {booking.status === 'CONFIRMED' ? 'Confirmado' :
                           booking.status === 'COMPLETED' ? 'Concluído' :
                           booking.status === 'CANCELLED' ? 'Cancelado' :
                           booking.status === 'NO_SHOW' ? 'Não compareceu' : 'Pendente'}
                        </Badge>
                        {booking.meetingLink && (
                          <Button variant="outline" size="sm" asChild>
                            <a href={booking.meetingLink} target="_blank" rel="noopener noreferrer">
                              <Video className="size-4 mr-1" />
                              Meet
                            </a>
                          </Button>
                        )}
                        <DropdownMenu>
                          <DropdownMenuTrigger asChild>
                            <Button variant="ghost" size="icon" className="h-8 w-8">
                              <MoreHorizontal className="size-4" />
                            </Button>
                          </DropdownMenuTrigger>
                          <DropdownMenuContent align="end">
                            <DropdownMenuItem onClick={() => updateBookingMutation.mutate({ id: booking.id, data: { status: 'COMPLETED' } })}>
                              <Check className="size-4 mr-2" />
                              Marcar como Concluído
                            </DropdownMenuItem>
                            <DropdownMenuItem onClick={() => updateBookingMutation.mutate({ id: booking.id, data: { status: 'NO_SHOW' } })}>
                              <X className="size-4 mr-2" />
                              Não Compareceu
                            </DropdownMenuItem>
                            <DropdownMenuItem 
                              onClick={() => cancelBookingMutation.mutate(booking.id)}
                              className="text-destructive"
                            >
                              <Trash2 className="size-4 mr-2" />
                              Cancelar Agendamento
                            </DropdownMenuItem>
                          </DropdownMenuContent>
                        </DropdownMenu>
                      </div>
                    </div>
                    {booking.notes && (
                      <p className="mt-3 text-sm text-muted-foreground border-t pt-3">{booking.notes}</p>
                    )}
                  </TactileCard>
                ))}
              </div>
            )}
          </TabsContent>
        </Tabs>
      </div>

      <Dialog open={modalOpen} onOpenChange={setModalOpen}>
        <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle>
              {selectedScheduler ? 'Editar Tipo de Reunião' : 'Novo Tipo de Reunião'}
            </DialogTitle>
          </DialogHeader>

          <form onSubmit={handleSubmit} className="space-y-6">
            <div className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="name">Nome *</Label>
                <Input
                  id="name"
                  value={formData.name}
                  onChange={e => setFormData({ ...formData, name: e.target.value })}
                  placeholder="Ex: Reunião de Consultoria"
                  required
                  data-testid="input-scheduler-name"
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="description">Descrição</Label>
                <Textarea
                  id="description"
                  value={formData.description}
                  onChange={e => setFormData({ ...formData, description: e.target.value })}
                  placeholder="Descreva brevemente este tipo de reunião..."
                  rows={2}
                  data-testid="input-scheduler-description"
                />
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label>Duração</Label>
                  <Select 
                    value={String(formData.durationMinutes)} 
                    onValueChange={v => setFormData({ ...formData, durationMinutes: parseInt(v) })}
                  >
                    <SelectTrigger data-testid="select-duration">
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      {DURATION_OPTIONS.map(d => (
                        <SelectItem key={d} value={String(d)}>{d} minutos</SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>

                <div className="space-y-2">
                  <Label>Cor</Label>
                  <div className="flex gap-2">
                    {COLOR_OPTIONS.map(color => (
                      <button
                        key={color}
                        type="button"
                        onClick={() => setFormData({ ...formData, color })}
                        className={`size-8 rounded-full transition-transform ${formData.color === color ? 'ring-2 ring-offset-2 ring-primary scale-110' : ''}`}
                        style={{ backgroundColor: color }}
                        data-testid={`color-option-${color}`}
                      />
                    ))}
                  </div>
                </div>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label>Intervalo antes (min)</Label>
                  <Input
                    type="number"
                    min="0"
                    max="60"
                    value={formData.bufferBeforeMinutes}
                    onChange={e => setFormData({ ...formData, bufferBeforeMinutes: parseInt(e.target.value) || 0 })}
                    data-testid="input-buffer-before"
                  />
                </div>
                <div className="space-y-2">
                  <Label>Intervalo depois (min)</Label>
                  <Input
                    type="number"
                    min="0"
                    max="60"
                    value={formData.bufferAfterMinutes}
                    onChange={e => setFormData({ ...formData, bufferAfterMinutes: parseInt(e.target.value) || 0 })}
                    data-testid="input-buffer-after"
                  />
                </div>
              </div>

              <div className="flex items-center justify-between p-4 bg-muted rounded-lg">
                <div className="flex items-center gap-3">
                  <Video className="size-5" />
                  <div>
                    <p className="font-medium">Criar link do Google Meet</p>
                    <p className="text-sm text-muted-foreground">Gerar automaticamente um link para videoconferência</p>
                  </div>
                </div>
                <Switch
                  checked={formData.createMeetLink}
                  onCheckedChange={v => setFormData({ ...formData, createMeetLink: v })}
                  data-testid="switch-create-meet"
                />
              </div>

              <div className="space-y-3">
                <Label>Disponibilidade</Label>
                <div className="space-y-2">
                  {DAY_NAMES.map((day, index) => {
                    const dayAvail = formData.availability.find(a => a.dayOfWeek === index);
                    return (
                      <div key={index} className="flex items-center gap-3 p-2 rounded-lg hover:bg-muted/50">
                        <Switch
                          checked={!!dayAvail}
                          onCheckedChange={() => toggleDayAvailability(index)}
                          data-testid={`switch-day-${index}`}
                        />
                        <span className="w-20 text-sm font-medium">{day}</span>
                        {dayAvail && (
                          <>
                            <Input
                              type="time"
                              value={dayAvail.startTime}
                              onChange={e => updateDayTime(index, 'startTime', e.target.value)}
                              className="w-28"
                              data-testid={`input-start-${index}`}
                            />
                            <span className="text-muted-foreground">até</span>
                            <Input
                              type="time"
                              value={dayAvail.endTime}
                              onChange={e => updateDayTime(index, 'endTime', e.target.value)}
                              className="w-28"
                              data-testid={`input-end-${index}`}
                            />
                          </>
                        )}
                      </div>
                    );
                  })}
                </div>
              </div>
            </div>

            <div className="flex justify-end gap-3 pt-4 border-t">
              <Button type="button" variant="outline" onClick={() => setModalOpen(false)}>
                Cancelar
              </Button>
              <Button 
                type="submit" 
                disabled={createMutation.isPending || updateMutation.isPending}
                data-testid="button-save-scheduler"
              >
                {selectedScheduler ? 'Salvar Alterações' : 'Criar Tipo de Reunião'}
              </Button>
            </div>
          </form>
        </DialogContent>
      </Dialog>

      <DeleteModal
        open={deleteModalOpen}
        onOpenChange={setDeleteModalOpen}
        onConfirm={() => deleteMutation.mutate(selectedScheduler?.id || '')}
        title="Excluir Tipo de Reunião"
        description={`Tem certeza que deseja excluir "${selectedScheduler?.name}"? Todos os agendamentos associados também serão removidos.`}
        isLoading={deleteMutation.isPending}
      />
    </AppShell>
  );
}
