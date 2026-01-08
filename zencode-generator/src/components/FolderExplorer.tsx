import { useState, useEffect } from 'react';
import { Folder, ChevronRight, ChevronLeft, Home } from 'lucide-react';

interface FolderExplorerProps {
    onSelect: (path: string) => void;
    initialPath?: string;
}

export default function FolderExplorer({ onSelect, initialPath }: FolderExplorerProps) {
    const [currentPath, setCurrentPath] = useState(initialPath || '');
    const [dirs, setDirs] = useState<string[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const fetchDirs = async (path: string) => {
        setLoading(true);
        setError('');
        try {
            const response = await fetch('http://localhost:3001/api/list-dirs', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ directory: path })
            });

            if (response.ok) {
                const data = await response.json();
                setCurrentPath(data.currentPath);
                setDirs(data.dirs);
            } else {
                const data = await response.json();
                setError(data.error || 'Failed to list directories');
            }
        } catch (err) {
            setError('Could not connect to bridge');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchDirs(currentPath);
    }, []);

    const handleDirClick = (dir: string) => {
        const separator = currentPath.includes('\\') ? '\\' : '/';
        const newPath = currentPath.endsWith(separator) ? currentPath + dir : currentPath + separator + dir;
        fetchDirs(newPath);
    };

    const handleGoBack = () => {
        const separator = currentPath.includes('\\') ? '\\' : '/';
        const parts = currentPath.split(separator).filter(p => p !== '');
        if (parts.length > 0) {
            parts.pop();
            const parent = (currentPath.startsWith('/') ? '/' : '') + parts.join(separator);
            fetchDirs(parent || '/');
        }
    };

    return (
        <div className="folder-explorer">
            <div className="explorer-header">
                <div className="current-path-display">
                    <span
                        className="home-icon"
                        onClick={() => fetchDirs('')}
                        title="Ir para Home"
                    >
                        <Home size={14} />
                    </span>
                    <span title={currentPath}>{currentPath}</span>
                </div>
                <div className="header-actions">
                    <button className="btn-icon-sm" onClick={handleGoBack} title="Voltar">
                        <ChevronLeft size={16} />
                    </button>
                    <button className="btn-primary btn-sm" onClick={() => onSelect(currentPath)}>
                        Selecionar
                    </button>
                </div>
            </div>

            <div className="explorer-list">
                {loading ? (
                    <div className="explorer-loading">Listando diretórios...</div>
                ) : error ? (
                    <div className="explorer-error">
                        {error}
                        <button className="btn-secondary btn-sm mt-2" onClick={() => fetchDirs('')}>Tentar Home</button>
                    </div>
                ) : (
                    dirs.map(dir => (
                        <div key={dir} className="explorer-item" onClick={() => handleDirClick(dir)}>
                            <Folder size={16} className="folder-icon" />
                            <span className="folder-name">{dir}</span>
                            <ChevronRight size={14} className="arrow-icon" />
                        </div>
                    ))
                )}
            </div>
        </div>
    );
}
