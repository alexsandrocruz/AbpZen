import React, { useState, useEffect, useRef } from 'react';
import {
    Play,
    Square,
    Terminal as TerminalIcon,
    Activity,
    Database,
    Server,
    Globe,
    X,
    Copy,
    Download
} from 'lucide-react';
import {
    runTerminalCommand,
    getTerminalLogs,
    stopTerminalCommand
} from '../lib/project/generator';
import type { FrontendTarget } from '../types';

interface ProjectRunnerProps {
    projectPath: string;
    projectName: string;
    projectNamespace?: string;
    frontends: FrontendTarget[];
    onClose: () => void;
}

interface ProcessStatus {
    id: string;
    name: string;
    icon: React.ReactNode;
    command: string;
    cwd: string;
    status: 'stopped' | 'running' | 'error';
    logs: Array<{ type: 'stdout' | 'stderr', content: string, timestamp: number }>;
    offset: number;
}

export const ProjectRunner: React.FC<ProjectRunnerProps> = ({
    projectPath,
    projectName,
    projectNamespace,
    frontends,
    onClose
}) => {
    const [activeTab, setActiveTab] = useState<string>('infra');
    const [processes, setProcesses] = useState<Record<string, ProcessStatus>>({});
    const logEndRef = useRef<HTMLDivElement>(null);

    // Initialize processes based on project config
    useEffect(() => {
        // ABP standard paths: Host project is in the root or in /src
        // Try namespace first, then projectName
        const backendPath = projectNamespace
            ? `${projectPath}/${projectNamespace}.HttpApi.Host`
            : `${projectPath}/Sapienza.${projectName}.HttpApi.Host`;

        const initialProcesses: Record<string, ProcessStatus> = {
            infra: {
                id: 'infra',
                name: 'Infrastructure',
                icon: <Database size={20} />,
                command: 'docker compose -f docker-compose.infra.yml up',
                cwd: projectPath, // Infra files are usually in the root
                status: 'stopped',
                logs: [],
                offset: 0
            },
            backend: {
                id: 'backend',
                name: 'Backend (.NET)',
                icon: <Server size={20} />,
                command: 'dotnet run',
                cwd: backendPath,
                status: 'stopped',
                logs: [],
                offset: 0
            }
        };

        if (frontends.includes('angular')) {
            initialProcesses.angular = {
                id: 'angular',
                name: 'Angular UI',
                icon: <Globe size={20} />,
                command: 'ng serve',
                cwd: `${projectPath}/angular`,
                status: 'stopped',
                logs: [],
                offset: 0
            };
        }

        if (frontends.includes('react')) {
            initialProcesses.react = {
                id: 'react',
                name: 'React UI',
                icon: <Globe size={20} />,
                command: 'npm run dev',
                cwd: `${projectPath}/abp-react`,
                status: 'stopped',
                logs: [],
                offset: 0
            };
        }

        setProcesses(initialProcesses);
    }, [projectPath, projectName, frontends]);

    // Polling for logs
    useEffect(() => {
        const interval = setInterval(async () => {
            // Keep polling if running OR in error state (to catch the spawn error message)
            const activeIds = Object.keys(processes).filter(id =>
                processes[id].status === 'running' || processes[id].status === 'error'
            );
            if (activeIds.length === 0) return;

            for (const id of activeIds) {
                try {
                    const result = await getTerminalLogs(id, processes[id].offset);
                    if (result && result.logs && result.logs.length > 0) {
                        setProcesses(prev => ({
                            ...prev,
                            [id]: {
                                ...prev[id],
                                logs: [...prev[id].logs, ...result.logs].slice(-2000),
                                offset: result.nextOffset,
                                status: result.status as any
                            }
                        }));
                    } else if (result && result.status !== processes[id].status) {
                        setProcesses(prev => ({
                            ...prev,
                            [id]: {
                                ...prev[id],
                                status: result.status as any
                            }
                        }));
                    }
                } catch (e) {
                    console.error(`Error polling logs for ${id}:`, e);
                }
            }
        }, 1000);

        return () => clearInterval(interval);
    }, [processes]);

    // Auto-scroll logs
    useEffect(() => {
        if (logEndRef.current) {
            logEndRef.current.scrollIntoView({ behavior: 'smooth' });
        }
    }, [processes[activeTab]?.logs]);

    const handleStart = async (id: string) => {
        const proc = processes[id];
        setProcesses(prev => ({ ...prev, [id]: { ...prev[id], status: 'running', logs: [], offset: 0 } }));
        try {
            const result = await runTerminalCommand(id, proc.command, proc.cwd);
            if (!result.success) {
                setProcesses(prev => ({
                    ...prev,
                    [id]: {
                        ...prev[id],
                        status: 'error',
                        logs: [{
                            type: 'stderr',
                            content: `❌ Failed to start: ${result.error || 'Unknown error'}\n\nCommand: ${proc.command}\nDirectory: ${proc.cwd}`,
                            timestamp: Date.now()
                        }]
                    }
                }));
            }
        } catch (e) {
            setProcesses(prev => ({
                ...prev,
                [id]: {
                    ...prev[id],
                    status: 'error',
                    logs: [{
                        type: 'stderr',
                        content: `❌ Exception: ${e instanceof Error ? e.message : 'Unknown error'}`,
                        timestamp: Date.now()
                    }]
                }
            }));
        }
    };

    const handleStop = async (id: string) => {
        await stopTerminalCommand(id);
    };

    const handleClearLogs = (id: string) => {
        setProcesses(prev => ({ ...prev, [id]: { ...prev[id], logs: [] } }));
    };

    const handleCopyLogs = (id: string) => {
        const logs = processes[id]?.logs || [];
        const text = logs.map(log =>
            `[${new Date(log.timestamp).toLocaleTimeString([], { hour12: false })}] ${log.content}`
        ).join('');
        navigator.clipboard.writeText(text);
    };

    const handleDownloadLogs = (id: string) => {
        const logs = processes[id]?.logs || [];
        const text = logs.map(log =>
            `[${new Date(log.timestamp).toLocaleTimeString([], { hour12: false })}] ${log.content}`
        ).join('');
        const blob = new Blob([text], { type: 'text/plain' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `${id}-logs-${new Date().toISOString().slice(0, 10)}.txt`;
        a.click();
        URL.revokeObjectURL(url);
    };

    return (
        <div className="dashboard-container">
            <div className="dashboard-header">
                <div className="status-overview">
                    <Activity size={24} className="text-blue-500" />
                    <div className="flex flex-col">
                        <h2 className="text-2xl font-bold text-white tracking-tight">Project Dashboard</h2>
                        <span className="text-xs text-slate-400 font-mono">{projectPath}</span>
                    </div>
                </div>
                <div className="dashboard-header-actions">
                    <button className="btn-secondary-glass" onClick={() => Object.keys(processes).forEach(handleStop)}>
                        <Square size={16} fill="white" />
                        <span>Stop All</span>
                    </button>
                    <button className="btn-close-glass" onClick={onClose}>
                        <X size={20} />
                    </button>
                </div>
            </div>

            <div className="dashboard-content">
                {/* Process Grid */}
                <div className="process-grid">
                    {Object.values(processes).map((proc) => (
                        <div
                            key={proc.id}
                            className={`process-card ${activeTab === proc.id ? 'active' : ''}`}
                            onClick={() => setActiveTab(proc.id)}
                        >
                            <div className="process-card-header">
                                <div className={`process-icon-box status-${proc.status}`}>
                                    {proc.icon}
                                </div>
                                <div className="flex-1">
                                    <h4 className="font-semibold text-slate-100">{proc.name}</h4>
                                    <span className="text-[10px] text-slate-500 font-mono truncate block w-40">{proc.command}</span>
                                </div>
                                <div className={`status-badge status-${proc.status}`}>
                                    {proc.status.toUpperCase()}
                                </div>
                            </div>

                            <div className="process-card-actions">
                                {proc.status === 'running' ? (
                                    <button
                                        className="btn-proc-stop"
                                        onClick={(e) => { e.stopPropagation(); handleStop(proc.id); }}
                                    >
                                        <Square size={14} fill="currentColor" />
                                        <span>STOP</span>
                                    </button>
                                ) : (
                                    <button
                                        className="btn-proc-start"
                                        onClick={(e) => { e.stopPropagation(); handleStart(proc.id); }}
                                    >
                                        <Play size={14} fill="currentColor" />
                                        <span>RUN</span>
                                    </button>
                                )}
                            </div>
                        </div>
                    ))}

                    {frontends.includes('razor') && (
                        <div className="info-card">
                            <Server size={20} className="text-orange-400" />
                            <p className="text-xs text-slate-400">
                                <strong className="text-slate-200">Razor Pages:</strong> Executed as part of the Backend process. No separate runner needed.
                            </p>
                        </div>
                    )}
                </div>

                {/* Terminal Area */}
                <div className="terminal-dashboard">
                    <div className="terminal-header">
                        <div className="flex items-center gap-2">
                            <TerminalIcon size={16} className="text-emerald-400" />
                            <span className="text-sm font-semibold text-slate-200 uppercase tracking-widest">{processes[activeTab]?.name} Output</span>
                        </div>
                        <div className="flex items-center gap-2">
                            <button className="btn-clear-logs" onClick={() => handleCopyLogs(activeTab)} title="Copy to clipboard">
                                <Copy size={14} />
                            </button>
                            <button className="btn-clear-logs" onClick={() => handleDownloadLogs(activeTab)} title="Download logs">
                                <Download size={14} />
                            </button>
                            <button className="btn-clear-logs" onClick={() => handleClearLogs(activeTab)}>
                                Clear
                            </button>
                        </div>
                    </div>
                    <div className="terminal-body">
                        {processes[activeTab]?.logs.length === 0 ? (
                            <div className="terminal-placeholder">
                                <div className="placeholder-icon">
                                    <TerminalIcon size={48} />
                                </div>
                                <p>No logs available for {processes[activeTab]?.name}.</p>
                                <span className="text-xs text-slate-600">Click RUN to start streaming.</span>
                            </div>
                        ) : (
                            <div className="logs-container">
                                {processes[activeTab]?.logs.map((log, i) => (
                                    <div key={i} className={`log-line type-${log.type}`}>
                                        <span className="log-time">[{new Date(log.timestamp).toLocaleTimeString([], { hour12: false })}]</span>
                                        <span className="log-content">{log.content}</span>
                                    </div>
                                ))}
                                <div ref={logEndRef} />
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
};
