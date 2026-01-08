import { storage } from "./storage";
import type { Workflow, WorkflowExecution, InsertNotification } from "@shared/schema";
import { sendSmsBySlug } from "./sms-service";
import { sendWhatsappBySlug } from "./whatsapp-service";
import { sendEmailBySlug } from "./email-service";

export type TriggerType = 
  | 'CLIENT_CREATED' | 'CLIENT_UPDATED'
  | 'PROJECT_CREATED' | 'PROJECT_STATUS_CHANGED' | 'PROJECT_COMPLETED'
  | 'TASK_CREATED' | 'TASK_COMPLETED' | 'TASK_ASSIGNED' | 'TASK_DUE_SOON' | 'TASK_OVERDUE'
  | 'PROPOSAL_CREATED' | 'PROPOSAL_SENT' | 'PROPOSAL_ACCEPTED' | 'PROPOSAL_REJECTED'
  | 'INVOICE_CREATED' | 'INVOICE_SENT' | 'INVOICE_PAID' | 'INVOICE_OVERDUE'
  | 'CONTRACT_CREATED' | 'CONTRACT_SIGNED'
  | 'LEAD_CREATED' | 'LEAD_STATUS_CHANGED'
  | 'TRANSACTION_CREATED' | 'TRANSACTION_DUE_SOON'
  | 'DOCUMENT_EXPIRING' | 'SCHEDULED';

export interface TriggerContext {
  workspaceId: string;
  userId?: string;
  entityType?: string;
  entityId?: string;
  entityData?: Record<string, any>;
  previousData?: Record<string, any>;
}

export interface WorkflowAction {
  type: string;
  config: Record<string, any>;
  order: number;
}

export interface ActionResult {
  success: boolean;
  message?: string;
  data?: any;
  error?: string;
}

class WorkflowEngine {
  async fireTrigger(triggerType: TriggerType, context: TriggerContext): Promise<void> {
    try {
      const workflows = await storage.getActiveWorkflowsByTrigger(context.workspaceId, triggerType);
      
      for (const workflow of workflows) {
        if (this.matchesConditions(workflow, context)) {
          this.executeWorkflow(workflow, context).catch(console.error);
        }
      }
    } catch (error) {
      console.error(`Error firing trigger ${triggerType}:`, error);
    }
  }

  private matchesConditions(workflow: Workflow, context: TriggerContext): boolean {
    if (!workflow.triggerConditions) return true;
    
    try {
      const conditions = JSON.parse(workflow.triggerConditions);
      
      for (const [field, expectedValue] of Object.entries(conditions)) {
        const actualValue = context.entityData?.[field];
        if (actualValue !== expectedValue) {
          return false;
        }
      }
      
      return true;
    } catch {
      return true;
    }
  }

  async executeWorkflow(workflow: Workflow, context: TriggerContext): Promise<WorkflowExecution> {
    const actions: WorkflowAction[] = JSON.parse(workflow.actions);
    
    const execution = await storage.createWorkflowExecution({
      workflowId: workflow.id,
      workspaceId: workflow.workspaceId,
      triggerType: workflow.triggerType,
      triggerData: JSON.stringify(context),
      status: 'RUNNING',
      totalSteps: actions.length,
      stepsCompleted: 0,
    });

    await storage.updateWorkflowExecution(execution.id, { startedAt: new Date() });
    
    const results: ActionResult[] = [];
    
    try {
      const sortedActions = [...actions].sort((a, b) => a.order - b.order);
      
      for (let i = 0; i < sortedActions.length; i++) {
        const action = sortedActions[i];
        
        await storage.updateWorkflowExecution(execution.id, {
          currentStep: action.type,
          stepsCompleted: i,
        });
        
        const result = await this.executeAction(action, context, results);
        results.push(result);
        
        if (!result.success) {
          throw new Error(result.error || `Action ${action.type} failed`);
        }
        
        if (action.type === 'WAIT_DELAY') {
          const delayMs = (action.config.minutes || 0) * 60 * 1000;
          if (delayMs > 0) {
            await new Promise(resolve => setTimeout(resolve, Math.min(delayMs, 5000)));
          }
        }
      }
      
      await storage.updateWorkflowExecution(execution.id, {
        status: 'COMPLETED',
        stepsCompleted: actions.length,
        result: JSON.stringify(results),
        completedAt: new Date(),
      });
      
      await storage.incrementWorkflowExecutionCount(workflow.id);
      
    } catch (error: any) {
      await storage.updateWorkflowExecution(execution.id, {
        status: 'FAILED',
        errorMessage: error.message,
        result: JSON.stringify(results),
        completedAt: new Date(),
      });
    }
    
    return (await storage.getWorkflowExecutions(workflow.id, 1))[0];
  }

  private async executeAction(
    action: WorkflowAction,
    context: TriggerContext,
    previousResults: ActionResult[]
  ): Promise<ActionResult> {
    try {
      switch (action.type) {
        case 'SEND_NOTIFICATION':
          return await this.actionSendNotification(action.config, context);
        
        case 'CREATE_TASK':
          return await this.actionCreateTask(action.config, context);
        
        case 'UPDATE_STATUS':
          return await this.actionUpdateStatus(action.config, context);
        
        case 'CREATE_COMMENT':
          return await this.actionCreateComment(action.config, context);
        
        case 'ASSIGN_USER':
          return await this.actionAssignUser(action.config, context);
        
        case 'WAIT_DELAY':
          return { success: true, message: `Waited ${action.config.minutes || 0} minutes` };
        
        case 'GENERATE_AI_CONTENT':
          return await this.actionGenerateAiContent(action.config, context);
        
        case 'SEND_EMAIL':
          return await this.actionSendEmail(action.config, context);
        
        case 'SEND_SMS':
          return await this.actionSendSms(action.config, context);
        
        case 'SEND_WHATSAPP':
          return await this.actionSendWhatsapp(action.config, context);
        
        default:
          return { success: true, message: `Action ${action.type} executed (no-op)` };
      }
    } catch (error: any) {
      return { success: false, error: error.message };
    }
  }

  private async actionSendNotification(config: Record<string, any>, context: TriggerContext): Promise<ActionResult> {
    const title = this.interpolateTemplate(config.title || 'Notificação', context);
    const message = this.interpolateTemplate(config.message || '', context);
    
    const targetUserIds: string[] = [];
    
    if (config.notifyOwner && context.userId) {
      targetUserIds.push(context.userId);
    }
    
    if (config.notifyAssignees && context.entityData?.assigneeId) {
      targetUserIds.push(context.entityData.assigneeId);
    }
    
    if (config.userIds?.length) {
      targetUserIds.push(...config.userIds);
    }
    
    const uniqueUserIds = Array.from(new Set(targetUserIds));
    
    for (const userId of uniqueUserIds) {
      const notificationData: InsertNotification = {
        workspaceId: context.workspaceId,
        userId,
        type: 'SYSTEM',
        title,
        message,
        entityType: context.entityType,
        entityId: context.entityId,
      };
      
      await storage.createNotification(notificationData);
    }
    
    return { success: true, message: `Sent notification to ${uniqueUserIds.length} users` };
  }

  private async actionCreateTask(config: Record<string, any>, context: TriggerContext): Promise<ActionResult> {
    const title = this.interpolateTemplate(config.title || 'Nova tarefa', context);
    const description = this.interpolateTemplate(config.description || '', context);
    
    const taskData = {
      workspaceId: context.workspaceId,
      projectId: config.projectId || context.entityData?.projectId || null,
      title,
      description,
      assigneeId: config.assigneeId || null,
      dueDate: config.dueDays ? new Date(Date.now() + config.dueDays * 24 * 60 * 60 * 1000) : null,
    };
    
    const task = await storage.createTask(taskData);
    
    return { success: true, message: `Task created: ${title}`, data: { taskId: task.id } };
  }

  private async actionUpdateStatus(config: Record<string, any>, context: TriggerContext): Promise<ActionResult> {
    if (!context.entityType || !context.entityId) {
      return { success: false, error: 'No entity to update' };
    }
    
    const newStatus = config.status;
    
    switch (context.entityType) {
      case 'project':
        await storage.updateProject(context.entityId, context.workspaceId, { status: newStatus });
        break;
      case 'task':
        await storage.updateTask(context.entityId, context.workspaceId, { isCompleted: newStatus === 'COMPLETED' });
        break;
      case 'lead':
        break;
      default:
        return { success: false, error: `Cannot update status for ${context.entityType}` };
    }
    
    return { success: true, message: `Status updated to ${newStatus}` };
  }

  private async actionCreateComment(config: Record<string, any>, context: TriggerContext): Promise<ActionResult> {
    if (!context.entityType || !context.entityId) {
      return { success: false, error: 'No entity to comment on' };
    }
    
    const content = this.interpolateTemplate(config.content || '', context);
    
    const entityType = context.entityType.toUpperCase() as 'TASK' | 'PROJECT' | 'PROPOSAL' | 'CONTRACT' | 'INVOICE' | 'LEAD';
    
    const commentData = {
      workspaceId: context.workspaceId,
      entityType,
      entityId: context.entityId,
      authorId: config.authorId || context.userId || '',
      content,
    };
    
    await storage.createComment(commentData);
    
    return { success: true, message: 'Comment created' };
  }

  private async actionAssignUser(config: Record<string, any>, context: TriggerContext): Promise<ActionResult> {
    if (!context.entityType || !context.entityId) {
      return { success: false, error: 'No entity to assign' };
    }
    
    const assigneeId = config.userId;
    
    if (context.entityType === 'task') {
      await storage.updateTask(context.entityId, context.workspaceId, { assigneeId });
    }
    
    return { success: true, message: `Assigned user ${assigneeId}` };
  }

  private async actionGenerateAiContent(config: Record<string, any>, context: TriggerContext): Promise<ActionResult> {
    return { success: true, message: 'AI content generation placeholder', data: {} };
  }

  private async actionSendEmail(config: Record<string, any>, context: TriggerContext): Promise<ActionResult> {
    const templateId = config.templateId;
    const email = config.email || context.entityData?.email || context.entityData?.clientEmail;
    
    if (!email) {
      return { success: false, error: 'Email do destinatário não encontrado' };
    }
    
    if (!templateId) {
      return { success: false, error: 'Template de email não especificado' };
    }
    
    const variables: Record<string, string> = {
      nome: context.entityData?.name || context.entityData?.clientName || '',
      email: email,
      valor: context.entityData?.total?.toString() || context.entityData?.amount?.toString() || '',
      vencimento: context.entityData?.dueDate || '',
      link: context.entityData?.link || '',
      proposta: context.entityData?.proposalNumber || '',
      contrato: context.entityData?.contractNumber || '',
      fatura: context.entityData?.invoiceNumber || '',
      projeto: context.entityData?.projectName || '',
      workspace: context.entityData?.workspaceName || '',
      ...config.variables,
    };
    
    try {
      const result = await sendEmailBySlug(context.workspaceId, email, templateId, variables);
      
      if (result.success) {
        return { success: true, message: `Email enviado para ${email}`, data: { messageId: result.messageId } };
      } else {
        return { success: false, error: result.error || 'Erro ao enviar email' };
      }
    } catch (error: any) {
      return { success: false, error: `Erro ao enviar email: ${error.message}` };
    }
  }

  private async actionSendSms(config: Record<string, any>, context: TriggerContext): Promise<ActionResult> {
    const templateSlug = config.templateSlug;
    const phoneNumber = config.phoneNumber || context.entityData?.phone || context.entityData?.clientPhone;
    
    if (!phoneNumber) {
      return { success: false, error: 'Número de telefone não encontrado' };
    }
    
    if (!templateSlug) {
      return { success: false, error: 'Template de SMS não especificado' };
    }
    
    const variables: Record<string, string> = {
      nome: context.entityData?.name || context.entityData?.clientName || '',
      valor: context.entityData?.total?.toString() || context.entityData?.amount?.toString() || '',
      vencimento: context.entityData?.dueDate || '',
      link: context.entityData?.link || '',
      proposta: context.entityData?.proposalNumber || '',
      contrato: context.entityData?.contractNumber || '',
      fatura: context.entityData?.invoiceNumber || '',
      projeto: context.entityData?.projectName || '',
      ...config.variables,
    };
    
    try {
      const result = await sendSmsBySlug(context.workspaceId, phoneNumber, templateSlug, variables);
      
      if (result.success) {
        return { success: true, message: `SMS enviado para ${phoneNumber}`, data: { messageId: result.messageId } };
      } else {
        return { success: false, error: result.error || 'Erro ao enviar SMS' };
      }
    } catch (error: any) {
      return { success: false, error: `Erro ao enviar SMS: ${error.message}` };
    }
  }

  private async actionSendWhatsapp(config: Record<string, any>, context: TriggerContext): Promise<ActionResult> {
    const templateSlug = config.templateSlug;
    const phoneNumber = config.phoneNumber || context.entityData?.phone || context.entityData?.clientPhone;
    
    if (!phoneNumber) {
      return { success: false, error: 'Número de telefone não encontrado' };
    }
    
    if (!templateSlug) {
      return { success: false, error: 'Template de WhatsApp não especificado' };
    }
    
    const variables: Record<string, string> = {
      nome: context.entityData?.name || context.entityData?.clientName || '',
      valor: context.entityData?.total?.toString() || context.entityData?.amount?.toString() || '',
      vencimento: context.entityData?.dueDate || '',
      link: context.entityData?.link || '',
      proposta: context.entityData?.proposalNumber || '',
      contrato: context.entityData?.contractNumber || '',
      fatura: context.entityData?.invoiceNumber || '',
      projeto: context.entityData?.projectName || '',
      ...config.variables,
    };
    
    try {
      const result = await sendWhatsappBySlug(context.workspaceId, phoneNumber, templateSlug, variables);
      
      if (result.success) {
        return { success: true, message: `WhatsApp enviado para ${phoneNumber}`, data: { messageId: result.messageId } };
      } else {
        return { success: false, error: result.error || 'Erro ao enviar WhatsApp' };
      }
    } catch (error: any) {
      return { success: false, error: `Erro ao enviar WhatsApp: ${error.message}` };
    }
  }

  private interpolateTemplate(template: string, context: TriggerContext): string {
    let result = template;
    
    result = result.replace(/\{\{entity\.(\w+)\}\}/g, (_, field) => {
      return context.entityData?.[field]?.toString() || '';
    });
    
    result = result.replace(/\{\{previous\.(\w+)\}\}/g, (_, field) => {
      return context.previousData?.[field]?.toString() || '';
    });
    
    result = result.replace(/\{\{workspaceId\}\}/g, context.workspaceId);
    result = result.replace(/\{\{entityId\}\}/g, context.entityId || '');
    result = result.replace(/\{\{entityType\}\}/g, context.entityType || '');
    
    return result;
  }
}

export const workflowEngine = new WorkflowEngine();
