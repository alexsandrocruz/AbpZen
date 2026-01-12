
import React, { useState } from 'react';
import { Database, Loader2, X, Check, Server, Eye, EyeOff } from 'lucide-react';
import type { Node, Edge } from 'reactflow';

interface ImportDbModalProps {
    isOpen: boolean;
    onClose: () => void;
    onImport: (nodes: Node[], edges: Edge[]) => void;
}

export function ImportDbModal({ isOpen, onClose, onImport }: ImportDbModalProps) {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [showPassword, setShowPassword] = useState(false);

    const [provider, setProvider] = useState<'postgres' | 'mssql'>('postgres');
    const [config, setConfig] = useState({
        host: 'localhost',
        port: '5432',
        database: '',
        user: 'postgres',
        password: '',
        schema: 'public'
    });

    if (!isOpen) return null;

    const handleProviderChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const newProvider = e.target.value as 'postgres' | 'mssql';
        setProvider(newProvider);
        // Set default ports/schemas
        setConfig(prev => ({
            ...prev,
            port: newProvider === 'postgres' ? '5432' : '1433',
            user: newProvider === 'postgres' ? 'postgres' : 'sa',
            schema: newProvider === 'postgres' ? 'public' : 'dbo'
        }));
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setConfig(prev => ({ ...prev, [name]: value }));
    };

    const handleImport = async () => {
        setLoading(true);
        setError(null);

        try {
            const response = await fetch('http://localhost:3005/api/import-db', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    provider,
                    ...config
                }),
            });

            const data = await response.json();

            if (!data.success) {
                throw new Error(data.error || 'Failed to import database');
            }

            if (data.nodes && data.nodes.length > 0) {
                onImport(data.nodes, data.edges || []);
                onClose();
            } else {
                setError('No tables found in the specified database/schema.');
            }
        } catch (err: any) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="modal-overlay" style={{ zIndex: 9999 }}>
            <div className="preview-modal" style={{ maxWidth: '480px' }}>
                <div className="modal-header">
                    <div className="header-left">
                        <div className="brand-icon" style={{ width: '32px', height: '32px', borderRadius: '8px', fontSize: '14px' }}>
                            <Database size={18} />
                        </div>
                        <h3>Import Database</h3>
                    </div>
                    <div className="header-actions">
                        <button className="btn-icon" onClick={onClose}>
                            <X size={20} />
                        </button>
                    </div>
                </div>

                <div className="modal-content">
                    <div className="form-section">
                        {/* Provider Selection */}
                        <div className="form-group">
                            <label>Database Provider</label>
                            <div className="sidebar" style={{ position: 'static', width: '100%', padding: 0, background: 'transparent', border: 'none' }}>
                                <select
                                    value={provider}
                                    onChange={handleProviderChange}
                                >
                                    <option value="postgres">PostgreSQL</option>
                                    <option value="mssql">SQL Server (MSSQL)</option>
                                </select>
                            </div>
                        </div>

                        {/* Connection Details */}
                        <div className="grid grid-cols-2 gap-4" style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                            <div className="form-group">
                                <label>Host</label>
                                <input
                                    name="host"
                                    value={config.host}
                                    onChange={handleChange}
                                    placeholder="localhost"
                                />
                            </div>
                            <div className="form-group">
                                <label>Port</label>
                                <input
                                    name="port"
                                    value={config.port}
                                    onChange={handleChange}
                                />
                            </div>
                        </div>

                        <div className="form-group">
                            <label>Database Name</label>
                            <input
                                name="database"
                                value={config.database}
                                onChange={handleChange}
                                placeholder="my_abp_project"
                            />
                        </div>

                        <div className="grid grid-cols-2 gap-4" style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                            <div className="form-group">
                                <label>Username</label>
                                <input
                                    name="user"
                                    value={config.user}
                                    onChange={handleChange}
                                />
                            </div>
                            <div className="form-group">
                                <label>Password</label>
                                <div style={{ position: 'relative' }}>
                                    <input
                                        type={showPassword ? "text" : "password"}
                                        name="password"
                                        value={config.password}
                                        onChange={handleChange}
                                        style={{ paddingRight: '36px' }}
                                    />
                                    <button
                                        type="button"
                                        onClick={() => setShowPassword(!showPassword)}
                                        style={{
                                            position: 'absolute',
                                            right: '8px',
                                            top: '50%',
                                            transform: 'translateY(-50%)',
                                            padding: '4px',
                                            background: 'none',
                                            border: 'none',
                                            color: '#94a3b8',
                                            cursor: 'pointer'
                                        }}
                                    >
                                        {showPassword ? <EyeOff size={16} /> : <Eye size={16} />}
                                    </button>
                                </div>
                            </div>
                        </div>

                        <div className="form-group">
                            <label>Schema</label>
                            <input
                                name="schema"
                                value={config.schema}
                                onChange={handleChange}
                            />
                        </div>

                        {error && (
                            <div style={{
                                padding: '12px',
                                background: 'rgba(239, 68, 68, 0.1)',
                                border: '1px solid rgba(239, 68, 68, 0.2)',
                                borderRadius: '8px',
                                color: '#fca5a5',
                                fontSize: '0.9rem',
                                display: 'flex',
                                alignItems: 'flex-start',
                                gap: '8px',
                                marginBottom: '16px'
                            }}>
                                <Server size={18} style={{ marginTop: '2px', flexShrink: 0 }} />
                                <span>{error}</span>
                            </div>
                        )}
                    </div>
                </div>

                <div className="modal-footer">
                    <button className="btn-secondary" onClick={onClose} style={{ padding: '10px 20px' }}>
                        Cancel
                    </button>
                    <button
                        className="btn-primary"
                        onClick={handleImport}
                        disabled={loading || !config.database}
                        style={{ padding: '10px 20px' }}
                    >
                        {loading ? (
                            <>
                                <Loader2 size={18} className="animate-spin" />
                                Connecting...
                            </>
                        ) : (
                            <>
                                <Check size={18} />
                                Connect & Import
                            </>
                        )}
                    </button>
                </div>
            </div>
        </div>
    );
}
