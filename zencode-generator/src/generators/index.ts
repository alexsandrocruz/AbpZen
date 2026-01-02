import { Liquid } from 'liquidjs';
import type { EntityData, EntityField, RelationshipData } from '../types';
import {
    pluralize,
    camelCase,
    pascalCase,
    kebabCase,
    snakeCase,
    toCSharpType,
    toTypeScriptType
} from './filters';

// Templates as string constants
import { getAppServiceTemplate, getAppServiceInterfaceTemplate } from './templates/service';
import { getDtoTemplate, getCreateUpdateDtoTemplate, getGetListInputTemplate } from './templates/dto';
import { getDbContextExtensionsTemplate } from './templates/efcore';
import { getPermissionsTemplate } from './templates/permissions';
import { getEntityTemplate, getEntityConstsTemplate } from './templates/entity';
import { getRepositoryInterfaceTemplate, getRepositoryImplementationTemplate } from './templates/repository';
import { getAutoMapperProfileTemplate } from './templates/automapper';
import { getMenuContributorTemplate } from './templates/menu';
import {
    getLocalizationEntriesEnTemplate,
    getLocalizationEntriesPtBrTemplate,
    getLocalizationJsonTemplate,
    getLocalizationJsonPtBrTemplate
} from './templates/localization';
import { getEnumTemplate, getEnumLocalizationEnTemplate, getEnumLocalizationPtBrTemplate } from './templates/enum';
// Razor page templates
import { getRazorIndexTemplate, getRazorIndexModelTemplate, getRazorIndexJsTemplate, getRazorIndexCssTemplate } from './templates/razor-index';
import { getRazorCreateModalViewTemplate, getRazorCreateModalModelTemplate, getRazorEditModalViewTemplate, getRazorEditModalModelTemplate } from './templates/razor-modal';
import { getRazorCreateViewModelTemplate, getRazorEditViewModelTemplate, getRazorAutoMapperProfileTemplate } from './templates/razor-viewmodel';

/**
 * Parent relationship context (this entity has a collection of children)
 */
export interface ParentRelationshipContext {
    childEntityName: string;
    childPluralName: string;
    navigationName: string;        // Collection property name (e.g., "Products")
    childNavigationName: string;   // Child's FK navigation name (e.g., "Category")
    childFkFieldName: string;      // Child's FK field (e.g., "CategoryId")
}

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
}

/**
 * Context passed to templates
 */
export interface GeneratorContext {
    project: {
        name: string;
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
}

/**
 * Generated file
 */
export interface GeneratedFile {
    path: string;
    content: string;
    layer: 'Domain' | 'Application' | 'Application.Contracts' | 'EntityFrameworkCore' | 'Web';
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
        return {
            project: {
                name: projectName,
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
            path: `${projectNamespace}.Application.Contracts/Permissions/${projectName}Permissions.${entity.name}.cs`,
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

        // Web AutoMapper profile
        files.push({
            path: `${projectNamespace}.Web/${entity.name}WebAutoMapperProfile.cs`,
            content: await this.engine.parseAndRender(getRazorAutoMapperProfileTemplate(), ctx),
            layer: 'Web',
        });

        return files;
    }

    /**
     * Generate all files for multiple entities with relationship support
     */
    async generateAll(
        entities: EntityData[],
        projectName: string,
        projectNamespace: string,
        relationships: { id: string; source: string; target: string; data: RelationshipData }[] = []
    ): Promise<GeneratedFile[]> {
        const allFiles: GeneratedFile[] = [];

        // Build entity lookup map (id -> EntityData with pluralName)
        const entityMap = new Map<string, EntityData>();
        for (const entity of entities) {
            // We need to find the entity ID, but entities array has EntityData, not {id, data}
            // This assumes entities are passed with their IDs tracked elsewhere
            // For now, we'll use entity.name as the key
            entityMap.set(entity.name, entity);
        }

        for (const entity of entities) {
            // Compute relationships for this entity
            const asParent: ParentRelationshipContext[] = [];
            const asChild: ChildRelationshipContext[] = [];

            for (const rel of relationships) {
                if (rel.data.type === 'one-to-many') {
                    // Source is the "One" side (parent), Target is the "Many" side (child)
                    const sourceEntity = entityMap.get(rel.source);
                    const targetEntity = entityMap.get(rel.target);

                    if (sourceEntity && targetEntity) {
                        // If this entity is the source (parent)
                        if (rel.source === entity.name) {
                            asParent.push({
                                childEntityName: targetEntity.name,
                                childPluralName: targetEntity.pluralName,
                                navigationName: rel.data.sourceNavigationName || targetEntity.pluralName,
                                childNavigationName: rel.data.targetNavigationName || sourceEntity.name,
                                childFkFieldName: `${sourceEntity.name}Id`,
                            });
                        }

                        // If this entity is the target (child)
                        if (rel.target === entity.name) {
                            asChild.push({
                                parentEntityName: sourceEntity.name,
                                parentPluralName: sourceEntity.pluralName,
                                fkFieldName: `${sourceEntity.name}Id`,
                                navigationName: rel.data.targetNavigationName || sourceEntity.name,
                                parentNavigationName: rel.data.sourceNavigationName || targetEntity.pluralName,
                                isRequired: rel.data.isRequired,
                            });
                        }
                    }
                }
            }

            const entityFiles = await this.generateEntity(
                entity,
                projectName,
                projectNamespace,
                asParent,
                asChild
            );
            allFiles.push(...entityFiles);
        }

        return allFiles;
    }
}

export const codeGenerator = new CodeGenerator();
