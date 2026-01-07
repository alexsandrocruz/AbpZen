/**
 * Gemini API Types
 * Types for AI entity import responses
 */

// ============ GEMINI API TYPES ============

export interface GeminiChatMessage {
    role: 'user' | 'model';
    parts: { text: string }[];
}

export interface GeminiRequestBody {
    contents: GeminiChatMessage[];
    generationConfig?: {
        temperature?: number;
        topK?: number;
        topP?: number;
        maxOutputTokens?: number;
        responseMimeType?: string;
    };
}

export interface GeminiResponse {
    candidates: {
        content: {
            parts: { text: string }[];
            role: string;
        };
        finishReason: string;
    }[];
}

// ============ AI IMPORT TYPES ============

/**
 * Field extracted from AI
 */
export interface AIExtractedField {
    name: string;
    type: 'string' | 'int' | 'long' | 'double' | 'decimal' | 'bool' | 'datetime' | 'guid' | 'enum';
    isRequired?: boolean;
    maxLength?: number;
    label?: string;
    placeholder?: string;
    description?: string;
    enumName?: string;
    enumValues?: string[];
}

/**
 * Entity extracted from AI
 */
export interface AIExtractedEntity {
    name: string;
    pluralName: string;
    description?: string;
    fields: AIExtractedField[];
}

/**
 * Relationship extracted from AI
 */
export interface AIExtractedRelationship {
    type: 'one-to-many' | 'many-to-many';
    sourceEntity: string;
    targetEntity: string;
    sourceNavigationName?: string;
    targetNavigationName?: string;
    description?: string;
}

/**
 * Enum extracted from AI
 */
export interface AIExtractedEnum {
    name: string;
    values: string[];
    description?: string;
}

/**
 * Complete AI extraction result
 */
export interface AIExtractionResult {
    entities: AIExtractedEntity[];
    relationships: AIExtractedRelationship[];
    enums: AIExtractedEnum[];
}

// ============ SETTINGS TYPES ============

/**
 * AI Settings stored in localStorage
 */
export interface AISettings {
    apiKey: string;
    model: 'gemini-2.0-flash-exp' | 'gemini-1.5-pro' | 'gemini-1.5-flash';
    temperature: number;
}

export const DEFAULT_AI_SETTINGS: AISettings = {
    apiKey: '',
    model: 'gemini-2.0-flash-exp',
    temperature: 0.2, // Low temperature for more consistent JSON output
};
