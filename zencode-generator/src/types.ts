export type FieldType = 'string' | 'int' | 'bool' | 'guid' | 'datetime' | 'long' | 'double' | 'decimal' | 'float' | 'char' | 'byte' | 'short' | 'enum';

export type EntityBaseClass =
    | 'Entity'
    | 'AuditedEntity'
    | 'FullAuditedEntity'
    | 'AggregateRoot'
    | 'AuditedAggregateRoot'
    | 'FullAuditedAggregateRoot';

export interface LookupConfig {
    mode: 'dropdown' | 'modal';
    targetEntity?: string;      // Related entity name
    displayField: string;       // Field to display (e.g., "name")
    searchFields?: string[];    // Fields to search
    valueField?: string;        // Field to save (default: "id")
}

/**
 * Enum option definition
 */
export interface EnumOption {
    value: number;              // The numeric value (0, 1, 2, ...)
    name: string;               // The C# enum name (e.g., "Active", "Inactive")
    displayText: string;        // The localized display text (e.g., "Ativo", "Inativo")
}

/**
 * Enum field configuration
 */
export interface EnumConfig {
    enumName: string;           // The enum type name (e.g., "OrderStatus")
    options: EnumOption[];      // The enum options
}

export interface EntityField {
    id: string;
    name: string;
    type: FieldType;
    isRequired: boolean;
    isNullable: boolean;
    isFilterable: boolean;
    isTextArea: boolean;
    defaultValue?: string;
    minLength?: number;
    maxLength?: number;
    regex?: string;
    emailValidation?: boolean;

    // Display Configuration
    label?: string;              // Label shown in forms/grid
    placeholder?: string;        // Placeholder text for inputs
    order?: number;              // Display order (lower = first)

    // Grid Configuration
    showInGrid?: boolean;        // Show this field in grid (default: true)
    gridWidth?: number;          // Width in grid (px)

    // Form Configuration
    showInForm?: boolean;        // Show in edit form (default: true)
    formWidth?: 'full' | 'half' | 'third';  // Width in form layout
    readOnly?: boolean;          // Read-only in form

    // Relationship Lookup Configuration (for FK fields)
    isLookup?: boolean;          // Is this a lookup/FK field?
    lookupConfig?: LookupConfig;

    // Enum Configuration (for enum fields)
    enumConfig?: EnumConfig;     // Config when type is 'enum'
}

export interface EntityData {
    name: string;
    pluralName: string;
    tableName: string;
    namespace: string;
    baseClass: EntityBaseClass;
    isMaster: boolean;
    fields: EntityField[];
}

export type RelationshipType = 'one-to-many' | 'many-to-many' | 'one-to-one';

export interface ChildGridConfig {
    title?: string;                      // Section title (e.g., "Order Items")
    allowAdd?: boolean;                  // Allow adding new child items
    allowRemove?: boolean;               // Allow removing child items
    allowEdit?: boolean;                 // Allow editing child items inline
    displayFields?: string[];            // Fields to show in the child grid (if empty, show all)
    defaultExpanded?: boolean;           // Start expanded
}

export interface JunctionConfig {
    tableName: string;                   // Junction table name (e.g., "AlunoTurma")
    junctionEntityId?: string;           // ID of the generated junction entity node
    additionalFields?: EntityField[];    // Extra fields (e.g., DataMatricula)
    showInSource?: boolean;              // Show grid in source entity form
    showInTarget?: boolean;              // Show grid in target entity form
}

export interface RelationshipData {
    type: RelationshipType;
    sourceNavigationName: string;
    targetNavigationName: string;
    isRequired: boolean;
    description?: string;

    // Master-Detail (Child Grid) configuration
    isChildGrid?: boolean;               // Is this a master-detail relationship?
    childGridConfig?: ChildGridConfig;   // Configuration for the child grid

    // Many-to-Many (N:N) configuration
    junctionConfig?: JunctionConfig;     // Configuration for junction table
}

export interface ZenMetadata {
    projectName: string;
    namespace: string;
    entities: {
        id: string;
        data: EntityData;
    }[];
    relationships: {
        id: string;
        source: string;
        target: string;
        data: RelationshipData;
    }[];
}
