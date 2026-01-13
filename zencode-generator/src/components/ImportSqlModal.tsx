import { X, Upload, FileText } from 'lucide-react';
import { useState, useCallback } from 'react';
import { parseSqlToEntities } from '../utils/sqlParser';
import type { EntityData, RelationshipData } from '../types';
import type { Node, Edge } from 'reactflow';
import { pluralize } from '../utils/pluralize';

interface ImportSqlModalProps {
    onImport: (nodes: Node<EntityData>[], edges: Edge[]) => void;
    onClose: () => void;
}

export default function ImportSqlModal({ onImport, onClose }: ImportSqlModalProps) {
    const [sqlText, setSqlText] = useState('');
    const [isDragging, setIsDragging] = useState(false);

    const handleProcess = () => {
        if (!sqlText.trim()) return;

        const result = parseSqlToEntities(sqlText);
        const { entities, relationships } = result;

        if (entities.length > 0) {
            // 1. Create Nodes
            const newNodes: Node<EntityData>[] = entities.map((entity, index) => {
                const id = `entity_${Date.now()}_${index}`;
                return {
                    id,
                    type: 'entity',
                    data: entity,
                    position: { x: 100 + index * 220, y: 100 + (index % 3) * 50 },
                };
            });

            // 2. Create Map: TableName (plural) -> NodeID
            const tableToNodeId = new Map<string, string>();
            newNodes.forEach(node => {
                // Map both name and plural/table name to handle variations
                tableToNodeId.set(node.data.name.toLowerCase(), node.id);
                tableToNodeId.set(node.data.tableName.toLowerCase(), node.id);
            });

            // 3. Create Edges
            const newEdges: Edge[] = [];
            relationships.forEach((rel, index) => {
                // Resolve Source/Target Node IDs
                // Note: The parser returns table names from SQL. We compare loosely.
                const sourceId = tableToNodeId.get(rel.sourceTable.toLowerCase());
                const targetId = tableToNodeId.get(rel.targetTable.toLowerCase());

                if (sourceId && targetId) {
                    // Update: In SQL FK, Source Table holds the FK column.
                    // e.g. Order table has CustomerId.
                    // Source: Order, Target: Customer.
                    // Relation: Customer (1) -> Order (N).
                    // In ReactFlow logic here: Source usually means "One" side, Target "Many"?
                    // Check OnConnect: addEdge({ source, target })
                    // Typically Source -> Target is the direction of the arrow.
                    // If Source=Customer, Target=Order. Edge: 1:N.

                    // But SQL FK is detected on "Order".
                    // Order has FK to Customer.
                    // So Order is the "Many" side. Customer is "One".
                    // If we want Arrow from One -> Many (Customer -> Order),
                    // Then ReactFlow Source = Customer (targetId), ReactFlow Target = Order (sourceId).

                    const rfSourceId = targetId; // Reference Table (1)
                    const rfTargetId = sourceId; // Table with FK (N)

                    // Verify if edge already exists?

                    newEdges.push({
                        id: `edge_sql_${Date.now()}_${index}`,
                        source: rfSourceId,
                        target: rfTargetId,
                        type: 'relation',
                        data: {
                            type: 'one-to-many',
                            // If Source=Customer, Target=Order.
                            // Name on Source (Customer): "Orders" (plural of Order table)
                            // Name on Target (Order): "Customer" (singular of Customer table)
                            sourceNavigationName: pluralize(rel.sourceTable), // "Orders"
                            targetNavigationName: rel.targetTable,            // "Customer"
                            isRequired: !rel.isCascade // Approximation
                        } as RelationshipData
                    });
                }
            });

            onImport(newNodes, newEdges);
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
