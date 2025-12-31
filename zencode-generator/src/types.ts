export type FieldType = 'string' | 'int' | 'bool' | 'guid' | 'datetime' | 'long' | 'double' | 'decimal' | 'float' | 'char' | 'byte' | 'short';

export type EntityBaseClass =
    | 'Entity'
    | 'AuditedEntity'
    | 'FullAuditedEntity'
    | 'AggregateRoot'
    | 'AuditedAggregateRoot'
    | 'FullAuditedAggregateRoot';

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

export interface RelationshipData {
    type: RelationshipType;
    sourceNavigationName: string;
    targetNavigationName: string;
    isRequired: boolean;
    description?: string;
}
