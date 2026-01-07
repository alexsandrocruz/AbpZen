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
 * Fetches boilerplate files from Bridge API and packages them with JSZip
 */
export async function createProjectZip(
    config: ProjectConfig
): Promise<{ success: boolean; error?: string }> {
    try {
        // Dynamically import JSZip
        const JSZip = (await import('jszip')).default;
        const zip = new JSZip();

        // Fetch boilerplate content from Bridge
        const response = await fetch(`${BRIDGE_URL}/api/get-boilerplate`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                projectName: config.name,
                frontends: config.frontends,
            }),
        });

        if (!response.ok) {
            const error = await response.json();
            return { success: false, error: error.error || 'Failed to get boilerplate' };
        }

        const data = await response.json();

        // Add all files to ZIP
        for (const file of data.files) {
            zip.file(file.path, file.content);
        }

        // Add zencode.json manifest
        const manifest: ProjectManifest = {
            version: '1.0',
            name: config.name,
            namespace: config.namespace,
            createdAt: new Date().toISOString(),
            frontends: config.frontends,
            entities: [],
        };
        zip.file('zencode.json', JSON.stringify(manifest, null, 2));

        // Generate and download ZIP with correct filename
        await downloadZipBlob(zip, `${config.name}.zip`);

        return { success: true };
    } catch (error) {
        console.error('ZIP generation failed:', error);
        return {
            success: false,
            error: error instanceof Error ? error.message : 'ZIP generation failed'
        };
    }
}

/**
 * Helper: Download a JSZip instance as a file
 */
async function downloadZipBlob(zip: unknown, filename: string): Promise<void> {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const blob = await (zip as any).generateAsync({
        type: 'blob',
        mimeType: 'application/zip'
    });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    a.style.display = 'none';
    document.body.appendChild(a);

    // Force click with proper event
    a.click();

    // Cleanup after delay to ensure download starts
    setTimeout(() => {
        document.body.removeChild(a);
        URL.revokeObjectURL(url);
    }, 100);
}

/**
 * Run a command in a terminal on the bridge
 */
export async function runTerminalCommand(id: string, command: string, cwd: string): Promise<{ success: boolean; error?: string }> {
    try {
        const response = await fetch(`${BRIDGE_URL}/api/terminal/run`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ id, command, cwd })
        });
        if (!response.ok) {
            const data = await response.json();
            return { success: false, error: data.error || `HTTP ${response.status}` };
        }
        return { success: true };
    } catch (e) {
        console.error('Failed to run terminal command:', e);
        return { success: false, error: e instanceof Error ? e.message : 'Connection failed' };
    }
}

/**
 * Get logs from a terminal
 */
export async function getTerminalLogs(id: string, offset: number = 0): Promise<{
    status: 'running' | 'stopped' | 'error';
    exitCode: number | null;
    logs: Array<{ type: 'stdout' | 'stderr', content: string, timestamp: number }>;
    nextOffset: number;
} | null> {
    try {
        const response = await fetch(`${BRIDGE_URL}/api/terminal/logs/${id}?offset=${offset}`);
        if (!response.ok) return null;
        return await response.json();
    } catch (e) {
        console.error('Failed to get terminal logs:', e);
        return null;
    }
}

/**
 * Stop a terminal process
 */
export async function stopTerminalCommand(id: string): Promise<boolean> {
    try {
        const response = await fetch(`${BRIDGE_URL}/api/terminal/stop/${id}`, {
            method: 'POST'
        });
        return response.ok;
    } catch (e) {
        console.error('Failed to stop terminal command:', e);
        return false;
    }
}

/**
 * Get status of all terminals
 */
export async function getTerminalsStatus(): Promise<Record<string, { status: string, exitCode: number | null }> | null> {
    try {
        const response = await fetch(`${BRIDGE_URL}/api/terminal/status`);
        if (!response.ok) return null;
        return await response.json();
    } catch (e) {
        console.error('Failed to get terminal status:', e);
        return null;
    }
}

/**
 * Download generated code files as ZIP (for use after modeling entities)
 */
export async function downloadGeneratedCodeAsZip(
    files: { path: string; content: string }[],
    projectName: string
): Promise<{ success: boolean; error?: string }> {
    try {
        const JSZip = (await import('jszip')).default;
        const zip = new JSZip();

        // Add all generated files to ZIP
        for (const file of files) {
            zip.file(file.path, file.content);
        }

        // Download
        await downloadZipBlob(zip, `${projectName}-generated.zip`);

        return { success: true };
    } catch (error) {
        console.error('ZIP download failed:', error);
        return {
            success: false,
            error: error instanceof Error ? error.message : 'ZIP download failed'
        };
    }
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
