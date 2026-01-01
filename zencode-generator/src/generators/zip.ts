import JSZip from 'jszip';
import { saveAs } from 'file-saver';
import type { GeneratedFile } from './index';

/**
 * Create a ZIP file from generated files and trigger download
 */
export async function downloadAsZip(files: GeneratedFile[], projectName: string): Promise<void> {
    const zip = new JSZip();

    // Group files by layer for organization
    for (const file of files) {
        zip.file(file.path, file.content);
    }

    // Generate the ZIP
    const content = await zip.generateAsync({ type: 'blob' });

    // Trigger download
    saveAs(content, `${projectName}-generated.zip`);
}

/**
 * Get file extension icon for display
 */
export function getFileIcon(path: string): string {
    if (path.endsWith('.cs')) return '🟢';
    if (path.endsWith('.ts') || path.endsWith('.tsx')) return '🔵';
    if (path.endsWith('.cshtml')) return '🟣';
    if (path.endsWith('.json')) return '🟡';
    return '📄';
}

/**
 * Get layer color for display
 */
export function getLayerColor(layer: GeneratedFile['layer']): string {
    const colors: Record<string, string> = {
        'Domain': '#22c55e',
        'Application': '#3b82f6',
        'Application.Contracts': '#8b5cf6',
        'EntityFrameworkCore': '#f59e0b',
        'Web': '#ec4899',
    };
    return colors[layer] || '#64748b';
}
