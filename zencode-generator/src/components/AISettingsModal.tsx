import { useState, useEffect } from 'react';
import { Settings, X, Loader2, Check, AlertCircle } from 'lucide-react';
import { getAISettings, saveAISettings, testGeminiConnection } from '../lib/gemini/client';
import type { AISettings } from '../lib/gemini/types';

interface AISettingsModalProps {
    onClose: () => void;
}

export default function AISettingsModal({ onClose }: AISettingsModalProps) {
    const [settings, setSettings] = useState<AISettings>({
        apiKey: '',
        model: 'gemini-2.0-flash-exp',
        temperature: 0.2
    });
    const [testing, setTesting] = useState(false);
    const [testResult, setTestResult] = useState<'success' | 'error' | null>(null);
    const [saved, setSaved] = useState(false);

    useEffect(() => {
        const stored = getAISettings();
        setSettings(stored);
    }, []);

    const handleTest = async () => {
        if (!settings.apiKey) return;

        setTesting(true);
        setTestResult(null);

        const success = await testGeminiConnection(settings.apiKey);
        setTestResult(success ? 'success' : 'error');
        setTesting(false);
    };

    const handleSave = () => {
        saveAISettings(settings);
        setSaved(true);
        setTimeout(() => setSaved(false), 2000);
    };

    const maskedApiKey = settings.apiKey
        ? `${settings.apiKey.slice(0, 8)}...${settings.apiKey.slice(-4)}`
        : '';

    return (
        <div className="ui-dialog-overlay" onClick={onClose}>
            <div
                className="ui-dialog"
                style={{ width: '500px' }}
                onClick={(e) => e.stopPropagation()}
            >
                <div className="ui-dialog-header">
                    <h2 className="ui-dialog-title">
                        <Settings size={20} />
                        AI Settings
                    </h2>
                    <button className="btn-icon" onClick={onClose}>
                        <X size={20} />
                    </button>
                </div>

                <div className="ui-dialog-content" style={{ padding: '24px' }}>
                    {/* API Key */}
                    <div className="form-group" style={{ marginBottom: '20px' }}>
                        <label style={{ color: '#f8fafc', marginBottom: '8px', display: 'block' }}>
                            Google Gemini API Key
                        </label>
                        <div style={{ display: 'flex', gap: '8px' }}>
                            <input
                                type="password"
                                value={settings.apiKey}
                                onChange={(e) => setSettings({ ...settings, apiKey: e.target.value })}
                                placeholder="AIza..."
                                className="ui-input"
                                style={{ flex: 1 }}
                            />
                            <button
                                className="ui-button ui-button-secondary"
                                onClick={handleTest}
                                disabled={!settings.apiKey || testing}
                                style={{ minWidth: '80px' }}
                            >
                                {testing ? (
                                    <Loader2 size={16} className="animate-spin" />
                                ) : testResult === 'success' ? (
                                    <Check size={16} style={{ color: '#22c55e' }} />
                                ) : testResult === 'error' ? (
                                    <AlertCircle size={16} style={{ color: '#ef4444' }} />
                                ) : (
                                    'Test'
                                )}
                            </button>
                        </div>
                        {settings.apiKey && (
                            <p style={{ color: '#64748b', fontSize: '0.75rem', marginTop: '4px' }}>
                                Stored: {maskedApiKey}
                            </p>
                        )}
                        {testResult === 'success' && (
                            <p style={{ color: '#22c55e', fontSize: '0.75rem', marginTop: '4px' }}>
                                ✓ Connection successful!
                            </p>
                        )}
                        {testResult === 'error' && (
                            <p style={{ color: '#ef4444', fontSize: '0.75rem', marginTop: '4px' }}>
                                ✗ Connection failed. Check your API key.
                            </p>
                        )}
                    </div>

                    {/* Model Selection */}
                    <div className="form-group" style={{ marginBottom: '20px' }}>
                        <label style={{ color: '#f8fafc', marginBottom: '8px', display: 'block' }}>
                            Model
                        </label>
                        <select
                            value={settings.model}
                            onChange={(e) => setSettings({ ...settings, model: e.target.value as AISettings['model'] })}
                            className="ui-input"
                        >
                            <option value="gemini-2.0-flash-exp">Gemini 2.0 Flash (Recommended)</option>
                            <option value="gemini-1.5-flash">Gemini 1.5 Flash (Fast)</option>
                            <option value="gemini-1.5-pro">Gemini 1.5 Pro (Advanced)</option>
                        </select>
                    </div>

                    {/* Temperature */}
                    <div className="form-group" style={{ marginBottom: '20px' }}>
                        <label style={{ color: '#f8fafc', marginBottom: '8px', display: 'block' }}>
                            Temperature: {settings.temperature}
                        </label>
                        <input
                            type="range"
                            min="0"
                            max="1"
                            step="0.1"
                            value={settings.temperature}
                            onChange={(e) => setSettings({ ...settings, temperature: parseFloat(e.target.value) })}
                            style={{ width: '100%' }}
                        />
                        <div style={{ display: 'flex', justifyContent: 'space-between', color: '#64748b', fontSize: '0.75rem' }}>
                            <span>Precise (0)</span>
                            <span>Creative (1)</span>
                        </div>
                    </div>

                    {/* Info */}
                    <div style={{
                        padding: '12px',
                        background: 'rgba(99, 102, 241, 0.1)',
                        borderRadius: '8px',
                        border: '1px solid rgba(99, 102, 241, 0.3)'
                    }}>
                        <p style={{ color: '#94a3b8', fontSize: '0.8rem', margin: 0 }}>
                            💡 Get your free API key at{' '}
                            <a
                                href="https://aistudio.google.com/apikey"
                                target="_blank"
                                rel="noopener noreferrer"
                                style={{ color: '#6366f1' }}
                            >
                                Google AI Studio
                            </a>
                        </p>
                    </div>
                </div>

                <div className="ui-dialog-footer">
                    <button className="ui-button ui-button-secondary" onClick={onClose}>
                        Cancel
                    </button>
                    <button
                        className="ui-button ui-button-primary"
                        onClick={handleSave}
                        style={{ minWidth: '100px' }}
                    >
                        {saved ? (
                            <>
                                <Check size={16} />
                                Saved!
                            </>
                        ) : (
                            'Save Settings'
                        )}
                    </button>
                </div>
            </div>
        </div>
    );
}
