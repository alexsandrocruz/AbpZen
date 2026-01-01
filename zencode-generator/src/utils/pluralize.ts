/**
 * Pluralizes an English word following basic English grammar rules.
 * Handles common cases like: y -> ies, s/sh/ch/x/z -> es
 */
export const pluralize = (word: string): string => {
    if (!word || word.length === 0) return word;

    const lowerWord = word.toLowerCase();
    const lastChar = lowerWord[lowerWord.length - 1];
    const lastTwoChars = lowerWord.slice(-2);

    // Words ending in consonant + y -> ies
    const vowels = ['a', 'e', 'i', 'o', 'u'];
    if (lastChar === 'y' && word.length > 1) {
        const secondLastChar = lowerWord[lowerWord.length - 2];
        if (!vowels.includes(secondLastChar)) {
            return word.slice(0, -1) + 'ies';
        }
    }

    // Words ending in s, sh, ch, x, z -> es
    if (
        lastChar === 's' ||
        lastChar === 'x' ||
        lastChar === 'z' ||
        lastTwoChars === 'sh' ||
        lastTwoChars === 'ch'
    ) {
        return word + 'es';
    }

    // Words ending in f or fe -> ves (common cases)
    if (lastTwoChars === 'fe') {
        return word.slice(0, -2) + 'ves';
    }
    if (lastChar === 'f' && !['ff', 'ef', 'if', 'of'].includes(lastTwoChars)) {
        return word.slice(0, -1) + 'ves';
    }

    // Default: just add s
    return word + 's';
};
