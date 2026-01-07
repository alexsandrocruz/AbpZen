import express from 'express';
import cors from 'cors';
import bodyParser from 'body-parser';
import fs from 'fs';
import path from 'path';
import os from 'os';
import { exec } from 'child_process';

import { injectCode } from './injector.js';

const app = express();
const port = 3001;

app.use(cors());
app.use(bodyParser.json({ limit: '50mb' }));

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

app.post('/api/inject-code', (req, res) => {
    const { projectPath, instructions } = req.body;
    console.log(`[Bridge] Injecting/Merging for ${instructions.length} instructions in ${projectPath}`);
    if (!projectPath || !instructions) {
        return res.status(400).json({ error: 'Missing projectPath or instructions' });
    }

    try {
        const results = injectCode(projectPath, instructions);
        res.json({ success: true, results });
    } catch (error) {
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

        // Base template path
        const baseTemplatePath = templatePath || path.join(process.cwd(), '..', 'zencode-template');
        const copiedItems = [];

        // Folders to skip during copy (build artifacts, dependencies, caches)
        const SKIP_FOLDERS = new Set([
            'node_modules',
            '.git',
            'bin',
            'obj',
            '.vs',
            '.idea',
            '.next',
            'dist',
            'build',
            'packages',
            '.nuget',
            'TestResults'
        ]);

        // Helper to copy directory recursively, skipping build folders
        const copyDir = (src, dest) => {
            if (!fs.existsSync(src)) {
                console.log(`[Bridge] Source not found: ${src}`);
                return false;
            }
            fs.mkdirSync(dest, { recursive: true });
            const entries = fs.readdirSync(src, { withFileTypes: true });
            for (const entry of entries) {
                // Skip excluded folders
                if (entry.isDirectory() && SKIP_FOLDERS.has(entry.name)) {
                    console.log(`[Bridge] Skipping: ${entry.name}`);
                    continue;
                }

                const srcPath = path.join(src, entry.name);
                const destPath = path.join(dest, entry.name);

                if (entry.isDirectory()) {
                    copyDir(srcPath, destPath);
                } else if (entry.isSymbolicLink()) {
                    // Skip symbolic links to avoid issues
                    console.log(`[Bridge] Skipping symlink: ${entry.name}`);
                    continue;
                } else {
                    try {
                        fs.copyFileSync(srcPath, destPath);
                    } catch (err) {
                        console.log(`[Bridge] Warning: Could not copy ${entry.name}: ${err.message}`);
                    }
                }
            }
            return true;
        };

        // Copy backend template (Sapienza.Zen.* folders)
        const backendSrc = path.join(baseTemplatePath);
        const backendDest = path.join(projectPath, 'backend');

        // Copy all Sapienza.Zen.* directories to backend
        if (fs.existsSync(backendSrc)) {
            const items = fs.readdirSync(backendSrc, { withFileTypes: true });
            for (const item of items) {
                if (item.isDirectory() && item.name.startsWith('Sapienza.Zen')) {
                    const src = path.join(backendSrc, item.name);
                    const dest = path.join(backendDest, item.name);
                    if (copyDir(src, dest)) {
                        copiedItems.push(`backend/${item.name}`);
                    }
                }
            }
        }

        // Copy frontend templates based on selection
        if (frontends && Array.isArray(frontends)) {
            for (const frontend of frontends) {
                let srcFolder = '';
                let destFolder = '';

                switch (frontend) {
                    case 'react':
                        srcFolder = path.join(baseTemplatePath, 'abp-react');
                        destFolder = path.join(projectPath, 'abp-react');
                        break;
                    case 'angular':
                        srcFolder = path.join(baseTemplatePath, 'angular');
                        destFolder = path.join(projectPath, 'angular');
                        break;
                    case 'razor':
                        // Razor is part of backend, already copied
                        continue;
                }

                if (srcFolder && copyDir(srcFolder, destFolder)) {
                    copiedItems.push(frontend);
                }
            }
        }

        // Create zencode.json manifest
        const manifest = {
            version: '1.0',
            name: projectName,
            namespace: `Sapienza.${projectName}`,
            createdAt: new Date().toISOString(),
            frontends: frontends || [],
            entities: []
        };
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

app.listen(port, () => {
    console.log(`ZenCode Bridge running at http://localhost:${port}`);
});
