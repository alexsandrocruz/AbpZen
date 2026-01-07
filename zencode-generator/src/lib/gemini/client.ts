/**
 * Gemini API Client
 * Handles communication with Google Gemini API
 */

import type {
    GeminiRequestBody,
    GeminiResponse,
    AIExtractionResult,
    AISettings
} from './types';
import { DEFAULT_AI_SETTINGS } from './types';
import { buildExtractionPrompt } from './prompts';

const GEMINI_API_BASE = 'https://generativelanguage.googleapis.com/v1beta/models';

/**
 * Get AI settings from localStorage
 */
export function getAISettings(): AISettings {
    if (typeof window === 'undefined') {
        return { ...DEFAULT_AI_SETTINGS };
    }

    try {
        const stored = localStorage.getItem('zencode-ai-settings');
        if (stored) {
            return { ...DEFAULT_AI_SETTINGS, ...JSON.parse(stored) };
        }
    } catch (e) {
        console.error('Failed to parse AI settings:', e);
    }

    return { ...DEFAULT_AI_SETTINGS };
}

/**
 * Save AI settings to localStorage
 */
export function saveAISettings(settings: Partial<AISettings>): void {
    if (typeof window === 'undefined') return;

    const current = getAISettings();
    const updated = { ...current, ...settings };
    localStorage.setItem('zencode-ai-settings', JSON.stringify(updated));
}

/**
 * Check if Gemini API is configured
 */
export function isGeminiConfigured(): boolean {
    const settings = getAISettings();
    return !!settings.apiKey && settings.apiKey.length > 0;
}

/**
 * Call Gemini API to extract entities from text
 */
export async function extractEntitiesFromText(
    userInput: string,
    settings?: AISettings
): Promise<AIExtractionResult> {
    const config = settings || getAISettings();

    if (!config.apiKey) {
        throw new Error('Gemini API key not configured. Please configure in Settings.');
    }

    const prompt = buildExtractionPrompt(userInput);

    const requestBody: GeminiRequestBody = {
        contents: [
            {
                role: 'user',
                parts: [{ text: prompt }]
            }
        ],
        generationConfig: {
            temperature: config.temperature,
            maxOutputTokens: 32768, // Increased to handle large entity lists
            responseMimeType: 'application/json'
        }
    };

    const url = `${GEMINI_API_BASE}/${config.model}:generateContent?key=${config.apiKey}`;

    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(requestBody)
    });

    if (!response.ok) {
        const error = await response.text();
        throw new Error(`Gemini API error: ${response.status} - ${error}`);
    }

    const data: GeminiResponse = await response.json();

    if (!data.candidates || data.candidates.length === 0) {
        throw new Error('No response from Gemini API');
    }

    const candidate = data.candidates[0];

    // Check if response was truncated due to token limit
    if (candidate.finishReason === 'MAX_TOKENS') {
        throw new Error(
            'A resposta foi truncada devido ao tamanho. Tente dividir sua descrição em partes menores ' +
            '(ex: primeiro grupo de entidades, depois segundo grupo).'
        );
    }

    const textResponse = candidate.content.parts[0].text;

    // Parse the JSON response
    try {
        // Clean up potential markdown formatting
        let cleanedResponse = textResponse.trim();
        if (cleanedResponse.startsWith('```json')) cleanedResponse = cleanedResponse.slice(7);
        if (cleanedResponse.startsWith('```')) cleanedResponse = cleanedResponse.slice(3);
        if (cleanedResponse.endsWith('```')) cleanedResponse = cleanedResponse.slice(0, -3);
        cleanedResponse = cleanedResponse.trim();

        const result = JSON.parse(cleanedResponse) as AIExtractionResult;

        // Validate structure
        if (!result.entities || !Array.isArray(result.entities)) {
            throw new Error('Invalid response: missing entities array');
        }

        // Ensure defaults
        result.relationships = result.relationships || [];
        result.enums = result.enums || [];

        console.log(`Successfully extracted ${result.entities.length} entities and ${result.relationships.length} relationships`);
        return result;
    } catch (e) {
        console.error('Failed to parse Gemini response. Raw:', textResponse);

        // Check if it looks like truncated JSON
        if (textResponse.length > 100 && !textResponse.trim().endsWith('}')) {
            throw new Error(
                'Resposta parece truncada. Tente dividir sua descrição em partes menores.'
            );
        }

        throw new Error(`Falha ao processar resposta da IA: ${e}`);
    }
}

/**
 * Test the API connection with a simple request
 */
export async function testGeminiConnection(apiKey: string): Promise<boolean> {
    const url = `${GEMINI_API_BASE}/gemini-2.0-flash-exp:generateContent?key=${apiKey}`;

    const requestBody: GeminiRequestBody = {
        contents: [
            {
                role: 'user',
                parts: [{ text: 'Respond with exactly: OK' }]
            }
        ],
        generationConfig: {
            maxOutputTokens: 10
        }
    };

    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(requestBody)
        });

        return response.ok;
    } catch {
        return false;
    }
}
