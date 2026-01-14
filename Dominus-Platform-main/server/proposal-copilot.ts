import OpenAI from "openai";

const openai = new OpenAI({
  apiKey: process.env.AI_INTEGRATIONS_OPENAI_API_KEY,
  baseURL: process.env.AI_INTEGRATIONS_OPENAI_BASE_URL,
});

export interface ExtractedEntities {
  clientName: string | null;
  clientNameVariants: string[];
  serviceName: string | null;
  serviceDescription: string | null;
  value: number | null;
  deadline: string | null;
  observations: string | null;
}

export async function extractProposalEntities(text: string): Promise<ExtractedEntities> {
  const response = await openai.chat.completions.create({
    model: "gpt-4o",
    messages: [
      {
        role: "system",
        content: `Você é um assistente especializado em extrair informações de textos para criar propostas comerciais.
Analise o texto do usuário e extraia as seguintes informações:
- clientName: nome do cliente mencionado (pessoa ou empresa)
- clientNameVariants: variantes do nome para busca (ex: se "Fabio Ribeiro", gere ["Fabio Ribeiro", "RIBEIRO", "Fabio"])
- serviceName: nome do serviço/produto mencionado
- serviceDescription: descrição detalhada do serviço
- value: valor em reais se mencionado (número, sem R$)
- deadline: prazo de entrega se mencionado
- observations: outras observações relevantes

Responda APENAS em JSON válido, sem markdown.`
      },
      {
        role: "user",
        content: text
      }
    ],
    response_format: { type: "json_object" },
    max_completion_tokens: 1000,
  });

  const content = response.choices[0]?.message?.content || "{}";
  try {
    const parsed = JSON.parse(content);
    return {
      clientName: parsed.clientName || null,
      clientNameVariants: parsed.clientNameVariants || [],
      serviceName: parsed.serviceName || null,
      serviceDescription: parsed.serviceDescription || null,
      value: parsed.value ? Number(parsed.value) : null,
      deadline: parsed.deadline || null,
      observations: parsed.observations || null,
    };
  } catch {
    return {
      clientName: null,
      clientNameVariants: [],
      serviceName: null,
      serviceDescription: null,
      value: null,
      deadline: null,
      observations: null,
    };
  }
}

export interface CopilotSuggestion {
  type: 'client_found' | 'client_suggestions' | 'client_not_found' | 'product_found' | 'product_suggestions' | 'product_not_found' | 'ready' | 'error';
  message: string;
  data?: any;
  needsConfirmation?: boolean;
  confirmAction?: string;
}

export interface CopilotState {
  extractedEntities: ExtractedEntities;
  selectedClientId?: string;
  selectedProductId?: string;
  proposalData: {
    clientId?: string;
    clientName?: string;
    items: Array<{
      name: string;
      description: string;
      price: number;
      quantity: number;
    }>;
    totalValue?: number;
  };
  step: 'extracting' | 'confirm_client' | 'confirm_product' | 'ready';
}
