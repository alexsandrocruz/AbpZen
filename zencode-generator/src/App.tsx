import { useCallback, useMemo, useEffect } from 'react';
import ReactFlow, {
  Background,
  Controls,
  MiniMap,
  useNodesState,
  useEdgesState,
  addEdge,
  type Connection,
  type Edge,
  type Node,
} from 'reactflow';
import 'reactflow/dist/style.css';
import { Plus, Download, FileJson, Upload } from 'lucide-react';
import { useState } from 'react';

import EntityNode from './components/EntityNode';
import RelationEdge from './components/RelationEdge';
import Sidebar from './components/Sidebar';
import MetadataPreview from './components/MetadataPreview';
import ImportSqlModal from './components/ImportSqlModal';
import type { EntityData, RelationshipData, ZenMetadata } from './types';
import { transformToMetadata, downloadJson } from './utils/exportUtils';
import './App.css';

const nodeTypes = {
  entity: EntityNode,
};

const edgeTypes = {
  relation: RelationEdge,
};

const initialNodes: Node<EntityData>[] = [];
const initialEdges: Edge[] = [];

function App() {
  const [nodes, setNodes, onNodesChange] = useNodesState<EntityData>(initialNodes);
  const [edges, setEdges, onEdgesChange] = useEdgesState(initialEdges);
  const [showPreview, setShowPreview] = useState(false);
  const [showImportModal, setShowImportModal] = useState(false);
  const [generatedMetadata, setGeneratedMetadata] = useState<ZenMetadata | null>(null);

  const onConnect = useCallback(
    (params: Connection) => {
      const defaultData: RelationshipData = {
        type: 'one-to-many',
        sourceNavigationName: '',
        targetNavigationName: '',
        isRequired: false,
      };
      setEdges((eds) => addEdge({ ...params, data: defaultData, type: 'relation' }, eds));
    },
    [setEdges]
  );

  const updateEntity = useCallback((id: string, data: EntityData) => {
    setNodes((nds) =>
      nds.map((node) => (node.id === id ? { ...node, data } : node))
    );
  }, [setNodes]);

  const updateEdge = useCallback((id: string, data: RelationshipData) => {
    setEdges((eds) =>
      eds.map((edge) => (edge.id === id ? { ...edge, data } : edge))
    );
  }, [setEdges]);

  // Handle inline rename event from EntityNode
  useEffect(() => {
    const handleRename = (e: any) => {
      const { id, name } = e.detail;
      setNodes((nds) => nds.map((node) => {
        if (node.id === id) {
          return {
            ...node,
            data: {
              ...node.data,
              name,
              pluralName: `${name}s`,
              tableName: `${name}s`
            }
          };
        }
        return node;
      }));
    };

    window.addEventListener('entity-rename', handleRename);
    return () => window.removeEventListener('entity-rename', handleRename);
  }, [setNodes]);

  const addEntity = useCallback(() => {
    const id = `entity_${Date.now()}`;
    const name = `NewEntity${nodes.length + 1}`;
    const newNode: Node<EntityData> = {
      id,
      type: 'entity',
      data: {
        name,
        pluralName: `${name}s`,
        tableName: `${name}s`,
        namespace: 'ZenDoctor',
        baseClass: 'FullAuditedAggregateRoot',
        isMaster: true,
        fields: []
      },
      position: { x: Math.random() * 400, y: Math.random() * 400 },
    };
    setNodes((nds) => nds.concat(newNode));
  }, [nodes, setNodes]);

  const selectedNode = useMemo(
    () => nodes.find((node) => node.selected) || null,
    [nodes]
  );

  const selectedEdge = useMemo(() => {
    const edge = edges.find((e) => e.selected);
    if (!edge) return null;

    const sourceNode = nodes.find((n) => n.id === edge.source);
    const targetNode = nodes.find((n) => n.id === edge.target);

    return {
      id: edge.id,
      data: edge.data as RelationshipData,
      sourceName: sourceNode?.data.name,
      targetName: targetNode?.data.name,
    };
  }, [edges, nodes]);

  const clearSelection = useCallback(() => {
    setNodes((nds) => nds.map((n) => ({ ...n, selected: false })));
    setEdges((eds) => eds.map((e) => ({ ...e, selected: false })));
  }, [setNodes, setEdges]);

  const deleteEntity = useCallback((id: string) => {
    setNodes((nds) => nds.filter((node) => node.id !== id));
    // Also remove any edges connected to this entity
    setEdges((eds) => eds.filter((edge) => edge.source !== id && edge.target !== id));
  }, [setNodes, setEdges]);

  const deleteEdge = useCallback((id: string) => {
    setEdges((eds) => eds.filter((edge) => edge.id !== id));
  }, [setEdges]);

  const handleExport = useCallback(() => {
    const data = transformToMetadata(nodes, edges);
    downloadJson(data);
  }, [nodes, edges]);

  const handlePreview = useCallback(() => {
    const data = transformToMetadata(nodes, edges);
    setGeneratedMetadata(data);
    setShowPreview(true);
  }, [nodes, edges]);

  const handleImportSql = useCallback((newEntities: EntityData[]) => {
    const newNodes: Node<EntityData>[] = newEntities.map((entity, index) => {
      const id = `entity_${Date.now()}_${index}`;
      return {
        id,
        type: 'entity',
        data: entity,
        position: { x: 100 + index * 220, y: 100 + (index % 3) * 50 },
      };
    });
    setNodes((nds) => nds.concat(newNodes));
  }, [setNodes]);

  return (
    <div className="app-container">
      <div className="controls-panel">
        <button className="btn-primary" onClick={addEntity}>
          <Plus size={18} />
          Add Entity
        </button>
        <button className="btn-secondary" onClick={() => setShowImportModal(true)}>
          <Upload size={18} />
          Import SQL
        </button>
        <button className="btn-secondary" onClick={handlePreview}>
          <FileJson size={18} />
          Preview
        </button>
        <button className="btn-secondary" onClick={handleExport}>
          <Download size={18} />
          Export JSON
        </button>
      </div>

      <div className="canvas-container">
        <ReactFlow
          nodes={nodes}
          edges={edges}
          nodeTypes={nodeTypes}
          edgeTypes={edgeTypes}
          onNodesChange={onNodesChange}
          onEdgesChange={onEdgesChange}
          onConnect={onConnect}
          fitView
        >
          <Background color="#334155" gap={20} />
          <Controls />
          <MiniMap
            style={{ backgroundColor: '#0f172a' }}
            maskColor="rgba(15, 23, 42, 0.6)"
            nodeColor="#334155"
          />
        </ReactFlow>
      </div>

      <Sidebar
        selectedEntity={selectedNode}
        selectedEdge={selectedEdge}
        onUpdateEntity={updateEntity}
        onUpdateEdge={updateEdge}
        onDeleteEntity={deleteEntity}
        onDeleteEdge={deleteEdge}
        onClose={clearSelection}
      />

      {showPreview && generatedMetadata && (
        <MetadataPreview
          metadata={generatedMetadata}
          onClose={() => setShowPreview(false)}
        />
      )}

      {showImportModal && (
        <ImportSqlModal
          onImport={handleImportSql}
          onClose={() => setShowImportModal(false)}
        />
      )}
    </div>
  );
}

export default App;
