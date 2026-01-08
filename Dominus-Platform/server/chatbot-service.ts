import OpenAI from 'openai';
import { db } from './storage';
import * as schema from '@shared/schema';
import { eq, and, desc } from 'drizzle-orm';
import { loadChatbotContext, formatContextForAI } from './chatbot-context-service';
import { sendWhatsapp, canUseChatbot } from './whatsapp-service';

let openaiClient: OpenAI | null = null;

function getOpenAIClient(): OpenAI | null {
  if (openaiClient) return openaiClient;
  
  const apiKey = process.env.OPENAI_API_KEY || process.env.AI_INTEGRATIONS_OPENAI_API_KEY;
  if (!apiKey) {
    console.warn('OpenAI API key not configured, chatbot will not work');
    return null;
  }
  
  openaiClient = new OpenAI({ apiKey });
  return openaiClient;
}

const SYSTEM_PROMPT = `Você é um assistente virtual prestativo para um negócio. Seu papel é:
1. Responder perguntas sobre os serviços oferecidos
2. Informar sobre horários de funcionamento e endereço
3. Ajudar clientes a agendar, consultar ou remarcar compromissos
4. Fornecer informações sobre faturas e pagamentos pendentes
5. Ser cordial, profissional e objetivo

Regras importantes:
- Sempre responda em português brasileiro
- Seja conciso e direto (máximo 500 caracteres por resposta)
- Se não souber uma informação, sugira que o cliente entre em contato com a equipe
- Use emojis moderadamente para tornar a conversa amigável
- Para agendamentos, sempre confirme data, horário e serviço antes de finalizar
- Não invente informações que não estejam no contexto fornecido

Quando um cliente quiser agendar:
1. Pergunte qual serviço deseja
2. Mostre as datas e horários disponíveis
3. Confirme o agendamento escolhido
4. Finalize com uma mensagem de confirmação

Você tem acesso às seguintes informações sobre o negócio e o cliente:`;

interface ChatMessage {
  role: 'user' | 'assistant' | 'system';
  content: string;
}

async function getOrCreateSession(
  workspaceId: string,
  clientPhone: string,
  clientId?: string
): Promise<schema.WhatsappChatbotSession> {
  let session = await db.query.whatsappChatbotSessions.findFirst({
    where: and(
      eq(schema.whatsappChatbotSessions.workspaceId, workspaceId),
      eq(schema.whatsappChatbotSessions.phoneNumber, clientPhone),
      eq(schema.whatsappChatbotSessions.isActive, true)
    ),
    orderBy: desc(schema.whatsappChatbotSessions.createdAt),
  });
  
  if (!session) {
    const [newSession] = await db.insert(schema.whatsappChatbotSessions).values({
      workspaceId,
      phoneNumber: clientPhone,
      clientId: clientId || null,
      isActive: true,
      contextData: '[]',
    }).returning();
    session = newSession;
  }
  
  return session;
}

async function getChatHistory(session: schema.WhatsappChatbotSession, limit: number = 10): Promise<ChatMessage[]> {
  if (!session.contextData) {
    return [];
  }
  
  try {
    const history = JSON.parse(session.contextData);
    return Array.isArray(history) ? history.slice(-limit) : [];
  } catch {
    return [];
  }
}

async function updateChatHistory(sessionId: string, messages: ChatMessage[]): Promise<void> {
  await db.update(schema.whatsappChatbotSessions)
    .set({ 
      contextData: JSON.stringify(messages),
      lastMessageAt: new Date(),
      updatedAt: new Date(),
    })
    .where(eq(schema.whatsappChatbotSessions.id, sessionId));
}

export async function processMessage(
  workspaceId: string,
  clientPhone: string,
  incomingMessage: string,
  clientId?: string
): Promise<string> {
  const hasPermission = await canUseChatbot(workspaceId);
  if (!hasPermission) {
    return 'Desculpe, o chatbot automático não está disponível no momento. Por favor, aguarde que nossa equipe entrará em contato.';
  }
  
  const session = await getOrCreateSession(workspaceId, clientPhone, clientId);
  
  const context = await loadChatbotContext(workspaceId, clientPhone);
  const contextStr = formatContextForAI(context);
  
  const chatHistory = await getChatHistory(session);
  
  const messages: ChatMessage[] = [
    { role: 'system', content: `${SYSTEM_PROMPT}\n\n${contextStr}` },
    ...chatHistory,
    { role: 'user', content: incomingMessage }
  ];
  
  try {
    const openai = getOpenAIClient();
    if (!openai) {
      return 'Desculpe, o chatbot automático não está configurado. Por favor, aguarde que nossa equipe entrará em contato.';
    }
    
    const completion = await openai.chat.completions.create({
      model: 'gpt-4o-mini',
      messages: messages.map(m => ({
        role: m.role as 'user' | 'assistant' | 'system',
        content: m.content,
      })),
      max_tokens: 300,
      temperature: 0.7,
    });
    
    const response = completion.choices[0]?.message?.content || 
      'Desculpe, não consegui processar sua mensagem. Por favor, tente novamente.';
    
    const updatedHistory: ChatMessage[] = [
      ...chatHistory,
      { role: 'user', content: incomingMessage },
      { role: 'assistant', content: response }
    ];
    
    await updateChatHistory(session.id, updatedHistory.slice(-20));
    
    return response;
  } catch (error: any) {
    console.error('Chatbot error:', error);
    
    if (error.code === 'insufficient_quota' || error.status === 429) {
      return 'Estamos com alta demanda no momento. Por favor, tente novamente em alguns minutos ou aguarde nosso retorno.';
    }
    
    return 'Desculpe, tive um problema ao processar sua mensagem. Nossa equipe será notificada para ajudá-lo.';
  }
}

export async function handleIncomingWhatsapp(
  workspaceId: string,
  clientPhone: string,
  message: string,
  clientId?: string
): Promise<{ response: string; shouldReply: boolean }> {
  const whatsappSettings = await db.query.whatsappSettings.findFirst({
    where: eq(schema.whatsappSettings.workspaceId, workspaceId),
  });
  
  if (!whatsappSettings?.chatbotEnabled) {
    return { response: '', shouldReply: false };
  }
  
  const response = await processMessage(workspaceId, clientPhone, message, clientId);
  
  return { response, shouldReply: true };
}

export async function sendChatbotReply(
  workspaceId: string,
  clientPhone: string,
  message: string
): Promise<{ success: boolean; error?: string }> {
  return await sendWhatsapp({
    to: clientPhone,
    message,
    workspaceId,
  });
}

export async function endChatbotSession(sessionId: string): Promise<void> {
  await db.update(schema.whatsappChatbotSessions)
    .set({ isActive: false, updatedAt: new Date() })
    .where(eq(schema.whatsappChatbotSessions.id, sessionId));
}
