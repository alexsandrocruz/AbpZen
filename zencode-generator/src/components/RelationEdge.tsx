import { BaseEdge, EdgeLabelRenderer, getBezierPath, type EdgeProps } from 'reactflow';
import type { RelationshipData } from '../types';

export default function RelationEdge({
    sourceX,
    sourceY,
    targetX,
    targetY,
    sourcePosition,
    targetPosition,
    style = {},
    markerEnd,
    data,
}: EdgeProps<RelationshipData>) {
    const [edgePath, labelX, labelY] = getBezierPath({
        sourceX,
        sourceY,
        sourcePosition,
        targetX,
        targetY,
        targetPosition,
    });

    const getLabel = (type: string | undefined) => {
        switch (type) {
            case 'one-to-many': return '1..*';
            case 'many-to-many': return '*..*';
            case 'one-to-one': return '1..1';
            default: return '';
        }
    };

    return (
        <>
            <BaseEdge path={edgePath} markerEnd={markerEnd} style={style} />
            <EdgeLabelRenderer>
                <div
                    style={{
                        position: 'absolute',
                        transform: `translate(-50%, -50%) translate(${labelX}px,${labelY}px)`,
                        background: '#1e293b',
                        padding: '2px 6px',
                        borderRadius: '4px',
                        fontSize: '10px',
                        fontWeight: 700,
                        color: '#6366f1',
                        border: '1px solid #334155',
                        pointerEvents: 'all',
                    }}
                    className="nodrag nopan"
                >
                    {getLabel(data?.type)}
                </div>
            </EdgeLabelRenderer>
        </>
    );
}
