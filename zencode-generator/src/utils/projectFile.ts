import type { Node, Edge } from 'reactflow';
import type { EntityData } from '../types';

export interface ProjectFile {
    version: string;
    name: string;
    createdAt: string;
    updatedAt: string;
    nodes: Node<EntityData>[];
    edges: Edge[];
}

export const createProjectFile = (
    name: string,
    nodes: Node<EntityData>[],
    edges: Edge[]
): ProjectFile => {
    const now = new Date().toISOString();
    return {
        version: '1.0',
        name,
        createdAt: now,
        updatedAt: now,
        nodes,
        edges,
    };
};

export const saveProjectToFile = (project: ProjectFile, fileName?: string) => {
    const json = JSON.stringify(project, null, 2);
    const blob = new Blob([json], { type: 'application/json' });
    const url = URL.createObjectURL(blob);

    const a = document.createElement('a');
    a.href = url;
    a.download = fileName || `${project.name}.zen`;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
};

export const loadProjectFromFile = (file: File): Promise<ProjectFile> => {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = (e) => {
            try {
                const content = e.target?.result as string;
                const project = JSON.parse(content) as ProjectFile;

                // Basic validation
                if (!project.version || !project.nodes || !project.edges) {
                    throw new Error('Invalid project file format');
                }

                resolve(project);
            } catch (error) {
                reject(new Error('Failed to parse project file'));
            }
        };
        reader.onerror = () => reject(new Error('Failed to read file'));
        reader.readAsText(file);
    });
};
