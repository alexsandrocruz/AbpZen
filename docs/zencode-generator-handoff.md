# Prompt de Transição: ZenCode Generator

Este arquivo contém o prompt consolidado para ser utilizado em um novo contexto ou repositório (ABP.io), garantindo que todo o conhecimento e decisões tomadas até aqui sejam preservados.

---

## 🤖 Contexto e Objetivos

Estou desenvolvendo o **ZenCode Generator**, uma ferramenta personalizada de geração de código para o ecossistema ABP.io. O objetivo é superar as limitações do ABP Suite, permitindo a geração de CRUDs completos em múltiplos frontends (Angular, React, React Native) com injeção de código segura em arquivos existentes.

## 🛠️ Decisões Técnicas Consolidadas

1.  **Manipulação de AST (Abstract Syntax Tree):**
    *   **Backend (C#):** Uso de **Roslyn (C# Compiler SDK)** para injetar propriedades (ex: `DbSet`) e métodos de forma robusta.
    *   **Frontend (TS):** Uso de **ts-morph** para manipular rotas, serviços e componentes sem depender de marcadores frágeis.
2.  **Sincronização e Idempotência:**
    *   Uso de **Classes Parciais** no C# (`Entity.cs` vs `Entity.Generated.cs`).
    *   Uso de **Regiões Protegidas** (`@zencode-start` / `@zencode-end`) em arquivos que não suportam partials.
3.  **Metadados (Single Source of Truth):**
    *   Um **Schema JSON** centralizado que descreve entidades, relacionamentos (1:1, 1:N, N:N) e `uiHints` (datepicker, lookup, etc).
4.  **Integração com IA (Gemini):**
    *   Workflow para transformar **SQL DDL** diretamente no JSON de metadados do gerador.

## 🚀 Próximas Atividades no Repositório ABP

Ao iniciar no novo repositório, execute as seguintes tarefas:

1.  **Pesquisa de Contexto:** Localizar arquivos JSON usados pelo ABP Suite (geralmente em `.suite/entities`) para entender como eles estruturam metadados nativamente e buscar retrocompatibilidade.
2.  **Análise de Padrões:** Estudar a estrutura atual do `ZenDoctor` ou projeto similar para mapear os pontos de injeção exatos (DbContext, Module, Navigation Providers).
3.  **Implementação do Editor Visual:** Iniciar o módulo React em `/zensuite` usando **React Flow** para a interface de desenho de entidades.
4.  **Proof of Concept (PoC):** Criar um gerador simples que injeta um novo `DbSet` em um `DbContext` real usando Roslyn.

---

> [!TIP]
> Use este prompt para me dar as boas-vindas no novo repositório e eu estarei pronto para continuar de onde paramos!
