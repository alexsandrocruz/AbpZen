import { useState } from 'react';
import { Download, X, FileCode, FolderOpen, ChevronDown, ChevronRight, Loader2 } from 'lucide-react';
import type { EntityData } from '../types';
import { codeGenerator, type GeneratedFile } from '../generators';
import { downloadAsZip, getFileIcon, getLayerColor } from '../generators/zip';

interface GenerateCodeModalProps {
    entities: EntityData[];
    projectName: string;
    projectNamespace: string;
    onClose: () => void;
}

export default function GenerateCodeModal({
    entities,
    projectName,
    projectNamespace,
    onClose
}: GenerateCodeModalProps) {
    const [files, setFiles] = useState<GeneratedFile[]>([]);
    const [generating, setGenerating] = useState(false);
    const [selectedFile, setSelectedFile] = useState<GeneratedFile | null>(null);
    const [expandedLayers, setExpandedLayers] = useState<Set<string>>(new Set(['Application', 'Application.Contracts']));

    const handleGenerate = async () => {
        setGenerating(true);
        try {
            const generated = await codeGenerator.generateAll(entities, projectName, projectNamespace);
            setFiles(generated);
            if (generated.length > 0) {
                setSelectedFile(generated[0]);
            }
        } catch (error) {
            console.error('Generation error:', error);
        } finally {
            setGenerating(false);
        }
    };

    const handleDownload = async () => {
        await downloadAsZip(files, projectName);
    };

    const toggleLayer = (layer: string) => {
        setExpandedLayers(prev => {
            const next = new Set(prev);
            if (next.has(layer)) {
                next.delete(layer);
            } else {
                next.add(layer);
            }
            return next;
        });
    };

    // Group files by layer
    const filesByLayer = files.reduce((acc, file) => {
        if (!acc[file.layer]) acc[file.layer] = [];
        acc[file.layer].push(file);
        return acc;
    }, {} as Record<string, GeneratedFile[]>);

    return (
        <div className="ui-dialog-overlay" onClick={onClose}>
            <div
                className="ui-dialog"
                style={{ width: '1200px', maxHeight: '90vh' }}
                onClick={(e) => e.stopPropagation()}
            >
                <div className="ui-dialog-header">
                    <h2 className="ui-dialog-title">
                        <FileCode size={20} />
                        Generate Code
                    </h2>
                    <button className="btn-icon" onClick={onClose}>
                        <X size={20} />
                    </button>
                </div>

                <div className="ui-dialog-content" style={{ padding: 0 }}>
                    {files.length === 0 ? (
                        <div style={{ padding: '48px', textAlign: 'center' }}>
                            <FileCode size={48} style={{ color: '#6366f1', marginBottom: '16px' }} />
                            <h3 style={{ color: '#f8fafc', marginBottom: '8px' }}>Ready to Generate</h3>
                            <p style={{ color: '#64748b', marginBottom: '24px' }}>
                                {entities.length} entities will be converted to ABP backend code
                            </p>

                            <div style={{ marginBottom: '24px', textAlign: 'left', maxWidth: '400px', margin: '0 auto 24px' }}>
                                <div className="form-group">
                                    <label>Project Name</label>
                                    <input type="text" value={projectName} disabled className="ui-input" />
                                </div>
                                <div className="form-group" style={{ marginTop: '12px' }}>
                                    <label>Namespace</label>
                                    <input type="text" value={projectNamespace} disabled className="ui-input" />
                                </div>
                            </div>

                            <button
                                className="ui-button ui-button-primary"
                                onClick={handleGenerate}
                                disabled={generating}
                                style={{ padding: '12px 32px' }}
                            >
                                {generating ? (
                                    <>
                                        <Loader2 size={18} className="animate-spin" />
                                        Generating...
                                    </>
                                ) : (
                                    <>
                                        <FileCode size={18} />
                                        Generate Code
                                    </>
                                )}
                            </button>
                        </div>
                    ) : (
                        <div style={{ display: 'flex', height: '500px' }}>
                            {/* File tree */}
                            <div style={{
                                width: '300px',
                                borderRight: '1px solid #334155',
                                overflow: 'auto',
                                background: '#0f172a'
                            }}>
                                <div style={{ padding: '12px', borderBottom: '1px solid #334155' }}>
                                    <span style={{ color: '#64748b', fontSize: '0.75rem' }}>
                                        {files.length} files generated
                                    </span>
                                </div>

                                {Object.entries(filesByLayer).map(([layer, layerFiles]) => (
                                    <div key={layer}>
                                        <div
                                            style={{
                                                padding: '8px 12px',
                                                display: 'flex',
                                                alignItems: 'center',
                                                gap: '8px',
                                                cursor: 'pointer',
                                                background: '#1e293b',
                                                borderBottom: '1px solid #334155',
                                            }}
                                            onClick={() => toggleLayer(layer)}
                                        >
                                            {expandedLayers.has(layer) ? <ChevronDown size={14} /> : <ChevronRight size={14} />}
                                            <FolderOpen size={14} style={{ color: getLayerColor(layer as any) }} />
                                            <span style={{ color: '#f8fafc', fontSize: '0.875rem' }}>{layer}</span>
                                            <span style={{ color: '#64748b', fontSize: '0.75rem', marginLeft: 'auto' }}>
                                                {layerFiles.length}
                                            </span>
                                        </div>

                                        {expandedLayers.has(layer) && (
                                            <div>
                                                {layerFiles.map(file => {
                                                    const fileName = file.path.split('/').pop();
                                                    return (
                                                        <div
                                                            key={file.path}
                                                            style={{
                                                                padding: '6px 12px 6px 32px',
                                                                display: 'flex',
                                                                alignItems: 'center',
                                                                gap: '8px',
                                                                cursor: 'pointer',
                                                                background: selectedFile?.path === file.path ? '#334155' : 'transparent',
                                                            }}
                                                            onClick={() => setSelectedFile(file)}
                                                        >
                                                            <span>{getFileIcon(file.path)}</span>
                                                            <span style={{
                                                                color: selectedFile?.path === file.path ? '#f8fafc' : '#94a3b8',
                                                                fontSize: '0.8125rem',
                                                                overflow: 'hidden',
                                                                textOverflow: 'ellipsis',
                                                                whiteSpace: 'nowrap',
                                                            }}>
                                                                {fileName}
                                                            </span>
                                                        </div>
                                                    );
                                                })}
                                            </div>
                                        )}
                                    </div>
                                ))}
                            </div>

                            {/* Code preview */}
                            <div style={{ flex: 1, overflow: 'auto', background: '#0f172a' }}>
                                {selectedFile && (
                                    <>
                                        <div style={{
                                            padding: '8px 16px',
                                            borderBottom: '1px solid #334155',
                                            background: '#1e293b',
                                        }}>
                                            <span style={{ color: '#64748b', fontSize: '0.75rem' }}>
                                                {selectedFile.path}
                                            </span>
                                        </div>
                                        <pre style={{
                                            margin: 0,
                                            padding: '16px',
                                            color: '#e2e8f0',
                                            fontSize: '0.8125rem',
                                            lineHeight: '1.6',
                                            fontFamily: 'JetBrains Mono, Consolas, monospace',
                                        }}>
                                            {selectedFile.content}
                                        </pre>
                                    </>
                                )}
                            </div>
                        </div>
                    )}
                </div>

                {files.length > 0 && (
                    <div className="ui-dialog-footer">
                        <button className="ui-button ui-button-secondary" onClick={() => setFiles([])}>
                            Regenerate
                        </button>
                        <button className="ui-button ui-button-primary" onClick={handleDownload}>
                            <Download size={18} />
                            Download ZIP
                        </button>
                    </div>
                )}
            </div>
        </div>
    );
}
