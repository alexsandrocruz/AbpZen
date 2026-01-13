import fs from 'fs';
import path from 'path';
import { codeGenerator } from './src/generators/index.ts';

const ZEN_FILE = '/Users/alexsandrocruz/Documents/dev/AbpZen/spz-meuprojeto/Lexus (8).zen';
const OUTPUT_DIR = '/Users/alexsandrocruz/Documents/dev/AbpZen/spz-meuprojeto/Lexus';

async function run() {
    console.log('Loading metadata...');
    const data = JSON.parse(fs.readFileSync(ZEN_FILE, 'utf-8'));
    const entities = data.nodes
        .filter((n: any) => n.type === 'entity')
        .map((n: any) => n.data);
    const relations = data.edges;

    console.log(`Processing ${entities.length} entities...`);
    const allRelContexts = codeGenerator.getGlobalRelationshipContexts(entities, relations);

    let totalFiles = 0;
    for (const entity of entities) {
        process.stdout.write(`Generating ${entity.name}... `);
        const relCtx = allRelContexts.find(r => r.entityName === entity.name) || { asParent: [], asChild: [] };

        // Generate backend
        const backendFiles = await codeGenerator.generateEntity(
            entity,
            'Sapienza.Lexus',
            'Sapienza.Lexus',
            relCtx.asParent,
            relCtx.asChild
        );

        // Generate React V2
        const reactFiles = await codeGenerator.generateReactV2Files(
            entity,
            'Sapienza.Lexus',
            'Sapienza.Lexus',
            relCtx.asParent,
            relCtx.asChild
        );

        const files = [...backendFiles, ...reactFiles];

        for (const file of files) {
            const fullPath = path.join(OUTPUT_DIR, file.path);
            fs.mkdirSync(path.dirname(fullPath), { recursive: true });
            fs.writeFileSync(fullPath, file.content);
            totalFiles++;
        }
        console.log('Done.');
    }

    console.log(`\nSuccessfully applied ${totalFiles} files to ${OUTPUT_DIR}`);
}

run().catch(console.error);
