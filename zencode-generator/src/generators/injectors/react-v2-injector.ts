/**
 * React V2 Route & Menu Injector
 * 
 * This module provides functions to automatically inject new entity routes
 * and menu items into the React V2 boilerplate's navigation.tsx file.
 * 
 * Uses ts-morph for AST manipulation to ensure safe code injection.
 */

import { Project, SyntaxKind } from 'ts-morph';
import * as path from 'path';
import * as fs from 'fs';

export interface InjectorConfig {
    /** Path to the React V2 src directory */
    reactV2SrcPath: string;
    /** Entity name in PascalCase (e.g., "Product") */
    entityName: string;
    /** Plural name in PascalCase (e.g., "Products") */
    pluralName: string;
    /** Icon name from lucide-react (e.g., "Package") */
    iconName?: string;
    /** Menu section: 'main' | 'admin' | 'host' | 'entities' */
    section?: 'main' | 'admin' | 'host' | 'entities';
}

/**
 * Converts PascalCase to kebab-case
 */
function toKebabCase(str: string): string {
    return str
        .replace(/([a-z])([A-Z])/g, '$1-$2')
        .replace(/[\s_]+/g, '-')
        .toLowerCase();
}

/**
 * Injects a new page import and route into navigation.tsx
 */
export async function injectReactV2Route(config: InjectorConfig): Promise<{ success: boolean; message: string }> {
    const { reactV2SrcPath, entityName } = config;
    const navigationPath = path.join(reactV2SrcPath, 'config', 'navigation.tsx');
    const kebabName = toKebabCase(entityName);

    try {
        if (!fs.existsSync(navigationPath)) {
            return { success: false, message: `Navigation file not found at ${navigationPath}` };
        }

        const project = new Project();
        const sourceFile = project.addSourceFileAtPath(navigationPath);

        // 1. Add import statement before // <GEN-IMPORTS> marker
        const importMarker = '// <GEN-IMPORTS>';
        const fileText = sourceFile.getFullText();

        const routePath = `/admin/${kebabName}`;
        if (fileText.includes(`path: "${routePath}"`) || fileText.includes(`path: '${routePath}'`)) {
            return { success: false, message: `Route for ${entityName} already exists.` };
        }

        const newImport = `import ${entityName}Page from "@/pages/admin/${kebabName}";\nimport ${entityName}FormPage from "@/pages/admin/${kebabName}/form";\n`;
        const updatedImports = fileText.replace(importMarker, `${newImport}${importMarker}`);
        sourceFile.replaceWithText(updatedImports);

        // 2. Add route to routes array before // <GEN-ROUTES> marker
        const routeMarker = '// <GEN-ROUTES>';
        const currentText = sourceFile.getFullText();
        const newRoute = `    { path: "${routePath}", component: ${entityName}Page },\n    { path: "${routePath}/create", component: ${entityName}FormPage },\n    { path: "${routePath}/:id/edit", component: ${entityName}FormPage },\n    `;
        const updatedRoutes = currentText.replace(routeMarker, `${newRoute}${routeMarker}`);
        sourceFile.replaceWithText(updatedRoutes);

        await sourceFile.save();

        return { success: true, message: `Successfully injected route for ${entityName} at ${routePath}` };
    } catch (error) {
        return { success: false, message: `Failed to inject route: ${error}` };
    }
}

/**
 * Injects a new menu item into the menuItems array in navigation.tsx
 */
export async function injectReactV2MenuItem(config: InjectorConfig): Promise<{ success: boolean; message: string }> {
    const { reactV2SrcPath, entityName, pluralName, iconName = 'Box', section = 'entities' } = config;
    const navigationPath = path.join(reactV2SrcPath, 'config', 'navigation.tsx');
    const kebabName = toKebabCase(entityName); // Use entityName to be consistent with route

    try {
        if (!fs.existsSync(navigationPath)) {
            return { success: false, message: `Navigation file not found at ${navigationPath}` };
        }

        const project = new Project();
        const sourceFile = project.addSourceFileAtPath(navigationPath);

        // Find the menuItems array
        const menuItemsDecl = sourceFile.getVariableDeclaration('menuItems');
        if (!menuItemsDecl) {
            return { success: false, message: 'Could not find menuItems declaration.' };
        }

        const initializer = menuItemsDecl.getInitializerIfKind(SyntaxKind.ArrayLiteralExpression);
        if (!initializer) {
            return { success: false, message: 'menuItems is not an array.' };
        }

        // Check if item already exists by href
        const href = `/admin/${kebabName}`;
        const existingItem = initializer.getElements().find(el => {
            const text = el.getText();
            return text.includes(`href: "${href}"`) || text.includes(`href: '${href}'`);
        });

        if (existingItem) {
            return { success: false, message: `Menu item for ${entityName} already exists.` };
        }

        // Add icon import if not present
        const fileText = sourceFile.getFullText();
        if (!fileText.includes(`${iconName},`) && !fileText.includes(`${iconName} }`)) {
            const importDecl = sourceFile.getImportDeclarations().find(
                decl => decl.getModuleSpecifierValue() === 'lucide-react'
            );
            if (importDecl) {
                const namedImports = importDecl.getNamedImports();
                if (!namedImports.some(ni => ni.getName() === iconName)) {
                    importDecl.addNamedImport(iconName);
                }
            } else {
                sourceFile.addImportDeclaration({
                    namedImports: [iconName],
                    moduleSpecifier: 'lucide-react'
                });
            }
        }

        // Add new menu item
        const newItem = `{ label: "${pluralName}", href: "/admin/${kebabName}", icon: ${iconName}, section: "${section}" }`;
        initializer.addElement(newItem);

        await sourceFile.save();

        return { success: true, message: `Successfully injected menu item for ${pluralName}` };
    } catch (error) {
        return { success: false, message: `Failed to inject menu item: ${error}` };
    }
}

/**
 * Convenience function to inject both route and menu item
 */
export async function injectReactV2Entity(config: InjectorConfig): Promise<{ routeResult: { success: boolean; message: string }; menuResult: { success: boolean; message: string } }> {
    const routeResult = await injectReactV2Route(config);
    const menuResult = await injectReactV2MenuItem(config);

    return { routeResult, menuResult };
}
