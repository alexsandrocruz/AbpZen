// Imports removed in favor of dynamic imports

// --- Node Conversion Logic ---

function convertSchemaToZenNodes(schema) {
    const nodes = [];
    const edges = [];
    const entityMap = new Map();

    const startX = 100;
    const startY = 100;
    const gapX = 350;
    const gapY = 400;
    const cols = 4;

    let index = 0;

    for (const table of schema.tables) {
        // Simple grid layout
        const col = index % cols;
        const row = Math.floor(index / cols);
        const position = { x: startX + col * gapX, y: startY + row * gapY };
        index++;

        const node = {
            id: table.tableName.toLowerCase(),
            type: 'entity',
            position,
            data: {
                name: table.tableName,
                pluralName: table.tableName + 's', // Simple pluralization
                tableName: table.tableName,
                namespace: `Sapienza.Zen.${table.tableName}`, // Default namespace
                baseClass: 'FullAuditedAggregateRoot',
                isMaster: true,
                fields: table.columns.map(col => ({
                    id: col.name.toLowerCase(),
                    name: col.name,
                    type: mapDataType(col.dataType),
                    isRequired: !col.isNullable,
                    isNullable: col.isNullable,
                    isFilterable: true,
                    isTextArea: false,
                    maxLength: col.maxLength ? parseInt(col.maxLength) : undefined
                }))
            }
        };

        nodes.push(node);
        entityMap.set(table.tableName, node.id);
    }

    // Process Relationships (Foreign Keys)
    for (const table of schema.tables) {
        if (!table.foreignKeys) continue;

        for (const fk of table.foreignKeys) {
            const sourceId = entityMap.get(table.tableName);
            const targetId = entityMap.get(fk.referencedTable);

            if (sourceId && targetId) {
                // Find relationships 
                // Defaulting to 1:N (Target -< Source)
                // e.g., Order (Source) has CustomerId (FK to Customer Target)
                // So Customer (1) -> Orders (N)

                // In ZenCode visualizer, we draw "Entity -> Related Entity".
                // If Order has CustomerId, the arrow usually points Source (Order) -> Target (Customer) representing lookup.

                edges.push({
                    id: `${sourceId}-${targetId}-${fk.columnName}`,
                    source: sourceId,
                    target: targetId,
                    type: 'relation',
                    data: {
                        type: 'one-to-many', // Assuming 1:N for standard FKs
                        sourceNavigationName: table.tableName + 's',
                        targetNavigationName: fk.referencedTable,
                        isRequired: true, // Simplified
                        description: `FK from ${table.tableName}.${fk.columnName}`
                    }
                });

                // Update the field to be a lookup
                const node = nodes.find(n => n.id === sourceId);
                const field = node.data.fields.find(f => f.name === fk.columnName);
                if (field) {
                    field.isLookup = true;
                    field.type = 'guid'; // FKs are usually guids in ABP
                    field.lookupConfig = {
                        mode: 'dropdown',
                        targetEntity: fk.referencedTable,
                        displayField: 'Id' // Default, user can change
                    };
                }
            }
        }
    }

    return { nodes, edges };
}

function mapDataType(dbType) {
    const type = dbType.toLowerCase();
    if (type.includes('int')) return 'int';
    if (type.includes('char') || type.includes('text')) return 'string';
    if (type.includes('bool') || type.includes('bit')) return 'boolean';
    if (type.includes('date') || type.includes('time')) return 'datetime';
    if (type.includes('decimal') || type.includes('money') || type.includes('numeric')) return 'decimal';
    if (type.includes('uniqueidentifier') || type.includes('uuid')) return 'guid';
    return 'string'; // Fallback
}


// --- Providers ---

class PostgresProvider {
    async connect(config) {
        const { default: pg } = await import('pg');
        const { Client } = pg;
        this.client = new Client({
            host: config.host || 'localhost',
            port: parseInt(config.port) || 5432,
            database: config.database,
            user: config.user,
            password: config.password,
        });
        await this.client.connect();
    }

    async disconnect() {
        if (this.client) await this.client.end();
    }

    async getSchema(schemaName = 'public') {
        // Get Tables
        const tablesRes = await this.client.query(`
            SELECT table_name 
            FROM information_schema.tables 
            WHERE table_schema = $1 AND table_type = 'BASE TABLE'
        `, [schemaName]);

        const tables = [];

        for (const row of tablesRes.rows) {
            const tableName = row.table_name;

            // Get Columns
            const colsRes = await this.client.query(`
                SELECT column_name, data_type, is_nullable, character_maximum_length
                FROM information_schema.columns 
                WHERE table_schema = $1 AND table_name = $2
            `, [schemaName, tableName]);

            // Get FKs
            const fksRes = await this.client.query(`
                SELECT
                    kcu.column_name,
                    ccu.table_name AS foreign_table_name,
                    ccu.column_name AS foreign_column_name
                FROM information_schema.key_column_usage AS kcu
                JOIN information_schema.constraint_column_usage AS ccu
                    ON ccu.constraint_name = kcu.constraint_name
                    AND ccu.table_schema = kcu.table_schema
                JOIN information_schema.table_constraints AS tc
                    ON tc.constraint_name = kcu.constraint_name
                    AND tc.table_schema = kcu.table_schema
                WHERE kcu.table_schema = $1 AND kcu.table_name = $2 AND tc.constraint_type = 'FOREIGN KEY'
            `, [schemaName, tableName]);

            tables.push({
                tableName: tableName,
                columns: colsRes.rows.map(c => ({
                    name: c.column_name,
                    dataType: c.data_type,
                    isNullable: c.is_nullable === 'YES',
                    maxLength: c.character_maximum_length
                })),
                foreignKeys: fksRes.rows.map(fk => ({
                    columnName: fk.column_name,
                    referencedTable: fk.foreign_table_name,
                    referencedColumn: fk.foreign_column_name
                }))
            });
        }

        return { tables };
    }
}

class SqlServerProvider {
    async connect(config) {
        const { default: mssql } = await import('mssql');
        this.pool = await mssql.connect({
            server: config.host || 'localhost',
            port: parseInt(config.port) || 1433,
            database: config.database,
            user: config.user,
            password: config.password,
            options: {
                encrypt: false, // For local dev
                trustServerCertificate: true
            }
        });
    }

    async disconnect() {
        if (this.pool) await this.pool.close();
    }

    async getSchema(schemaName = 'dbo') {
        const query = `
            SELECT 
                t.name AS TableName,
                c.name AS ColumnName,
                ty.name AS DataType,
                c.is_nullable AS IsNullable,
                c.max_length AS MaxLength
            FROM sys.tables t
            INNER JOIN sys.columns c ON t.object_id = c.object_id
            INNER JOIN sys.types ty ON c.user_type_id = ty.user_type_id
            WHERE t.schema_id = SCHEMA_ID('${schemaName}')
            ORDER BY t.name, c.column_id;
        `;

        const fkQuery = `
            SELECT 
                tp.name AS TableName,
                cp.name AS ColumnName,
                tr.name AS ReferencedTableName,
                cr.name AS ReferencedColumnName
            FROM sys.foreign_keys fk
            INNER JOIN sys.tables tp ON fk.parent_object_id = tp.object_id
            INNER JOIN sys.tables tr ON fk.referenced_object_id = tr.object_id
            INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
            INNER JOIN sys.columns cp ON fkc.parent_column_id = cp.column_id AND fkc.parent_object_id = cp.object_id
            INNER JOIN sys.columns cr ON fkc.referenced_column_id = cr.column_id AND fkc.referenced_object_id = cr.object_id
            WHERE tp.schema_id = SCHEMA_ID('${schemaName}') AND tr.schema_id = SCHEMA_ID('${schemaName}')
        `;

        const colsResult = await this.pool.request().query(query);
        const fkResult = await this.pool.request().query(fkQuery);

        const tables = [];
        const tableMap = new Map();

        // Group columns by table
        for (const row of colsResult.recordset) {
            if (!tableMap.has(row.TableName)) {
                tableMap.set(row.TableName, {
                    tableName: row.TableName,
                    columns: [],
                    foreignKeys: []
                });
            }
            const table = tableMap.get(row.TableName);
            table.columns.push({
                name: row.ColumnName,
                dataType: row.DataType,
                isNullable: row.IsNullable,
                maxLength: row.MaxLength
            });
        }

        // Attach FKs
        for (const row of fkResult.recordset) {
            const table = tableMap.get(row.TableName);
            if (table) {
                table.foreignKeys.push({
                    columnName: row.ColumnName,
                    referencedTable: row.ReferencedTableName,
                    referencedColumn: row.ReferencedColumnName
                });
            }
        }

        return { tables: Array.from(tableMap.values()) };
    }
}

export async function importDatabase(config) {
    let provider;

    try {
        if (config.provider === 'postgres') {
            provider = new PostgresProvider();
        } else if (config.provider === 'mssql') {
            provider = new SqlServerProvider();
        } else {
            throw new Error(`Unsupported provider: ${config.provider}`);
        }

        await provider.connect(config);
        const schema = await provider.getSchema(config.schema);
        const result = convertSchemaToZenNodes(schema);

        return result;

    } catch (error) {
        console.error("DB Import Error:", error);
        throw error;
    } finally {
        if (provider) await provider.disconnect();
    }
}
