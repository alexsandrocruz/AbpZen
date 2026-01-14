import { db } from './storage';
import { invoices, clients, workspaces, smsSettings, smsLogs, smsTemplates, whatsappSettings, whatsappLogs, whatsappTemplates } from '@shared/schema';
import { eq, and, sql, ne } from 'drizzle-orm';
import { sendTemplateSms } from './sms-service';
import { sendTemplateWhatsapp } from './whatsapp-service';

const CHECK_INTERVAL = 1000 * 60 * 60 * 24; // Run once per day

// Get calendar date as YYYY-MM-DD using local timezone (consistent with user intent)
function getLocalDateString(date: Date): string {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
}

function getDaysDifference(now: Date, dueDate: Date): number {
  const nowStr = getLocalDateString(now);
  const dueStr = getLocalDateString(dueDate);
  const nowMs = new Date(nowStr + 'T12:00:00Z').getTime();
  const dueMs = new Date(dueStr + 'T12:00:00Z').getTime();
  return Math.round((nowMs - dueMs) / (1000 * 60 * 60 * 24));
}

async function getTemplateForTrigger(triggerType: string, workspaceId: string): Promise<string | null> {
  const workspaceTemplate = await db
    .select()
    .from(smsTemplates)
    .where(
      and(
        eq(smsTemplates.triggerType, triggerType as any),
        eq(smsTemplates.workspaceId, workspaceId),
        eq(smsTemplates.isActive, true)
      )
    )
    .limit(1);

  if (workspaceTemplate.length > 0) {
    return workspaceTemplate[0].id;
  }

  const systemTemplate = await db
    .select()
    .from(smsTemplates)
    .where(
      and(
        eq(smsTemplates.triggerType, triggerType as any),
        eq(smsTemplates.isSystem, true),
        eq(smsTemplates.isActive, true)
      )
    )
    .limit(1);

  return systemTemplate.length > 0 ? systemTemplate[0].id : null;
}

async function hasSentReminderToday(invoiceId: string, workspaceId: string, triggerType: string): Promise<boolean> {
  const now = new Date();
  const todayStr = getLocalDateString(now);
  const todayStart = new Date(todayStr + 'T00:00:00');
  const todayEnd = new Date(todayStr + 'T23:59:59.999');

  const templateId = await getTemplateForTrigger(triggerType, workspaceId);
  if (!templateId) return false;

  const existingLogs = await db
    .select()
    .from(smsLogs)
    .where(
      and(
        eq(smsLogs.workspaceId, workspaceId),
        eq(smsLogs.templateId, templateId),
        sql`${smsLogs.sentAt} >= ${todayStart}`,
        sql`${smsLogs.sentAt} <= ${todayEnd}`,
        sql`${smsLogs.metadata}::jsonb->>'invoiceId' = ${invoiceId}`
      )
    )
    .limit(1);

  return existingLogs.length > 0;
}

async function checkOverdueInvoices() {
  try {
    const now = new Date();
    
    // Get all workspaces with SMS or WhatsApp reminders enabled
    const allSmsSettings = await db.select().from(smsSettings).where(
      and(
        eq(smsSettings.smsEnabled, true),
        eq(smsSettings.autoInvoiceReminder, true)
      )
    );
    
    const allWhatsappSettings = await db.select().from(whatsappSettings).where(
      and(
        eq(whatsappSettings.whatsappEnabled, true),
        eq(whatsappSettings.autoInvoiceReminder, true)
      )
    );
    
    // Combine workspace IDs that have at least one channel enabled
    const workspaceIdsSet = new Set([
      ...allSmsSettings.map(s => s.workspaceId),
      ...allWhatsappSettings.map(s => s.workspaceId)
    ]);
    const workspaceIds = Array.from(workspaceIdsSet);
    
    for (const workspaceId of workspaceIds) {
      const smsConfig = allSmsSettings.find(s => s.workspaceId === workspaceId);
      const waConfig = allWhatsappSettings.find(s => s.workspaceId === workspaceId);
      
      // Use the first available config for timing (WhatsApp only has reminderDaysBefore)
      const reminderDaysBefore = smsConfig?.reminderDaysBefore || waConfig?.reminderDaysBefore || 3;
      const reminderDaysAfter = smsConfig?.reminderDaysAfter || 1;

      const pendingInvoicesResult = await db
        .select({
          invoice: invoices,
          client: clients,
          workspace: workspaces,
        })
        .from(invoices)
        .innerJoin(clients, eq(invoices.clientId, clients.id))
        .innerJoin(workspaces, eq(invoices.workspaceId, workspaces.id))
        .where(
          and(
            eq(invoices.workspaceId, workspaceId),
            ne(invoices.status, 'PAID'),
            ne(invoices.status, 'CANCELLED'),
            sql`${clients.phone} IS NOT NULL AND ${clients.phone} != ''`
          )
        );

      for (const { invoice, client, workspace } of pendingInvoicesResult) {
        if (!client.phone) continue;

        const daysDiff = getDaysDifference(now, new Date(invoice.dueDate));
        const baseUrl = process.env.REPLIT_DEV_DOMAIN 
          ? `https://${process.env.REPLIT_DEV_DOMAIN}`
          : 'https://dominus.zensuite.com.br';

        if (daysDiff >= reminderDaysAfter) {
          if (daysDiff === reminderDaysAfter || daysDiff % 7 === 0) {
            const overdueVars = {
              nome: client.name,
              fatura: invoice.number,
              vencimento: new Date(invoice.dueDate).toLocaleDateString('pt-BR'),
              link: `${baseUrl}/${workspace.slug}/invoices`,
            };
            const options = { 
              workspaceId: workspaceId, 
              toName: client.name,
              metadata: { invoiceId: invoice.id }
            };
            
            // Send SMS if enabled
            if (smsConfig) {
              const alreadySent = await hasSentReminderToday(invoice.id, workspaceId, 'INVOICE_OVERDUE');
              if (!alreadySent) {
                await sendTemplateSms('INVOICE_OVERDUE', client.phone, overdueVars, options);
                console.log(`Sent overdue SMS for invoice ${invoice.number} (${daysDiff} days overdue)`);
              }
            }
            
            // Send WhatsApp if enabled
            if (waConfig) {
              await sendTemplateWhatsapp('INVOICE_OVERDUE', client.phone, overdueVars, options);
              console.log(`Sent overdue WhatsApp for invoice ${invoice.number} (${daysDiff} days overdue)`);
            }
          }
        } else if (daysDiff === -reminderDaysBefore) {
          const reminderVars = {
            nome: client.name,
            fatura: invoice.number,
            vencimento: new Date(invoice.dueDate).toLocaleDateString('pt-BR'),
            link: `${baseUrl}/${workspace.slug}/invoices`,
          };
          const options = { 
            workspaceId: workspaceId, 
            toName: client.name,
            metadata: { invoiceId: invoice.id }
          };
          
          // Send SMS if enabled
          if (smsConfig) {
            const alreadySent = await hasSentReminderToday(invoice.id, workspaceId, 'INVOICE_REMINDER');
            if (!alreadySent) {
              await sendTemplateSms('INVOICE_REMINDER', client.phone, reminderVars, options);
              console.log(`Sent upcoming SMS for invoice ${invoice.number} (${-daysDiff} days until due)`);
            }
          }
          
          // Send WhatsApp if enabled
          if (waConfig) {
            await sendTemplateWhatsapp('INVOICE_REMINDER', client.phone, reminderVars, options);
            console.log(`Sent upcoming WhatsApp for invoice ${invoice.number} (${-daysDiff} days until due)`);
          }
        }
      }
    }
  } catch (error) {
    console.error('Error checking overdue invoices:', error);
  }
}

export function startInvoiceReminderJob() {
  console.log('Starting invoice reminder job...');
  checkOverdueInvoices();
  setInterval(checkOverdueInvoices, CHECK_INTERVAL);
}
