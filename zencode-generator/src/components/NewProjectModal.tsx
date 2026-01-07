import { useState, useEffect } from 'react';
import {
    FolderPlus, X, ChevronRight, ChevronLeft,
    Download, Folder, Check, Server, Layout, Layers, Loader2, FolderOpen
} from 'lucide-react';
import type { ProjectConfig, ProjectCreationMode } from '../lib/project/types';
import type { FrontendTarget } from '../types';
import { pickDirectory, createProjectLocal, isBridgeAvailable } from '../lib/project/generator';

interface NewProjectModalProps {
    onClose: () => void;
    onCreate: (config: ProjectConfig, mode: ProjectCreationMode) => void;
}

type WizardStep = 'name' | 'frontends' | 'destination';

const FRONTEND_OPTIONS: { id: FrontendTarget; name: string; icon: React.ReactNode; description: string }[] = [
    { id: 'razor', name: 'Razor Pages', icon: <Layout size={24} />, description: 'ASP.NET Core Razor Pages UI' },
    { id: 'react', name: 'React (Next.js)', icon: <Layers size={24} />, description: 'Modern React with Next.js 15' },
    { id: 'angular', name: 'Angular', icon: <Layers size={24} />, description: 'Angular 17+ with ABP UI' },
];

export default function NewProjectModal({ onClose, onCreate }: NewProjectModalProps) {
    const [step, setStep] = useState<WizardStep>('name');
    const [name, setName] = useState('');
    const [namespace, setNamespace] = useState('');
    const [selectedFrontends, setSelectedFrontends] = useState<Set<FrontendTarget>>(new Set(['razor']));
    const [creationMode, setCreationMode] = useState<ProjectCreationMode>('local');
    const [destinationPath, setDestinationPath] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [bridgeAvailable, setBridgeAvailable] = useState(true);

    // Check if Bridge is available on mount
    useEffect(() => {
        isBridgeAvailable().then(available => {
            setBridgeAvailable(available);
            if (!available) {
                setCreationMode('download');
            }
        });
    }, []);

    const handleNameChange = (value: string) => {
        // Auto-format: capitalize first letter, remove spaces
        const formatted = value.replace(/\s+/g, '');
        setName(formatted);
        // Auto-generate namespace
        if (!namespace || namespace === `Sapienza.${name}`) {
            setNamespace(`Sapienza.${formatted}`);
        }
    };

    const toggleFrontend = (frontend: FrontendTarget) => {
        const newSet = new Set(selectedFrontends);
        if (newSet.has(frontend)) {
            newSet.delete(frontend);
        } else {
            newSet.add(frontend);
        }
        setSelectedFrontends(newSet);
    };

    const handleNext = () => {
        if (step === 'name') setStep('frontends');
        else if (step === 'frontends') setStep('destination');
    };

    const handleBack = () => {
        if (step === 'frontends') setStep('name');
        else if (step === 'destination') setStep('frontends');
    };

    const handleCreate = async () => {
        setError(null);
        setIsLoading(true);

        const config: ProjectConfig = {
            name,
            namespace,
            frontends: Array.from(selectedFrontends),
            includeBackend: true,
        };

        if (creationMode === 'local') {
            if (!destinationPath) {
                setError('Please select a destination folder');
                setIsLoading(false);
                return;
            }

            const result = await createProjectLocal(config, destinationPath);

            if (result.success) {
                onCreate(config, creationMode);
            } else {
                setError(result.error || 'Failed to create project');
            }
        } else {
            // Download mode - just call onCreate for now (ZIP will be implemented later)
            onCreate(config, creationMode);
        }

        setIsLoading(false);
    };

    const handlePickDirectory = async () => {
        const path = await pickDirectory();
        if (path) {
            setDestinationPath(path);
        }
    };

    const canProceed = () => {
        if (step === 'name') return name.length >= 3 && namespace.length >= 3;
        if (step === 'frontends') return true; // Can have no frontend selected
        return true;
    };

    const stepNumber = step === 'name' ? 1 : step === 'frontends' ? 2 : 3;

    return (
        <div className="ui-dialog-overlay" onClick={onClose}>
            <div
                className="ui-dialog"
                style={{ width: '600px', maxHeight: '85vh' }}
                onClick={(e) => e.stopPropagation()}
            >
                <div className="ui-dialog-header">
                    <h2 className="ui-dialog-title">
                        <FolderPlus size={20} style={{ color: '#22c55e' }} />
                        New Project
                    </h2>
                    <button className="btn-icon" onClick={onClose}>
                        <X size={20} />
                    </button>
                </div>

                {/* Progress Indicator */}
                <div style={{
                    display: 'flex',
                    justifyContent: 'center',
                    gap: '8px',
                    padding: '16px',
                    borderBottom: '1px solid #334155'
                }}>
                    {['name', 'frontends', 'destination'].map((s, i) => (
                        <div
                            key={s}
                            style={{
                                width: '32px',
                                height: '32px',
                                borderRadius: '50%',
                                display: 'flex',
                                alignItems: 'center',
                                justifyContent: 'center',
                                background: stepNumber > i ? '#22c55e' : stepNumber === i + 1 ? '#6366f1' : '#334155',
                                color: '#fff',
                                fontSize: '14px',
                                fontWeight: 600,
                            }}
                        >
                            {stepNumber > i ? <Check size={16} /> : i + 1}
                        </div>
                    ))}
                </div>

                <div className="ui-dialog-content" style={{ padding: '24px' }}>
                    {/* Step 1: Name */}
                    {step === 'name' && (
                        <div style={{ display: 'flex', flexDirection: 'column', gap: '20px' }}>
                            <h3 style={{ color: '#f8fafc', margin: 0 }}>Project Identity</h3>

                            <div className="form-group">
                                <label style={{ color: '#f8fafc', marginBottom: '8px', display: 'block' }}>
                                    Project Name
                                </label>
                                <input
                                    type="text"
                                    value={name}
                                    onChange={(e) => handleNameChange(e.target.value)}
                                    placeholder="MeuProjeto"
                                    className="ui-input"
                                    autoFocus
                                />
                                <p style={{ color: '#64748b', fontSize: '0.75rem', marginTop: '4px' }}>
                                    Used for folder and solution name
                                </p>
                            </div>

                            <div className="form-group">
                                <label style={{ color: '#f8fafc', marginBottom: '8px', display: 'block' }}>
                                    Namespace
                                </label>
                                <input
                                    type="text"
                                    value={namespace}
                                    onChange={(e) => setNamespace(e.target.value)}
                                    placeholder="Sapienza.MeuProjeto"
                                    className="ui-input"
                                />
                                <p style={{ color: '#64748b', fontSize: '0.75rem', marginTop: '4px' }}>
                                    .NET namespace for all projects
                                </p>
                            </div>
                        </div>
                    )}

                    {/* Step 2: Frontends */}
                    {step === 'frontends' && (
                        <div style={{ display: 'flex', flexDirection: 'column', gap: '20px' }}>
                            <h3 style={{ color: '#f8fafc', margin: 0 }}>Select Frontends</h3>
                            <p style={{ color: '#64748b', margin: 0 }}>
                                Backend (.NET) is always included. Choose your frontend(s):
                            </p>

                            {/* Backend (always included) */}
                            <div style={{
                                padding: '16px',
                                background: 'rgba(34, 197, 94, 0.1)',
                                border: '2px solid #22c55e',
                                borderRadius: '12px',
                                display: 'flex',
                                alignItems: 'center',
                                gap: '16px',
                            }}>
                                <Server size={32} style={{ color: '#22c55e' }} />
                                <div style={{ flex: 1 }}>
                                    <strong style={{ color: '#f8fafc' }}>Backend (.NET 8)</strong>
                                    <p style={{ color: '#94a3b8', margin: 0, fontSize: '0.875rem' }}>
                                        ABP Framework with all modules
                                    </p>
                                </div>
                                <Check size={24} style={{ color: '#22c55e' }} />
                            </div>

                            {/* Frontend Options */}
                            <div style={{ display: 'flex', flexDirection: 'column', gap: '12px' }}>
                                {FRONTEND_OPTIONS.map((frontend) => {
                                    const isSelected = selectedFrontends.has(frontend.id);
                                    return (
                                        <div
                                            key={frontend.id}
                                            onClick={() => toggleFrontend(frontend.id)}
                                            style={{
                                                padding: '16px',
                                                background: isSelected ? 'rgba(99, 102, 241, 0.1)' : '#0f172a',
                                                border: `2px solid ${isSelected ? '#6366f1' : '#334155'}`,
                                                borderRadius: '12px',
                                                display: 'flex',
                                                alignItems: 'center',
                                                gap: '16px',
                                                cursor: 'pointer',
                                                transition: 'all 0.2s',
                                            }}
                                        >
                                            <div style={{ color: isSelected ? '#6366f1' : '#64748b' }}>
                                                {frontend.icon}
                                            </div>
                                            <div style={{ flex: 1 }}>
                                                <strong style={{ color: '#f8fafc' }}>{frontend.name}</strong>
                                                <p style={{ color: '#94a3b8', margin: 0, fontSize: '0.875rem' }}>
                                                    {frontend.description}
                                                </p>
                                            </div>
                                            <div style={{
                                                width: '24px',
                                                height: '24px',
                                                borderRadius: '6px',
                                                border: `2px solid ${isSelected ? '#6366f1' : '#334155'}`,
                                                background: isSelected ? '#6366f1' : 'transparent',
                                                display: 'flex',
                                                alignItems: 'center',
                                                justifyContent: 'center',
                                            }}>
                                                {isSelected && <Check size={16} style={{ color: '#fff' }} />}
                                            </div>
                                        </div>
                                    );
                                })}
                            </div>
                        </div>
                    )}

                    {/* Step 3: Destination */}
                    {step === 'destination' && (
                        <div style={{ display: 'flex', flexDirection: 'column', gap: '20px' }}>
                            <h3 style={{ color: '#f8fafc', margin: 0 }}>Where to create?</h3>

                            {/* Bridge unavailable warning */}
                            {!bridgeAvailable && (
                                <div style={{
                                    padding: '12px 16px',
                                    background: 'rgba(234, 179, 8, 0.1)',
                                    border: '1px solid #eab308',
                                    borderRadius: '8px',
                                    color: '#eab308',
                                    fontSize: '0.875rem',
                                }}>
                                    ⚠️ Bridge server not running. Start with <code>node bridge.js</code> for local creation.
                                </div>
                            )}

                            {/* Error display */}
                            {error && (
                                <div style={{
                                    padding: '12px 16px',
                                    background: 'rgba(239, 68, 68, 0.1)',
                                    border: '1px solid #ef4444',
                                    borderRadius: '8px',
                                    color: '#ef4444',
                                    fontSize: '0.875rem',
                                }}>
                                    {error}
                                </div>
                            )}

                            <div style={{ display: 'flex', flexDirection: 'column', gap: '12px' }}>
                                {/* Local option */}
                                <div
                                    onClick={() => bridgeAvailable && setCreationMode('local')}
                                    style={{
                                        padding: '20px',
                                        background: creationMode === 'local' ? 'rgba(34, 197, 94, 0.1)' : '#0f172a',
                                        border: `2px solid ${creationMode === 'local' ? '#22c55e' : '#334155'}`,
                                        borderRadius: '12px',
                                        display: 'flex',
                                        alignItems: 'center',
                                        gap: '16px',
                                        cursor: bridgeAvailable ? 'pointer' : 'not-allowed',
                                        opacity: bridgeAvailable ? 1 : 0.5,
                                    }}
                                >
                                    <Folder size={32} style={{ color: creationMode === 'local' ? '#22c55e' : '#64748b' }} />
                                    <div style={{ flex: 1 }}>
                                        <strong style={{ color: '#f8fafc' }}>Create in Local Folder</strong>
                                        <p style={{ color: '#94a3b8', margin: 0, fontSize: '0.875rem' }}>
                                            {bridgeAvailable ? 'Creates project in your local filesystem' : 'Requires Bridge server'}
                                        </p>
                                    </div>
                                    <div style={{
                                        width: '24px',
                                        height: '24px',
                                        borderRadius: '50%',
                                        border: `2px solid ${creationMode === 'local' ? '#22c55e' : '#334155'}`,
                                        background: creationMode === 'local' ? '#22c55e' : 'transparent',
                                        display: 'flex',
                                        alignItems: 'center',
                                        justifyContent: 'center',
                                    }}>
                                        {creationMode === 'local' && <Check size={14} style={{ color: '#fff' }} />}
                                    </div>
                                </div>

                                {/* Folder picker for local mode */}
                                {creationMode === 'local' && bridgeAvailable && (
                                    <div style={{
                                        padding: '16px',
                                        background: '#0f172a',
                                        border: '1px solid #334155',
                                        borderRadius: '8px',
                                    }}>
                                        <label style={{ color: '#94a3b8', fontSize: '0.75rem', marginBottom: '8px', display: 'block' }}>
                                            Destination Folder
                                        </label>
                                        <div style={{ display: 'flex', gap: '8px' }}>
                                            <input
                                                type="text"
                                                value={destinationPath}
                                                onChange={(e) => setDestinationPath(e.target.value)}
                                                placeholder="/Users/you/projects"
                                                className="ui-input"
                                                style={{ flex: 1 }}
                                            />
                                            <button
                                                className="ui-button ui-button-secondary"
                                                onClick={handlePickDirectory}
                                                type="button"
                                            >
                                                <FolderOpen size={16} />
                                                Browse
                                            </button>
                                        </div>
                                        <p style={{ color: '#64748b', fontSize: '0.75rem', marginTop: '4px' }}>
                                            Project will be created in: {destinationPath ? `${destinationPath}/${name}` : '(select folder)'}
                                        </p>
                                    </div>
                                )}

                                {/* Download option */}
                                <div
                                    onClick={() => setCreationMode('download')}
                                    style={{
                                        padding: '20px',
                                        background: creationMode === 'download' ? 'rgba(99, 102, 241, 0.1)' : '#0f172a',
                                        border: `2px solid ${creationMode === 'download' ? '#6366f1' : '#334155'}`,
                                        borderRadius: '12px',
                                        display: 'flex',
                                        alignItems: 'center',
                                        gap: '16px',
                                        cursor: 'pointer',
                                    }}
                                >
                                    <Download size={32} style={{ color: creationMode === 'download' ? '#6366f1' : '#64748b' }} />
                                    <div style={{ flex: 1 }}>
                                        <strong style={{ color: '#f8fafc' }}>Download as ZIP</strong>
                                        <p style={{ color: '#94a3b8', margin: 0, fontSize: '0.875rem' }}>
                                            Package project and download to your computer
                                        </p>
                                    </div>
                                    <div style={{
                                        width: '24px',
                                        height: '24px',
                                        borderRadius: '50%',
                                        border: `2px solid ${creationMode === 'download' ? '#6366f1' : '#334155'}`,
                                        background: creationMode === 'download' ? '#6366f1' : 'transparent',
                                        display: 'flex',
                                        alignItems: 'center',
                                        justifyContent: 'center',
                                    }}>
                                        {creationMode === 'download' && <Check size={14} style={{ color: '#fff' }} />}
                                    </div>
                                </div>
                            </div>

                            {/* Summary */}
                            <div style={{
                                padding: '16px',
                                background: '#0f172a',
                                borderRadius: '8px',
                                border: '1px solid #334155',
                            }}>
                                <h4 style={{ color: '#f8fafc', margin: '0 0 12px 0', fontSize: '0.875rem' }}>
                                    Project Summary
                                </h4>
                                <div style={{ display: 'grid', gridTemplateColumns: '120px 1fr', gap: '8px', fontSize: '0.875rem' }}>
                                    <span style={{ color: '#64748b' }}>Name:</span>
                                    <span style={{ color: '#f8fafc' }}>{name}</span>
                                    <span style={{ color: '#64748b' }}>Namespace:</span>
                                    <span style={{ color: '#f8fafc' }}>{namespace}</span>
                                    <span style={{ color: '#64748b' }}>Frontends:</span>
                                    <span style={{ color: '#f8fafc' }}>
                                        {selectedFrontends.size > 0
                                            ? Array.from(selectedFrontends).join(', ')
                                            : 'None (backend only)'}
                                    </span>
                                </div>
                            </div>
                        </div>
                    )}
                </div>

                <div className="ui-dialog-footer">
                    {step !== 'name' ? (
                        <button className="ui-button ui-button-secondary" onClick={handleBack}>
                            <ChevronLeft size={16} />
                            Back
                        </button>
                    ) : (
                        <button className="ui-button ui-button-secondary" onClick={onClose}>
                            Cancel
                        </button>
                    )}

                    {step !== 'destination' ? (
                        <button
                            className="ui-button ui-button-primary"
                            onClick={handleNext}
                            disabled={!canProceed()}
                        >
                            Next
                            <ChevronRight size={16} />
                        </button>
                    ) : (
                        <button
                            className="ui-button ui-button-primary"
                            onClick={handleCreate}
                            disabled={isLoading || (creationMode === 'local' && !destinationPath)}
                            style={{ background: 'linear-gradient(135deg, #22c55e 0%, #16a34a 100%)' }}
                        >
                            {isLoading ? (
                                <>
                                    <Loader2 size={16} style={{ animation: 'spin 1s linear infinite' }} />
                                    Creating...
                                </>
                            ) : (
                                <>
                                    {creationMode === 'local' ? <Folder size={16} /> : <Download size={16} />}
                                    {creationMode === 'local' ? 'Create Project' : 'Download ZIP'}
                                </>
                            )}
                        </button>
                    )}
                </div>
            </div>
        </div>
    );
}
