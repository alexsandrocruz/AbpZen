import { memo } from 'react';
import { Handle, Position, type NodeProps } from 'reactflow';
import type { EntityData } from '../types';
import { Box, Hash, Type, Calendar, CheckSquare, Fingerprint } from 'lucide-react';

const TypeIcon = ({ type }: { type: string }) => {
    switch (type) {
        case 'string': return <Type size={12} className="text-blue-400" />;
        case 'int': return <Hash size={12} className="text-green-400" />;
        case 'datetime': return <Calendar size={12} className="text-purple-400" />;
        case 'bool': return <CheckSquare size={12} className="text-yellow-400" />;
        case 'guid': return <Fingerprint size={12} className="text-red-400" />;
        default: return null;
    }
};

const EntityNode = ({ data, selected }: NodeProps<EntityData>) => {
    return (
        <div className={`entity-node ${selected ? 'selected' : ''}`}>
            <Handle type="target" position={Position.Top} className="handle" />

            <div className="entity-header">
                <Box size={16} />
                <span>{data.name}</span>
            </div>

            <div className="entity-fields">
                {data.fields.length === 0 && (
                    <div className="no-fields">No fields</div>
                )}
                {data.fields.map((field) => (
                    <div key={field.id} className="field-item">
                        <TypeIcon type={field.type} />
                        <span className="field-name">{field.name}</span>
                        <span className="field-type-label">{field.type}</span>
                    </div>
                ))}
            </div>

            <Handle type="source" position={Position.Bottom} className="handle" />
        </div>
    );
};

export default memo(EntityNode);
