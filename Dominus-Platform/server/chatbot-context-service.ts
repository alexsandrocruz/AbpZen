import { db } from './storage';
import * as schema from '@shared/schema';
import { eq, and, gte, lte, desc, asc, sql } from 'drizzle-orm';
import { addDays, startOfDay, endOfDay, format } from 'date-fns';
import { ptBR } from 'date-fns/locale';

export interface BusinessContext {
  workspaceId: string;
  businessName: string;
  businessAddress: string | null;
  businessHours: Record<string, string> | null;
  services: Array<{
    id: string;
    name: string;
    description: string | null;
    durationMinutes: number;
  }>;
}

export interface ClientContext {
  clientId: string;
  name: string;
  email: string | null;
  phone: string | null;
  upcomingBookings: Array<{
    id: string;
    serviceName: string;
    date: string;
    time: string;
    status: string;
  }>;
  recentInvoices: Array<{
    id: string;
    number: string;
    status: string;
    total: number;
    dueDate: string;
  }>;
  recentProposals: Array<{
    id: string;
    title: string;
    status: string;
  }>;
}

export interface AvailabilitySlot {
  date: string;
  dayOfWeek: string;
  slots: string[];
}

async function getBusinessContext(workspaceId: string): Promise<BusinessContext | null> {
  const workspace = await db.query.workspaces.findFirst({
    where: eq(schema.workspaces.id, workspaceId),
  });
  
  if (!workspace) {
    return null;
  }
  
  const whatsappSettings = await db.query.whatsappSettings.findFirst({
    where: eq(schema.whatsappSettings.workspaceId, workspaceId),
  });
  
  const schedulerTypes = await db.query.schedulerTypes.findMany({
    where: and(
      eq(schema.schedulerTypes.workspaceId, workspaceId),
      eq(schema.schedulerTypes.isActive, true)
    ),
  });
  
  let businessHours: Record<string, string> | null = null;
  if (whatsappSettings?.businessHours) {
    try {
      businessHours = JSON.parse(whatsappSettings.businessHours);
    } catch {
      businessHours = null;
    }
  }
  
  return {
    workspaceId,
    businessName: workspace.name,
    businessAddress: whatsappSettings?.businessAddress || null,
    businessHours,
    services: schedulerTypes.map(s => ({
      id: s.id,
      name: s.name,
      description: s.description,
      durationMinutes: s.durationMinutes,
    })),
  };
}

async function getClientContext(clientId: string, workspaceId: string): Promise<ClientContext | null> {
  const client = await db.query.clients.findFirst({
    where: and(
      eq(schema.clients.id, clientId),
      eq(schema.clients.workspaceId, workspaceId)
    ),
  });
  
  if (!client) {
    return null;
  }
  
  const now = new Date();
  
  const upcomingBookings = await db
    .select({
      booking: schema.schedulerBookings,
      service: schema.schedulerTypes,
    })
    .from(schema.schedulerBookings)
    .leftJoin(schema.schedulerTypes, eq(schema.schedulerBookings.schedulerTypeId, schema.schedulerTypes.id))
    .where(
      and(
        eq(schema.schedulerBookings.clientId, clientId),
        gte(schema.schedulerBookings.startTime, now)
      )
    )
    .orderBy(asc(schema.schedulerBookings.startTime))
    .limit(5);
  
  const invoices = await db.query.invoices.findMany({
    where: and(
      eq(schema.invoices.clientId, clientId),
      eq(schema.invoices.workspaceId, workspaceId)
    ),
    orderBy: desc(schema.invoices.createdAt),
    limit: 5,
  });
  
  const invoiceItems = await Promise.all(
    invoices.map(async (inv) => {
      const items = await db.query.invoiceItems.findMany({
        where: eq(schema.invoiceItems.invoiceId, inv.id),
      });
      const total = items.reduce((sum, item) => sum + parseFloat(String(item.price || 0)) * (item.quantity || 1), 0);
      return { invoiceId: inv.id, total };
    })
  );
  
  const proposals = await db.query.proposals.findMany({
    where: and(
      eq(schema.proposals.clientId, clientId),
      eq(schema.proposals.workspaceId, workspaceId)
    ),
    orderBy: desc(schema.proposals.createdAt),
    limit: 3,
  });
  
  return {
    clientId,
    name: client.name,
    email: client.email,
    phone: client.phone,
    upcomingBookings: upcomingBookings.map(({ booking, service }) => ({
      id: booking.id,
      serviceName: service?.name || 'Serviço',
      date: format(booking.startTime, "dd/MM/yyyy", { locale: ptBR }),
      time: format(booking.startTime, "HH:mm", { locale: ptBR }),
      status: booking.status || 'PENDING',
    })),
    recentInvoices: invoices.map(inv => {
      const itemData = invoiceItems.find(i => i.invoiceId === inv.id);
      return {
        id: inv.id,
        number: inv.number || '',
        status: inv.status || 'DRAFT',
        total: itemData?.total || 0,
        dueDate: inv.dueDate ? format(new Date(inv.dueDate), "dd/MM/yyyy", { locale: ptBR }) : '',
      };
    }),
    recentProposals: proposals.map(p => ({
      id: p.id,
      title: p.title || 'Proposta',
      status: p.status || 'DRAFT',
    })),
  };
}

const DAY_NAMES = ['Domingo', 'Segunda-feira', 'Terça-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sábado'];

async function getAvailability(workspaceId: string, serviceId: string, daysAhead: number = 14): Promise<AvailabilitySlot[]> {
  const service = await db.query.schedulerTypes.findFirst({
    where: and(
      eq(schema.schedulerTypes.id, serviceId),
      eq(schema.schedulerTypes.workspaceId, workspaceId)
    ),
  });
  
  if (!service) {
    return [];
  }
  
  const availability = await db.query.schedulerAvailability.findMany({
    where: eq(schema.schedulerAvailability.schedulerTypeId, serviceId),
  });
  
  const exceptions = await db.query.schedulerExceptions.findMany({
    where: eq(schema.schedulerExceptions.schedulerTypeId, serviceId),
  });
  
  const now = new Date();
  const slots: AvailabilitySlot[] = [];
  
  for (let i = 0; i < daysAhead; i++) {
    const date = addDays(now, i);
    const dayOfWeek = date.getDay();
    const dateStr = format(date, "yyyy-MM-dd");
    
    const exception = exceptions.find(e => e.date === dateStr);
    if (exception?.isBlocked) {
      continue;
    }
    
    let startTime: string | null = null;
    let endTime: string | null = null;
    
    if (exception && exception.startTime && exception.endTime) {
      startTime = exception.startTime;
      endTime = exception.endTime;
    } else {
      const dayAvailability = availability.find(a => a.dayOfWeek === dayOfWeek);
      if (dayAvailability) {
        startTime = dayAvailability.startTime;
        endTime = dayAvailability.endTime;
      }
    }
    
    if (!startTime || !endTime) {
      continue;
    }
    
    const existingBookings = await db.query.schedulerBookings.findMany({
      where: and(
        eq(schema.schedulerBookings.schedulerTypeId, serviceId),
        gte(schema.schedulerBookings.startTime, startOfDay(date)),
        lte(schema.schedulerBookings.startTime, endOfDay(date)),
        sql`${schema.schedulerBookings.status} != 'CANCELLED'`
      ),
    });
    
    const bookedTimes = new Set(
      existingBookings.map(b => format(b.startTime, "HH:mm"))
    );
    
    const timeSlots: string[] = [];
    const [startHour, startMin] = startTime.split(':').map(Number);
    const [endHour, endMin] = endTime.split(':').map(Number);
    
    const slotDuration = service.durationMinutes + (service.bufferAfterMinutes || 0);
    let currentMinutes = startHour * 60 + startMin;
    const endMinutes = endHour * 60 + endMin;
    
    while (currentMinutes + service.durationMinutes <= endMinutes) {
      const hour = Math.floor(currentMinutes / 60);
      const min = currentMinutes % 60;
      const timeStr = `${hour.toString().padStart(2, '0')}:${min.toString().padStart(2, '0')}`;
      
      if (!bookedTimes.has(timeStr)) {
        const slotDate = new Date(date);
        slotDate.setHours(hour, min, 0, 0);
        if (slotDate > now) {
          timeSlots.push(timeStr);
        }
      }
      
      currentMinutes += slotDuration;
    }
    
    if (timeSlots.length > 0) {
      slots.push({
        date: format(date, "dd/MM/yyyy", { locale: ptBR }),
        dayOfWeek: DAY_NAMES[dayOfWeek],
        slots: timeSlots,
      });
    }
  }
  
  return slots;
}

export interface ChatbotFullContext {
  business: BusinessContext | null;
  client: ClientContext | null;
  availability: AvailabilitySlot[];
}

export async function loadChatbotContext(
  workspaceId: string,
  clientPhone: string,
  serviceId?: string
): Promise<ChatbotFullContext> {
  const business = await getBusinessContext(workspaceId);
  
  const client = await db.query.clients.findFirst({
    where: and(
      eq(schema.clients.workspaceId, workspaceId),
      eq(schema.clients.phone, clientPhone)
    ),
  });
  
  let clientContext: ClientContext | null = null;
  if (client) {
    clientContext = await getClientContext(client.id, workspaceId);
  }
  
  let availability: AvailabilitySlot[] = [];
  if (serviceId) {
    availability = await getAvailability(workspaceId, serviceId);
  } else if (business && business.services.length > 0) {
    availability = await getAvailability(workspaceId, business.services[0].id);
  }
  
  return {
    business,
    client: clientContext,
    availability,
  };
}

export function formatContextForAI(context: ChatbotFullContext): string {
  const lines: string[] = [];
  
  if (context.business) {
    lines.push(`## Informações do Negócio`);
    lines.push(`- Nome: ${context.business.businessName}`);
    if (context.business.businessAddress) {
      lines.push(`- Endereço: ${context.business.businessAddress}`);
    }
    if (context.business.businessHours) {
      lines.push(`- Horário de funcionamento:`);
      const dayMap: Record<string, string> = {
        mon: 'Segunda', tue: 'Terça', wed: 'Quarta', thu: 'Quinta',
        fri: 'Sexta', sat: 'Sábado', sun: 'Domingo'
      };
      for (const [day, hours] of Object.entries(context.business.businessHours)) {
        if (hours) {
          lines.push(`  - ${dayMap[day] || day}: ${hours}`);
        }
      }
    }
    if (context.business.services.length > 0) {
      lines.push(`\n## Serviços Disponíveis`);
      for (const service of context.business.services) {
        lines.push(`- ${service.name} (${service.durationMinutes} min)${service.description ? ': ' + service.description : ''}`);
      }
    }
  }
  
  if (context.client) {
    lines.push(`\n## Informações do Cliente`);
    lines.push(`- Nome: ${context.client.name}`);
    if (context.client.email) {
      lines.push(`- Email: ${context.client.email}`);
    }
    
    if (context.client.upcomingBookings.length > 0) {
      lines.push(`\n### Agendamentos Futuros`);
      for (const booking of context.client.upcomingBookings) {
        lines.push(`- ${booking.serviceName} em ${booking.date} às ${booking.time} (${booking.status})`);
      }
    }
    
    if (context.client.recentInvoices.length > 0) {
      lines.push(`\n### Faturas Recentes`);
      for (const inv of context.client.recentInvoices) {
        lines.push(`- Fatura #${inv.number}: R$ ${inv.total.toFixed(2)} - Vencimento: ${inv.dueDate} (${inv.status})`);
      }
    }
  }
  
  if (context.availability.length > 0) {
    lines.push(`\n## Horários Disponíveis para Agendamento`);
    for (const day of context.availability.slice(0, 5)) {
      lines.push(`- ${day.dayOfWeek} (${day.date}): ${day.slots.slice(0, 6).join(', ')}${day.slots.length > 6 ? '...' : ''}`);
    }
  }
  
  return lines.join('\n');
}
