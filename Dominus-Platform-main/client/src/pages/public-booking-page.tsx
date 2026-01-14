import { useState, useEffect } from "react";
import { useRoute } from "wouter";
import { useQuery, useMutation } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Skeleton } from "@/components/ui/skeleton";
import { useToast } from "@/hooks/use-toast";
import { Calendar, Clock, ArrowLeft, ArrowRight, Check, Video, User, Mail, Phone, FileText } from "lucide-react";
import { format, addDays, startOfWeek, isSameDay } from "date-fns";
import { ptBR } from "date-fns/locale";

interface SchedulerInfo {
  id: string;
  name: string;
  description?: string;
  durationMinutes: number;
  color: string;
  workspaceName: string;
  availability: Array<{ dayOfWeek: number; startTime: string; endTime: string }>;
}

interface BookingConfirmation {
  id: string;
  clientName: string;
  clientEmail: string;
  startTime: string;
  endTime: string;
  meetingLink?: string;
  schedulerName: string;
  workspaceName: string;
}

export default function PublicBookingPage() {
  const [, params] = useRoute("/s/:workspaceSlug/:schedulerSlug");
  const { toast } = useToast();
  
  const [step, setStep] = useState<'date' | 'time' | 'form' | 'success'>('date');
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  const [selectedSlot, setSelectedSlot] = useState<string | null>(null);
  const [weekStart, setWeekStart] = useState(startOfWeek(new Date(), { weekStartsOn: 1 }));
  const [formData, setFormData] = useState({
    clientName: '',
    clientEmail: '',
    clientPhone: '',
    notes: '',
  });
  const [bookingResult, setBookingResult] = useState<BookingConfirmation | null>(null);

  const { data: scheduler, isLoading, error } = useQuery<SchedulerInfo>({
    queryKey: ['public-scheduler', params?.workspaceSlug, params?.schedulerSlug],
    queryFn: () => api.getPublicScheduler(params!.workspaceSlug, params!.schedulerSlug),
    enabled: !!params?.workspaceSlug && !!params?.schedulerSlug,
  });

  const { data: slotsData, isLoading: loadingSlots } = useQuery({
    queryKey: ['public-slots', params?.workspaceSlug, params?.schedulerSlug, selectedDate?.toISOString()],
    queryFn: () => api.getPublicSchedulerSlots(
      params!.workspaceSlug, 
      params!.schedulerSlug, 
      selectedDate!.toISOString().split('T')[0]
    ),
    enabled: !!selectedDate && !!params?.workspaceSlug && !!params?.schedulerSlug,
  });

  const bookMutation = useMutation({
    mutationFn: () => api.createPublicBooking(params!.workspaceSlug, params!.schedulerSlug, {
      ...formData,
      startTime: selectedSlot,
    }),
    onSuccess: (data) => {
      setBookingResult(data);
      setStep('success');
    },
    onError: (error: Error) => {
      toast({ title: "Erro ao agendar", description: error.message, variant: "destructive" });
    },
  });

  const getAvailableDays = () => {
    if (!scheduler) return new Set<number>();
    return new Set(scheduler.availability.map(a => a.dayOfWeek));
  };

  const availableDays = getAvailableDays();

  const weekDays = Array.from({ length: 7 }, (_, i) => addDays(weekStart, i));

  const handleDateSelect = (date: Date) => {
    setSelectedDate(date);
    setSelectedSlot(null);
    setStep('time');
  };

  const handleSlotSelect = (slot: string) => {
    setSelectedSlot(slot);
    setStep('form');
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    bookMutation.mutate();
  };

  if (isLoading) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-background to-muted/30 flex items-center justify-center p-4">
        <TactileCard className="w-full max-w-xl p-8">
          <Skeleton className="h-8 w-48 mx-auto mb-4" />
          <Skeleton className="h-4 w-64 mx-auto mb-8" />
          <Skeleton className="h-64 w-full" />
        </TactileCard>
      </div>
    );
  }

  if (error || !scheduler) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-background to-muted/30 flex items-center justify-center p-4">
        <TactileCard className="w-full max-w-xl p-8 text-center">
          <Calendar className="size-12 mx-auto text-muted-foreground mb-4" />
          <h1 className="text-xl font-semibold mb-2">Agendamento não encontrado</h1>
          <p className="text-muted-foreground">
            Este link de agendamento não existe ou foi desativado.
          </p>
        </TactileCard>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-background to-muted/30 flex items-center justify-center p-4">
      <TactileCard className="w-full max-w-xl p-8">
        <div className="text-center mb-6">
          <div 
            className="size-16 rounded-xl flex items-center justify-center text-white mx-auto mb-4"
            style={{ backgroundColor: scheduler.color }}
          >
            <Calendar className="size-8" />
          </div>
          <p className="text-sm text-muted-foreground mb-1">{scheduler.workspaceName}</p>
          <h1 className="text-2xl font-semibold">{scheduler.name}</h1>
          {scheduler.description && (
            <p className="text-muted-foreground mt-2">{scheduler.description}</p>
          )}
          <div className="flex items-center justify-center gap-4 mt-3 text-sm text-muted-foreground">
            <div className="flex items-center gap-1">
              <Clock className="size-4" />
              {scheduler.durationMinutes} minutos
            </div>
            <div className="flex items-center gap-1">
              <Video className="size-4" />
              Google Meet
            </div>
          </div>
        </div>

        {step === 'date' && (
          <div className="space-y-4">
            <div className="flex items-center justify-between mb-4">
              <Button 
                variant="ghost" 
                size="icon"
                onClick={() => setWeekStart(addDays(weekStart, -7))}
                disabled={weekStart <= new Date()}
                data-testid="button-prev-week"
              >
                <ArrowLeft className="size-4" />
              </Button>
              <span className="font-medium">
                {format(weekStart, "MMMM yyyy", { locale: ptBR })}
              </span>
              <Button 
                variant="ghost" 
                size="icon"
                onClick={() => setWeekStart(addDays(weekStart, 7))}
                data-testid="button-next-week"
              >
                <ArrowRight className="size-4" />
              </Button>
            </div>

            <div className="grid grid-cols-7 gap-2">
              {['Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'].map((day, i) => (
                <div key={i} className="text-center text-xs font-medium text-muted-foreground py-2">
                  {day}
                </div>
              ))}
              {weekDays.map((date, i) => {
                const dayOfWeek = date.getDay();
                const isAvailable = availableDays.has(dayOfWeek) && date >= new Date(new Date().setHours(0,0,0,0));
                const isSelected = selectedDate && isSameDay(date, selectedDate);
                
                return (
                  <button
                    key={i}
                    onClick={() => isAvailable && handleDateSelect(date)}
                    disabled={!isAvailable}
                    className={`
                      aspect-square rounded-lg flex flex-col items-center justify-center transition-all
                      ${isAvailable 
                        ? 'hover:bg-primary/10 cursor-pointer' 
                        : 'opacity-40 cursor-not-allowed'
                      }
                      ${isSelected ? 'bg-primary text-primary-foreground' : ''}
                    `}
                    data-testid={`date-${format(date, 'yyyy-MM-dd')}`}
                  >
                    <span className="text-lg font-medium">{format(date, 'd')}</span>
                  </button>
                );
              })}
            </div>
          </div>
        )}

        {step === 'time' && selectedDate && (
          <div className="space-y-4">
            <button 
              onClick={() => setStep('date')}
              className="flex items-center gap-2 text-sm text-muted-foreground hover:text-foreground"
              data-testid="button-back-date"
            >
              <ArrowLeft className="size-4" />
              Voltar
            </button>

            <div className="text-center py-2 bg-muted/50 rounded-lg">
              <p className="font-medium">
                {format(selectedDate, "EEEE, d 'de' MMMM", { locale: ptBR })}
              </p>
            </div>

            {loadingSlots ? (
              <div className="grid grid-cols-3 gap-2">
                {[1,2,3,4,5,6].map(i => (
                  <Skeleton key={i} className="h-10 rounded-lg" />
                ))}
              </div>
            ) : slotsData?.slots?.length === 0 ? (
              <div className="text-center py-8 text-muted-foreground">
                <Clock className="size-8 mx-auto mb-2 opacity-50" />
                <p>Nenhum horário disponível neste dia</p>
                {slotsData?.blockedReason && (
                  <p className="text-sm mt-1">{slotsData.blockedReason}</p>
                )}
              </div>
            ) : (
              <div className="grid grid-cols-3 gap-2 max-h-64 overflow-y-auto">
                {slotsData?.slots?.map((slot: string) => (
                  <Button
                    key={slot}
                    variant={selectedSlot === slot ? "default" : "outline"}
                    onClick={() => handleSlotSelect(slot)}
                    className="w-full"
                    data-testid={`slot-${slot}`}
                  >
                    {format(new Date(slot), 'HH:mm')}
                  </Button>
                ))}
              </div>
            )}
          </div>
        )}

        {step === 'form' && selectedSlot && (
          <form onSubmit={handleSubmit} className="space-y-4">
            <button 
              type="button"
              onClick={() => setStep('time')}
              className="flex items-center gap-2 text-sm text-muted-foreground hover:text-foreground"
              data-testid="button-back-time"
            >
              <ArrowLeft className="size-4" />
              Voltar
            </button>

            <div className="text-center py-3 bg-muted/50 rounded-lg space-y-1">
              <p className="font-medium">
                {format(new Date(selectedSlot), "EEEE, d 'de' MMMM", { locale: ptBR })}
              </p>
              <p className="text-primary font-semibold">
                {format(new Date(selectedSlot), 'HH:mm')} - {format(new Date(new Date(selectedSlot).getTime() + scheduler.durationMinutes * 60000), 'HH:mm')}
              </p>
            </div>

            <div className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="clientName" className="flex items-center gap-2">
                  <User className="size-4" />
                  Nome completo *
                </Label>
                <Input
                  id="clientName"
                  value={formData.clientName}
                  onChange={e => setFormData({ ...formData, clientName: e.target.value })}
                  placeholder="Seu nome"
                  required
                  data-testid="input-client-name"
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="clientEmail" className="flex items-center gap-2">
                  <Mail className="size-4" />
                  E-mail *
                </Label>
                <Input
                  id="clientEmail"
                  type="email"
                  value={formData.clientEmail}
                  onChange={e => setFormData({ ...formData, clientEmail: e.target.value })}
                  placeholder="seu@email.com"
                  required
                  data-testid="input-client-email"
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="clientPhone" className="flex items-center gap-2">
                  <Phone className="size-4" />
                  Telefone
                </Label>
                <Input
                  id="clientPhone"
                  value={formData.clientPhone}
                  onChange={e => setFormData({ ...formData, clientPhone: e.target.value })}
                  placeholder="(11) 99999-9999"
                  data-testid="input-client-phone"
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="notes" className="flex items-center gap-2">
                  <FileText className="size-4" />
                  Observações
                </Label>
                <Textarea
                  id="notes"
                  value={formData.notes}
                  onChange={e => setFormData({ ...formData, notes: e.target.value })}
                  placeholder="Alguma informação adicional para a reunião..."
                  rows={3}
                  data-testid="input-notes"
                />
              </div>
            </div>

            <Button 
              type="submit" 
              className="w-full" 
              size="lg"
              disabled={bookMutation.isPending}
              data-testid="button-confirm-booking"
            >
              {bookMutation.isPending ? 'Agendando...' : 'Confirmar Agendamento'}
            </Button>
          </form>
        )}

        {step === 'success' && bookingResult && (
          <div className="text-center space-y-6">
            <div className="size-16 rounded-full bg-green-100 dark:bg-green-900/30 flex items-center justify-center mx-auto">
              <Check className="size-8 text-green-600 dark:text-green-400" />
            </div>

            <div>
              <h2 className="text-xl font-semibold mb-2">Agendamento Confirmado!</h2>
              <p className="text-muted-foreground">
                Um e-mail de confirmação foi enviado para {bookingResult.clientEmail}
              </p>
            </div>

            <TactileCard className="p-4 text-left bg-muted/50">
              <div className="space-y-2 text-sm">
                <div className="flex justify-between">
                  <span className="text-muted-foreground">Data:</span>
                  <span className="font-medium">
                    {format(new Date(bookingResult.startTime), "dd/MM/yyyy", { locale: ptBR })}
                  </span>
                </div>
                <div className="flex justify-between">
                  <span className="text-muted-foreground">Horário:</span>
                  <span className="font-medium">
                    {format(new Date(bookingResult.startTime), "HH:mm")} - {format(new Date(bookingResult.endTime), "HH:mm")}
                  </span>
                </div>
                <div className="flex justify-between">
                  <span className="text-muted-foreground">Tipo:</span>
                  <span className="font-medium">{bookingResult.schedulerName}</span>
                </div>
              </div>
            </TactileCard>

            {bookingResult.meetingLink && (
              <Button asChild className="w-full gap-2" size="lg">
                <a href={bookingResult.meetingLink} target="_blank" rel="noopener noreferrer">
                  <Video className="size-4" />
                  Entrar no Google Meet
                </a>
              </Button>
            )}
          </div>
        )}
      </TactileCard>
    </div>
  );
}
