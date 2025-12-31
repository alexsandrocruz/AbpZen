# Módulos ABP.io Pro - Documentação Detalhada

Documentação detalhada de cada módulo comercial ABP.io Pro disponível neste repositório.

## 📦 Volo.Saas - Multi-tenancy & SaaS

O módulo SaaS fornece gerenciamento completo de multi-tenancy para aplicações SaaS.

### Funcionalidades

- ✅ Gerenciamento de Tenants (CRUD)
- ✅ Gerenciamento de Editions (planos)
- ✅ Features por Tenant/Edition
- ✅ Connection strings por tenant
- ✅ Estado de ativação (ativo, inativo, tempo limitado)
- ✅ Integração com Payment para assinaturas

### Entidades Principais

| Entidade | Descrição | Arquivo |
|----------|-----------|---------|
| `Tenant` | Tenant do sistema | `src/Volo.Saas.Domain/Volo/Saas/Tenants/Tenant.cs` |
| `Edition` | Edição/plano | `src/Volo.Saas.Domain/Volo/Saas/Editions/Edition.cs` |
| `TenantConnectionString` | Connection string | `Tenants/TenantConnectionString.cs` |

### Projetos do Módulo

```
Volo.Saas/
├── Volo.Saas.Domain.Shared           # Constantes, Enums
├── Volo.Saas.Domain                  # Entidades, Managers
├── Volo.Saas.EntityFrameworkCore     # Migrations, DbContext
├── Volo.Saas.MongoDB                 # Implementação MongoDB
├── Volo.Saas.Host.Application        # App Service (Host)
├── Volo.Saas.Host.Application.Contracts
├── Volo.Saas.Host.HttpApi           # Controllers REST
├── Volo.Saas.Host.HttpApi.Client    # Cliente HTTP
├── Volo.Saas.Host.Web               # UI MVC
├── Volo.Saas.Host.Blazor            # UI Blazor
├── Volo.Saas.Host.Blazor.Server
├── Volo.Saas.Host.Blazor.WebAssembly
├── Volo.Saas.Tenant.Application     # App Service (Tenant)
├── Volo.Saas.Tenant.Application.Contracts
├── Volo.Saas.Tenant.HttpApi
├── Volo.Saas.Tenant.HttpApi.Client
├── Volo.Saas.Tenant.Web
├── Volo.Saas.Tenant.Blazor
└── Volo.Saas.Installer
```

### Dependências

```xml
<PackageReference Include="Volo.Abp.MultiTenancy" />
<PackageReference Include="Volo.Abp.FeatureManagement.Domain" />
<PackageReference Include="Volo.Abp.Commercial.Core" />
```

---

## 💳 Volo.Payment - Gateway de Pagamentos

Módulo de integração com múltiplos gateways de pagamento.

### Gateways Suportados

| Gateway | Projeto Domain | Projeto Web |
|---------|---------------|-------------|
| **Stripe** | `Volo.Payment.Stripe.Domain` | `Volo.Payment.Stripe.Web` |
| **PayPal** | `Volo.Payment.PayPal.Domain` | `Volo.Payment.PayPal.Web` |
| **Iyzico** | `Volo.Payment.Iyzico.Domain` | `Volo.Payment.Iyzico.Web` |
| **PayU** | `Volo.Payment.Payu.Domain` | `Volo.Payment.Payu.Web` |
| **2Checkout** | `Volo.Payment.TwoCheckout.Domain` | `Volo.Payment.TwoCheckout.Web` |
| **Alipay** | `Volo.Payment.Alipay.Domain` | `Volo.Payment.Alipay.Web` |
| **WeChatPay** | `Volo.Payment.WeChatPay.Domain` | `Volo.Payment.WeChatPay.Web` |

### Entidades Principais

| Entidade | Descrição |
|----------|-----------|
| `Plan` | Plano de assinatura |
| `GatewayPlan` | Mapeamento plan → gateway |
| `PaymentRequest` | Solicitação de pagamento |

### Estrutura

```
Volo.Payment/
├── Volo.Payment.Domain.Shared
├── Volo.Payment.Domain              # Core domain
│   ├── Plans/                       # Planos
│   ├── Requests/                    # Payment requests
│   └── Gateways/                    # Base gateway
├── Volo.Payment.EntityFrameworkCore
├── Volo.Payment.MongoDB
├── Volo.Payment.Application
├── Volo.Payment.Application.Contracts
├── Volo.Payment.HttpApi
├── Volo.Payment.HttpApi.Client
├── Volo.Payment.Web                 # UI comum
├── Volo.Payment.Admin.*             # Admin UI
└── Volo.Payment.{Gateway}.*         # Gateways específicos
```

---

## 💬 Volo.Chat - Chat em Tempo Real

Sistema de chat em tempo real usando SignalR.

### Funcionalidades

- ✅ Conversas 1-1
- ✅ Histórico de mensagens
- ✅ Indicador de leitura
- ✅ Usuários online
- ✅ Notificações push

### Entidades Principais

| Entidade | Descrição |
|----------|-----------|
| `Conversation` | Conversa entre usuários |
| `Message` | Mensagem individual |
| `ChatUser` | Usuário do chat |

### Estrutura

```
Volo.Chat/
├── Volo.Chat.Domain
│   ├── Conversations/
│   ├── Messages/
│   ├── Users/
│   └── Settings/
├── Volo.Chat.SignalR              # Hub SignalR
├── Volo.Chat.Application
├── Volo.Chat.Blazor.MauiBlazor    # MAUI Blazor
└── ...
```

---

## 📝 Volo.Forms - Formulários Dinâmicos

Criação e gerenciamento de formulários dinâmicos (pesquisas, enquetes).

### Funcionalidades

- ✅ Criador de formulários drag-and-drop
- ✅ Múltiplos tipos de questões
- ✅ Coleta de respostas
- ✅ Análise de resultados

### Entidades Principais

| Entidade | Descrição |
|----------|-----------|
| `Form` | Formulário |
| `Question` | Pergunta do formulário |
| `Choice` | Opção de escolha |
| `FormResponse` | Resposta do formulário |
| `Answer` | Resposta individual |

### Tipos de Questões

- Text
- Paragraph
- Multiple Choice
- Checkbox
- Dropdown
- Rating
- Date
- ...

### Estrutura

```
Volo.Forms/
├── Volo.Forms.Domain
│   ├── Forms/
│   ├── Questions/
│   ├── Choices/
│   ├── Responses/
│   └── Answers/
└── ...
```

**Nota**: Este módulo NÃO depende de `Volo.Abp.Commercial.Core`.

---

## 📁 Volo.FileManagement - Gerenciamento de Arquivos

Sistema de gerenciamento de arquivos com pastas hierárquicas.

### Funcionalidades

- ✅ Upload/Download de arquivos
- ✅ Estrutura de pastas
- ✅ Permissões por pasta
- ✅ Preview de arquivos
- ✅ Versionamento

### Estrutura

```
Volo.FileManagement/
├── Volo.FileManagement.Domain
│   ├── Files/
│   └── Directories/
└── ...
```

---

## 📰 Volo.CmsKit.Pro - Sistema de Conteúdo

Extensão Pro do CMS Kit com funcionalidades adicionais.

### Funcionalidades

- ✅ Blogs avançados
- ✅ Newsletter com integrações
- ✅ FAQ system
- ✅ URL Forwarding
- ✅ Contact form
- ✅ Polls

### Estrutura

```
Volo.CmsKit.Pro/
├── Volo.CmsKit.Pro.Domain
├── Volo.CmsKit.Pro.Admin.*        # Admin APIs
├── Volo.CmsKit.Pro.Public.*       # Public APIs
└── Volo.CmsKit.Pro.Common.*       # Compartilhado
```

---

## 👤 Volo.Abp.Identity.Pro - Identity Avançado

Extensão Pro do módulo Identity com recursos avançados.

### Funcionalidades

- ✅ Gerenciamento de usuários avançado
- ✅ Claim types personalizados
- ✅ Organization Units
- ✅ Security Logs
- ✅ Two-Factor Authentication
- ✅ External logins (Google, Microsoft, Twitter)

### Estrutura

```
Volo.Abp.Identity.Pro/
├── Volo.Abp.Identity.Pro.Domain
├── Volo.Abp.Identity.Pro.EntityFrameworkCore
├── Volo.Abp.Identity.Pro.Application
├── Volo.Abp.Identity.Pro.HttpApi
├── Volo.Abp.Identity.Pro.Web
├── Volo.Abp.Identity.Pro.Blazor*
└── Volo.Abp.Identity.Pro.Installer
```

---

## 🔐 Volo.Abp.Account.Pro - Account Avançado

Módulo Pro de gerenciamento de conta de usuário.

### Funcionalidades

- ✅ Login/Logout
- ✅ Registro de usuário
- ✅ Recuperação de senha
- ✅ Perfil do usuário
- ✅ Two-Factor Authentication
- ✅ External login providers
- ✅ Impersonation

### Projetos

```
Volo.Abp.Account.Pro/
├── Volo.Abp.Account.Pro.Admin.*          # Admin APIs
├── Volo.Abp.Account.Pro.Public.*         # Public APIs
├── Volo.Abp.Account.Pro.Shared.*         # Compartilhado
├── Volo.Abp.Account.Pro.Public.Web.OpenIddict    # OpenIddict
├── Volo.Abp.Account.Pro.Public.Web.IdentityServer # IdentityServer
├── Volo.Abp.Account.Pro.Public.Web.Impersonation  # Impersonation
└── Volo.Abp.Account.Pro.Public.MauiBlazor # MAUI
```

---

## 🔑 Volo.Abp.OpenIddict.Pro - OpenIddict Management

Gerenciamento de aplicações OAuth 2.0 / OpenIdConnect.

### Funcionalidades

- ✅ Gerenciamento de Applications
- ✅ Gerenciamento de Scopes
- ✅ UI para configuração

---

## 📋 Volo.Abp.AuditLogging - Logs de Auditoria

Visualização e gerenciamento de logs de auditoria.

### Funcionalidades

- ✅ Visualização de audit logs
- ✅ Filtros avançados
- ✅ Detalhes de mudanças de entidades
- ✅ Exportação

---

## 🌐 Volo.Abp.LanguageManagement - Idiomas

Gerenciamento de idiomas e traduções em tempo de execução.

### Funcionalidades

- ✅ Adicionar/remover idiomas
- ✅ Editar traduções via UI
- ✅ Exportar/importar traduções

---

## 📄 Volo.Abp.TextTemplateManagement - Templates

Gerenciamento de templates de texto/email.

### Funcionalidades

- ✅ Editar templates via UI
- ✅ Preview de templates
- ✅ Templates por cultura

---

## 🛡️ Volo.Abp.Gdpr - Conformidade GDPR

Ferramentas para conformidade com GDPR.

### Funcionalidades

- ✅ Solicitação de dados pessoais
- ✅ Download de dados
- ✅ Exclusão de dados
- ✅ Cookie consent

---

## 📱 Volo.Abp.Sms.Twilio - SMS

Integração com Twilio para envio de SMS.

### Funcionalidades

- ✅ Envio de SMS via Twilio
- ✅ Configuração via appsettings

---

## 🎨 Volo.Abp.LeptonXTheme - Tema

Tema visual premium LeptonX.

### Variantes

- ✅ MVC/Razor Pages
- ✅ Blazor Server
- ✅ Blazor WebAssembly
- ✅ MAUI Blazor

### Layouts

- Side Menu
- Top Menu

### Personalização

- CSS variables
- SCSS customization
- Multiple color styles
