import JSZip from 'jszip';
import type { GeneratedFile } from './index';

/**
 * Create a ZIP file from generated files and trigger download
 */
export async function downloadAsZip(files: any[], projectName: string): Promise<void> {
    const zip = new JSZip();

    // Group files by layer for organization
    for (const file of files) {
        if (file.encoding === 'base64') {
            zip.file(file.path, file.content, { base64: true });
        } else {
            zip.file(file.path, file.content);
        }
    }

    // Generate the ZIP
    const content = await zip.generateAsync({
        type: 'blob',
        compression: 'DEFLATE',
        compressionOptions: { level: 6 }
    });

    // Trigger download manually as saveAs sometimes fails with filenames in some environments
    const filename = `${projectName.toLowerCase()}-generated.zip`;
    const url = window.URL.createObjectURL(content);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', filename);
    document.body.appendChild(link);
    link.click();

    // Cleanup
    setTimeout(() => {
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
    }, 100);
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
        'React': '#06b6d4',
        'Angular': '#dc2626',
    };
    return colors[layer] || '#64748b';
}
