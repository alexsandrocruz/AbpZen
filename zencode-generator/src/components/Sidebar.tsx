import { Plus, Trash2, X } from 'lucide-react';
import type { EntityData, EntityField, FieldType } from '../types';

interface SidebarProps {
    selectedEntity: { id: string; data: EntityData } | null;
    onUpdateEntity: (id: string, data: EntityData) => void;
    onClose: () => void;
}

const Sidebar = ({ selectedEntity, onUpdateEntity, onClose }: SidebarProps) => {
    if (!selectedEntity) return null;

    const { id, data } = selectedEntity;

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
        const newFields = data.fields.map((f) =>
            f.id === fieldId ? { ...f, ...updates } : f
        );
        onUpdateEntity(id, { ...data, fields: newFields });
    };

    const removeField = (fieldId: string) => {
        const newFields = data.fields.filter((f) => f.id !== fieldId);
        onUpdateEntity(id, { ...data, fields: newFields });
    };

    return (
        <div className="sidebar">
            <div className="sidebar-header">
                <h3>Edit Entity</h3>
                <button className="btn-icon" onClick={onClose}><X size={18} /></button>
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
                                    pluralName: `${newName}s`,
                                    tableName: `${newName}s`
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
                        {data.fields.map((field) => (
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
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Sidebar;
