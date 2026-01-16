import { Liquid } from 'liquidjs';
import { getRazorCreateModalJsTemplate, getRazorEditModalJsTemplate } from './templates/razor-modal-js.ts';

export interface ParentRelationshipContext {
    childEntityName: string;
    childPluralName: string;
    navigationName: string;        // Collection property name (e.g., "Products")
    childNavigationName: string;   // Child's FK navigation name (e.g., "Category")
    childFkFieldName: string;      // Child's FK field (e.g., "CategoryId")
    targetEntityName?: string;     // Aliases for template convenience
    targetPluralName?: string;
    isChildGrid?: boolean;
    childGridConfig?: ChildGridConfig;
    isManyToMany?: boolean;
    junctionConfig?: JunctionConfig;
    targetFields?: EntityField[];
}


import type { EntityData, EntityField, RelationshipData, ChildGridConfig, JunctionConfig } from '../types.ts';
import {
    pluralize,
    camelCase,
    pascalCase,
    kebabCase,
    snakeCase,
    toCSharpType,
    toTypeScriptType
} from './filters.ts';

// Templates as string constants
import { getAppServiceTemplate, getAppServiceInterfaceTemplate } from './templates/service.ts';
import { getDtoTemplate, getCreateUpdateDtoTemplate, getGetListInputTemplate } from './templates/dto.ts';
import { getDbContextExtensionsTemplate } from './templates/efcore.ts';
import { getPermissionsTemplate } from './templates/permissions.ts';
import { getEntityTemplate, getEntityConstsTemplate } from './templates/entity.ts';
import { getRepositoryInterfaceTemplate, getRepositoryImplementationTemplate } from './templates/repository.ts';
import { getAutoMapperProfileTemplate } from './templates/automapper.ts';
import {
    getLocalizationEntriesEnTemplate,
    getLocalizationEntriesPtBrTemplate,
} from './templates/localization.ts';
import { getEnumTemplate, getEnumLocalizationEnTemplate, getEnumLocalizationPtBrTemplate } from './templates/enum.ts';
// Razor page templates
import { getRazorIndexTemplate, getRazorIndexModelTemplate, getRazorIndexJsTemplate, getRazorIndexCssTemplate } from './templates/razor-index.ts';
import { getRazorCreateModalViewTemplate, getRazorCreateModalModelTemplate, getRazorEditModalViewTemplate, getRazorEditModalModelTemplate } from './templates/razor-modal.ts';
import { getRazorCreateViewModelTemplate, getRazorEditViewModelTemplate, getRazorAutoMapperProfileTemplate } from './templates/razor-viewmodel.ts';
// ZenLookup templates
// ZenLookup templates
import { getZenLookupTagHelperTemplate } from './templates/zen-lookup-taghelper.ts';
import {
    getReactV2PageTemplate,
    getReactV2ListComponentTemplate,
    getReactV2FormComponentTemplate,
    getReactV2CardComponentTemplate,
    getReactV2HookTemplate,
    getReactV2MasterDetailListPageTemplate,
    getReactV2MasterDetailFormPageTemplate
} from './templates/react-v2/index.ts';
import { getZenLookupModalTemplate, getZenLookupCssTemplate } from './templates/zen-lookup-modal.ts';
import { getZenLookupJsTemplate } from './templates/zen-lookup-js.ts';
// Razor Page Full templates (for Master-Detail)
import {
    getRazorCreatePageModelTemplate,
    getRazorCreatePageViewTemplate,
    getRazorEditPageModelTemplate,
    getRazorEditPageViewTemplate,
    getRazorCreatePageJsTemplate,
    getRazorEditPageJsTemplate
} from './templates/razor-page-full.ts';



// Angular templates
import {
    getAngularModuleTemplate,
    getAngularRoutingModuleTemplate,
    getAngularComponentTsTemplate,
    getAngularComponentHtmlTemplate,
    getAngularRouteProviderTemplate,
} from './templates/angular/index.ts';

import type { FrontendTarget } from '../types.ts';

export interface RelationshipInfo {
    id: string;
    source: string;  // Source entity name
    target: string;  // Target entity name
    data: RelationshipData;
}

// Duplicate definition removed to keep consistency with the one above

/**
 * Child relationship context (this entity has a FK to parent)
 */
export interface ChildRelationshipContext {
    parentEntityName: string;
    parentPluralName: string;
    fkFieldName: string;           // FK field name (e.g., "CategoryId")
    navigationName: string;        // Navigation property name (e.g., "Category")
    parentNavigationName: string;  // Parent's collection name (e.g., "Products")
    isRequired: boolean;
    lookupMode?: 'dropdown' | 'modal';
    displayField: string;          // Human readable field (e.g., "Name")
}

/**
 * Context passed to templates
 */
export interface GeneratorContext {
    project: {
        name: string;
        shortName: string;
        fullName: string;
        namespace: string;
    };
    entity: {
        name: string;
        pluralName: string;
        tableName: string;
        namespace: string;
        baseClass: string;
        primaryKey: string;
        fields: EntityField[];
    };
    dto: {
        readTypeName: string;
        createTypeName: string;
        updateTypeName: string;
    };
    // Relationship context
    relationships: {
        asParent: ParentRelationshipContext[];  // This entity is the "One" side
        asChild: ChildRelationshipContext[];    // This entity is the "Many" side
    };
    isMasterDetail: boolean;
}

/**
 * Generated file
 */
export interface GeneratedFile {
    path: string;
    content: string;
    layer: 'Domain' | 'Application' | 'Application.Contracts' | 'EntityFrameworkCore' | 'Web' | 'React' | 'Angular';
}

/**
 * Code generator using LiquidJS
 */
export class CodeGenerator {
    private engine: Liquid;

    constructor() {
        this.engine = new Liquid({
            strictVariables: false,
            strictFilters: false,
        });

        // Register custom filters
        this.engine.registerFilter('pluralize', pluralize);
        this.engine.registerFilter('camelCase', camelCase);
        this.engine.registerFilter('pascalCase', pascalCase);
        this.engine.registerFilter('kebabCase', kebabCase);
        this.engine.registerFilter('snakeCase', snakeCase);
        this.engine.registerFilter('csharpType', (type: string, nullable?: boolean) => toCSharpType(type, nullable));
        this.engine.registerFilter('tsType', toTypeScriptType);
    }

    /**
     * Create context from EntityData with relationship information
     */
    createContext(
        entity: EntityData,
        projectName: string,
        projectNamespace: string,
        asParent: ParentRelationshipContext[] = [],
        asChild: ChildRelationshipContext[] = []
    ): GeneratorContext {
        // Track occupied names to avoid CS0102 and CS0542
        const occupiedNames = new Set<string>();
        occupiedNames.add(entity.name);
        entity.fields.forEach(f => occupiedNames.add(f.name));

        // Standard ABP properties
        ['Id', 'ExtraProperties', 'ConcurrencyStamp', 'CreationTime', 'CreatorId',
            'LastModificationTime', 'LastModifierId', 'IsDeleted', 'DeleterId', 'DeletionTime']
            .forEach(name => occupiedNames.add(name));

        // Refine asParent names
        const uniqueAsParent = asParent.map(rel => {
            let uniqueNavName = rel.navigationName;
            if (occupiedNames.has(uniqueNavName)) {
                uniqueNavName = `${rel.navigationName}Collection`;
            }
            let counter = 1;
            while (occupiedNames.has(uniqueNavName)) {
                uniqueNavName = `${rel.navigationName}Collection${counter}`;
                counter++;
            }
            occupiedNames.add(uniqueNavName);
            return { ...rel, navigationName: uniqueNavName };
        });

        // Refine asChild names
        const uniqueAsChild = asChild.map(rel => {
            let uniqueNavName = rel.navigationName;
            if (occupiedNames.has(uniqueNavName)) {
                uniqueNavName = `${rel.navigationName}Nav`;
            }
            let counter = 1;
            while (occupiedNames.has(uniqueNavName)) {
                uniqueNavName = `${rel.navigationName}Nav${counter}`;
                counter++;
            }
            occupiedNames.add(uniqueNavName);
            return { ...rel, navigationName: uniqueNavName };
        });

        // Extract short name from projectName or namespace (last segment)
        const shortName = projectName.includes('.')
            ? projectName.split('.').pop() || projectName
            : (projectName !== projectNamespace ? projectName : projectNamespace.split('.').pop() || projectName);

        return {
            project: {
                name: projectName,
                shortName: shortName,
                fullName: projectNamespace,
                namespace: projectNamespace,
            },
            entity: {
                name: entity.name,
                pluralName: entity.pluralName,
                tableName: entity.tableName,
                namespace: `${projectNamespace}.${entity.name}`,
                baseClass: entity.baseClass,
                primaryKey: 'Guid',
                fields: entity.fields,
            },
            dto: {
                readTypeName: `${entity.name}Dto`,
                createTypeName: `CreateUpdate${entity.name}Dto`,
                updateTypeName: `CreateUpdate${entity.name}Dto`,
            },
            relationships: {
                asParent: uniqueAsParent,
                asChild: uniqueAsChild,
            },
            isMasterDetail: uniqueAsParent.some(r => r.isChildGrid)
        };
    }

    /**
     * Generate all files for an entity
     */
    async generateEntity(
        entity: EntityData,
        projectName: string,
        projectNamespace: string,
        asParent: ParentRelationshipContext[] = [],
        asChild: ChildRelationshipContext[] = []
    ): Promise<GeneratedFile[]> {
        const ctx = this.createContext(entity, projectName, projectNamespace, asParent, asChild);
        const files: GeneratedFile[] = [];

        // ============ DOMAIN LAYER ============
        // Entity class
        files.push({
            path: `${projectNamespace}.Domain/${entity.name}/${entity.name}.cs`,
            content: await this.engine.parseAndRender(getEntityTemplate(), ctx),
            layer: 'Domain',
        });

        // Entity consts
        files.push({
            path: `${projectNamespace}.Domain/${entity.name}/${entity.name}Consts.cs`,
            content: await this.engine.parseAndRender(getEntityConstsTemplate(), ctx),
            layer: 'Domain',
        });

        // Repository interface
        files.push({
            path: `${projectNamespace}.Domain/${entity.name}/I${entity.name}Repository.cs`,
            content: await this.engine.parseAndRender(getRepositoryInterfaceTemplate(), ctx),
            layer: 'Domain',
        });

        // ============ APPLICATION LAYER ============
        // AppService
        files.push({
            path: `${projectNamespace}.Application/${entity.name}/${entity.name}AppService.cs`,
            content: await this.engine.parseAndRender(getAppServiceTemplate(), ctx),
            layer: 'Application',
        });

        // AutoMapper profile
        files.push({
            path: `${projectNamespace}.Application/${entity.name}/${entity.name}AutoMapperProfile.cs`,
            content: await this.engine.parseAndRender(getAutoMapperProfileTemplate(), ctx),
            layer: 'Application',
        });

        // ============ APPLICATION.CONTRACTS LAYER ============
        // AppService interface
        files.push({
            path: `${projectNamespace}.Application.Contracts/${entity.name}/I${entity.name}AppService.cs`,
            content: await this.engine.parseAndRender(getAppServiceInterfaceTemplate(), ctx),
            layer: 'Application.Contracts',
        });

        // DTOs
        files.push({
            path: `${projectNamespace}.Application.Contracts/${entity.name}/Dtos/${entity.name}Dto.cs`,
            content: await this.engine.parseAndRender(getDtoTemplate(), ctx),
            layer: 'Application.Contracts',
        });

        files.push({
            path: `${projectNamespace}.Application.Contracts/${entity.name}/Dtos/CreateUpdate${entity.name}Dto.cs`,
            content: await this.engine.parseAndRender(getCreateUpdateDtoTemplate(), ctx),
            layer: 'Application.Contracts',
        });

        files.push({
            path: `${projectNamespace}.Application.Contracts/${entity.name}/Dtos/${entity.name}GetListInput.cs`,
            content: await this.engine.parseAndRender(getGetListInputTemplate(), ctx),
            layer: 'Application.Contracts',
        });

        // Permissions
        files.push({
            path: `${projectNamespace}.Application.Contracts/Permissions/${entity.name}Permissions.cs`,
            content: await this.engine.parseAndRender(getPermissionsTemplate(), ctx),
            layer: 'Application.Contracts',
        });

        // ============ EF CORE LAYER ============
        // DbContext extensions
        files.push({
            path: `${projectNamespace}.EntityFrameworkCore/${entity.name}/${entity.name}DbContextExtensions.cs`,
            content: await this.engine.parseAndRender(getDbContextExtensionsTemplate(), ctx),
            layer: 'EntityFrameworkCore',
        });

        // Repository implementation
        files.push({
            path: `${projectNamespace}.EntityFrameworkCore/${entity.name}/Ef${entity.name}Repository.cs`,
            content: await this.engine.parseAndRender(getRepositoryImplementationTemplate(), ctx),
            layer: 'EntityFrameworkCore',
        });

        // ============ WEB LAYER ============

        // Localization EN
        files.push({
            path: `${projectNamespace}.Domain.Shared/Localization/${projectName}/en.json-merge`,
            content: await this.engine.parseAndRender(getLocalizationEntriesEnTemplate(), ctx),
            layer: 'Domain',
        });

        // Localization PT-BR
        files.push({
            path: `${projectNamespace}.Domain.Shared/Localization/${projectName}/pt-BR.json-merge`,
            content: await this.engine.parseAndRender(getLocalizationEntriesPtBrTemplate(), ctx),
            layer: 'Domain',
        });

        // ============ ENUM FILES (for enum fields) ============
        const enumFields = entity.fields.filter(f => f.type === 'enum' && f.enumConfig);
        for (const field of enumFields) {
            const enumCtx = {
                project: ctx.project,
                enumName: field.enumConfig!.enumName,
                options: field.enumConfig!.options,
            };

            // Enum class
            files.push({
                path: `${projectNamespace}.Domain.Shared/${field.enumConfig!.enumName}.cs`,
                content: await this.engine.parseAndRender(getEnumTemplate(), enumCtx),
                layer: 'Domain',
            });

            // Enum localization EN
            files.push({
                path: `${projectNamespace}.Domain.Shared/Localization/${projectName}/en-${field.enumConfig!.enumName}.json`,
                content: await this.engine.parseAndRender(getEnumLocalizationEnTemplate(), enumCtx),
                layer: 'Domain',
            });

            // Enum localization PT-BR
            files.push({
                path: `${projectNamespace}.Domain.Shared/Localization/${projectName}/pt-BR-${field.enumConfig!.enumName}.json`,
                content: await this.engine.parseAndRender(getEnumLocalizationPtBrTemplate(), enumCtx),
                layer: 'Domain',
            });
        }

        // ============ WEB/RAZOR PAGES ============
        // Index page
        files.push({
            path: `${projectNamespace}.Web/Pages/${entity.name}/Index.cshtml`,
            content: await this.engine.parseAndRender(getRazorIndexTemplate(), ctx),
            layer: 'Web',
        });
        files.push({
            path: `${projectNamespace}.Web/Pages/${entity.name}/Index.cshtml.cs`,
            content: await this.engine.parseAndRender(getRazorIndexModelTemplate(), ctx),
            layer: 'Web',
        });
        files.push({
            path: `${projectNamespace}.Web/Pages/${entity.name}/index.js`,
            content: await this.engine.parseAndRender(getRazorIndexJsTemplate(), ctx),
            layer: 'Web',
        });
        files.push({
            path: `${projectNamespace}.Web/Pages/${entity.name}/index.css`,
            content: await this.engine.parseAndRender(getRazorIndexCssTemplate(), ctx),
            layer: 'Web',
        });

        const hasChildGrid = asParent.some(r => r.isChildGrid);

        if (hasChildGrid) {
            // Full Page CRUD
            files.push({
                path: `${projectNamespace}.Web/Pages/${entity.name}/Create.cshtml`,
                content: await this.engine.parseAndRender(getRazorCreatePageViewTemplate(), ctx),
                layer: 'Web',
            });
            files.push({
                path: `${projectNamespace}.Web/Pages/${entity.name}/Create.cshtml.cs`,
                content: await this.engine.parseAndRender(getRazorCreatePageModelTemplate(), ctx),
                layer: 'Web',
            });

            files.push({
                path: `${projectNamespace}.Web/Pages/${entity.name}/Edit.cshtml`,
                content: await this.engine.parseAndRender(getRazorEditPageViewTemplate(), ctx),
                layer: 'Web',
            });
            files.push({
                path: `${projectNamespace}.Web/Pages/${entity.name}/Edit.cshtml.cs`,
                content: await this.engine.parseAndRender(getRazorEditPageModelTemplate(), ctx),
                layer: 'Web',
            });
        } else {
            // Modal CRUD (Default)
            // Create Modal
            files.push({
                path: `${projectNamespace}.Web/Pages/${entity.name}/CreateModal.cshtml`,
                content: await this.engine.parseAndRender(getRazorCreateModalViewTemplate(), ctx),
                layer: 'Web',
            });
            files.push({
                path: `${projectNamespace}.Web/Pages/${entity.name}/CreateModal.cshtml.cs`,
                content: await this.engine.parseAndRender(getRazorCreateModalModelTemplate(), ctx),
                layer: 'Web',
            });

            // Edit Modal
            files.push({
                path: `${projectNamespace}.Web/Pages/${entity.name}/EditModal.cshtml`,
                content: await this.engine.parseAndRender(getRazorEditModalViewTemplate(), ctx),
                layer: 'Web',
            });
            files.push({
                path: `${projectNamespace}.Web/Pages/${entity.name}/EditModal.cshtml.cs`,
                content: await this.engine.parseAndRender(getRazorEditModalModelTemplate(), ctx),
                layer: 'Web',
            });
        }

        // ViewModels
        files.push({
            path: `${projectNamespace}.Web/Pages/${entity.name}/ViewModels/Create${entity.name}ViewModel.cs`,
            content: await this.engine.parseAndRender(getRazorCreateViewModelTemplate(), ctx),
            layer: 'Web',
        });
        files.push({
            path: `${projectNamespace}.Web/Pages/${entity.name}/ViewModels/Edit${entity.name}ViewModel.cs`,
            content: await this.engine.parseAndRender(getRazorEditViewModelTemplate(), ctx),
            layer: 'Web',
        });

        // Master-Detail JS Logic (if applicable)
        // const hasChildGrid = asParent.some(r => r.isChildGrid); // Already computed above
        if (hasChildGrid) {
            if (entity.renderType === 'full-page') {
                files.push({
                    path: `${projectNamespace}.Web/Pages/${entity.name}/Create.js`,
                    content: await this.engine.parseAndRender(getRazorCreatePageJsTemplate(), ctx),
                    layer: 'Web',
                });
                files.push({
                    path: `${projectNamespace}.Web/Pages/${entity.name}/Edit.js`,
                    content: await this.engine.parseAndRender(getRazorEditPageJsTemplate(), ctx),
                    layer: 'Web',
                });
            } else {
                files.push({
                    path: `${projectNamespace}.Web/Pages/${entity.name}/CreateModal.js`,
                    content: await this.engine.parseAndRender(getRazorCreateModalJsTemplate(), ctx),
                    layer: 'Web',
                });
                files.push({
                    path: `${projectNamespace}.Web/Pages/${entity.name}/EditModal.js`,
                    content: await this.engine.parseAndRender(getRazorEditModalJsTemplate(), ctx),
                    layer: 'Web',
                });
            }
        }

        // Web AutoMapper profile
        files.push({
            path: `${projectNamespace}.Web/${entity.name}WebAutoMapperProfile.cs`,
            content: await this.engine.parseAndRender(getRazorAutoMapperProfileTemplate(), ctx),
            layer: 'Web',
        });

        return files;
    }

    /**
     * Generate React (Next.js) files for an entity
     */


    /**
     * Generate React V2 (Vite + Tailwind 4) files for an entity
     */
    async generateReactV2Files(
        entity: EntityData,
        projectName: string,
        projectNamespace: string,
        asParent: ParentRelationshipContext[] = [],
        asChild: ChildRelationshipContext[] = []
    ): Promise<GeneratedFile[]> {
        const ctx = this.createContext(entity, projectName, projectNamespace, asParent, asChild);
        const files: GeneratedFile[] = [];
        const kebabName = kebabCase(entity.name);

        // Determine if this entity should use full-page layout
        const hasChildGrid = asParent.some(r => r.isChildGrid);
        const isFullPage = entity.renderType === 'full-page' || hasChildGrid;

        if (isFullPage) {
            // Master-Detail: Full page layout with dedicated routes
            // List page (navigates to /new and /:id/edit)
            files.push({
                path: `abp-react-v2/src/pages/admin/${kebabName}/index.tsx`,
                content: await this.engine.parseAndRender(getReactV2MasterDetailListPageTemplate(), ctx),
                layer: 'React',
            });

            // Form page (used for both create and edit)
            // Add child entities context for the form template
            const formCtx = {
                ...ctx,
                entity: {
                    ...ctx.entity,
                    childEntities: asParent
                        .filter(r => r.isChildGrid)
                        .map(r => ({
                            entityName: r.childEntityName,
                            title: r.childGridConfig?.title || `${r.childEntityName}s`,
                            fields: r.targetFields || [],
                            displayFields: (r.targetFields || []).filter(f =>
                                f.name !== 'Id' &&
                                !f.name.endsWith('Id') &&
                                f.showInGrid !== false
                            ).slice(0, 5), // Limit to 5 display fields
                            requiredFields: (r.targetFields || []).filter(f => f.isRequired),
                        })),
                    manyToManyEntities: asParent
                        .filter(r => r.isManyToMany)
                        .map(r => ({
                            entityName: r.childEntityName,
                            pluralName: r.childPluralName,
                            title: r.navigationName || r.childPluralName,
                            relatedEntity: r.childEntityName,
                            // Infers the display field, favoring Name/Title
                            displayField: 'name'
                        }))
                }
            };

            files.push({
                path: `abp-react-v2/src/pages/admin/${kebabName}/form.tsx`,
                content: await this.engine.parseAndRender(getReactV2MasterDetailFormPageTemplate(), formCtx),
                layer: 'React',
            });

            // Card component (for grid view in master-detail listing)
            files.push({
                path: `abp-react-v2/src/components/${kebabName}/${pascalCase(entity.name)}Card.tsx`,
                content: await this.engine.parseAndRender(getReactV2CardComponentTemplate(), ctx),
                layer: 'React',
            });
        } else {
            // Modal layout (default for simple entities)
            // Page component
            files.push({
                path: `abp-react-v2/src/pages/admin/${kebabName}/index.tsx`,
                content: await this.engine.parseAndRender(getReactV2PageTemplate(), ctx),
                layer: 'React',
            });

            // List component
            files.push({
                path: `abp-react-v2/src/components/${kebabName}/${pascalCase(entity.name)}List.tsx`,
                content: await this.engine.parseAndRender(getReactV2ListComponentTemplate(), ctx),
                layer: 'React',
            });

            // Form component (modal)
            files.push({
                path: `abp-react-v2/src/components/${kebabName}/${pascalCase(entity.name)}Form.tsx`,
                content: await this.engine.parseAndRender(getReactV2FormComponentTemplate(), ctx),
                layer: 'React',
            });

            // Card component (for grid view)
            files.push({
                path: `abp-react-v2/src/components/${kebabName}/${pascalCase(entity.name)}Card.tsx`,
                content: await this.engine.parseAndRender(getReactV2CardComponentTemplate(), ctx),
                layer: 'React',
            });
        }

        // Hook (always generated)
        files.push({
            path: `abp-react-v2/src/lib/abp/hooks/use${pascalCase(entity.pluralName)}.ts`,
            content: await this.engine.parseAndRender(getReactV2HookTemplate(), ctx),
            layer: 'React',
        });

        return files;
    }

    /**
     * Generate Angular files for an entity (ABP style)
     */
    async generateAngularFiles(
        entity: EntityData,
        projectName: string,
        projectNamespace: string,
        asParent: ParentRelationshipContext[] = [],
        asChild: ChildRelationshipContext[] = []
    ): Promise<GeneratedFile[]> {
        const ctx = this.createContext(entity, projectName, projectNamespace, asParent, asChild);
        const files: GeneratedFile[] = [];
        const kebabName = kebabCase(entity.name);

        // Module
        files.push({
            path: `angular/src/app/${kebabName}/${kebabName}.module.ts`,
            content: await this.engine.parseAndRender(getAngularModuleTemplate(), ctx),
            layer: 'Angular',
        });

        // Routing
        files.push({
            path: `angular/src/app/${kebabName}/${kebabName}-routing.module.ts`,
            content: await this.engine.parseAndRender(getAngularRoutingModuleTemplate(), ctx),
            layer: 'Angular',
        });

        // Component TS
        files.push({
            path: `angular/src/app/${kebabName}/${kebabName}.component.ts`,
            content: await this.engine.parseAndRender(getAngularComponentTsTemplate(), ctx),
            layer: 'Angular',
        });

        // Component HTML
        files.push({
            path: `angular/src/app/${kebabName}/${kebabName}.component.html`,
            content: await this.engine.parseAndRender(getAngularComponentHtmlTemplate(), ctx),
            layer: 'Angular',
        });

        return files;
    }

    /**
     * Generate global Angular files (e.g., routing, menu)
     */
    async generateAngularGlobalFiles(
        entities: EntityData[],
        projectName: string,
        projectNamespace: string
    ): Promise<GeneratedFile[]> {
        const files: GeneratedFile[] = [];
        const ctx = {
            project: { name: projectName, namespace: projectNamespace },
            entities: entities
        };

        files.push({
            path: `angular/src/app/route.provider.ts`,
            content: await this.engine.parseAndRender(getAngularRouteProviderTemplate(), ctx),
            layer: 'Angular',
        });

        return files;
    }


    /**
     * Generate entity files with frontend selection
     */
    async generateEntityWithFrontends(
        entity: EntityData,
        projectName: string,
        projectNamespace: string,
        asParent: ParentRelationshipContext[] = [],
        asChild: ChildRelationshipContext[] = [],
        frontends: FrontendTarget[] = ['razor']
    ): Promise<GeneratedFile[]> {
        const files: GeneratedFile[] = [];

        // Always generate backend files
        const backendFiles = await this.generateEntity(entity, projectName, projectNamespace, asParent, asChild);

        // Filter based on frontend selection
        const hasRazor = frontends.includes('razor');
        const hasReactV2 = frontends.includes('react-v2');
        const hasAngular = frontends.includes('angular');

        // Backend files (always included)
        const backendLayers = ['Domain', 'Application', 'Application.Contracts', 'EntityFrameworkCore'];
        files.push(...backendFiles.filter(f => backendLayers.includes(f.layer)));

        // Razor files (if selected)
        if (hasRazor) {
            files.push(...backendFiles.filter(f => f.layer === 'Web'));
        }



        // React V2 files (if selected)
        if (hasReactV2) {
            const reactV2Files = await this.generateReactV2Files(entity, projectName, projectNamespace, asParent, asChild);
            files.push(...reactV2Files);
        }

        // Angular files (if selected)
        if (hasAngular) {
            const angularFiles = await this.generateAngularFiles(entity, projectName, projectNamespace, asParent, asChild);
            files.push(...angularFiles);
        }

        return files;
    }

    /**
     * Get global relationship contexts for all entities
     */
    public getGlobalRelationshipContexts(
        entities: EntityData[],
        relationships: RelationshipInfo[],
        entityIdMap?: Map<string, string>
    ): Array<{ entityName: string, asParent: ParentRelationshipContext[], asChild: ChildRelationshipContext[] }> {
        const entityMapByName = new Map<string, EntityData>(entities.map(e => [e.name, e]));

        const resolveEntityName = (idOrName: string): string | undefined => {
            if (entityMapByName.has(idOrName)) return idOrName;

            // Try ID map
            if (entityIdMap) {
                if (entityIdMap.has(idOrName)) return entityIdMap.get(idOrName);
                // Also check if idOrName IS an entity name in the map (reverse lookup or direct hit)
                for (const [, name] of entityIdMap.entries()) {
                    if (name === idOrName) return name;
                }
            }

            // Case-insensitive fallback
            for (const name of entityMapByName.keys()) {
                if (name.toLowerCase() === idOrName.toLowerCase()) return name;
            }
            return undefined;
        };

        return entities.map(entity => {
            const asParent: ParentRelationshipContext[] = [];
            const asChild: ChildRelationshipContext[] = [];

            // Tracking set for all member names in this entity to avoid CS0102 and CS0542
            const occupiedNames = new Set<string>();
            occupiedNames.add(entity.name);
            entity.fields.forEach(f => occupiedNames.add(f.name));
            ['Id', 'ExtraProperties', 'ConcurrencyStamp', 'CreationTime', 'CreatorId',
                'LastModificationTime', 'LastModifierId', 'IsDeleted', 'DeleterId', 'DeletionTime']
                .forEach(name => occupiedNames.add(name));

            for (const rel of relationships) {
                if (rel.data.type === 'one-to-many') {
                    const sourceName = resolveEntityName(rel.source);
                    const targetName = resolveEntityName(rel.target);

                    const sourceEntity = sourceName ? entityMapByName.get(sourceName) : undefined;
                    const targetEntity = targetName ? entityMapByName.get(targetName) : undefined;

                    if (sourceEntity && targetEntity) {
                        // If this entity is the parent (target)
                        if (targetEntity.name === entity.name) {
                            let navName = rel.data.sourceNavigationName || sourceEntity.pluralName;

                            // Collision resolution
                            let uniqueNavName = navName;
                            if (occupiedNames.has(uniqueNavName)) uniqueNavName = `${navName}Collection`;
                            let counter = 1;
                            while (occupiedNames.has(uniqueNavName)) {
                                uniqueNavName = `${navName}Collection${counter}`;
                                counter++;
                            }
                            occupiedNames.add(uniqueNavName);

                            // Avoid duplicates
                            if (!asParent.some(p => p.navigationName === uniqueNavName && p.childEntityName === sourceEntity.name)) {
                                asParent.push({
                                    childEntityName: sourceEntity.name,
                                    childPluralName: sourceEntity.pluralName,
                                    navigationName: uniqueNavName,
                                    childNavigationName: rel.data.targetNavigationName || targetEntity.name,
                                    childFkFieldName: `${targetEntity.name}Id`,
                                    // Fields for Master-Detail
                                    targetEntityName: sourceEntity.name,
                                    targetPluralName: sourceEntity.pluralName,
                                    isChildGrid: rel.data.isChildGrid,
                                    childGridConfig: rel.data.childGridConfig,
                                    targetFields: sourceEntity.fields
                                });
                            }
                        }

                        // If this entity is the child (source)
                        if (sourceEntity.name === entity.name) {
                            const fkField = sourceEntity.fields.find(f =>
                                f.isLookup && f.lookupConfig?.targetEntity === targetEntity.name
                            );

                            const displayFieldField = targetEntity.fields.find(f => f.name === 'Name' || f.name === 'name')
                                || targetEntity.fields.find(f => f.name === 'Title' || f.name === 'title')
                                || targetEntity.fields.find(f => f.type === 'string')
                                || { name: 'Id' };

                            let navName = rel.data.targetNavigationName || targetEntity.name;

                            // Collision resolution
                            let uniqueNavName = navName;
                            if (occupiedNames.has(uniqueNavName)) uniqueNavName = `${navName}Nav`;
                            let counter = 1;
                            while (occupiedNames.has(uniqueNavName)) {
                                uniqueNavName = `${navName}Nav${counter}`;
                                counter++;
                            }
                            occupiedNames.add(uniqueNavName);

                            // Avoid duplicates
                            const fkName = fkField?.name || `${targetEntity.name}Id`;
                            if (!asChild.some(c => c.fkFieldName === fkName && c.parentEntityName === targetEntity.name)) {
                                asChild.push({
                                    parentEntityName: targetEntity.name,
                                    parentPluralName: targetEntity.pluralName,
                                    fkFieldName: fkName,
                                    navigationName: uniqueNavName,
                                    parentNavigationName: rel.data.sourceNavigationName || sourceEntity.pluralName,
                                    isRequired: rel.data.isRequired,
                                    lookupMode: fkField?.lookupConfig?.mode || 'dropdown',
                                    displayField: displayFieldField.name
                                });
                            }
                        }
                    }
                } else if (rel.data.type === 'many-to-many') {
                    const sourceName = resolveEntityName(rel.source);
                    const targetName = resolveEntityName(rel.target);

                    const sourceEntity = sourceName ? entityMapByName.get(sourceName) : undefined;
                    const targetEntity = targetName ? entityMapByName.get(targetName) : undefined;

                    if (sourceEntity && targetEntity && rel.data.junctionConfig) {
                        const junctionId = rel.data.junctionConfig.junctionEntityId;
                        const junctionName = junctionId ? resolveEntityName(junctionId) : rel.data.junctionConfig.tableName;
                        const junctionEntity = entities.find(e => e.name === junctionName);

                        if (junctionEntity) {
                            // If this entity is the source
                            if (sourceEntity.name === entity.name && rel.data.junctionConfig.showInSource) {
                                let navName = rel.data.sourceNavigationName || junctionEntity.pluralName;
                                let uniqueNavName = navName;
                                if (occupiedNames.has(uniqueNavName)) uniqueNavName = `${navName}Collection`;
                                let counter = 1;
                                while (occupiedNames.has(uniqueNavName)) {
                                    uniqueNavName = `${navName}Collection${counter}`;
                                    counter++;
                                }
                                occupiedNames.add(uniqueNavName);

                                if (!asParent.some(p => p.navigationName === uniqueNavName && p.childEntityName === junctionEntity.name)) {
                                    asParent.push({
                                        childEntityName: junctionEntity.name,
                                        childPluralName: junctionEntity.pluralName,
                                        navigationName: uniqueNavName,
                                        childNavigationName: sourceEntity.name,
                                        childFkFieldName: `${sourceEntity.name}Id`,
                                        targetEntityName: junctionEntity.name,
                                        targetPluralName: junctionEntity.pluralName,
                                        isChildGrid: true,
                                        isManyToMany: true,
                                        junctionConfig: rel.data.junctionConfig,
                                        childGridConfig: {
                                            title: targetEntity.pluralName,
                                            allowAdd: true,
                                            allowRemove: true,
                                            allowEdit: false,
                                            renderMode: 'tab'
                                        },
                                        targetFields: junctionEntity.fields
                                    });
                                }
                            }
                            // If this entity is the target
                            if (targetEntity.name === entity.name && rel.data.junctionConfig.showInTarget) {
                                let navName = rel.data.targetNavigationName || junctionEntity.pluralName;
                                let uniqueNavName = navName;
                                if (occupiedNames.has(uniqueNavName)) uniqueNavName = `${navName}Collection`;
                                let counter = 1;
                                while (occupiedNames.has(uniqueNavName)) {
                                    uniqueNavName = `${navName}Collection${counter}`;
                                    counter++;
                                }
                                occupiedNames.add(uniqueNavName);

                                if (!asParent.some(p => p.navigationName === uniqueNavName && p.childEntityName === junctionEntity.name)) {
                                    asParent.push({
                                        childEntityName: junctionEntity.name,
                                        childPluralName: junctionEntity.pluralName,
                                        navigationName: uniqueNavName,
                                        childNavigationName: targetEntity.name,
                                        childFkFieldName: `${targetEntity.name}Id`,
                                        targetEntityName: junctionEntity.name,
                                        targetPluralName: junctionEntity.pluralName,
                                        isChildGrid: true,
                                        isManyToMany: true,
                                        junctionConfig: rel.data.junctionConfig,
                                        childGridConfig: {
                                            title: sourceEntity.pluralName,
                                            allowAdd: true,
                                            allowRemove: true,
                                            allowEdit: false,
                                            renderMode: 'tab'
                                        },
                                        targetFields: junctionEntity.fields
                                    });
                                }
                            }

                            // If this entity IS the junction entity
                            if (junctionEntity.name === entity.name) {
                                const sourceDisplayField = sourceEntity.fields.find(f => f.name === 'Name' || f.name === 'name')
                                    || sourceEntity.fields.find(f => f.name === 'Nome')
                                    || sourceEntity.fields.find(f => f.type === 'string')
                                    || { name: 'Id' };

                                let sourceNavName = sourceEntity.name;
                                if (occupiedNames.has(sourceNavName)) sourceNavName = `${sourceEntity.name}Nav`;
                                occupiedNames.add(sourceNavName);

                                if (!asChild.some(c => c.fkFieldName === (rel.data.junctionConfig!.sourceForeignKey || `${sourceEntity.name}Id`))) {
                                    asChild.push({
                                        parentEntityName: sourceEntity.name,
                                        parentPluralName: sourceEntity.pluralName,
                                        fkFieldName: rel.data.junctionConfig.sourceForeignKey || `${sourceEntity.name}Id`,
                                        navigationName: sourceNavName,
                                        parentNavigationName: junctionEntity.pluralName,
                                        isRequired: true,
                                        lookupMode: 'dropdown',
                                        displayField: sourceDisplayField.name
                                    });
                                }

                                const targetDisplayField = targetEntity.fields.find(f => f.name === 'Name' || f.name === 'name')
                                    || targetEntity.fields.find(f => f.name === 'Nome')
                                    || targetEntity.fields.find(f => f.type === 'string')
                                    || { name: 'Id' };

                                let targetNavName = targetEntity.name;
                                if (occupiedNames.has(targetNavName)) targetNavName = `${targetEntity.name}Nav`;
                                occupiedNames.add(targetNavName);

                                if (!asChild.some(c => c.fkFieldName === (rel.data.junctionConfig!.targetForeignKey || `${targetEntity.name}Id`))) {
                                    asChild.push({
                                        parentEntityName: targetEntity.name,
                                        parentPluralName: targetEntity.pluralName,
                                        fkFieldName: rel.data.junctionConfig.targetForeignKey || `${targetEntity.name}Id`,
                                        navigationName: targetNavName,
                                        parentNavigationName: junctionEntity.pluralName,
                                        isRequired: true,
                                        lookupMode: 'dropdown',
                                        displayField: targetDisplayField.name
                                    });
                                }
                            }
                        }
                    }
                }
            }

            // AFTER processing all edges, check for any fields marked as isLookup 
            // that don't have a corresponding relationship in asChild yet.
            for (const field of entity.fields) {
                if (field.isLookup && field.lookupConfig?.targetEntity) {
                    const targetEntityName = resolveEntityName(field.lookupConfig.targetEntity);
                    const targetEntity = targetEntityName ? entityMapByName.get(targetEntityName) : undefined;

                    if (targetEntity) {
                        const alreadyAdded = asChild.some(c => c.parentEntityName === targetEntity.name || c.fkFieldName === field.name);

                        if (!alreadyAdded) {
                            let navName = targetEntity.name;
                            let uniqueNavName = navName;
                            if (occupiedNames.has(uniqueNavName)) uniqueNavName = `${navName}Nav`;
                            let counter = 1;
                            while (occupiedNames.has(uniqueNavName)) {
                                uniqueNavName = `${navName}Nav${counter}`;
                                counter++;
                            }
                            occupiedNames.add(uniqueNavName);

                            const displayFieldField = targetEntity.fields.find(f => f.name === 'Name' || f.name === 'name')
                                || targetEntity.fields.find(f => f.name === 'Title' || f.name === 'title')
                                || targetEntity.fields.find(f => f.name === 'Nome' || f.name === 'nome')
                                || targetEntity.fields.find(f => f.type === 'string')
                                || { name: 'Id' };

                            asChild.push({
                                parentEntityName: targetEntity.name,
                                parentPluralName: targetEntity.pluralName,
                                fkFieldName: field.name,
                                navigationName: uniqueNavName,
                                parentNavigationName: entity.pluralName,
                                isRequired: field.isRequired || false,
                                lookupMode: field.lookupConfig.mode || 'dropdown',
                                displayField: displayFieldField.name
                            });
                        }
                    }
                }
            }

            return { entityName: entity.name, asParent, asChild };
        });
    }

    public async generateAll(
        entities: EntityData[],
        relationships: RelationshipInfo[],
        projectName: string,
        projectNamespace: string,
        frontends: FrontendTarget[] = ['razor'],
        entityIdMap?: Map<string, string>
    ): Promise<GeneratedFile[]> {
        const allFiles: GeneratedFile[] = [];

        // Generate global Angular files if selected
        if (frontends.includes('angular')) {
            const globalAngularFiles = await this.generateAngularGlobalFiles(entities, projectName, projectNamespace);
            allFiles.push(...globalAngularFiles);
        }

        const contexts = this.getGlobalRelationshipContexts(entities, relationships, entityIdMap);
        const contextMap = new Map(contexts.map(c => [c.entityName, c]));

        for (const entity of entities) {
            const ctx = contextMap.get(entity.name);
            const asParent = ctx?.asParent || [];
            const asChild = ctx?.asChild || [];

            const entityFiles = await this.generateEntityWithFrontends(
                entity,
                projectName,
                projectNamespace,
                asParent,
                asChild,
                frontends
            );
            allFiles.push(...entityFiles);
        }

        // Check if any entity has modal lookups - if so, generate shared ZenLookup files
        const hasModalLookups = entities.some(entity =>
            entity.fields.some(f => f.isLookup && f.lookupConfig?.mode === 'modal')
        );

        if (hasModalLookups) {
            const sharedCtx = { project: { name: projectName, namespace: projectNamespace } };

            // ZenLookup TagHelper
            allFiles.push({
                path: `${projectNamespace}.Web/TagHelpers/ZenLookupInputTagHelper.cs`,
                content: await this.engine.parseAndRender(getZenLookupTagHelperTemplate(), sharedCtx),
                layer: 'Web',
            });

            // ZenLookup Modal Partial
            allFiles.push({
                path: `${projectNamespace}.Web/Shared/_ZenLookupModal.cshtml`,
                content: await this.engine.parseAndRender(getZenLookupModalTemplate(), sharedCtx),
                layer: 'Web',
            });

            // ZenLookup JavaScript
            allFiles.push({
                path: `${projectNamespace}.Web/wwwroot/libs/zen/lookup.js`,
                content: await this.engine.parseAndRender(getZenLookupJsTemplate(), sharedCtx),
                layer: 'Web',
            });

            // ZenLookup CSS
            allFiles.push({
                path: `${projectNamespace}.Web/wwwroot/libs/zen/lookup.css`,
                content: await this.engine.parseAndRender(getZenLookupCssTemplate(), sharedCtx),
                layer: 'Web',
            });
        }

        return allFiles;
    }
}

export const codeGenerator = new CodeGenerator();
