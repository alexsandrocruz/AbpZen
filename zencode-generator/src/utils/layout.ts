import dagre from 'dagre';
import { Position, type Node, type Edge } from 'reactflow';
import type { EntityData } from '../types';

const dagreGraph = new dagre.graphlib.Graph();
dagreGraph.setDefaultEdgeLabel(() => ({}));

const nodeWidth = 220;
const nodeHeight = 200; // Approximate height of entity card

export const getLayoutedElements = (nodes: Node[], edges: Edge[], direction = 'TB') => {
    const isHorizontal = direction === 'LR';
    dagreGraph.setGraph({ rankdir: direction });

    nodes.forEach((node) => {
        // Only layout visible nodes that are not children (or handle children logic later)
        // For now, simpler approach: layout top-level nodes
        dagreGraph.setNode(node.id, { width: nodeWidth, height: nodeHeight });
    });

    edges.forEach((edge) => {
        dagreGraph.setEdge(edge.source, edge.target);
    });

    dagre.layout(dagreGraph);

    const layoutedNodes = nodes.map((node) => {
        const nodeWithPosition = dagreGraph.node(node.id);

        // Safety check if node was included in layout
        if (!nodeWithPosition) return node;

        // Shift position to be centered (dagre returns center point)
        return {
            ...node,
            targetPosition: isHorizontal ? Position.Left : Position.Top,
            sourcePosition: isHorizontal ? Position.Right : Position.Bottom,
            position: {
                x: nodeWithPosition.x - nodeWidth / 2,
                y: nodeWithPosition.y - nodeHeight / 2,
            },
        };
    });

    return { nodes: layoutedNodes, edges };
};

/**
 * Heuristic to group nodes based on common prefixes in their names.
 * e.g., "AbpUsers", "AbpRoles" -> Group "Abp"
 */
export const getGroupedElements = (nodes: Node<EntityData>[], edges: Edge[]) => {
    const groupedNodes: Node[] = [];
    const groupsMap = new Map<string, Node[]>();
    const processedNodeIds = new Set<string>();

    const minGroupSize = 2; // Min entities to form a group

    // 1. Identify potential groups by prefix
    // Simple algorithm: Take the first word (CamelCase or underscore)
    // "AppUser" -> "App"
    // "Finance_Invoice" -> "Finance"

    // Sort nodes by name to make prefix logic stable
    const sortedNodes = [...nodes].sort((a, b) => a.data.name.localeCompare(b.data.name));

    sortedNodes.forEach(node => {
        if (node.parentNode) return; // Don't regroup already grouped

        const name = node.data.name;

        // Try underscore prefix first
        let prefix = "";
        if (name.includes('_')) {
            prefix = name.split('_')[0];
        } else {
            // Try CamelCase prefix (e.g. "AbpUser" -> "Abp")
            // Regex to find first UpperCase word
            const match = name.match(/^([A-Z][a-z0-9]+)/);
            if (match) {
                prefix = match[1];
            }
        }

        if (prefix && prefix.length >= 2 && prefix !== name) {
            if (!groupsMap.has(prefix)) {
                groupsMap.set(prefix, []);
            }
            groupsMap.get(prefix)!.push(node);
        } else {
            // No clear prefix, leave as standalone (or 'Common' group?)
            groupedNodes.push(node);
            processedNodeIds.add(node.id);
        }
    });

    // 2. Create Group Nodes and Assign Children
    groupsMap.forEach((groupNodesList, prefix) => {
        if (groupNodesList.length >= minGroupSize) {
            const groupId = `group_${prefix}`;

            // Create the Parent Group Node
            const groupNode: Node = {
                id: groupId,
                type: 'group', // React Flow default group type is implied if we dont have custom
                // Wait, we need a custom group node or use 'group' in default types?
                // Default 'group' is not built-in, usually we use { style: { ... }, type: 'group' } doesn't exist by default in recent versions?
                // Actually 'group' is just a node without handles usually, or simply a container.
                // Best practice: Use a node with `data: { label: prefix }` and `style`.
                data: { label: prefix },
                position: { x: 0, y: 0 }, // Will be calculated by layout
                style: {
                    backgroundColor: 'rgba(255, 255, 255, 0.05)',
                    border: '1px dashed rgba(255, 255, 255, 0.2)',
                    width: 100, // Dynamic
                    height: 100,
                    borderRadius: 8,
                    color: '#fff',
                    padding: 10
                },
                zIndex: -1
            };

            groupedNodes.push(groupNode);

            // Assign children
            groupNodesList.forEach(child => {
                groupedNodes.push({
                    ...child,
                    parentNode: groupId,
                    extent: 'parent', // Optional: confine to parent
                    position: { x: 0, y: 0 } // Relative to parent! Needed to be recalculated.
                });
                processedNodeIds.add(child.id);
            });
        } else {
            // Not enough items, release them back
            groupNodesList.forEach(node => {
                groupedNodes.push(node);
                processedNodeIds.add(node.id);
            });
        }
    });

    // Add any missing nodes (fallback)
    nodes.forEach(n => {
        if (!processedNodeIds.has(n.id)) {
            groupedNodes.push(n);
        }
    });

    // Note: Group layouting is complex. 
    // Simply creating groups without positions might stack children at 0,0 inside group.
    // We would need a "Layout Inside Group" pass.

    return { nodes: groupedNodes, edges };
};

/**
 * Radial Layout: Places the most connected node in center and others in a circle/spiral
 */
export const getRadialLayout = (nodes: Node[], edges: Edge[]) => {
    if (nodes.length === 0) return { nodes, edges };

    // 1. Calculate degrees
    const degrees = new Map<string, number>();
    nodes.forEach(n => degrees.set(n.id, 0));

    edges.forEach(e => {
        degrees.set(e.source, (degrees.get(e.source) || 0) + 1);
        degrees.set(e.target, (degrees.get(e.target) || 0) + 1);
    });

    // 2. Find center node (highest degree)
    let centerNodeId = nodes[0].id;
    let maxDegree = -1;

    degrees.forEach((deg, id) => {
        if (deg > maxDegree) {
            maxDegree = deg;
            centerNodeId = id;
        }
    });

    // 3. Arrange others in a circle
    const otherNodes = nodes.filter(n => n.id !== centerNodeId);

    // Calculate radius based on number of nodes to avoid overlap
    // Circumference C = N * NodeWidth. C = 2 * PI * R => R = C / (2 * PI)
    const padding = 50;
    const minDistance = Math.max(nodeWidth, nodeHeight) + padding;
    const circumference = otherNodes.length * minDistance;
    const radius = Math.max(circumference / (2 * Math.PI), minDistance * 2);

    const layoutedNodes = nodes.map(node => {
        if (node.id === centerNodeId) {
            return {
                ...node,
                position: { x: 0, y: 0 }
            };
        }

        // Find index in "otherNodes" to determine angle
        const index = otherNodes.findIndex(n => n.id === node.id);
        const angle = (2 * Math.PI * index) / otherNodes.length;

        return {
            ...node,
            position: {
                x: radius * Math.cos(angle),
                y: radius * Math.sin(angle)
            }
        };
    });

    return { nodes: layoutedNodes, edges };
};
