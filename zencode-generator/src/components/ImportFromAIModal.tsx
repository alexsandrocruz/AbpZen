import { useState } from 'react';
import { Sparkles, X, Loader2, AlertCircle, Check } from 'lucide-react';
import { extractEntitiesFromText, isGeminiConfigured } from '../lib/gemini/client';
import type { AIExtractionResult, AIExtractedEntity, AIExtractedRelationship } from '../lib/gemini/types';

interface ImportFromAIModalProps {
    onClose: () => void;
    onImport: (result: AIExtractionResult) => void;
    onOpenSettings: () => void;
}

export default function ImportFromAIModal({ onClose, onImport, onOpenSettings }: ImportFromAIModalProps) {
    const [input, setInput] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [result, setResult] = useState<AIExtractionResult | null>(null);
    const [step, setStep] = useState<'input' | 'preview'>('input');

    const isConfigured = isGeminiConfigured();

    const handleExtract = async () => {
        if (!input.trim()) return;

        setLoading(true);
        setError(null);

        try {
            const extracted = await extractEntitiesFromText(input);
            setResult(extracted);
            setStep('preview');
        } catch (e: any) {
            setError(e.message || 'Failed to extract entities');
        } finally {
            setLoading(false);
        }
    };

    const handleConfirmImport = () => {
        if (result) {
            onImport(result);
            onClose();
        }
    };

    const handleBack = () => {
        setStep('input');
        setResult(null);
    };

    return (
        <div className="ui-dialog-overlay" onClick={onClose}>
            <div
                className="ui-dialog"
                style={{ width: '800px', maxHeight: '85vh' }}
                onClick={(e) => e.stopPropagation()}
            >
                <div className="ui-dialog-header">
                    <h2 className="ui-dialog-title">
                        <Sparkles size={20} style={{ color: '#a855f7' }} />
                        Import from AI
                    </h2>
                    <button className="btn-icon" onClick={onClose}>
                        <X size={20} />
                    </button>
                </div>

                <div className="ui-dialog-content" style={{ padding: '24px' }}>
                    {!isConfigured ? (
                        /* Not Configured State */
                        <div style={{ textAlign: 'center', padding: '40px 20px' }}>
                            <AlertCircle size={48} style={{ color: '#f59e0b', marginBottom: '16px' }} />
                            <h3 style={{ color: '#f8fafc', marginBottom: '8px' }}>API Key Required</h3>
                            <p style={{ color: '#64748b', marginBottom: '24px' }}>
                                Configure your Google Gemini API key to use AI-powered entity import.
                            </p>
                            <button
                                className="ui-button ui-button-primary"
                                onClick={() => { onClose(); onOpenSettings(); }}
                            >
                                Open Settings
                            </button>
                        </div>
                    ) : step === 'input' ? (
                        /* Input Step */
                        <>
                            <div style={{ marginBottom: '16px' }}>
                                <label style={{ color: '#f8fafc', marginBottom: '8px', display: 'block' }}>
                                    Describe your entities and relationships
                                </label>
                                <textarea
                                    value={input}
                                    onChange={(e) => setInput(e.target.value)}
                                    placeholder={`Example:

I need a clinic management system with:
- Doctors (name, CRM, specialty, email, phone)
- Clinics (name, address, phone, CNPJ)
- Services (name, price, duration in minutes)
- Appointments (date, time, status)
- Patients (name, CPF, email, phone, birthDate)

Relationships:
- A Doctor works in multiple Clinics (N:N)
- A Doctor offers multiple Services (N:N)  
- An Appointment links Doctor + Patient + Service`}
                                    className="ui-input"
                                    style={{
                                        height: '300px',
                                        resize: 'vertical',
                                        fontFamily: 'inherit'
                                    }}
                                />
                            </div>

                            {error && (
                                <div style={{
                                    padding: '12px',
                                    background: 'rgba(239, 68, 68, 0.1)',
                                    borderRadius: '8px',
                                    color: '#f87171',
                                    marginBottom: '16px',
                                    display: 'flex',
                                    alignItems: 'center',
                                    gap: '8px'
                                }}>
                                    <AlertCircle size={16} />
                                    {error}
                                </div>
                            )}

                            <div style={{
                                padding: '12px',
                                background: 'rgba(99, 102, 241, 0.1)',
                                borderRadius: '8px',
                                border: '1px solid rgba(99, 102, 241, 0.3)'
                            }}>
                                <p style={{ color: '#94a3b8', fontSize: '0.8rem', margin: 0 }}>
                                    💡 <strong>Tip:</strong> Paste the output from your ChatGPT/Claude conversation directly.
                                    The AI will extract entities, fields, types, and relationships automatically.
                                </p>
                            </div>
                        </>
                    ) : (
                        /* Preview Step */
                        <div style={{ display: 'flex', flexDirection: 'column', gap: '20px' }}>
                            <div style={{
                                padding: '12px',
                                background: 'rgba(34, 197, 94, 0.1)',
                                borderRadius: '8px',
                                border: '1px solid rgba(34, 197, 94, 0.3)'
                            }}>
                                <p style={{ color: '#4ade80', fontSize: '0.875rem', margin: 0 }}>
                                    <Check size={16} style={{ display: 'inline', marginRight: '8px' }} />
                                    <strong>{result?.entities.length} entities</strong> and{' '}
                                    <strong>{result?.relationships.length} relationships</strong> detected
                                </p>
                            </div>

                            {/* Entities */}
                            <div>
                                <h4 style={{ color: '#f8fafc', marginBottom: '12px' }}>Entities</h4>
                                <div style={{ display: 'grid', gridTemplateColumns: 'repeat(2, 1fr)', gap: '12px' }}>
                                    {result?.entities.map((entity, idx) => (
                                        <EntityCard key={idx} entity={entity} />
                                    ))}
                                </div>
                            </div>

                            {/* Relationships */}
                            {result?.relationships && result.relationships.length > 0 && (
                                <div>
                                    <h4 style={{ color: '#f8fafc', marginBottom: '12px' }}>Relationships</h4>
                                    <div style={{ display: 'flex', flexDirection: 'column', gap: '8px' }}>
                                        {result.relationships.map((rel, idx) => (
                                            <RelationshipCard key={idx} relationship={rel} />
                                        ))}
                                    </div>
                                </div>
                            )}

                            {/* Enums */}
                            {result?.enums && result.enums.length > 0 && (
                                <div>
                                    <h4 style={{ color: '#f8fafc', marginBottom: '12px' }}>Enums</h4>
                                    <div style={{ display: 'flex', flexWrap: 'wrap', gap: '8px' }}>
                                        {result.enums.map((en, idx) => (
                                            <span
                                                key={idx}
                                                style={{
                                                    padding: '4px 12px',
                                                    background: 'rgba(168, 85, 247, 0.15)',
                                                    border: '1px solid rgba(168, 85, 247, 0.3)',
                                                    borderRadius: '16px',
                                                    color: '#c084fc',
                                                    fontSize: '0.8rem'
                                                }}
                                            >
                                                {en.name} ({en.values.length} values)
                                            </span>
                                        ))}
                                    </div>
                                </div>
                            )}
                        </div>
                    )}
                </div>

                <div className="ui-dialog-footer">
                    {step === 'input' ? (
                        <>
                            <button className="ui-button ui-button-secondary" onClick={onClose}>
                                Cancel
                            </button>
                            <button
                                className="ui-button ui-button-primary"
                                onClick={handleExtract}
                                disabled={!input.trim() || loading || !isConfigured}
                                style={{
                                    background: 'linear-gradient(135deg, #8b5cf6 0%, #a855f7 100%)',
                                    minWidth: '140px'
                                }}
                            >
                                {loading ? (
                                    <>
                                        <Loader2 size={16} className="animate-spin" />
                                        Extracting...
                                    </>
                                ) : (
                                    <>
                                        <Sparkles size={16} />
                                        Extract Entities
                                    </>
                                )}
                            </button>
                        </>
                    ) : (
                        <>
                            <button className="ui-button ui-button-secondary" onClick={handleBack}>
                                ← Back to Edit
                            </button>
                            <button
                                className="ui-button ui-button-primary"
                                onClick={handleConfirmImport}
                                style={{
                                    background: 'linear-gradient(135deg, #22c55e 0%, #16a34a 100%)'
                                }}
                            >
                                <Check size={16} />
                                Import to Canvas
                            </button>
                        </>
                    )}
                </div>
            </div>
        </div>
    );
}

/* Entity Preview Card */
function EntityCard({ entity }: { entity: AIExtractedEntity }) {
    return (
        <div style={{
            padding: '12px',
            background: '#0f172a',
            border: '1px solid #334155',
            borderRadius: '8px'
        }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '8px' }}>
                <strong style={{ color: '#f8fafc' }}>{entity.name}</strong>
                <span style={{ color: '#64748b', fontSize: '0.75rem' }}>
                    {entity.fields.length} fields
                </span>
            </div>
            <div style={{ display: 'flex', flexWrap: 'wrap', gap: '4px' }}>
                {entity.fields.slice(0, 5).map((field, idx) => (
                    <span
                        key={idx}
                        style={{
                            padding: '2px 8px',
                            background: '#1e293b',
                            borderRadius: '4px',
                            color: '#94a3b8',
                            fontSize: '0.7rem'
                        }}
                    >
                        {field.name}
                        <span style={{ color: '#64748b' }}> : {field.type}</span>
                    </span>
                ))}
                {entity.fields.length > 5 && (
                    <span style={{ color: '#64748b', fontSize: '0.7rem', padding: '2px' }}>
                        +{entity.fields.length - 5} more
                    </span>
                )}
            </div>
        </div>
    );
}

/* Relationship Preview Card */
function RelationshipCard({ relationship }: { relationship: AIExtractedRelationship }) {
    const typeColor = relationship.type === 'many-to-many' ? '#f59e0b' : '#3b82f6';
    const typeLabel = relationship.type === 'many-to-many' ? 'N:N' : '1:N';

    return (
        <div style={{
            padding: '8px 12px',
            background: '#0f172a',
            border: '1px solid #334155',
            borderRadius: '6px',
            display: 'flex',
            alignItems: 'center',
            gap: '12px'
        }}>
            <span style={{
                padding: '2px 8px',
                background: `${typeColor}20`,
                border: `1px solid ${typeColor}40`,
                borderRadius: '4px',
                color: typeColor,
                fontSize: '0.75rem',
                fontWeight: 600
            }}>
                {typeLabel}
            </span>
            <span style={{ color: '#f8fafc', fontSize: '0.875rem' }}>
                {relationship.sourceEntity}
            </span>
            <span style={{ color: '#64748b' }}>→</span>
            <span style={{ color: '#f8fafc', fontSize: '0.875rem' }}>
                {relationship.targetEntity}
            </span>
            {relationship.description && (
                <span style={{ color: '#64748b', fontSize: '0.75rem', marginLeft: 'auto' }}>
                    {relationship.description}
                </span>
            )}
        </div>
    );
}
