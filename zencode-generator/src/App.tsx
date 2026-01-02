import { useCallback, useMemo, useEffect, useRef } from 'react';
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
import { Plus, Download, FileJson, Upload, Undo2, Redo2, Save, FolderOpen, Folder } from 'lucide-react';
import { useState } from 'react';

import EntityNode from './components/EntityNode';
import RelationEdge from './components/RelationEdge';
import Sidebar from './components/Sidebar';
import MetadataPreview from './components/MetadataPreview';
import ImportSqlModal from './components/ImportSqlModal';
import CrudPreview from './components/CrudPreview';
import GenerateCodeModal from './components/GenerateCodeModal';
import SettingsModal from './components/SettingsModal';
import { Settings } from 'lucide-react';
import type { EntityData, RelationshipData, ZenMetadata } from './types';
import { transformToMetadata, downloadJson } from './utils/exportUtils';
import { useHistory } from './hooks/useHistory';
import { createProjectFile, saveProjectToFile, loadProjectFromFile } from './utils/projectFile';
import { pluralize } from './utils/pluralize';
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
  const [showCrudPreview, setShowCrudPreview] = useState(false);
  const [showGenerateCode, setShowGenerateCode] = useState(false);
  const [previewEntityId, setPreviewEntityId] = useState<string | null>(null);
  const [generatedMetadata, setGeneratedMetadata] = useState<ZenMetadata | null>(null);
  const [projectName, setProjectName] = useState(() => localStorage.getItem('zen_project_name') || 'Untitled');
  const [projectNamespace, setProjectNamespace] = useState(() => localStorage.getItem('zen_project_namespace') || '');
  const [projectPath, setProjectPath] = useState(() => localStorage.getItem('zen_project_path') || '');
  const [showSettings, setShowSettings] = useState(false);

  // Undo/Redo history
  const { pushState, undo, redo, canUndo, canRedo } = useHistory<Node<EntityData>, Edge>(initialNodes, initialEdges);
  const fileInputRef = useRef<HTMLInputElement>(null);

  // Track changes for undo
  useEffect(() => {
    const timer = setTimeout(() => {
      pushState(nodes, edges);
    }, 500); // Debounce to avoid too many snapshots
    return () => clearTimeout(timer);
  }, [nodes, edges, pushState]);

  const onConnect = useCallback(
    (params: Connection) => {
      if (!params.source || !params.target) return;

      // Get source and target entity names
      const sourceNode = nodes.find(n => n.id === params.source);
      const targetNode = nodes.find(n => n.id === params.target);

      if (!sourceNode || !targetNode) return;

      const sourceEntityName = sourceNode.data.name;
      const targetEntityName = targetNode.data.name;

      // For now, default to one-to-many (user can change in sidebar)
      // We'll handle many-to-many case separately
      const fkFieldName = `${targetEntityName}Id`;

      // Check if FK field already exists
      const fieldExists = sourceNode.data.fields.some(
        f => f.name.toLowerCase() === fkFieldName.toLowerCase()
      );

      // Create the relationship edge
      const defaultData: RelationshipData = {
        type: 'one-to-many',
        sourceNavigationName: pluralize(sourceEntityName),
        targetNavigationName: targetEntityName,
        isRequired: false,
      };
      setEdges((eds) => addEdge({ ...params, data: defaultData, type: 'relation' }, eds));

      // Auto-add FK field to source entity if it doesn't exist
      if (!fieldExists) {
        const newFkField = {
          id: `field_${Date.now()}`,
          name: fkFieldName,
          type: 'guid' as const,
          isRequired: false,
          isNullable: true,
          isFilterable: true,
          isTextArea: false,
          isLookup: true,
          label: targetEntityName,
          lookupConfig: {
            mode: 'dropdown' as const,
            targetEntity: targetEntityName,
            displayField: 'name',
          },
        };

        setNodes((nds) => nds.map((node) => {
          if (node.id === params.source) {
            return {
              ...node,
              data: {
                ...node.data,
                fields: [...node.data.fields, newFkField],
              },
            };
          }
          return node;
        }));
      }
    },
    [setEdges, setNodes, nodes]
  );

  // Handler for changing relationship type (called from sidebar)
  const handleRelationshipTypeChange = useCallback((edgeId: string, newType: RelationshipData['type']) => {
    const edge = edges.find(e => e.id === edgeId);
    if (!edge || !edge.data) return;

    const sourceNode = nodes.find(n => n.id === edge.source);
    const targetNode = nodes.find(n => n.id === edge.target);
    if (!sourceNode || !targetNode) return;

    const sourceEntityName = sourceNode.data.name;
    const targetEntityName = targetNode.data.name;

    if (newType === 'many-to-many' && edge.data.type !== 'many-to-many') {
      // Generate junction table for N:N
      const junctionName = `${sourceEntityName}${targetEntityName}`;
      const junctionId = `entity_junction_${Date.now()}`;

      // Create the junction entity
      const junctionEntity: EntityData = {
        name: junctionName,
        pluralName: pluralize(junctionName),
        tableName: pluralize(junctionName),
        namespace: sourceNode.data.namespace,
        baseClass: 'FullAuditedAggregateRoot',
        isMaster: false,
        fields: [
          {
            id: `field_${Date.now()}_1`,
            name: `${sourceEntityName}Id`,
            type: 'guid',
            isRequired: true,
            isNullable: false,
            isFilterable: true,
            isTextArea: false,
            isLookup: true,
            label: sourceEntityName,
            lookupConfig: {
              mode: 'dropdown',
              targetEntity: sourceEntityName,
              displayField: 'name',
            },
          },
          {
            id: `field_${Date.now()}_2`,
            name: `${targetEntityName}Id`,
            type: 'guid',
            isRequired: true,
            isNullable: false,
            isFilterable: true,
            isTextArea: false,
            isLookup: true,
            label: targetEntityName,
            lookupConfig: {
              mode: 'dropdown',
              targetEntity: targetEntityName,
              displayField: 'name',
            },
          },
        ],
      };

      // Position the junction between source and target
      const junctionPosition = {
        x: (sourceNode.position.x + targetNode.position.x) / 2,
        y: (sourceNode.position.y + targetNode.position.y) / 2 + 100,
      };

      // Create the junction node
      const junctionNode: Node<EntityData> = {
        id: junctionId,
        type: 'entity',
        position: junctionPosition,
        data: junctionEntity,
      };

      // Add junction node
      setNodes((nds) => [...nds, junctionNode]);

      // Update the original edge with junction config
      setEdges((eds) => eds.map(e => {
        if (e.id === edgeId) {
          return {
            ...e,
            data: {
              ...e.data!,
              type: 'many-to-many' as const,
              junctionConfig: {
                tableName: junctionName,
                junctionEntityId: junctionId,
                showInSource: true,
                showInTarget: true,
              },
            },
          };
        }
        return e;
      }));

      // Create two 1:N edges from junction to each entity
      const edge1: Edge = {
        id: `edge_junction_${Date.now()}_1`,
        source: junctionId,
        target: edge.source,
        type: 'relation',
        data: {
          type: 'one-to-many',
          sourceNavigationName: pluralize(junctionName),
          targetNavigationName: sourceEntityName,
          isRequired: true,
          isChildGrid: true,
          childGridConfig: {
            title: pluralize(targetEntityName),
            allowAdd: true,
            allowRemove: true,
            allowEdit: false,
          },
        } as RelationshipData,
      };

      const edge2: Edge = {
        id: `edge_junction_${Date.now()}_2`,
        source: junctionId,
        target: edge.target,
        type: 'relation',
        data: {
          type: 'one-to-many',
          sourceNavigationName: pluralize(junctionName),
          targetNavigationName: targetEntityName,
          isRequired: true,
          isChildGrid: true,
          childGridConfig: {
            title: pluralize(sourceEntityName),
            allowAdd: true,
            allowRemove: true,
            allowEdit: false,
          },
        } as RelationshipData,
      };

      setEdges((eds) => [...eds, edge1, edge2]);

    } else {
      // Just update the type
      setEdges((eds) => eds.map(e => {
        if (e.id === edgeId) {
          return { ...e, data: { ...e.data!, type: newType } };
        }
        return e;
      }));
    }
  }, [edges, nodes, setNodes, setEdges]);

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
              pluralName: pluralize(name),
              tableName: pluralize(name),
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

  // Undo handler
  const handleUndo = useCallback(() => {
    const state = undo();
    if (state) {
      setNodes(state.nodes);
      setEdges(state.edges);
    }
  }, [undo, setNodes, setEdges]);

  // Redo handler
  const handleRedo = useCallback(() => {
    const state = redo();
    if (state) {
      setNodes(state.nodes);
      setEdges(state.edges);
    }
  }, [redo, setNodes, setEdges]);

  // Save project
  const handleSave = useCallback(() => {
    let name = projectName;
    if (name === 'Untitled' || !name) {
      const input = prompt('Digite o nome do projeto:', 'MeuProjeto');
      if (!input) return; // User cancelled
      name = input;
      setProjectName(name);
    }
    const project = createProjectFile(name, nodes, edges);
    saveProjectToFile(project);
  }, [projectName, nodes, edges]);

  // Load project
  const handleLoad = useCallback(async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;

    try {
      const project = await loadProjectFromFile(file);
      setNodes(project.nodes);
      setEdges(project.edges);
      setProjectName(project.name);
    } catch (error) {
      alert('Erro ao carregar projeto: ' + (error as Error).message);
    }

    // Reset input
    if (fileInputRef.current) {
      fileInputRef.current.value = '';
    }
  }, [setNodes, setEdges]);

  // Duplicate entity
  const duplicateEntity = useCallback((id: string) => {
    const node = nodes.find(n => n.id === id);
    if (!node) return;

    const newId = `entity_${Date.now()}`;
    const newName = `${node.data.name}Copy`;
    const newNode: Node<EntityData> = {
      id: newId,
      type: 'entity',
      data: {
        ...node.data,
        name: newName,
        pluralName: pluralize(newName),
        tableName: pluralize(newName),
        fields: node.data.fields.map(f => ({ ...f, id: `field_${Date.now()}_${Math.random().toString(36).substr(2, 5)}` }))
      },
      position: { x: node.position.x + 50, y: node.position.y + 50 },
    };
    setNodes((nds) => nds.concat(newNode));
  }, [nodes, setNodes]);

  return (
    <div className="app-container">
      <div className="controls-panel">
        <div className="controls-row">
          <button className="btn-icon-sm" onClick={handleUndo} disabled={!canUndo} title="Undo">
            <Undo2 size={18} />
          </button>
          <button className="btn-icon-sm" onClick={handleRedo} disabled={!canRedo} title="Redo">
            <Redo2 size={18} />
          </button>
          <div className="controls-divider" />
          <button className="btn-icon-sm" onClick={() => setShowSettings(true)} title="Settings">
            <Settings size={18} />
          </button>
        </div>
        <button className="btn-primary" onClick={addEntity}>
          <Plus size={18} />
          Add Entity
        </button>
        <button className="btn-secondary" onClick={() => setShowImportModal(true)}>
          <Upload size={18} />
          Import SQL
        </button>
        <button className="btn-secondary" onClick={handleSave}>
          <Save size={18} />
          Save Project
        </button>
        <button className="btn-secondary" onClick={() => fileInputRef.current?.click()}>
          <FolderOpen size={18} />
          Load Project
        </button>
        <input
          type="file"
          ref={fileInputRef}
          accept=".zen,.json"
          onChange={handleLoad}
          style={{ display: 'none' }}
        />
        <hr className="controls-divider" />
        <button className="btn-secondary" onClick={handlePreview}>
          <FileJson size={18} />
          Preview
        </button>
        <button className="btn-secondary" onClick={handleExport}>
          <Download size={18} />
          Export JSON
        </button>
        <button className="btn-primary" onClick={() => setShowGenerateCode(true)}>
          <Download size={18} />
          Generate Code
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
        allEntities={nodes.map(n => n.data)}
        onUpdateEntity={updateEntity}
        onUpdateEdge={updateEdge}
        onChangeRelationType={handleRelationshipTypeChange}
        onDeleteEntity={deleteEntity}
        onDeleteEdge={deleteEdge}
        onDuplicateEntity={duplicateEntity}
        onPreviewEntity={(id) => { setPreviewEntityId(id); setShowCrudPreview(true); }}
        onClose={clearSelection}
      />

      {
        showPreview && generatedMetadata && (
          <MetadataPreview
            metadata={generatedMetadata}
            onClose={() => setShowPreview(false)}
          />
        )
      }

      {
        showImportModal && (
          <ImportSqlModal
            onImport={handleImportSql}
            onClose={() => setShowImportModal(false)}
          />
        )
      }

      {showCrudPreview && previewEntityId && (() => {
        const previewNode = nodes.find(n => n.id === previewEntityId);
        if (!previewNode) return null;

        // Find child relations where this entity is the SOURCE (parent) and isChildGrid is true
        // In the relationship Proposta -> ItensProposta, Proposta is source and ItensProposta is target
        const childRelations = edges
          .filter(edge => edge.source === previewEntityId && edge.data?.isChildGrid)
          .map(edge => {
            const childNode = nodes.find(n => n.id === edge.target);
            if (!childNode) return null;
            return {
              childEntity: childNode.data,
              config: edge.data!,
            };
          })
          .filter(Boolean) as { childEntity: typeof previewNode.data; config: typeof edges[0]['data'] }[];

        return (
          <CrudPreview
            entity={previewNode.data}
            allEntities={nodes.map(n => n.data)}
            childRelations={childRelations}
            onClose={() => { setShowCrudPreview(false); setPreviewEntityId(null); }}
          />
        );
      })()}

      {showGenerateCode && (
        <GenerateCodeModal
          entities={nodes.map(n => n.data)}
          projectName={projectName || generatedMetadata?.projectName || 'ZenGenerated'}
          projectNamespace={projectNamespace || generatedMetadata?.namespace || 'ZenApp'}
          projectPath={projectPath}
          onClose={() => setShowGenerateCode(false)}
        />
      )}


      {showSettings && (
        <SettingsModal
          projectPath={projectPath}
          projectName={projectName}
          projectNamespace={projectNamespace}
          onSave={(config) => {
            setProjectPath(config.path);
            setProjectName(config.projectName);
            setProjectNamespace(config.namespace);
            localStorage.setItem('zen_project_path', config.path);
            localStorage.setItem('zen_project_name', config.projectName);
            localStorage.setItem('zen_project_namespace', config.namespace);
            setShowSettings(false);
          }}
          onClose={() => setShowSettings(false)}
        />
      )}

      {projectPath && (
        <div className="project-indicator">
          <Folder size={14} />
          <span>Modeling: {projectPath.split('/').pop()}</span>
        </div>
      )}
    </div>
  );
}

export default App;
