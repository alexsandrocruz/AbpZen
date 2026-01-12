import twilio from 'twilio';
import { db } from './storage';
import * as schema from '@shared/schema';
import { eq, and, desc, isNull } from 'drizzle-orm';

const globalAccountSid = process.env.TWILIO_ACCOUNT_SID;
const globalAuthToken = process.env.TWILIO_AUTH_TOKEN;
const globalWhatsappNumber = process.env.TWILIO_WHATSAPP_NUMBER || process.env.TWILIO_PHONE_NUMBER;

interface TwilioCredentials {
  accountSid: string;
  authToken: string;
  whatsappNumber: string;
  isWorkspaceCredentials: boolean;
}

async function getTwilioCredentials(workspaceId?: string): Promise<TwilioCredentials | null> {
  // Try workspace-specific credentials first
  if (workspaceId) {
    const settings = await db.query.whatsappSettings.findFirst({
      where: eq(schema.whatsappSettings.workspaceId, workspaceId),
    });
    
    if (settings?.twilioAccountSid && settings?.twilioAuthToken && settings?.twilioPhoneNumber) {
      return {
        accountSid: settings.twilioAccountSid,
        authToken: settings.twilioAuthToken,
        whatsappNumber: settings.twilioPhoneNumber,
        isWorkspaceCredentials: true,
      };
    }
  }
  
  // Fall back to global credentials
  if (globalAccountSid && globalAuthToken && globalWhatsappNumber) {
    return {
      accountSid: globalAccountSid,
      authToken: globalAuthToken,
      whatsappNumber: globalWhatsappNumber,
      isWorkspaceCredentials: false,
    };
  }
  
  return null;
}

function getTwilioClient(credentials: TwilioCredentials) {
  return twilio(credentials.accountSid, credentials.authToken);
}

function formatWhatsappNumber(phone: string): string {
  let cleaned = phone.replace(/\D/g, '');
  
  if (!cleaned.startsWith('55') && cleaned.length <= 11) {
    cleaned = '55' + cleaned;
  }
  
  return 'whatsapp:+' + cleaned;
}

function formatFromNumber(phone: string): string {
  const cleaned = phone.replace(/\D/g, '');
  return 'whatsapp:+' + cleaned;
}

interface SendWhatsappOptions {
  to: string;
  toName?: string;
  message: string;
  workspaceId?: string;
  clientId?: string;
  templateId?: string;
  isFromChatbot?: boolean;
  chatbotSessionId?: string;
  metadata?: Record<string, any>;
}

export async function sendWhatsapp(options: SendWhatsappOptions): Promise<{ success: boolean; messageId?: string; error?: string; logId?: string }> {
  const credentials = await getTwilioCredentials(options.workspaceId);
  
  if (!credentials) {
    return { success: false, error: 'Twilio credentials not configured. Set TWILIO_ACCOUNT_SID, TWILIO_AUTH_TOKEN, and TWILIO_WHATSAPP_NUMBER.' };
  }

  const toFormatted = formatWhatsappNumber(options.to);
  const fromFormatted = formatFromNumber(credentials.whatsappNumber);

  const logEntry = await db.insert(schema.whatsappLogs).values({
    workspaceId: options.workspaceId || null,
    templateId: options.templateId || null,
    clientId: options.clientId || null,
    direction: 'OUTBOUND',
    toPhone: toFormatted,
    toName: options.toName || null,
    fromPhone: fromFormatted,
    content: options.message,
    status: 'PENDING',
    isFromChatbot: options.isFromChatbot || false,
    chatbotSessionId: options.chatbotSessionId || null,
    metadata: options.metadata ? JSON.stringify(options.metadata) : null,
  }).returning();

  const logId = logEntry[0]?.id;

  try {
    const client = getTwilioClient(credentials);
    
    const credentialsSource = credentials.isWorkspaceCredentials ? 'workspace' : 'global';
    console.log(`Sending WhatsApp to ${toFormatted} from ${fromFormatted} (using ${credentialsSource} credentials)`);
    
    const message = await client.messages.create({
      body: options.message,
      from: fromFormatted,
      to: toFormatted,
    });

    if (logId) {
      await db.update(schema.whatsappLogs)
        .set({
          status: 'SENT',
          twilioMessageId: message.sid,
          sentAt: new Date(),
        })
        .where(eq(schema.whatsappLogs.id, logId));
    }

    console.log(`WhatsApp sent successfully. SID: ${message.sid}, Status: ${message.status}`);
    return { success: true, messageId: message.sid, logId };
  } catch (error: any) {
    console.error('WhatsApp send error:', error);
    if (logId) {
      await db.update(schema.whatsappLogs)
        .set({ 
          status: 'FAILED',
          failedAt: new Date(),
          errorMessage: error.message,
        })
        .where(eq(schema.whatsappLogs.id, logId));
    }
    return { success: false, error: error.message };
  }
}

function replaceVariables(content: string, variables: Record<string, string>): string {
  let result = content;
  for (const [key, value] of Object.entries(variables)) {
    result = result.replace(new RegExp(`\\{\\{${key}\\}\\}`, 'g'), value || '');
  }
  return result;
}

export async function sendTemplateWhatsapp(
  triggerType: schema.WhatsappTriggerType,
  to: string,
  variables: Record<string, string>,
  options?: {
    workspaceId?: string;
    clientId?: string;
    toName?: string;
    metadata?: Record<string, any>;
  }
): Promise<{ success: boolean; error?: string }> {
  try {
    // Check subscription plan if workspace is provided
    if (options?.workspaceId) {
      const hasPermission = await canUseWhatsapp(options.workspaceId);
      if (!hasPermission) {
        console.log(`WhatsApp blocked for workspace ${options.workspaceId}: subscription does not allow WhatsApp`);
        return { success: false, error: 'Subscription plan does not include WhatsApp' };
      }
    }
    
    const template = await db.query.whatsappTemplates.findFirst({
      where: and(
        eq(schema.whatsappTemplates.triggerType, triggerType),
        eq(schema.whatsappTemplates.isActive, true),
        options?.workspaceId 
          ? eq(schema.whatsappTemplates.workspaceId, options.workspaceId)
          : eq(schema.whatsappTemplates.isSystem, true)
      ),
    });

    if (!template) {
      const systemTemplate = await db.query.whatsappTemplates.findFirst({
        where: and(
          eq(schema.whatsappTemplates.triggerType, triggerType),
          eq(schema.whatsappTemplates.isSystem, true),
          eq(schema.whatsappTemplates.isActive, true)
        ),
      });
      
      if (!systemTemplate) {
        return { success: false, error: `No active WhatsApp template found for trigger: ${triggerType}` };
      }
      
      const message = replaceVariables(systemTemplate.content, variables);

      return await sendWhatsapp({
        to,
        toName: options?.toName,
        message,
        workspaceId: options?.workspaceId,
        clientId: options?.clientId,
        templateId: systemTemplate.id,
        metadata: options?.metadata,
      });
    }

    const message = replaceVariables(template.content, variables);

    return await sendWhatsapp({
      to,
      toName: options?.toName,
      message,
      workspaceId: options?.workspaceId,
      clientId: options?.clientId,
      templateId: template.id,
      metadata: options?.metadata,
    });
  } catch (error: any) {
    console.error('Template WhatsApp error:', error);
    return { success: false, error: error.message };
  }
}

export async function sendTestWhatsapp(to: string): Promise<{ success: boolean; messageId?: string; error?: string }> {
  return sendWhatsapp({
    to,
    message: '🎉 Olá! Este é uma mensagem de teste do Dominus via WhatsApp. Sua integração está funcionando perfeitamente!',
  });
}

export async function sendWhatsappBySlug(
  workspaceId: string,
  to: string,
  templateSlug: string,
  variables: Record<string, string>,
  options?: {
    toName?: string;
    clientId?: string;
    metadata?: Record<string, any>;
  }
): Promise<{ success: boolean; messageId?: string; error?: string }> {
  try {
    const quotaCheck = await canUseWhatsapp(workspaceId);
    if (!quotaCheck.allowed) {
      console.log(`WhatsApp blocked for workspace ${workspaceId}: ${quotaCheck.reason}`);
      return { success: false, error: quotaCheck.reason || 'WhatsApp not allowed' };
    }

    const template = await db.query.whatsappTemplates.findFirst({
      where: and(
        eq(schema.whatsappTemplates.slug, templateSlug),
        eq(schema.whatsappTemplates.isActive, true),
        eq(schema.whatsappTemplates.workspaceId, workspaceId)
      ),
    });

    let result;
    if (!template) {
      const systemTemplate = await db.query.whatsappTemplates.findFirst({
        where: and(
          eq(schema.whatsappTemplates.slug, templateSlug),
          eq(schema.whatsappTemplates.isSystem, true),
          eq(schema.whatsappTemplates.isActive, true)
        ),
      });
      
      if (!systemTemplate) {
        return { success: false, error: `No active WhatsApp template found with slug: ${templateSlug}` };
      }
      
      const message = replaceVariables(systemTemplate.content, variables);

      result = await sendWhatsapp({
        to,
        toName: options?.toName,
        message,
        workspaceId,
        clientId: options?.clientId,
        templateId: systemTemplate.id,
        metadata: options?.metadata,
      });
    } else {
      const message = replaceVariables(template.content, variables);

      result = await sendWhatsapp({
        to,
        toName: options?.toName,
        message,
        workspaceId,
        clientId: options?.clientId,
        templateId: template.id,
        metadata: options?.metadata,
      });
    }

    if (result.success) {
      await incrementWhatsappUsage(workspaceId);
    }

    return result;
  } catch (error: any) {
    console.error('Send WhatsApp by slug error:', error);
    return { success: false, error: error.message };
  }
}

export async function getWhatsappSettings(workspaceId: string): Promise<schema.WhatsappSettings | null> {
  const settings = await db.query.whatsappSettings.findFirst({
    where: eq(schema.whatsappSettings.workspaceId, workspaceId),
  });
  return settings || null;
}

export async function updateWhatsappSettings(workspaceId: string, data: Partial<schema.InsertWhatsappSettings>): Promise<schema.WhatsappSettings> {
  const existing = await getWhatsappSettings(workspaceId);
  
  if (existing) {
    const [updated] = await db.update(schema.whatsappSettings)
      .set({ ...data, updatedAt: new Date() })
      .where(eq(schema.whatsappSettings.workspaceId, workspaceId))
      .returning();
    return updated;
  }
  
  const [created] = await db.insert(schema.whatsappSettings)
    .values({ workspaceId, ...data })
    .returning();
  return created;
}

export async function findClientByPhone(phone: string): Promise<{ client: schema.Client; workspaceId: string } | null> {
  const formattedPhone = phone.replace(/\D/g, '');
  
  const clients = await db.query.clients.findMany({
    where: eq(schema.clients.status, 'ACTIVE'),
  });
  
  for (const client of clients) {
    if (client.phone) {
      const clientPhone = client.phone.replace(/\D/g, '');
      if (clientPhone === formattedPhone || 
          clientPhone.endsWith(formattedPhone) || 
          formattedPhone.endsWith(clientPhone)) {
        return { client, workspaceId: client.workspaceId };
      }
    }
  }
  
  return null;
}

export async function findWorkspaceByLastMessage(phone: string): Promise<string | null> {
  const formattedPhone = formatWhatsappNumber(phone);
  
  const lastMessage = await db.query.whatsappLogs.findFirst({
    where: eq(schema.whatsappLogs.toPhone, formattedPhone),
    orderBy: [desc(schema.whatsappLogs.createdAt)],
  });
  
  return lastMessage?.workspaceId || null;
}

export async function getOrCreateChatbotSession(
  phoneNumber: string, 
  workspaceId: string,
  clientId?: string
): Promise<schema.WhatsappChatbotSession> {
  const formattedPhone = phoneNumber.replace(/\D/g, '');
  
  const existingSession = await db.query.whatsappChatbotSessions.findFirst({
    where: and(
      eq(schema.whatsappChatbotSessions.phoneNumber, formattedPhone),
      eq(schema.whatsappChatbotSessions.workspaceId, workspaceId),
      eq(schema.whatsappChatbotSessions.isActive, true)
    ),
  });
  
  if (existingSession) {
    await db.update(schema.whatsappChatbotSessions)
      .set({ lastMessageAt: new Date(), updatedAt: new Date() })
      .where(eq(schema.whatsappChatbotSessions.id, existingSession.id));
    return existingSession;
  }
  
  const [newSession] = await db.insert(schema.whatsappChatbotSessions)
    .values({
      workspaceId,
      clientId: clientId || null,
      phoneNumber: formattedPhone,
      isActive: true,
    })
    .returning();
  
  return newSession;
}

export async function updateChatbotSession(
  sessionId: string,
  data: { currentIntent?: string; contextData?: string }
): Promise<void> {
  await db.update(schema.whatsappChatbotSessions)
    .set({ ...data, updatedAt: new Date() })
    .where(eq(schema.whatsappChatbotSessions.id, sessionId));
}

export async function logIncomingWhatsapp(
  phone: string,
  content: string,
  workspaceId?: string,
  clientId?: string,
  chatbotSessionId?: string
): Promise<string> {
  const formattedPhone = formatWhatsappNumber(phone);
  
  // Get the receiving number (our number) from credentials
  const credentials = await getTwilioCredentials(workspaceId);
  const toNumber = credentials ? formatFromNumber(credentials.whatsappNumber) : 'unknown';
  
  const [log] = await db.insert(schema.whatsappLogs).values({
    workspaceId: workspaceId || null,
    clientId: clientId || null,
    direction: 'INBOUND',
    toPhone: toNumber,
    fromPhone: formattedPhone,
    content,
    status: 'DELIVERED',
    deliveredAt: new Date(),
    isFromChatbot: false,
    chatbotSessionId: chatbotSessionId || null,
  }).returning();
  
  return log.id;
}

export async function initializeSystemWhatsappTemplates(): Promise<void> {
  const existingTemplates = await db.query.whatsappTemplates.findMany({
    where: eq(schema.whatsappTemplates.isSystem, true),
  });

  const systemTemplates = [
    {
      name: 'Boas-vindas',
      slug: 'boas_vindas',
      triggerType: 'WELCOME' as const,
      content: 'Olá {{nome}}! 👋 Seja bem-vindo(a) ao {{workspace}}. Estamos muito felizes em tê-lo conosco!',
    },
    {
      name: 'Proposta Enviada',
      slug: 'proposta_enviada',
      triggerType: 'PROPOSAL_SENT' as const,
      content: 'Olá {{nome}}! 📋 Sua proposta *{{proposta}}* foi enviada. Acesse para visualizar: {{link}}',
    },
    {
      name: 'Proposta Aceita',
      slug: 'proposta_aceita',
      triggerType: 'PROPOSAL_ACCEPTED' as const,
      content: '🎉 Ótima notícia! A proposta *{{proposta}}* foi aceita por {{cliente}}.',
    },
    {
      name: 'Proposta Recusada',
      slug: 'proposta_recusada',
      triggerType: 'PROPOSAL_REJECTED' as const,
      content: 'A proposta *{{proposta}}* foi recusada por {{cliente}}.',
    },
    {
      name: 'Contrato Enviado',
      slug: 'contrato_enviado',
      triggerType: 'CONTRACT_SENT' as const,
      content: 'Olá {{nome}}! 📝 Seu contrato está pronto para assinatura. Acesse: {{link}}',
    },
    {
      name: 'Contrato Assinado',
      slug: 'contrato_assinado',
      triggerType: 'CONTRACT_SIGNED' as const,
      content: '✅ O contrato *{{contrato}}* foi assinado por {{cliente}}.',
    },
    {
      name: 'Fatura Enviada',
      slug: 'fatura_enviada',
      triggerType: 'INVOICE_SENT' as const,
      content: 'Olá {{nome}}! 💰 Fatura *#{{numero}}* no valor de R$ {{valor}}.\n📅 Vencimento: {{vencimento}}\n🔗 Acesse: {{link}}',
    },
    {
      name: 'Fatura Paga',
      slug: 'fatura_paga',
      triggerType: 'INVOICE_PAID' as const,
      content: '💚 Recebemos seu pagamento da fatura *#{{numero}}*. Obrigado!',
    },
    {
      name: 'Fatura Vencida',
      slug: 'fatura_vencida',
      triggerType: 'INVOICE_OVERDUE' as const,
      content: '⚠️ Olá {{nome}}! A fatura *#{{numero}}* (R$ {{valor}}) está vencida.\n\nPor favor, regularize: {{link}}',
    },
    {
      name: 'Lembrete de Fatura',
      slug: 'lembrete_fatura',
      triggerType: 'INVOICE_REMINDER' as const,
      content: '🔔 Olá {{nome}}! Lembrete: fatura *#{{numero}}* vence em *{{dias}} dia(s)*.\n💰 Valor: R$ {{valor}}',
    },
    {
      name: 'Agendamento Confirmado',
      slug: 'agendamento_confirmado',
      triggerType: 'BOOKING_CONFIRMED' as const,
      content: '✅ Seu agendamento foi confirmado!\n\n📅 Data: {{data}}\n⏰ Horário: {{hora}}\n📍 Local: {{local}}',
    },
    {
      name: 'Lembrete de Agendamento',
      slug: 'lembrete_agendamento',
      triggerType: 'BOOKING_REMINDER' as const,
      content: '🔔 Lembrete: Você tem um compromisso amanhã!\n\n📅 {{data}} às {{hora}}\n📍 {{local}}',
    },
    {
      name: 'Agendamento Cancelado',
      slug: 'agendamento_cancelado',
      triggerType: 'BOOKING_CANCELLED' as const,
      content: '❌ Seu agendamento de {{data}} às {{hora}} foi cancelado.',
    },
    {
      name: 'Agendamento Remarcado',
      slug: 'agendamento_remarcado',
      triggerType: 'BOOKING_RESCHEDULED' as const,
      content: '📅 Seu agendamento foi remarcado!\n\nNova data: {{data}}\nNovo horário: {{hora}}\n📍 Local: {{local}}',
    },
    {
      name: 'Tarefa Atribuída',
      slug: 'tarefa_atribuida',
      triggerType: 'TASK_ASSIGNED' as const,
      content: '📋 Uma nova tarefa foi atribuída a você: *{{tarefa}}*\n\nProjeto: {{projeto}}',
    },
    {
      name: 'Tarefa Concluída',
      slug: 'tarefa_concluida',
      triggerType: 'TASK_COMPLETED' as const,
      content: '✅ A tarefa *{{tarefa}}* foi concluída por {{responsavel}}.',
    },
  ];

  for (const template of systemTemplates) {
    const exists = existingTemplates.find(t => t.triggerType === template.triggerType);
    if (!exists) {
      await db.insert(schema.whatsappTemplates).values({
        ...template,
        workspaceId: null,
        isSystem: true,
        isActive: true,
      });
      console.log(`Created system WhatsApp template: ${template.name}`);
    }
  }
}

export async function canUseWhatsapp(workspaceId: string): Promise<{ allowed: boolean; reason?: string; remaining?: number }> {
  const subscription = await db.query.workspaceSubscriptions.findFirst({
    where: eq(schema.workspaceSubscriptions.workspaceId, workspaceId),
  });
  
  if (!subscription) {
    return { allowed: false, reason: 'No subscription found' };
  }
  
  const now = new Date();
  
  // Check and reset quota if needed
  if (subscription.quotaResetAt && new Date(subscription.quotaResetAt) <= now) {
    await resetWhatsappQuota(workspaceId);
  }
  
  if (subscription.status === 'TRIAL' && subscription.trialEndsAt) {
    if (now < subscription.trialEndsAt) {
      const trialLimit = 30; // Trial gets 30 WhatsApp messages
      const remaining = trialLimit - subscription.whatsappUsed;
      if (remaining <= 0) {
        return { allowed: false, reason: 'Trial WhatsApp quota exceeded', remaining: 0 };
      }
      return { allowed: true, remaining };
    }
  }
  
  if (!subscription.canUseWhatsapp) {
    return { allowed: false, reason: 'WhatsApp not included in plan' };
  }
  
  // Check quota (0 = unlimited)
  if (subscription.whatsappQuota > 0) {
    const remaining = subscription.whatsappQuota - subscription.whatsappUsed;
    if (remaining <= 0) {
      return { allowed: false, reason: 'WhatsApp quota exceeded', remaining: 0 };
    }
    return { allowed: true, remaining };
  }
  
  return { allowed: true }; // Unlimited
}

export async function incrementWhatsappUsage(workspaceId: string): Promise<void> {
  const sub = await db.query.workspaceSubscriptions.findFirst({
    where: eq(schema.workspaceSubscriptions.workspaceId, workspaceId),
  });
  
  await db.update(schema.workspaceSubscriptions)
    .set({ whatsappUsed: (sub?.whatsappUsed || 0) + 1 })
    .where(eq(schema.workspaceSubscriptions.workspaceId, workspaceId));
}

export async function getWhatsappUsage(workspaceId: string): Promise<{ 
  used: number; 
  quota: number; 
  remaining: number;
  percentUsed: number;
  resetsAt: string;
  hasCustomCredentials: boolean;
}> {
  const subscription = await db.query.workspaceSubscriptions.findFirst({
    where: eq(schema.workspaceSubscriptions.workspaceId, workspaceId),
  });
  
  const settings = await db.query.whatsappSettings.findFirst({
    where: eq(schema.whatsappSettings.workspaceId, workspaceId),
  });
  
  const hasCustomCredentials = !!(settings?.twilioAccountSid && settings?.twilioAuthToken && settings?.twilioPhoneNumber);
  
  if (!subscription) {
    return { used: 0, quota: 0, remaining: 0, percentUsed: 0, resetsAt: '', hasCustomCredentials };
  }
  
  const isTrial = subscription.status === 'TRIAL' && 
    subscription.trialEndsAt && 
    new Date(subscription.trialEndsAt) > new Date();
  
  const quota = isTrial ? 30 : subscription.whatsappQuota;
  const remaining = quota === 0 ? -1 : Math.max(0, quota - subscription.whatsappUsed);
  const percentUsed = quota === 0 ? 0 : Math.round((subscription.whatsappUsed / quota) * 100);
  const resetsAt = subscription.quotaResetAt?.toISOString() || '';
  
  return { used: subscription.whatsappUsed, quota, remaining, percentUsed, resetsAt, hasCustomCredentials };
}

async function resetWhatsappQuota(workspaceId: string): Promise<void> {
  const nextReset = new Date();
  nextReset.setMonth(nextReset.getMonth() + 1);
  nextReset.setDate(1);
  nextReset.setHours(0, 0, 0, 0);
  
  await db.update(schema.workspaceSubscriptions)
    .set({ 
      smsUsed: 0, 
      whatsappUsed: 0, 
      quotaResetAt: nextReset 
    })
    .where(eq(schema.workspaceSubscriptions.workspaceId, workspaceId));
}

export async function canUseChatbot(workspaceId: string): Promise<boolean> {
  const subscription = await db.query.workspaceSubscriptions.findFirst({
    where: eq(schema.workspaceSubscriptions.workspaceId, workspaceId),
  });
  
  if (!subscription) {
    return false;
  }
  
  if (subscription.status === 'TRIAL' && subscription.trialEndsAt) {
    const now = new Date();
    if (now < subscription.trialEndsAt) {
      return true;
    }
  }
  
  return subscription.canUseChatbot;
}
