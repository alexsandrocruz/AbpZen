import type { EntityData, EntityField, FieldType } from '../types';
import { pluralize } from './pluralize';

export const parseSqlToEntities = (sql: string): EntityData[] => {
    const entities: EntityData[] = [];

    // Remove SQL comments but preserve structure
    let cleanSql = sql
        .replace(/\/\*[\s\S]*?\*\//g, '') // Remove block comments
        .replace(/--.*$/gm, '');          // Remove line comments

    // Find all CREATE TABLE statements using a simple approach
    const tableRegex = /CREATE\s+TABLE\s+(?:(?:\[?\w+\]?|\"\w+\")\.)?(?:\[?(\w+)\]?|\"(\w+)\")\s*\(/gi;

    let match;
    while ((match = tableRegex.exec(cleanSql)) !== null) {
        const tableName = match[1] || match[2];
        const startIndex = match.index + match[0].length;

        // Find the matching closing parenthesis by counting
        let depth = 1;
        let endIndex = startIndex;
        for (let i = startIndex; i < cleanSql.length && depth > 0; i++) {
            if (cleanSql[i] === '(') depth++;
            if (cleanSql[i] === ')') depth--;
            endIndex = i;
        }

        const columnsBody = cleanSql.substring(startIndex, endIndex);

        if (!tableName || !columnsBody) continue;

        const fields: EntityField[] = [];

        // Split by newlines first, then by commas as fallback
        const lines = columnsBody.split(/\n/).map(l => l.trim()).filter(l => l);

        lines.forEach((line, index) => {
            // Remove trailing comma
            let trimmedLine = line.replace(/,\s*$/, '').trim();

            // Skip empty lines and constraint definitions
            if (!trimmedLine) return;
            const upperLine = trimmedLine.toUpperCase();
            if (upperLine.startsWith('PRIMARY KEY') ||
                upperLine.startsWith('CONSTRAINT') ||
                upperLine.startsWith('INDEX') ||
                upperLine.startsWith('FOREIGN KEY') ||
                upperLine.startsWith('UNIQUE') ||
                upperLine.startsWith('CHECK')) {
                return;
            }

            // Match column: [ColumnName] or "ColumnName" or ColumnName followed by DataType
            const colRegex = /^\[?(\w+)\]?\s+(\w+)(?:\s*\([^)]*\))?(\s+NOT\s+NULL|\s+NULL)?/i;
            const colMatch = colRegex.exec(trimmedLine);

            if (colMatch) {
                const colName = colMatch[1];
                const sqlType = colMatch[2].toUpperCase();
                const nullabilitySpec = colMatch[3]?.toUpperCase() || '';
                const isNotNull = nullabilitySpec.includes('NOT NULL');

                fields.push({
                    id: `field_${Date.now()}_${index}_${Math.random().toString(36).substr(2, 5)}`,
                    name: colName,
                    type: mapSqlTypeToAppType(sqlType),
                    isNullable: !isNotNull,
                    isRequired: isNotNull,
                    isFilterable: true,
                    isTextArea: sqlType === 'TEXT' || sqlType.includes('MAX'),
                });
            }
        });

        if (fields.length > 0) {
            entities.push({
                name: tableName,
                pluralName: pluralize(tableName),
                tableName: pluralize(tableName),
                namespace: 'ZenDoctor',
                baseClass: 'FullAuditedAggregateRoot',
                isMaster: true,
                fields
            });
        }
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
