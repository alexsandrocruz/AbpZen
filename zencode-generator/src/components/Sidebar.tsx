import { Plus, Trash2, X, Copy, Eye } from 'lucide-react';
import type { EntityData, EntityField, FieldType, RelationshipData } from '../types';
import { pluralize } from '../utils/pluralize';

interface SidebarProps {
    selectedEntity: { id: string; data: EntityData } | null;
    selectedEdge: { id: string; data: RelationshipData; sourceName?: string; targetName?: string } | null;
    allEntities: EntityData[];
    onUpdateEntity: (id: string, data: EntityData) => void;
    onUpdateEdge: (id: string, data: RelationshipData) => void;
    onChangeRelationType: (edgeId: string, newType: RelationshipData['type']) => void;
    onDeleteEntity: (id: string) => void;
    onDeleteEdge: (id: string) => void;
    onDuplicateEntity: (id: string) => void;
    onPreviewEntity: (id: string) => void;
    onClose: () => void;
}

const Sidebar = ({ selectedEntity, selectedEdge, allEntities, onUpdateEntity, onUpdateEdge, onChangeRelationType, onDeleteEntity, onDeleteEdge, onDuplicateEntity, onPreviewEntity, onClose }: SidebarProps) => {
    if (!selectedEntity && !selectedEdge) return null;

    if (selectedEdge && !selectedEntity) {
        const { id, data, sourceName, targetName } = selectedEdge;
        return (
            <div className="sidebar">
                <div className="sidebar-header">
                    <h3>Edit Relation</h3>
                    <div className="header-actions">
                        <button
                            className="btn-icon text-red-400"
                            onClick={() => { onDeleteEdge(id); onClose(); }}
                            title="Delete Relationship"
                        >
                            <Trash2 size={18} />
                        </button>
                        <button className="btn-icon" onClick={onClose}><X size={18} /></button>
                    </div>
                </div>
                <div className="sidebar-content">
                    <div className="relation-display">
                        <span className="node-badge">{sourceName || 'Source'}</span>
                        <span className="arrow">→</span>
                        <span className="node-badge">{targetName || 'Target'}</span>
                    </div>

                    <div className="form-section">
                        <h4>Relation Details</h4>
                        <div className="form-group">
                            <label>Relation Type</label>
                            <select
                                value={data.type}
                                onChange={(e) => onChangeRelationType(id, e.target.value as RelationshipData['type'])}
                            >
                                <option value="one-to-many">One to Many (1..*)</option>
                                <option value="many-to-many">Many to Many (*..*)</option>
                                <option value="one-to-one">One to One (1..1)</option>
                            </select>
                            {data.type === 'many-to-many' && data.junctionConfig && (
                                <p style={{ fontSize: '0.75rem', color: '#22c55e', marginTop: '4px' }}>
                                    ✓ Junction table "{data.junctionConfig.tableName}" created
                                </p>
                            )}
                        </div>
                        <div className="form-group">
                            <label>Source Navigation Name</label>
                            <input
                                type="text"
                                value={data.sourceNavigationName}
                                onChange={(e) => onUpdateEdge(id, { ...data, sourceNavigationName: e.target.value })}
                                placeholder={`${targetName}s`}
                            />
                        </div>
                        <div className="form-group">
                            <label>Target Navigation Name</label>
                            <input
                                type="text"
                                value={data.targetNavigationName}
                                onChange={(e) => onUpdateEdge(id, { ...data, targetNavigationName: e.target.value })}
                                placeholder={sourceName}
                            />
                        </div>
                        <div className="form-group flex-row">
                            <label>Is Required?</label>
                            <input
                                type="checkbox"
                                checked={data.isRequired}
                                onChange={(e) => onUpdateEdge(id, { ...data, isRequired: e.target.checked })}
                            />
                        </div>
                    </div>

                    {/* Child Grid (Master-Detail) Configuration */}
                    {data.type === 'one-to-many' && (
                        <div className="form-section" style={{ marginTop: '16px', paddingTop: '16px', borderTop: '1px solid #334155' }}>
                            <div className="form-group flex-row">
                                <label style={{ color: '#f59e0b', fontWeight: 500 }}>
                                    <input
                                        type="checkbox"
                                        checked={data.isChildGrid || false}
                                        onChange={(e) => onUpdateEdge(id, {
                                            ...data,
                                            isChildGrid: e.target.checked,
                                            childGridConfig: e.target.checked ? {
                                                title: `${sourceName} Items`,
                                                allowAdd: true,
                                                allowRemove: true,
                                                allowEdit: true,
                                                defaultExpanded: true,
                                            } : undefined
                                        })}
                                    /> Master-Detail (Child Grid)
                                </label>
                            </div>

                            {data.isChildGrid && (
                                <>
                                    <div className="form-group" style={{ marginTop: '12px' }}>
                                        <label>Section Title</label>
                                        <input
                                            type="text"
                                            value={data.childGridConfig?.title || ''}
                                            onChange={(e) => onUpdateEdge(id, {
                                                ...data,
                                                childGridConfig: { ...data.childGridConfig!, title: e.target.value }
                                            })}
                                            placeholder="e.g. Order Items"
                                        />
                                    </div>
                                    <div className="field-options" style={{ marginTop: '8px' }}>
                                        <label>
                                            <input
                                                type="checkbox"
                                                checked={data.childGridConfig?.allowAdd !== false}
                                                onChange={(e) => onUpdateEdge(id, {
                                                    ...data,
                                                    childGridConfig: { ...data.childGridConfig!, allowAdd: e.target.checked }
                                                })}
                                            /> Allow Add
                                        </label>
                                        <label>
                                            <input
                                                type="checkbox"
                                                checked={data.childGridConfig?.allowRemove !== false}
                                                onChange={(e) => onUpdateEdge(id, {
                                                    ...data,
                                                    childGridConfig: { ...data.childGridConfig!, allowRemove: e.target.checked }
                                                })}
                                            /> Allow Remove
                                        </label>
                                        <label>
                                            <input
                                                type="checkbox"
                                                checked={data.childGridConfig?.allowEdit !== false}
                                                onChange={(e) => onUpdateEdge(id, {
                                                    ...data,
                                                    childGridConfig: { ...data.childGridConfig!, allowEdit: e.target.checked }
                                                })}
                                            /> Allow Edit
                                        </label>
                                    </div>
                                </>
                            )}
                        </div>
                    )}
                </div>
            </div>
        );
    }

    // Since we returned above if selectedEdge was prioritized, we safe access selectedEntity here
    const { id, data } = selectedEntity!;

    const addField = () => {
        const newField: EntityField = {
            id: `field_${Date.now()}`,
            name: `NewField${data.fields.length + 1}`,
            type: 'string',
            isRequired: false,
            isNullable: true,
            isFilterable: true,
            isTextArea: false,
        };
        onUpdateEntity(id, { ...data, fields: [...data.fields, newField] });
    };

    const updateField = (fieldId: string, updates: Partial<EntityField>) => {
        const newFields = data.fields.map((f: EntityField) =>
            f.id === fieldId ? { ...f, ...updates } : f
        );
        onUpdateEntity(id, { ...data, fields: newFields });
    };

    const removeField = (fieldId: string) => {
        const newFields = data.fields.filter((f: EntityField) => f.id !== fieldId);
        onUpdateEntity(id, { ...data, fields: newFields });
    };

    return (
        <div className="sidebar">
            <div className="sidebar-header">
                <h3>Edit Entity</h3>
                <div className="header-actions">
                    <button
                        className="btn-icon text-green-400"
                        onClick={() => { onPreviewEntity(id); }}
                        title="Preview CRUD"
                    >
                        <Eye size={18} />
                    </button>
                    <button
                        className="btn-icon text-blue-400"
                        onClick={() => { onDuplicateEntity(id); }}
                        title="Duplicate Entity"
                    >
                        <Copy size={18} />
                    </button>
                    <button
                        className="btn-icon text-red-400"
                        onClick={() => { onDeleteEntity(id); onClose(); }}
                        title="Delete Entity"
                    >
                        <Trash2 size={18} />
                    </button>
                    <button className="btn-icon" onClick={onClose}><X size={18} /></button>
                </div>
            </div>

            <div className="sidebar-content">
                <div className="form-section">
                    <h4>Entity Info</h4>
                    <div className="form-group">
                        <label>Entity Name</label>
                        <input
                            type="text"
                            value={data.name}
                            onChange={(e) => {
                                const newName = e.target.value;
                                onUpdateEntity(id, {
                                    ...data,
                                    name: newName,
                                    pluralName: pluralize(newName),
                                    tableName: pluralize(newName)
                                });
                            }}
                        />
                    </div>
                    <div className="form-group">
                        <label>Plural Name</label>
                        <input type="text" value={data.pluralName} onChange={(e) => onUpdateEntity(id, { ...data, pluralName: e.target.value })} />
                    </div>
                    <div className="form-group">
                        <label>Table Name</label>
                        <input type="text" value={data.tableName} onChange={(e) => onUpdateEntity(id, { ...data, tableName: e.target.value })} />
                    </div>
                    <div className="form-group">
                        <label>Base Class</label>
                        <select value={data.baseClass} onChange={(e) => onUpdateEntity(id, { ...data, baseClass: e.target.value as any })}>
                            <option value="Entity">Entity</option>
                            <option value="AuditedEntity">AuditedEntity</option>
                            <option value="FullAuditedEntity">FullAuditedEntity</option>
                            <option value="AggregateRoot">AggregateRoot</option>
                            <option value="AuditedAggregateRoot">AuditedAggregateRoot</option>
                            <option value="FullAuditedAggregateRoot">FullAuditedAggregateRoot</option>
                        </select>
                    </div>
                    <div className="form-group flex-row">
                        <label>Is Master?</label>
                        <input type="checkbox" checked={data.isMaster} onChange={(e) => onUpdateEntity(id, { ...data, isMaster: e.target.checked })} />
                    </div>
                </div>

                <div className="fields-section">
                    <div className="section-header">
                        <h4>Fields</h4>
                        <button className="btn-secondary btn-sm" onClick={addField}>
                            <Plus size={14} /> Add
                        </button>
                    </div>

                    <div className="fields-list">
                        {data.fields.map((field: EntityField) => (
                            <div key={field.id} className="field-editor-box">
                                <div className="field-row">
                                    <input
                                        type="text"
                                        value={field.name}
                                        className="field-name-input"
                                        onChange={(e) => updateField(field.id, { name: e.target.value })}
                                        placeholder="Field name"
                                    />
                                    <select
                                        value={field.type}
                                        onChange={(e) => updateField(field.id, { type: e.target.value as FieldType })}
                                    >
                                        <option value="string">string</option>
                                        <option value="int">int</option>
                                        <option value="long">long</option>
                                        <option value="bool">bool</option>
                                        <option value="guid">guid</option>
                                        <option value="datetime">datetime</option>
                                        <option value="decimal">decimal</option>
                                    </select>
                                    <button className="btn-icon text-red-400" onClick={() => removeField(field.id)}>
                                        <Trash2 size={14} />
                                    </button>
                                </div>
                                <div className="field-options">
                                    <label><input type="checkbox" checked={field.isRequired} onChange={(e) => updateField(field.id, { isRequired: e.target.checked })} /> Required</label>
                                    <label><input type="checkbox" checked={field.isNullable} onChange={(e) => updateField(field.id, { isNullable: e.target.checked })} /> Nullable</label>
                                    <label><input type="checkbox" checked={field.isTextArea} onChange={(e) => updateField(field.id, { isTextArea: e.target.checked })} /> Text Area</label>
                                    <label><input type="checkbox" checked={field.isFilterable} onChange={(e) => updateField(field.id, { isFilterable: e.target.checked })} /> Filterable</label>
                                    <label><input type="checkbox" checked={field.emailValidation} onChange={(e) => updateField(field.id, { emailValidation: e.target.checked })} /> Email Validation</label>
                                </div>
                                <div className="field-advanced">
                                    <div className="inner-group">
                                        <label>Default Value</label>
                                        <input type="text" value={field.defaultValue || ''} onChange={(e) => updateField(field.id, { defaultValue: e.target.value })} placeholder="Default..." />
                                    </div>
                                    <div className="inner-group">
                                        <label>Regex</label>
                                        <input type="text" value={field.regex || ''} onChange={(e) => updateField(field.id, { regex: e.target.value })} placeholder="Regex pattern" />
                                    </div>
                                    <div className="inner-row">
                                        <div className="inner-group">
                                            <label>Min</label>
                                            <input type="number" value={field.minLength || ''} onChange={(e) => updateField(field.id, { minLength: parseInt(e.target.value) })} />
                                        </div>
                                        <div className="inner-group">
                                            <label>Max</label>
                                            <input type="number" value={field.maxLength || ''} onChange={(e) => updateField(field.id, { maxLength: parseInt(e.target.value) })} />
                                        </div>
                                    </div>
                                    {/* Display Settings */}
                                    <FieldDisplaySettings field={field} onUpdate={(updates: Partial<EntityField>) => updateField(field.id, updates)} />
                                    {/* Lookup Settings */}
                                    {field.type === 'guid' && (
                                        <FieldLookupSettings
                                            field={field}
                                            allEntities={allEntities}
                                            onUpdate={(updates: Partial<EntityField>) => updateField(field.id, updates)}
                                        />
                                    )}
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    );
};

// Sub-component for Display Settings
const FieldDisplaySettings = ({ field, onUpdate }: { field: EntityField; onUpdate: (updates: Partial<EntityField>) => void }) => {
    return (
        <div className="field-display-settings" style={{ marginTop: '12px', paddingTop: '12px', borderTop: '1px solid #334155' }}>
            <div style={{ fontSize: '0.75rem', color: '#6366f1', marginBottom: '8px', fontWeight: 500 }}>Display Settings</div>
            <div className="inner-row">
                <div className="inner-group">
                    <label>Label</label>
                    <input type="text" value={field.label || ''} onChange={(e) => onUpdate({ label: e.target.value })} placeholder="Field label..." />
                </div>
                <div className="inner-group">
                    <label>Order</label>
                    <input type="number" value={field.order ?? ''} onChange={(e) => onUpdate({ order: parseInt(e.target.value) || undefined })} placeholder="#" />
                </div>
            </div>
            <div className="inner-group" style={{ marginTop: '8px' }}>
                <label>Placeholder</label>
                <input type="text" value={field.placeholder || ''} onChange={(e) => onUpdate({ placeholder: e.target.value })} placeholder="Enter placeholder..." />
            </div>
            <div className="inner-row" style={{ marginTop: '8px' }}>
                <div className="inner-group">
                    <label>Form Width</label>
                    <select value={field.formWidth || 'full'} onChange={(e) => onUpdate({ formWidth: e.target.value as 'full' | 'half' | 'third' })}>
                        <option value="full">Full</option>
                        <option value="half">Half</option>
                        <option value="third">Third</option>
                    </select>
                </div>
                <div className="inner-group">
                    <label>Grid Width</label>
                    <input type="number" value={field.gridWidth || ''} onChange={(e) => onUpdate({ gridWidth: parseInt(e.target.value) || undefined })} placeholder="px" />
                </div>
            </div>
            <div className="field-options" style={{ marginTop: '8px' }}>
                <label><input type="checkbox" checked={field.showInGrid !== false} onChange={(e) => onUpdate({ showInGrid: e.target.checked })} /> Show in Grid</label>
                <label><input type="checkbox" checked={field.showInForm !== false} onChange={(e) => onUpdate({ showInForm: e.target.checked })} /> Show in Form</label>
                <label><input type="checkbox" checked={field.readOnly || false} onChange={(e) => onUpdate({ readOnly: e.target.checked })} /> Read Only</label>
            </div>
        </div>
    );
};

// Sub-component for Lookup Settings
const FieldLookupSettings = ({ field, allEntities, onUpdate }: { field: EntityField; allEntities: EntityData[]; onUpdate: (updates: Partial<EntityField>) => void }) => {
    return (
        <div className="field-lookup-settings" style={{ marginTop: '12px', paddingTop: '12px', borderTop: '1px solid #334155' }}>
            <div style={{ display: 'flex', alignItems: 'center', gap: '8px', marginBottom: '8px' }}>
                <label style={{ fontSize: '0.75rem', color: '#f59e0b', fontWeight: 500 }}>
                    <input
                        type="checkbox"
                        checked={field.isLookup || false}
                        onChange={(e) => onUpdate({
                            isLookup: e.target.checked,
                            lookupConfig: e.target.checked ? { mode: 'dropdown', displayField: 'name' } : undefined
                        })}
                    /> Is Lookup (FK)
                </label>
            </div>
            {field.isLookup && (
                <>
                    <div className="inner-row">
                        <div className="inner-group">
                            <label>Target Entity</label>
                            <select
                                value={field.lookupConfig?.targetEntity || ''}
                                onChange={(e) => onUpdate({
                                    lookupConfig: { ...field.lookupConfig!, targetEntity: e.target.value }
                                })}
                            >
                                <option value="">Select entity...</option>
                                {allEntities.map(entity => (
                                    <option key={entity.name} value={entity.name}>{entity.name}</option>
                                ))}
                            </select>
                        </div>
                        <div className="inner-group">
                            <label>Lookup Mode</label>
                            <select
                                value={field.lookupConfig?.mode || 'dropdown'}
                                onChange={(e) => onUpdate({
                                    lookupConfig: { ...field.lookupConfig!, mode: e.target.value as 'dropdown' | 'modal' }
                                })}
                            >
                                <option value="dropdown">Dropdown</option>
                                <option value="modal">Modal Picker</option>
                            </select>
                        </div>
                    </div>
                    <div className="inner-group" style={{ marginTop: '8px' }}>
                        <label>Display Field</label>
                        <input
                            type="text"
                            value={field.lookupConfig?.displayField || ''}
                            onChange={(e) => onUpdate({
                                lookupConfig: { ...field.lookupConfig!, displayField: e.target.value }
                            })}
                            placeholder="e.g. name"
                        />
                    </div>
                </>
            )}
        </div>
    );
};

export default Sidebar;
