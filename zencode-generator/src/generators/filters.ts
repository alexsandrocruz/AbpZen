/**
 * Custom LiquidJS filters for ABP code generation
 */

/**
 * Pluralize a word (English rules)
 */
export function pluralize(word: string): string {
    if (!word) return word;

    // Irregular plurals
    const irregulars: Record<string, string> = {
        'person': 'people',
        'child': 'children',
        'man': 'men',
        'woman': 'women',
        'foot': 'feet',
        'tooth': 'teeth',
        'goose': 'geese',
        'mouse': 'mice',
    };

    const lower = word.toLowerCase();
    if (irregulars[lower]) {
        // Preserve original casing
        return word[0] + irregulars[lower].slice(1);
    }

    // Rules
    if (word.endsWith('y') && !['a', 'e', 'i', 'o', 'u'].includes(word[word.length - 2]?.toLowerCase())) {
        return word.slice(0, -1) + 'ies';
    }
    if (word.endsWith('s') || word.endsWith('x') || word.endsWith('z') ||
        word.endsWith('ch') || word.endsWith('sh')) {
        return word + 'es';
    }
    if (word.endsWith('f')) {
        return word.slice(0, -1) + 'ves';
    }
    if (word.endsWith('fe')) {
        return word.slice(0, -2) + 'ves';
    }

    return word + 's';
}

/**
 * Convert string to camelCase
 */
export function camelCase(str: string): string {
    if (!str) return str;
    return str.charAt(0).toLowerCase() + str.slice(1);
}

/**
 * Convert string to PascalCase
 */
export function pascalCase(str: string): string {
    if (!str) return str;
    return str.charAt(0).toUpperCase() + str.slice(1);
}

/**
 * Convert string to kebab-case
 */
export function kebabCase(str: string): string {
    if (!str) return str;
    return str
        .replace(/([a-z])([A-Z])/g, '$1-$2')
        .replace(/[\s_]+/g, '-')
        .toLowerCase();
}

/**
 * Convert string to snake_case
 */
export function snakeCase(str: string): string {
    if (!str) return str;
    return str
        .replace(/([a-z])([A-Z])/g, '$1_$2')
        .replace(/[\s-]+/g, '_')
        .toLowerCase();
}

/**
 * Get C# type from our field type
 */
export function toCSharpType(type: string, isNullable: boolean = false): string {
    const typeMap: Record<string, string> = {
        'string': 'string',
        'int': 'int',
        'long': 'long',
        'float': 'float',
        'double': 'double',
        'decimal': 'decimal',
        'bool': 'bool',
        'datetime': 'DateTime',
        'guid': 'Guid',
    };

    const csharpType = typeMap[type] || type;

    // Add nullable marker for value types
    if (isNullable && ['int', 'long', 'float', 'double', 'decimal', 'bool', 'DateTime', 'Guid'].includes(csharpType)) {
        return csharpType + '?';
    }

    return csharpType;
}

/**
 * Get TypeScript type from our field type
 */
export function toTypeScriptType(type: string): string {
    const typeMap: Record<string, string> = {
        'string': 'string',
        'int': 'number',
        'long': 'number',
        'float': 'number',
        'double': 'number',
        'decimal': 'number',
        'bool': 'boolean',
        'datetime': 'Date',
        'guid': 'string',
    };

    return typeMap[type] || type;
}
