import { useState } from 'react';
import { X, Search } from 'lucide-react';
import type { EntityData, EntityField } from '../types';
import './ui/styles.css';

interface CrudPreviewProps {
    entity: EntityData;
    allEntities: EntityData[];
    onClose: () => void;
}

type TabType = 'grid' | 'form';

// Sample data generator
const generateSampleData = (fields: EntityField[]): Record<string, string>[] => {
    const samples = [
        { string: 'Sample Text', int: '42', bool: 'true', guid: 'a1b2c3d4...', datetime: '2024-01-15', long: '1234567890', double: '99.99', decimal: '150.00', float: '3.14' },
        { string: 'Another Value', int: '123', bool: 'false', guid: 'e5f6g7h8...', datetime: '2024-02-20', long: '9876543210', double: '45.50', decimal: '299.99', float: '2.71' },
        { string: 'Third Item', int: '7', bool: 'true', guid: 'i9j0k1l2...', datetime: '2024-03-10', long: '5555555555', double: '10.00', decimal: '50.00', float: '1.41' },
    ];

    return samples.map((sample, idx) => {
        const row: Record<string, string> = { id: `${idx + 1}` };
        fields.forEach(field => {
            if (field.isLookup) {
                row[field.name] = `Related Item ${idx + 1}`;
            } else {
                row[field.name] = sample[field.type as keyof typeof sample] || 'N/A';
            }
        });
        return row;
    });
};

// Sort fields by order
const sortFieldsByOrder = (fields: EntityField[]): EntityField[] => {
    return [...fields].sort((a, b) => (a.order ?? 999) - (b.order ?? 999));
};

export default function CrudPreview({ entity, allEntities, onClose }: CrudPreviewProps) {
    const [activeTab, setActiveTab] = useState<TabType>('grid');

    const sortedFields = sortFieldsByOrder(entity.fields);
    const gridFields = sortedFields.filter(f => f.showInGrid !== false);
    const formFields = sortedFields.filter(f => f.showInForm !== false);
    const sampleData = generateSampleData(sortedFields);

    return (
        <div className="ui-dialog-overlay" onClick={onClose}>
            <div
                className="ui-dialog"
                style={{ width: '1000px' }}
                onClick={(e) => e.stopPropagation()}
            >
                <div className="ui-dialog-header">
                    <h2 className="ui-dialog-title">
                        Preview: {entity.name}
                    </h2>
                    <button className="btn-icon" onClick={onClose}>
                        <X size={20} />
                    </button>
                </div>

                <div className="ui-tabs">
                    <button
                        className={`ui-tab ${activeTab === 'grid' ? 'active' : ''}`}
                        onClick={() => setActiveTab('grid')}
                    >
                        Grid View
                    </button>
                    <button
                        className={`ui-tab ${activeTab === 'form' ? 'active' : ''}`}
                        onClick={() => setActiveTab('form')}
                    >
                        Edit Form
                    </button>
                </div>

                <div className="ui-dialog-content">
                    {activeTab === 'grid' && (
                        <GridPreview
                            fields={gridFields}
                            data={sampleData}
                            entityName={entity.pluralName}
                        />
                    )}
                    {activeTab === 'form' && (
                        <FormPreview
                            fields={formFields}
                            entityName={entity.name}
                            allEntities={allEntities}
                        />
                    )}
                </div>
            </div>
        </div>
    );
}

interface GridPreviewProps {
    fields: EntityField[];
    data: Record<string, string>[];
    entityName: string;
}

function GridPreview({ fields, data, entityName }: GridPreviewProps) {
    return (
        <div className="preview-grid-container">
            <div className="preview-actions-bar">
                <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
                    <h3 style={{ margin: 0, fontSize: '1rem', color: '#f8fafc' }}>
                        {entityName}
                    </h3>
                    <span className="preview-sample-tag">Sample Data</span>
                </div>
                <div style={{ display: 'flex', gap: '8px' }}>
                    <button className="ui-button ui-button-secondary">
                        <Search size={16} />
                        Search
                    </button>
                    <button className="ui-button ui-button-primary">
                        + New
                    </button>
                </div>
            </div>

            <table className="ui-table">
                <thead>
                    <tr>
                        {fields.map(field => (
                            <th
                                key={field.id}
                                style={{ width: field.gridWidth ? `${field.gridWidth}px` : 'auto' }}
                            >
                                {field.label || field.name}
                            </th>
                        ))}
                        <th style={{ width: '100px' }}>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {data.map((row, idx) => (
                        <tr key={idx}>
                            {fields.map(field => (
                                <td key={field.id}>
                                    {field.isLookup ? (
                                        <span style={{ color: '#6366f1' }}>{row[field.name]}</span>
                                    ) : (
                                        row[field.name]
                                    )}
                                </td>
                            ))}
                            <td>
                                <div style={{ display: 'flex', gap: '4px' }}>
                                    <button className="ui-button ui-button-ghost" style={{ padding: '4px 8px' }}>
                                        Edit
                                    </button>
                                    <button className="ui-button ui-button-ghost" style={{ padding: '4px 8px', color: '#ef4444' }}>
                                        Delete
                                    </button>
                                </div>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

interface FormPreviewProps {
    fields: EntityField[];
    entityName: string;
    allEntities: EntityData[];
}

function FormPreview({ fields, entityName, allEntities }: FormPreviewProps) {
    return (
        <div className="preview-form-container">
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '24px' }}>
                <div>
                    <h3 style={{ margin: 0, fontSize: '1.25rem', color: '#f8fafc' }}>
                        Edit {entityName}
                    </h3>
                    <p style={{ margin: '4px 0 0', fontSize: '0.875rem', color: '#64748b' }}>
                        Update the information below
                    </p>
                </div>
                <span className="preview-sample-tag">Preview Only</span>
            </div>

            <div className="ui-form">
                {fields.map(field => (
                    <FormField
                        key={field.id}
                        field={field}
                        allEntities={allEntities}
                    />
                ))}
            </div>

            <div style={{ marginTop: '32px', display: 'flex', justifyContent: 'flex-end', gap: '12px' }}>
                <button className="ui-button ui-button-secondary">
                    Cancel
                </button>
                <button className="ui-button ui-button-primary">
                    Save Changes
                </button>
            </div>
        </div>
    );
}

interface FormFieldProps {
    field: EntityField;
    allEntities: EntityData[];
}

function FormField({ field, allEntities }: FormFieldProps) {
    const widthClass = field.formWidth || 'full';

    // Get placeholder text
    const getPlaceholder = () => {
        if (field.placeholder) return field.placeholder;
        if (field.isLookup) return `Select ${field.lookupConfig?.targetEntity || 'item'}...`;
        return `Enter ${field.label || field.name}...`;
    };

    // Render lookup field
    if (field.isLookup && field.lookupConfig) {
        const { mode } = field.lookupConfig;

        return (
            <div className={`ui-form-group ${widthClass}`}>
                <label className="ui-label">
                    {field.label || field.name}
                    {field.isRequired && <span style={{ color: '#ef4444' }}> *</span>}
                </label>
                {mode === 'dropdown' ? (
                    <select className="ui-select" disabled={field.readOnly}>
                        <option value="">{getPlaceholder()}</option>
                        <option value="1">Sample Option 1</option>
                        <option value="2">Sample Option 2</option>
                        <option value="3">Sample Option 3</option>
                    </select>
                ) : (
                    <div className="ui-lookup-field">
                        <input
                            type="text"
                            className={`ui-input ${field.readOnly ? 'readonly' : ''}`}
                            placeholder={getPlaceholder()}
                            readOnly
                        />
                        <button className="ui-lookup-button" title="Search">
                            <Search size={16} />
                        </button>
                    </div>
                )}
            </div>
        );
    }

    // Render regular field
    return (
        <div className={`ui-form-group ${widthClass}`}>
            <label className="ui-label">
                {field.label || field.name}
                {field.isRequired && <span style={{ color: '#ef4444' }}> *</span>}
            </label>
            {field.isTextArea ? (
                <textarea
                    className={`ui-input ${field.readOnly ? 'readonly' : ''}`}
                    placeholder={getPlaceholder()}
                    readOnly={field.readOnly}
                    rows={3}
                    style={{ resize: 'vertical' }}
                />
            ) : field.type === 'bool' ? (
                <select className="ui-select" disabled={field.readOnly}>
                    <option value="">Select...</option>
                    <option value="true">Yes</option>
                    <option value="false">No</option>
                </select>
            ) : (
                <input
                    type={field.type === 'datetime' ? 'date' : field.type === 'int' || field.type === 'decimal' || field.type === 'double' ? 'number' : 'text'}
                    className={`ui-input ${field.readOnly ? 'readonly' : ''}`}
                    placeholder={getPlaceholder()}
                    readOnly={field.readOnly}
                />
            )}
        </div>
    );
}
