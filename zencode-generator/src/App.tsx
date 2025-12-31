import { useCallback } from 'react';
import ReactFlow, {
  Background,
  Controls,
  MiniMap,
  useNodesState,
  useEdgesState,
  addEdge,
  type Connection,
  type Edge,
  type Node
} from 'reactflow';
import 'reactflow/dist/style.css';
import { Plus } from 'lucide-react';

import './App.css';

const initialNodes: Node[] = [];
const initialEdges: Edge[] = [];

function App() {
  const [nodes, setNodes, onNodesChange] = useNodesState(initialNodes);
  const [edges, setEdges, onEdgesChange] = useEdgesState(initialEdges);

  const onConnect = useCallback(
    (params: Connection) => setEdges((eds) => addEdge(params, eds)),
    [setEdges]
  );

  const addEntity = useCallback(() => {
    const id = `entity_${Date.now()}`;
    const newNode: Node = {
      id,
      data: { label: `New Entity ${nodes.length + 1}` },
      position: { x: Math.random() * 400, y: Math.random() * 400 },
      style: {
        background: '#1e293b',
        color: '#f8fafc',
        border: '1px solid #334155',
        borderRadius: '8px',
        padding: '10px',
        width: 150
      }
    };
    setNodes((nds) => nds.concat(newNode));
  }, [nodes, setNodes]);

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
    </div>
  );
}

export default App;
