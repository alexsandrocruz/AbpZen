import { db } from './storage';
import { emailTemplates } from '@shared/schema';
import { sql } from 'drizzle-orm';

interface EmailTemplateSeed {
  name: string;
  subject: string;
  htmlContent: string;
  textContent: string;
  triggerType: 'WELCOME' | 'PASSWORD_RESET' | 'PROPOSAL_SENT' | 'PROPOSAL_VIEWED' | 'PROPOSAL_ACCEPTED' | 'PROPOSAL_REJECTED' | 'CONTRACT_SENT' | 'CONTRACT_SIGNED' | 'INVOICE_SENT' | 'INVOICE_PAID' | 'INVOICE_OVERDUE' | 'TASK_ASSIGNED' | 'TASK_COMPLETED' | 'PROJECT_CREATED' | 'PROJECT_COMPLETED' | 'DOCUMENT_EXPIRING' | 'BOOKING_CONFIRMED' | 'BOOKING_REMINDER' | 'CUSTOM';
  isSystem: boolean;
}

const baseStyle = `
  <style>
    body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; }
    .container { max-width: 600px; margin: 0 auto; padding: 20px; }
    .header { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 8px 8px 0 0; }
    .content { background: #ffffff; padding: 30px; border: 1px solid #e5e7eb; }
    .footer { background: #f9fafb; padding: 20px; text-align: center; font-size: 12px; color: #6b7280; border-radius: 0 0 8px 8px; border: 1px solid #e5e7eb; border-top: none; }
    .button { display: inline-block; background: #667eea; color: white; padding: 12px 24px; text-decoration: none; border-radius: 6px; margin: 20px 0; }
    .button:hover { background: #5a67d8; }
    h1 { margin: 0; font-size: 24px; }
    .highlight { background: #f3f4f6; padding: 15px; border-radius: 6px; margin: 15px 0; }
  </style>
`;

const systemEmailTemplates: EmailTemplateSeed[] = [
  {
    name: 'Boas-vindas',
    subject: 'Bem-vindo(a) à {{workspace}}!',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header"><h1>Bem-vindo(a)!</h1></div>
        <div class="content">
          <p>Olá <strong>{{nome}}</strong>,</p>
          <p>É um prazer tê-lo(a) conosco! Sua conta foi criada com sucesso na <strong>{{workspace}}</strong>.</p>
          <p>Agora você pode acessar todos os recursos disponíveis para gerenciar seus projetos, propostas e muito mais.</p>
          <a href="{{link}}" class="button">Acessar Minha Conta</a>
          <p>Se tiver alguma dúvida, estamos à disposição para ajudar.</p>
        </div>
        <div class="footer">
          <p>{{workspace}} - Gestão Simplificada</p>
          <p>Este é um email automático, por favor não responda.</p>
        </div>
      </div>
    </body></html>`,
    textContent: `Olá {{nome}},

Bem-vindo(a) à {{workspace}}!

Sua conta foi criada com sucesso. Agora você pode acessar todos os recursos disponíveis.

Acesse sua conta: {{link}}

Atenciosamente,
{{workspace}}`,
    triggerType: 'WELCOME',
    isSystem: true
  },
  {
    name: 'Redefinição de Senha',
    subject: 'Redefinir sua senha - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header"><h1>Redefinição de Senha</h1></div>
        <div class="content">
          <p>Olá <strong>{{nome}}</strong>,</p>
          <p>Recebemos uma solicitação para redefinir a senha da sua conta.</p>
          <p>Clique no botão abaixo para criar uma nova senha:</p>
          <a href="{{link}}" class="button">Redefinir Senha</a>
          <div class="highlight">
            <p><strong>Importante:</strong> Este link expira em 1 hora.</p>
            <p>Se você não solicitou esta alteração, ignore este email.</p>
          </div>
        </div>
        <div class="footer">
          <p>{{workspace}} - Gestão Simplificada</p>
          <p>Este é um email automático, por favor não responda.</p>
        </div>
      </div>
    </body></html>`,
    textContent: `Olá {{nome}},

Recebemos uma solicitação para redefinir sua senha.

Clique no link abaixo para criar uma nova senha:
{{link}}

Este link expira em 1 hora.

Se você não solicitou esta alteração, ignore este email.

Atenciosamente,
{{workspace}}`,
    triggerType: 'PASSWORD_RESET',
    isSystem: true
  },
  {
    name: 'Proposta Enviada',
    subject: 'Nova proposta: {{proposta_titulo}} - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header"><h1>Nova Proposta</h1></div>
        <div class="content">
          <p>Olá <strong>{{nome}}</strong>,</p>
          <p>Você recebeu uma nova proposta comercial de <strong>{{workspace}}</strong>.</p>
          <div class="highlight">
            <p><strong>Proposta:</strong> {{proposta_titulo}}</p>
            <p><strong>Valor:</strong> {{proposta_valor}}</p>
            <p><strong>Válida até:</strong> {{proposta_validade}}</p>
          </div>
          <p>Clique no botão abaixo para visualizar todos os detalhes:</p>
          <a href="{{link}}" class="button">Ver Proposta</a>
        </div>
        <div class="footer">
          <p>{{workspace}}</p>
          <p>Este é um email automático, por favor não responda.</p>
        </div>
      </div>
    </body></html>`,
    textContent: `Olá {{nome}},

Você recebeu uma nova proposta comercial de {{workspace}}.

Proposta: {{proposta_titulo}}
Valor: {{proposta_valor}}
Válida até: {{proposta_validade}}

Visualize a proposta: {{link}}

Atenciosamente,
{{workspace}}`,
    triggerType: 'PROPOSAL_SENT',
    isSystem: true
  },
  {
    name: 'Proposta Visualizada',
    subject: '{{nome}} visualizou sua proposta - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header"><h1>Proposta Visualizada</h1></div>
        <div class="content">
          <p>Boas notícias!</p>
          <p>O cliente <strong>{{nome}}</strong> acabou de visualizar sua proposta.</p>
          <div class="highlight">
            <p><strong>Proposta:</strong> {{proposta_titulo}}</p>
            <p><strong>Data/Hora:</strong> {{data}}</p>
          </div>
          <p>Este é um bom momento para fazer um follow-up!</p>
          <a href="{{link}}" class="button">Ver Proposta</a>
        </div>
        <div class="footer">
          <p>{{workspace}}</p>
        </div>
      </div>
    </body></html>`,
    textContent: `Boas notícias!

O cliente {{nome}} acabou de visualizar sua proposta.

Proposta: {{proposta_titulo}}
Data/Hora: {{data}}

Este é um bom momento para fazer um follow-up!

Ver proposta: {{link}}`,
    triggerType: 'PROPOSAL_VIEWED',
    isSystem: true
  },
  {
    name: 'Proposta Aceita',
    subject: 'Proposta aceita por {{nome}} - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header" style="background: linear-gradient(135deg, #10b981 0%, #059669 100%);">
          <h1>Proposta Aceita!</h1>
        </div>
        <div class="content">
          <p>Parabéns! 🎉</p>
          <p>O cliente <strong>{{nome}}</strong> aceitou sua proposta!</p>
          <div class="highlight">
            <p><strong>Proposta:</strong> {{proposta_titulo}}</p>
            <p><strong>Valor:</strong> {{proposta_valor}}</p>
            <p><strong>Data de aceite:</strong> {{data}}</p>
          </div>
          <p>Próximos passos: envie o contrato e comece o projeto!</p>
          <a href="{{link}}" class="button" style="background: #10b981;">Ver Detalhes</a>
        </div>
        <div class="footer">
          <p>{{workspace}}</p>
        </div>
      </div>
    </body></html>`,
    textContent: `Parabéns!

O cliente {{nome}} aceitou sua proposta!

Proposta: {{proposta_titulo}}
Valor: {{proposta_valor}}
Data de aceite: {{data}}

Próximos passos: envie o contrato e comece o projeto!

Ver detalhes: {{link}}`,
    triggerType: 'PROPOSAL_ACCEPTED',
    isSystem: true
  },
  {
    name: 'Proposta Rejeitada',
    subject: 'Proposta não aceita: {{proposta_titulo}} - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header" style="background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%);">
          <h1>Proposta Não Aceita</h1>
        </div>
        <div class="content">
          <p>Infelizmente, o cliente <strong>{{nome}}</strong> optou por não aceitar a proposta.</p>
          <div class="highlight">
            <p><strong>Proposta:</strong> {{proposta_titulo}}</p>
            <p><strong>Data:</strong> {{data}}</p>
            <p><strong>Motivo:</strong> {{motivo}}</p>
          </div>
          <p>Não desanime! Use este feedback para melhorar suas próximas propostas.</p>
          <a href="{{link}}" class="button">Ver Proposta</a>
        </div>
        <div class="footer">
          <p>{{workspace}}</p>
        </div>
      </div>
    </body></html>`,
    textContent: `O cliente {{nome}} optou por não aceitar a proposta.

Proposta: {{proposta_titulo}}
Data: {{data}}
Motivo: {{motivo}}

Use este feedback para melhorar suas próximas propostas.

Ver proposta: {{link}}`,
    triggerType: 'PROPOSAL_REJECTED',
    isSystem: true
  },
  {
    name: 'Contrato Enviado',
    subject: 'Contrato para assinatura: {{contrato_titulo}} - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header"><h1>Contrato para Assinatura</h1></div>
        <div class="content">
          <p>Olá <strong>{{nome}}</strong>,</p>
          <p>Seu contrato está pronto para assinatura!</p>
          <div class="highlight">
            <p><strong>Contrato:</strong> {{contrato_titulo}}</p>
            <p><strong>Valor:</strong> {{contrato_valor}}</p>
            <p><strong>Parcelas:</strong> {{contrato_parcelas}}</p>
          </div>
          <p>Clique no botão abaixo para revisar e assinar o contrato:</p>
          <a href="{{link}}" class="button">Assinar Contrato</a>
        </div>
        <div class="footer">
          <p>{{workspace}}</p>
          <p>Este é um email automático, por favor não responda.</p>
        </div>
      </div>
    </body></html>`,
    textContent: `Olá {{nome}},

Seu contrato está pronto para assinatura!

Contrato: {{contrato_titulo}}
Valor: {{contrato_valor}}
Parcelas: {{contrato_parcelas}}

Revise e assine: {{link}}

Atenciosamente,
{{workspace}}`,
    triggerType: 'CONTRACT_SENT',
    isSystem: true
  },
  {
    name: 'Contrato Assinado',
    subject: 'Contrato assinado: {{contrato_titulo}} - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header" style="background: linear-gradient(135deg, #10b981 0%, #059669 100%);">
          <h1>Contrato Assinado!</h1>
        </div>
        <div class="content">
          <p>O contrato foi assinado com sucesso! 🎉</p>
          <div class="highlight">
            <p><strong>Contrato:</strong> {{contrato_titulo}}</p>
            <p><strong>Assinado por:</strong> {{nome}}</p>
            <p><strong>Data:</strong> {{data}}</p>
          </div>
          <p>Uma cópia do contrato assinado está disponível para download.</p>
          <a href="{{link}}" class="button" style="background: #10b981;">Ver Contrato</a>
        </div>
        <div class="footer">
          <p>{{workspace}}</p>
        </div>
      </div>
    </body></html>`,
    textContent: `O contrato foi assinado com sucesso!

Contrato: {{contrato_titulo}}
Assinado por: {{nome}}
Data: {{data}}

Ver contrato: {{link}}

{{workspace}}`,
    triggerType: 'CONTRACT_SIGNED',
    isSystem: true
  },
  {
    name: 'Fatura Enviada',
    subject: 'Fatura #{{fatura_numero}} - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header"><h1>Nova Fatura</h1></div>
        <div class="content">
          <p>Olá <strong>{{nome}}</strong>,</p>
          <p>Sua fatura está disponível para pagamento.</p>
          <div class="highlight">
            <p><strong>Fatura:</strong> #{{fatura_numero}}</p>
            <p><strong>Valor:</strong> {{fatura_valor}}</p>
            <p><strong>Vencimento:</strong> {{fatura_vencimento}}</p>
          </div>
          <a href="{{link}}" class="button">Ver Fatura</a>
        </div>
        <div class="footer">
          <p>{{workspace}}</p>
          <p>Este é um email automático, por favor não responda.</p>
        </div>
      </div>
    </body></html>`,
    textContent: `Olá {{nome}},

Sua fatura está disponível para pagamento.

Fatura: #{{fatura_numero}}
Valor: {{fatura_valor}}
Vencimento: {{fatura_vencimento}}

Ver fatura: {{link}}

Atenciosamente,
{{workspace}}`,
    triggerType: 'INVOICE_SENT',
    isSystem: true
  },
  {
    name: 'Fatura Paga',
    subject: 'Pagamento confirmado - Fatura #{{fatura_numero}} - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header" style="background: linear-gradient(135deg, #10b981 0%, #059669 100%);">
          <h1>Pagamento Confirmado!</h1>
        </div>
        <div class="content">
          <p>Olá <strong>{{nome}}</strong>,</p>
          <p>Recebemos seu pagamento. Obrigado! 🎉</p>
          <div class="highlight">
            <p><strong>Fatura:</strong> #{{fatura_numero}}</p>
            <p><strong>Valor pago:</strong> {{fatura_valor}}</p>
            <p><strong>Data do pagamento:</strong> {{data}}</p>
          </div>
          <a href="{{link}}" class="button" style="background: #10b981;">Ver Recibo</a>
        </div>
        <div class="footer">
          <p>{{workspace}}</p>
        </div>
      </div>
    </body></html>`,
    textContent: `Olá {{nome}},

Recebemos seu pagamento. Obrigado!

Fatura: #{{fatura_numero}}
Valor pago: {{fatura_valor}}
Data do pagamento: {{data}}

Ver recibo: {{link}}

{{workspace}}`,
    triggerType: 'INVOICE_PAID',
    isSystem: true
  },
  {
    name: 'Fatura Vencida',
    subject: 'Fatura vencida - #{{fatura_numero}} - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header" style="background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);">
          <h1>Fatura Vencida</h1>
        </div>
        <div class="content">
          <p>Olá <strong>{{nome}}</strong>,</p>
          <p>Identificamos que a fatura abaixo está vencida:</p>
          <div class="highlight">
            <p><strong>Fatura:</strong> #{{fatura_numero}}</p>
            <p><strong>Valor:</strong> {{fatura_valor}}</p>
            <p><strong>Vencimento:</strong> {{fatura_vencimento}}</p>
            <p><strong>Dias em atraso:</strong> {{dias_atraso}}</p>
          </div>
          <p>Por favor, regularize sua situação o mais breve possível.</p>
          <a href="{{link}}" class="button" style="background: #f59e0b;">Pagar Agora</a>
        </div>
        <div class="footer">
          <p>{{workspace}}</p>
          <p>Dúvidas? Entre em contato conosco.</p>
        </div>
      </div>
    </body></html>`,
    textContent: `Olá {{nome}},

Identificamos que a fatura abaixo está vencida:

Fatura: #{{fatura_numero}}
Valor: {{fatura_valor}}
Vencimento: {{fatura_vencimento}}
Dias em atraso: {{dias_atraso}}

Por favor, regularize sua situação: {{link}}

{{workspace}}`,
    triggerType: 'INVOICE_OVERDUE',
    isSystem: true
  },
  {
    name: 'Tarefa Atribuída',
    subject: 'Nova tarefa: {{tarefa_titulo}} - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header"><h1>Nova Tarefa</h1></div>
        <div class="content">
          <p>Olá <strong>{{nome}}</strong>,</p>
          <p>Uma nova tarefa foi atribuída a você:</p>
          <div class="highlight">
            <p><strong>Tarefa:</strong> {{tarefa_titulo}}</p>
            <p><strong>Projeto:</strong> {{projeto_nome}}</p>
            <p><strong>Prazo:</strong> {{tarefa_prazo}}</p>
            <p><strong>Prioridade:</strong> {{tarefa_prioridade}}</p>
          </div>
          <a href="{{link}}" class="button">Ver Tarefa</a>
        </div>
        <div class="footer">
          <p>{{workspace}}</p>
        </div>
      </div>
    </body></html>`,
    textContent: `Olá {{nome}},

Uma nova tarefa foi atribuída a você:

Tarefa: {{tarefa_titulo}}
Projeto: {{projeto_nome}}
Prazo: {{tarefa_prazo}}
Prioridade: {{tarefa_prioridade}}

Ver tarefa: {{link}}

{{workspace}}`,
    triggerType: 'TASK_ASSIGNED',
    isSystem: true
  },
  {
    name: 'Tarefa Concluída',
    subject: 'Tarefa concluída: {{tarefa_titulo}} - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header" style="background: linear-gradient(135deg, #10b981 0%, #059669 100%);">
          <h1>Tarefa Concluída!</h1>
        </div>
        <div class="content">
          <p>A tarefa foi marcada como concluída:</p>
          <div class="highlight">
            <p><strong>Tarefa:</strong> {{tarefa_titulo}}</p>
            <p><strong>Projeto:</strong> {{projeto_nome}}</p>
            <p><strong>Concluída por:</strong> {{nome}}</p>
            <p><strong>Data:</strong> {{data}}</p>
          </div>
          <a href="{{link}}" class="button" style="background: #10b981;">Ver Detalhes</a>
        </div>
        <div class="footer">
          <p>{{workspace}}</p>
        </div>
      </div>
    </body></html>`,
    textContent: `A tarefa foi marcada como concluída:

Tarefa: {{tarefa_titulo}}
Projeto: {{projeto_nome}}
Concluída por: {{nome}}
Data: {{data}}

Ver detalhes: {{link}}

{{workspace}}`,
    triggerType: 'TASK_COMPLETED',
    isSystem: true
  },
  {
    name: 'Projeto Criado',
    subject: 'Novo projeto: {{projeto_nome}} - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header"><h1>Novo Projeto</h1></div>
        <div class="content">
          <p>Olá <strong>{{nome}}</strong>,</p>
          <p>Um novo projeto foi criado para você:</p>
          <div class="highlight">
            <p><strong>Projeto:</strong> {{projeto_nome}}</p>
            <p><strong>Cliente:</strong> {{cliente_nome}}</p>
            <p><strong>Início:</strong> {{projeto_inicio}}</p>
            <p><strong>Previsão:</strong> {{projeto_fim}}</p>
          </div>
          <a href="{{link}}" class="button">Ver Projeto</a>
        </div>
        <div class="footer">
          <p>{{workspace}}</p>
        </div>
      </div>
    </body></html>`,
    textContent: `Olá {{nome}},

Um novo projeto foi criado para você:

Projeto: {{projeto_nome}}
Cliente: {{cliente_nome}}
Início: {{projeto_inicio}}
Previsão: {{projeto_fim}}

Ver projeto: {{link}}

{{workspace}}`,
    triggerType: 'PROJECT_CREATED',
    isSystem: true
  },
  {
    name: 'Projeto Concluído',
    subject: 'Projeto concluído: {{projeto_nome}} - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header" style="background: linear-gradient(135deg, #10b981 0%, #059669 100%);">
          <h1>Projeto Concluído!</h1>
        </div>
        <div class="content">
          <p>Parabéns! 🎉</p>
          <p>O projeto foi concluído com sucesso:</p>
          <div class="highlight">
            <p><strong>Projeto:</strong> {{projeto_nome}}</p>
            <p><strong>Cliente:</strong> {{cliente_nome}}</p>
            <p><strong>Data de conclusão:</strong> {{data}}</p>
          </div>
          <a href="{{link}}" class="button" style="background: #10b981;">Ver Relatório</a>
        </div>
        <div class="footer">
          <p>{{workspace}}</p>
        </div>
      </div>
    </body></html>`,
    textContent: `Parabéns!

O projeto foi concluído com sucesso:

Projeto: {{projeto_nome}}
Cliente: {{cliente_nome}}
Data de conclusão: {{data}}

Ver relatório: {{link}}

{{workspace}}`,
    triggerType: 'PROJECT_COMPLETED',
    isSystem: true
  },
  {
    name: 'Documento Expirando',
    subject: 'Documento expirando: {{documento_nome}} - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header" style="background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);">
          <h1>Documento Expirando</h1>
        </div>
        <div class="content">
          <p>Olá <strong>{{nome}}</strong>,</p>
          <p>Um documento está próximo do vencimento:</p>
          <div class="highlight">
            <p><strong>Documento:</strong> {{documento_nome}}</p>
            <p><strong>Tipo:</strong> {{documento_tipo}}</p>
            <p><strong>Expira em:</strong> {{documento_expiracao}}</p>
          </div>
          <p>Por favor, tome as providências necessárias.</p>
          <a href="{{link}}" class="button" style="background: #f59e0b;">Ver Documento</a>
        </div>
        <div class="footer">
          <p>{{workspace}}</p>
        </div>
      </div>
    </body></html>`,
    textContent: `Olá {{nome}},

Um documento está próximo do vencimento:

Documento: {{documento_nome}}
Tipo: {{documento_tipo}}
Expira em: {{documento_expiracao}}

Ver documento: {{link}}

{{workspace}}`,
    triggerType: 'DOCUMENT_EXPIRING',
    isSystem: true
  },
  {
    name: 'Agendamento Confirmado',
    subject: 'Agendamento confirmado para {{data}} - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header" style="background: linear-gradient(135deg, #10b981 0%, #059669 100%);">
          <h1>Agendamento Confirmado!</h1>
        </div>
        <div class="content">
          <p>Olá <strong>{{nome}}</strong>,</p>
          <p>Seu agendamento foi confirmado com sucesso!</p>
          <div class="highlight">
            <p><strong>Serviço:</strong> {{servico_nome}}</p>
            <p><strong>Data:</strong> {{data}}</p>
            <p><strong>Horário:</strong> {{horario}}</p>
            <p><strong>Local:</strong> {{local}}</p>
          </div>
          <p>Adicione ao seu calendário para não esquecer!</p>
          <a href="{{link}}" class="button" style="background: #10b981;">Ver Agendamento</a>
        </div>
        <div class="footer">
          <p>{{workspace}}</p>
        </div>
      </div>
    </body></html>`,
    textContent: `Olá {{nome}},

Seu agendamento foi confirmado!

Serviço: {{servico_nome}}
Data: {{data}}
Horário: {{horario}}
Local: {{local}}

Ver agendamento: {{link}}

{{workspace}}`,
    triggerType: 'BOOKING_CONFIRMED',
    isSystem: true
  },
  {
    name: 'Lembrete de Agendamento',
    subject: 'Lembrete: Agendamento amanhã - {{workspace}}',
    htmlContent: `<!DOCTYPE html><html><head>${baseStyle}</head><body>
      <div class="container">
        <div class="header"><h1>Lembrete de Agendamento</h1></div>
        <div class="content">
          <p>Olá <strong>{{nome}}</strong>,</p>
          <p>Este é um lembrete do seu agendamento:</p>
          <div class="highlight">
            <p><strong>Serviço:</strong> {{servico_nome}}</p>
            <p><strong>Data:</strong> {{data}}</p>
            <p><strong>Horário:</strong> {{horario}}</p>
            <p><strong>Local:</strong> {{local}}</p>
          </div>
          <p>Estamos aguardando você!</p>
          <a href="{{link}}" class="button">Ver Detalhes</a>
        </div>
        <div class="footer">
          <p>{{workspace}}</p>
          <p>Precisa remarcar? Entre em contato conosco.</p>
        </div>
      </div>
    </body></html>`,
    textContent: `Olá {{nome}},

Lembrete do seu agendamento:

Serviço: {{servico_nome}}
Data: {{data}}
Horário: {{horario}}
Local: {{local}}

Ver detalhes: {{link}}

{{workspace}}`,
    triggerType: 'BOOKING_REMINDER',
    isSystem: true
  }
];

export async function seedEmailTemplates() {
  console.log('Seeding system email templates...');
  
  for (const template of systemEmailTemplates) {
    const existing = await db
      .select()
      .from(emailTemplates)
      .where(
        sql`${emailTemplates.triggerType} = ${template.triggerType} AND ${emailTemplates.isSystem} = true`
      )
      .limit(1);
    
    if (existing.length === 0) {
      await db.insert(emailTemplates).values({
        workspaceId: null,
        name: template.name,
        subject: template.subject,
        htmlContent: template.htmlContent,
        textContent: template.textContent,
        triggerType: template.triggerType,
        isSystem: true,
        isActive: true,
      });
      console.log(`  Created: ${template.name} (${template.triggerType})`);
    } else {
      console.log(`  Skipped: ${template.name} (already exists)`);
    }
  }
  
  console.log('Email templates seed completed!');
}

if (import.meta.url === `file://${process.argv[1]}`) {
  seedEmailTemplates()
    .then(() => process.exit(0))
    .catch((err) => {
      console.error('Seed failed:', err);
      process.exit(1);
    });
}
