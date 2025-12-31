import { X, Upload, FileText, Check } from 'lucide-react';
import { useState, useCallback } from 'react';
import { parseSqlToEntities } from '../utils/sqlParser';
import type { EntityData } from '../types';

interface ImportSqlModalProps {
    onImport: (entities: EntityData[]) => void;
    onClose: () => void;
}

export default function ImportSqlModal({ onImport, onClose }: ImportSqlModalProps) {
    const [sqlText, setSqlText] = useState('');
    const [isDragging, setIsDragging] = useState(false);

    const handleProcess = () => {
        if (!sqlText.trim()) return;
        const entities = parseSqlToEntities(sqlText);
        if (entities.length > 0) {
            onImport(entities);
            onClose();
        } else {
            alert('Nenhuma instrução CREATE TABLE válida encontrada.');
        }
    };

    const handleFileUpload = (files: FileList | null) => {
        if (!files) return;

        Array.from(files).forEach(file => {
            const reader = new FileReader();
            reader.onload = (e) => {
                const text = e.target?.result as string;
                setSqlText(prev => prev + '\n\n' + text);
            };
            reader.readAsText(file);
        });
    };

    const onDrop = useCallback((e: React.DragEvent) => {
        e.preventDefault();
        setIsDragging(false);
        handleFileUpload(e.dataTransfer.files);
    }, []);

    return (
        <div className="modal-overlay">
            <div className="preview-modal import-modal">
                <div className="modal-header">
                    <div className="header-left">
                        <Upload size={20} className="text-blue-400" />
                        <h3>Import SQL DDL</h3>
                    </div>
                    <button className="btn-icon-alt" onClick={onClose}>
                        <X size={18} />
                    </button>
                </div>

                <div className="modal-content">
                    <p className="modal-description">
                        Paste as instruções <code>CREATE TABLE</code> abaixo ou arraste seus arquivos <code>.sql</code>.
                    </p>

                    <div
                        className={`drop-zone ${isDragging ? 'dragging' : ''}`}
                        onDragOver={(e) => { e.preventDefault(); setIsDragging(true); }}
                        onDragLeave={() => setIsDragging(false)}
                        onDrop={onDrop}
                    >
                        <textarea
                            className="sql-textarea"
                            placeholder="CREATE TABLE [SA1] ( ... )"
                            value={sqlText}
                            onChange={(e) => setSqlText(e.target.value)}
                        />

                        <div className="drop-zone-overlay">
                            <FileText size={48} />
                            <p>Solte os arquivos SQL aqui</p>
                        </div>
                    </div>

                    <div className="file-input-wrapper">
                        <input
                            type="file"
                            multiple
                            accept=".sql"
                            id="sql-file-input"
                            onChange={(e) => handleFileUpload(e.target.files)}
                            className="hidden-input"
                        />
                        <label htmlFor="sql-file-input" className="btn-secondary btn-full">
                            <Upload size={16} />
                            Selecionar Arquivos .sql
                        </label>
                    </div>
                </div>

                <div className="modal-footer">
                    <button className="btn-secondary" onClick={onClose}>Cancelar</button>
                    <button className="btn-primary" onClick={handleProcess} disabled={!sqlText.trim()}>
                        Gerar Entidades
                    </button>
                </div>
            </div>
        </div>
    );
}
