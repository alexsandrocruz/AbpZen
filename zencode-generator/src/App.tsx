import { useCallback, useMemo } from 'react';
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
import { Plus } from 'lucide-react';

import EntityNode from './components/EntityNode';
import Sidebar from './components/Sidebar';
import type { EntityData } from './types';
import './App.css';

const nodeTypes = {
  entity: EntityNode,
};

const initialNodes: Node<EntityData>[] = [];
const initialEdges: Edge[] = [];

function App() {
  const [nodes, setNodes, onNodesChange] = useNodesState<EntityData>(initialNodes);
  const [edges, setEdges, onEdgesChange] = useEdgesState(initialEdges);

  const onConnect = useCallback(
    (params: Connection) => setEdges((eds) => addEdge(params, eds)),
    [setEdges]
  );

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
        namespace: 'ZenDoctor', // Default namespace
        baseClass: 'FullAuditedAggregateRoot',
        isMaster: true,
        fields: []
      },
      position: { x: Math.random() * 400, y: Math.random() * 400 },
    };
    setNodes((nds) => nds.concat(newNode));
  }, [nodes, setNodes]);

  const updateEntity = useCallback((id: string, data: EntityData) => {
    setNodes((nds) =>
      nds.map((node) => (node.id === id ? { ...node, data } : node))
    );
  }, [setNodes]);

  const selectedNode = useMemo(
    () => nodes.find((node) => node.selected) || null,
    [nodes]
  );

  const clearSelection = useCallback(() => {
    setNodes((nds) => nds.map((n) => ({ ...n, selected: false })));
  }, [setNodes]);

  return (
    <div className="app-container">
      <div className="controls-panel">
        <button className="btn-primary" onClick={addEntity}>
          <Plus size={18} />
          Add Entity
        </button>
      </div>

      <div className="canvas-container">
        <ReactFlow
          nodes={nodes}
          edges={edges}
          nodeTypes={nodeTypes}
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
        selectedEntity={selectedNode as any}
        onUpdateEntity={updateEntity}
        onClose={clearSelection}
      />
    </div>
  );
}

export default App;
