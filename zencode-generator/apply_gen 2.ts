
import { CodeGenerator } from './src/generators';
import * as fs from 'fs';
import * as path from 'path';

async function main() {
    const zenPath = '../spz-meuprojeto/Lexus (8).zen';
    console.log(`Loading metadata from ${zenPath}...`);
    const zen = JSON.parse(fs.readFileSync(zenPath, 'utf-8'));

    const entities = zen.nodes.filter((n: any) => n.type === 'entity').map((n: any) => ({
        ...n.data,
        id: n.id
    }));
    const relationships = zen.edges.filter((e: any) => e.type === 'relation');

    const generator = new CodeGenerator();
    console.log('Resolving global relationship contexts...');
    const contexts = generator.getGlobalRelationshipContexts(entities, relationships);
    const contextMap = new Map(contexts.map(c => [c.entityName, c]));

    const projectName = zen.config?.projectName || 'Lexus';
    const projectNamespace = zen.config?.namespace || 'Sapienza.Lexus';
    const projectPathValue = zen.config?.projectPath || '/Users/alexsandrocruz/Documents/dev/AbpZen/spz-meuprojeto/Lexus';
    const frontends = ['react-v2']; // Based on project history

    console.log(`Generating code for ${entities.length} entities...`);
    let totalFiles = 0;

    for (const entity of entities) {
        process.stdout.write(`Generating ${entity.name}... `);
        const ctx = contextMap.get(entity.name);
        const asParent = ctx?.asParent || [];
        const asChild = ctx?.asChild || [];

        try {
            const files = await generator.generateEntityWithFrontends(
                entity,
                projectName,
                projectNamespace,
                asParent,
                asChild,
                frontends as any
            );

            for (const file of files) {
                const fullPath = path.join(projectPathValue, file.path);
                const dir = path.dirname(fullPath);

                if (!fs.existsSync(dir)) {
                    fs.mkdirSync(dir, { recursive: true });
                }

                fs.writeFileSync(fullPath, file.content);
                totalFiles++;
            }
            console.log('Done.');
        } catch (err) {
            console.error(`\nError generating ${entity.name}:`, err);
        }
    }

    console.log(`\nSuccessfully applied ${totalFiles} files to ${projectPathValue}`);
}

main().catch(console.error);
