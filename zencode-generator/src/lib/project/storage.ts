/**
 * Project Storage
 * LocalStorage utilities for recent projects
 */

import type { RecentProject } from './types';

const STORAGE_KEY = 'zencode-recent-projects';
const MAX_RECENT_PROJECTS = 10;

/**
 * Get recent projects from localStorage
 */
export function getRecentProjects(): RecentProject[] {
    if (typeof window === 'undefined') return [];

    try {
        const stored = localStorage.getItem(STORAGE_KEY);
        if (stored) {
            return JSON.parse(stored) as RecentProject[];
        }
    } catch (e) {
        console.error('Failed to load recent projects:', e);
    }

    return [];
}

/**
 * Add a project to recent projects
 */
export function addRecentProject(project: RecentProject): void {
    if (typeof window === 'undefined') return;

    const projects = getRecentProjects();

    // Remove if already exists (will be re-added at top)
    const filtered = projects.filter(p => p.path !== project.path);

    // Add to top
    const updated = [project, ...filtered].slice(0, MAX_RECENT_PROJECTS);

    localStorage.setItem(STORAGE_KEY, JSON.stringify(updated));
}

/**
 * Remove a project from recent projects
 */
export function removeRecentProject(path: string): void {
    if (typeof window === 'undefined') return;

    const projects = getRecentProjects();
    const filtered = projects.filter(p => p.path !== path);

    localStorage.setItem(STORAGE_KEY, JSON.stringify(filtered));
}

/**
 * Clear all recent projects
 */
export function clearRecentProjects(): void {
    if (typeof window === 'undefined') return;
    localStorage.removeItem(STORAGE_KEY);
}
