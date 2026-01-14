import sgMail from '@sendgrid/mail';
import nodemailer from 'nodemailer';
import { Resend } from 'resend';
import { db } from './storage';
import * as schema from '@shared/schema';
import { eq, and, or } from 'drizzle-orm';
import crypto from 'crypto';

let connectionSettings: any;

const DEFAULT_FROM_EMAIL = process.env.RESEND_FROM_EMAIL || process.env.SMTP_FROM_EMAIL || process.env.SENDGRID_FROM_EMAIL || 'noreply@zensuite.com.br';

type EmailProvider = 'resend' | 'smtp' | 'sendgrid';

function getEmailProvider(): EmailProvider {
  if (process.env.RESEND_API_KEY) {
    return 'resend';
  }
  if (process.env.SMTP_HOST && process.env.SMTP_USER && process.env.SMTP_PASS) {
    return 'smtp';
  }
  return 'sendgrid';
}

function getResendClient() {
  return new Resend(process.env.RESEND_API_KEY);
}

function getSmtpTransporter() {
  const port = parseInt(process.env.SMTP_PORT || '465');
  return nodemailer.createTransport({
    host: process.env.SMTP_HOST,
    port,
    secure: port === 465,
    auth: {
      user: process.env.SMTP_USER,
      pass: process.env.SMTP_PASS,
    },
  });
}

async function getCredentials(): Promise<{ apiKey: string; email: string }> {
  if (process.env.SENDGRID_API_KEY) {
    return { 
      apiKey: process.env.SENDGRID_API_KEY, 
      email: process.env.SENDGRID_FROM_EMAIL || DEFAULT_FROM_EMAIL 
    };
  }
  
  const hostname = process.env.REPLIT_CONNECTORS_HOSTNAME;
  const xReplitToken = process.env.REPL_IDENTITY
    ? 'repl ' + process.env.REPL_IDENTITY
    : process.env.WEB_REPL_RENEWAL
    ? 'depl ' + process.env.WEB_REPL_RENEWAL
    : null;

  if (!xReplitToken) {
    throw new Error('SENDGRID_API_KEY not configured and Replit connector not available');
  }

  connectionSettings = await fetch(
    'https://' + hostname + '/api/v2/connection?include_secrets=true&connector_names=sendgrid',
    {
      headers: {
        'Accept': 'application/json',
        'X_REPLIT_TOKEN': xReplitToken
      }
    }
  ).then(res => res.json()).then(data => data.items?.[0]);

  if (!connectionSettings || (!connectionSettings.settings.api_key || !connectionSettings.settings.from_email)) {
    throw new Error('SendGrid not connected. Set SENDGRID_API_KEY environment variable.');
  }
  return { apiKey: connectionSettings.settings.api_key, email: connectionSettings.settings.from_email };
}

async function getSendGridClient() {
  const { apiKey, email } = await getCredentials();
  sgMail.setApiKey(apiKey);
  return {
    client: sgMail,
    fromEmail: email
  };
}

interface SendEmailOptions {
  to: string;
  toName?: string;
  subject: string;
  html: string;
  text?: string;
  replyTo?: string;
  attachments?: Array<{
    content: string;
    filename: string;
    type: string;
    disposition?: 'attachment' | 'inline';
  }>;
  workspaceId?: string;
  templateId?: string;
  metadata?: Record<string, any>;
  trackOpens?: boolean;
  trackClicks?: boolean;
}

export async function sendEmail(options: SendEmailOptions): Promise<{ success: boolean; messageId?: string; error?: string; logId?: string }> {
  const provider = getEmailProvider();
  const fromEmail = DEFAULT_FROM_EMAIL;
  
  const baseUrl = process.env.REPLIT_DEV_DOMAIN 
    ? `https://${process.env.REPLIT_DEV_DOMAIN}`
    : 'https://dominus.zensuite.com.br';
  
  let htmlWithTracking = options.html;
  
  const logEntry = await db.insert(schema.emailLogs).values({
    workspaceId: options.workspaceId || null,
    templateId: options.templateId || null,
    toEmail: options.to,
    toName: options.toName || null,
    fromEmail: fromEmail,
    fromName: 'Dominus',
    replyTo: options.replyTo || null,
    subject: options.subject,
    htmlContent: options.html,
    textContent: options.text || null,
    status: 'PENDING',
    metadata: options.metadata ? JSON.stringify(options.metadata) : null,
  }).returning();

  const logId = logEntry[0]?.id;

  if (options.trackOpens !== false && logId) {
    const trackingPixel = `<img src="${baseUrl}/api/email/track/open/${logId}" width="1" height="1" style="display:none" alt="" />`;
    htmlWithTracking = htmlWithTracking.replace('</body>', `${trackingPixel}</body>`);
  }

  if (options.trackClicks !== false && logId) {
    htmlWithTracking = htmlWithTracking.replace(
      /href="(https?:\/\/[^"]+)"/g,
      (match, url) => `href="${baseUrl}/api/email/track/click/${logId}?url=${encodeURIComponent(url)}"`
    );
  }

  try {
    if (provider === 'resend') {
      const resend = getResendClient();
      const { data, error } = await resend.emails.send({
        from: `Dominus <${fromEmail}>`,
        to: options.to,
        subject: options.subject,
        html: htmlWithTracking,
        text: options.text || options.html.replace(/<[^>]+>/g, ''),
        replyTo: options.replyTo,
      });

      if (error) {
        throw new Error(error.message);
      }

      const messageId = data?.id;

      if (logId) {
        await db.update(schema.emailLogs)
          .set({
            status: 'SENT',
            sendgridMessageId: messageId,
            sentAt: new Date(),
          })
          .where(eq(schema.emailLogs.id, logId));
      }

      console.log(`Email sent via Resend to ${options.to}, messageId: ${messageId}`);
      return { success: true, messageId, logId };
    } else if (provider === 'smtp') {
      const transporter = getSmtpTransporter();
      const info = await transporter.sendMail({
        from: `"Dominus" <${fromEmail}>`,
        to: options.to,
        subject: options.subject,
        html: htmlWithTracking,
        text: options.text || options.html.replace(/<[^>]+>/g, ''),
        replyTo: options.replyTo,
        attachments: options.attachments?.map(att => ({
          filename: att.filename,
          content: Buffer.from(att.content, 'base64'),
          contentType: att.type,
        })),
      });

      const messageId = info.messageId;

      if (logId) {
        await db.update(schema.emailLogs)
          .set({
            status: 'SENT',
            sendgridMessageId: messageId,
            sentAt: new Date(),
          })
          .where(eq(schema.emailLogs.id, logId));
      }

      console.log(`Email sent via SMTP to ${options.to}, messageId: ${messageId}`);
      return { success: true, messageId, logId };
    } else {
      const { client, fromEmail: sgFromEmail } = await getSendGridClient();
      
      const msg: any = {
        to: options.to,
        from: {
          email: sgFromEmail,
          name: 'Dominus'
        },
        subject: options.subject,
        html: htmlWithTracking,
        text: options.text || options.html.replace(/<[^>]+>/g, ''),
      };

      if (options.replyTo) {
        msg.replyTo = options.replyTo;
      }

      if (options.attachments && options.attachments.length > 0) {
        msg.attachments = options.attachments;
      }

      const [response] = await client.send(msg);
      const messageId = response.headers['x-message-id'];

      if (logId) {
        await db.update(schema.emailLogs)
          .set({
            status: 'SENT',
            sendgridMessageId: messageId,
            sentAt: new Date(),
          })
          .where(eq(schema.emailLogs.id, logId));
      }

      return { success: true, messageId, logId };
    }
  } catch (error: any) {
    console.error('Email send error:', error);
    if (logId) {
      await db.update(schema.emailLogs)
        .set({ status: 'FAILED' })
        .where(eq(schema.emailLogs.id, logId));
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

export async function sendTemplateEmail(
  triggerType: schema.EmailTriggerType,
  to: string,
  variables: Record<string, string>,
  options?: {
    workspaceId?: string;
    toName?: string;
    attachments?: SendEmailOptions['attachments'];
    metadata?: Record<string, any>;
  }
): Promise<{ success: boolean; error?: string }> {
  try {
    const template = await db.query.emailTemplates.findFirst({
      where: and(
        eq(schema.emailTemplates.triggerType, triggerType),
        eq(schema.emailTemplates.isActive, true),
        options?.workspaceId 
          ? eq(schema.emailTemplates.workspaceId, options.workspaceId)
          : eq(schema.emailTemplates.isSystem, true)
      ),
    });

    if (!template) {
      const systemTemplate = await db.query.emailTemplates.findFirst({
        where: and(
          eq(schema.emailTemplates.triggerType, triggerType),
          eq(schema.emailTemplates.isSystem, true),
          eq(schema.emailTemplates.isActive, true)
        ),
      });
      
      if (!systemTemplate) {
        return { success: false, error: `No active template found for trigger: ${triggerType}` };
      }
      
      const subject = replaceVariables(systemTemplate.subject, variables);
      const html = replaceVariables(systemTemplate.htmlContent, variables);
      const text = systemTemplate.textContent ? replaceVariables(systemTemplate.textContent, variables) : undefined;

      return await sendEmail({
        to,
        toName: options?.toName,
        subject,
        html,
        text,
        workspaceId: options?.workspaceId,
        templateId: systemTemplate.id,
        attachments: options?.attachments,
        metadata: options?.metadata,
      });
    }

    const subject = replaceVariables(template.subject, variables);
    const html = replaceVariables(template.htmlContent, variables);
    const text = template.textContent ? replaceVariables(template.textContent, variables) : undefined;

    return await sendEmail({
      to,
      toName: options?.toName,
      subject,
      html,
      text,
      workspaceId: options?.workspaceId,
      templateId: template.id,
      attachments: options?.attachments,
      metadata: options?.metadata,
    });
  } catch (error: any) {
    console.error('Template email error:', error);
    return { success: false, error: error.message };
  }
}

export async function sendEmailBySlug(
  workspaceId: string,
  to: string,
  templateId: string,
  variables: Record<string, string>,
  options?: {
    toName?: string;
    attachments?: SendEmailOptions['attachments'];
    metadata?: Record<string, any>;
  }
): Promise<{ success: boolean; error?: string; messageId?: string }> {
  try {
    const template = await db.query.emailTemplates.findFirst({
      where: and(
        eq(schema.emailTemplates.id, templateId),
        eq(schema.emailTemplates.isActive, true),
        or(
          eq(schema.emailTemplates.workspaceId, workspaceId),
          eq(schema.emailTemplates.isSystem, true)
        )
      ),
    });

    if (!template) {
      return { success: false, error: `Template de email não encontrado: ${templateId}` };
    }

    const subject = replaceVariables(template.subject, variables);
    const html = replaceVariables(template.htmlContent, variables);
    const text = template.textContent ? replaceVariables(template.textContent, variables) : undefined;

    return await sendEmail({
      to,
      toName: options?.toName,
      subject,
      html,
      text,
      workspaceId,
      templateId: template.id,
      attachments: options?.attachments,
      metadata: options?.metadata,
    });
  } catch (error: any) {
    console.error('Email by slug error:', error);
    return { success: false, error: error.message };
  }
}

export async function createPasswordResetToken(userId: string): Promise<string> {
  const token = crypto.randomBytes(32).toString('hex');
  const expiresAt = new Date(Date.now() + 60 * 60 * 1000);

  await db.insert(schema.passwordResetTokens).values({
    userId,
    token,
    expiresAt,
  });

  return token;
}

export async function validatePasswordResetToken(token: string): Promise<{ valid: boolean; userId?: string; error?: string }> {
  const resetToken = await db.query.passwordResetTokens.findFirst({
    where: eq(schema.passwordResetTokens.token, token),
  });

  if (!resetToken) {
    return { valid: false, error: 'Token inválido' };
  }

  if (resetToken.usedAt) {
    return { valid: false, error: 'Token já utilizado' };
  }

  if (new Date() > resetToken.expiresAt) {
    return { valid: false, error: 'Token expirado' };
  }

  return { valid: true, userId: resetToken.userId };
}

export async function markTokenAsUsed(token: string): Promise<void> {
  await db.update(schema.passwordResetTokens)
    .set({ usedAt: new Date() })
    .where(eq(schema.passwordResetTokens.token, token));
}

export async function sendPasswordResetEmail(user: { id: string; email: string; name: string }): Promise<{ success: boolean; error?: string }> {
  const token = await createPasswordResetToken(user.id);
  
  const baseUrl = process.env.REPLIT_DEV_DOMAIN 
    ? `https://${process.env.REPLIT_DEV_DOMAIN}`
    : 'https://dominus.zensuite.com.br';
  
  const resetLink = `${baseUrl}/reset-password/${token}`;

  return await sendTemplateEmail('PASSWORD_RESET', user.email, {
    nome: user.name,
    email: user.email,
    link: resetLink,
    workspace: 'Dominus',
  }, {
    toName: user.name,
    metadata: { userId: user.id, type: 'password_reset' },
  });
}

export async function sendWelcomeEmail(user: { id: string; email: string; name: string }, workspaceName?: string): Promise<{ success: boolean; error?: string }> {
  const baseUrl = process.env.REPLIT_DEV_DOMAIN 
    ? `https://${process.env.REPLIT_DEV_DOMAIN}`
    : 'https://dominus.zensuite.com.br';

  return await sendTemplateEmail('WELCOME', user.email, {
    userName: user.name,
    userEmail: user.email,
    workspaceName: workspaceName || 'Dominus',
    loginLink: `${baseUrl}/login`,
    dashboardLink: `${baseUrl}/dashboard`,
  }, {
    toName: user.name,
    metadata: { userId: user.id, type: 'welcome' },
  });
}

export async function initializeSystemEmailTemplates(): Promise<void> {
  const existingTemplates = await db.query.emailTemplates.findMany({
    where: eq(schema.emailTemplates.isSystem, true),
  });

  const systemTemplates = [
    {
      name: 'Recuperação de Senha',
      subject: 'Redefinir sua senha - Dominus',
      triggerType: 'PASSWORD_RESET' as const,
      htmlContent: `<!DOCTYPE html>
<html>
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Redefinir Senha</title>
</head>
<body style="margin:0;padding:0;font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;background-color:#f4f4f5;">
  <table width="100%" cellpadding="0" cellspacing="0" style="padding:40px 20px;">
    <tr>
      <td align="center">
        <table width="100%" style="max-width:600px;background:#ffffff;border-radius:12px;overflow:hidden;box-shadow:0 4px 6px rgba(0,0,0,0.1);">
          <tr>
            <td style="background:linear-gradient(135deg,#6366f1,#8b5cf6);padding:40px;text-align:center;">
              <h1 style="color:#ffffff;margin:0;font-size:28px;">Dominus</h1>
            </td>
          </tr>
          <tr>
            <td style="padding:40px;">
              <h2 style="color:#18181b;margin:0 0 16px;font-size:22px;">Olá, {{userName}}!</h2>
              <p style="color:#52525b;line-height:1.6;margin:0 0 24px;">
                Recebemos uma solicitação para redefinir a senha da sua conta. Clique no botão abaixo para criar uma nova senha:
              </p>
              <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                  <td align="center" style="padding:16px 0;">
                    <a href="{{resetLink}}" style="display:inline-block;padding:14px 32px;background:linear-gradient(135deg,#6366f1,#8b5cf6);color:#ffffff;text-decoration:none;border-radius:8px;font-weight:600;font-size:16px;">
                      Redefinir Senha
                    </a>
                  </td>
                </tr>
              </table>
              <p style="color:#71717a;font-size:14px;line-height:1.5;margin:24px 0 0;">
                Este link expira em {{expirationTime}}. Se você não solicitou a redefinição de senha, ignore este email.
              </p>
              <hr style="border:none;border-top:1px solid #e4e4e7;margin:24px 0;">
              <p style="color:#a1a1aa;font-size:12px;margin:0;">
                Se o botão não funcionar, copie e cole este link no seu navegador:<br>
                <a href="{{resetLink}}" style="color:#6366f1;word-break:break-all;">{{resetLink}}</a>
              </p>
            </td>
          </tr>
          <tr>
            <td style="background:#fafafa;padding:24px;text-align:center;">
              <p style="color:#a1a1aa;font-size:12px;margin:0;">
                © 2025 Dominus. Todos os direitos reservados.
              </p>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>`,
      textContent: `Olá, {{userName}}!

Recebemos uma solicitação para redefinir a senha da sua conta.

Clique no link abaixo para criar uma nova senha:
{{resetLink}}

Este link expira em {{expirationTime}}.

Se você não solicitou a redefinição de senha, ignore este email.

© 2025 Dominus`,
    },
    {
      name: 'Boas-vindas',
      subject: 'Bem-vindo ao Dominus! 🎉',
      triggerType: 'WELCOME' as const,
      htmlContent: `<!DOCTYPE html>
<html>
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Bem-vindo ao Dominus</title>
</head>
<body style="margin:0;padding:0;font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;background-color:#f4f4f5;">
  <table width="100%" cellpadding="0" cellspacing="0" style="padding:40px 20px;">
    <tr>
      <td align="center">
        <table width="100%" style="max-width:600px;background:#ffffff;border-radius:12px;overflow:hidden;box-shadow:0 4px 6px rgba(0,0,0,0.1);">
          <tr>
            <td style="background:linear-gradient(135deg,#6366f1,#8b5cf6);padding:40px;text-align:center;">
              <h1 style="color:#ffffff;margin:0;font-size:28px;">Dominus</h1>
              <p style="color:rgba(255,255,255,0.9);margin:8px 0 0;font-size:16px;">Gestão Inteligente para seu Negócio</p>
            </td>
          </tr>
          <tr>
            <td style="padding:40px;">
              <h2 style="color:#18181b;margin:0 0 16px;font-size:24px;">Bem-vindo, {{userName}}! 🎉</h2>
              <p style="color:#52525b;line-height:1.6;margin:0 0 24px;">
                Sua conta foi criada com sucesso! Você agora tem acesso a todas as ferramentas do Dominus para gerenciar clientes, projetos, propostas e muito mais.
              </p>
              <table width="100%" cellpadding="0" cellspacing="0" style="margin:24px 0;">
                <tr>
                  <td style="padding:16px;background:#f4f4f5;border-radius:8px;">
                    <p style="color:#18181b;font-weight:600;margin:0 0 8px;">Próximos passos:</p>
                    <ul style="color:#52525b;margin:0;padding-left:20px;line-height:1.8;">
                      <li>Complete seu perfil</li>
                      <li>Adicione seus primeiros clientes</li>
                      <li>Crie sua primeira proposta</li>
                      <li>Explore as automações disponíveis</li>
                    </ul>
                  </td>
                </tr>
              </table>
              <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                  <td align="center" style="padding:16px 0;">
                    <a href="{{dashboardLink}}" style="display:inline-block;padding:14px 32px;background:linear-gradient(135deg,#6366f1,#8b5cf6);color:#ffffff;text-decoration:none;border-radius:8px;font-weight:600;font-size:16px;">
                      Acessar Painel
                    </a>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <tr>
            <td style="background:#fafafa;padding:24px;text-align:center;">
              <p style="color:#71717a;font-size:14px;margin:0 0 8px;">
                Precisa de ajuda? Entre em contato conosco!
              </p>
              <p style="color:#a1a1aa;font-size:12px;margin:0;">
                © 2025 Dominus. Todos os direitos reservados.
              </p>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>`,
      textContent: `Bem-vindo, {{userName}}! 🎉

Sua conta foi criada com sucesso! Você agora tem acesso a todas as ferramentas do Dominus.

Próximos passos:
- Complete seu perfil
- Adicione seus primeiros clientes
- Crie sua primeira proposta
- Explore as automações disponíveis

Acesse seu painel: {{dashboardLink}}

© 2025 Dominus`,
    },
  ];

  for (const template of systemTemplates) {
    const exists = existingTemplates.find(t => t.triggerType === template.triggerType);
    if (!exists) {
      await db.insert(schema.emailTemplates).values({
        ...template,
        workspaceId: null,
        isSystem: true,
        isActive: true,
      });
      console.log(`Created system email template: ${template.name}`);
    }
  }
}
