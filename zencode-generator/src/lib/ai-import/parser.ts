/**
 * AI Import Parser
 * Converts AI extracted entities to ZenCode EntityData format
 */

import type { AIExtractionResult, AIExtractedEntity, AIExtractedEnum } from '../gemini/types';
import type { EntityData, EntityField, FieldType } from '../../types';
import { pluralize } from '../../utils/pluralize';

/**
 * Convert AI extracted field type to ZenCode field type
 */
function mapFieldType(aiType: string): FieldType {
    const typeMap: Record<string, FieldType> = {
        'string': 'string',
        'int': 'int',
        'long': 'long',
        'double': 'double',
        'decimal': 'decimal',
        'bool': 'bool',
        'datetime': 'datetime',
        'guid': 'guid',
        'enum': 'enum',
    };
    return typeMap[aiType.toLowerCase()] || 'string';
}

/**
 * Convert AI extracted entity to ZenCode EntityData
 */
export function convertAIEntityToEntityData(
    aiEntity: AIExtractedEntity,
    _enums: AIExtractedEnum[]
): EntityData {
    const fields: EntityField[] = aiEntity.fields.map((field, index) => {
        const entityField: EntityField = {
            id: `field_${Date.now()}_${index}`,
            name: field.name,
            type: mapFieldType(field.type),
            isRequired: field.isRequired ?? false,
            isNullable: !field.isRequired,
            isFilterable: true,
            isTextArea: field.type === 'string' && (field.maxLength ?? 0) > 500,
            showInGrid: true,
            showInForm: true,
            label: field.label || field.name,
            placeholder: field.placeholder,
        };

        // Add validation
        if (field.maxLength) {
            entityField.maxLength = field.maxLength;
        }

        // Note: Enum handling will need EntityField type to be extended
        // For now, we skip enum-specific properties

        return entityField;
    });

    return {
        name: aiEntity.name,
        pluralName: aiEntity.pluralName || pluralize(aiEntity.name),
        tableName: aiEntity.pluralName || pluralize(aiEntity.name),
        namespace: 'App', // Will be set by user in settings
        baseClass: 'FullAuditedAggregateRoot',
        isMaster: true,
        fields,
    };
}

/**
 * Result of parsing AI extraction to canvas format
 */
export interface ParsedAIImport {
    entities: EntityData[];
    relationships: {
        sourceEntityName: string;
        targetEntityName: string;
        type: 'one-to-many' | 'many-to-many';
        description?: string;
    }[];
}

/**
 * Parse full AI extraction result to canvas-ready format
 */
export function parseAIExtractionToCanvas(result: AIExtractionResult): ParsedAIImport {
    const entities = result.entities.map(e => convertAIEntityToEntityData(e, result.enums));

    const relationships = result.relationships.map(rel => ({
        sourceEntityName: rel.sourceEntity,
        targetEntityName: rel.targetEntity,
        type: rel.type,
        description: rel.description,
    }));

    return { entities, relationships };
}

/**
 * Calculate grid positions for imported entities
 */
export function calculateEntityPositions(count: number): { x: number; y: number }[] {
    const positions: { x: number; y: number }[] = [];
    const cols = Math.ceil(Math.sqrt(count));
    const spacing = { x: 280, y: 220 };
    const offset = { x: 100, y: 100 };

    for (let i = 0; i < count; i++) {
        const col = i % cols;
        const row = Math.floor(i / cols);
        positions.push({
            x: offset.x + col * spacing.x,
            y: offset.y + row * spacing.y,
        });
    }

    return positions;
}
