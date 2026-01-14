import twilio from 'twilio';
import { db } from './storage';
import * as schema from '@shared/schema';
import { eq, and } from 'drizzle-orm';

const globalAccountSid = process.env.TWILIO_ACCOUNT_SID;
const globalAuthToken = process.env.TWILIO_AUTH_TOKEN;
const globalFromNumber = process.env.TWILIO_PHONE_NUMBER;

interface TwilioCredentials {
  accountSid: string;
  authToken: string;
  fromNumber: string;
  isWorkspaceCredentials: boolean;
}

async function getTwilioCredentials(workspaceId?: string): Promise<TwilioCredentials | null> {
  // Try workspace-specific credentials first
  if (workspaceId) {
    const settings = await db.query.smsSettings.findFirst({
      where: eq(schema.smsSettings.workspaceId, workspaceId),
    });
    
    if (settings?.twilioAccountSid && settings?.twilioAuthToken && settings?.twilioPhoneNumber) {
      return {
        accountSid: settings.twilioAccountSid,
        authToken: settings.twilioAuthToken,
        fromNumber: settings.twilioPhoneNumber,
        isWorkspaceCredentials: true,
      };
    }
  }
  
  // Fall back to global credentials
  if (globalAccountSid && globalAuthToken && globalFromNumber) {
    return {
      accountSid: globalAccountSid,
      authToken: globalAuthToken,
      fromNumber: globalFromNumber,
      isWorkspaceCredentials: false,
    };
  }
  
  return null;
}

function getTwilioClient(credentials: TwilioCredentials) {
  return twilio(credentials.accountSid, credentials.authToken);
}

function formatPhoneNumber(phone: string): string {
  let cleaned = phone.replace(/\D/g, '');
  
  if (!cleaned.startsWith('55') && cleaned.length <= 11) {
    cleaned = '55' + cleaned;
  }
  
  return '+' + cleaned;
}

interface SendSmsOptions {
  to: string;
  toName?: string;
  message: string;
  workspaceId?: string;
  templateId?: string;
  metadata?: Record<string, any>;
}

export async function sendSms(options: SendSmsOptions): Promise<{ success: boolean; messageId?: string; error?: string; logId?: string }> {
  const credentials = await getTwilioCredentials(options.workspaceId);
  
  if (!credentials) {
    return { success: false, error: 'Twilio credentials not configured. Set TWILIO_ACCOUNT_SID, TWILIO_AUTH_TOKEN, and TWILIO_PHONE_NUMBER.' };
  }

  const toFormatted = formatPhoneNumber(options.to);

  const logEntry = await db.insert(schema.smsLogs).values({
    workspaceId: options.workspaceId || null,
    templateId: options.templateId || null,
    toPhone: toFormatted,
    toName: options.toName || null,
    fromPhone: credentials.fromNumber,
    content: options.message,
    status: 'PENDING',
    metadata: options.metadata ? JSON.stringify(options.metadata) : null,
  }).returning();

  const logId = logEntry[0]?.id;

  try {
    const client = getTwilioClient(credentials);
    
    const credentialsSource = credentials.isWorkspaceCredentials ? 'workspace' : 'global';
    console.log(`Sending SMS to ${toFormatted} from ${credentials.fromNumber} (using ${credentialsSource} credentials)`);
    
    const message = await client.messages.create({
      body: options.message,
      from: credentials.fromNumber,
      to: toFormatted,
    });

    if (logId) {
      await db.update(schema.smsLogs)
        .set({
          status: 'SENT',
          twilioMessageId: message.sid,
          sentAt: new Date(),
        })
        .where(eq(schema.smsLogs.id, logId));
    }

    console.log(`SMS sent successfully. SID: ${message.sid}, Status: ${message.status}`);
    return { success: true, messageId: message.sid, logId };
  } catch (error: any) {
    console.error('SMS send error:', error);
    if (logId) {
      await db.update(schema.smsLogs)
        .set({ 
          status: 'FAILED',
          failedAt: new Date(),
          errorMessage: error.message,
        })
        .where(eq(schema.smsLogs.id, logId));
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

export async function sendTemplateSms(
  triggerType: schema.SmsTriggerType,
  to: string,
  variables: Record<string, string>,
  options?: {
    workspaceId?: string;
    toName?: string;
    metadata?: Record<string, any>;
  }
): Promise<{ success: boolean; error?: string }> {
  try {
    // Check subscription plan if workspace is provided
    if (options?.workspaceId) {
      const hasPermission = await canUseSms(options.workspaceId);
      if (!hasPermission) {
        console.log(`SMS blocked for workspace ${options.workspaceId}: subscription does not allow SMS`);
        return { success: false, error: 'Subscription plan does not include SMS' };
      }
    }
    
    const template = await db.query.smsTemplates.findFirst({
      where: and(
        eq(schema.smsTemplates.triggerType, triggerType),
        eq(schema.smsTemplates.isActive, true),
        options?.workspaceId 
          ? eq(schema.smsTemplates.workspaceId, options.workspaceId)
          : eq(schema.smsTemplates.isSystem, true)
      ),
    });

    if (!template) {
      const systemTemplate = await db.query.smsTemplates.findFirst({
        where: and(
          eq(schema.smsTemplates.triggerType, triggerType),
          eq(schema.smsTemplates.isSystem, true),
          eq(schema.smsTemplates.isActive, true)
        ),
      });
      
      if (!systemTemplate) {
        return { success: false, error: `No active SMS template found for trigger: ${triggerType}` };
      }
      
      const message = replaceVariables(systemTemplate.content, variables);

      return await sendSms({
        to,
        toName: options?.toName,
        message,
        workspaceId: options?.workspaceId,
        templateId: systemTemplate.id,
        metadata: options?.metadata,
      });
    }

    const message = replaceVariables(template.content, variables);

    return await sendSms({
      to,
      toName: options?.toName,
      message,
      workspaceId: options?.workspaceId,
      templateId: template.id,
      metadata: options?.metadata,
    });
  } catch (error: any) {
    console.error('Template SMS error:', error);
    return { success: false, error: error.message };
  }
}

export async function sendTestSms(to: string): Promise<{ success: boolean; messageId?: string; error?: string }> {
  return sendSms({
    to,
    message: '🎉 Olá! Este é um SMS de teste do Dominus. Sua integração com Twilio está funcionando perfeitamente!',
  });
}

export async function sendSmsBySlug(
  workspaceId: string,
  to: string,
  templateSlug: string,
  variables: Record<string, string>,
  options?: {
    toName?: string;
    metadata?: Record<string, any>;
  }
): Promise<{ success: boolean; messageId?: string; error?: string }> {
  try {
    const quotaCheck = await canUseSms(workspaceId);
    if (!quotaCheck.allowed) {
      console.log(`SMS blocked for workspace ${workspaceId}: ${quotaCheck.reason}`);
      return { success: false, error: quotaCheck.reason || 'SMS not allowed' };
    }

    const template = await db.query.smsTemplates.findFirst({
      where: and(
        eq(schema.smsTemplates.slug, templateSlug),
        eq(schema.smsTemplates.isActive, true),
        eq(schema.smsTemplates.workspaceId, workspaceId)
      ),
    });

    let result;
    if (!template) {
      const systemTemplate = await db.query.smsTemplates.findFirst({
        where: and(
          eq(schema.smsTemplates.slug, templateSlug),
          eq(schema.smsTemplates.isSystem, true),
          eq(schema.smsTemplates.isActive, true)
        ),
      });
      
      if (!systemTemplate) {
        return { success: false, error: `No active SMS template found with slug: ${templateSlug}` };
      }
      
      const message = replaceVariables(systemTemplate.content, variables);

      result = await sendSms({
        to,
        toName: options?.toName,
        message,
        workspaceId,
        templateId: systemTemplate.id,
        metadata: options?.metadata,
      });
    } else {
      const message = replaceVariables(template.content, variables);

      result = await sendSms({
        to,
        toName: options?.toName,
        message,
        workspaceId,
        templateId: template.id,
        metadata: options?.metadata,
      });
    }

    if (result.success) {
      await incrementSmsUsage(workspaceId);
    }

    return result;
  } catch (error: any) {
    console.error('Send SMS by slug error:', error);
    return { success: false, error: error.message };
  }
}

export async function getSmsSettings(workspaceId: string): Promise<schema.SmsSettings | null> {
  const settings = await db.query.smsSettings.findFirst({
    where: eq(schema.smsSettings.workspaceId, workspaceId),
  });
  return settings || null;
}

export async function updateSmsSettings(workspaceId: string, data: Partial<schema.InsertSmsSettings>): Promise<schema.SmsSettings> {
  const existing = await getSmsSettings(workspaceId);
  
  if (existing) {
    const [updated] = await db.update(schema.smsSettings)
      .set({ ...data, updatedAt: new Date() })
      .where(eq(schema.smsSettings.workspaceId, workspaceId))
      .returning();
    return updated;
  }
  
  const [created] = await db.insert(schema.smsSettings)
    .values({ workspaceId, ...data })
    .returning();
  return created;
}

export async function initializeSystemSmsTemplates(): Promise<void> {
  const existingTemplates = await db.query.smsTemplates.findMany({
    where: eq(schema.smsTemplates.isSystem, true),
  });

  const systemTemplates = [
    {
      name: 'Proposta Enviada',
      slug: 'proposta_enviada',
      triggerType: 'PROPOSAL_SENT' as const,
      content: 'Olá {{nome}}! Sua proposta {{proposta}} foi enviada. Acesse: {{link}}',
    },
    {
      name: 'Proposta Aceita',
      slug: 'proposta_aceita',
      triggerType: 'PROPOSAL_ACCEPTED' as const,
      content: 'Ótima notícia! A proposta {{proposta}} foi aceita por {{cliente}}. 🎉',
    },
    {
      name: 'Proposta Recusada',
      slug: 'proposta_recusada',
      triggerType: 'PROPOSAL_REJECTED' as const,
      content: 'A proposta {{proposta}} foi recusada por {{cliente}}.',
    },
    {
      name: 'Contrato Enviado',
      slug: 'contrato_enviado',
      triggerType: 'CONTRACT_SENT' as const,
      content: 'Olá {{nome}}! Seu contrato está pronto para assinatura. Acesse: {{link}}',
    },
    {
      name: 'Contrato Assinado',
      slug: 'contrato_assinado',
      triggerType: 'CONTRACT_SIGNED' as const,
      content: 'O contrato {{contrato}} foi assinado por {{cliente}}. ✅',
    },
    {
      name: 'Fatura Enviada',
      slug: 'fatura_enviada',
      triggerType: 'INVOICE_SENT' as const,
      content: 'Olá {{nome}}! Fatura #{{numero}} no valor de R$ {{valor}}. Vencimento: {{vencimento}}. Acesse: {{link}}',
    },
    {
      name: 'Fatura Paga',
      slug: 'fatura_paga',
      triggerType: 'INVOICE_PAID' as const,
      content: 'Recebemos seu pagamento da fatura #{{numero}}. Obrigado! 💚',
    },
    {
      name: 'Fatura Vencida',
      slug: 'fatura_vencida',
      triggerType: 'INVOICE_OVERDUE' as const,
      content: 'Olá {{nome}}! A fatura #{{numero}} (R$ {{valor}}) está vencida. Por favor, regularize: {{link}}',
    },
    {
      name: 'Lembrete de Fatura',
      slug: 'lembrete_fatura',
      triggerType: 'INVOICE_REMINDER' as const,
      content: 'Olá {{nome}}! Lembrete: fatura #{{numero}} vence em {{dias}} dia(s). Valor: R$ {{valor}}',
    },
    {
      name: 'Agendamento Confirmado',
      slug: 'agendamento_confirmado',
      triggerType: 'BOOKING_CONFIRMED' as const,
      content: 'Seu agendamento foi confirmado para {{data}} às {{hora}}. Local: {{local}}',
    },
    {
      name: 'Lembrete de Agendamento',
      slug: 'lembrete_agendamento',
      triggerType: 'BOOKING_REMINDER' as const,
      content: 'Lembrete: Você tem um compromisso amanhã ({{data}}) às {{hora}}.',
    },
  ];

  for (const template of systemTemplates) {
    const exists = existingTemplates.find(t => t.triggerType === template.triggerType);
    if (!exists) {
      await db.insert(schema.smsTemplates).values({
        ...template,
        workspaceId: null,
        isSystem: true,
        isActive: true,
      });
      console.log(`Created system SMS template: ${template.name}`);
    }
  }
}

export async function canUseSms(workspaceId: string): Promise<{ allowed: boolean; reason?: string; remaining?: number }> {
  const subscription = await db.query.workspaceSubscriptions.findFirst({
    where: eq(schema.workspaceSubscriptions.workspaceId, workspaceId),
  });
  
  if (!subscription) {
    return { allowed: false, reason: 'No subscription found' };
  }
  
  const now = new Date();
  
  // Check and reset quota if needed
  if (subscription.quotaResetAt && new Date(subscription.quotaResetAt) <= now) {
    await resetQuota(workspaceId);
  }
  
  // Trial always allows all features (limited quota for trial)
  const isTrial = subscription.status === 'TRIAL' && 
    subscription.trialEndsAt && 
    new Date(subscription.trialEndsAt) > now;
  
  if (isTrial) {
    const trialLimit = 50; // Trial gets 50 SMS
    const remaining = trialLimit - subscription.smsUsed;
    if (remaining <= 0) {
      return { allowed: false, reason: 'Trial SMS quota exceeded', remaining: 0 };
    }
    return { allowed: true, remaining };
  }
  
  if (!subscription.canUseSms) {
    return { allowed: false, reason: 'SMS not included in plan' };
  }
  
  // Check quota (0 = unlimited)
  if (subscription.smsQuota > 0) {
    const remaining = subscription.smsQuota - subscription.smsUsed;
    if (remaining <= 0) {
      return { allowed: false, reason: 'SMS quota exceeded', remaining: 0 };
    }
    return { allowed: true, remaining };
  }
  
  return { allowed: true }; // Unlimited
}

export async function incrementSmsUsage(workspaceId: string): Promise<void> {
  await db.update(schema.workspaceSubscriptions)
    .set({ 
      smsUsed: (await db.query.workspaceSubscriptions.findFirst({
        where: eq(schema.workspaceSubscriptions.workspaceId, workspaceId),
      }))?.smsUsed! + 1 || 1 
    })
    .where(eq(schema.workspaceSubscriptions.workspaceId, workspaceId));
}

export async function getSmsUsage(workspaceId: string): Promise<{ 
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
  
  const settings = await db.query.smsSettings.findFirst({
    where: eq(schema.smsSettings.workspaceId, workspaceId),
  });
  
  const hasCustomCredentials = !!(settings?.twilioAccountSid && settings?.twilioAuthToken && settings?.twilioPhoneNumber);
  
  if (!subscription) {
    return { used: 0, quota: 0, remaining: 0, percentUsed: 0, resetsAt: '', hasCustomCredentials };
  }
  
  const isTrial = subscription.status === 'TRIAL' && 
    subscription.trialEndsAt && 
    new Date(subscription.trialEndsAt) > new Date();
  
  const quota = isTrial ? 50 : subscription.smsQuota;
  const remaining = quota === 0 ? -1 : Math.max(0, quota - subscription.smsUsed);
  const percentUsed = quota === 0 ? 0 : Math.round((subscription.smsUsed / quota) * 100);
  const resetsAt = subscription.quotaResetAt?.toISOString() || '';
  
  return { used: subscription.smsUsed, quota, remaining, percentUsed, resetsAt, hasCustomCredentials };
}

async function resetQuota(workspaceId: string): Promise<void> {
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
