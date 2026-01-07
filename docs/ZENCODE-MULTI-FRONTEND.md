# ZenCode Generator: Multi-Frontend Implementation Plan

## 📋 Sumário Executivo

Este documento descreve o plano de implementação para expandir o ZenCode Generator, adicionando suporte à geração de código para **Angular** e **React (Next.js)**, além do já implementado **Razor (ASP.NET MVC)**.

### Objetivos

1. Manter um único `zencode-template` com backend compartilhado
2. Permitir seleção de múltiplos frontends durante a geração
3. Gerar código seguindo os padrões de cada framework
4. Integrar com as bibliotecas oficiais do ABP para cada frontend

---

## 🎯 Visão Geral dos Frontends

| Frontend | Framework | API Client | UI Components | Status |
|----------|-----------|------------|---------------|--------|
| **Razor** | ASP.NET MVC | AppService direto | Bootstrap 5 + jQuery | ✅ Implementado |
| **Angular** | Angular 17+ | ABP Proxy | PrimeNG / ngx-datatable | 🔧 Planejado |
| **React** | Next.js 15 | OpenAPI + TanStack | Radix UI + Tailwind | 🔧 Planejado |

---

## 🏗️ Arquitetura do Generator

### Estrutura de Templates Proposta

```
zencode-generator/src/generators/
├── templates/
│   ├── backend/                    # ✅ Compartilhado (já existe)
│   │   ├── entity.ts
│   │   ├── dto.ts
│   │   ├── service.ts
│   │   ├── permissions.ts
│   │   ├── menu.ts
│   │   ├── localization.ts
│   │   └── repository.ts
│   │
│   ├── razor/                      # ✅ Implementado
│   │   ├── razor-index.ts
│   │   ├── razor-modal.ts
│   │   ├── razor-modal-js.ts
│   │   ├── razor-page-full.ts
│   │   └── razor-viewmodel.ts
│   │
│   ├── angular/                    # 🔧 A implementar
│   │   ├── component-list.ts
│   │   ├── component-form.ts
│   │   ├── module.ts
│   │   ├── routing.ts
│   │   ├── service.ts
│   │   └── injections.ts
│   │
│   └── react/                      # 🔧 A implementar
│       ├── page.ts
│       ├── list-component.ts
│       ├── add-component.ts
│       ├── edit-component.ts
│       ├── delete-component.ts
│       ├── form-component.ts
│       ├── hook.ts
│       └── columns.ts
```

---

## 📦 Alterações no Schema de Metadados

### Arquivo: `types.ts`

```typescript
// Novo tipo para targets de frontend
export type FrontendTarget = 'razor' | 'angular' | 'react';

// Configuração específica do React
export interface ReactConfig {
    baseApiUrl: string;              // Ex: "http://localhost:44322"
    useOpenApiClient: boolean;       // Usar cliente gerado via OpenAPI
    uiLibrary: 'radix' | 'shadcn';   // Biblioteca de componentes
}

// Configuração específica do Angular
export interface AngularConfig {
    proxyModule: string;             // Ex: "@proxy"
    uiFramework: 'primeng' | 'material' | 'bootstrap';
    useNgxDatatable: boolean;
}

// Adicionar ao ZenMetadata
export interface ZenMetadata {
    projectName: string;
    namespace: string;
    
    // 🆕 Frontends a gerar
    frontendTargets: FrontendTarget[];
    
    // 🆕 Configurações específicas
    reactConfig?: ReactConfig;
    angularConfig?: AngularConfig;
    
    entities: {...}[];
    relationships: {...}[];
}
```

---

## 🅰️ Implementação Angular

### Baseado em: `zencode-template/angular`

O template Angular do ABP já está integrado e usa:
- **@abp/ng.core** - Core do ABP
- **@volo/abp.ng.theme.lepton-x** - Tema LeptonX
- **@volo/abp.ng.identity** - Módulos Pro

### Arquivos a Gerar (por Entidade)

```
src/app/{entity-kebab}/
├── {entity-kebab}.module.ts
├── {entity-kebab}-routing.module.ts
├── {entity-kebab}.component.ts
├── {entity-kebab}.component.html
├── {entity-kebab}.component.scss
├── components/
│   ├── {entity-kebab}-list/
│   │   ├── {entity-kebab}-list.component.ts
│   │   └── {entity-kebab}-list.component.html
│   └── {entity-kebab}-form/
│       ├── {entity-kebab}-form.component.ts
│       └── {entity-kebab}-form.component.html
└── services/
    └── {entity-kebab}.service.ts
```

### Pontos de Injeção

| Arquivo | O que Injetar |
|---------|---------------|
| `app-routing.module.ts` | Lazy-load route para o módulo |
| `route.provider.ts` | Item de menu no sidebar |
| `environment.ts` | Configuração de API (se necessário) |

### Exemplo de Template: `angular/component-list.ts`

```typescript
export function generateAngularListComponent(entity: EntityData): string {
  const { name, pluralName, fields } = entity;
  
  return `import { Component, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { ${name}Service } from '../services/${toKebabCase(name)}.service';
import { ${name}Dto } from '@proxy/${toKebabCase(pluralName)}';

@Component({
  selector: 'app-${toKebabCase(name)}-list',
  templateUrl: './${toKebabCase(name)}-list.component.html',
  providers: [ListService]
})
export class ${name}ListComponent implements OnInit {
  items: PagedResultDto<${name}Dto> = { items: [], totalCount: 0 };
  
  constructor(
    public readonly list: ListService,
    private readonly service: ${name}Service
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.service.getList({
      skipCount: this.list.page * this.list.maxResultCount,
      maxResultCount: this.list.maxResultCount
    }).subscribe(result => {
      this.items = result;
    });
  }
}`;
}
```

---

## ⚛️ Implementação React (Next.js)

### Baseado em: `abp-react-main`

O projeto abp-react é uma solução moderna que usa:
- **Next.js 15** com App Router
- **TanStack Query** para gerenciamento de dados
- **TanStack Table** para tabelas
- **Radix UI + Tailwind** para componentes
- **OpenAPI Client** gerado automaticamente

### Arquivos a Gerar (por Entidade)

```
src/
├── app/admin/{entity-kebab}/
│   └── page.tsx                    # Página principal
│
├── components/{entity-kebab}/
│   ├── {Entity}List.tsx            # Lista com TanStack Table
│   ├── Add{Entity}.tsx             # Botão + Dialog de criação
│   ├── {Entity}Edit.tsx            # Dialog de edição
│   ├── Delete{Entity}.tsx          # Dialog de confirmação
│   └── {Entity}Form.tsx            # Formulário compartilhado (react-hook-form)
│
└── lib/hooks/
    └── use{Entities}.ts            # Hook com TanStack Query
```

### Pontos de Injeção

| Arquivo | O que Injetar |
|---------|---------------|
| `lib/hooks/QueryConstants.ts` | Nome da query para invalidação |
| `components/menu/` | Item de menu no sidebar |
| `client/` | Tipos gerados via OpenAPI |

### Exemplo de Template: `react/list-component.ts`

```typescript
export function generateReactListComponent(entity: EntityData): string {
  const { name, pluralName, fields } = entity;
  const camelName = toCamelCase(name);
  const kebabName = toKebabCase(name);
  
  const gridFields = fields.filter(f => f.showInGrid !== false);
  
  return `'use client'
import { ${name}Dto } from '@/client'
import { CustomTable } from '@/components/ui/CustomTable'
import { Search } from '@/components/ui/Search'
import { use${pluralName} } from '@/lib/hooks/use${pluralName}'
import { useQueryClient } from '@tanstack/react-query'
import { ColumnDef, PaginationState, getCoreRowModel, useReactTable } from '@tanstack/react-table'
import { useMemo, useState } from 'react'
import { PermissionActions } from '../permission/PermissionActions'
import { Add${name} } from './Add${name}'
import { Delete${name} } from './Delete${name}'
import { ${name}Edit } from './${name}Edit'

export const ${name}List = () => {
  const queryClient = useQueryClient()
  const [searchStr, setSearchStr] = useState<string>('')
  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  })

  const { isLoading, data, isError } = use${pluralName}(
    pagination.pageIndex,
    pagination.pageSize,
    searchStr || undefined
  )

  const columns = useMemo(() => get${name}Columns(), [])

  const table = useReactTable({
    data: data?.items ?? [],
    pageCount: Math.ceil((data?.totalCount ?? 0) / pagination.pageSize),
    state: { pagination },
    columns,
    getCoreRowModel: getCoreRowModel(),
    onPaginationChange: setPagination,
    manualPagination: true,
  })

  if (isLoading) return <div className="p-4">Loading...</div>
  if (isError) return <div className="p-4 text-red-500">Error loading data</div>

  return (
    <div className="w-full">
      <Add${name} />
      <Search onUpdate={setSearchStr} value={searchStr} />
      <CustomTable<${name}Dto>
        table={table}
        totalCount={data?.totalCount ?? 0}
        pageSize={pagination.pageSize}
      />
    </div>
  )
}

const get${name}Columns = (): ColumnDef<${name}Dto>[] => [
  {
    header: '${pluralName}',
    columns: [
      {
        accessorKey: 'actions',
        header: 'Actions',
        cell: (info) => (
          <PermissionActions
            actions={[
              { icon: 'pencil', callback: () => {} },
              { icon: 'trash', callback: () => {} },
            ]}
          />
        ),
      },
${gridFields.map(f => `      {
        accessorKey: '${toCamelCase(f.name)}',
        header: '${f.label || f.name}',
        cell: (info) => ${getReactCellRenderer(f)},
      },`).join('\n')}
    ],
  },
]
`
}
```

### Exemplo de Template: `react/hook.ts`

```typescript
export function generateReactHook(entity: EntityData): string {
  const { name, pluralName } = entity;
  
  return `import { useQuery } from '@tanstack/react-query'
import { ${camelCase(name)}Service } from '@/client'
import { QueryNames } from './QueryConstants'

export const use${pluralName} = (
  pageIndex: number,
  pageSize: number,
  filter?: string
) => {
  return useQuery({
    queryKey: [QueryNames.Get${pluralName}, pageIndex, pageSize, filter],
    queryFn: () =>
      ${camelCase(name)}Service.getList({
        skipCount: pageIndex * pageSize,
        maxResultCount: pageSize,
        filter,
      }),
  })
}

export const use${name} = (id: string) => {
  return useQuery({
    queryKey: [QueryNames.Get${name}, id],
    queryFn: () => ${camelCase(name)}Service.get({ id }),
    enabled: !!id,
  })
}
`
}
```

---

## 🖥️ Alterações na UI do Generator

### Arquivo: `GenerateCodeModal.tsx`

Adicionar seleção de frontends:

```tsx
interface GenerationConfig {
  frontendTargets: {
    razor: boolean;
    angular: boolean;
    react: boolean;
  };
  reactConfig?: ReactConfig;
  angularConfig?: AngularConfig;
}

// No componente
<div className="space-y-4">
  <Label className="text-lg font-semibold">Frontends a Gerar</Label>
  
  <div className="grid grid-cols-3 gap-4">
    <Card className={`p-4 cursor-pointer ${config.frontendTargets.razor ? 'border-blue-500' : ''}`}>
      <Checkbox 
        id="razor" 
        checked={config.frontendTargets.razor}
        onCheckedChange={(v) => updateFrontend('razor', v)}
      />
      <Label htmlFor="razor" className="ml-2">
        <strong>Razor</strong>
        <p className="text-sm text-gray-500">ASP.NET MVC + Bootstrap</p>
      </Label>
    </Card>
    
    <Card className={`p-4 cursor-pointer ${config.frontendTargets.angular ? 'border-red-500' : ''}`}>
      <Checkbox 
        id="angular" 
        checked={config.frontendTargets.angular}
        onCheckedChange={(v) => updateFrontend('angular', v)}
      />
      <Label htmlFor="angular" className="ml-2">
        <strong>Angular</strong>
        <p className="text-sm text-gray-500">Angular 17 + ABP Proxy</p>
      </Label>
    </Card>
    
    <Card className={`p-4 cursor-pointer ${config.frontendTargets.react ? 'border-cyan-500' : ''}`}>
      <Checkbox 
        id="react" 
        checked={config.frontendTargets.react}
        onCheckedChange={(v) => updateFrontend('react', v)}
      />
      <Label htmlFor="react" className="ml-2">
        <strong>React</strong>
        <p className="text-sm text-gray-500">Next.js 15 + TanStack</p>
      </Label>
    </Card>
  </div>
</div>
```

---

## 📁 Estrutura Final do ZIP Gerado

```
generated-code.zip
├── backend/                        # Sempre incluído
│   ├── Domain/
│   │   └── {Entity}/{Entity}.cs
│   ├── Domain.Shared/
│   │   ├── Localization/
│   │   └── Permissions/
│   ├── Application.Contracts/
│   │   └── {Entity}/
│   │       ├── {Entity}Dto.cs
│   │       ├── Create{Entity}Dto.cs
│   │       └── I{Entity}AppService.cs
│   ├── Application/
│   │   └── {Entity}/{Entity}AppService.cs
│   ├── HttpApi/
│   │   └── {Entity}/{Entity}Controller.cs
│   └── MongoDB/  (ou EntityFrameworkCore/)
│       └── {Entity}Repository.cs
│
├── razor/                          # Se selecionado
│   └── Web/Pages/{Entity}/
│       ├── Index.cshtml
│       ├── Index.cshtml.cs
│       ├── Index.js
│       ├── CreateModal.cshtml
│       └── EditModal.cshtml
│
├── angular/                        # Se selecionado
│   └── src/app/{entity-kebab}/
│       ├── {entity-kebab}.module.ts
│       ├── {entity-kebab}-routing.module.ts
│       └── components/...
│
└── react/                          # Se selecionado
    └── src/
        ├── app/admin/{entity-kebab}/page.tsx
        ├── components/{entity-kebab}/...
        └── lib/hooks/use{Entities}.ts
```

---

## 📅 Cronograma de Implementação

### Fase 1: Preparação (1-2 dias)
- [ ] Refatorar templates existentes para pasta `backend/`
- [ ] Mover templates Razor para pasta `razor/`
- [ ] Atualizar `types.ts` com novos tipos
- [ ] Atualizar UI do Generator com seleção de frontends

### Fase 2: Implementação React (3-5 dias)
- [ ] Criar templates base (page, list, form)
- [ ] Implementar geração de hooks TanStack Query
- [ ] Implementar geração de colunas TanStack Table
- [ ] Adicionar suporte a lookups e enums
- [ ] Testar integração com `abp-react-main`

### Fase 3: Implementação Angular (3-5 dias)
- [ ] Criar templates base (module, component, service)
- [ ] Implementar geração de formulários reativos
- [ ] Implementar geração de tabelas (ngx-datatable ou PrimeNG)
- [ ] Adicionar injeção em routing e menu
- [ ] Testar integração com `zencode-template/angular`

### Fase 4: Documentação e Testes (2 dias)
- [ ] Documentar uso de cada frontend
- [ ] Criar exemplos de entidades geradas
- [ ] Testes de integração end-to-end
- [ ] Atualizar README do Generator

---

## 🔗 Referências

### Templates Base
- **Razor**: `demo-zen/LeptonXDemoApp.Web/Pages/`
- **Angular**: `zencode-template/angular/src/app/`
- **React**: `abp-react-main/src/src/`

### Documentação
- [ABP Angular UI](https://docs.abp.io/en/abp/latest/UI/Angular/Quick-Start)
- [ABP React (Comunidade)](https://github.com/antosubash/abp-react)
- [TanStack Query](https://tanstack.com/query/latest)
- [TanStack Table](https://tanstack.com/table/latest)

---

## 📝 Notas de Implementação

### Considerações Importantes

1. **Cliente API React**: O projeto `abp-react` usa `@hey-api/openapi-ts` para gerar o cliente. O Generator deve gerar código compatível com esse cliente.

2. **Permissões**: Todos os frontends devem respeitar o sistema de permissões do ABP. O React usa o componente `PermissionActions`, enquanto o Angular usa diretivas.

3. **Lookups**: Para campos FK, o React usa `Select` do Radix UI com dados carregados via hook separado. O Angular usa `p-dropdown` do PrimeNG.

4. **Validação**: 
   - React: `react-hook-form` + `zod`
   - Angular: Reactive Forms + Validators
   - Razor: jQuery Validation + FluentValidation

5. **Child Grids (Master-Detail)**: Implementação varia por framework. Priorizar para Razor primeiro, depois expandir.

---

*Documento criado em: 2026-01-06*
*Última atualização: 2026-01-06*
