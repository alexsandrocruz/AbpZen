import fs from 'fs';
import path from 'path';

/**
 * Converts ZenMetadata to React Flow nodes and edges for the ZenCode Generator UI
 */
function synchronizeMetadata() {
    const projectPath = '/Users/alexsandrocruz/Documents/dev/AbpZen/spz-meuprojeto/Lexus';
    const jsonPath = path.join(projectPath, 'zencode.json');

    if (!fs.existsSync(jsonPath)) {
        console.error('zencode.json not found');
        return;
    }

    const metadata = JSON.parse(fs.readFileSync(jsonPath, 'utf8'));

    // Create nodes
    const nodes = metadata.entities.map((ent, index) => ({
        id: ent.id,
        type: 'entity',
        position: {
            x: 100 + (index % 3) * 350,
            y: 100 + Math.floor(index / 3) * 450
        },
        data: ent.data
    }));

    // Create edges
    const edges = metadata.relationships.map((rel, index) => ({
        id: rel.id,
        source: rel.source,
        target: rel.target,
        type: 'relation',
        data: rel.data
    }));

    // Generate Javascript to run in console
    const script = `
        localStorage.setItem('zen_project_name', '${metadata.name}');
        localStorage.setItem('zen_project_namespace', '${metadata.namespace}');
        localStorage.setItem('zen_project_path', '${projectPath}');
        localStorage.setItem('zen_canvas_nodes', JSON.stringify(${JSON.stringify(nodes)}));
        localStorage.setItem('zen_canvas_edges', JSON.stringify(${JSON.stringify(edges)}));
        window.location.reload();
    `;

    console.log('--- RUN THIS IN BROWSER CONSOLE ---');
    console.log(script);
    console.log('-----------------------------------');

    return script;
}

synchronizeMetadata();
