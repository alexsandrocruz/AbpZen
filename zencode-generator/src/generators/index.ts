import { Liquid } from 'liquidjs';
import type { EntityData, EntityField } from '../types';
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
import { getLocalizationJsonTemplate, getLocalizationJsonPtBrTemplate } from './templates/localization';
import { getEnumTemplate, getEnumLocalizationEnTemplate, getEnumLocalizationPtBrTemplate } from './templates/enum';

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
     * Create context from EntityData
     */
    createContext(entity: EntityData, projectName: string, projectNamespace: string): GeneratorContext {
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
        };
    }

    /**
     * Generate all files for an entity
     */
    async generateEntity(entity: EntityData, projectName: string, projectNamespace: string): Promise<GeneratedFile[]> {
        const ctx = this.createContext(entity, projectName, projectNamespace);
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
        // Menu contributor
        files.push({
            path: `${projectNamespace}.Web/Menus/${entity.name}MenuContributor.cs`,
            content: await this.engine.parseAndRender(getMenuContributorTemplate(), ctx),
            layer: 'Web',
        });

        // Localization EN
        files.push({
            path: `${projectNamespace}.Domain.Shared/Localization/${projectName}/en-${entity.name}.json`,
            content: await this.engine.parseAndRender(getLocalizationJsonTemplate(), ctx),
            layer: 'Domain',
        });

        // Localization PT-BR
        files.push({
            path: `${projectNamespace}.Domain.Shared/Localization/${projectName}/pt-BR-${entity.name}.json`,
            content: await this.engine.parseAndRender(getLocalizationJsonPtBrTemplate(), ctx),
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

        return files;
    }

    /**
     * Generate all files for multiple entities
     */
    async generateAll(entities: EntityData[], projectName: string, projectNamespace: string): Promise<GeneratedFile[]> {
        const allFiles: GeneratedFile[] = [];

        for (const entity of entities) {
            const entityFiles = await this.generateEntity(entity, projectName, projectNamespace);
            allFiles.push(...entityFiles);
        }

        return allFiles;
    }
}

export const codeGenerator = new CodeGenerator();
