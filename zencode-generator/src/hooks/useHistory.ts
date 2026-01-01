import { useCallback, useRef, useState } from 'react';

interface HistoryState<T, E> {
    nodes: T[];
    edges: E[];
}

const MAX_HISTORY_SIZE = 50;

export function useHistory<T, E>(
    initialNodes: T[],
    initialEdges: E[]
) {
    const [history, setHistory] = useState<HistoryState<T, E>[]>([
        { nodes: initialNodes, edges: initialEdges }
    ]);
    const [currentIndex, setCurrentIndex] = useState(0);
    const isUndoRedoAction = useRef(false);

    const canUndo = currentIndex > 0;
    const canRedo = currentIndex < history.length - 1;

    const pushState = useCallback((nodes: T[], edges: E[]) => {
        // Skip if this was triggered by undo/redo
        if (isUndoRedoAction.current) {
            isUndoRedoAction.current = false;
            return;
        }

        setHistory((prev) => {
            // Trim future states if we're not at the end
            const newHistory = prev.slice(0, currentIndex + 1);

            // Add new state
            newHistory.push({ nodes: [...nodes], edges: [...edges] });

            // Limit history size
            if (newHistory.length > MAX_HISTORY_SIZE) {
                newHistory.shift();
                return newHistory;
            }
            return newHistory;
        });

        setCurrentIndex((prev) => Math.min(prev + 1, MAX_HISTORY_SIZE - 1));
    }, [currentIndex]);

    const undo = useCallback((): HistoryState<T, E> | null => {
        if (!canUndo) return null;

        isUndoRedoAction.current = true;
        const newIndex = currentIndex - 1;
        setCurrentIndex(newIndex);
        return history[newIndex];
    }, [canUndo, currentIndex, history]);

    const redo = useCallback((): HistoryState<T, E> | null => {
        if (!canRedo) return null;

        isUndoRedoAction.current = true;
        const newIndex = currentIndex + 1;
        setCurrentIndex(newIndex);
        return history[newIndex];
    }, [canRedo, currentIndex, history]);

    return {
        pushState,
        undo,
        redo,
        canUndo,
        canRedo,
    };
}
