/**
 * Project Generator
 * Handles project creation via Bridge API or ZIP download
 */

import type { ProjectConfig, ProjectCreationMode, ProjectManifest } from './types';

const BRIDGE_URL = 'http://localhost:3001';

/**
 * Check if Bridge API is available
 */
export async function isBridgeAvailable(): Promise<boolean> {
    try {
        const response = await fetch(`${BRIDGE_URL}/api/list-dirs`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({}),
        });
        return response.ok;
    } catch {
        return false;
    }
}

/**
 * Pick a directory using system dialog (via Bridge)
 */
export async function pickDirectory(): Promise<string | null> {
    try {
        const response = await fetch(`${BRIDGE_URL}/api/pick-directory`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
        });
        const data = await response.json();
        if (data.canceled) return null;
        return data.path || null;
    } catch (error) {
        console.error('Failed to pick directory:', error);
        return null;
    }
}

/**
 * Create project locally via Bridge API
 */
export async function createProjectLocal(
    config: ProjectConfig,
    destinationPath: string
): Promise<{ success: boolean; projectPath?: string; error?: string }> {
    try {
        const response = await fetch(`${BRIDGE_URL}/api/create-project`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                projectName: config.name,
                destinationPath,
                frontends: config.frontends,
            }),
        });

        const data = await response.json();

        if (!response.ok) {
            return { success: false, error: data.error || 'Unknown error' };
        }

        return {
            success: true,
            projectPath: data.projectPath
        };
    } catch (error) {
        return {
            success: false,
            error: error instanceof Error ? error.message : 'Bridge connection failed'
        };
    }
}

/**
 * Create project as ZIP download
 * (Uses JSZip - will be implemented in Phase 4)
 */
export async function createProjectZip(
    config: ProjectConfig
): Promise<{ success: boolean; error?: string }> {
    // TODO: Implement ZIP generation with JSZip
    console.log('ZIP download not yet implemented:', config);
    return { success: false, error: 'ZIP download coming soon' };
}

/**
 * Main project creation function
 */
export async function createProject(
    config: ProjectConfig,
    mode: ProjectCreationMode,
    destinationPath?: string
): Promise<{ success: boolean; projectPath?: string; error?: string }> {
    if (mode === 'local') {
        if (!destinationPath) {
            return { success: false, error: 'Destination path required for local mode' };
        }
        return createProjectLocal(config, destinationPath);
    } else {
        return createProjectZip(config);
    }
}
