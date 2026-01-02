import fs from 'fs';
import path from 'path';

/**
 * Handles code injection into existing C# files using markers.
 */
export function injectCode(projectPath, instructions) {
    // instructions is an array of { file: string, marker: string, content: string }
    const results = [];

    for (const inst of instructions) {
        const fullPath = path.join(projectPath, inst.file);
        if (!fs.existsSync(fullPath)) {
            results.push({ file: inst.file, success: false, error: 'File not found' });
            continue;
        }

        let content = fs.readFileSync(fullPath, 'utf8');
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

    return results;
}
