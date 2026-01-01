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

        // Application layer
        files.push({
            path: `${projectNamespace}.Application/${entity.name}/${entity.name}AppService.cs`,
            content: await this.engine.parseAndRender(getAppServiceTemplate(), ctx),
            layer: 'Application',
        });

        // Application.Contracts layer
        files.push({
            path: `${projectNamespace}.Application.Contracts/${entity.name}/I${entity.name}AppService.cs`,
            content: await this.engine.parseAndRender(getAppServiceInterfaceTemplate(), ctx),
            layer: 'Application.Contracts',
        });

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

        // EF Core
        files.push({
            path: `${projectNamespace}.EntityFrameworkCore/${entity.name}DbContextExtensions.cs`,
            content: await this.engine.parseAndRender(getDbContextExtensionsTemplate(), ctx),
            layer: 'EntityFrameworkCore',
        });

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
