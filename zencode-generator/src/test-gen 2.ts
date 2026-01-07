
import { codeGenerator } from './generators/index.js';
import { EntityData, RelationshipData, RelationshipInfo } from './types.js';

// Mocks
const entities: EntityData[] = [
    {
        name: 'Order',
        pluralName: 'Orders',
        tableName: 'Orders',
        baseClass: 'FullAudited.AggregateRoot',
        fields: [
            { id: '1', name: 'Number', type: 'string', isRequired: true }
        ]
    },
    {
        name: 'OrderItem',
        pluralName: 'OrderItems',
        tableName: 'OrderItems',
        baseClass: 'FullAudited.AggregateRoot',
        fields: [
            { id: '2', name: 'Quantity', type: 'int', isRequired: true },
            { id: '3', name: 'OrderId', type: 'guid', isLookup: true, lookupConfig: { targetEntity: 'Order' } }
        ]
    }
];

const relationships: RelationshipInfo[] = [
    {
        id: 'rel1',
        source: 'OrderItem',
        target: 'Order',
        data: {
            type: 'one-to-many',
            sourceNavigationName: 'OrderItems',
            targetNavigationName: 'Order',
            isRequired: true,
            isChildGrid: true,
            childGridConfig: { allowAdd: true }
        }
    }
];

async function run() {
    console.log("Generating...");
    const files = await codeGenerator.generateAll(entities, relationships, 'ZenGenerated', 'ZenDoctor');

    console.log(`Generated ${files.length} files.`);

    // Check specific files
    const appService = files.find(f => f.path.includes('OrderAppService.cs'));
    const createModalJs = files.find(f => f.path.includes('CreateModal.js'));
    const dto = files.find(f => f.path.includes('CreateUpdateOrderDto.cs'));

    if (appService) {
        console.log("--- OrderAppService.cs (Excerpt) ---");
        // Simple check for child logic
        if (appService.content.includes("_orderItemRepository")) console.log("✅ Repository Injected");
        if (appService.content.includes("Master-Detail: OrderItem")) console.log("✅ Master-Detail Logic Present");
    } else {
        console.error("❌ OrderAppService.cs NOT found");
    }

    if (createModalJs) {
        console.log("--- CreateModal.js Found ---");
        if (createModalJs.content.includes("_orderItemList")) console.log("✅ List logic present");
    } else {
        console.error("❌ CreateModal.js NOT found");
    }

    if (dto) {
        console.log("--- CreateUpdateOrderDto.cs ---");
        if (dto.content.includes("List<CreateUpdateOrderItemDto> OrderItems")) console.log("✅ DTO List Property Present");
    }
}

run();
