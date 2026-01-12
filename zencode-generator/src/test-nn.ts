
import { codeGenerator } from './generators/index.ts';
import type { RelationshipInfo } from './generators/index.ts';
import type { EntityData } from './types.ts';
import * as fs from 'fs';
import * as path from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

// ========== ENTITIES ==========

// Aluno (Student)
const alunoEntity: EntityData = {
    name: 'Aluno',
    namespace: 'LeptonXDemoApp.Aluno',
    isMaster: true,
    pluralName: 'Alunos',
    tableName: 'Alunos',
    baseClass: 'FullAuditedEntity',
    renderType: 'full-page',
    fields: [
        { id: 'a1', name: 'Nome', type: 'string', isRequired: true, isFilterable: true, isNullable: false, isTextArea: false, maxLength: 100 },
        { id: 'a2', name: 'Email', type: 'string', isRequired: true, isFilterable: true, isNullable: false, isTextArea: false, maxLength: 200 },
        { id: 'a3', name: 'Matricula', type: 'string', isRequired: true, isFilterable: true, isNullable: false, isTextArea: false, maxLength: 20 },
        { id: 'a4', name: 'DataNascimento', type: 'datetime', isRequired: false, isFilterable: false, isNullable: true, isTextArea: false }
    ]
};

// Turma (Class)
const turmaEntity: EntityData = {
    name: 'Turma',
    namespace: 'LeptonXDemoApp.Turma',
    isMaster: true,
    pluralName: 'Turmas',
    tableName: 'Turmas',
    baseClass: 'FullAuditedEntity',
    renderType: 'full-page',
    fields: [
        { id: 't1', name: 'Nome', type: 'string', isRequired: true, isFilterable: true, isNullable: false, isTextArea: false, maxLength: 100 },
        { id: 't2', name: 'Codigo', type: 'string', isRequired: true, isFilterable: true, isNullable: false, isTextArea: false, maxLength: 20 },
        { id: 't3', name: 'AnoLetivo', type: 'int', isRequired: true, isFilterable: true, isNullable: false, isTextArea: false },
        { id: 't4', name: 'Semestre', type: 'int', isRequired: true, isFilterable: false, isNullable: false, isTextArea: false }
    ]
};

// AlunoTurma (Junction Table with extra field)
const alunoTurmaEntity: EntityData = {
    name: 'AlunoTurma',
    namespace: 'LeptonXDemoApp.AlunoTurma',
    isMaster: false,
    pluralName: 'AlunoTurmas',
    tableName: 'AlunoTurmas',
    baseClass: 'FullAuditedEntity',
    fields: [
        {
            id: 'at1',
            name: 'AlunoId',
            type: 'guid',
            isRequired: true,
            isNullable: false,
            isTextArea: false,
            isFilterable: true,
            isLookup: true,
            lookupConfig: {
                mode: 'modal',
                targetEntity: 'Aluno',
                displayField: 'Nome'
            }
        },
        {
            id: 'at2',
            name: 'TurmaId',
            type: 'guid',
            isRequired: true,
            isNullable: false,
            isTextArea: false,
            isFilterable: true,
            isLookup: true,
            lookupConfig: {
                mode: 'modal',
                targetEntity: 'Turma',
                displayField: 'Nome'
            }
        },
        { id: 'at3', name: 'DataMatricula', type: 'datetime', isRequired: true, isFilterable: true, isNullable: false, isTextArea: false },
        { id: 'at4', name: 'Situacao', type: 'string', isRequired: false, isFilterable: false, isNullable: true, isTextArea: false, maxLength: 50 }
    ]
};

// ========== RELATIONSHIPS ==========
const relationships: RelationshipInfo[] = [
    {
        id: 'rel-aluno-turma',
        source: 'Aluno',
        target: 'Turma',
        data: {
            type: 'many-to-many',
            isRequired: false,
            sourceNavigationName: 'AlunoTurmas', // Collection on Aluno
            targetNavigationName: 'AlunoTurmas', // Collection on Turma
            junctionConfig: {
                tableName: 'AlunoTurma',
                junctionEntityId: 'AlunoTurma',
                sourceForeignKey: 'AlunoId',
                targetForeignKey: 'TurmaId',
                showInSource: true,  // Show Turmas tab on Aluno page
                showInTarget: true   // Show Alunos tab on Turma page
            }
        }
    }
];

async function run() {
    const projectPath = path.resolve(__dirname, '../../demo-zen');
    const namespace = 'LeptonXDemoApp';

    console.log(`Generating code for ${namespace} (N:N relationship test - Aluno/Turma)...`);

    // Clean up old files if exist
    const entitiesToClean = ['Aluno', 'Turma', 'AlunoTurma'];
    for (const entityName of entitiesToClean) {
        const webPagesPath = path.join(projectPath, `LeptonXDemoApp.Web/Pages/${entityName}`);
        if (fs.existsSync(webPagesPath)) {
            fs.rmSync(webPagesPath, { recursive: true, force: true });
            console.log(`Cleaned up: ${webPagesPath}`);
        }
    }

    // Generate all files
    const files = await codeGenerator.generateAll(
        [alunoEntity, turmaEntity, alunoTurmaEntity],
        relationships,
        'LeptonXDemoApp',
        'LeptonXDemoApp'
    );

    console.log(`\n=== Generated ${files.length} files ===\n`);

    for (const file of files) {
        const targetPath = path.join(projectPath, file.path);
        const dir = path.dirname(targetPath);

        if (!fs.existsSync(dir)) {
            fs.mkdirSync(dir, { recursive: true });
        }
        fs.writeFileSync(targetPath, file.content);
        console.log(`Written: ${file.path}`);
    }

    console.log('\n=== Generation Complete ===');
}

run().catch(console.error);
