import type { Node, Edge } from 'reactflow';
import type { EntityData, RelationshipData, ZenMetadata } from '../types';

export const transformToMetadata = (
    nodes: Node<EntityData>[],
    edges: Edge<RelationshipData>[],
    namespace: string = 'ZenDoctor'
): ZenMetadata => {
    return {
        projectName: 'ZenGenerated',
        namespace,
        entities: nodes.map((node) => ({
            id: node.id,
            data: node.data,
        })),
        relationships: edges.map((edge) => ({
            id: edge.id,
            source: edge.source,
            target: edge.target,
            data: edge.data as RelationshipData,
        })),
    };
};

export const downloadJson = (data: ZenMetadata, filename: string = 'zen-metadata.json') => {
    const jsonString = JSON.stringify(data, null, 2);
    const blob = new Blob([jsonString], { type: 'application/json' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url);
};
