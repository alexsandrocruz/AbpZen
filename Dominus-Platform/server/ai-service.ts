import OpenAI from "openai";
import { storage } from "./storage";
import type { AiChatMessage } from "@shared/schema";

const openai = new OpenAI({
  apiKey: process.env.AI_INTEGRATIONS_OPENAI_API_KEY,
  baseURL: process.env.AI_INTEGRATIONS_OPENAI_BASE_URL,
});

export interface ChatContext {
  workspaceId: string;
  userId: string;
  contextType?: string;
  contextId?: string;
  contextData?: Record<string, any>;
}

export interface AiAction {
  type: 'CREATE_TASK' | 'CREATE_CLIENT' | 'CREATE_PROPOSAL' | 'SEARCH' | 'ANSWER';
  params?: Record<string, any>;
  result?: any;
}

const SYSTEM_PROMPT = `Você é o Super Work AI, o assistente inteligente do Dominus - uma plataforma de gestão empresarial.

Você pode ajudar os usuários com:
- Responder perguntas sobre o sistema e seus dados
- Criar tarefas, clientes, propostas
- Sugerir próximos passos
- Gerar conteúdo (descrições, propostas, emails)
- Agendar lembretes

Quando o usuário pedir para criar algo, responda confirmando o que será criado.
Quando perguntarem sobre dados, use o contexto fornecido para responder.
Seja conciso, profissional e útil.
Responda sempre em português brasileiro.

Se precisar executar uma ação, inclua no final da resposta um bloco JSON no formato:
[ACTION: {"type": "CREATE_TASK", "params": {"title": "...", "description": "..."}}]

Tipos de ação disponíveis:
- CREATE_TASK: criar uma tarefa (params: title, description, projectId?, assigneeId?, dueDate?)
- CREATE_CLIENT: criar um cliente (params: name, email, phone?, companyName?)
- ANSWER: apenas responder (padrão)
`;

class AiService {
  async chat(
    sessionId: string,
    userMessage: string,
    context: ChatContext
  ): Promise<{ response: string; action?: AiAction }> {
    const messages = await storage.getAiChatMessages(sessionId);
    
    await storage.createAiChatMessage({
      sessionId,
      role: 'user',
      content: userMessage,
    });
    
    const contextInfo = await this.buildContextInfo(context);
    
    const openaiMessages: OpenAI.Chat.ChatCompletionMessageParam[] = [
      { role: 'system', content: SYSTEM_PROMPT + '\n\nContexto atual:\n' + contextInfo },
      ...messages.map(m => ({
        role: m.role as 'user' | 'assistant' | 'system',
        content: m.content,
      })),
      { role: 'user', content: userMessage },
    ];
    
    try {
      const completion = await openai.chat.completions.create({
        model: 'gpt-4o',
        messages: openaiMessages,
        max_completion_tokens: 2048,
      });
      
      const responseContent = completion.choices[0]?.message?.content || 'Desculpe, não consegui processar sua solicitação.';
      const tokensUsed = completion.usage?.total_tokens || 0;
      
      const { cleanResponse, action } = this.parseResponse(responseContent);
      
      let executedAction: AiAction | undefined;
      let finalResponse = cleanResponse;
      
      if (action) {
        executedAction = await this.executeAction(action, context);
        
        if (executedAction.result?.success && executedAction.result?.message) {
          finalResponse = `${cleanResponse}\n\n✅ ${executedAction.result.message}`;
        } else if (executedAction.result?.success === false) {
          finalResponse = `${cleanResponse}\n\n❌ Erro ao executar ação: ${executedAction.result.error}`;
        }
      }
      
      await storage.createAiChatMessage({
        sessionId,
        role: 'assistant',
        content: finalResponse,
        tokensUsed,
        actionExecuted: executedAction ? JSON.stringify(executedAction) : undefined,
      });
      
      await storage.updateAiChatSession(sessionId, { updatedAt: new Date() } as any);
      
      return { response: cleanResponse, action: executedAction };
    } catch (error: any) {
      console.error('AI Service error:', error);
      
      const errorResponse = 'Desculpe, ocorreu um erro ao processar sua solicitação. Por favor, tente novamente.';
      
      await storage.createAiChatMessage({
        sessionId,
        role: 'assistant',
        content: errorResponse,
      });
      
      return { response: errorResponse };
    }
  }

  async generateContent(
    prompt: string,
    type: 'proposal' | 'email' | 'task_description' | 'general',
    context?: Record<string, any>
  ): Promise<string> {
    const typePrompts: Record<string, string> = {
      proposal: 'Gere o texto de uma proposta comercial profissional com base no seguinte:',
      email: 'Escreva um email profissional com base no seguinte:',
      task_description: 'Escreva uma descrição clara e detalhada para a seguinte tarefa:',
      general: 'Com base no seguinte contexto, escreva:',
    };
    
    const systemPrompt = typePrompts[type] || typePrompts.general;
    
    try {
      const completion = await openai.chat.completions.create({
        model: 'gpt-4o',
        messages: [
          { role: 'system', content: systemPrompt },
          { role: 'user', content: prompt + (context ? '\n\nContexto: ' + JSON.stringify(context) : '') },
        ],
        max_completion_tokens: 2048,
      });
      
      return completion.choices[0]?.message?.content || '';
    } catch (error) {
      console.error('AI content generation error:', error);
      throw new Error('Falha ao gerar conteúdo');
    }
  }

  private async buildContextInfo(context: ChatContext): Promise<string> {
    const parts: string[] = [];
    
    parts.push(`Workspace ID: ${context.workspaceId}`);
    
    if (context.contextType && context.contextId) {
      parts.push(`Contexto: ${context.contextType} (ID: ${context.contextId})`);
      
      if (context.contextData) {
        parts.push(`Dados: ${JSON.stringify(context.contextData)}`);
      }
    }
    
    try {
      const [clients, projects, tasks] = await Promise.all([
        storage.getClientsByWorkspace(context.workspaceId),
        storage.getProjectsByWorkspace(context.workspaceId),
        storage.getTasksByWorkspace(context.workspaceId),
      ]);
      
      parts.push(`\nResumo do workspace:`);
      parts.push(`- ${clients.length} clientes`);
      parts.push(`- ${projects.length} projetos`);
      parts.push(`- ${tasks.filter((t: any) => !t.isCompleted).length} tarefas pendentes`);
      
      if (clients.length > 0 && clients.length <= 10) {
        parts.push(`\nClientes: ${clients.map((c: any) => c.name).join(', ')}`);
      }
      
      if (projects.length > 0 && projects.length <= 10) {
        parts.push(`\nProjetos: ${projects.map((p: any) => p.title).join(', ')}`);
      }
    } catch (error) {
      console.error('Error building context:', error);
    }
    
    return parts.join('\n');
  }

  private parseResponse(response: string): { cleanResponse: string; action?: AiAction } {
    const actionStartIndex = response.indexOf('[ACTION:');
    
    if (actionStartIndex !== -1) {
      const jsonStartIndex = response.indexOf('{', actionStartIndex);
      if (jsonStartIndex !== -1) {
        let braceCount = 0;
        let jsonEndIndex = jsonStartIndex;
        
        for (let i = jsonStartIndex; i < response.length; i++) {
          if (response[i] === '{') braceCount++;
          if (response[i] === '}') braceCount--;
          if (braceCount === 0) {
            jsonEndIndex = i + 1;
            break;
          }
        }
        
        const jsonString = response.substring(jsonStartIndex, jsonEndIndex);
        const actionEndIndex = response.indexOf(']', jsonEndIndex);
        
        try {
          const action = JSON.parse(jsonString) as AiAction;
          const actionFullMatch = response.substring(actionStartIndex, actionEndIndex + 1);
          const cleanResponse = response.replace(actionFullMatch, '').trim();
          console.log('[AI Service] Parsed action:', JSON.stringify(action));
          return { cleanResponse, action };
        } catch (e) {
          console.error('[AI Service] Failed to parse action JSON:', jsonString, e);
          return { cleanResponse: response };
        }
      }
    }
    
    return { cleanResponse: response };
  }

  private async executeAction(action: AiAction, context: ChatContext): Promise<AiAction> {
    console.log('[AI Service] Executing action:', action.type, 'with params:', JSON.stringify(action.params));
    
    try {
      switch (action.type) {
        case 'CREATE_TASK':
          if (action.params?.title) {
            console.log('[AI Service] Creating task:', action.params.title);
            
            const projects = await storage.getProjectsByWorkspace(context.workspaceId);
            if (projects.length === 0) {
              action.result = { success: false, error: 'Nenhum projeto encontrado. Crie um projeto primeiro.' };
              break;
            }
            
            let projectId = null;
            const params = action.params!;
            
            if (params.projectId) {
              const existingProject = projects.find((p: any) => p.id === params.projectId);
              if (existingProject) {
                projectId = existingProject.id;
              }
            }
            
            if (!projectId && params.projectName) {
              const matchingProject = projects.find((p: any) => 
                p.title.toLowerCase().includes(params.projectName.toLowerCase())
              );
              if (matchingProject) {
                projectId = matchingProject.id;
              }
            }
            
            if (!projectId) {
              projectId = projects[0].id;
              console.log('[AI Service] Using default project:', projects[0].title);
            }
            
            const task = await storage.createTask({
              workspaceId: context.workspaceId,
              title: action.params.title,
              description: action.params.description || null,
              projectId: projectId,
              assigneeId: action.params.assigneeId || null,
              dueDate: action.params.dueDate ? new Date(action.params.dueDate) : null,
            });
            console.log('[AI Service] Task created with ID:', task.id);
            action.result = { success: true, taskId: task.id, message: `Tarefa "${action.params.title}" criada com sucesso!` };
          } else {
            action.result = { success: false, error: 'Título da tarefa não fornecido' };
          }
          break;
          
        case 'CREATE_CLIENT':
          if (action.params?.name) {
            console.log('[AI Service] Creating client:', action.params.name);
            const client = await storage.createClient({
              workspaceId: context.workspaceId,
              name: action.params.name,
              email: action.params.email || `${action.params.name.toLowerCase().replace(/\s+/g, '.')}@temp.com`,
              phone: action.params.phone || null,
              companyName: action.params.companyName || null,
            });
            console.log('[AI Service] Client created with ID:', client.id);
            action.result = { success: true, clientId: client.id, message: `Cliente "${action.params.name}" criado com sucesso!` };
          } else {
            action.result = { success: false, error: 'Nome do cliente não fornecido' };
          }
          break;
          
        default:
          action.result = { success: true };
      }
    } catch (error: any) {
      console.error('[AI Service] Error executing action:', error);
      action.result = { success: false, error: error.message };
    }
    
    console.log('[AI Service] Action result:', JSON.stringify(action.result));
    return action;
  }
}

export const aiService = new AiService();
