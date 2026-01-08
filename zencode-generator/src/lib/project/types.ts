/**
 * Project Management Types
 * Types for ZenCode project creation and management
 */

import type { FrontendTarget } from '../../types';

/**
 * Project configuration
 */
export interface ProjectConfig {
    name: string;               // Project name (e.g., "MeuProjeto")
    namespace: string;          // .NET namespace (e.g., "Sapienza.MeuProjeto")
    frontends: FrontendTarget[]; // Selected frontends
    includeBackend: boolean;    // Always true for now
}

/**
 * Project manifest stored in zencode.json
 */
export interface ProjectManifest {
    version: string;
    name: string;
    namespace: string;
    createdAt: string;
    frontends: FrontendTarget[];
    entities: string[]; // Entity names in this project
}

/**
 * Recent project entry
 */
export interface RecentProject {
    name: string;
    path: string;
    lastOpened: string;
    frontends: FrontendTarget[];
}

/**
 * Project creation mode
 */
export type ProjectCreationMode = 'local' | 'download';

/**
 * Project creation options
 */
export interface ProjectCreationOptions {
    config: ProjectConfig;
    mode: ProjectCreationMode;
    destinationPath?: string; // For local mode
}

/**
 * Boilerplate info
 */
export interface BoilerplateInfo {
    id: FrontendTarget | 'backend';
    name: string;
    description: string;
    sourcePath: string;
    required: boolean;
}

/**
 * Available boilerplates
 */
export const AVAILABLE_BOILERPLATES: BoilerplateInfo[] = [
    {
        id: 'backend',
        name: 'Backend (.NET 8)',
        description: 'ABP Framework backend with all modules',
        sourcePath: 'zencode-template/Sapienza.Zen.*',
        required: true,
    },
    {
        id: 'razor',
        name: 'Razor Pages',
        description: 'ABP Razor Pages UI (LeptonX Theme)',
        sourcePath: 'zencode-template/Sapienza.Zen.Web',
        required: false,
    },
    {
        id: 'react-v2',
        name: 'React (Vite)',
        description: 'Modern React with Vite + Tailwind 4',
        sourcePath: 'zencode-template/abp-react-v2',
        required: false,
    },
    {
        id: 'angular',
        name: 'Angular 17+',
        description: 'ABP Angular UI',
        sourcePath: 'zencode-template/angular',
        required: false,
    },
];
