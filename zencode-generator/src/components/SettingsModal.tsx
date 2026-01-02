import { X, CheckCircle, AlertCircle, FolderPlus, RefreshCw } from 'lucide-react';
import { useState } from 'react';

interface ProjectConfig {
    path: string;
    projectName: string;
    namespace: string;
}

interface SettingsModalProps {
    projectPath: string;
    projectName?: string;
    projectNamespace?: string;
    onSave: (config: ProjectConfig) => void;
    onClose: () => void;
}

export default function SettingsModal({
    projectPath: initialPath,
    projectName: initialName = '',
    projectNamespace: initialNamespace = '',
    onSave,
    onClose
}: SettingsModalProps) {
    const [path, setPath] = useState(initialPath);
    const [projectName, setProjectName] = useState(initialName);
    const [namespace, setNamespace] = useState(initialNamespace);
    const [isVerifying, setIsVerifying] = useState(false);
    const [isDetecting, setIsDetecting] = useState(false);
    const [status, setStatus] = useState<'idle' | 'success' | 'error'>('idle');
    const [errorMessage, setErrorMessage] = useState('');

    const handlePickDirectory = async () => {
        try {
            const response = await fetch('http://localhost:3001/api/pick-directory', {
                method: 'POST'
            });
            const data = await response.json();
            if (data.path) {
                const newPath = data.path.replace(/\/$/, '');
                setPath(newPath);
                // Auto-detect project info after picking directory
                detectProjectInfo(newPath);
            }
        } catch {
            setStatus('error');
            setErrorMessage('Bridge not running. Make sure to run "node bridge.js"');
        }
    };

    const detectProjectInfo = async (projectPath: string) => {
        if (!projectPath) return;
        setIsDetecting(true);
        try {
            const response = await fetch('http://localhost:3001/api/detect-project-info', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ projectPath })
            });
            const data = await response.json();
            if (data.projectName) setProjectName(data.projectName);
            if (data.namespace) setNamespace(data.namespace);
        } catch {
            // Silently fail - user can still enter manually
        } finally {
            setIsDetecting(false);
        }
    };

    const handleVerify = async () => {
        if (!path) return;
        setIsVerifying(true);
        setStatus('idle');

        try {
            const response = await fetch('http://localhost:3001/api/save-metadata', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ projectPath: path, metadata: { test: true } })
            });

            if (response.ok) {
                setStatus('success');
                // Also detect project info on verify
                await detectProjectInfo(path);
            } else {
                const data = await response.json();
                setStatus('error');
                setErrorMessage(data.error || 'Failed to connect to bridge');
            }
        } catch {
            setStatus('error');
            setErrorMessage('Bridge not running. Make sure to run "node bridge.js"');
        } finally {
            setIsVerifying(false);
        }
    };

    return (
        <div className="modal-overlay">
            <div className="modal-content settings-modal">
                <div className="modal-header">
                    <h3>Project Settings</h3>
                    <button className="btn-icon-sm" onClick={onClose}>
                        <X size={18} />
                    </button>
                </div>

                <div className="modal-body">
                    <div className="form-group">
                        <label>Project Root Path (Absolute)</label>
                        <div className="input-with-button">
                            <input
                                type="text"
                                value={path}
                                onChange={(e) => setPath(e.target.value)}
                                placeholder="/Users/alexsandrocruz/Documents/dev/AbpZen/demo-zen"
                                className="btn-input"
                            />
                            <button
                                className="btn-icon-sm"
                                onClick={handlePickDirectory}
                                title="Open Native Folder Picker"
                            >
                                <FolderPlus size={18} />
                            </button>
                            <button
                                className="btn-secondary"
                                onClick={handleVerify}
                                disabled={isVerifying || !path}
                            >
                                {isVerifying ? 'Verifying...' : 'Verify Path'}
                            </button>
                        </div>
                        <p className="form-help">Path to the ABP project where code will be injected.</p>
                    </div>

                    <div className="form-group">
                        <label>Project Name</label>
                        <div className="input-with-button">
                            <input
                                type="text"
                                value={projectName}
                                onChange={(e) => setProjectName(e.target.value)}
                                placeholder="LeptonXDemoApp"
                                className="btn-input"
                            />
                            <button
                                className="btn-icon-sm"
                                onClick={() => detectProjectInfo(path)}
                                disabled={isDetecting || !path}
                                title="Auto-detect from project"
                            >
                                <RefreshCw size={16} className={isDetecting ? 'spin' : ''} />
                            </button>
                        </div>
                        <p className="form-help">Name used for generated project folders.</p>
                    </div>

                    <div className="form-group">
                        <label>Namespace</label>
                        <input
                            type="text"
                            value={namespace}
                            onChange={(e) => setNamespace(e.target.value)}
                            placeholder="LeptonXDemoApp"
                            className="btn-input"
                        />
                        <p className="form-help">C# namespace for generated code (e.g., YourCompany.YourApp).</p>
                    </div>

                    {status === 'success' && (
                        <div className="status-msg success">
                            <CheckCircle size={16} />
                            <span>Bridge connected and path verified!</span>
                        </div>
                    )}

                    {status === 'error' && (
                        <div className="status-msg error">
                            <AlertCircle size={16} />
                            <span>{errorMessage}</span>
                        </div>
                    )}
                </div>

                <div className="modal-footer">
                    <button className="btn-secondary" onClick={onClose}>Cancel</button>
                    <button
                        className="btn-primary"
                        onClick={() => onSave({ path, projectName, namespace })}
                        disabled={!path || !projectName || !namespace}
                    >
                        Save Settings
                    </button>
                </div>
            </div>
        </div>
    );
}
