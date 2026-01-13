import express from 'express';
import cors from 'cors';
import bodyParser from 'body-parser';
import fs from 'fs';
import path from 'path';
import os from 'os';
import { exec, spawn } from 'child_process';

// Local modules will be imported dynamically to prevent startup crashes
// import { injectCode } from './injector.js';
// import { importDatabase } from './db-importer.js';

const app = express();
const port = process.env.PORT || 3005;

// === DEBUGGING STARTUP ===
console.log('[Bridge] Starting up...');
console.log(`[Bridge] Environment PORT: ${process.env.PORT}`);
console.log(`[Bridge] Resolved Port: ${port}`);
console.log(`[Bridge] Current Working Directory: ${process.cwd()}`);

try {
    const rootFiles = fs.readdirSync(process.cwd());
    console.log('[Bridge] Files in CWD:', rootFiles.join(', '));
} catch (e) {
    console.error('[Bridge] Failed to list CWD:', e.message);
}

// Global Error Handlers
process.on('uncaughtException', (err) => {
    console.error('[Bridge] CRITICAL: Uncaught Exception:', err);
    // process.exit(1); // Don't exit immediately to allow logs to flush? Actually, better to exit so Docker restarts it.
});

process.on('unhandledRejection', (reason, promise) => {
    console.error('[Bridge] CRITICAL: Unhandled Rejection at:', promise, 'reason:', reason);
});
// =========================

app.use(cors());
app.use(bodyParser.json({ limit: '50mb' }));

// Serve static files from 'dist' (Vite build) if available
const distPath = path.join(process.cwd(), 'dist');
if (fs.existsSync(distPath)) {
    console.log(`[Bridge] Serving static files from ${distPath}`);
    app.use(express.static(distPath));
}

// Global Constants
const SKIP_FOLDERS = new Set([
    'node_modules', '.git', 'bin', 'obj', '.vs', '.idea',
    '.next', 'dist', 'build', 'packages', '.nuget', 'TestResults'
]);

const BINARY_EXTENSIONS = new Set([
    '.exe', '.dll', '.pdb', '.cache', '.nupkg', '.zip',
    '.png', '.jpg', '.jpeg', '.gif', '.ico', '.woff', '.woff2', '.ttf', '.eot'
]);

app.post('/api/pick-directory', (req, res) => {
    // macOS only for now using osascript
    const appleScript = 'POSIX path of (choose folder with prompt "Select ABP Project Root")';
    exec(`osascript -e '${appleScript}'`, (error, stdout, stderr) => {
        if (error) {
            // Error code 1 usually means user canceled
            if (error.code === 1) {
                return res.json({ canceled: true });
            }
            return res.status(500).json({ error: stderr || error.message });
        }
        const selectedPath = stdout.trim();
        res.json({ path: selectedPath });
    });
});

app.post('/api/import-db', async (req, res) => {
    const config = req.body;
    console.log(`[Bridge] Importing from DB: ${config.provider} @ ${config.host}`);

    try {
        const { importDatabase } = await import('./db-importer.js');
        const result = await importDatabase(config);
        res.json({ success: true, ...result });
    } catch (error) {
        console.error(`[Bridge] Import DB Error: ${error.message}`);
        res.status(500).json({ error: error.message });
    }
});

app.post('/api/detect-project-info', (req, res) => {
    const { projectPath } = req.body;
    if (!projectPath) {
        return res.status(400).json({ error: 'Missing projectPath' });
    }

    try {
        // Find all .csproj files in the project
        const findCsproj = (dir, depth = 0) => {
            if (depth > 3) return []; // Limit depth to avoid scanning too deep
            const items = fs.readdirSync(dir, { withFileTypes: true });
            let csprojFiles = [];
            for (const item of items) {
                if (item.isFile() && item.name.endsWith('.csproj')) {
                    csprojFiles.push(path.join(dir, item.name));
                } else if (item.isDirectory() && !item.name.startsWith('.') && item.name !== 'node_modules') {
                    csprojFiles = csprojFiles.concat(findCsproj(path.join(dir, item.name), depth + 1));
                }
            }
            return csprojFiles;
        };

        const csprojFiles = findCsproj(projectPath);

        // Look for the Domain project (usually contains the base namespace)
        let projectName = '';
        let namespace = '';

        // Try to find .sln file for project name
        const slnFiles = fs.readdirSync(projectPath).filter(f => f.endsWith('.sln'));
        if (slnFiles.length > 0) {
            projectName = slnFiles[0].replace('.sln', '');
        }

        // Look for Domain.csproj to get namespace
        const domainCsproj = csprojFiles.find(f => f.includes('.Domain') && !f.includes('.Shared'));
        if (domainCsproj) {
            const content = fs.readFileSync(domainCsproj, 'utf-8');
            // Try to extract RootNamespace
            const rootNsMatch = content.match(/<RootNamespace>([^<]+)<\/RootNamespace>/);
            if (rootNsMatch) {
                namespace = rootNsMatch[1];
            } else {
                // Use csproj filename as fallback
                namespace = path.basename(domainCsproj, '.csproj').replace('.Domain', '');
            }

            if (!projectName) {
                projectName = namespace;
            }
        } else if (csprojFiles.length > 0) {
            // Fallback: use first csproj name
            const firstCsproj = path.basename(csprojFiles[0], '.csproj');
            namespace = firstCsproj.split('.')[0];
            projectName = projectName || namespace;
        }

        res.json({ projectName, namespace, csprojCount: csprojFiles.length });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

app.post('/api/save-metadata', (req, res) => {
    const { projectPath, metadata } = req.body;
    if (!projectPath || !metadata) {
        return res.status(400).json({ error: 'Missing projectPath or metadata' });
    }

    try {
        const zenDir = path.join(projectPath, '.zen');
        if (!fs.existsSync(zenDir)) {
            fs.mkdirSync(zenDir, { recursive: true });
        }
        fs.writeFileSync(path.join(zenDir, 'model.json'), JSON.stringify(metadata, null, 2));
        res.json({ success: true });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

app.post('/api/generate-code', (req, res) => {
    const { projectPath, files } = req.body;
    console.log(`[Bridge] Received code generation request for ${files?.length} files in ${projectPath}`);
    if (!projectPath || !files) {
        return res.status(400).json({ error: 'Missing projectPath or files' });
    }

    try {
        for (const file of files) {
            const fullPath = path.join(projectPath, file.path);
            const dir = path.dirname(fullPath);
            console.log(`[Bridge] Writing file: ${file.path}`);
            if (!fs.existsSync(dir)) {
                fs.mkdirSync(dir, { recursive: true });
            }
            fs.writeFileSync(fullPath, file.content);
        }
        console.log(`[Bridge] Successfully wrote ${files.length} files`);
        res.json({ success: true, count: files.length });
    } catch (error) {
        console.error(`[Bridge] Error writing files: ${error.message}`);
        res.status(500).json({ error: error.message });
    }
});

app.post('/api/list-dirs', (req, res) => {
    let { directory } = req.body;

    if (!directory) {
        directory = os.homedir();
    }

    try {
        if (!fs.existsSync(directory)) {
            return res.status(404).json({ error: 'Directory not found' });
        }

        const items = fs.readdirSync(directory, { withFileTypes: true });
        const dirs = items
            .filter(item => item.isDirectory())
            .map(item => item.name)
            .sort();

        res.json({
            currentPath: directory,
            parentPath: path.dirname(directory),
            dirs
        });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

app.post('/api/inject-code', async (req, res) => {
    const { projectPath, instructions } = req.body;
    console.log(`[Bridge] Injecting/Merging for ${instructions.length} instructions in ${projectPath}`);
    if (!projectPath || !instructions) {
        return res.status(400).json({ error: 'Missing projectPath or instructions' });
    }

    try {
        const { injectCode } = await import('./injector.js');
        const results = injectCode(projectPath, instructions);
        res.json({ success: true, results });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

// Get boilerplate files for ZIP download
app.post('/api/get-boilerplate', (req, res) => {
    const { projectName, frontends, templatePath } = req.body;
    console.log(`[Bridge] Getting boilerplate files for: ${projectName}`);

    if (!projectName) {
        return res.status(400).json({ error: 'Missing projectName' });
    }

    try {
        const baseTemplatePath = templatePath || path.join(process.cwd(), '..', 'zencode-template');
        const files = [];

        // Recursive function to collect files
        const collectFiles = (dir, baseDir, prefix = '') => {
            if (!fs.existsSync(dir)) return;

            const entries = fs.readdirSync(dir, { withFileTypes: true });
            for (const entry of entries) {
                if (entry.isDirectory() && SKIP_FOLDERS.has(entry.name)) continue;
                if (entry.isSymbolicLink()) continue;

                const fullPath = path.join(dir, entry.name);
                const relativePath = path.join(prefix, entry.name);

                if (entry.isDirectory()) {
                    collectFiles(fullPath, baseDir, relativePath);
                } else {
                    const ext = path.extname(entry.name).toLowerCase();
                    if (BINARY_EXTENSIONS.has(ext)) continue;

                    try {
                        const content = fs.readFileSync(fullPath, 'utf-8');
                        files.push({ path: relativePath, content });
                    } catch (err) {
                        // Skip files that can't be read as text
                        console.log(`[Bridge] Skipping binary file: ${relativePath}`);
                    }
                }
            }
        };

        // Collect backend files
        if (fs.existsSync(baseTemplatePath)) {
            const items = fs.readdirSync(baseTemplatePath, { withFileTypes: true });
            for (const item of items) {
                if (item.isDirectory() && item.name.startsWith('Sapienza.Zen')) {
                    collectFiles(
                        path.join(baseTemplatePath, item.name),
                        baseTemplatePath,
                        `backend/${item.name}`
                    );
                }
            }
        }

        // Collect frontend files based on selection
        if (frontends && Array.isArray(frontends)) {
            for (const frontend of frontends) {
                let srcFolder = '';
                let destPrefix = '';

                switch (frontend) {
                    case 'react-v2':
                        srcFolder = path.join(baseTemplatePath, 'abp-react-v2');
                        destPrefix = 'abp-react-v2';
                        break;
                    case 'angular':
                        srcFolder = path.join(baseTemplatePath, 'angular');
                        destPrefix = 'angular';
                        break;
                    case 'razor':
                        continue; // Part of backend
                }

                if (srcFolder && fs.existsSync(srcFolder)) {
                    collectFiles(srcFolder, srcFolder, destPrefix);
                }
            }
        }

        console.log(`[Bridge] Collected ${files.length} files for ZIP`);
        res.json({ success: true, files, count: files.length });
    } catch (error) {
        console.error(`[Bridge] Error getting boilerplate: ${error.message}`);
        res.status(500).json({ error: error.message });
    }
});

// Create a new project by copying boilerplates
app.post('/api/create-project', (req, res) => {
    const { projectName, destinationPath, frontends, templatePath } = req.body;
    console.log(`[Bridge] Creating project: ${projectName} at ${destinationPath}`);

    if (!projectName || !destinationPath) {
        return res.status(400).json({ error: 'Missing projectName or destinationPath' });
    }

    try {
        // Full path for the new project
        const projectPath = path.join(destinationPath, projectName);

        // Create project directory
        if (fs.existsSync(projectPath)) {
            return res.status(400).json({ error: 'Project directory already exists' });
        }
        fs.mkdirSync(projectPath, { recursive: true });

        // [Production Fix] In container, zencode-template is at /zencode-template
        const defaultTemplate = fs.existsSync('/zencode-template')
            ? '/zencode-template'
            : path.join(process.cwd(), '..', 'zencode-template');

        const baseTemplatePath = templatePath || defaultTemplate;
        const copiedItems = [];

        // Create zencode.json manifest
        const manifest = {
            version: '1.0',
            name: projectName,
            namespace: `Sapienza.${projectName}`,
            createdAt: new Date().toISOString(),
            frontend: (frontends && frontends.length > 0) ? frontends[0] : 'react-v2',
            entities: []
        };

        // Helper to copy directory recursively, renaming items and replacing content
        const copyAndTransform = (src, dest, replacements) => {
            if (!fs.existsSync(src)) return false;

            const entries = fs.readdirSync(src, { withFileTypes: true });
            if (!fs.existsSync(dest)) {
                fs.mkdirSync(dest, { recursive: true });
            }

            for (const entry of entries) {
                // Skip excluded folders
                if (entry.isDirectory() && SKIP_FOLDERS.has(entry.name)) continue;
                if (entry.isSymbolicLink()) continue;

                // Rename the entry name
                let targetName = entry.name;
                for (const r of replacements) {
                    targetName = targetName.split(r.search).join(r.replace);
                }

                const srcPath = path.join(src, entry.name);
                const destPath = path.join(dest, targetName);

                if (entry.isDirectory()) {
                    copyAndTransform(srcPath, destPath, replacements);
                } else {
                    const ext = path.extname(entry.name).toLowerCase();
                    if (BINARY_EXTENSIONS.has(ext)) {
                        fs.copyFileSync(srcPath, destPath);
                    } else {
                        try {
                            let content = fs.readFileSync(srcPath, 'utf8');
                            for (const r of replacements) {
                                content = content.split(r.search).join(r.replace);
                            }
                            fs.writeFileSync(destPath, content);
                        } catch (err) {
                            console.log(`[Bridge] Warning: Could not transform ${entry.name}: ${err.message}`);
                            fs.copyFileSync(srcPath, destPath);
                        }
                    }
                }
            }
            return true;
        };

        const replacements = [
            { search: 'Sapienza.Zen', replace: manifest.namespace },
            { search: 'SapienzaZen', replace: projectName }
        ];

        // Copy everything from template root to project root, transforming filenames and content
        if (fs.existsSync(baseTemplatePath)) {
            const items = fs.readdirSync(baseTemplatePath, { withFileTypes: true });
            for (const item of items) {
                if (SKIP_FOLDERS.has(item.name)) continue;
                if (item.name === 'abp-react-v2' || item.name === 'angular') continue; // Handled below

                const src = path.join(baseTemplatePath, item.name);

                // For files/folders starting with Sapienza.Zen, they go to root
                let targetName = item.name;
                for (const r of replacements) {
                    targetName = targetName.split(r.search).join(r.replace);
                }
                const dest = path.join(projectPath, targetName);

                if (item.isDirectory()) {
                    copyAndTransform(src, dest, replacements);
                    copiedItems.push(targetName);
                } else {
                    // Files at root
                    const ext = path.extname(item.name).toLowerCase();
                    if (BINARY_EXTENSIONS.has(ext)) {
                        fs.copyFileSync(src, dest);
                    } else {
                        let content = fs.readFileSync(src, 'utf8');
                        for (const r of replacements) {
                            content = content.split(r.search).join(r.replace);
                        }
                        fs.writeFileSync(dest, content);
                    }
                    copiedItems.push(targetName);
                }
            }
        }

        // Copy frontend templates based on selection
        if (frontends && Array.isArray(frontends)) {
            for (const frontend of frontends) {
                let srcFolder = '';
                let destFolder = '';

                switch (frontend) {
                    case 'react-v2':
                        srcFolder = path.join(baseTemplatePath, 'abp-react-v2');
                        destFolder = path.join(projectPath, 'abp-react-v2');
                        break;
                    case 'angular':
                        srcFolder = path.join(baseTemplatePath, 'angular');
                        destFolder = path.join(projectPath, 'angular');
                        break;
                }

                if (srcFolder && copyAndTransform(srcFolder, destFolder, replacements)) {
                    copiedItems.push(frontend);
                }
            }
        }

        fs.writeFileSync(
            path.join(projectPath, 'zencode.json'),
            JSON.stringify(manifest, null, 2)
        );
        copiedItems.push('zencode.json');

        console.log(`[Bridge] Project created successfully: ${copiedItems.length} items`);
        res.json({
            success: true,
            projectPath,
            copiedItems,
            manifest
        });
    } catch (error) {
        console.error(`[Bridge] Error creating project: ${error.message}`);
        res.status(500).json({ error: error.message });
    }
});

// Scaffold a complete project from a .zen file
// This combines create-project with entity info for frontend to generate
app.post('/api/scaffold-from-zen', (req, res) => {
    const { zenFilePath, destinationPath, projectName: overrideName, frontends = ['react-v2'] } = req.body;
    console.log(`[Bridge] Scaffolding project from .zen file: ${zenFilePath}`);

    if (!zenFilePath || !destinationPath) {
        return res.status(400).json({ error: 'Missing zenFilePath or destinationPath' });
    }

    try {
        // 1. Read and parse the .zen file
        if (!fs.existsSync(zenFilePath)) {
            return res.status(404).json({ error: `Zen file not found: ${zenFilePath}` });
        }

        const zenContent = fs.readFileSync(zenFilePath, 'utf8');
        let zenData;
        try {
            zenData = JSON.parse(zenContent);
        } catch (parseErr) {
            return res.status(400).json({ error: `Invalid JSON in .zen file: ${parseErr.message}` });
        }

        // Extract project info from .zen file
        const projectName = overrideName || zenData.name || zenData.config?.projectName || 'NewProject';
        const namespace = zenData.config?.namespace || `Sapienza.${projectName}`;
        const entities = zenData.nodes?.filter(n => n.type === 'entity') || [];
        const relationships = zenData.edges?.filter(e => e.type === 'relation') || [];

        console.log(`[Bridge] Project: ${projectName}, Namespace: ${namespace}, Entities: ${entities.length}`);

        // 2. Create project directory
        const projectPath = path.join(destinationPath, projectName);
        if (fs.existsSync(projectPath)) {
            return res.status(400).json({ error: 'Project directory already exists' });
        }
        fs.mkdirSync(projectPath, { recursive: true });

        const baseTemplatePath = path.join(process.cwd(), '..', 'zencode-template');
        if (!fs.existsSync(baseTemplatePath)) {
            return res.status(500).json({ error: `Template not found at: ${baseTemplatePath}` });
        }

        const copiedItems = [];

        // 3. Prepare replacements
        const replacements = [
            { search: 'Sapienza.Zen', replace: namespace },
            { search: 'SapienzaZen', replace: projectName }
        ];

        // Helper to copy and transform (reuse from create-project)
        const copyAndTransform = (src, dest) => {
            if (!fs.existsSync(src)) return false;

            const entries = fs.readdirSync(src, { withFileTypes: true });
            if (!fs.existsSync(dest)) {
                fs.mkdirSync(dest, { recursive: true });
            }

            for (const entry of entries) {
                if (entry.isDirectory() && SKIP_FOLDERS.has(entry.name)) continue;
                if (entry.isSymbolicLink()) continue;

                let targetName = entry.name;
                for (const r of replacements) {
                    targetName = targetName.split(r.search).join(r.replace);
                }

                const srcPath = path.join(src, entry.name);
                const destPath = path.join(dest, targetName);

                if (entry.isDirectory()) {
                    copyAndTransform(srcPath, destPath);
                } else {
                    const ext = path.extname(entry.name).toLowerCase();
                    if (BINARY_EXTENSIONS.has(ext)) {
                        fs.copyFileSync(srcPath, destPath);
                    } else {
                        try {
                            let content = fs.readFileSync(srcPath, 'utf8');
                            for (const r of replacements) {
                                content = content.split(r.search).join(r.replace);
                            }
                            fs.writeFileSync(destPath, content);
                        } catch (err) {
                            fs.copyFileSync(srcPath, destPath);
                        }
                    }
                }
            }
            return true;
        };

        // 4. Copy backend template files
        const items = fs.readdirSync(baseTemplatePath, { withFileTypes: true });
        for (const item of items) {
            if (SKIP_FOLDERS.has(item.name)) continue;
            if (item.name === 'abp-react-v2' || item.name === 'angular') continue;

            const src = path.join(baseTemplatePath, item.name);
            let targetName = item.name;
            for (const r of replacements) {
                targetName = targetName.split(r.search).join(r.replace);
            }
            const dest = path.join(projectPath, targetName);

            if (item.isDirectory()) {
                copyAndTransform(src, dest);
                copiedItems.push(targetName);
            } else {
                const ext = path.extname(item.name).toLowerCase();
                if (BINARY_EXTENSIONS.has(ext)) {
                    fs.copyFileSync(src, dest);
                } else {
                    let content = fs.readFileSync(src, 'utf8');
                    for (const r of replacements) {
                        content = content.split(r.search).join(r.replace);
                    }
                    fs.writeFileSync(dest, content);
                }
                copiedItems.push(targetName);
            }
        }

        // 5. Copy frontend templates
        for (const frontend of frontends) {
            let srcFolder = '';
            let destFolder = '';

            switch (frontend) {
                case 'react-v2':
                    srcFolder = path.join(baseTemplatePath, 'abp-react-v2');
                    destFolder = path.join(projectPath, 'abp-react-v2');
                    break;
                case 'angular':
                    srcFolder = path.join(baseTemplatePath, 'angular');
                    destFolder = path.join(projectPath, 'angular');
                    break;
            }

            if (srcFolder && copyAndTransform(srcFolder, destFolder)) {
                copiedItems.push(frontend);
            }
        }

        // 6. Create updated zencode.json in new project
        const manifest = {
            ...zenData,
            config: {
                ...zenData.config,
                projectName,
                namespace,
                projectPath,
                frontends
            }
        };
        fs.writeFileSync(path.join(projectPath, 'zencode.json'), JSON.stringify(manifest, null, 2));
        copiedItems.push('zencode.json');

        // 7. Also save the .zen file reference
        const zenFileName = path.basename(zenFilePath);
        fs.copyFileSync(zenFilePath, path.join(path.dirname(projectPath), zenFileName));

        console.log(`[Bridge] Project scaffolded successfully: ${copiedItems.length} items, ${entities.length} entities to generate`);

        res.json({
            success: true,
            projectPath,
            projectName,
            namespace,
            copiedItems,
            // Return entity data for frontend to generate code
            entities: entities.map(e => e.data),
            relationships: relationships.map(r => ({ id: r.id, source: r.source, target: r.target, data: r.data })),
            frontends,
            message: `Project scaffolded. Use /api/generate-code to generate entity files.`
        });
    } catch (error) {
        console.error(`[Bridge] Error scaffolding project: ${error.message}`);
        res.status(500).json({ error: error.message });
    }
});

// --- Terminal & Process Management ---

const activeTerminals = new Map();

app.post('/api/terminal/run', (req, res) => {
    let { id, command, cwd } = req.body;

    // Sanitize inputs
    if (id) id = id.trim();
    if (command) command = command.trim();
    if (cwd) cwd = cwd.trim();

    console.log(`[Bridge] Starting terminal: ${id} -> ${command} in ${cwd}`);
    console.log(`[Bridge] Current PATH: ${process.env.PATH}`);

    if (!id || !command || !cwd) {
        return res.status(400).json({
            error: 'Missing parameters',
            details: { id: !!id, command: !!command, cwd: !!cwd }
        });
    }

    if (activeTerminals.has(id)) {
        console.log(`[Bridge] Terminal ${id} already exists. Stopping and removing it...`);
        const existing = activeTerminals.get(id);
        if (existing.process) {
            existing.process.kill('SIGKILL');
        }
        activeTerminals.delete(id);
    }

    if (!fs.existsSync(cwd)) {
        console.error(`[Bridge] Directory NOT FOUND: "${cwd}"`);
        const parent = path.dirname(cwd);
        let siblings = [];
        if (fs.existsSync(parent)) {
            siblings = fs.readdirSync(parent);
        }
        return res.status(400).json({
            error: `Directory not found: "${cwd}"`,
            parentExists: fs.existsSync(parent),
            availableInParent: siblings
        });
    }

    try {
        console.log(`[Bridge] Spawning: ${command} in ${cwd} (shell: /bin/zsh)`);
        const child = spawn(command, [], {
            cwd,
            shell: '/bin/zsh', // Explicitly use zsh as it's the user's default
            env: { ...process.env, FORCE_COLOR: 'true' },
            stdio: ['ignore', 'pipe', 'pipe'], // Ensure stdout/stderr are piped
            detached: false // Keep attached to parent
        });

        const terminal = {
            id,
            process: child,
            logs: [],
            status: 'running',
            exitCode: null
        };

        child.stdout.on('data', (data) => {
            terminal.logs.push({ type: 'stdout', content: data.toString(), timestamp: Date.now() });
            if (terminal.logs.length > 2000) terminal.logs.shift(); // Keep last 2000 lines
        });

        child.stderr.on('data', (data) => {
            terminal.logs.push({ type: 'stderr', content: data.toString(), timestamp: Date.now() });
            if (terminal.logs.length > 2000) terminal.logs.shift();
        });

        child.on('close', (code) => {
            console.log(`[Bridge] Terminal ${id} exited with code ${code}`);
            terminal.status = 'stopped';
            terminal.exitCode = code;
        });

        child.on('error', (err) => {
            console.error(`[Bridge] Terminal ${id} spawn error:`, err);
            terminal.status = 'error';
            terminal.logs.push({
                type: 'stderr',
                content: `Spawn Error: ${err.message}\nCommand: ${command}\nCWD: ${cwd}`,
                timestamp: Date.now()
            });
        });

        activeTerminals.set(id, terminal);
        res.json({ success: true });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

app.get('/api/terminal/logs/:id', (req, res) => {
    const { id } = req.params;
    const offset = parseInt(req.query.offset) || 0;
    const terminal = activeTerminals.get(id);

    if (!terminal) {
        return res.status(404).json({ error: 'Terminal not found' });
    }

    const newLogs = terminal.logs.slice(offset);
    res.json({
        status: terminal.status,
        exitCode: terminal.exitCode,
        logs: newLogs,
        nextOffset: terminal.logs.length
    });
});

app.post('/api/terminal/stop/:id', (req, res) => {
    const { id } = req.params;
    const terminal = activeTerminals.get(id);

    if (!terminal) {
        return res.status(404).json({ error: 'Terminal not found' });
    }

    if (terminal.process) {
        try {
            // In Windows/macOS, just SIGINT might not work for all shells
            terminal.process.kill('SIGINT');
            setTimeout(() => {
                if (terminal.status === 'running') {
                    terminal.process.kill('SIGKILL');
                }
            }, 2000);
        } catch (e) {
            console.error(`Error killing process ${id}:`, e);
        }
    }

    res.json({ success: true });
});

app.get('/api/terminal/status', (req, res) => {
    const status = {};
    for (const [id, term] of activeTerminals.entries()) {
        status[id] = {
            status: term.status,
            exitCode: term.exitCode
        };
    }
    res.json(status);
});

// SPA Catch-all: If not an API request and file exists, serve index.html
// Health Check
app.get('/health', (req, res) => {
    res.json({ status: 'ok', uptime: process.uptime(), timestamp: Date.now() });
});

if (fs.existsSync(distPath)) {
    app.get(/.*/, (req, res, next) => {
        if (req.path.startsWith('/api/')) return next();
        res.sendFile(path.join(distPath, 'index.html'));
    });
} else {
    // Fallback if frontend build is missing
    app.get(/.*/, (req, res) => {
        res.send(`
            <h1>ZenCode Bridge is Running!</h1>
            <p>Frontend static files were not found at <code>${distPath}</code>.</p>
            <p>Check the build logs or Dockerfile.</p>
        `);
    });
}

const server = app.listen(port, () => {
    console.log(`ZenCode Bridge running at http://localhost:${port}`);
});

server.on('error', (err) => {
    console.error('[Bridge] Server failed to start:', err);
});
