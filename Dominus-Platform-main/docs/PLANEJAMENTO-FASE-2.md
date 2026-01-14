# Dominus - Planejamento Fase 2

## Visão Geral
Expansão do sistema Dominus com módulos financeiros avançados, gestão de contratos, melhorias no cadastro de clientes e recursos de produtividade.

---

## 1. Comentários em Tarefas
- Adicionar sistema de comentários nas tarefas
- Cada comentário com autor, data/hora e texto
- Lista de comentários ordenada cronologicamente
- Contador de comentários no card da tarefa

## 2. Contratos Embutidos nas Propostas
- Campo de texto rico para termos do contrato
- Área de assinatura digital
- Status de assinatura (Pendente/Assinado)
- Data e hora da assinatura

## 3. Registro de Tempo Manual
- Interface para registrar horas trabalhadas
- Campos: projeto, tarefa, descrição, data, duração
- Visualização por dia/semana/mês
- Totalizador de horas por projeto

## 4. Dashboard com Métricas
- Cards com indicadores principais:
  - Receita total do mês
  - Despesas do mês
  - Saldo atual
  - Projetos ativos
  - Tarefas pendentes
  - Faturas em aberto
- Gráficos visuais com Recharts

## 5. Melhorias Visuais
- Refinamento da interface
- Animações suaves
- Feedback visual nas ações
- Responsividade aprimorada

---

## 6. Módulo Financeiro Completo

### 6.1 Contas a Receber (Receitas)
- Cadastro de receitas com categorias
- Status: Pendente, Recebido, Atrasado
- Campos: valor, data vencimento, data recebimento, categoria, descrição
- Upload de anexos: comprovantes, notas fiscais, recibos
- Vínculo opcional com fatura/proposta

### 6.2 Contas a Pagar (Despesas)
- Cadastro de despesas com categorias
- Status: Pendente, Pago, Atrasado
- Campos: valor, data vencimento, data pagamento, categoria, fornecedor
- Upload de anexos: notas fiscais, recibos, comprovantes
- Recorrência opcional (mensal, anual)

### 6.3 Categorias Financeiras
- Cadastro de categorias para receitas
- Cadastro de categorias para despesas
- Cores para identificação visual

### 6.4 Orçamento/Budget
- Definir orçamento mensal por categoria
- Comparativo orçado vs realizado
- Alertas quando ultrapassar limite

### 6.5 Fluxo de Caixa
- Gráfico de linhas: receitas vs despesas ao longo do tempo
- Previsão de fluxo de caixa futuro (baseado em contas a pagar/receber)
- Saldo projetado

### 6.6 Relatórios
- Gráfico de origem das receitas (por categoria/cliente)
- Gráfico de distribuição de despesas
- DRE simplificado (Receitas - Despesas = Resultado)

---

## 7. Sistema de Orçamentos Aprimorado

### 7.1 Cadastro de Produtos e Serviços
- Tabela de produtos/serviços pré-cadastrados
- Campos: nome, descrição, preço unitário, unidade (hora, unidade, projeto)
- Categorias de produtos/serviços

### 7.2 Orçamento Rápido
- Selecionar produtos/serviços do catálogo
- Adicionar quantidade
- Cálculo automático de totais
- Desconto por item ou global

### 7.3 Versão PDF do Orçamento
- Geração de PDF profissional
- Logo da empresa
- Dados do cliente
- Itens detalhados
- Termos e condições
- Validade do orçamento

### 7.4 Visualização pelo Cliente
- Link público para cliente visualizar proposta
- Aceite/assinatura online
- Histórico de visualizações

---

## 8. Módulo de Contratos

### 8.1 Templates de Contratos
- Cadastro de modelos de contrato
- Variáveis dinâmicas: {{cliente.nome}}, {{cliente.cpf}}, {{projeto.valor}}, etc.
- Editor de texto rico

### 8.2 Geração de Contratos
- Selecionar template
- Selecionar cliente
- Preenchimento automático das variáveis
- Edição manual se necessário
- Geração de PDF

### 8.3 Assinatura Digital
- Envio por link para cliente
- Área de assinatura
- Registro de IP, data/hora
- Status: Pendente, Assinado, Expirado

---

## 9. Cadastro de Clientes Aprimorado

### 9.1 Tipo de Cliente
- Pessoa Física ou Pessoa Jurídica
- Campo "empresaId" para vincular pessoa a empresa

### 9.2 Dados de Pessoa Física
- Nome completo
- CPF
- RG
- Data de nascimento
- Endereço completo

### 9.3 Dados de Pessoa Jurídica
- Razão social
- Nome fantasia
- CNPJ
- Inscrição estadual
- Inscrição municipal
- Endereço completo

### 9.4 Contatos Múltiplos
- Lista de contatos por cliente/empresa
- Campos: nome, cargo, email, telefone, WhatsApp
- Marcar contato principal
- Notas sobre cada contato

### 9.5 Dados Fiscais/Financeiros
- Banco, agência, conta
- Chave PIX
- Forma de pagamento preferida
- Prazo de pagamento padrão

### 9.6 Foto/Logo do Cliente
- Upload de imagem
- Armazenamento local ou Object Storage
- Exibição no perfil e nos documentos

### 9.7 Visualização de Contatos da Empresa
- Quando cliente é empresa, mostrar lista de pessoas vinculadas
- Acesso rápido aos contatos

---

## Modelo de Dados (Resumo)

### Novas Tabelas
- `financial_categories` - Categorias financeiras
- `transactions` - Receitas e despesas
- `transaction_attachments` - Anexos das transações
- `budgets` - Orçamentos por categoria/mês
- `products` - Produtos e serviços cadastrados
- `contract_templates` - Templates de contratos
- `contracts` - Contratos gerados
- `client_contacts` - Contatos adicionais do cliente

### Alterações em Tabelas Existentes
- `clients` - Adicionar campos fiscais, tipo (PF/PJ), empresaId, foto
- `proposals` - Adicionar campos de assinatura, token público, validade

---

## Prioridade de Implementação

### Fase 2A - Base
1. Atualizar cadastro de clientes (PF/PJ, dados fiscais, contatos)
2. Upload de foto/logo do cliente
3. Comentários em tarefas
4. Registro de tempo

### Fase 2B - Financeiro
5. Categorias financeiras
6. Contas a receber com anexos
7. Contas a pagar com anexos
8. Dashboard financeiro com gráficos

### Fase 2C - Orçamentos e Contratos
9. Cadastro de produtos/serviços
10. Orçamento rápido com catálogo
11. Templates de contratos
12. Geração de PDF (orçamentos e contratos)
13. Visualização/assinatura pelo cliente

### Fase 2D - Avançado
14. Fluxo de caixa com previsão
15. Orçamento/budget por categoria
16. Melhorias visuais e UX

---

## Tecnologias Adicionais Necessárias
- Upload de arquivos (Object Storage ou local)
- Geração de PDF (jspdf ou html2pdf)
- Editor de texto rico (opcional para contratos)
- Gráficos (Recharts - já instalado)
