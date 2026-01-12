import type { Express, Request, Response, NextFunction } from "express";
import { createServer, type Server } from "http";
import { storage } from "./storage";
import passport from "passport";
import { registerObjectStorageRoutes, ObjectStorageService } from "./replit_integrations/object_storage";
import { scrypt, randomBytes } from "crypto";
import { promisify } from "util";
import { z } from "zod";
import { openai } from "./openai";
import { initWebSocket, getWebSocketService } from "./websocket";
import { 
  insertUserSchema, 
  insertClientSchema, 
  insertClientContactSchema,
  insertProjectSchema, 
  insertTaskSchema, 
  insertTaskCommentSchema,
  insertInvoiceSchema, 
  insertInvoiceItemSchema, 
  insertProposalSchema, 
  insertProposalItemSchema,
  insertTimeEntrySchema,
  insertFinancialCategorySchema,
  insertTransactionSchema,
  insertTransactionAttachmentSchema,
  insertBudgetSchema,
  insertProductSchema,
  insertContractTemplateSchema,
  insertContractSchema,
  insertLeadWorkflowSchema,
  insertLeadWorkflowStageSchema,
  insertLeadFormSchema,
  insertLeadFormFieldSchema,
  insertLeadLandingPageSchema,
  insertLeadSchema,
  insertLeadFormSubmissionSchema,
  insertLeadStageHistorySchema,
  insertConversationSchema,
  insertMessageSchema,
  insertCommentSchema,
  insertFileSchema,
  insertClientDocumentSchema,
  insertWorkflowSchema,
  insertAiChatSessionSchema,
  insertAiChatMessageSchema,
  insertProposalTemplateSchema,
  insertProposalBlockInstanceSchema,
} from "@shared/schema";
import { workflowEngine } from "./workflow-engine";
import { aiService } from "./ai-service";
import { 
  sendPasswordResetEmail, 
  sendWelcomeEmail, 
  validatePasswordResetToken, 
  markTokenAsUsed,
  initializeSystemEmailTemplates 
} from "./email-service";
import { sendTestSms, getSmsSettings, updateSmsSettings, initializeSystemSmsTemplates, sendTemplateSms, canUseSms, getSmsUsage } from "./sms-service";
import { 
  sendTestWhatsapp, 
  getWhatsappSettings, 
  updateWhatsappSettings, 
  initializeSystemWhatsappTemplates,
  sendTemplateWhatsapp,
  sendWhatsapp,
  canUseWhatsapp,
  canUseChatbot,
  findClientByPhone,
  findWorkspaceByLastMessage,
  logIncomingWhatsapp,
  getWhatsappUsage
} from "./whatsapp-service";
import { handleIncomingWhatsapp, sendChatbotReply } from "./chatbot-service";
import { extractTasksFromText } from "./task-extractor";
import { extractProposalEntities } from "./proposal-copilot";
import * as schema from "@shared/schema";

const scryptAsync = promisify(scrypt);

const crypto = {
  hash: async (password: string) => {
    const salt = randomBytes(16).toString("hex");
    const buf = (await scryptAsync(password, salt, 64)) as Buffer;
    return `${buf.toString("hex")}.${salt}`;
  },
  compare: async (supplied: string, stored: string) => {
    const [hashedPassword, salt] = stored.split(".");
    const buf = (await scryptAsync(supplied, salt, 64)) as Buffer;
    return buf.toString("hex") === hashedPassword;
  },
};

// Middleware: Require authentication
function requireAuth(req: Request, res: Response, next: NextFunction) {
  if (!req.isAuthenticated()) {
    return res.status(401).json({ message: "Unauthorized" });
  }
  next();
}

// Middleware: Require workspace membership
async function requireWorkspace(req: Request, res: Response, next: NextFunction) {
  if (!req.user) {
    return res.status(401).json({ message: "Unauthorized" });
  }

  const { workspaceId } = req.params;
  if (!workspaceId) {
    return res.status(400).json({ message: "Workspace ID required" });
  }

  const membership = await storage.getUserWorkspaceMembership(req.user.id, workspaceId);
  if (!membership) {
    return res.status(403).json({ message: "No access to this workspace" });
  }

  next();
}

async function validateSiteAccess(req: Request, res: Response, next: NextFunction) {
  if (!req.user) {
    return res.status(401).json({ message: "Unauthorized" });
  }

  const { siteId } = req.params;
  if (!siteId) {
    return res.status(400).json({ message: "Site ID required" });
  }

  const site = await storage.getSiteProjectByIdForValidation(siteId);
  if (!site) {
    return res.status(404).json({ message: "Site not found" });
  }

  const membership = await storage.getUserWorkspaceMembership(req.user.id, site.workspaceId);
  if (!membership) {
    return res.status(403).json({ message: "No access to this site" });
  }

  (req as any).siteWorkspaceId = site.workspaceId;
  next();
}

export async function registerRoutes(
  httpServer: Server,
  app: Express
): Promise<Server> {
  
  // Register object storage routes for file uploads
  registerObjectStorageRoutes(app);
  
  // ============ AUTH ROUTES ============
  
  // Register (Create user + workspace)
  app.post("/api/auth/register", async (req: Request, res: Response) => {
    try {
      const { email, password, name, workspaceName, workspaceSlug } = req.body;

      if (!email || !password || !name || !workspaceName || !workspaceSlug) {
        return res.status(400).json({ message: "Missing required fields" });
      }

      // Check if user exists
      const existingUser = await storage.getUserByEmail(email);
      if (existingUser) {
        return res.status(400).json({ message: "User already exists" });
      }

      // Check if workspace slug is taken
      const existingWorkspace = await storage.getWorkspaceBySlug(workspaceSlug);
      if (existingWorkspace) {
        return res.status(400).json({ message: "Workspace slug already taken" });
      }

      // Hash password
      const hashedPassword = await crypto.hash(password);

      // Create user
      const user = await storage.createUser({
        email,
        password: hashedPassword,
        name,
        avatar: `https://api.dicebear.com/7.x/initials/svg?seed=${encodeURIComponent(name)}`,
      });

      // Create workspace
      const workspace = await storage.createWorkspace({
        name: workspaceName,
        slug: workspaceSlug,
        ownerId: user.id,
      });

      // Add user as owner
      await storage.addWorkspaceMember({
        workspaceId: workspace.id,
        userId: user.id,
        role: 'OWNER',
      });

      // Auto login
      req.login({ id: user.id, email: user.email, name: user.name, avatar: user.avatar || undefined, globalRole: user.globalRole }, (err) => {
        if (err) {
          return res.status(500).json({ message: "Login failed after registration" });
        }
        res.json({ user: { id: user.id, email: user.email, name: user.name, avatar: user.avatar, globalRole: user.globalRole }, workspace });
      });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Register for invite (Create user and accept invite)
  app.post("/api/auth/register-for-invite", async (req: Request, res: Response) => {
    try {
      const { email, password, name, inviteToken } = req.body;

      if (!email || !password || !name || !inviteToken) {
        return res.status(400).json({ message: "Campos obrigatórios não preenchidos" });
      }

      // Check invite validity
      const invite = await storage.getInviteByToken(inviteToken);
      if (!invite) {
        return res.status(404).json({ message: "Convite não encontrado" });
      }
      if (invite.status !== 'PENDING') {
        return res.status(400).json({ message: "Este convite já foi utilizado" });
      }
      if (new Date(invite.expiresAt) < new Date()) {
        return res.status(400).json({ message: "Este convite expirou" });
      }
      if (invite.email !== email) {
        return res.status(400).json({ message: "E-mail não corresponde ao convite" });
      }

      // Check if email already exists
      const existingUser = await storage.getUserByEmail(email);
      if (existingUser) {
        return res.status(400).json({ message: "Este e-mail já está cadastrado. Faça login para aceitar o convite." });
      }

      // Hash password
      const hashedPassword = await crypto.hash(password);

      // Create user
      const user = await storage.createUser({
        email,
        password: hashedPassword,
        name,
        avatar: `https://api.dicebear.com/7.x/initials/svg?seed=${encodeURIComponent(name)}`,
      });

      // Add user to workspace
      await storage.addWorkspaceMember({
        workspaceId: invite.workspaceId,
        userId: user.id,
        role: 'MEMBER',
        memberRole: invite.memberRole,
      });

      // Mark invite as accepted
      await storage.updateInviteStatus(invite.id, 'ACCEPTED');

      const workspace = await storage.getWorkspaceById(invite.workspaceId);

      // Auto login
      req.login({ id: user.id, email: user.email, name: user.name, avatar: user.avatar || undefined, globalRole: user.globalRole }, (err) => {
        if (err) {
          return res.status(500).json({ message: "Falha no login após registro" });
        }
        res.json({ user: { id: user.id, email: user.email, name: user.name, avatar: user.avatar, globalRole: user.globalRole }, workspace });
      });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Login
  app.post("/api/auth/login", (req: Request, res: Response, next: NextFunction) => {
    passport.authenticate("local", (err: any, user: Express.User | false, info: any) => {
      if (err) {
        return res.status(500).json({ message: err.message });
      }
      if (!user) {
        return res.status(401).json({ message: info?.message || "Invalid credentials" });
      }
      req.login(user, async (loginErr) => {
        if (loginErr) {
          return res.status(500).json({ message: loginErr.message });
        }
        
        // Get user's workspaces
        const workspaces = await storage.getUserWorkspaces(user.id);
        
        res.json({ 
          user: { id: user.id, email: user.email, name: user.name, avatar: user.avatar, globalRole: user.globalRole },
          workspaces 
        });
      });
    })(req, res, next);
  });

  // Logout
  app.post("/api/auth/logout", (req: Request, res: Response) => {
    req.logout((err) => {
      if (err) {
        return res.status(500).json({ message: err.message });
      }
      res.json({ message: "Logged out successfully" });
    });
  });

  // Get current user
  app.get("/api/auth/me", requireAuth, async (req: Request, res: Response) => {
    const workspaces = await storage.getUserWorkspaces(req.user!.id);
    res.json({ user: req.user, workspaces });
  });

  // ============ PASSWORD RESET ROUTES ============

  app.post("/api/auth/forgot-password", async (req: Request, res: Response) => {
    try {
      const { email } = req.body;

      if (!email) {
        return res.status(400).json({ message: "Email é obrigatório" });
      }

      const user = await storage.getUserByEmail(email);
      
      if (user) {
        await sendPasswordResetEmail({ id: user.id, email: user.email, name: user.name });
      }

      res.json({ message: "Se o email estiver cadastrado, você receberá as instruções para redefinir sua senha." });
    } catch (error: any) {
      console.error('Forgot password error:', error);
      res.json({ message: "Se o email estiver cadastrado, você receberá as instruções para redefinir sua senha." });
    }
  });

  app.get("/api/auth/validate-reset-token/:token", async (req: Request, res: Response) => {
    try {
      const { token } = req.params;
      const result = await validatePasswordResetToken(token);
      
      if (!result.valid) {
        return res.status(400).json({ valid: false, message: result.error });
      }

      res.json({ valid: true });
    } catch (error: any) {
      res.status(500).json({ valid: false, message: "Erro ao validar token" });
    }
  });

  app.post("/api/auth/reset-password", async (req: Request, res: Response) => {
    try {
      const { token, password } = req.body;

      if (!token || !password) {
        return res.status(400).json({ message: "Token e senha são obrigatórios" });
      }

      if (password.length < 6) {
        return res.status(400).json({ message: "A senha deve ter pelo menos 6 caracteres" });
      }

      const result = await validatePasswordResetToken(token);
      
      if (!result.valid || !result.userId) {
        return res.status(400).json({ message: result.error || "Token inválido" });
      }

      const hashedPassword = await crypto.hash(password);
      await storage.updateUser(result.userId, { password: hashedPassword });
      await markTokenAsUsed(token);

      res.json({ message: "Senha redefinida com sucesso!" });
    } catch (error: any) {
      console.error('Reset password error:', error);
      res.status(500).json({ message: "Erro ao redefinir senha" });
    }
  });

  // ============ EMAIL TRACKING ROUTES ============

  app.get("/api/email/track/open/:logId", async (req: Request, res: Response) => {
    try {
      const { logId } = req.params;
      
      const log = await storage.getEmailLogById(logId);
      if (log && !log.openedAt) {
        await storage.updateEmailLog(logId, { 
          status: 'OPENED', 
          openedAt: new Date() 
        });
      }

      const pixel = Buffer.from('R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7', 'base64');
      res.set('Content-Type', 'image/gif');
      res.set('Cache-Control', 'no-cache, no-store, must-revalidate');
      res.send(pixel);
    } catch (error) {
      const pixel = Buffer.from('R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7', 'base64');
      res.set('Content-Type', 'image/gif');
      res.send(pixel);
    }
  });

  app.get("/api/email/track/click/:logId", async (req: Request, res: Response) => {
    try {
      const { logId } = req.params;
      const { url } = req.query;
      
      if (!url || typeof url !== 'string') {
        return res.status(400).json({ message: "URL is required" });
      }

      const log = await storage.getEmailLogById(logId);
      if (log && !log.clickedAt) {
        await storage.updateEmailLog(logId, { 
          status: 'CLICKED', 
          clickedAt: new Date() 
        });
      }

      res.redirect(url);
    } catch (error) {
      const { url } = req.query;
      if (url && typeof url === 'string') {
        res.redirect(url);
      } else {
        res.status(500).json({ message: "Redirect failed" });
      }
    }
  });

  // ============ SMS ROUTES ============

  app.post("/api/sms/test", async (req: Request, res: Response) => {
    try {
      const { phone } = req.body;
      if (!phone) {
        return res.status(400).json({ message: "Phone number is required" });
      }
      const result = await sendTestSms(phone);
      if (result.success) {
        res.json({ message: "SMS sent successfully", messageId: result.messageId });
      } else {
        res.status(500).json({ message: result.error });
      }
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // SMS Settings
  app.get("/api/workspaces/:workspaceId/sms-settings", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const settings = await getSmsSettings(req.params.workspaceId);
      res.json(settings || { 
        smsEnabled: false, 
        autoInvoiceReminder: false,
        reminderDaysBefore: 3,
        reminderDaysAfter: 1 
      });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/sms-settings", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const settings = await updateSmsSettings(req.params.workspaceId, req.body);
      res.json(settings);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // SMS Usage/Quota
  app.get("/api/workspaces/:workspaceId/sms-usage", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const usage = await getSmsUsage(req.params.workspaceId);
      res.json(usage);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // SMS Templates
  app.get("/api/workspaces/:workspaceId/sms-templates", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const templates = await storage.getSmsTemplatesByWorkspace(req.params.workspaceId);
      res.json(templates);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/sms-templates/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.getSmsTemplateById(req.params.id);
      if (!template) {
        return res.status(404).json({ message: "Template not found" });
      }
      res.json(template);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/sms-templates", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.createSmsTemplate({
        ...req.body,
        workspaceId: req.params.workspaceId,
        isSystem: false,
      });
      res.status(201).json(template);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/sms-templates/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.updateSmsTemplate(req.params.id, req.params.workspaceId, req.body);
      if (!template) {
        return res.status(404).json({ message: "Template not found" });
      }
      res.json(template);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/sms-templates/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteSmsTemplate(req.params.id, req.params.workspaceId);
      res.json({ message: "Template deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // SMS Logs
  app.get("/api/workspaces/:workspaceId/sms-logs", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const logs = await storage.getSmsLogsByWorkspace(req.params.workspaceId);
      res.json(logs);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ WHATSAPP ROUTES ============

  app.post("/api/whatsapp/test", async (req: Request, res: Response) => {
    try {
      const { phone } = req.body;
      if (!phone) {
        return res.status(400).json({ message: "Phone number is required" });
      }
      const result = await sendTestWhatsapp(phone);
      if (result.success) {
        res.json({ message: "WhatsApp message sent successfully", messageId: result.messageId });
      } else {
        res.status(500).json({ message: result.error });
      }
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // WhatsApp Settings
  app.get("/api/workspaces/:workspaceId/whatsapp-settings", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const settings = await getWhatsappSettings(req.params.workspaceId);
      res.json(settings || { 
        whatsappEnabled: false,
        chatbotEnabled: false,
        autoInvoiceReminder: false,
        autoBookingReminder: false,
        reminderDaysBefore: 1,
        reminderHoursBefore: 24
      });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/whatsapp-settings", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const settings = await updateWhatsappSettings(req.params.workspaceId, req.body);
      res.json(settings);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // WhatsApp Usage/Quota
  app.get("/api/workspaces/:workspaceId/whatsapp-usage", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const usage = await getWhatsappUsage(req.params.workspaceId);
      res.json(usage);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // WhatsApp Templates
  app.get("/api/workspaces/:workspaceId/whatsapp-templates", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const templates = await storage.getWhatsappTemplatesByWorkspace(req.params.workspaceId);
      res.json(templates);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/whatsapp-templates/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.getWhatsappTemplateById(req.params.id);
      if (!template) {
        return res.status(404).json({ message: "Template not found" });
      }
      res.json(template);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/whatsapp-templates", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.createWhatsappTemplate({
        ...req.body,
        workspaceId: req.params.workspaceId,
        isSystem: false,
      });
      res.status(201).json(template);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/whatsapp-templates/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.updateWhatsappTemplate(req.params.id, req.params.workspaceId, req.body);
      if (!template) {
        return res.status(404).json({ message: "Template not found" });
      }
      res.json(template);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/whatsapp-templates/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteWhatsappTemplate(req.params.id, req.params.workspaceId);
      res.json({ message: "Template deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // WhatsApp Logs
  app.get("/api/workspaces/:workspaceId/whatsapp-logs", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const logs = await storage.getWhatsappLogsByWorkspace(req.params.workspaceId);
      res.json(logs);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // WhatsApp Webhook for incoming messages (Twilio)
  app.post("/api/webhooks/whatsapp", async (req: Request, res: Response) => {
    try {
      const { From, Body, MessageSid } = req.body;
      
      if (!From || !Body) {
        return res.status(400).send('Bad Request');
      }
      
      const phone = From.replace('whatsapp:', '').replace('+', '');
      
      const clientResult = await findClientByPhone(phone);
      let workspaceId: string | null = null;
      let clientId: string | null = null;
      
      if (clientResult) {
        workspaceId = clientResult.workspaceId;
        clientId = clientResult.client.id;
      } else {
        workspaceId = await findWorkspaceByLastMessage(phone);
      }
      
      if (!workspaceId) {
        console.log(`WhatsApp message from unknown number: ${phone}`);
        res.status(200).send('<Response></Response>');
        return;
      }
      
      await logIncomingWhatsapp(phone, Body, workspaceId, clientId || undefined);
      
      // Process with chatbot if enabled
      const { response, shouldReply } = await handleIncomingWhatsapp(
        workspaceId, 
        phone, 
        Body, 
        clientId || undefined
      );
      
      if (shouldReply && response) {
        console.log(`Chatbot reply for workspace ${workspaceId}: ${response.substring(0, 50)}...`);
        sendChatbotReply(workspaceId, phone, response).catch(console.error);
      }
      
      res.status(200).send('<Response></Response>');
    } catch (error: any) {
      console.error('WhatsApp webhook error:', error);
      res.status(500).send('Internal Server Error');
    }
  });

  // Check feature availability
  app.get("/api/workspaces/:workspaceId/features", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const subscription = await storage.getWorkspaceSubscription(req.params.workspaceId);
      
      if (!subscription) {
        return res.json({
          canUseSms: false,
          canUseWhatsapp: false,
          canUseChatbot: false,
          canUseAi: false,
          isTrial: false,
          trialEndsAt: null
        });
      }
      
      const now = new Date();
      const isTrial = subscription.status === 'TRIAL' && subscription.trialEndsAt && now < subscription.trialEndsAt;
      
      res.json({
        canUseSms: isTrial || subscription.canUseSms,
        canUseWhatsapp: isTrial || subscription.canUseWhatsapp,
        canUseChatbot: isTrial || subscription.canUseChatbot,
        canUseAi: isTrial || subscription.canUseAi,
        isTrial,
        trialEndsAt: subscription.trialEndsAt
      });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ WORKSPACE SETTINGS ============

  app.patch("/api/workspaces/:workspaceId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { defaultProposalTemplateId } = req.body;
      const workspace = await storage.updateWorkspace(req.params.workspaceId, { defaultProposalTemplateId });
      res.json(workspace);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ EMAIL TEMPLATES ============

  app.get("/api/workspaces/:workspaceId/email-templates", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const templates = await storage.getEmailTemplatesByWorkspace(req.params.workspaceId);
      res.json(templates);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/email-templates/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.getEmailTemplateById(req.params.id);
      if (!template) {
        return res.status(404).json({ message: "Template not found" });
      }
      res.json(template);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/email-templates", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.createEmailTemplate({
        ...req.body,
        workspaceId: req.params.workspaceId,
        isSystem: false,
      });
      res.status(201).json(template);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/email-templates/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.updateEmailTemplate(req.params.id, req.params.workspaceId, req.body);
      if (!template) {
        return res.status(404).json({ message: "Template not found" });
      }
      res.json(template);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/email-templates/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteEmailTemplate(req.params.id, req.params.workspaceId);
      res.json({ message: "Template deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ CLIENT ROUTES ============
  
  app.get("/api/workspaces/:workspaceId/clients", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { page, pageSize, search, clientType, sortBy } = req.query;
      
      if (page || search || clientType || sortBy) {
        const options: any = {};
        if (page) options.page = parseInt(page as string);
        if (pageSize) options.pageSize = parseInt(pageSize as string);
        if (search && typeof search === 'string') options.search = search;
        if (clientType && typeof clientType === 'string') options.clientType = clientType;
        if (sortBy && typeof sortBy === 'string') options.sortBy = sortBy;
        
        const result = await storage.getClientsByWorkspacePaginated(req.params.workspaceId, options);
        res.json(result);
      } else {
        const clients = await storage.getClientsByWorkspace(req.params.workspaceId);
        res.json(clients);
      }
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/clients", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertClientSchema.parse({ ...req.body, workspaceId: req.params.workspaceId });
      const client = await storage.createClient(data);
      res.status(201).json(client);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/clients/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const client = await storage.getClientById(req.params.id, req.params.workspaceId);
      if (!client) {
        return res.status(404).json({ message: "Client not found" });
      }
      res.json(client);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/clients/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const client = await storage.updateClient(req.params.id, req.params.workspaceId, req.body);
      if (!client) {
        return res.status(404).json({ message: "Client not found" });
      }
      res.json(client);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/clients/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteClient(req.params.id, req.params.workspaceId);
      res.json({ message: "Client deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ CLIENT CONTACTS ROUTES ============

  app.get("/api/workspaces/:workspaceId/clients/:clientId/contacts", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const contacts = await storage.getClientContacts(req.params.clientId);
      res.json(contacts);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/clients/:clientId/contacts", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertClientContactSchema.parse({ ...req.body, clientId: req.params.clientId });
      const contact = await storage.createClientContact(data);
      res.status(201).json(contact);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/clients/:clientId/contacts/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const contact = await storage.updateClientContact(req.params.id, req.body);
      if (!contact) {
        return res.status(404).json({ message: "Contact not found" });
      }
      res.json(contact);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/clients/:clientId/contacts/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteClientContact(req.params.id);
      res.json({ message: "Contact deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/clients/:clientId/contacts/:id/set-primary", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.setPrimaryContact(req.params.clientId, req.params.id);
      res.json({ message: "Primary contact updated" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Client Messages
  app.get("/api/workspaces/:workspaceId/clients/:clientId/messages", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const messages = await storage.getClientMessages(req.params.clientId, req.params.workspaceId);
      res.json(messages);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/clients/:clientId/messages", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const message = await storage.createClientMessage({
        ...req.body,
        workspaceId: req.params.workspaceId,
        clientId: req.params.clientId,
        sentBy: req.user!.id,
        status: 'SENT',
        sentAt: new Date(),
      });
      res.status(201).json(message);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/clients/:clientId/messages/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteClientMessage(req.params.id, req.params.workspaceId);
      res.json({ message: "Message deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Client 360 View (aggregated data)
  app.get("/api/workspaces/:workspaceId/clients/:clientId/360", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const workspaceId = req.params.workspaceId;
      const clientId = req.params.clientId;

      const [client, projects, tasks, proposals, invoices, transactions, contracts, messages, contacts] = await Promise.all([
        storage.getClientById(clientId, workspaceId),
        storage.getProjectsByWorkspace(workspaceId),
        storage.getTasksByWorkspace(workspaceId),
        storage.getProposalsByWorkspace(workspaceId),
        storage.getInvoicesByWorkspace(workspaceId),
        storage.getTransactionsByWorkspace(workspaceId),
        storage.getContractsByWorkspace(workspaceId),
        storage.getClientMessages(clientId, workspaceId),
        storage.getClientContacts(clientId),
      ]);

      if (!client) {
        return res.status(404).json({ message: "Client not found" });
      }

      const clientProjects = projects.filter(p => p.clientId === clientId);
      const clientProjectIds = clientProjects.map(p => p.id);
      const clientTasks = tasks.filter(t => clientProjectIds.includes(t.projectId));
      const clientProposals = proposals.filter(p => p.clientId === clientId);
      const clientInvoices = invoices.filter(i => i.clientId === clientId);
      const clientTransactions = transactions.filter(t => t.clientId === clientId);
      const clientContracts = contracts.filter(c => c.clientId === clientId);

      res.json({
        client,
        contacts,
        projects: clientProjects,
        openTasks: clientTasks.filter(t => !t.isCompleted),
        activeProjects: clientProjects.filter(p => p.status === 'IN_PROGRESS'),
        openProposals: clientProposals.filter(p => p.status === 'SENT' || p.status === 'DRAFT'),
        pendingInvoices: clientInvoices.filter(i => i.status === 'SENT' || i.status === 'OVERDUE'),
        activeContracts: clientContracts.filter(c => c.status === 'SIGNED' || c.status === 'ACTIVE'),
        recentTransactions: clientTransactions.slice(0, 10),
        messages: messages.slice(0, 20),
        stats: {
          totalProjects: clientProjects.length,
          activeProjects: clientProjects.filter(p => p.status === 'IN_PROGRESS').length,
          completedProjects: clientProjects.filter(p => p.status === 'COMPLETED').length,
          totalTasks: clientTasks.length,
          openTasks: clientTasks.filter(t => !t.isCompleted).length,
          totalProposals: clientProposals.length,
          acceptedProposals: clientProposals.filter(p => p.status === 'SIGNED').length,
          totalInvoices: clientInvoices.length,
          paidInvoices: clientInvoices.filter(i => i.status === 'PAID').length,
          totalRevenue: clientTransactions
            .filter(t => t.type === 'INCOME' && (t.status === 'RECEIVED' || t.status === 'PAID'))
            .reduce((sum, t) => sum + parseFloat(t.amount), 0),
        },
      });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ PROJECT ROUTES ============
  
  app.get("/api/workspaces/:workspaceId/projects", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const projects = await storage.getProjectsByWorkspace(req.params.workspaceId);
      res.json(projects);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/projects", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertProjectSchema.parse({ ...req.body, workspaceId: req.params.workspaceId });
      const project = await storage.createProject(data);
      res.status(201).json(project);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/projects/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const project = await storage.updateProject(req.params.id, req.params.workspaceId, req.body);
      if (!project) {
        return res.status(404).json({ message: "Project not found" });
      }
      res.json(project);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/projects/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteProject(req.params.id, req.params.workspaceId);
      res.json({ message: "Project deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/projects/reorder", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { projectIds, status } = req.body;
      if (!Array.isArray(projectIds)) {
        return res.status(400).json({ message: "projectIds must be an array" });
      }
      await storage.reorderProjects(req.params.workspaceId, projectIds, status);
      res.json({ message: "Projects reordered" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Project Responsibles
  app.get("/api/workspaces/:workspaceId/projects/:projectId/responsibles", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const responsibles = await storage.getProjectResponsibles(req.params.projectId);
      res.json(responsibles);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/projects/:projectId/responsibles", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { memberId } = req.body;
      if (!memberId) {
        return res.status(400).json({ message: "memberId is required" });
      }
      const responsible = await storage.addProjectResponsible(req.params.projectId, memberId);
      res.status(201).json(responsible);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/projects/:projectId/responsibles/:memberId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.removeProjectResponsible(req.params.projectId, req.params.memberId);
      res.json({ message: "Responsible removed" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Project Followers
  app.get("/api/workspaces/:workspaceId/projects/:projectId/followers", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const followers = await storage.getProjectFollowers(req.params.projectId);
      res.json(followers);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/projects/:projectId/followers", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { memberId } = req.body;
      if (!memberId) {
        return res.status(400).json({ message: "memberId is required" });
      }
      const follower = await storage.addProjectFollower(req.params.projectId, memberId);
      res.status(201).json(follower);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/projects/:projectId/followers/:memberId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.removeProjectFollower(req.params.projectId, req.params.memberId);
      res.json({ message: "Follower removed" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Project Communications
  app.get("/api/workspaces/:workspaceId/projects/:projectId/communications", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const communications = await storage.getProjectCommunications(req.params.projectId);
      res.json(communications);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/projects/:projectId/communications", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = {
        ...req.body,
        projectId: req.params.projectId,
        workspaceId: req.params.workspaceId,
        createdById: (req as any).userId,
      };
      const communication = await storage.createProjectCommunication(data);
      res.status(201).json(communication);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/projects/:projectId/communications/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const communication = await storage.updateProjectCommunication(req.params.id, req.params.workspaceId, req.body);
      if (!communication) {
        return res.status(404).json({ message: "Communication not found" });
      }
      res.json(communication);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/projects/:projectId/communications/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteProjectCommunication(req.params.id, req.params.workspaceId);
      res.json({ message: "Communication deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ TASK ROUTES ============
  
  app.get("/api/workspaces/:workspaceId/tasks", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const tasks = await storage.getTasksByWorkspace(req.params.workspaceId);
      res.json(tasks);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/tasks", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertTaskSchema.parse({ ...req.body, workspaceId: req.params.workspaceId });
      const task = await storage.createTask(data);
      res.status(201).json(task);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/tasks/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const task = await storage.getTaskById(req.params.id, req.params.workspaceId);
      if (!task) {
        return res.status(404).json({ message: "Task not found" });
      }
      res.json(task);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/tasks/:id/toggle", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const task = await storage.toggleTaskComplete(req.params.id, req.params.workspaceId);
      if (!task) {
        return res.status(404).json({ message: "Task not found" });
      }
      res.json(task);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/tasks/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const task = await storage.updateTask(req.params.id, req.params.workspaceId, req.body);
      if (!task) {
        return res.status(404).json({ message: "Task not found" });
      }
      res.json(task);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/tasks/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteTask(req.params.id, req.params.workspaceId);
      res.json({ message: "Task deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/tasks/reorder", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { taskIds, isCompleted } = req.body;
      if (!Array.isArray(taskIds)) {
        return res.status(400).json({ message: "taskIds must be an array" });
      }
      await storage.reorderTasks(req.params.workspaceId, taskIds, isCompleted);
      res.json({ message: "Tasks reordered" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/tasks/parse-from-text", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { text } = req.body;
      if (!text || typeof text !== 'string') {
        return res.status(400).json({ message: "Text is required" });
      }
      if (text.length > 50000) {
        return res.status(400).json({ message: "Text is too long (max 50000 characters)" });
      }
      const result = await extractTasksFromText(text);
      res.json(result);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/tasks/batch-create", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { tasks, projectId } = req.body;
      if (!Array.isArray(tasks) || tasks.length === 0) {
        return res.status(400).json({ message: "Tasks array is required" });
      }
      if (tasks.length > 50) {
        return res.status(400).json({ message: "Maximum 50 tasks per batch" });
      }
      
      const createdTasks = [];
      for (const task of tasks) {
        const data = insertTaskSchema.parse({
          title: task.title,
          description: task.description || null,
          priority: task.priority || 'MEDIUM',
          estimatedMinutes: task.estimatedMinutes || null,
          projectId: projectId || null,
          workspaceId: req.params.workspaceId,
        });
        const created = await storage.createTask(data);
        createdTasks.push(created);
      }
      
      res.status(201).json({ created: createdTasks.length, tasks: createdTasks });
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  // ============ TASK COMMENTS ROUTES ============

  app.get("/api/workspaces/:workspaceId/tasks/:taskId/comments", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const comments = await storage.getTaskComments(req.params.taskId);
      res.json(comments);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/tasks/:taskId/comments", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertTaskCommentSchema.parse({ 
        ...req.body, 
        taskId: req.params.taskId,
        userId: req.user!.id 
      });
      const comment = await storage.createTaskComment(data);
      res.status(201).json(comment);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/tasks/:taskId/comments/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteTaskComment(req.params.id);
      res.json({ message: "Comment deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ TIME ENTRIES ROUTES ============

  app.get("/api/workspaces/:workspaceId/time-entries", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const entries = await storage.getTimeEntriesByWorkspace(req.params.workspaceId);
      res.json(entries);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/time-entries", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertTimeEntrySchema.parse({ 
        ...req.body, 
        workspaceId: req.params.workspaceId,
        userId: req.user!.id 
      });
      const entry = await storage.createTimeEntry(data);
      res.status(201).json(entry);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/time-entries/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const entry = await storage.updateTimeEntry(req.params.id, req.params.workspaceId, req.body);
      if (!entry) {
        return res.status(404).json({ message: "Time entry not found" });
      }
      res.json(entry);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/time-entries/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteTimeEntry(req.params.id, req.params.workspaceId);
      res.json({ message: "Time entry deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ INVOICE ROUTES ============
  
  app.get("/api/workspaces/:workspaceId/invoices", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const invoices = await storage.getInvoicesByWorkspace(req.params.workspaceId);
      
      const invoicesWithTotal = await Promise.all(
        invoices.map(async (invoice) => {
          const items = await storage.getInvoiceItems(invoice.id);
          const total = items.reduce((sum, item) => {
            return sum + parseFloat(String(item.price || 0)) * (item.quantity || 1);
          }, 0);
          return { ...invoice, total };
        })
      );
      
      res.json(invoicesWithTotal);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/invoices", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { items, ...invoiceData } = req.body;
      const data = insertInvoiceSchema.parse({ ...invoiceData, workspaceId: req.params.workspaceId });
      const invoice = await storage.createInvoice(data);
      
      // Create items if provided
      if (items && Array.isArray(items)) {
        for (const item of items) {
          await storage.createInvoiceItem({ ...item, invoiceId: invoice.id });
        }
      }
      
      // Send SMS/WhatsApp if invoice is created with status SENT
      if (invoice.status === 'SENT' && invoice.clientId) {
        const client = await storage.getClientById(invoice.clientId, req.params.workspaceId);
        const smsSettings = await getSmsSettings(req.params.workspaceId);
        const whatsappSettings = await getWhatsappSettings(req.params.workspaceId);
        const baseUrl = process.env.REPLIT_DEV_DOMAIN 
          ? `https://${process.env.REPLIT_DEV_DOMAIN}`
          : 'https://dominus.zensuite.com.br';
        const createdItems = await storage.getInvoiceItems(invoice.id);
        const total = createdItems.reduce((sum, item) => sum + parseFloat(String(item.price || 0)) * (item.quantity || 1), 0);
        const dueDate = invoice.dueDate ? new Date(invoice.dueDate).toLocaleDateString('pt-BR') : '';
        
        const messageVars = {
          nome: client?.name || '',
          numero: invoice.number || '',
          valor: total.toFixed(2),
          vencimento: dueDate,
          link: baseUrl,
        };
        
        if (client?.phone && smsSettings?.smsEnabled) {
          sendTemplateSms('INVOICE_SENT', client.phone, messageVars, { workspaceId: req.params.workspaceId, toName: client.name }).catch(console.error);
        }
        if (client?.phone && whatsappSettings?.whatsappEnabled) {
          sendTemplateWhatsapp('INVOICE_SENT', client.phone, messageVars, { workspaceId: req.params.workspaceId, toName: client.name }).catch(console.error);
        }
      }
      
      res.status(201).json(invoice);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/invoices/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const oldInvoice = await storage.getInvoiceById(req.params.id, req.params.workspaceId);
      
      const updateData = { ...req.body };
      if (updateData.issueDate && typeof updateData.issueDate === 'string') {
        updateData.issueDate = new Date(updateData.issueDate);
      }
      if (updateData.dueDate && typeof updateData.dueDate === 'string') {
        updateData.dueDate = new Date(updateData.dueDate);
      }
      
      const invoice = await storage.updateInvoice(req.params.id, req.params.workspaceId, updateData);
      if (!invoice) {
        return res.status(404).json({ message: "Invoice not found" });
      }
      
      if (invoice.clientId) {
        const client = await storage.getClientById(invoice.clientId, req.params.workspaceId);
        const smsSettings = await getSmsSettings(req.params.workspaceId);
        const whatsappSettings = await getWhatsappSettings(req.params.workspaceId);
        
        if (client?.phone) {
          const baseUrl = process.env.REPLIT_DEV_DOMAIN 
            ? `https://${process.env.REPLIT_DEV_DOMAIN}`
            : 'https://dominus.zensuite.com.br';
          const items = await storage.getInvoiceItems(invoice.id);
          const total = items.reduce((sum, item) => sum + parseFloat(String(item.price || 0)) * (item.quantity || 1), 0);
          const dueDate = invoice.dueDate ? new Date(invoice.dueDate).toLocaleDateString('pt-BR') : '';
          
          if (req.body.status === 'SENT' && oldInvoice?.status !== 'SENT') {
            const sentVars = {
              nome: client.name,
              numero: invoice.number || '',
              valor: total.toFixed(2),
              vencimento: dueDate,
              link: baseUrl,
            };
            if (smsSettings?.smsEnabled) {
              sendTemplateSms('INVOICE_SENT', client.phone, sentVars, { workspaceId: req.params.workspaceId, toName: client.name }).catch(console.error);
            }
            if (whatsappSettings?.whatsappEnabled) {
              sendTemplateWhatsapp('INVOICE_SENT', client.phone, sentVars, { workspaceId: req.params.workspaceId, toName: client.name }).catch(console.error);
            }
          } else if (req.body.status === 'PAID' && oldInvoice?.status !== 'PAID') {
            const paidVars = {
              nome: client.name,
              numero: invoice.number || '',
            };
            if (smsSettings?.smsEnabled) {
              sendTemplateSms('INVOICE_PAID', client.phone, paidVars, { workspaceId: req.params.workspaceId, toName: client.name }).catch(console.error);
            }
            if (whatsappSettings?.whatsappEnabled) {
              sendTemplateWhatsapp('INVOICE_PAID', client.phone, paidVars, { workspaceId: req.params.workspaceId, toName: client.name }).catch(console.error);
            }
          }
        }
      }
      
      res.json(invoice);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/invoices/:id/items", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const items = await storage.getInvoiceItems(req.params.id);
      res.json(items);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/invoices/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteInvoice(req.params.id, req.params.workspaceId);
      res.json({ message: "Invoice deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ PROPOSAL ROUTES ============
  
  app.get("/api/workspaces/:workspaceId/proposals", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const proposals = await storage.getProposalsByWorkspace(req.params.workspaceId);
      res.json(proposals);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/proposals", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { items, ...proposalData } = req.body;
      const publicToken = randomBytes(32).toString('hex');
      const data = insertProposalSchema.parse({ ...proposalData, workspaceId: req.params.workspaceId, publicToken });
      const proposal = await storage.createProposal(data);
      
      if (items && Array.isArray(items)) {
        for (const item of items) {
          if (item.description && item.price) {
            await storage.createProposalItem({ ...item, proposalId: proposal.id });
          }
        }
      }
      
      const savedItems = await storage.getProposalItems(proposal.id);
      res.status(201).json({ ...proposal, items: savedItems });
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/proposals/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const proposal = await storage.getProposalById(req.params.id, req.params.workspaceId);
      if (!proposal) {
        return res.status(404).json({ message: "Proposal not found" });
      }
      const items = await storage.getProposalItems(proposal.id);
      res.json({ ...proposal, items });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/proposals/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { items, ...proposalData } = req.body;
      
      const oldProposal = await storage.getProposalById(req.params.id, req.params.workspaceId);
      const proposal = await storage.updateProposal(req.params.id, req.params.workspaceId, proposalData);
      if (!proposal) {
        return res.status(404).json({ message: "Proposal not found" });
      }
      
      if (items && Array.isArray(items)) {
        await storage.deleteProposalItems(proposal.id);
        for (const item of items) {
          if (item.description && item.price) {
            await storage.createProposalItem({ ...item, proposalId: proposal.id });
          }
        }
      }
      
      if (proposalData.status === 'SENT' && oldProposal?.status !== 'SENT' && proposal.clientId) {
        const client = await storage.getClientById(proposal.clientId, req.params.workspaceId);
        const smsSettings = await getSmsSettings(req.params.workspaceId);
        const whatsappSettings = await getWhatsappSettings(req.params.workspaceId);
        
        if (client?.phone) {
          const baseUrl = process.env.REPLIT_DEV_DOMAIN 
            ? `https://${process.env.REPLIT_DEV_DOMAIN}`
            : 'https://dominus.zensuite.com.br';
          const sentVars = {
            nome: client.name,
            proposta: proposal.title || 'Proposta',
            link: proposal.publicToken ? `${baseUrl}/p/${proposal.publicToken}` : baseUrl,
          };
          if (smsSettings?.smsEnabled) {
            sendTemplateSms('PROPOSAL_SENT', client.phone, sentVars, { workspaceId: req.params.workspaceId, toName: client.name }).catch(console.error);
          }
          if (whatsappSettings?.whatsappEnabled) {
            sendTemplateWhatsapp('PROPOSAL_SENT', client.phone, sentVars, { workspaceId: req.params.workspaceId, toName: client.name }).catch(console.error);
          }
        }
      }
      
      const savedItems = await storage.getProposalItems(proposal.id);
      res.json({ ...proposal, items: savedItems });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/proposals/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteProposal(req.params.id, req.params.workspaceId);
      res.json({ message: "Proposal deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/proposals/:id/items", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const items = await storage.getProposalItems(req.params.id);
      res.json(items);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Generate public link for proposal
  app.post("/api/workspaces/:workspaceId/proposals/:id/generate-link", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const token = randomBytes(32).toString('hex');
      const proposal = await storage.updateProposal(req.params.id, req.params.workspaceId, { 
        publicToken: token 
      });
      if (!proposal) {
        return res.status(404).json({ message: "Proposal not found" });
      }
      res.json({ token, url: `/p/${token}` });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Generate contract from proposal
  app.post("/api/workspaces/:workspaceId/proposals/:id/generate-contract", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const proposal = await storage.getProposalById(req.params.id, req.params.workspaceId);
      if (!proposal) {
        return res.status(404).json({ message: "Proposta não encontrada" });
      }

      const proposalItems = await storage.getProposalItems(proposal.id);
      
      const generateContractSchema = z.object({
        title: z.string().min(1),
        content: z.string().optional().default(''),
        templateId: z.string().nullable().optional(),
        projectId: z.string().nullable().optional(),
        totalValue: z.number().positive(),
        startsOn: z.string(),
        endsOn: z.string(),
        scheduleType: z.enum(['EQUAL', 'PERCENTAGE', 'MANUAL']),
        installmentCount: z.number().min(1).optional(),
        installmentFrequency: z.string().optional(),
        installmentDueDay: z.number().min(1).max(31).optional(),
        installments: z.array(z.object({
          sequence: z.number(),
          amount: z.number(),
          percentage: z.number().optional(),
          dueDate: z.string(),
          offsetDays: z.number().optional(),
          description: z.string().optional(),
        })).optional(),
        generateTransactions: z.boolean().optional().default(false),
        categoryId: z.string().nullable().optional(),
      });

      const validated = generateContractSchema.parse(req.body);
      const publicToken = randomBytes(32).toString('hex');

      const contract = await storage.createContract({
        workspaceId: req.params.workspaceId,
        clientId: proposal.clientId,
        projectId: validated.projectId || null,
        proposalId: proposal.id,
        templateId: validated.templateId || null,
        title: validated.title,
        content: validated.content || proposal.content,
        status: 'DRAFT',
        totalValue: validated.totalValue.toString(),
        startsOn: new Date(validated.startsOn),
        endsOn: new Date(validated.endsOn),
        scheduleType: validated.scheduleType,
        installmentCount: validated.installmentCount || null,
        installmentFrequency: validated.installmentFrequency || null,
        installmentDueDay: validated.installmentDueDay || null,
        publicToken,
      });

      if (proposalItems.length > 0) {
        const contractItemsData = proposalItems.map(item => ({
          contractId: contract.id,
          productId: item.productId,
          description: item.description,
          quantity: item.quantity,
          price: item.price,
          discount: item.discount,
          discountType: item.discountType,
        }));
        await storage.createContractItemsBatch(contractItemsData);
      }

      if (validated.installments && validated.installments.length > 0) {
        const installmentsData = validated.installments.map(inst => ({
          contractId: contract.id,
          sequence: inst.sequence,
          amount: inst.amount.toString(),
          percentage: inst.percentage?.toString() || null,
          dueDate: new Date(inst.dueDate),
          offsetDays: inst.offsetDays || null,
          description: inst.description || `Parcela ${inst.sequence}/${validated.installments!.length}`,
          status: 'SCHEDULED' as const,
        }));
        await storage.createContractInstallmentsBatch(installmentsData);

        if (validated.generateTransactions) {
          const transactionsData = validated.installments.map(inst => ({
            workspaceId: req.params.workspaceId,
            type: 'INCOME' as const,
            description: `${validated.title} - Parcela ${inst.sequence}/${validated.installments!.length}`,
            amount: inst.amount.toString(),
            dueDate: new Date(inst.dueDate),
            clientId: proposal.clientId,
            projectId: validated.projectId || null,
            categoryId: validated.categoryId || null,
            status: 'PENDING' as const,
          }));
          await storage.createTransactionsBatch(transactionsData);
        }
      }

      res.status(201).json({ 
        message: "Contrato gerado com sucesso",
        contract 
      });
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Erro de validação", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  // Public proposal view (no auth required)
  app.get("/api/public/proposals/:token", async (req: Request, res: Response) => {
    try {
      const proposal = await storage.getProposalByToken(req.params.token);
      if (!proposal) {
        return res.status(404).json({ message: "Proposal not found" });
      }
      
      // Record view time if not already viewed
      if (!proposal.viewedAt) {
        await storage.updateProposal(proposal.id, proposal.workspaceId, {
          viewedAt: new Date(),
        });
      }
      
      const items = await storage.getProposalItems(proposal.id);
      const client = await storage.getClientById(proposal.clientId, proposal.workspaceId);
      res.json({ proposal, client, items });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Sign proposal (no auth required)
  app.post("/api/public/proposals/:token/sign", async (req: Request, res: Response) => {
    try {
      const proposal = await storage.getProposalByToken(req.params.token);
      if (!proposal) {
        return res.status(404).json({ message: "Proposal not found" });
      }
      
      const { signedByName, signatureData } = req.body;
      const signedByIp = req.ip || req.headers['x-forwarded-for'] as string || 'unknown';
      
      // We need to get workspaceId from the proposal to update it
      const updated = await storage.updateProposal(proposal.id, proposal.workspaceId, {
        status: 'SIGNED',
        signedAt: new Date(),
        signedByName,
        signedByIp,
        signatureData,
      });
      
      res.json(updated);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ PROPOSAL COPILOT ROUTES ============

  app.post("/api/workspaces/:workspaceId/proposal-copilot/analyze", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { text } = req.body;
      if (!text || typeof text !== 'string') {
        return res.status(400).json({ message: "Texto é obrigatório" });
      }

      const entities = await extractProposalEntities(text);
      
      let clients: any[] = [];
      let products: any[] = [];

      if (entities.clientNameVariants.length > 0) {
        clients = await storage.searchClientsFlexible(req.params.workspaceId, entities.clientNameVariants);
      }

      if (entities.serviceName) {
        const serviceTerms = entities.serviceName.split(' ').filter((t: string) => t.length > 2);
        products = await storage.searchProducts(req.params.workspaceId, serviceTerms);
      }

      res.json({
        entities,
        suggestions: {
          clients,
          products,
        },
        clientFound: clients.length === 1,
        clientSuggestions: clients.length > 1,
        clientNotFound: clients.length === 0,
        productFound: products.length === 1,
        productSuggestions: products.length > 1,
        productNotFound: products.length === 0,
      });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/proposal-copilot/search-clients", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { terms } = req.body;
      if (!terms || !Array.isArray(terms)) {
        return res.status(400).json({ message: "Termos de busca são obrigatórios" });
      }
      const clients = await storage.searchClientsFlexible(req.params.workspaceId, terms);
      res.json(clients);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/proposal-copilot/search-products", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { terms } = req.body;
      if (!terms || !Array.isArray(terms)) {
        return res.status(400).json({ message: "Termos de busca são obrigatórios" });
      }
      const products = await storage.searchProducts(req.params.workspaceId, terms);
      res.json(products);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ VISUAL PROPOSAL BUILDER ROUTES ============

  // Get proposal templates for workspace (includes public templates)
  app.get("/api/workspaces/:workspaceId/proposal-templates", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const templates = await storage.getProposalTemplates(req.params.workspaceId);
      res.json(templates);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Create proposal template
  app.post("/api/workspaces/:workspaceId/proposal-templates", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertProposalTemplateSchema.parse({ ...req.body, workspaceId: req.params.workspaceId });
      const template = await storage.createProposalTemplate(data);
      res.status(201).json(template);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Erro de validação", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  // Get single template with its default blocks
  app.get("/api/workspaces/:workspaceId/proposal-templates/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.getProposalTemplateById(req.params.id);
      if (!template) {
        return res.status(404).json({ message: "Template não encontrado" });
      }
      const blocks = await storage.getProposalTemplateBlocks(req.params.id);
      res.json({ template, blocks });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Update proposal template
  app.patch("/api/workspaces/:workspaceId/proposal-templates/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.updateProposalTemplate(req.params.id, req.body);
      if (!template) {
        return res.status(404).json({ message: "Template não encontrado" });
      }
      res.json(template);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Delete proposal template
  app.delete("/api/workspaces/:workspaceId/proposal-templates/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteProposalTemplate(req.params.id);
      res.status(204).send();
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Create template block
  app.post("/api/workspaces/:workspaceId/proposal-templates/:templateId/blocks", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.getProposalTemplateById(req.params.templateId);
      if (!template || template.workspaceId !== req.params.workspaceId) {
        return res.status(403).json({ message: "Acesso negado a este template" });
      }
      const block = await storage.createProposalTemplateBlock({
        templateId: req.params.templateId,
        blockType: req.body.blockType,
        position: req.body.position ?? 0,
        defaultContent: req.body.content || '{}',
        defaultStyle: req.body.style || '{}',
      });
      res.status(201).json(block);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Update template block
  app.patch("/api/workspaces/:workspaceId/proposal-templates/:templateId/blocks/:blockId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.getProposalTemplateById(req.params.templateId);
      if (!template || template.workspaceId !== req.params.workspaceId) {
        return res.status(403).json({ message: "Acesso negado a este template" });
      }
      const { content, style, position } = req.body;
      const updateData: any = { position };
      if (content !== undefined) updateData.defaultContent = content;
      if (style !== undefined) updateData.defaultStyle = style;
      const block = await storage.updateProposalTemplateBlock(req.params.blockId, req.params.templateId, updateData);
      if (!block) {
        return res.status(404).json({ message: "Bloco não encontrado" });
      }
      res.json(block);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Delete template block
  app.delete("/api/workspaces/:workspaceId/proposal-templates/:templateId/blocks/:blockId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.getProposalTemplateById(req.params.templateId);
      if (!template || template.workspaceId !== req.params.workspaceId) {
        return res.status(403).json({ message: "Acesso negado a este template" });
      }
      await storage.deleteProposalTemplateBlock(req.params.blockId, req.params.templateId);
      res.status(204).send();
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Reorder template blocks
  app.post("/api/workspaces/:workspaceId/proposal-templates/:templateId/blocks/reorder", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.getProposalTemplateById(req.params.templateId);
      if (!template || template.workspaceId !== req.params.workspaceId) {
        return res.status(403).json({ message: "Acesso negado a este template" });
      }
      const { orderedIds } = req.body;
      if (!Array.isArray(orderedIds)) {
        return res.status(400).json({ message: "orderedIds deve ser um array" });
      }
      await storage.reorderProposalTemplateBlocks(req.params.templateId, orderedIds);
      res.json({ success: true });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Apply template to a proposal (replaces existing blocks)
  app.post("/api/workspaces/:workspaceId/proposals/:proposalId/apply-template", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { templateId } = req.body;
      if (!templateId) {
        return res.status(400).json({ message: "templateId é obrigatório" });
      }
      
      // Verify template belongs to workspace or is public
      const template = await storage.getProposalTemplateById(templateId);
      if (!template) {
        return res.status(404).json({ message: "Template não encontrado" });
      }
      
      // Security check: template must be public or belong to this workspace
      if (!template.isPublic && template.workspaceId !== req.params.workspaceId) {
        return res.status(403).json({ message: "Acesso negado a este template" });
      }
      
      // Get template blocks
      const templateBlocks = await storage.getProposalTemplateBlocks(templateId);
      if (!templateBlocks || templateBlocks.length === 0) {
        return res.status(404).json({ message: "Template não possui blocos" });
      }
      
      // Delete existing blocks for this proposal (bulk delete)
      await storage.deleteAllProposalBlockInstances(req.params.proposalId);
      
      // Create new blocks from template
      const blocksToCreate = templateBlocks.map((tb: any, index: number) => ({
        proposalId: req.params.proposalId,
        blockType: tb.blockType,
        position: index,
        content: tb.defaultContent || '{}',
        style: tb.defaultStyle || '{}',
        isVisible: true,
      }));
      
      const createdBlocks = await storage.createProposalBlockInstances(blocksToCreate);
      
      // Also enable visual builder flag on the proposal
      await storage.updateProposal(req.params.proposalId, req.params.workspaceId, {
        useVisualBuilder: true,
        templateId: templateId,
        templateStyle: template.style,
      });
      
      res.json(createdBlocks);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get proposal blocks
  app.get("/api/workspaces/:workspaceId/proposals/:proposalId/blocks", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const blocks = await storage.getProposalBlockInstances(req.params.proposalId);
      res.json(blocks);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Create proposal block
  app.post("/api/workspaces/:workspaceId/proposals/:proposalId/blocks", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertProposalBlockInstanceSchema.parse({ ...req.body, proposalId: req.params.proposalId });
      const block = await storage.createProposalBlockInstance(data);
      res.status(201).json(block);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Erro de validação", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  // Batch create blocks (for initializing from template)
  app.post("/api/workspaces/:workspaceId/proposals/:proposalId/blocks/batch", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { blocks } = req.body;
      if (!Array.isArray(blocks)) {
        return res.status(400).json({ message: "blocks deve ser um array" });
      }
      const blocksData = blocks.map((b: any, i: number) => insertProposalBlockInstanceSchema.parse({ 
        ...b, 
        proposalId: req.params.proposalId,
        position: b.position ?? i 
      }));
      const created = await storage.createProposalBlockInstances(blocksData);
      res.status(201).json(created);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Erro de validação", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  // Update block
  app.patch("/api/workspaces/:workspaceId/proposals/:proposalId/blocks/:blockId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const block = await storage.updateProposalBlockInstance(req.params.blockId, req.body);
      if (!block) {
        return res.status(404).json({ message: "Bloco não encontrado" });
      }
      res.json(block);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Delete block
  app.delete("/api/workspaces/:workspaceId/proposals/:proposalId/blocks/:blockId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteProposalBlockInstance(req.params.blockId);
      res.status(204).send();
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Reorder blocks
  app.post("/api/workspaces/:workspaceId/proposals/:proposalId/blocks/reorder", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { orderedIds } = req.body;
      if (!Array.isArray(orderedIds)) {
        return res.status(400).json({ message: "orderedIds deve ser um array" });
      }
      await storage.reorderProposalBlockInstances(req.params.proposalId, orderedIds);
      res.status(200).json({ message: "Blocos reordenados" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Delete all blocks (to reset proposal)
  app.delete("/api/workspaces/:workspaceId/proposals/:proposalId/blocks", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteAllProposalBlockInstances(req.params.proposalId);
      res.status(204).send();
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get proposal with blocks for visual builder
  app.get("/api/workspaces/:workspaceId/proposals/:proposalId/visual", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const result = await storage.getProposalWithBlocks(req.params.proposalId);
      if (!result) {
        return res.status(404).json({ message: "Proposta não encontrada" });
      }
      const items = await storage.getProposalItems(req.params.proposalId);
      const client = await storage.getClientById(result.proposal.clientId, req.params.workspaceId);
      res.json({ ...result, items, client });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Initialize proposal with template blocks
  app.post("/api/workspaces/:workspaceId/proposals/:proposalId/initialize-template", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { templateId } = req.body;
      const template = await storage.getProposalTemplateById(templateId);
      if (!template) {
        return res.status(404).json({ message: "Template não encontrado" });
      }
      
      // Get template blocks
      const templateBlocks = await storage.getProposalTemplateBlocks(templateId);
      
      // Clear existing blocks
      await storage.deleteAllProposalBlockInstances(req.params.proposalId);
      
      // Create new blocks from template
      const newBlocks = templateBlocks.map(tb => ({
        proposalId: req.params.proposalId,
        blockType: tb.blockType,
        position: tb.position,
        content: tb.defaultContent,
        style: tb.defaultStyle,
        isVisible: true,
      }));
      
      const created = await storage.createProposalBlockInstances(newBlocks);
      
      // Update proposal with template info
      await storage.updateProposal(req.params.proposalId, req.params.workspaceId, {
        useVisualBuilder: true,
        templateId,
        templateStyle: template.style,
      });
      
      res.status(201).json({ blocks: created, template });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Public route: Get visual proposal with blocks
  app.get("/api/public/proposals/:token/visual", async (req: Request, res: Response) => {
    try {
      const proposal = await storage.getProposalByToken(req.params.token);
      if (!proposal) {
        return res.status(404).json({ message: "Proposta não encontrada" });
      }
      
      if (!proposal.viewedAt) {
        await storage.updateProposal(proposal.id, proposal.workspaceId, {
          viewedAt: new Date(),
        });
      }
      
      const blocks = await storage.getProposalBlockInstances(proposal.id);
      const items = await storage.getProposalItems(proposal.id);
      const client = await storage.getClientById(proposal.clientId, proposal.workspaceId);
      const workspace = await storage.getWorkspaceById(proposal.workspaceId);
      
      res.json({ proposal, blocks, items, client, workspace });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ FINANCIAL CATEGORIES ROUTES ============

  app.get("/api/workspaces/:workspaceId/financial-categories", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const categories = await storage.getFinancialCategoriesByWorkspace(req.params.workspaceId);
      res.json(categories);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/financial-categories", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertFinancialCategorySchema.parse({ ...req.body, workspaceId: req.params.workspaceId });
      const category = await storage.createFinancialCategory(data);
      res.status(201).json(category);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/financial-categories/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const category = await storage.updateFinancialCategory(req.params.id, req.params.workspaceId, req.body);
      if (!category) {
        return res.status(404).json({ message: "Category not found" });
      }
      res.json(category);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/financial-categories/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteFinancialCategory(req.params.id, req.params.workspaceId);
      res.json({ message: "Category deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ TRANSACTIONS ROUTES ============

  app.get("/api/workspaces/:workspaceId/transactions", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const transactions = await storage.getTransactionsByWorkspace(req.params.workspaceId);
      res.json(transactions);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/transactions", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertTransactionSchema.parse({ ...req.body, workspaceId: req.params.workspaceId });
      const transaction = await storage.createTransaction(data);
      res.status(201).json(transaction);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/transactions/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const transaction = await storage.getTransactionById(req.params.id, req.params.workspaceId);
      if (!transaction) {
        return res.status(404).json({ message: "Transaction not found" });
      }
      const attachments = await storage.getTransactionAttachments(transaction.id);
      res.json({ ...transaction, attachments });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/transactions/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const transaction = await storage.updateTransaction(req.params.id, req.params.workspaceId, req.body);
      if (!transaction) {
        return res.status(404).json({ message: "Transaction not found" });
      }
      res.json(transaction);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/transactions/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteTransaction(req.params.id, req.params.workspaceId);
      res.json({ message: "Transaction deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Create recurring transactions
  app.post("/api/workspaces/:workspaceId/transactions/recurring", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const recurringSchema = z.object({
        type: z.enum(['INCOME', 'EXPENSE']),
        description: z.string().min(1, "Descrição é obrigatória"),
        amount: z.number().positive("Valor deve ser positivo"),
        categoryId: z.string().nullable().optional(),
        clientId: z.string().nullable().optional(),
        projectId: z.string().nullable().optional(),
        supplier: z.string().nullable().optional(),
        notes: z.string().nullable().optional(),
        startDate: z.string(),
        dueDay: z.number().min(1).max(31).optional(),
        recurrenceCount: z.number().min(1).max(60),
        recurrenceType: z.enum(['MONTHLY', 'WEEKLY', 'BIWEEKLY', 'YEARLY']).default('MONTHLY'),
      });

      const validated = recurringSchema.parse(req.body);
      const { 
        type, description, amount, categoryId, clientId, projectId,
        supplier, notes, startDate, dueDay, recurrenceCount, recurrenceType 
      } = validated;

      const start = new Date(startDate);
      const transactionsToCreate = [];
      
      for (let i = 0; i < recurrenceCount; i++) {
        const dueDate = new Date(start);
        
        if (recurrenceType === 'MONTHLY') {
          dueDate.setMonth(dueDate.getMonth() + i);
          if (dueDay) dueDate.setDate(Math.min(dueDay, new Date(dueDate.getFullYear(), dueDate.getMonth() + 1, 0).getDate()));
        } else if (recurrenceType === 'WEEKLY') {
          dueDate.setDate(dueDate.getDate() + (i * 7));
        } else if (recurrenceType === 'BIWEEKLY') {
          dueDate.setDate(dueDate.getDate() + (i * 14));
        } else if (recurrenceType === 'YEARLY') {
          dueDate.setFullYear(dueDate.getFullYear() + i);
          if (dueDay) dueDate.setDate(Math.min(dueDay, new Date(dueDate.getFullYear(), dueDate.getMonth() + 1, 0).getDate()));
        }

        const installmentNumber = i + 1;
        const transactionDescription = `${description} (${installmentNumber}/${recurrenceCount})`;

        transactionsToCreate.push({
          workspaceId: req.params.workspaceId,
          type,
          description: transactionDescription,
          amount: amount.toString(),
          categoryId: categoryId || null,
          clientId: clientId || null,
          projectId: projectId || null,
          supplier: supplier || null,
          notes: notes || null,
          dueDate,
          status: 'PENDING' as const,
        });
      }

      const transactions = await storage.createTransactionsBatch(transactionsToCreate);

      res.status(201).json({ 
        message: `${recurrenceCount} transações criadas com sucesso`,
        transactions 
      });
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Erro de validação", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  // Transaction Attachments
  app.get("/api/workspaces/:workspaceId/transactions/:transactionId/attachments", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const attachments = await storage.getTransactionAttachments(req.params.transactionId);
      res.json(attachments);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/transactions/:transactionId/attachments", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertTransactionAttachmentSchema.parse({ ...req.body, transactionId: req.params.transactionId });
      const attachment = await storage.createTransactionAttachment(data);
      res.status(201).json(attachment);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/transactions/:transactionId/attachments/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteTransactionAttachment(req.params.id);
      res.json({ message: "Attachment deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ BUDGETS ROUTES ============

  app.get("/api/workspaces/:workspaceId/budgets", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const year = req.query.year ? parseInt(req.query.year as string) : undefined;
      const budgets = await storage.getBudgetsByWorkspace(req.params.workspaceId, year);
      res.json(budgets);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/budgets", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertBudgetSchema.parse({ ...req.body, workspaceId: req.params.workspaceId });
      const budget = await storage.createBudget(data);
      res.status(201).json(budget);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/budgets/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const budget = await storage.updateBudget(req.params.id, req.body);
      if (!budget) {
        return res.status(404).json({ message: "Budget not found" });
      }
      res.json(budget);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/budgets/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteBudget(req.params.id);
      res.json({ message: "Budget deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ PRODUCTS ROUTES ============

  app.get("/api/workspaces/:workspaceId/products", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const products = await storage.getProductsByWorkspace(req.params.workspaceId);
      res.json(products);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/products", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertProductSchema.parse({ ...req.body, workspaceId: req.params.workspaceId });
      const product = await storage.createProduct(data);
      res.status(201).json(product);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/products/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const product = await storage.getProductById(req.params.id, req.params.workspaceId);
      if (!product) {
        return res.status(404).json({ message: "Product not found" });
      }
      res.json(product);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/products/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const product = await storage.updateProduct(req.params.id, req.params.workspaceId, req.body);
      if (!product) {
        return res.status(404).json({ message: "Product not found" });
      }
      res.json(product);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/products/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteProduct(req.params.id, req.params.workspaceId);
      res.json({ message: "Product deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ CONTRACT TEMPLATES ROUTES ============

  app.get("/api/workspaces/:workspaceId/contract-templates", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const templates = await storage.getContractTemplatesByWorkspace(req.params.workspaceId);
      res.json(templates);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/contract-templates", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertContractTemplateSchema.parse({ ...req.body, workspaceId: req.params.workspaceId });
      const template = await storage.createContractTemplate(data);
      res.status(201).json(template);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/contract-templates/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.getContractTemplateById(req.params.id, req.params.workspaceId);
      if (!template) {
        return res.status(404).json({ message: "Template not found" });
      }
      res.json(template);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/contract-templates/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const template = await storage.updateContractTemplate(req.params.id, req.params.workspaceId, req.body);
      if (!template) {
        return res.status(404).json({ message: "Template not found" });
      }
      res.json(template);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/contract-templates/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteContractTemplate(req.params.id, req.params.workspaceId);
      res.json({ message: "Template deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ CONTRACTS ROUTES ============

  app.get("/api/workspaces/:workspaceId/contracts", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const contracts = await storage.getContractsByWorkspace(req.params.workspaceId);
      res.json(contracts);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/contracts", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertContractSchema.parse({ ...req.body, workspaceId: req.params.workspaceId });
      const contract = await storage.createContract(data);
      res.status(201).json(contract);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/contracts/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const contract = await storage.getContractById(req.params.id, req.params.workspaceId);
      if (!contract) {
        return res.status(404).json({ message: "Contract not found" });
      }
      res.json(contract);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/contracts/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const oldContract = await storage.getContractById(req.params.id, req.params.workspaceId);
      const contract = await storage.updateContract(req.params.id, req.params.workspaceId, req.body);
      if (!contract) {
        return res.status(404).json({ message: "Contract not found" });
      }
      
      if (contract.clientId) {
        const client = await storage.getClientById(contract.clientId, req.params.workspaceId);
        const smsSettings = await getSmsSettings(req.params.workspaceId);
        const whatsappSettings = await getWhatsappSettings(req.params.workspaceId);
        
        if (client?.phone) {
          const baseUrl = process.env.REPLIT_DEV_DOMAIN 
            ? `https://${process.env.REPLIT_DEV_DOMAIN}`
            : 'https://dominus.zensuite.com.br';
          
          if (req.body.status === 'SENT' && oldContract?.status !== 'SENT') {
            const sentVars = {
              nome: client.name,
              contrato: contract.title || '',
              link: contract.publicToken ? `${baseUrl}/c/${contract.publicToken}` : baseUrl,
            };
            if (smsSettings?.smsEnabled) {
              sendTemplateSms('CONTRACT_SENT', client.phone, sentVars, { workspaceId: req.params.workspaceId, toName: client.name }).catch(console.error);
            }
            if (whatsappSettings?.whatsappEnabled) {
              sendTemplateWhatsapp('CONTRACT_SENT', client.phone, sentVars, { workspaceId: req.params.workspaceId, toName: client.name }).catch(console.error);
            }
          } else if (req.body.status === 'SIGNED' && oldContract?.status !== 'SIGNED') {
            const signedVars = {
              contrato: contract.title || '',
              cliente: client.name,
            };
            if (smsSettings?.smsEnabled) {
              sendTemplateSms('CONTRACT_SIGNED', client.phone, signedVars, { workspaceId: req.params.workspaceId, toName: client.name }).catch(console.error);
            }
            if (whatsappSettings?.whatsappEnabled) {
              sendTemplateWhatsapp('CONTRACT_SIGNED', client.phone, signedVars, { workspaceId: req.params.workspaceId, toName: client.name }).catch(console.error);
            }
          }
        }
      }
      
      res.json(contract);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/contracts/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteContract(req.params.id, req.params.workspaceId);
      res.json({ message: "Contract deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Generate public link for contract
  app.post("/api/workspaces/:workspaceId/contracts/:id/generate-link", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const token = randomBytes(32).toString('hex');
      const contract = await storage.updateContract(req.params.id, req.params.workspaceId, { 
        publicToken: token 
      });
      if (!contract) {
        return res.status(404).json({ message: "Contract not found" });
      }
      res.json({ token, url: `/c/${token}` });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Public contract view (no auth required)
  app.get("/api/public/contracts/:token", async (req: Request, res: Response) => {
    try {
      const contract = await storage.getContractByToken(req.params.token);
      if (!contract) {
        return res.status(404).json({ message: "Contract not found" });
      }
      res.json(contract);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Sign contract (no auth required)
  app.post("/api/public/contracts/:token/sign", async (req: Request, res: Response) => {
    try {
      const contract = await storage.getContractByToken(req.params.token);
      if (!contract) {
        return res.status(404).json({ message: "Contract not found" });
      }
      
      const { signedByName, signatureData } = req.body;
      const signedByIp = req.ip || req.headers['x-forwarded-for'] as string || 'unknown';
      
      const updated = await storage.updateContract(contract.id, contract.workspaceId, {
        status: 'SIGNED',
        signedAt: new Date(),
        signedByName,
        signedByIp,
        signatureData,
      });
      
      res.json(updated);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ LEAD WORKFLOWS ROUTES ============

  app.get("/api/workspaces/:workspaceId/lead-workflows", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const workflows = await storage.getLeadWorkflowsByWorkspace(req.params.workspaceId);
      res.json(workflows);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/lead-workflows", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertLeadWorkflowSchema.parse({ ...req.body, workspaceId: req.params.workspaceId });
      const workflow = await storage.createLeadWorkflow(data);
      res.status(201).json(workflow);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/lead-workflows/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const workflow = await storage.getLeadWorkflowById(req.params.id, req.params.workspaceId);
      if (!workflow) {
        return res.status(404).json({ message: "Workflow not found" });
      }
      const stages = await storage.getLeadWorkflowStages(workflow.id);
      res.json({ ...workflow, stages });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/lead-workflows/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const workflow = await storage.updateLeadWorkflow(req.params.id, req.params.workspaceId, req.body);
      if (!workflow) {
        return res.status(404).json({ message: "Workflow not found" });
      }
      res.json(workflow);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/lead-workflows/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteLeadWorkflow(req.params.id, req.params.workspaceId);
      res.json({ message: "Workflow deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ LEAD WORKFLOW STAGES ROUTES ============

  app.get("/api/workspaces/:workspaceId/lead-workflows/:workflowId/stages", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const stages = await storage.getLeadWorkflowStages(req.params.workflowId);
      res.json(stages);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/lead-workflows/:workflowId/stages", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertLeadWorkflowStageSchema.parse({ ...req.body, workflowId: req.params.workflowId });
      const stage = await storage.createLeadWorkflowStage(data);
      res.status(201).json(stage);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/lead-workflows/:workflowId/stages/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const stage = await storage.updateLeadWorkflowStage(req.params.id, req.body);
      if (!stage) {
        return res.status(404).json({ message: "Stage not found" });
      }
      res.json(stage);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/lead-workflows/:workflowId/stages/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteLeadWorkflowStage(req.params.id);
      res.json({ message: "Stage deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/lead-workflows/:workflowId/stages/reorder", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { stageIds } = req.body;
      if (!Array.isArray(stageIds)) {
        return res.status(400).json({ message: "stageIds must be an array" });
      }
      await storage.reorderLeadWorkflowStages(req.params.workflowId, stageIds);
      res.json({ message: "Stages reordered" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ LEAD FORMS ROUTES ============

  app.get("/api/workspaces/:workspaceId/lead-forms", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const forms = await storage.getLeadFormsByWorkspace(req.params.workspaceId);
      res.json(forms);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/lead-forms", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertLeadFormSchema.parse({ ...req.body, workspaceId: req.params.workspaceId });
      const form = await storage.createLeadForm(data);
      res.status(201).json(form);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/lead-forms/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const form = await storage.getLeadFormById(req.params.id, req.params.workspaceId);
      if (!form) {
        return res.status(404).json({ message: "Form not found" });
      }
      const fields = await storage.getLeadFormFields(form.id);
      res.json({ ...form, fields });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/lead-forms/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const form = await storage.updateLeadForm(req.params.id, req.params.workspaceId, req.body);
      if (!form) {
        return res.status(404).json({ message: "Form not found" });
      }
      res.json(form);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/lead-forms/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteLeadForm(req.params.id, req.params.workspaceId);
      res.json({ message: "Form deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ LEAD FORM FIELDS ROUTES ============

  app.post("/api/workspaces/:workspaceId/lead-forms/:formId/fields", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertLeadFormFieldSchema.parse({ ...req.body, formId: req.params.formId });
      const field = await storage.createLeadFormField(data);
      res.status(201).json(field);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/lead-forms/:formId/fields/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const field = await storage.updateLeadFormField(req.params.id, req.body);
      if (!field) {
        return res.status(404).json({ message: "Field not found" });
      }
      res.json(field);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/lead-forms/:formId/fields/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteLeadFormField(req.params.id);
      res.json({ message: "Field deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/lead-forms/:formId/fields/reorder", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { fieldIds } = req.body;
      if (!Array.isArray(fieldIds)) {
        return res.status(400).json({ message: "fieldIds must be an array" });
      }
      await storage.reorderLeadFormFields(req.params.formId, fieldIds);
      res.json({ message: "Fields reordered" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ LEAD LANDING PAGES ROUTES ============

  app.get("/api/workspaces/:workspaceId/lead-landing-pages", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const pages = await storage.getLeadLandingPagesByWorkspace(req.params.workspaceId);
      res.json(pages);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/lead-landing-pages", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertLeadLandingPageSchema.parse({ ...req.body, workspaceId: req.params.workspaceId });
      const page = await storage.createLeadLandingPage(data);
      res.status(201).json(page);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/lead-landing-pages/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const page = await storage.updateLeadLandingPage(req.params.id, req.params.workspaceId, req.body);
      if (!page) {
        return res.status(404).json({ message: "Landing page not found" });
      }
      res.json(page);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/lead-landing-pages/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteLeadLandingPage(req.params.id, req.params.workspaceId);
      res.json({ message: "Landing page deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ LEADS ROUTES ============

  app.post("/api/workspaces/:workspaceId/leads", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const lead = await storage.createLead({
        ...req.body,
        workspaceId: req.params.workspaceId,
      });
      res.status(201).json(lead);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/leads/import", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { leads } = req.body;
      if (!Array.isArray(leads)) {
        return res.status(400).json({ message: "leads deve ser um array" });
      }
      
      const created = [];
      for (const leadData of leads) {
        const lead = await storage.createLead({
          ...leadData,
          workspaceId: req.params.workspaceId,
          source: leadData.source || 'IMPORT',
        });
        created.push(lead);
      }
      
      res.status(201).json({ imported: created.length, leads: created });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/leads/export", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const leads = await storage.getLeadsByWorkspace(req.params.workspaceId);
      
      const csvHeader = "Nome,Email,Telefone,Status,Origem,Data Criação\n";
      const csvRows = leads.map(lead => {
        const name = (lead.name || '').replace(/,/g, ';');
        const email = (lead.email || '').replace(/,/g, ';');
        const phone = (lead.phone || '').replace(/,/g, ';');
        const status = lead.status || '';
        const source = lead.source || '';
        const createdAt = lead.createdAt ? new Date(lead.createdAt).toLocaleDateString('pt-BR') : '';
        return `${name},${email},${phone},${status},${source},${createdAt}`;
      }).join('\n');
      
      res.setHeader('Content-Type', 'text/csv; charset=utf-8');
      res.setHeader('Content-Disposition', 'attachment; filename=leads.csv');
      res.send(csvHeader + csvRows);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Google Contacts OAuth - Status
  app.get("/api/workspaces/:workspaceId/google-contacts/status", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const credentials = await storage.getWorkspaceGoogleCredentials(req.params.workspaceId);
      if (credentials) {
        // Consider connected if we have a refresh token (can refresh access token server-side)
        const hasRefreshToken = !!credentials.refreshToken;
        res.json({
          connected: hasRefreshToken || (credentials.accessToken && (!credentials.expiresAt || new Date(credentials.expiresAt) > new Date())),
          email: credentials.googleEmail,
          expiresAt: credentials.expiresAt,
        });
      } else {
        res.json({ connected: false });
      }
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Google Contacts OAuth - Generate Auth URL
  app.get("/api/workspaces/:workspaceId/google-contacts/auth-url", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const clientId = process.env.GOOGLE_CLIENT_ID;
      const clientSecret = process.env.GOOGLE_CLIENT_SECRET;
      
      if (!clientId || !clientSecret) {
        return res.status(400).json({ 
          message: "Credenciais do Google OAuth não configuradas. Entre em contato com o administrador." 
        });
      }
      
      const { google } = await import('googleapis');
      const baseUrl = process.env.APP_URL || (process.env.REPLIT_DEV_DOMAIN ? 'https://' + process.env.REPLIT_DEV_DOMAIN : 'http://localhost:5000');
      const redirectUri = `${baseUrl}/api/integrations/google-contacts/callback`;
      
      const oauth2Client = new google.auth.OAuth2(clientId, clientSecret, redirectUri);
      
      const state = Buffer.from(JSON.stringify({
        workspaceId: req.params.workspaceId,
        userId: (req.user as any).id,
      })).toString('base64');
      
      const authUrl = oauth2Client.generateAuthUrl({
        access_type: 'offline',
        scope: [
          'https://www.googleapis.com/auth/contacts.readonly',
          'https://www.googleapis.com/auth/userinfo.email',
        ],
        state,
        prompt: 'consent',
      });
      
      res.json({ authUrl });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Google Contacts OAuth - Callback
  app.get("/api/integrations/google-contacts/callback", async (req: Request, res: Response) => {
    try {
      const { code, state } = req.query;
      
      if (!code || !state) {
        return res.redirect('/?error=google_auth_failed');
      }
      
      const clientId = process.env.GOOGLE_CLIENT_ID;
      const clientSecret = process.env.GOOGLE_CLIENT_SECRET;
      
      if (!clientId || !clientSecret) {
        return res.redirect('/?error=google_not_configured');
      }
      
      const stateData = JSON.parse(Buffer.from(state as string, 'base64').toString());
      const { workspaceId } = stateData;
      
      const { google } = await import('googleapis');
      const baseUrl = process.env.APP_URL || (process.env.REPLIT_DEV_DOMAIN ? 'https://' + process.env.REPLIT_DEV_DOMAIN : 'http://localhost:5000');
      const redirectUri = `${baseUrl}/api/integrations/google-contacts/callback`;
      
      const oauth2Client = new google.auth.OAuth2(clientId, clientSecret, redirectUri);
      
      const { tokens } = await oauth2Client.getToken(code as string);
      oauth2Client.setCredentials(tokens);
      
      // Get user email
      const oauth2 = google.oauth2({ version: 'v2', auth: oauth2Client });
      const userInfo = await oauth2.userinfo.get();
      
      await storage.saveWorkspaceGoogleCredentials({
        workspaceId,
        accessToken: tokens.access_token!,
        refreshToken: tokens.refresh_token || null,
        expiresAt: tokens.expiry_date ? new Date(tokens.expiry_date) : null,
        scopes: tokens.scope || null,
        googleEmail: userInfo.data.email || null,
      });
      
      // Get workspace slug for redirect
      const workspace = await storage.getWorkspaceById(workspaceId);
      const redirectPath = workspace ? `/${workspace.slug}/leads?google_connected=true` : '/?google_connected=true';
      
      res.redirect(redirectPath);
    } catch (error: any) {
      console.error('Google OAuth callback error:', error);
      res.redirect('/?error=google_auth_failed');
    }
  });

  // Google Contacts OAuth - Disconnect
  app.delete("/api/workspaces/:workspaceId/google-contacts/disconnect", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteWorkspaceGoogleCredentials(req.params.workspaceId);
      res.json({ success: true });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Google Contacts - Import using workspace credentials
  app.post("/api/workspaces/:workspaceId/leads/import-google-contacts", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const credentials = await storage.getWorkspaceGoogleCredentials(req.params.workspaceId);
      
      if (!credentials) {
        return res.status(400).json({ 
          message: "Google Contacts não conectado. Conecte sua conta do Google primeiro.",
          needsAuth: true
        });
      }
      
      const clientId = process.env.GOOGLE_CLIENT_ID;
      const clientSecret = process.env.GOOGLE_CLIENT_SECRET;
      
      if (!clientId || !clientSecret) {
        return res.status(400).json({ 
          message: "Credenciais do Google OAuth não configuradas." 
        });
      }
      
      const { google } = await import('googleapis');
      const oauth2Client = new google.auth.OAuth2(clientId, clientSecret);
      
      oauth2Client.setCredentials({
        access_token: credentials.accessToken,
        refresh_token: credentials.refreshToken,
      });
      
      // Refresh token if needed
      if (credentials.expiresAt && new Date(credentials.expiresAt) < new Date()) {
        try {
          const { credentials: newTokens } = await oauth2Client.refreshAccessToken();
          await storage.saveWorkspaceGoogleCredentials({
            workspaceId: req.params.workspaceId,
            accessToken: newTokens.access_token!,
            refreshToken: newTokens.refresh_token || credentials.refreshToken,
            expiresAt: newTokens.expiry_date ? new Date(newTokens.expiry_date) : null,
            scopes: credentials.scopes,
            googleEmail: credentials.googleEmail,
          });
          oauth2Client.setCredentials(newTokens);
        } catch (refreshError) {
          await storage.deleteWorkspaceGoogleCredentials(req.params.workspaceId);
          return res.status(401).json({ 
            message: "Sessão expirada. Por favor, reconecte sua conta do Google.",
            needsAuth: true
          });
        }
      }
      
      const people = google.people({ version: 'v1', auth: oauth2Client });
      
      const response = await people.people.connections.list({
        resourceName: 'people/me',
        pageSize: 1000,
        personFields: 'names,emailAddresses,phoneNumbers',
      });
      
      const contacts = response.data.connections || [];
      const created = [];
      
      for (const contact of contacts) {
        const name = contact.names?.[0]?.displayName;
        const email = contact.emailAddresses?.[0]?.value;
        const phone = contact.phoneNumbers?.[0]?.value;
        
        if (!name && !email) continue;
        
        try {
          const lead = await storage.createLead({
            workspaceId: req.params.workspaceId,
            name: name || email || '',
            email: email || null,
            phone: phone || null,
            source: 'GOOGLE_CONTACTS',
            status: 'NEW',
          });
          created.push(lead);
        } catch (e) {
          console.log('Failed to import contact:', e);
        }
      }
      
      res.json({ imported: created.length, leads: created });
    } catch (error: any) {
      console.error('Google Contacts import error:', error?.message || error);
      console.error('Error details:', JSON.stringify({
        code: error?.code,
        status: error?.response?.status,
        data: error?.response?.data,
        message: error?.message,
      }));
      
      if (error.message?.includes('invalid_grant') || error.code === 401 || error?.response?.status === 401) {
        await storage.deleteWorkspaceGoogleCredentials(req.params.workspaceId);
        return res.status(401).json({ 
          message: "Sessão expirada. Por favor, reconecte sua conta do Google.",
          needsAuth: true
        });
      }
      
      // Check if People API is not enabled
      if (error.message?.includes('People API') || error.message?.includes('has not been used') || error?.response?.status === 403) {
        return res.status(400).json({ 
          message: "A API People não está habilitada no Google Cloud. Por favor, habilite em: https://console.cloud.google.com/apis/library/people.googleapis.com" 
        });
      }
      
      res.status(500).json({ 
        message: error?.message || "Erro ao importar contatos do Google." 
      });
    }
  });

  // ============ GOOGLE CALENDAR OAUTH (Per-Workspace) ============

  // Google Calendar OAuth - Status
  app.get("/api/workspaces/:workspaceId/google-calendar/status", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const credentials = await storage.getWorkspaceGoogleCalendarCredentials(req.params.workspaceId);
      if (credentials) {
        const hasRefreshToken = !!credentials.refreshToken;
        res.json({
          connected: hasRefreshToken || (credentials.accessToken && (!credentials.expiresAt || new Date(credentials.expiresAt) > new Date())),
          email: credentials.googleEmail,
          expiresAt: credentials.expiresAt,
        });
      } else {
        res.json({ connected: false });
      }
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Google Calendar OAuth - Generate Auth URL
  app.get("/api/workspaces/:workspaceId/google-calendar/auth-url", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const clientId = process.env.GOOGLE_CLIENT_ID;
      const clientSecret = process.env.GOOGLE_CLIENT_SECRET;
      
      if (!clientId || !clientSecret) {
        return res.status(400).json({ 
          message: "Credenciais do Google OAuth não configuradas. Entre em contato com o administrador." 
        });
      }
      
      const { google } = await import('googleapis');
      const baseUrl = process.env.APP_URL || (process.env.REPLIT_DEV_DOMAIN ? 'https://' + process.env.REPLIT_DEV_DOMAIN : 'http://localhost:5000');
      const redirectUri = `${baseUrl}/api/integrations/google-calendar/callback`;
      
      const oauth2Client = new google.auth.OAuth2(clientId, clientSecret, redirectUri);
      
      const state = Buffer.from(JSON.stringify({
        workspaceId: req.params.workspaceId,
        userId: (req.user as any).id,
        service: 'calendar',
      })).toString('base64');
      
      const authUrl = oauth2Client.generateAuthUrl({
        access_type: 'offline',
        scope: [
          'https://www.googleapis.com/auth/calendar',
          'https://www.googleapis.com/auth/calendar.events',
          'https://www.googleapis.com/auth/userinfo.email',
        ],
        state,
        prompt: 'consent',
      });
      
      res.json({ authUrl });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Google Calendar OAuth - Callback
  app.get("/api/integrations/google-calendar/callback", async (req: Request, res: Response) => {
    try {
      const { code, state } = req.query;
      
      if (!code || !state) {
        return res.redirect('/?error=google_calendar_auth_failed');
      }
      
      const clientId = process.env.GOOGLE_CLIENT_ID;
      const clientSecret = process.env.GOOGLE_CLIENT_SECRET;
      
      if (!clientId || !clientSecret) {
        return res.redirect('/?error=google_not_configured');
      }
      
      const stateData = JSON.parse(Buffer.from(state as string, 'base64').toString());
      const { workspaceId } = stateData;
      
      const { google } = await import('googleapis');
      const baseUrl = process.env.APP_URL || (process.env.REPLIT_DEV_DOMAIN ? 'https://' + process.env.REPLIT_DEV_DOMAIN : 'http://localhost:5000');
      const redirectUri = `${baseUrl}/api/integrations/google-calendar/callback`;
      
      const oauth2Client = new google.auth.OAuth2(clientId, clientSecret, redirectUri);
      
      const { tokens } = await oauth2Client.getToken(code as string);
      oauth2Client.setCredentials(tokens);
      
      // Get user email
      const oauth2 = google.oauth2({ version: 'v2', auth: oauth2Client });
      const userInfo = await oauth2.userinfo.get();
      
      await storage.saveWorkspaceGoogleCalendarCredentials({
        workspaceId,
        accessToken: tokens.access_token!,
        refreshToken: tokens.refresh_token || null,
        expiresAt: tokens.expiry_date ? new Date(tokens.expiry_date) : null,
        scopes: tokens.scope || null,
        googleEmail: userInfo.data.email || null,
      });
      
      // Get workspace slug for redirect
      const workspace = await storage.getWorkspaceById(workspaceId);
      const redirectPath = workspace ? `/${workspace.slug}/schedulers?google_calendar_connected=true` : '/?google_calendar_connected=true';
      
      res.redirect(redirectPath);
    } catch (error: any) {
      console.error('Google Calendar OAuth callback error:', error);
      res.redirect('/?error=google_calendar_auth_failed');
    }
  });

  // Google Calendar OAuth - Disconnect
  app.delete("/api/workspaces/:workspaceId/google-calendar/disconnect", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteWorkspaceGoogleCalendarCredentials(req.params.workspaceId);
      res.json({ success: true });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/leads", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { workflowId, stageId, page, pageSize, search, source, status, tagIds, sortBy } = req.query;
      
      // If pagination params provided, use paginated version
      if (page || search || source || status || tagIds || sortBy) {
        const options: any = {};
        if (page) options.page = parseInt(page as string);
        if (pageSize) options.pageSize = parseInt(pageSize as string);
        if (search && typeof search === 'string') options.search = search;
        if (source && typeof source === 'string') options.source = source;
        if (status && typeof status === 'string') options.status = status;
        if (workflowId && typeof workflowId === 'string') options.workflowId = workflowId;
        if (stageId && typeof stageId === 'string') options.stageId = stageId;
        if (sortBy && typeof sortBy === 'string') options.sortBy = sortBy;
        if (tagIds) {
          options.tagIds = typeof tagIds === 'string' ? tagIds.split(',') : tagIds;
        }
        
        const result = await storage.getLeadsByWorkspacePaginated(req.params.workspaceId, options);
        
        // Enrich leads with tags
        const leadsWithTags = await Promise.all(result.items.map(async (lead) => {
          const tags = await storage.getLeadTagAssignments(lead.id);
          return { ...lead, tags };
        }));
        
        res.json({ ...result, items: leadsWithTags });
      } else {
        // Legacy: return all leads without pagination
        const filters: { workflowId?: string; stageId?: string } = {};
        if (workflowId && typeof workflowId === 'string') filters.workflowId = workflowId;
        if (stageId && typeof stageId === 'string') filters.stageId = stageId;
        const leads = await storage.getLeadsByWorkspace(req.params.workspaceId, filters);
        
        // Enrich leads with tags
        const leadsWithTags = await Promise.all(leads.map(async (lead) => {
          const tags = await storage.getLeadTagAssignments(lead.id);
          return { ...lead, tags };
        }));
        
        res.json(leadsWithTags);
      }
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Lead Tags CRUD
  app.get("/api/workspaces/:workspaceId/lead-tags", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const tags = await storage.getLeadTagsByWorkspace(req.params.workspaceId);
      res.json(tags);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/lead-tags", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const tag = await storage.createLeadTag({
        ...req.body,
        workspaceId: req.params.workspaceId,
      });
      res.json(tag);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/lead-tags/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const tag = await storage.updateLeadTag(req.params.id, req.params.workspaceId, req.body);
      if (!tag) {
        return res.status(404).json({ message: "Tag not found" });
      }
      res.json(tag);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/lead-tags/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteLeadTag(req.params.id, req.params.workspaceId);
      res.json({ success: true });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Lead Tag Assignments
  app.post("/api/workspaces/:workspaceId/leads/:id/tags", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { tagIds } = req.body;
      if (!Array.isArray(tagIds)) {
        return res.status(400).json({ message: "tagIds must be an array" });
      }
      await storage.setLeadTags(req.params.id, tagIds);
      const tags = await storage.getLeadTagAssignments(req.params.id);
      res.json({ tags });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/leads/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const lead = await storage.getLeadById(req.params.id, req.params.workspaceId);
      if (!lead) {
        return res.status(404).json({ message: "Lead not found" });
      }
      const history = await storage.getLeadStageHistory(lead.id);
      const submissions = await storage.getLeadFormSubmissions(lead.id);
      res.json({ ...lead, history, submissions });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/leads/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const lead = await storage.updateLead(req.params.id, req.params.workspaceId, req.body);
      if (!lead) {
        return res.status(404).json({ message: "Lead not found" });
      }
      res.json(lead);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/leads/:id/move-stage", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { toStageId, notes } = req.body;
      if (!toStageId) {
        return res.status(400).json({ message: "toStageId is required" });
      }
      
      const lead = await storage.getLeadById(req.params.id, req.params.workspaceId);
      if (!lead) {
        return res.status(404).json({ message: "Lead not found" });
      }
      
      const fromStageId = lead.stageId;
      
      await storage.createLeadStageHistory({
        leadId: lead.id,
        fromStageId,
        toStageId,
        changedBy: req.user!.id,
        notes,
      });
      
      const updatedLead = await storage.updateLead(req.params.id, req.params.workspaceId, { stageId: toStageId });
      res.json(updatedLead);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/leads/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteLead(req.params.id, req.params.workspaceId);
      res.json({ message: "Lead deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ LEAD CLEANUP ROUTES ============

  app.get("/api/workspaces/:workspaceId/leads-cleanup/without-contact", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const leads = await storage.getLeadsWithoutContact(req.params.workspaceId);
      res.json(leads);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/leads-cleanup/duplicates", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const duplicates = await storage.getDuplicateLeads(req.params.workspaceId);
      res.json(duplicates);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/leads-cleanup/delete-batch", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { leadIds } = req.body;
      if (!Array.isArray(leadIds) || leadIds.length === 0) {
        return res.status(400).json({ message: "leadIds array is required" });
      }
      const deletedCount = await storage.deleteLeadsBatch(req.params.workspaceId, leadIds);
      res.json({ deletedCount });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ PUBLIC LANDING PAGE ROUTES ============

  app.get("/api/public/landing/:slug", async (req: Request, res: Response) => {
    try {
      const landingPage = await storage.getLeadLandingPageBySlug(req.params.slug);
      if (!landingPage || !landingPage.isActive) {
        return res.status(404).json({ message: "Landing page not found" });
      }
      
      let form = null;
      let fields: any[] = [];
      if (landingPage.formId) {
        form = await storage.getLeadFormById(landingPage.formId, landingPage.workspaceId);
        if (form) {
          fields = await storage.getLeadFormFields(form.id);
        }
      }
      
      res.json({ ...landingPage, form: form ? { ...form, fields } : null });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/public/landing/:slug/submit", async (req: Request, res: Response) => {
    try {
      const landingPage = await storage.getLeadLandingPageBySlug(req.params.slug);
      if (!landingPage || !landingPage.isActive) {
        return res.status(404).json({ message: "Landing page not found" });
      }
      
      if (!landingPage.formId) {
        return res.status(400).json({ message: "No form configured for this landing page" });
      }
      
      const form = await storage.getLeadFormById(landingPage.formId, landingPage.workspaceId);
      if (!form || !form.isActive) {
        return res.status(400).json({ message: "Form not available" });
      }
      
      const { name, email, phone, ...customData } = req.body;
      
      let defaultStageId = null;
      if (landingPage.workflowId) {
        const stages = await storage.getLeadWorkflowStages(landingPage.workflowId);
        const defaultStage = stages.find(s => s.isDefault) || stages[0];
        if (defaultStage) {
          defaultStageId = defaultStage.id;
        }
      }
      
      const referralCode = randomBytes(8).toString('hex');
      
      // Calculate queue position
      const existingLeads = await storage.getLeadsByWorkspace(landingPage.workspaceId, 
        landingPage.workflowId ? { workflowId: landingPage.workflowId } : undefined);
      const queuePosition = existingLeads.length + 1;
      
      // Determine source based on referral and form type
      const referredBy = req.body.referredBy || null;
      let source: 'DIRECT' | 'REFERRAL' | 'FORM' = 'DIRECT';
      if (referredBy) {
        source = 'REFERRAL';
      } else if (form.isLeadGenerator) {
        source = 'FORM';
      }
      
      // Build notes from additional custom fields (only if form is lead generator)
      let leadNotes = null;
      if (form.isLeadGenerator && Object.keys(customData).length > 0) {
        leadNotes = Object.entries(customData)
          .filter(([key]) => !['referredBy'].includes(key))
          .map(([key, value]) => `${key}: ${value}`)
          .join('\n');
      }
      
      const lead = await storage.createLead({
        workspaceId: landingPage.workspaceId,
        workflowId: landingPage.workflowId,
        stageId: defaultStageId,
        landingPageId: landingPage.id,
        name,
        email,
        phone,
        customData: JSON.stringify(customData),
        referralCode,
        referredBy,
        queuePosition,
        status: 'NEW',
        source,
        notes: leadNotes,
      });
      
      // Increment referral count for referring lead
      if (referredBy) {
        await storage.incrementLeadReferralCount(referredBy);
      }
      
      await storage.createLeadFormSubmission({
        leadId: lead.id,
        formId: form.id,
        data: JSON.stringify(req.body),
        ipAddress: req.ip || req.headers['x-forwarded-for'] as string || null,
        userAgent: req.headers['user-agent'] || null,
      });
      
      if (defaultStageId) {
        await storage.createLeadStageHistory({
          leadId: lead.id,
          fromStageId: null,
          toStageId: defaultStageId,
          changedBy: null,
          notes: 'Lead created via landing page',
        });
      }
      
      res.status(201).json({ 
        message: form.successMessage || 'Obrigado! Entraremos em contato em breve.',
        referralCode: lead.referralCode,
        queuePosition: lead.queuePosition,
      });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ GLOBAL SEARCH ============

  app.get("/api/workspaces/:workspaceId/search", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const query = (req.query.q as string || '').toLowerCase().trim();
      if (!query || query.length < 2) {
        return res.json([]);
      }

      const workspaceId = req.params.workspaceId;
      const [clients, projects, tasks, proposals, invoices, transactions, contracts] = await Promise.all([
        storage.getClientsByWorkspace(workspaceId),
        storage.getProjectsByWorkspace(workspaceId),
        storage.getTasksByWorkspace(workspaceId),
        storage.getProposalsByWorkspace(workspaceId),
        storage.getInvoicesByWorkspace(workspaceId),
        storage.getTransactionsByWorkspace(workspaceId),
        storage.getContractsByWorkspace(workspaceId),
      ]);

      const results: Array<{
        type: string;
        id: string;
        title: string;
        subtitle: string;
        url: string;
      }> = [];

      clients.forEach(c => {
        const docId = c.cnpj || c.cpf || '';
        if (c.name.toLowerCase().includes(query) || c.email?.toLowerCase().includes(query) || docId.includes(query)) {
          results.push({
            type: 'client',
            id: c.id,
            title: c.name,
            subtitle: c.email || docId || 'Cliente',
            url: `/clients/${c.id}`,
          });
        }
      });

      projects.forEach(p => {
        if (p.title.toLowerCase().includes(query) || p.description?.toLowerCase().includes(query)) {
          results.push({
            type: 'project',
            id: p.id,
            title: p.title,
            subtitle: p.description?.substring(0, 50) || 'Projeto',
            url: `/projects?id=${p.id}`,
          });
        }
      });

      tasks.forEach(t => {
        if (t.title.toLowerCase().includes(query) || t.description?.toLowerCase().includes(query)) {
          results.push({
            type: 'task',
            id: t.id,
            title: t.title,
            subtitle: t.description?.substring(0, 50) || 'Tarefa',
            url: `/tasks?id=${t.id}`,
          });
        }
      });

      proposals.forEach(p => {
        if (p.title.toLowerCase().includes(query) || p.content?.toLowerCase().includes(query)) {
          results.push({
            type: 'proposal',
            id: p.id,
            title: p.title,
            subtitle: 'Proposta',
            url: `/proposals?id=${p.id}`,
          });
        }
      });

      invoices.forEach(i => {
        if (i.number?.toLowerCase().includes(query)) {
          results.push({
            type: 'invoice',
            id: i.id,
            title: `Fatura #${i.number}`,
            subtitle: 'Fatura',
            url: `/invoices?id=${i.id}`,
          });
        }
      });

      transactions.forEach(t => {
        if (t.description.toLowerCase().includes(query)) {
          results.push({
            type: 'transaction',
            id: t.id,
            title: t.description,
            subtitle: `R$ ${parseFloat(t.amount).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}`,
            url: `/financeiro?id=${t.id}`,
          });
        }
      });

      contracts.forEach(c => {
        if (c.title.toLowerCase().includes(query) || c.content?.toLowerCase().includes(query)) {
          results.push({
            type: 'contract',
            id: c.id,
            title: c.title,
            subtitle: 'Contrato',
            url: `/contracts?id=${c.id}`,
          });
        }
      });

      res.json(results.slice(0, 20));
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ DASHBOARD / REPORTS ROUTES ============

  app.get("/api/workspaces/:workspaceId/dashboard/stats", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const [clients, projects, tasks, invoices, transactions] = await Promise.all([
        storage.getClientsByWorkspace(req.params.workspaceId),
        storage.getProjectsByWorkspace(req.params.workspaceId),
        storage.getTasksByWorkspace(req.params.workspaceId),
        storage.getInvoicesByWorkspace(req.params.workspaceId),
        storage.getTransactionsByWorkspace(req.params.workspaceId),
      ]);

      const now = new Date();
      const startOfMonth = new Date(now.getFullYear(), now.getMonth(), 1);
      const endOfMonth = new Date(now.getFullYear(), now.getMonth() + 1, 0);

      const monthlyIncome = transactions
        .filter(t => t.type === 'INCOME' && (t.status === 'RECEIVED' || t.status === 'PAID') && new Date(t.dueDate) >= startOfMonth && new Date(t.dueDate) <= endOfMonth)
        .reduce((sum, t) => sum + parseFloat(t.amount), 0);

      const monthlyExpense = transactions
        .filter(t => t.type === 'EXPENSE' && t.status === 'PAID' && new Date(t.dueDate) >= startOfMonth && new Date(t.dueDate) <= endOfMonth)
        .reduce((sum, t) => sum + parseFloat(t.amount), 0);

      const openInvoices = invoices.filter(i => i.status === 'SENT' || i.status === 'OVERDUE');
      const pendingTasks = tasks.filter(t => !t.isCompleted);
      const activeProjects = projects.filter(p => p.status === 'IN_PROGRESS');

      res.json({
        totalClients: clients.length,
        activeProjects: activeProjects.length,
        pendingTasks: pendingTasks.length,
        openInvoices: openInvoices.length,
        monthlyIncome,
        monthlyExpense,
        balance: monthlyIncome - monthlyExpense,
      });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/dashboard/cashflow", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const transactions = await storage.getTransactionsByWorkspace(req.params.workspaceId);
      
      // Group by month for last 12 months
      const now = new Date();
      const months: { [key: string]: { income: number; expense: number } } = {};
      
      for (let i = 11; i >= 0; i--) {
        const date = new Date(now.getFullYear(), now.getMonth() - i, 1);
        const key = `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}`;
        months[key] = { income: 0, expense: 0 };
      }

      transactions.forEach(t => {
        const date = new Date(t.dueDate);
        const key = `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}`;
        if (months[key]) {
          if (t.type === 'INCOME' && (t.status === 'RECEIVED' || t.status === 'PAID')) {
            months[key].income += parseFloat(t.amount);
          } else if (t.type === 'EXPENSE' && t.status === 'PAID') {
            months[key].expense += parseFloat(t.amount);
          }
        }
      });

      const data = Object.entries(months).map(([month, values]) => ({
        month,
        income: values.income,
        expense: values.expense,
        balance: values.income - values.expense,
      }));

      res.json(data);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/dashboard/cashflow-forecast", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const transactions = await storage.getTransactionsByWorkspace(req.params.workspaceId);
      
      // Calculate current balance from completed transactions
      let currentBalance = 0;
      transactions.forEach(t => {
        if (t.status === 'RECEIVED' || t.status === 'PAID') {
          if (t.type === 'INCOME') {
            currentBalance += parseFloat(t.amount);
          } else if (t.type === 'EXPENSE') {
            currentBalance -= parseFloat(t.amount);
          }
        }
      });
      
      const now = new Date();
      const months: { [key: string]: { income: number; expense: number } } = {};
      
      for (let i = 0; i < 12; i++) {
        const date = new Date(now.getFullYear(), now.getMonth() + i, 1);
        const key = `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}`;
        months[key] = { income: 0, expense: 0 };
      }

      transactions.forEach(t => {
        if (t.status !== 'PENDING' && t.status !== 'OVERDUE') return;
        
        const date = new Date(t.dueDate);
        const key = `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}`;
        if (months[key]) {
          if (t.type === 'INCOME') {
            months[key].income += parseFloat(t.amount);
          } else if (t.type === 'EXPENSE') {
            months[key].expense += parseFloat(t.amount);
          }
        }
      });

      let runningBalance = currentBalance;
      const data = Object.entries(months).map(([month, values]) => {
        runningBalance += values.income - values.expense;
        return {
          month,
          income: values.income,
          expense: values.expense,
          balance: runningBalance,
        };
      });

      res.json(data);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/dashboard/income-by-category", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const [transactions, categories] = await Promise.all([
        storage.getTransactionsByWorkspace(req.params.workspaceId),
        storage.getFinancialCategoriesByWorkspace(req.params.workspaceId),
      ]);

      const incomeTransactions = transactions.filter(t => t.type === 'INCOME' && (t.status === 'RECEIVED' || t.status === 'PAID'));
      
      const byCategory: { [key: string]: { name: string; value: number; color: string } } = {};
      
      incomeTransactions.forEach(t => {
        const category = categories.find(c => c.id === t.categoryId);
        const key = t.categoryId || 'uncategorized';
        if (!byCategory[key]) {
          byCategory[key] = {
            name: category?.name || 'Sem categoria',
            value: 0,
            color: category?.color || '#6366f1',
          };
        }
        byCategory[key].value += parseFloat(t.amount);
      });

      res.json(Object.values(byCategory));
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ AI PROPOSAL ASSISTANT ============

  app.post("/api/workspaces/:workspaceId/ai/proposal-assistant", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { messages, clientInfo, context } = req.body;
      
      if (!messages || !Array.isArray(messages)) {
        return res.status(400).json({ message: "Messages are required" });
      }

      const systemPrompt = `Você é um assistente de vendas especializado em criar propostas comerciais profissionais.
Seu objetivo é ajudar o usuário a criar propostas persuasivas e bem estruturadas.

${clientInfo ? `Informações do cliente:
- Nome: ${clientInfo.name || 'Não informado'}
- Empresa: ${clientInfo.company || 'Não informado'}
- Email: ${clientInfo.email || 'Não informado'}
- Telefone: ${clientInfo.phone || 'Não informado'}` : ''}

${context ? `Contexto adicional: ${context}` : ''}

Você pode ajudar com:
1. Sugerir títulos atrativos para a proposta
2. Escrever descrições de serviços/produtos
3. Criar textos persuasivos para a proposta
4. Sugerir estrutura e organização
5. Revisar e melhorar textos existentes
6. Calcular e sugerir preços (se solicitado)
7. Criar termos e condições
8. Sugerir prazos de entrega

Responda sempre em português brasileiro, de forma profissional mas amigável.
Seja direto e objetivo nas respostas.`;

      const response = await openai.chat.completions.create({
        model: "gpt-4o-mini",
        messages: [
          { role: "system", content: systemPrompt },
          ...messages.map((m: { role: string; content: string }) => ({
            role: m.role as "user" | "assistant",
            content: m.content,
          })),
        ],
        max_tokens: 2048,
        temperature: 0.7,
      });

      const assistantMessage = response.choices[0]?.message?.content || "Desculpe, não consegui processar sua solicitação.";

      res.json({ message: assistantMessage });
    } catch (error: any) {
      console.error("AI Proposal Assistant Error:", error);
      res.status(500).json({ message: error.message || "Erro ao processar solicitação de IA" });
    }
  });

  // AI Generate Proposal Content
  app.post("/api/workspaces/:workspaceId/ai/generate-proposal", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { clientName, projectDescription, services, budget } = req.body;

      const prompt = `Crie uma proposta comercial profissional com as seguintes informações:

Cliente: ${clientName || 'Cliente'}
Descrição do Projeto: ${projectDescription || 'Projeto de desenvolvimento'}
${services ? `Serviços: ${services}` : ''}
${budget ? `Orçamento aproximado: R$ ${budget}` : ''}

Gere uma proposta estruturada com:
1. Título atrativo
2. Introdução (apresentação e entendimento do projeto)
3. Escopo dos serviços
4. Metodologia de trabalho
5. Cronograma sugerido
6. Investimento (se orçamento informado)
7. Termos e condições
8. Conclusão com call-to-action

Formate em HTML simples (pode usar <h2>, <h3>, <p>, <ul>, <li>).
Responda em português brasileiro.`;

      const response = await openai.chat.completions.create({
        model: "gpt-4o-mini",
        messages: [
          { role: "system", content: "Você é um especialista em propostas comerciais. Crie conteúdo profissional e persuasivo em português brasileiro." },
          { role: "user", content: prompt },
        ],
        max_tokens: 4096,
        temperature: 0.7,
      });

      const content = response.choices[0]?.message?.content || "";

      res.json({ content });
    } catch (error: any) {
      console.error("AI Generate Proposal Error:", error);
      res.status(500).json({ message: error.message || "Erro ao gerar proposta" });
    }
  });

  // ============ CUSTOM FIELD ROUTES ============

  // Get all custom field definitions for workspace (optionally filtered by entity type)
  app.get("/api/workspaces/:workspaceId/custom-fields", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { entityType } = req.query;
      const definitions = await storage.getCustomFieldDefinitionsByWorkspace(
        req.params.workspaceId,
        entityType as string | undefined
      );
      res.json(definitions);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Create custom field definition
  app.post("/api/workspaces/:workspaceId/custom-fields", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { fieldKey, label, fieldType, entityType } = req.body;
      
      if (!fieldKey || !label || !fieldType || !entityType) {
        return res.status(400).json({ message: "fieldKey, label, fieldType e entityType são obrigatórios" });
      }
      
      const validEntityTypes = ['CLIENT', 'PROJECT', 'PROPOSAL', 'INVOICE', 'LEAD', 'TASK'];
      const validFieldTypes = ['TEXT', 'TEXTAREA', 'NUMBER', 'CURRENCY', 'DATE', 'CHECKBOX', 'SELECT', 'MULTISELECT', 'URL', 'EMAIL', 'PHONE'];
      
      if (!validEntityTypes.includes(entityType)) {
        return res.status(400).json({ message: "entityType inválido" });
      }
      if (!validFieldTypes.includes(fieldType)) {
        return res.status(400).json({ message: "fieldType inválido" });
      }
      
      const definition = await storage.createCustomFieldDefinition({
        ...req.body,
        workspaceId: req.params.workspaceId,
      });
      res.status(201).json(definition);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Update custom field definition
  app.patch("/api/workspaces/:workspaceId/custom-fields/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const definition = await storage.updateCustomFieldDefinition(
        req.params.id,
        req.params.workspaceId,
        req.body
      );
      if (!definition) {
        return res.status(404).json({ message: "Custom field not found" });
      }
      res.json(definition);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Delete custom field definition
  app.delete("/api/workspaces/:workspaceId/custom-fields/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteCustomFieldDefinition(req.params.id, req.params.workspaceId);
      res.json({ message: "Custom field deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Reorder custom field definitions
  app.post("/api/workspaces/:workspaceId/custom-fields/reorder", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { entityType, definitionIds } = req.body;
      await storage.reorderCustomFieldDefinitions(req.params.workspaceId, entityType, definitionIds);
      res.json({ message: "Custom fields reordered" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get custom field values for an entity
  app.get("/api/workspaces/:workspaceId/custom-field-values/:entityType/:entityId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const values = await storage.getCustomFieldValues(
        req.params.workspaceId,
        req.params.entityType,
        req.params.entityId
      );
      res.json(values);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Save custom field values for an entity
  app.post("/api/workspaces/:workspaceId/custom-field-values/:entityType/:entityId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { values } = req.body;
      await storage.upsertCustomFieldValues(
        req.params.workspaceId,
        req.params.entityType,
        req.params.entityId,
        values
      );
      res.json({ message: "Custom field values saved" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // =============== PROFILE ROUTES ===============

  // Get current user profile
  app.get("/api/profile", requireAuth, async (req: Request, res: Response) => {
    try {
      const user = await storage.getUserById(req.user!.id);
      if (!user) {
        return res.status(404).json({ message: "User not found" });
      }
      const { password, ...safeUser } = user;
      res.json(safeUser);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Update current user profile
  app.patch("/api/profile", requireAuth, async (req: Request, res: Response) => {
    try {
      const { name, email, avatar } = req.body;
      const updates: any = {};
      if (name !== undefined) updates.name = name;
      if (email !== undefined) updates.email = email;
      if (avatar !== undefined) updates.avatar = avatar;

      const user = await storage.updateUser(req.user!.id, updates);
      if (!user) {
        return res.status(404).json({ message: "User not found" });
      }
      const { password, ...safeUser } = user;
      res.json(safeUser);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Change password
  app.post("/api/profile/change-password", requireAuth, async (req: Request, res: Response) => {
    try {
      const { currentPassword, newPassword } = req.body;
      
      const user = await storage.getUserById(req.user!.id);
      if (!user) {
        return res.status(404).json({ message: "User not found" });
      }

      const isMatch = await crypto.compare(currentPassword, user.password);
      if (!isMatch) {
        return res.status(400).json({ message: "Senha atual incorreta" });
      }

      const hashedPassword = await crypto.hash(newPassword);
      await storage.updateUser(req.user!.id, { password: hashedPassword });
      
      res.json({ message: "Senha alterada com sucesso" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // =============== TEAM ROUTES ===============

  // Get workspace team members with user info
  app.get("/api/workspaces/:workspaceId/team", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const members = await storage.getWorkspaceMembersWithUsers(req.params.workspaceId);
      const safeMembers = members.map(m => ({
        ...m,
        user: { id: m.user.id, name: m.user.name, email: m.user.email, avatar: m.user.avatar }
      }));
      res.json(safeMembers);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Update member role
  app.patch("/api/workspaces/:workspaceId/team/:memberId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { memberRole } = req.body;
      const member = await storage.updateWorkspaceMember(req.params.memberId, req.params.workspaceId, { memberRole });
      if (!member) {
        return res.status(404).json({ message: "Member not found" });
      }
      res.json(member);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Remove member from workspace
  app.delete("/api/workspaces/:workspaceId/team/:memberId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.removeWorkspaceMember(req.params.memberId, req.params.workspaceId);
      res.json({ message: "Member removed" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get member permissions
  app.get("/api/workspaces/:workspaceId/team/:memberId/permissions", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const permissions = await storage.getMemberPermissions(req.params.memberId, req.params.workspaceId);
      res.json(permissions);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Set member permission
  app.post("/api/workspaces/:workspaceId/team/:memberId/permissions", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { module, action, granted } = req.body;
      const permission = await storage.setMemberPermission({
        workspaceId: req.params.workspaceId,
        memberId: req.params.memberId,
        module,
        action,
        granted,
      });
      res.json(permission);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Clear all member permissions
  app.delete("/api/workspaces/:workspaceId/team/:memberId/permissions", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.clearMemberPermissions(req.params.memberId, req.params.workspaceId);
      res.json({ message: "Permissions cleared" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Check permission (utility endpoint)
  app.get("/api/workspaces/:workspaceId/check-permission", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { module, action } = req.query;
      const membership = await storage.getUserWorkspaceMembership(req.user!.id, req.params.workspaceId);
      if (!membership) {
        return res.json({ hasPermission: false });
      }
      const hasPermission = await storage.checkPermission(membership.id, req.params.workspaceId, module as any, action as any);
      res.json({ hasPermission });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // =============== INVITE ROUTES ===============

  // Get workspace invites
  app.get("/api/workspaces/:workspaceId/invites", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const invites = await storage.getWorkspaceInvites(req.params.workspaceId);
      res.json(invites);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Create invite
  app.post("/api/workspaces/:workspaceId/invites", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { email, memberRole } = req.body;
      const crypto = await import('crypto');
      const token = crypto.randomBytes(32).toString('hex');
      const expiresAt = new Date();
      expiresAt.setDate(expiresAt.getDate() + 7); // Expires in 7 days

      const invite = await storage.createWorkspaceInvite({
        workspaceId: req.params.workspaceId,
        email,
        memberRole: memberRole || 'MEMBRO',
        token,
        invitedBy: req.user!.id,
        status: 'PENDING',
        expiresAt,
      });
      res.status(201).json(invite);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Cancel invite
  app.delete("/api/workspaces/:workspaceId/invites/:inviteId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteWorkspaceInvite(req.params.inviteId, req.params.workspaceId);
      res.json({ message: "Invite cancelled" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Accept invite (public route - token based)
  app.post("/api/invites/:token/accept", async (req: Request, res: Response) => {
    try {
      const invite = await storage.getInviteByToken(req.params.token);
      if (!invite) {
        return res.status(404).json({ message: "Convite não encontrado" });
      }
      if (invite.status !== 'PENDING') {
        return res.status(400).json({ message: "Convite já foi usado ou expirou" });
      }
      if (new Date() > invite.expiresAt) {
        await storage.updateInviteStatus(invite.id, 'EXPIRED');
        return res.status(400).json({ message: "Convite expirado" });
      }

      // Check if user exists
      let user = await storage.getUserByEmail(invite.email);
      
      if (!user) {
        // User needs to register first
        return res.status(400).json({ 
          message: "Usuário não encontrado. Por favor, cadastre-se primeiro.",
          needsRegistration: true,
          email: invite.email
        });
      }

      // Check if already a member
      const existingMember = await storage.getUserWorkspaceMembership(user.id, invite.workspaceId);
      if (existingMember) {
        await storage.updateInviteStatus(invite.id, 'ACCEPTED');
        return res.status(400).json({ message: "Você já é membro deste workspace" });
      }

      // Add user to workspace
      await storage.addWorkspaceMember({
        workspaceId: invite.workspaceId,
        userId: user.id,
        role: 'MEMBER',
        memberRole: invite.memberRole,
      });

      await storage.updateInviteStatus(invite.id, 'ACCEPTED');
      
      const workspace = await storage.getWorkspaceById(invite.workspaceId);
      res.json({ message: "Convite aceito com sucesso!", workspace });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get invite details (public route)
  app.get("/api/invites/:token", async (req: Request, res: Response) => {
    try {
      const invite = await storage.getInviteByToken(req.params.token);
      if (!invite) {
        return res.status(404).json({ message: "Convite não encontrado" });
      }
      
      const workspace = await storage.getWorkspaceById(invite.workspaceId);
      res.json({
        email: invite.email,
        memberRole: invite.memberRole,
        status: invite.status,
        expiresAt: invite.expiresAt,
        workspace: workspace ? { name: workspace.name, slug: workspace.slug } : null,
      });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ SCHEDULER ROUTES ============

  // Scheduler Types CRUD
  app.get("/api/workspaces/:workspaceId/schedulers", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const schedulers = await storage.getSchedulerTypesByWorkspace(req.params.workspaceId);
      res.json(schedulers);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/schedulers", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { name, description, durationMinutes, color, bufferBeforeMinutes, bufferAfterMinutes, createMeetLink } = req.body;
      
      const slug = name
        .toLowerCase()
        .normalize('NFD')
        .replace(/[\u0300-\u036f]/g, '')
        .replace(/[^a-z0-9]+/g, '-')
        .replace(/^-|-$/g, '');

      const existing = await storage.getSchedulerTypeBySlug(slug, req.params.workspaceId);
      const finalSlug = existing ? `${slug}-${Date.now()}` : slug;

      const scheduler = await storage.createSchedulerType({
        workspaceId: req.params.workspaceId,
        name,
        slug: finalSlug,
        description,
        durationMinutes: parseInt(durationMinutes),
        color: color || '#2563eb',
        bufferBeforeMinutes: bufferBeforeMinutes ? parseInt(bufferBeforeMinutes) : 0,
        bufferAfterMinutes: bufferAfterMinutes ? parseInt(bufferAfterMinutes) : 0,
        createMeetLink: createMeetLink !== false,
        isActive: true,
      });

      res.status(201).json(scheduler);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/schedulers/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const scheduler = await storage.getSchedulerTypeById(req.params.id, req.params.workspaceId);
      if (!scheduler) {
        return res.status(404).json({ message: "Tipo de agendamento não encontrado" });
      }
      
      const [availability, exceptions, bookings] = await Promise.all([
        storage.getSchedulerAvailability(scheduler.id),
        storage.getSchedulerExceptions(scheduler.id),
        storage.getSchedulerBookingsBySchedulerType(scheduler.id),
      ]);

      res.json({ ...scheduler, availability, exceptions, bookings });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/schedulers/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const scheduler = await storage.updateSchedulerType(req.params.id, req.params.workspaceId, req.body);
      if (!scheduler) {
        return res.status(404).json({ message: "Tipo de agendamento não encontrado" });
      }
      res.json(scheduler);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/schedulers/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteSchedulerType(req.params.id, req.params.workspaceId);
      res.status(204).send();
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Scheduler Availability
  app.put("/api/workspaces/:workspaceId/schedulers/:id/availability", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { availability } = req.body;
      const scheduler = await storage.getSchedulerTypeById(req.params.id, req.params.workspaceId);
      if (!scheduler) {
        return res.status(404).json({ message: "Tipo de agendamento não encontrado" });
      }

      const availabilityData = availability.map((a: any) => ({
        workspaceId: req.params.workspaceId,
        schedulerTypeId: req.params.id,
        dayOfWeek: a.dayOfWeek,
        startTime: a.startTime,
        endTime: a.endTime,
      }));

      const result = await storage.replaceSchedulerAvailability(req.params.id, availabilityData);
      res.json(result);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Scheduler Exceptions
  app.post("/api/workspaces/:workspaceId/schedulers/:id/exceptions", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { date, isBlocked, startTime, endTime, reason } = req.body;
      const exception = await storage.createSchedulerException({
        schedulerTypeId: req.params.id,
        date,
        isBlocked: isBlocked !== false,
        startTime,
        endTime,
        reason,
      });
      res.status(201).json(exception);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/schedulers/:id/exceptions/:exceptionId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteSchedulerException(req.params.exceptionId);
      res.status(204).send();
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Scheduler Bookings (admin)
  app.get("/api/workspaces/:workspaceId/bookings", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const bookings = await storage.getSchedulerBookingsByWorkspace(req.params.workspaceId);
      res.json(bookings);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/bookings/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const booking = await storage.updateSchedulerBooking(req.params.id, req.params.workspaceId, req.body);
      if (!booking) {
        return res.status(404).json({ message: "Agendamento não encontrado" });
      }
      res.json(booking);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/bookings/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const booking = await storage.getSchedulerBookingById(req.params.id, req.params.workspaceId);
      if (booking?.calendarEventId) {
        const { deleteCalendarEvent } = await import('./google-calendar');
        await deleteCalendarEvent(booking.calendarEventId);
      }
      await storage.deleteSchedulerBooking(req.params.id, req.params.workspaceId);
      res.status(204).send();
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ PUBLIC SCHEDULER ROUTES ============

  // Get scheduler info by workspace slug and scheduler slug (public)
  app.get("/api/public/schedule/:workspaceSlug/:schedulerSlug", async (req: Request, res: Response) => {
    try {
      const workspace = await storage.getWorkspaceBySlug(req.params.workspaceSlug);
      if (!workspace) {
        return res.status(404).json({ message: "Workspace não encontrado" });
      }

      const scheduler = await storage.getSchedulerTypeBySlug(req.params.schedulerSlug, workspace.id);
      if (!scheduler || !scheduler.isActive) {
        return res.status(404).json({ message: "Agendamento não encontrado" });
      }

      const availability = await storage.getSchedulerAvailability(scheduler.id);

      res.json({
        id: scheduler.id,
        name: scheduler.name,
        description: scheduler.description,
        durationMinutes: scheduler.durationMinutes,
        color: scheduler.color,
        workspaceName: workspace.name,
        availability,
      });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get available slots for a specific date (public)
  app.get("/api/public/schedule/:workspaceSlug/:schedulerSlug/slots", async (req: Request, res: Response) => {
    try {
      const { date } = req.query;
      if (!date) {
        return res.status(400).json({ message: "Data é obrigatória" });
      }

      const workspace = await storage.getWorkspaceBySlug(req.params.workspaceSlug);
      if (!workspace) {
        return res.status(404).json({ message: "Workspace não encontrado" });
      }

      const scheduler = await storage.getSchedulerTypeBySlug(req.params.schedulerSlug, workspace.id);
      if (!scheduler || !scheduler.isActive) {
        return res.status(404).json({ message: "Agendamento não encontrado" });
      }

      const targetDate = new Date(date as string);
      const dayOfWeek = targetDate.getDay();

      const [availability, exceptions, existingBookings] = await Promise.all([
        storage.getSchedulerAvailability(scheduler.id),
        storage.getSchedulerExceptions(scheduler.id),
        storage.getSchedulerBookingsByDateRange(
          scheduler.id,
          new Date(targetDate.setHours(0, 0, 0, 0)),
          new Date(targetDate.setHours(23, 59, 59, 999))
        ),
      ]);

      const dateStr = (date as string).split('T')[0];
      const exception = exceptions.find(e => e.date === dateStr);
      if (exception?.isBlocked) {
        return res.json({ slots: [], blockedReason: exception.reason });
      }

      const dayAvailability = availability.filter(a => a.dayOfWeek === dayOfWeek);
      if (dayAvailability.length === 0) {
        return res.json({ slots: [] });
      }

      let googleBusySlots: Array<{start: Date, end: Date}> = [];
      try {
        const { getFreeBusySlots, isCalendarConnected } = await import('./google-calendar');
        if (await isCalendarConnected()) {
          const startOfDay = new Date(date as string);
          startOfDay.setHours(0, 0, 0, 0);
          const endOfDay = new Date(date as string);
          endOfDay.setHours(23, 59, 59, 999);
          googleBusySlots = await getFreeBusySlots(startOfDay, endOfDay);
        }
      } catch (e) {
        console.log('Google Calendar not connected:', e);
      }

      const slots: string[] = [];
      const duration = scheduler.durationMinutes;
      const bufferBefore = scheduler.bufferBeforeMinutes || 0;
      const bufferAfter = scheduler.bufferAfterMinutes || 0;

      for (const avail of dayAvailability) {
        const [startHour, startMin] = avail.startTime.split(':').map(Number);
        const [endHour, endMin] = avail.endTime.split(':').map(Number);
        
        let currentTime = new Date(date as string);
        currentTime.setHours(startHour, startMin, 0, 0);
        
        const endTime = new Date(date as string);
        endTime.setHours(endHour, endMin, 0, 0);

        while (currentTime.getTime() + duration * 60000 <= endTime.getTime()) {
          const slotStart = new Date(currentTime);
          const slotEnd = new Date(currentTime.getTime() + duration * 60000);
          
          const hasConflict = existingBookings.some(b => {
            const bookingStart = new Date(b.startTime);
            const bookingEnd = new Date(b.endTime);
            const bufferStart = new Date(bookingStart.getTime() - bufferBefore * 60000);
            const bufferEnd = new Date(bookingEnd.getTime() + bufferAfter * 60000);
            return slotStart < bufferEnd && slotEnd > bufferStart;
          });

          const hasGoogleConflict = googleBusySlots.some(busy => {
            return slotStart < busy.end && slotEnd > busy.start;
          });

          if (!hasConflict && !hasGoogleConflict && slotStart > new Date()) {
            slots.push(slotStart.toISOString());
          }

          currentTime = new Date(currentTime.getTime() + 30 * 60000);
        }
      }

      res.json({ slots });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Create booking (public)
  app.post("/api/public/schedule/:workspaceSlug/:schedulerSlug/book", async (req: Request, res: Response) => {
    try {
      const { clientName, clientEmail, clientPhone, notes, startTime } = req.body;

      if (!clientName || !clientEmail || !startTime) {
        return res.status(400).json({ message: "Nome, email e horário são obrigatórios" });
      }

      const workspace = await storage.getWorkspaceBySlug(req.params.workspaceSlug);
      if (!workspace) {
        return res.status(404).json({ message: "Workspace não encontrado" });
      }

      const scheduler = await storage.getSchedulerTypeBySlug(req.params.schedulerSlug, workspace.id);
      if (!scheduler || !scheduler.isActive) {
        return res.status(404).json({ message: "Agendamento não encontrado" });
      }

      const startDate = new Date(startTime);
      const endDate = new Date(startDate.getTime() + scheduler.durationMinutes * 60000);

      const existingBookings = await storage.getSchedulerBookingsByDateRange(
        scheduler.id,
        new Date(startDate.getTime() - 60 * 60000),
        new Date(endDate.getTime() + 60 * 60000)
      );

      const hasConflict = existingBookings.some(b => {
        const bookingStart = new Date(b.startTime);
        const bookingEnd = new Date(b.endTime);
        return startDate < bookingEnd && endDate > bookingStart;
      });

      if (hasConflict) {
        return res.status(409).json({ message: "Horário não disponível" });
      }

      let calendarEventId: string | undefined;
      let meetingLink: string | undefined;

      if (scheduler.createMeetLink) {
        try {
          const { createCalendarEvent, isCalendarConnected } = await import('./google-calendar');
          if (await isCalendarConnected()) {
            const result = await createCalendarEvent({
              summary: `${scheduler.name} - ${clientName}`,
              description: notes ? `Notas: ${notes}\nEmail: ${clientEmail}\nTelefone: ${clientPhone || 'Não informado'}` : `Email: ${clientEmail}`,
              startTime: startDate,
              endTime: endDate,
              attendeeEmail: clientEmail,
              createMeetLink: true,
            });
            if (result) {
              calendarEventId = result.eventId;
              meetingLink = result.meetLink;
            }
          }
        } catch (e) {
          console.log('Could not create calendar event:', e);
        }
      }

      const booking = await storage.createSchedulerBooking({
        workspaceId: workspace.id,
        schedulerTypeId: scheduler.id,
        clientName,
        clientEmail,
        clientPhone,
        notes,
        startTime: startDate,
        endTime: endDate,
        status: 'CONFIRMED',
        calendarEventId,
        meetingLink,
      });

      try {
        await storage.createLead({
          workspaceId: workspace.id,
          name: clientName,
          email: clientEmail,
          phone: clientPhone,
          source: 'BOOKING',
          status: 'NEW',
        });
      } catch (e) {
        console.log('Could not create lead from booking:', e);
      }

      res.status(201).json({
        ...booking,
        schedulerName: scheduler.name,
        workspaceName: workspace.name,
      });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ SITE BUILDER ============

  // Site Projects
  app.get("/api/workspaces/:workspaceId/sites", requireAuth, requireWorkspace, async (req, res) => {
    try {
      const sites = await storage.getSiteProjectsByWorkspace(req.params.workspaceId);
      res.json(sites);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/workspaces/:workspaceId/sites", requireAuth, requireWorkspace, async (req, res) => {
    try {
      const site = await storage.createSiteProject({ ...req.body, workspaceId: req.params.workspaceId });
      res.status(201).json(site);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/workspaces/:workspaceId/sites/:siteId", requireAuth, requireWorkspace, async (req, res) => {
    try {
      const site = await storage.getSiteProjectById(req.params.siteId, req.params.workspaceId);
      if (!site) return res.status(404).json({ message: "Site not found" });
      res.json(site);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/workspaces/:workspaceId/sites/:siteId", requireAuth, requireWorkspace, async (req, res) => {
    try {
      const site = await storage.updateSiteProject(req.params.siteId, req.params.workspaceId, req.body);
      if (!site) return res.status(404).json({ message: "Site not found" });
      res.json(site);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/workspaces/:workspaceId/sites/:siteId", requireAuth, requireWorkspace, async (req, res) => {
    try {
      await storage.deleteSiteProject(req.params.siteId, req.params.workspaceId);
      res.status(204).send();
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Site Pages
  app.get("/api/sites/:siteId/pages", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const pages = await storage.getSitePagesBySite(req.params.siteId);
      res.json(pages);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/sites/:siteId/pages", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const page = await storage.createSitePage({ ...req.body, siteId: req.params.siteId });
      res.status(201).json(page);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/sites/:siteId/pages/:pageId", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const page = await storage.getSitePageById(req.params.pageId);
      if (!page || page.siteId !== req.params.siteId) {
        return res.status(404).json({ message: "Page not found" });
      }
      res.json(page);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/sites/:siteId/pages/:pageId", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const existingPage = await storage.getSitePageById(req.params.pageId);
      if (!existingPage || existingPage.siteId !== req.params.siteId) {
        return res.status(404).json({ message: "Page not found" });
      }
      const page = await storage.updateSitePage(req.params.pageId, req.body);
      res.json(page);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/sites/:siteId/pages/:pageId", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const existingPage = await storage.getSitePageById(req.params.pageId);
      if (!existingPage || existingPage.siteId !== req.params.siteId) {
        return res.status(404).json({ message: "Page not found" });
      }
      await storage.deleteSitePage(req.params.pageId);
      res.status(204).send();
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Site Page Versions
  app.get("/api/sites/:siteId/pages/:pageId/versions", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const page = await storage.getSitePageById(req.params.pageId);
      if (!page || page.siteId !== req.params.siteId) {
        return res.status(404).json({ message: "Page not found" });
      }
      const versions = await storage.getSitePageVersions(req.params.pageId);
      res.json(versions);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/sites/:siteId/pages/:pageId/versions", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const page = await storage.getSitePageById(req.params.pageId);
      if (!page || page.siteId !== req.params.siteId) {
        return res.status(404).json({ message: "Page not found" });
      }
      const version = await storage.createSitePageVersion({ ...req.body, pageId: req.params.pageId });
      res.status(201).json(version);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Blog Categories
  app.get("/api/sites/:siteId/blog/categories", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const categories = await storage.getBlogCategoriesBySite(req.params.siteId);
      res.json(categories);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/sites/:siteId/blog/categories", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const category = await storage.createBlogCategory({ ...req.body, siteId: req.params.siteId });
      res.status(201).json(category);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/sites/:siteId/blog/categories/:categoryId", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const existingCategory = await storage.getBlogCategoryById(req.params.categoryId);
      if (!existingCategory || existingCategory.siteId !== req.params.siteId) {
        return res.status(404).json({ message: "Category not found" });
      }
      const category = await storage.updateBlogCategory(req.params.categoryId, req.body);
      res.json(category);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/sites/:siteId/blog/categories/:categoryId", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const existingCategory = await storage.getBlogCategoryById(req.params.categoryId);
      if (!existingCategory || existingCategory.siteId !== req.params.siteId) {
        return res.status(404).json({ message: "Category not found" });
      }
      await storage.deleteBlogCategory(req.params.categoryId);
      res.status(204).send();
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Blog Posts
  app.get("/api/sites/:siteId/blog/posts", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const posts = await storage.getBlogPostsBySite(req.params.siteId);
      res.json(posts);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/sites/:siteId/blog/posts", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const post = await storage.createBlogPost({ ...req.body, siteId: req.params.siteId });
      res.status(201).json(post);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/sites/:siteId/blog/posts/:postId", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const post = await storage.getBlogPostById(req.params.postId);
      if (!post || post.siteId !== req.params.siteId) {
        return res.status(404).json({ message: "Post not found" });
      }
      res.json(post);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.patch("/api/sites/:siteId/blog/posts/:postId", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const existingPost = await storage.getBlogPostById(req.params.postId);
      if (!existingPost || existingPost.siteId !== req.params.siteId) {
        return res.status(404).json({ message: "Post not found" });
      }
      const post = await storage.updateBlogPost(req.params.postId, req.body);
      res.json(post);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.delete("/api/sites/:siteId/blog/posts/:postId", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const existingPost = await storage.getBlogPostById(req.params.postId);
      if (!existingPost || existingPost.siteId !== req.params.siteId) {
        return res.status(404).json({ message: "Post not found" });
      }
      await storage.deleteBlogPost(req.params.postId);
      res.status(204).send();
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Blog Post Versions
  app.get("/api/sites/:siteId/blog/posts/:postId/versions", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const post = await storage.getBlogPostById(req.params.postId);
      if (!post || post.siteId !== req.params.siteId) {
        return res.status(404).json({ message: "Post not found" });
      }
      const versions = await storage.getBlogPostVersions(req.params.postId);
      res.json(versions);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.post("/api/sites/:siteId/blog/posts/:postId/versions", requireAuth, validateSiteAccess, async (req, res) => {
    try {
      const post = await storage.getBlogPostById(req.params.postId);
      if (!post || post.siteId !== req.params.siteId) {
        return res.status(404).json({ message: "Post not found" });
      }
      const version = await storage.createBlogPostVersion({ ...req.body, postId: req.params.postId });
      res.status(201).json(version);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ PUBLIC SITE ROUTES ============

  // Public site page view (with preview mode for DRAFT sites)
  app.get("/api/public/site/:workspaceSlug/:siteSlug", async (req, res) => {
    try {
      const workspace = await storage.getWorkspaceBySlug(req.params.workspaceSlug);
      if (!workspace) return res.status(404).json({ message: "Workspace not found" });
      
      const site = await storage.getSiteProjectBySlug(workspace.id, req.params.siteSlug);
      if (!site) return res.status(404).json({ message: "Site not found" });
      
      const isPreview = req.query.preview === 'true' || site.status === 'DRAFT';
      
      if (site.status === 'ARCHIVED') {
        return res.status(404).json({ message: "Site not found" });
      }
      
      const pages = await storage.getSitePagesBySite(site.id);
      const homePage = pages.find(p => p.isHomePage);
      
      if (isPreview) {
        res.json({ site, page: homePage, pages: pages.filter(p => p.showInNavigation), preview: true });
      } else {
        res.json({ 
          site, 
          page: pages.find(p => p.isHomePage && p.status === 'PUBLISHED'), 
          pages: pages.filter(p => p.status === 'PUBLISHED' && p.showInNavigation) 
        });
      }
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/public/site/:workspaceSlug/:siteSlug/:pageSlug", async (req, res) => {
    try {
      const workspace = await storage.getWorkspaceBySlug(req.params.workspaceSlug);
      if (!workspace) return res.status(404).json({ message: "Workspace not found" });
      
      const site = await storage.getSiteProjectBySlug(workspace.id, req.params.siteSlug);
      if (!site || site.status !== 'PUBLISHED') return res.status(404).json({ message: "Site not found" });
      
      const page = await storage.getSitePageBySlug(site.id, req.params.pageSlug);
      if (!page || page.status !== 'PUBLISHED') return res.status(404).json({ message: "Page not found" });
      
      const pages = await storage.getSitePagesBySite(site.id);
      res.json({ site, page, pages: pages.filter(p => p.status === 'PUBLISHED' && p.showInNavigation) });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Public blog routes
  app.get("/api/public/site/:workspaceSlug/:siteSlug/blog", async (req, res) => {
    try {
      const workspace = await storage.getWorkspaceBySlug(req.params.workspaceSlug);
      if (!workspace) return res.status(404).json({ message: "Workspace not found" });
      
      const site = await storage.getSiteProjectBySlug(workspace.id, req.params.siteSlug);
      if (!site || site.status !== 'PUBLISHED') return res.status(404).json({ message: "Site not found" });
      
      const posts = await storage.getBlogPostsBySite(site.id);
      const categories = await storage.getBlogCategoriesBySite(site.id);
      
      res.json({ 
        site, 
        posts: posts.filter(p => p.status === 'PUBLISHED'),
        categories 
      });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  app.get("/api/public/site/:workspaceSlug/:siteSlug/blog/:postSlug", async (req, res) => {
    try {
      const workspace = await storage.getWorkspaceBySlug(req.params.workspaceSlug);
      if (!workspace) return res.status(404).json({ message: "Workspace not found" });
      
      const site = await storage.getSiteProjectBySlug(workspace.id, req.params.siteSlug);
      if (!site || site.status !== 'PUBLISHED') return res.status(404).json({ message: "Site not found" });
      
      const post = await storage.getBlogPostBySlug(site.id, req.params.postSlug);
      if (!post || post.status !== 'PUBLISHED') return res.status(404).json({ message: "Post not found" });
      
      await storage.incrementBlogPostViewCount(post.id);
      
      res.json({ site, post });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Public form endpoint for embedded forms
  app.get("/api/public/forms/:formId", async (req, res) => {
    try {
      const form = await storage.getLeadFormById(req.params.formId, '');
      if (!form || !form.isActive) return res.status(404).json({ message: "Form not found" });
      
      const fields = await storage.getLeadFormFields(form.id);
      res.json({ form, fields });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Public form submission
  app.post("/api/public/forms/:formId/submit", async (req, res) => {
    try {
      const form = await storage.getLeadFormById(req.params.formId, '');
      if (!form || !form.isActive) return res.status(404).json({ message: "Form not found" });
      
      // Create or find lead
      const leadData = {
        workspaceId: form.workspaceId,
        name: req.body.name || req.body.email?.split('@')[0] || 'Novo Lead',
        email: req.body.email,
        phone: req.body.phone,
        source: 'FORM' as const,
        status: 'NEW' as const,
        formId: form.id,
      };
      
      const lead = await storage.createLead(leadData);
      
      // Save form submission
      await storage.createLeadFormSubmission({
        formId: form.id,
        leadId: lead.id,
        data: req.body,
      });
      
      res.json({ success: true, message: form.successMessage || 'Enviado com sucesso!' });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Embeddable form script - serves JavaScript that renders the form on external sites
  app.get("/api/embed/forms/:formId/script.js", async (req, res) => {
    try {
      const form = await storage.getLeadFormById(req.params.formId, '');
      if (!form || !form.isActive) {
        res.setHeader('Content-Type', 'application/javascript');
        return res.send('console.error("Dominus Form: Form not found or inactive");');
      }
      
      // HTML escape function to prevent XSS
      const escapeHtml = (str: string | null | undefined): string => {
        if (!str) return '';
        return str
          .replace(/&/g, '&amp;')
          .replace(/</g, '&lt;')
          .replace(/>/g, '&gt;')
          .replace(/"/g, '&quot;')
          .replace(/'/g, '&#39;');
      };
      
      // Parse options - handle both JSON array and newline-separated string
      const parseOptions = (options: string | null | undefined): string[] => {
        if (!options) return [];
        try {
          const parsed = JSON.parse(options);
          if (Array.isArray(parsed)) return parsed.map(o => String(o));
        } catch {}
        // Fallback: split by newlines
        return options.split('\n').map(o => o.trim()).filter(o => o.length > 0);
      };
      
      const fields = await storage.getLeadFormFields(form.id);
      const formConfig = JSON.stringify({
        id: form.id,
        name: escapeHtml(form.name),
        description: escapeHtml(form.description),
        submitButtonText: escapeHtml(form.submitButtonText) || 'Enviar',
        successMessage: escapeHtml(form.successMessage) || 'Obrigado! Entraremos em contato em breve.',
        fields: fields.map(f => ({
          id: f.id,
          type: f.type,
          label: escapeHtml(f.label),
          placeholder: escapeHtml(f.placeholder),
          required: f.required,
          options: parseOptions(f.options).map(o => escapeHtml(o)),
          position: f.position,
        })).sort((a, b) => a.position - b.position),
      });
      
      const baseUrl = `${req.protocol}://${req.get('host')}`;
      
      const script = `
(function() {
  var formConfig = ${formConfig};
  var baseUrl = "${baseUrl}";
  
  function renderDominusForm(containerId, options) {
    options = options || {};
    var container = document.getElementById(containerId);
    if (!container) {
      console.error('Dominus Form: Container #' + containerId + ' not found');
      return;
    }
    
    var theme = options.theme || 'light';
    var primaryColor = options.primaryColor || '#6366f1';
    
    var formHtml = '<form id="dominus-form-' + formConfig.id + '" class="dominus-form" style="' +
      'font-family: system-ui, -apple-system, sans-serif;' +
      'max-width: 100%;' +
      '">';
    
    if (formConfig.description && options.showDescription !== false) {
      formHtml += '<p style="color: #6b7280; margin-bottom: 16px; font-size: 14px;">' + formConfig.description + '</p>';
    }
    
    formConfig.fields.forEach(function(field) {
      var inputId = 'dominus-field-' + field.id;
      var requiredMark = field.required ? ' <span style="color: #ef4444;">*</span>' : '';
      
      formHtml += '<div style="margin-bottom: 16px;">';
      formHtml += '<label for="' + inputId + '" style="display: block; margin-bottom: 6px; font-size: 14px; font-weight: 500; color: ' + (theme === 'dark' ? '#e5e7eb' : '#374151') + ';">' + field.label + requiredMark + '</label>';
      
      var inputStyle = 'width: 100%; padding: 10px 12px; border: 1px solid ' + (theme === 'dark' ? '#374151' : '#d1d5db') + '; border-radius: 6px; font-size: 14px; box-sizing: border-box; background: ' + (theme === 'dark' ? '#1f2937' : '#fff') + '; color: ' + (theme === 'dark' ? '#e5e7eb' : '#111827') + ';';
      
      if (field.type === 'TEXTAREA') {
        formHtml += '<textarea id="' + inputId + '" name="' + field.label.toLowerCase().replace(/\\s+/g, '_') + '" placeholder="' + (field.placeholder || '') + '" ' + (field.required ? 'required' : '') + ' style="' + inputStyle + ' min-height: 100px; resize: vertical;"></textarea>';
      } else if (field.type === 'SELECT') {
        formHtml += '<select id="' + inputId + '" name="' + field.label.toLowerCase().replace(/\\s+/g, '_') + '" ' + (field.required ? 'required' : '') + ' style="' + inputStyle + '">';
        formHtml += '<option value="">' + (field.placeholder || 'Selecione...') + '</option>';
        if (field.options && Array.isArray(field.options)) {
          field.options.forEach(function(opt) {
            formHtml += '<option value="' + opt + '">' + opt + '</option>';
          });
        }
        formHtml += '</select>';
      } else if (field.type === 'CHECKBOX') {
        formHtml += '<input type="checkbox" id="' + inputId + '" name="' + field.label.toLowerCase().replace(/\\s+/g, '_') + '" ' + (field.required ? 'required' : '') + ' style="width: auto; margin-right: 8px;">';
      } else {
        var inputType = 'text';
        if (field.type === 'EMAIL') inputType = 'email';
        else if (field.type === 'PHONE') inputType = 'tel';
        else if (field.type === 'NUMBER') inputType = 'number';
        else if (field.type === 'DATE') inputType = 'date';
        else if (field.type === 'URL') inputType = 'url';
        formHtml += '<input type="' + inputType + '" id="' + inputId + '" name="' + field.label.toLowerCase().replace(/\\s+/g, '_') + '" placeholder="' + (field.placeholder || '') + '" ' + (field.required ? 'required' : '') + ' style="' + inputStyle + '">';
      }
      formHtml += '</div>';
    });
    
    formHtml += '<button type="submit" style="' +
      'width: 100%;' +
      'padding: 12px 24px;' +
      'background: ' + primaryColor + ';' +
      'color: white;' +
      'border: none;' +
      'border-radius: 6px;' +
      'font-size: 16px;' +
      'font-weight: 600;' +
      'cursor: pointer;' +
      'transition: opacity 0.2s;' +
      '">' + formConfig.submitButtonText + '</button>';
    
    formHtml += '</form>';
    formHtml += '<div id="dominus-form-message-' + formConfig.id + '" style="display: none; padding: 16px; border-radius: 6px; margin-top: 16px; text-align: center;"></div>';
    
    container.innerHTML = formHtml;
    
    var formEl = document.getElementById('dominus-form-' + formConfig.id);
    var messageEl = document.getElementById('dominus-form-message-' + formConfig.id);
    
    formEl.addEventListener('submit', function(e) {
      e.preventDefault();
      var submitBtn = formEl.querySelector('button[type="submit"]');
      submitBtn.disabled = true;
      submitBtn.textContent = 'Enviando...';
      
      var formData = {};
      formConfig.fields.forEach(function(field) {
        var input = document.getElementById('dominus-field-' + field.id);
        if (input) {
          if (field.type === 'CHECKBOX') {
            formData[field.label.toLowerCase().replace(/\\s+/g, '_')] = input.checked;
          } else {
            formData[field.label.toLowerCase().replace(/\\s+/g, '_')] = input.value;
          }
          if (field.type === 'EMAIL') formData.email = input.value;
          if (field.label.toLowerCase().includes('nome') || field.label.toLowerCase().includes('name')) formData.name = input.value;
          if (field.type === 'PHONE') formData.phone = input.value;
        }
      });
      
      fetch(baseUrl + '/api/public/forms/' + formConfig.id + '/submit', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData)
      })
      .then(function(response) { return response.json(); })
      .then(function(data) {
        if (data.success) {
          formEl.style.display = 'none';
          messageEl.style.display = 'block';
          messageEl.style.background = '#dcfce7';
          messageEl.style.color = '#166534';
          messageEl.textContent = data.message || formConfig.successMessage;
          if (options.onSuccess) options.onSuccess(data);
        } else {
          throw new Error(data.message || 'Erro ao enviar');
        }
      })
      .catch(function(err) {
        messageEl.style.display = 'block';
        messageEl.style.background = '#fee2e2';
        messageEl.style.color = '#991b1b';
        messageEl.textContent = err.message || 'Erro ao enviar formulário';
        submitBtn.disabled = false;
        submitBtn.textContent = formConfig.submitButtonText;
        if (options.onError) options.onError(err);
      });
    });
  }
  
  window.DominusForm = window.DominusForm || {};
  window.DominusForm['${form.id}'] = { render: renderDominusForm, config: formConfig };
  
  // Auto-render if data-dominus-form attribute exists
  document.querySelectorAll('[data-dominus-form="${form.id}"]').forEach(function(el) {
    renderDominusForm(el.id || 'dominus-container-' + Date.now(), el.dataset);
  });
})();
`;
      
      res.setHeader('Content-Type', 'application/javascript');
      res.setHeader('Access-Control-Allow-Origin', '*');
      res.send(script);
    } catch (error: any) {
      res.setHeader('Content-Type', 'application/javascript');
      res.send('console.error("Dominus Form Error: ' + (error.message || 'Unknown error') + '");');
    }
  });

  // Site analytics tracking
  app.post("/api/public/sites/:siteId/track", async (req, res) => {
    try {
      const site = await storage.getSiteProjectById(req.params.siteId, '');
      if (!site) return res.status(404).json({ message: "Site not found" });
      
      await storage.createSiteVisitEvent({
        workspaceId: site.workspaceId,
        siteId: site.id,
        pageId: req.body.pageId || null,
        blogPostId: req.body.blogPostId || null,
        visitorId: req.body.visitorId || null,
        sessionId: req.body.sessionId || null,
        referrer: req.body.referrer || null,
        utmSource: req.body.utmSource || null,
        utmMedium: req.body.utmMedium || null,
        utmCampaign: req.body.utmCampaign || null,
        userAgent: req.headers['user-agent'] || null,
        deviceType: req.body.deviceType || null,
        country: null,
      });
      
      res.status(201).json({ success: true });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ COLLABORATION MODULE - CONVERSATIONS ============

  // List all conversations for current user
  app.get("/api/workspaces/:workspaceId/conversations", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const conversations = await storage.getConversations(req.params.workspaceId);
      res.json(conversations);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get single conversation with participants
  app.get("/api/workspaces/:workspaceId/conversations/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const conversation = await storage.getConversation(req.params.id, req.params.workspaceId);
      if (!conversation) {
        return res.status(404).json({ message: "Conversation not found" });
      }
      const participants = await storage.getConversationParticipants(req.params.id);
      res.json({ ...conversation, participants });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Create new conversation (GROUP or PROJECT type)
  app.post("/api/workspaces/:workspaceId/conversations", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertConversationSchema.parse({ ...req.body, workspaceId: req.params.workspaceId });
      const conversation = await storage.createConversation(data);
      
      // Add creator as participant
      await storage.addConversationParticipant({
        conversationId: conversation.id,
        userId: req.user!.id,
      });
      
      // Add other participants if provided
      if (req.body.participantIds && Array.isArray(req.body.participantIds)) {
        for (const userId of req.body.participantIds) {
          if (userId !== req.user!.id) {
            await storage.addConversationParticipant({
              conversationId: conversation.id,
              userId,
            });
          }
        }
      }
      
      res.status(201).json(conversation);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  // Get or create direct message conversation with a user
  app.post("/api/workspaces/:workspaceId/conversations/direct/:userId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const conversation = await storage.getOrCreateDirectConversation(
        req.params.workspaceId,
        req.user!.id,
        req.params.userId
      );
      const participants = await storage.getConversationParticipants(conversation.id);
      res.json({ ...conversation, participants });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ COLLABORATION MODULE - MESSAGES ============

  // Get messages with pagination
  app.get("/api/workspaces/:workspaceId/conversations/:conversationId/messages", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const limit = parseInt(req.query.limit as string) || 50;
      const offset = parseInt(req.query.offset as string) || 0;
      const messages = await storage.getMessages(req.params.conversationId, limit, offset);
      res.json(messages);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Send a message
  app.post("/api/workspaces/:workspaceId/conversations/:conversationId/messages", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertMessageSchema.parse({
        ...req.body,
        conversationId: req.params.conversationId,
        senderId: req.user!.id,
      });
      const message = await storage.createMessage(data);
      
      // Broadcast message via WebSocket
      const wsService = getWebSocketService();
      if (wsService) {
        wsService.broadcastNewMessage(req.params.workspaceId, req.params.conversationId, message);
      }
      
      res.status(201).json(message);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  // Edit message
  app.put("/api/workspaces/:workspaceId/conversations/:conversationId/messages/:messageId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const message = await storage.getMessage(req.params.messageId);
      if (!message) {
        return res.status(404).json({ message: "Message not found" });
      }
      if (message.senderId !== req.user!.id) {
        return res.status(403).json({ message: "You can only edit your own messages" });
      }
      const updated = await storage.updateMessage(req.params.messageId, req.body.content);
      res.json(updated);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Delete message
  app.delete("/api/workspaces/:workspaceId/conversations/:conversationId/messages/:messageId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const message = await storage.getMessage(req.params.messageId);
      if (!message) {
        return res.status(404).json({ message: "Message not found" });
      }
      if (message.senderId !== req.user!.id) {
        return res.status(403).json({ message: "You can only delete your own messages" });
      }
      await storage.deleteMessage(req.params.messageId);
      res.json({ message: "Message deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Mark conversation as read
  app.post("/api/workspaces/:workspaceId/conversations/:conversationId/read", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.updateParticipantLastRead(req.params.conversationId, req.user!.id);
      res.json({ message: "Marked as read" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ COLLABORATION MODULE - COMMENTS ============

  // Get comments for an entity
  app.get("/api/workspaces/:workspaceId/comments/:entityType/:entityId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const comments = await storage.getComments(req.params.workspaceId, req.params.entityType, req.params.entityId);
      res.json(comments);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Add comment to entity
  app.post("/api/workspaces/:workspaceId/comments/:entityType/:entityId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertCommentSchema.parse({
        ...req.body,
        workspaceId: req.params.workspaceId,
        entityType: req.params.entityType,
        entityId: req.params.entityId,
        authorId: req.user!.id,
      });
      const comment = await storage.createComment(data);
      
      // Broadcast comment via WebSocket
      const wsService = getWebSocketService();
      if (wsService) {
        wsService.broadcastNewComment(req.params.workspaceId, req.params.entityType, req.params.entityId, comment);
      }
      
      res.status(201).json(comment);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  // Edit comment
  app.put("/api/workspaces/:workspaceId/comments/:commentId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const updated = await storage.updateComment(req.params.commentId, req.body.content);
      if (!updated) {
        return res.status(404).json({ message: "Comment not found" });
      }
      res.json(updated);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Delete comment
  app.delete("/api/workspaces/:workspaceId/comments/:commentId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteComment(req.params.commentId);
      res.json({ message: "Comment deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ COLLABORATION MODULE - NOTIFICATIONS ============

  // Get user notifications
  app.get("/api/workspaces/:workspaceId/notifications", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const notifications = await storage.getNotifications(req.params.workspaceId, req.user!.id);
      res.json(notifications);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get unread count
  app.get("/api/workspaces/:workspaceId/notifications/unread-count", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const count = await storage.getUnreadNotificationCount(req.params.workspaceId, req.user!.id);
      res.json({ count });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Mark notification as read
  app.put("/api/workspaces/:workspaceId/notifications/:id/read", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const notification = await storage.markNotificationRead(req.params.id);
      if (!notification) {
        return res.status(404).json({ message: "Notification not found" });
      }
      res.json(notification);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Mark all as read
  app.put("/api/workspaces/:workspaceId/notifications/read-all", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.markAllNotificationsRead(req.params.workspaceId, req.user!.id);
      res.json({ message: "All notifications marked as read" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ FILES API ============

  const objectStorage = new ObjectStorageService();

  // List files with optional filters
  app.get("/api/workspaces/:workspaceId/files", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { projectId, clientId, taskId, folder } = req.query;
      const files = await storage.getFiles(req.params.workspaceId, {
        projectId: projectId as string | undefined,
        clientId: clientId as string | undefined,
        taskId: taskId as string | undefined,
        folder: folder as string | undefined,
      });
      res.json(files);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get single file
  app.get("/api/workspaces/:workspaceId/files/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const file = await storage.getFile(req.params.id, req.params.workspaceId);
      if (!file) {
        return res.status(404).json({ message: "File not found" });
      }
      res.json(file);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Request presigned upload URL for files
  app.post("/api/workspaces/:workspaceId/files/request-url", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const uploadUrl = await objectStorage.getObjectEntityUploadURL();
      const objectPath = objectStorage.normalizeObjectEntityPath(uploadUrl);
      res.json({ uploadUrl, objectPath });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Create file record after successful upload
  app.post("/api/workspaces/:workspaceId/files", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertFileSchema.parse({
        ...req.body,
        workspaceId: req.params.workspaceId,
        uploadedBy: req.user!.id,
      });
      const file = await storage.createFile(data);
      res.status(201).json(file);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  // Delete file (also delete from storage)
  app.delete("/api/workspaces/:workspaceId/files/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const file = await storage.getFile(req.params.id, req.params.workspaceId);
      if (!file) {
        return res.status(404).json({ message: "File not found" });
      }

      // Try to delete from object storage if it's an object path
      if (file.fileUrl.startsWith("/objects/")) {
        try {
          const objectFile = await objectStorage.getObjectEntityFile(file.fileUrl);
          await objectFile.delete();
        } catch (err) {
          console.error("Error deleting from object storage:", err);
        }
      }

      await storage.deleteFile(req.params.id, req.params.workspaceId);
      res.json({ message: "File deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ============ CLIENT DOCUMENTS API ============

  // List documents for a client
  app.get("/api/workspaces/:workspaceId/clients/:clientId/documents", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const documents = await storage.getClientDocuments(req.params.workspaceId, req.params.clientId);
      res.json(documents);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get documents expiring soon
  app.get("/api/workspaces/:workspaceId/documents/expiring", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const days = parseInt(req.query.days as string) || 30;
      const documents = await storage.getExpiringDocuments(req.params.workspaceId, days);
      res.json(documents);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get single document
  app.get("/api/workspaces/:workspaceId/documents/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const document = await storage.getClientDocument(req.params.id, req.params.workspaceId);
      if (!document) {
        return res.status(404).json({ message: "Document not found" });
      }
      res.json(document);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Request presigned upload URL for document
  app.post("/api/workspaces/:workspaceId/clients/:clientId/documents/request-url", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const uploadUrl = await objectStorage.getObjectEntityUploadURL();
      const objectPath = objectStorage.normalizeObjectEntityPath(uploadUrl);
      res.json({ uploadUrl, objectPath });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Create document record
  app.post("/api/workspaces/:workspaceId/clients/:clientId/documents", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertClientDocumentSchema.parse({
        ...req.body,
        workspaceId: req.params.workspaceId,
        clientId: req.params.clientId,
        uploadedBy: req.user!.id,
      });
      const document = await storage.createClientDocument(data);
      res.status(201).json(document);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  // Update document metadata
  app.put("/api/workspaces/:workspaceId/documents/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const document = await storage.updateClientDocument(req.params.id, req.body);
      if (!document) {
        return res.status(404).json({ message: "Document not found" });
      }
      res.json(document);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Delete document
  app.delete("/api/workspaces/:workspaceId/documents/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const document = await storage.getClientDocument(req.params.id, req.params.workspaceId);
      if (!document) {
        return res.status(404).json({ message: "Document not found" });
      }

      // Try to delete from object storage if it's an object path
      if (document.fileUrl.startsWith("/objects/")) {
        try {
          const objectFile = await objectStorage.getObjectEntityFile(document.fileUrl);
          await objectFile.delete();
        } catch (err) {
          console.error("Error deleting from object storage:", err);
        }
      }

      await storage.deleteClientDocument(req.params.id, req.params.workspaceId);
      res.json({ message: "Document deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ==========================================
  // WORKFLOWS (AUTOMATION)
  // ==========================================

  // Get all workflows
  app.get("/api/workspaces/:workspaceId/workflows", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const workflows = await storage.getWorkflows(req.params.workspaceId);
      res.json(workflows);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get workflow templates
  app.get("/api/workflows/templates", requireAuth, async (req: Request, res: Response) => {
    try {
      const templates = await storage.getWorkflowTemplates();
      res.json(templates);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get single workflow
  app.get("/api/workspaces/:workspaceId/workflows/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const workflow = await storage.getWorkflow(req.params.id, req.params.workspaceId);
      if (!workflow) {
        return res.status(404).json({ message: "Workflow not found" });
      }
      res.json(workflow);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Create workflow
  app.post("/api/workspaces/:workspaceId/workflows", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertWorkflowSchema.parse({
        ...req.body,
        workspaceId: req.params.workspaceId,
        createdBy: req.user!.id,
        actions: typeof req.body.actions === 'string' ? req.body.actions : JSON.stringify(req.body.actions || []),
      });
      const workflow = await storage.createWorkflow(data);
      res.status(201).json(workflow);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  // Update workflow
  app.put("/api/workspaces/:workspaceId/workflows/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const updateData = {
        ...req.body,
        actions: typeof req.body.actions === 'string' ? req.body.actions : JSON.stringify(req.body.actions || []),
      };
      const workflow = await storage.updateWorkflow(req.params.id, req.params.workspaceId, updateData);
      if (!workflow) {
        return res.status(404).json({ message: "Workflow not found" });
      }
      res.json(workflow);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Delete workflow
  app.delete("/api/workspaces/:workspaceId/workflows/:id", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.deleteWorkflow(req.params.id, req.params.workspaceId);
      res.json({ message: "Workflow deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Toggle workflow status
  app.post("/api/workspaces/:workspaceId/workflows/:id/toggle", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const workflow = await storage.getWorkflow(req.params.id, req.params.workspaceId);
      if (!workflow) {
        return res.status(404).json({ message: "Workflow not found" });
      }
      const newStatus = workflow.status === 'ACTIVE' ? 'PAUSED' : 'ACTIVE';
      const updated = await storage.updateWorkflow(req.params.id, req.params.workspaceId, { status: newStatus });
      res.json(updated);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get workflow executions
  app.get("/api/workspaces/:workspaceId/workflows/:id/executions", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const executions = await storage.getWorkflowExecutions(req.params.id);
      res.json(executions);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Test workflow (manual trigger)
  app.post("/api/workspaces/:workspaceId/workflows/:id/test", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const workflow = await storage.getWorkflow(req.params.id, req.params.workspaceId);
      if (!workflow) {
        return res.status(404).json({ message: "Workflow not found" });
      }
      const execution = await workflowEngine.executeWorkflow(workflow, {
        workspaceId: req.params.workspaceId,
        userId: req.user!.id,
        entityType: 'test',
        entityId: 'test',
        entityData: req.body.testData || {},
      });
      res.json(execution);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ==========================================
  // AI CHAT (SUPER WORK AI)
  // ==========================================

  // Get AI chat sessions
  app.get("/api/workspaces/:workspaceId/ai/sessions", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const sessions = await storage.getAiChatSessions(req.params.workspaceId, req.user!.id);
      res.json(sessions);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Create new AI chat session
  app.post("/api/workspaces/:workspaceId/ai/sessions", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const data = insertAiChatSessionSchema.parse({
        ...req.body,
        workspaceId: req.params.workspaceId,
        userId: req.user!.id,
      });
      const session = await storage.createAiChatSession(data);
      res.status(201).json(session);
    } catch (error: any) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Validation error", errors: error.errors });
      }
      res.status(500).json({ message: error.message });
    }
  });

  // Get AI chat session with messages
  app.get("/api/workspaces/:workspaceId/ai/sessions/:sessionId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const session = await storage.getAiChatSession(req.params.sessionId);
      if (!session || session.workspaceId !== req.params.workspaceId || session.userId !== req.user!.id) {
        return res.status(404).json({ message: "Session not found" });
      }
      const messages = await storage.getAiChatMessages(req.params.sessionId);
      res.json({ session, messages });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Send message to AI chat
  app.post("/api/workspaces/:workspaceId/ai/sessions/:sessionId/messages", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const session = await storage.getAiChatSession(req.params.sessionId);
      if (!session || session.workspaceId !== req.params.workspaceId || session.userId !== req.user!.id) {
        return res.status(404).json({ message: "Session not found" });
      }
      
      const { message, contextData } = req.body;
      if (!message) {
        return res.status(400).json({ message: "Message is required" });
      }
      
      const result = await aiService.chat(req.params.sessionId, message, {
        workspaceId: req.params.workspaceId,
        userId: req.user!.id,
        contextType: session.contextType || undefined,
        contextId: session.contextId || undefined,
        contextData,
      });
      
      res.json(result);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Delete AI chat session
  app.delete("/api/workspaces/:workspaceId/ai/sessions/:sessionId", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const session = await storage.getAiChatSession(req.params.sessionId);
      if (!session || session.workspaceId !== req.params.workspaceId || session.userId !== req.user!.id) {
        return res.status(404).json({ message: "Session not found" });
      }
      await storage.deleteAiChatSession(req.params.sessionId);
      res.json({ message: "Session deleted" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Generate AI content (standalone)
  app.post("/api/workspaces/:workspaceId/ai/generate", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const { prompt, type, context } = req.body;
      if (!prompt) {
        return res.status(400).json({ message: "Prompt is required" });
      }
      const content = await aiService.generateContent(prompt, type || 'general', context);
      res.json({ content });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ==========================================
  // NOTIFICATIONS
  // ==========================================

  // Get notifications for current user
  app.get("/api/workspaces/:workspaceId/notifications", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const notifications = await storage.getNotifications(req.params.workspaceId, req.user!.id);
      res.json(notifications);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get unread notification count
  app.get("/api/workspaces/:workspaceId/notifications/unread-count", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const count = await storage.getUnreadNotificationCount(req.params.workspaceId, req.user!.id);
      res.json({ count });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Mark notification as read
  app.post("/api/workspaces/:workspaceId/notifications/:id/read", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      const notification = await storage.markNotificationRead(req.params.id);
      res.json(notification);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Mark all notifications as read
  app.post("/api/workspaces/:workspaceId/notifications/read-all", requireAuth, requireWorkspace, async (req: Request, res: Response) => {
    try {
      await storage.markAllNotificationsRead(req.params.workspaceId, req.user!.id);
      res.json({ message: "All notifications marked as read" });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ==========================================
  // HOST PANEL ROUTES
  // ==========================================

  // Middleware: Require HOST role
  async function requireHost(req: Request, res: Response, next: NextFunction) {
    if (!req.user) {
      return res.status(401).json({ message: "Unauthorized" });
    }
    if (req.user.globalRole !== 'HOST') {
      return res.status(403).json({ message: "Acesso negado. Apenas hosts podem acessar este recurso." });
    }
    next();
  }

  // Check if current user is a host
  app.get("/api/host/check", requireAuth, async (req: Request, res: Response) => {
    try {
      res.json({ isHost: req.user!.globalRole === 'HOST' });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get host dashboard metrics
  app.get("/api/host/dashboard", requireAuth, requireHost, async (req: Request, res: Response) => {
    try {
      const metrics = await storage.getAggregatedUsageMetrics();
      const recentEvents = await storage.getRecentAccessEvents(10);
      const workspaces = await storage.getAllWorkspacesWithSubscriptions();
      
      // Count by plan
      const planCounts = {
        TRIAL: 0,
        STARTER: 0,
        PROFESSIONAL: 0,
        ENTERPRISE: 0,
      };
      
      workspaces.forEach(w => {
        const plan = w.subscription?.plan || 'TRIAL';
        planCounts[plan as keyof typeof planCounts]++;
      });
      
      res.json({
        ...metrics,
        planCounts,
        recentEvents,
      });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get all workspaces (host view - "clientes")
  app.get("/api/host/workspaces", requireAuth, requireHost, async (req: Request, res: Response) => {
    try {
      const workspaces = await storage.getAllWorkspacesWithSubscriptions();
      
      // Enrich with member count and owner info
      const enrichedWorkspaces = await Promise.all(workspaces.map(async (w) => {
        const members = await storage.getWorkspaceMembers(w.id);
        const owner = await storage.getUserById(w.ownerId);
        return {
          ...w,
          memberCount: members.length,
          ownerName: owner?.name || 'N/A',
          ownerEmail: owner?.email || 'N/A',
        };
      }));
      
      res.json(enrichedWorkspaces);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get single workspace details
  app.get("/api/host/workspaces/:workspaceId", requireAuth, requireHost, async (req: Request, res: Response) => {
    try {
      const workspace = await storage.getWorkspaceWithSubscription(req.params.workspaceId);
      if (!workspace) {
        return res.status(404).json({ message: "Workspace não encontrado" });
      }
      
      const members = await storage.getWorkspaceMembersWithUsers(workspace.id);
      const owner = await storage.getUserById(workspace.ownerId);
      
      // Get counts
      const clients = await storage.getClientsByWorkspace(workspace.id);
      const projects = await storage.getProjectsByWorkspace(workspace.id);
      const tasks = await storage.getTasksByWorkspace(workspace.id);
      
      res.json({
        ...workspace,
        members,
        ownerName: owner?.name,
        ownerEmail: owner?.email,
        clientsCount: clients.length,
        projectsCount: projects.length,
        tasksCount: tasks.length,
      });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Update workspace subscription
  app.patch("/api/host/workspaces/:workspaceId/subscription", requireAuth, requireHost, async (req: Request, res: Response) => {
    try {
      const { plan, status, maxUsers, maxProjects, maxStorage, trialEndsAt, billingEmail, nextBillingDate } = req.body;
      
      let subscription = await storage.getWorkspaceSubscription(req.params.workspaceId);
      
      if (!subscription) {
        subscription = await storage.createWorkspaceSubscription({
          workspaceId: req.params.workspaceId,
          plan: plan || 'TRIAL',
          status: status || 'TRIAL',
          maxUsers: maxUsers || 3,
          maxProjects: maxProjects || 5,
          maxStorage: maxStorage || 1024,
          trialEndsAt: trialEndsAt ? new Date(trialEndsAt) : null,
          billingEmail,
          nextBillingDate: nextBillingDate ? new Date(nextBillingDate) : null,
        });
      } else {
        subscription = await storage.updateWorkspaceSubscription(req.params.workspaceId, {
          plan,
          status,
          maxUsers,
          maxProjects,
          maxStorage,
          trialEndsAt: trialEndsAt ? new Date(trialEndsAt) : undefined,
          billingEmail,
          nextBillingDate: nextBillingDate ? new Date(nextBillingDate) : undefined,
        });
      }
      
      res.json(subscription);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get all users (host view)
  app.get("/api/host/users", requireAuth, requireHost, async (req: Request, res: Response) => {
    try {
      const users = await storage.getAllUsers();
      
      // Get workspace counts per user
      const enrichedUsers = await Promise.all(users.map(async (u) => {
        const workspaces = await storage.getUserWorkspaces(u.id);
        return {
          id: u.id,
          email: u.email,
          name: u.name,
          avatar: u.avatar,
          globalRole: u.globalRole,
          createdAt: u.createdAt,
          workspaceCount: workspaces.length,
        };
      }));
      
      res.json(enrichedUsers);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Update user global role
  app.patch("/api/host/users/:userId/role", requireAuth, requireHost, async (req: Request, res: Response) => {
    try {
      const { role } = req.body;
      if (!role || !['USER', 'HOST'].includes(role)) {
        return res.status(400).json({ message: "Role inválido. Use 'USER' ou 'HOST'." });
      }
      
      const updated = await storage.updateUserGlobalRole(req.params.userId, role);
      if (!updated) {
        return res.status(404).json({ message: "Usuário não encontrado" });
      }
      
      res.json({ id: updated.id, email: updated.email, name: updated.name, globalRole: updated.globalRole });
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get workspace access events
  app.get("/api/host/workspaces/:workspaceId/events", requireAuth, requireHost, async (req: Request, res: Response) => {
    try {
      const limit = parseInt(req.query.limit as string) || 100;
      const events = await storage.getWorkspaceAccessEvents(req.params.workspaceId, limit);
      res.json(events);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Get recent access events across all workspaces
  app.get("/api/host/events", requireAuth, requireHost, async (req: Request, res: Response) => {
    try {
      const limit = parseInt(req.query.limit as string) || 50;
      const events = await storage.getRecentAccessEvents(limit);
      res.json(events);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // ========== PUBLIC LANDING PAGE ROUTES ==========
  
  // Submit lead from landing page (no auth required)
  app.post("/api/leads/landing", async (req: Request, res: Response) => {
    try {
      const { name, email, company, message } = req.body;
      
      if (!name || !email) {
        return res.status(400).json({ message: "Nome e email são obrigatórios" });
      }
      
      const lead = await storage.createLandingLead({
        name,
        email,
        company: company || null,
        message: message || null,
        source: 'landing',
        status: 'NEW',
        notes: null,
      });
      
      res.status(201).json({ success: true, id: lead.id });
    } catch (error: any) {
      console.error("Error creating landing lead:", error);
      res.status(500).json({ message: "Erro ao enviar mensagem" });
    }
  });
  
  // Get all landing leads (host only)
  app.get("/api/host/landing-leads", requireAuth, requireHost, async (req: Request, res: Response) => {
    try {
      const leads = await storage.getLandingLeads();
      res.json(leads);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });
  
  // Update landing lead status (host only)
  app.patch("/api/host/landing-leads/:leadId", requireAuth, requireHost, async (req: Request, res: Response) => {
    try {
      const { status, notes } = req.body;
      const lead = await storage.updateLandingLead(req.params.leadId, { status, notes });
      if (!lead) {
        return res.status(404).json({ message: "Lead não encontrado" });
      }
      res.json(lead);
    } catch (error: any) {
      res.status(500).json({ message: error.message });
    }
  });

  // Initialize WebSocket server for real-time updates
  initWebSocket(httpServer);

  return httpServer;
}
