
import { codeGenerator } from './generators/index.ts';
import type { RelationshipInfo } from './generators/index.ts';
import type { EntityData, RelationshipData } from './types.ts';
import * as fs from 'fs';
import * as path from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

// Mock Data
const orderEntity: EntityData = {
    name: 'Order',
    namespace: 'LeptonXDemoApp.Order',
    isMaster: true,
    pluralName: 'Orders',
    tableName: 'Orders',
    baseClass: 'FullAuditedEntity',
    fields: [
        { id: 'f1', name: 'Number', type: 'string', isRequired: true, isFilterable: true, isNullable: false, isTextArea: false },
        { id: 'f2', name: 'Date', type: 'datetime', isRequired: true, isFilterable: true, isNullable: false, isTextArea: false },
        {
            id: 'f3',
            name: 'Status',
            type: 'enum',
            isRequired: true,
            isFilterable: true,
            isNullable: false,
            isTextArea: false,
            enumConfig: {
                enumName: 'OrderStatus',
                options: [
                    { name: 'Draft', value: 0, displayText: 'Draft' },
                    { name: 'Confirmed', value: 1, displayText: 'Confirmed' },
                    { name: 'Shipped', value: 2, displayText: 'Shipped' },
                    { name: 'Cancelled', value: 3, displayText: 'Cancelled' }
                ]
            }
        },
        { id: 'f4', name: 'Obs', type: 'string', isRequired: false, isNullable: true, isTextArea: true, isFilterable: false },
        {
            id: 'f5',
            name: 'CustomerId',
            type: 'guid',
            isRequired: true,
            isLookup: true,
            isNullable: false,
            isTextArea: false,
            isFilterable: true,
            lookupConfig: {
                mode: 'modal',
                targetEntity: 'Customer',
                displayField: 'Name'
            }
        }
    ]
};

const orderItemEntity: EntityData = {
    name: 'OrderItem',
    namespace: 'LeptonXDemoApp.OrderItem',
    isMaster: false,
    pluralName: 'OrderItems',
    tableName: 'OrderItems',
    baseClass: 'FullAuditedEntity',
    fields: [
        {
            id: 'fi1',
            name: 'ProductId',
            type: 'guid',
            isRequired: true,
            isLookup: true,
            isNullable: false,
            isTextArea: false,
            isFilterable: true,
            lookupConfig: {
                mode: 'modal',
                targetEntity: 'Product',
                displayField: 'Name'
            }
        },
        { id: 'fi2', name: 'OrderId', type: 'guid', isRequired: true, isNullable: false, isTextArea: false, isFilterable: false }, // FK to Order
        { id: 'fi3', name: 'Quant', type: 'int', isRequired: true, isNullable: false, isTextArea: false, isFilterable: false },
        { id: 'fi4', name: 'Price', type: 'decimal', isRequired: true, isNullable: false, isTextArea: false, isFilterable: false },
        { id: 'fi5', name: 'Total', type: 'decimal', isRequired: true, isNullable: false, isTextArea: false, isFilterable: false }
    ]
};

// Relationships
const relationships: RelationshipInfo[] = [
    {
        id: 'rel-1',
        source: 'OrderItem', // Child
        target: 'Order',     // Parent
        data: {
            type: 'one-to-many',
            isChildGrid: true, // MASTER-DETAIL TRIGGER
            childGridConfig: {
                title: 'Order Items'
            },
            isRequired: true,
            sourceNavigationName: 'OrderItems',
            targetNavigationName: 'Order'
        }
    }
];

async function run() {
    const projectPath = path.resolve(__dirname, '../../demo-zen');
    const namespace = 'LeptonXDemoApp';

    console.log(`Generating code for Order (Master-Detail)...`);

    // Clean up old Modal files for Order to avoid confusion usage
    const webPagesPath = path.join(projectPath, 'LeptonXDemoApp.Web/Pages/Order');
    if (fs.existsSync(webPagesPath)) {
        const modalFiles = ['CreateModal.cshtml', 'CreateModal.cshtml.cs', 'EditModal.cshtml', 'EditModal.cshtml.cs', 'CreateModal.js', 'EditModal.js'];
        for (const f of modalFiles) {
            if (fs.existsSync(path.join(webPagesPath, f))) {
                fs.unlinkSync(path.join(webPagesPath, f));
                console.log(`Deleted old modal file: ${f}`);
            }
        }
    }

    // Generate for Order (Parent)
    const files = await codeGenerator.generateAll(
        [orderEntity, orderItemEntity],
        relationships,
        'LeptonXDemoApp',
        'LeptonXDemoApp'
    );

    for (const file of files) {
        const targetPath = path.join(projectPath, file.path);
        const dir = path.dirname(targetPath);

        if (!fs.existsSync(dir)) {
            fs.mkdirSync(dir, { recursive: true });
        }
        fs.writeFileSync(targetPath, file.content);
        console.log(`Written: ${file.path}`);
    }
}

run().catch(console.error);
