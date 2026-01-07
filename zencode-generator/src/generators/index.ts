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

// React (Next.js) templates
import {
    getReactPageTemplate,
    getReactListComponentTemplate,
    getReactAddComponentTemplate,
    getReactEditComponentTemplate,
    getReactDeleteComponentTemplate,
    getReactFormComponentTemplate,
    getReactHookTemplate,
} from './templates/react/index.ts';

// Angular templates
import {
    getAngularModuleTemplate,
    getAngularRoutingModuleTemplate,
    getAngularComponentTsTemplate,
    getAngularComponentHtmlTemplate,
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
                asParent,
                asChild,
            },
            isMasterDetail: asParent.some(r => r.isChildGrid)
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
    async generateReactFiles(
        entity: EntityData,
        projectName: string,
        projectNamespace: string,
        asParent: ParentRelationshipContext[] = [],
        asChild: ChildRelationshipContext[] = []
    ): Promise<GeneratedFile[]> {
        const ctx = this.createContext(entity, projectName, projectNamespace, asParent, asChild);
        const files: GeneratedFile[] = [];
        const kebabName = entity.name.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase();

        // Page component
        files.push({
            path: `abp-react/src/app/admin/${kebabName}/page.tsx`,
            content: await this.engine.parseAndRender(getReactPageTemplate(), ctx),
            layer: 'React',
        });

        // List component
        files.push({
            path: `abp-react/src/components/${kebabName}/${entity.name}List.tsx`,
            content: await this.engine.parseAndRender(getReactListComponentTemplate(), ctx),
            layer: 'React',
        });

        // Add component
        files.push({
            path: `abp-react/src/components/${kebabName}/Add${entity.name}.tsx`,
            content: await this.engine.parseAndRender(getReactAddComponentTemplate(), ctx),
            layer: 'React',
        });

        // Edit component
        files.push({
            path: `abp-react/src/components/${kebabName}/${entity.name}Edit.tsx`,
            content: await this.engine.parseAndRender(getReactEditComponentTemplate(), ctx),
            layer: 'React',
        });

        // Delete component
        files.push({
            path: `abp-react/src/components/${kebabName}/Delete${entity.name}.tsx`,
            content: await this.engine.parseAndRender(getReactDeleteComponentTemplate(), ctx),
            layer: 'React',
        });

        // Form component
        files.push({
            path: `abp-react/src/components/${kebabName}/${entity.name}Form.tsx`,
            content: await this.engine.parseAndRender(getReactFormComponentTemplate(), ctx),
            layer: 'React',
        });

        // Hook
        files.push({
            path: `abp-react/src/lib/hooks/use${entity.pluralName}.ts`,
            content: await this.engine.parseAndRender(getReactHookTemplate(), ctx),
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
        const kebabName = entity.name.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase();

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
        const hasReact = frontends.includes('react');
        const hasAngular = frontends.includes('angular');

        // Backend files (always included)
        const backendLayers = ['Domain', 'Application', 'Application.Contracts', 'EntityFrameworkCore'];
        files.push(...backendFiles.filter(f => backendLayers.includes(f.layer)));

        // Razor files (if selected)
        if (hasRazor) {
            files.push(...backendFiles.filter(f => f.layer === 'Web'));
        }

        // React files (if selected)
        if (hasReact) {
            const reactFiles = await this.generateReactFiles(entity, projectName, projectNamespace, asParent, asChild);
            files.push(...reactFiles);
        }

        // Angular files (if selected)
        if (hasAngular) {
            const angularFiles = await this.generateAngularFiles(entity, projectName, projectNamespace, asParent, asChild);
            files.push(...angularFiles);
        }

        return files;
    }

    /**
     * Generate all files for multiple entities with relationship support
     */
    public async generateAll(
        entities: EntityData[],
        relationships: RelationshipInfo[],
        projectName: string,
        projectNamespace: string,
        frontends: FrontendTarget[] = ['razor'],
        entityIdMap?: Map<string, string> // Optional: maps entity ID to entity name
    ): Promise<GeneratedFile[]> {
        const allFiles: GeneratedFile[] = [];
        const entityMapByName = new Map<string, EntityData>(entities.map(e => [e.name, e]));

        // If entityIdMap is provided, use it; otherwise try to extract from relationship source/target
        // by matching against entity names (fallback for when IDs are entity names)
        const resolveEntityName = (idOrName: string): string | undefined => {
            // First, check if it's an entity name directly
            if (entityMapByName.has(idOrName)) {
                return idOrName;
            }
            // Then, check if we have an ID map
            if (entityIdMap && entityIdMap.has(idOrName)) {
                return entityIdMap.get(idOrName);
            }
            // Fallback: check if any entity's lookupConfig.targetEntity matches
            return undefined;
        };

        for (const entity of entities) {
            const asParent: ParentRelationshipContext[] = [];
            const asChild: ChildRelationshipContext[] = [];

            for (const rel of relationships) {
                if (rel.data.type === 'one-to-many') {
                    const sourceName = resolveEntityName(rel.source);
                    const targetName = resolveEntityName(rel.target);

                    const sourceEntity = sourceName ? entityMapByName.get(sourceName) : undefined;
                    const targetEntity = targetName ? entityMapByName.get(targetName) : undefined;

                    if (sourceEntity && targetEntity) {
                        // If this entity is the parent (target)
                        if (targetEntity.name === entity.name) {
                            asParent.push({
                                childEntityName: sourceEntity.name,
                                childPluralName: sourceEntity.pluralName,
                                navigationName: rel.data.sourceNavigationName || sourceEntity.pluralName,
                                childNavigationName: rel.data.targetNavigationName || targetEntity.name,
                                childFkFieldName: `${targetEntity.name}Id`,
                                // New fields for Master-Detail (mapped to ParentRelationshipContext)
                                targetEntityName: sourceEntity.name,
                                targetPluralName: sourceEntity.pluralName,
                                isChildGrid: rel.data.isChildGrid,
                                childGridConfig: rel.data.childGridConfig,
                                targetFields: sourceEntity.fields
                            });
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

                            asChild.push({
                                parentEntityName: targetEntity.name,
                                parentPluralName: targetEntity.pluralName,
                                fkFieldName: fkField?.name || `${targetEntity.name}Id`,
                                navigationName: rel.data.targetNavigationName || targetEntity.name,
                                parentNavigationName: rel.data.sourceNavigationName || sourceEntity.pluralName,
                                isRequired: rel.data.isRequired,
                                lookupMode: fkField?.lookupConfig?.mode || 'dropdown',
                                displayField: displayFieldField.name
                            });
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
                            if (sourceEntity.name === entity.name && rel.data.junctionConfig.showInSource) {
                                asParent.push({
                                    childEntityName: junctionEntity.name,
                                    childPluralName: junctionEntity.pluralName,
                                    navigationName: rel.data.sourceNavigationName || junctionEntity.pluralName,
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
                            if (targetEntity.name === entity.name && rel.data.junctionConfig.showInTarget) {
                                asParent.push({
                                    childEntityName: junctionEntity.name,
                                    childPluralName: junctionEntity.pluralName,
                                    navigationName: rel.data.targetNavigationName || junctionEntity.pluralName,
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

                            // If this entity IS the junction entity, add both source and target as asChild
                            // so that the junction entity gets the FK properties (AlunoId, TurmaId, etc.)
                            if (junctionEntity.name === entity.name) {
                                // Add source entity as parent (e.g., Aluno)
                                const sourceDisplayField = sourceEntity.fields.find(f => f.name === 'Name' || f.name === 'name')
                                    || sourceEntity.fields.find(f => f.name === 'Nome')
                                    || sourceEntity.fields.find(f => f.type === 'string')
                                    || { name: 'Id' };

                                asChild.push({
                                    parentEntityName: sourceEntity.name,
                                    parentPluralName: sourceEntity.pluralName,
                                    fkFieldName: rel.data.junctionConfig.sourceForeignKey || `${sourceEntity.name}Id`,
                                    navigationName: sourceEntity.name,
                                    parentNavigationName: junctionEntity.pluralName,
                                    isRequired: true,
                                    lookupMode: 'dropdown',
                                    displayField: sourceDisplayField.name
                                });

                                // Add target entity as parent (e.g., Turma)
                                const targetDisplayField = targetEntity.fields.find(f => f.name === 'Name' || f.name === 'name')
                                    || targetEntity.fields.find(f => f.name === 'Nome')
                                    || targetEntity.fields.find(f => f.type === 'string')
                                    || { name: 'Id' };

                                asChild.push({
                                    parentEntityName: targetEntity.name,
                                    parentPluralName: targetEntity.pluralName,
                                    fkFieldName: rel.data.junctionConfig.targetForeignKey || `${targetEntity.name}Id`,
                                    navigationName: targetEntity.name,
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
