import { memo, useState, useEffect } from 'react';
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

const EntityNode = ({ id, data, selected }: NodeProps<EntityData>) => {
    const [isEditing, setIsEditing] = useState(false);
    const [localName, setLocalName] = useState(data.name);

    // Sync local name if it changes from sidebar
    useEffect(() => {
        setLocalName(data.name);
    }, [data.name]);

    const handleBlur = () => {
        setIsEditing(false);
        // Logic to update entity will be handled via a callback if needed, 
        // but React Flow nodes usually rely on shared state.
        // We'll use a custom event or a shared update function if we can't easily reach setNodes.
        // For now, let's assume we can trigger an update event or use the data object if it has a setter.
        // Actually, in our App.tsx, we have updateEntity. We might need to pass it down or use a store.

        // Dispatch a custom event to notify App.tsx about the rename
        const event = new CustomEvent('entity-rename', {
            detail: { id, name: localName }
        });
        window.dispatchEvent(event);
    };

    const handleKeyDown = (e: React.KeyboardEvent) => {
        if (e.key === 'Enter') handleBlur();
    };

    return (
        <div className={`entity-node ${selected ? 'selected' : ''} ${!data.isMaster ? 'is-child' : ''}`}>
            <Handle type="target" position={Position.Top} className="handle" />

            <div className="entity-header">
                <Box size={16} />
                <div className="entity-name-text" onDoubleClick={() => setIsEditing(true)}>
                    {isEditing ? (
                        <input
                            autoFocus
                            className="inline-name-input"
                            value={localName}
                            onChange={(e) => setLocalName(e.target.value)}
                            onBlur={handleBlur}
                            onKeyDown={handleKeyDown}
                        />
                    ) : (
                        <span>{data.name}</span>
                    )}
                </div>
                {!data.isMaster && <span className="child-badge">CHILD</span>}
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
