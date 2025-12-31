# Guia de Início Rápido - ABP.io Pro

Este guia mostra como começar a trabalhar com os módulos ABP.io Pro deste repositório.

## 📋 Pré-requisitos

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js 18+](https://nodejs.org/)
- [SQL Server](https://www.microsoft.com/sql-server) ou [PostgreSQL](https://www.postgresql.org/)
- IDE: [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [JetBrains Rider](https://www.jetbrains.com/rider/)

## 🚀 Começando

### 1. Configurar NuGet

Primeiro, configure o acesso aos pacotes NuGet. O arquivo `NuGet.Config` já está configurado:

```xml
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="ABP Commercial NuGet Source" 
         value="https://nuget.abp.io/{sua-license-key}/v3/index.json" />
  </packageSources>
</configuration>
```

> **Nota**: Substitua `{sua-license-key}` pela sua chave de licença ABP Commercial.

### 2. Restaurar Dependências

```bash
cd ABPIO-Fontes-Completos
dotnet restore
```

### 3. Abrir uma Solução

Cada módulo tem sua própria solução:

```bash
# Módulo SaaS (Multi-tenancy)
dotnet sln Volo.Saas.sln

# Módulo Payment
dotnet sln Volo.Payment.sln

# Módulo Chat
dotnet sln Volo.Chat.sln

# Módulo CMS Kit Pro
dotnet sln Volo.CmsKit.Pro.sln
```

## 🏃 Executando um Host de Exemplo

### Host Unificado (Web + API)

```bash
# SaaS
cd host/Volo.Saas.DemoApp
dotnet run

# Chat
cd host/Volo.Chat.Web.Unified
dotnet run

# CMS Kit Pro
cd host/Volo.CmsKit.Pro.Web.Unified
dotnet run
```

### Host Separado (API + Web separados)

```bash
# Terminal 1 - API
cd host/Volo.{Module}.HttpApi.Host
dotnet run

# Terminal 2 - Web
cd host/Volo.{Module}.Web.Host
dotnet run
```

## 🗄️ Configurando Banco de Dados

### Connection String

Edite o `appsettings.json` do host:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=AbpProDemo;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### PostgreSQL

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Database=AbpProDemo;Username=postgres;Password=yourpassword"
  }
}
```

### Migrations

```bash
# Criar migration
cd src/Volo.{Module}.EntityFrameworkCore
dotnet ef migrations add Initial

# Aplicar migration
dotnet ef database update
```

## 🔨 Compilando

### Compilar Solução Completa

```bash
dotnet build Volo.Saas.sln
```

### Compilar Projeto Específico

```bash
dotnet build src/Volo.Saas.Domain/Volo.Saas.Domain.csproj
```

### Compilar Todos os Módulos

```bash
# Script para compilar todas as soluções
for sln in *.sln; do
    echo "Building $sln..."
    dotnet build "$sln" --no-restore
done
```

## 🧪 Executando Testes

```bash
# Todos os testes de um módulo
dotnet test Volo.Saas.sln

# Testes específicos
dotnet test test/Volo.Saas.Domain.Tests/Volo.Saas.Domain.Tests.csproj
```

## 📱 Projetos Angular

### Estrutura

```
angular/
└── projects/
    ├── account/      # Módulo Account
    ├── audit-logging/
    ├── chat/
    ├── file-management/
    ├── gdpr/
    ├── identity/
    ├── language-management/
    ├── lepton-x/     # Tema LeptonX
    ├── openiddictpro/
    ├── payment/
    ├── saas/
    └── text-template-management/
```

### Instalação

```bash
cd angular
npm install
```

### Build

```bash
npm run build
```

## 🎨 Demo LeptonX

Para ver o tema LeptonX em ação:

```bash
cd demo/LeptonXDemoApp
dotnet run
```

## 📁 Estrutura de Pastas

```
ABPIO-Fontes-Completos/
├── src/                    # Código fonte dos módulos
│   ├── Volo.Saas.Domain/
│   ├── Volo.Payment.Domain/
│   └── ...
├── host/                   # Aplicações de host
│   ├── Volo.Saas.Unified/
│   └── ...
├── app/                    # Demo apps
│   ├── Volo.Saas.DemoApp/
│   └── ...
├── test/                   # Projetos de teste
├── angular/                # Bibliotecas Angular
├── demo/                   # LeptonX Demo
├── docs/                   # Documentação
└── *.sln                   # Solutions
```

## 🔧 Usando Módulos no Seu Projeto

### Referenciando via Projeto

Se você quer usar o código fonte:

```xml
<ProjectReference Include="path/to/Volo.Saas.Domain/Volo.Saas.Domain.csproj" />
```

### Referenciando via NuGet (gerado localmente)

1. Gere os pacotes:

```bash
dotnet pack src/Volo.Saas.Domain -c Release -o ./nupkgs
```

2. Configure feed local:

```xml
<configuration>
  <packageSources>
    <add key="Local" value="./nupkgs" />
  </packageSources>
</configuration>
```

3. Referencie:

```xml
<PackageReference Include="Volo.Saas.Domain" Version="9.0.4" />
```

## 📝 Dicas

### VS Code Extensions Recomendadas

- C# Dev Kit
- NuGet Package Manager
- Angular Language Service

### Comandos Úteis

```bash
# Limpar binários
find . -name "bin" -type d -exec rm -rf {} +
find . -name "obj" -type d -exec rm -rf {} +

# Ou no Windows (PowerShell)
Get-ChildItem -Include bin,obj -Recurse -Directory | Remove-Item -Recurse -Force

# Ver dependências de um projeto
dotnet list package

# Ver dependências transitivas
dotnet list package --include-transitive
```

## 🔗 Próximos Passos

1. [Leia sobre a Arquitetura](./ARCHITECTURE.md)
2. [Explore os Módulos](./MODULES.md)
3. [Entenda as Dependências](./DEPENDENCIES.md)
4. [Verifique a questão de Licença](./LICENSE-VERIFICATION.md)
