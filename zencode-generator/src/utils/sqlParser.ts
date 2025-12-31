import type { EntityData, EntityField, FieldType } from '../types';

export const parseSqlToEntities = (sql: string): EntityData[] => {
    const entities: EntityData[] = [];

    // Normalize SQL: remove comments, multiple spaces, etc.
    const cleanSql = sql
        .replace(/\/\*[\s\S]*?\*\/|--.*?\n/g, '') // Remove comments
        .replace(/\s+/g, ' '); // Standardize spaces

    // Match CREATE TABLE blocks
    // Regex looks for CREATE TABLE [maybe schema].Table ( ... )
    const createTableRegex = /CREATE TABLE\s+(?:\[?(\w+)\]?\.)?\[?(\w+)\]?\s*\(([\s\S]*?)\)(?:\s*;|\s*$|\s+(?=CREATE TABLE))/gi;

    let tableMatch;
    while ((tableMatch = createTableRegex.exec(cleanSql)) !== null) {
        const tableName = tableMatch[2];
        const columnsBody = tableMatch[3];

        const fields: EntityField[] = [];

        // Parse columns
        // Splits by comma, but tries to ignore commas inside parentheses (like DECIMAL(18,2))
        const columnLines = columnsBody.split(/,(?![^(]*\))/);

        columnLines.forEach(line => {
            const trimmedLine = line.trim();
            if (!trimmedLine || trimmedLine.toUpperCase().startsWith('PRIMARY KEY') || trimmedLine.toUpperCase().startsWith('CONSTRAINT') || trimmedLine.toUpperCase().startsWith('INDEX')) {
                return;
            }

            // Match [ColumnName] DataType [NULL|NOT NULL]
            const colRegex = /\[?(\w+)\]?\s+(\w+)(?:\s*\([\d,\s]+\))?(\s+NOT\s+NULL)?/i;
            const colMatch = colRegex.exec(trimmedLine);

            if (colMatch) {
                const colName = colMatch[1];
                const sqlType = colMatch[2].toUpperCase();
                const isNotNull = !!colMatch[3];

                fields.push({
                    id: `field_${Date.now()}_${Math.random().toString(36).substr(2, 5)}`,
                    name: colName,
                    type: mapSqlTypeToAppType(sqlType),
                    isNullable: !isNotNull,
                    isRequired: isNotNull,
                    isFilterable: true,
                    isTextArea: sqlType === 'TEXT' || sqlType === 'NVARCHAR(MAX)',
                    // Extract length if needed, for now keep simple
                });
            }
        });

        entities.push({
            name: tableName,
            pluralName: `${tableName}s`,
            tableName: tableName,
            namespace: 'ZenDoctor', // Default
            baseClass: 'FullAuditedAggregateRoot',
            isMaster: true,
            fields
        });
    }

    return entities;
};

const mapSqlTypeToAppType = (sqlType: string): FieldType => {
    switch (sqlType) {
        case 'VARCHAR':
        case 'NVARCHAR':
        case 'TEXT':
        case 'CHAR':
        case 'NCHAR':
        case 'STRING':
            return 'string';

        case 'INT':
        case 'INTEGER':
        case 'SMALLINT':
        case 'TINYINT':
            return 'int';

        case 'BIGINT':
            return 'long';

        case 'FLOAT':
        case 'REAL':
            return 'float';

        case 'DOUBLE':
        case 'DECIMAL':
        case 'NUMERIC':
        case 'MONEY':
            return 'double';

        case 'BIT':
        case 'BOOLEAN':
            return 'bool';

        case 'DATE':
        case 'DATETIME':
        case 'DATETIME2':
        case 'TIMESTAMP':
            return 'datetime';

        case 'UNIQUEIDENTIFIER':
        case 'GUID':
        case 'UUID':
            return 'guid';

        default:
            return 'string';
    }
};
