# ABP.io Commercial Pro - Documentação

Este repositório contém o código-fonte dos módulos comerciais Pro do ABP.io Framework, baixados conforme assinatura contratada.

## 📋 Índice

- [Visão Geral](#visão-geral)
- [Arquitetura](#arquitetura)
- [Módulos Disponíveis](#módulos-disponíveis)
- [Estrutura de Módulos](#estrutura-de-módulos)
- [Dependências](#dependências)
- [Verificação de Licença](#verificação-de-licença)
- [Como Usar](#como-usar)
- [Referências](#referências)

## Visão Geral

O ABP.io Pro é um conjunto de módulos comerciais pré-construídos para o framework ABP, projetados para acelerar o desenvolvimento de aplicações empresariais. Os módulos seguem uma arquitetura DDD (Domain-Driven Design) modular, permitindo:

- ✅ **Compilação mais rápida** - Cada módulo gera DLLs independentes
- ✅ **Abertura rápida na IDE** - Projetos menores e focados
- ✅ **Reutilização** - Módulos podem ser usados em diferentes projetos
- ✅ **Manutenibilidade** - Código organizado e separado por responsabilidade

## Arquitetura

### Stack Tecnológico

| Camada | Tecnologia | Versão |
|--------|------------|--------|
| Backend | .NET | 9.0 |
| Framework | ABP Framework | 9.0.4 |
| ORM | Entity Framework Core / MongoDB | 9.0.0 |
| Autenticação | OpenIddict / IdentityServer | Pro |
| Frontend Web | Angular | 18+ |
| Frontend Blazor | Blazor Server/WASM/MAUI | .NET 9 |
| Tema | LeptonX | 3.2.0-preview |

### Estrutura de Pastas Principal

```
ABPIO-Fontes-Completos/
├── src/              # Código-fonte dos módulos (.NET)
├── angular/          # Bibliotecas Angular (15 projetos)
├── host/             # Aplicações de host/exemplo
├── app/              # Demo apps
├── test/             # Projetos de teste
├── demo/             # LeptonX Demo App
├── database/         # Scripts de banco
├── etc/              # Configurações extras
├── source-scss/      # SCSS do tema
└── docs/             # Esta documentação
```

## Módulos Disponíveis

O repositório inclui **15 módulos comerciais** principais:

### Módulos de Infraestrutura

| Módulo | Descrição | Solution |
|--------|-----------|----------|
| **Volo.Saas** | Multi-tenancy, Editions, Tenant Management | `Volo.Saas.sln` |
| **Volo.Abp.Identity.Pro** | Gerenciamento avançado de usuários, roles, claims | `Volo.Abp.Identity.Pro.sln` |
| **Volo.Abp.Account.Pro** | Login, registro, recuperação senha, perfil | `Volo.Abp.Account.Pro.sln` |
| **Volo.Abp.OpenIddict.Pro** | OAuth 2.0/OpenIdConnect server management | `Volo.Abp.OpenIddict.Pro.sln` |
| **Volo.Abp.AuditLogging** | Logs de auditoria avançados | `Volo.Abp.AuditLogging.sln` |

### Módulos de Funcionalidades

| Módulo | Descrição | Solution |
|--------|-----------|----------|
| **Volo.Payment** | Gateway de pagamentos (Stripe, PayPal, Iyzico, etc) | `Volo.Payment.sln` |
| **Volo.Chat** | Chat em tempo real (SignalR) | `Volo.Chat.sln` |
| **Volo.CmsKit.Pro** | CMS: blogs, FAQs, newsletters | `Volo.CmsKit.Pro.sln` |
| **Volo.FileManagement** | Gerenciamento de arquivos | `Volo.FileManagement.sln` |
| **Volo.Forms** | Criação de formulários dinâmicos | `Volo.Forms.sln` |

### Módulos de Suporte

| Módulo | Descrição | Solution |
|--------|-----------|----------|
| **Volo.Abp.LanguageManagement** | Gerenciamento de idiomas e traduções | `Volo.Abp.LanguageManagement.sln` |
| **Volo.Abp.TextTemplateManagement** | Templates de texto/email | `Volo.Abp.TextTemplateManagement.sln` |
| **Volo.Abp.Gdpr** | Conformidade GDPR | `Volo.Abp.Gdpr.sln` |
| **Volo.Abp.Sms.Twilio** | Integração SMS com Twilio | `Volo.Abp.Sms.Twilio.sln` |
| **Volo.Abp.LeptonXTheme** | Tema visual premium | `Volo.Abp.LeptonXTheme.sln` |

## Estrutura de Módulos

Cada módulo segue a arquitetura DDD em camadas do ABP Framework:

```
Volo.{ModuleName}/
├── Volo.{ModuleName}.Domain.Shared    # Constantes, enums, DTOs compartilhados
├── Volo.{ModuleName}.Domain           # Entidades, repositórios, serviços de domínio  
├── Volo.{ModuleName}.Application.Contracts  # Interfaces e DTOs de aplicação
├── Volo.{ModuleName}.Application      # Implementação dos serviços de aplicação
├── Volo.{ModuleName}.EntityFrameworkCore  # Implementação EF Core
├── Volo.{ModuleName}.MongoDB          # Implementação MongoDB
├── Volo.{ModuleName}.HttpApi          # Controllers REST
├── Volo.{ModuleName}.HttpApi.Client   # Cliente HTTP dinâmico
├── Volo.{ModuleName}.Web              # UI MVC/Razor Pages
├── Volo.{ModuleName}.Blazor           # UI Blazor compartilhada
├── Volo.{ModuleName}.Blazor.Server    # Blazor Server específico
├── Volo.{ModuleName}.Blazor.WebAssembly  # Blazor WASM específico
└── Volo.{ModuleName}.Installer        # Configuração do instalador
```

### Exemplo: Módulo SaaS

O módulo SaaS demonstra a estrutura típica:

**Entidades principais:**
- `Tenant` - Representa um tenant no sistema multi-tenant
- `Edition` - Edições/planos disponíveis
- `TenantConnectionString` - Connection strings por tenant

**Funcionalidades:**
- Gerenciamento de Tenants
- Gerenciamento de Editions
- Features por Tenant/Edition
- Integração com módulo Payment para assinaturas

## Dependências

### Dependências ABP Framework (Open Source)

Os módulos utilizam pacotes do ABP Framework open source:

```xml
<PackageReference Include="Volo.Abp.Ddd.Domain" VersionOverride="9.0.4" />
<PackageReference Include="Volo.Abp.AutoMapper" VersionOverride="9.0.4" />
<PackageReference Include="Volo.Abp.Data" VersionOverride="9.0.4" />
<PackageReference Include="Volo.Abp.MultiTenancy" VersionOverride="9.0.4" />
<PackageReference Include="Volo.Abp.FeatureManagement.Domain" VersionOverride="9.0.4" />
```

### Dependência Commercial Core

**91 projetos** referenciam o pacote `Volo.Abp.Commercial.Core`:

```xml
<PackageReference Include="Volo.Abp.Commercial.Core" VersionOverride="9.0.4" />
```

Este pacote está disponível via NuGet comercial:
- **URL**: `https://nuget.abp.io/{license-key}/v3/index.json`

## ⚠️ Verificação de Licença

### Status da Análise

Após análise detalhada do código-fonte:

> **✅ NÃO foi encontrado código de verificação de licença nos arquivos-fonte deste repositório.**

A verificação de licença do ABP.io Commercial está contida no pacote NuGet externo:
- `Volo.Abp.Commercial.Core` (versão 9.0.4)

### Localização da Verificação

O `Volo.Abp.Commercial.Core` é um **pacote NuGet compilado** (DLL), não incluído como código-fonte. Este pacote:

1. É baixado do feed NuGet privado ABP Commercial
2. Contém a lógica de validação de licença
3. Não está disponível como código-fonte neste repositório

### Opções para Desabilitar Verificação

Para desabilitar a verificação de licença, você tem as seguintes opções:

#### Opção 1: Remover Dependências do Commercial.Core

Editar os arquivos `.csproj` e remover as referências ao pacote:

```diff
- <PackageReference Include="Volo.Abp.Commercial.Core" VersionOverride="9.0.4" />
```

**Impacto**: Alguns módulos podem quebrar se usarem funcionalidades deste pacote.

#### Opção 2: Criar Pacote Substituto

Criar um projeto `Volo.Abp.Commercial.Core` local com implementação vazia:

```csharp
namespace Volo.Abp.Commercial;

// Implementação vazia ou mock
public class AbpCommercialCoreModule : AbpModule
{
    // Módulo vazio
}
```

#### Opção 3: NuGet Local

Configurar o `NuGet.Config` para apontar para um feed local com pacotes modificados.

### NuGet.Config Atual

O repositório está configurado para usar o feed comercial ABP:

```xml
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="ABP Commercial NuGet Source" value="https://nuget.abp.io/{license-key}/v3/index.json" />
  </packageSources>
</configuration>
```

## Como Usar

### 1. Abrir Solução Específica

Cada módulo tem sua própria solução:

```bash
# Abrir módulo SaaS
dotnet sln Volo.Saas.sln

# Abrir módulo Payment
dotnet sln Volo.Payment.sln
```

### 2. Restaurar Dependências

```bash
dotnet restore
```

### 3. Compilar

```bash
dotnet build
```

### 4. Rodar Host de Exemplo

```bash
cd host/Volo.{ModuleName}.Web.Unified
dotnet run
```

## Referências

- [Documentação ABP Commercial Modules](https://abp.io/modules)
- [ABP Framework Documentation](https://abp.io/docs)
- [Repositório ABP Framework OSS](https://github.com/abpframework/abp)

---

> 📝 **Nota**: Esta documentação foi gerada a partir da análise do código-fonte em 31/12/2024.
