import fs from 'fs';
import path from 'path';

/**
 * Handles code injection into existing C# files using markers.
 */
export function injectCode(projectPath, instructions) {
    // instructions is an array of { file: string, marker?: string, content: string, type?: 'marker' | 'json-merge' }
    const results = [];

    for (const inst of instructions) {
        const fullPath = path.join(projectPath, inst.file);
        if (!fs.existsSync(fullPath)) {
            results.push({ file: inst.file, success: false, error: 'File not found' });
            continue;
        }

        try {
            let content = fs.readFileSync(fullPath, 'utf8');

            if (inst.type === 'json-merge') {
                const existingJson = JSON.parse(content);
                const newEntries = JSON.parse(inst.content);

                // Merge texts collection
                if (!existingJson.texts) existingJson.texts = {};
                if (!existingJson.Texts) existingJson.Texts = {};

                const textsKey = existingJson.texts ? 'texts' : 'Texts';

                // Deep merge or simple assign? Simple assign for localization is usually enough
                Object.assign(existingJson[textsKey], newEntries);

                fs.writeFileSync(fullPath, JSON.stringify(existingJson, null, 2));
                results.push({ file: inst.file, success: true });
            } else {
                // Default: marker-based injection
                const marker = `// <${inst.marker}>`;

                if (content.includes(marker)) {
                    // Avoid duplicate injection
                    if (content.includes(inst.content.trim())) {
                        results.push({ file: inst.file, success: true, message: 'Content already exists' });
                        continue;
                    }

                    const newContent = content.replace(marker, `${inst.content}\n      ${marker}`);
                    fs.writeFileSync(fullPath, newContent);
                    results.push({ file: inst.file, success: true });
                } else {
                    results.push({ file: inst.file, success: false, error: `Marker ${marker} not found` });
                }
            }
        } catch (error) {
            results.push({ file: inst.file, success: false, error: error.message });
        }
    }

    return results;
}
