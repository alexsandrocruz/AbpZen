import OpenAI from "openai";

const openai = new OpenAI({
  apiKey: process.env.AI_INTEGRATIONS_OPENAI_API_KEY,
  baseURL: process.env.AI_INTEGRATIONS_OPENAI_BASE_URL,
});

export interface ExtractedTask {
  title: string;
  description?: string;
  priority?: 'LOW' | 'MEDIUM' | 'HIGH';
  estimatedMinutes?: number;
}

export interface ParseTasksResult {
  tasks: ExtractedTask[];
  summary?: string;
}

export async function extractTasksFromText(text: string): Promise<ParseTasksResult> {
  const systemPrompt = `Você é um assistente especializado em extrair tarefas acionáveis de textos.
Analise o texto fornecido e identifique itens que podem ser transformados em tarefas de projeto.

Regras:
1. Identifique passos, ações, etapas ou itens de lista que representam trabalho a ser feito
2. Cada tarefa deve ter um título claro e conciso (máximo 100 caracteres)
3. Inclua uma descrição resumida se houver detalhes importantes
4. Atribua prioridade baseada na ordem ou importância implícita
5. Estime tempo em minutos quando possível (15, 30, 60, 120, etc)
6. Ignore textos introdutórios, explicações ou contexto que não sejam ações

Responda APENAS em JSON válido com a estrutura:
{
  "tasks": [
    {
      "title": "string",
      "description": "string ou null",
      "priority": "LOW" | "MEDIUM" | "HIGH",
      "estimatedMinutes": number ou null
    }
  ],
  "summary": "resumo breve do contexto geral"
}`;

  const response = await openai.chat.completions.create({
    model: "gpt-4o-mini",
    messages: [
      { role: "system", content: systemPrompt },
      { role: "user", content: text }
    ],
    response_format: { type: "json_object" },
    max_completion_tokens: 4096,
  });

  const content = response.choices[0]?.message?.content;
  if (!content) {
    throw new Error("Não foi possível extrair tarefas do texto");
  }

  try {
    const result = JSON.parse(content) as ParseTasksResult;
    return result;
  } catch (e) {
    throw new Error("Erro ao processar resposta da IA");
  }
}
