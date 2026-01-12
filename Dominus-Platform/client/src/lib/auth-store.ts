import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { api } from './api';

interface User {
  id: string;
  email: string;
  name: string;
  avatar?: string;
  globalRole: 'USER' | 'HOST';
}

interface Workspace {
  id: string;
  slug: string;
  name: string;
  ownerId: string;
}

interface AuthState {
  user: User | null;
  workspaces: Workspace[];
  currentWorkspace: Workspace | null;
  isLoading: boolean;
  error: string | null;

  // Actions
  login: (email: string, password: string) => Promise<void>;
  register: (data: { email: string; password: string; name: string; workspaceName: string; workspaceSlug: string }) => Promise<void>;
  logout: () => Promise<void>;
  checkAuth: () => Promise<void>;
  setCurrentWorkspace: (workspace: Workspace) => void;
  clearError: () => void;
}

export const useAuth = create<AuthState>()(
  persist(
    (set) => ({
      user: null,
      workspaces: [],
      currentWorkspace: null,
      isLoading: false,
      error: null,

      login: async (email, password) => {
        set({ isLoading: true, error: null });
        try {
          const response = await api.login({ email, password });
          set({ 
            user: response.user, 
            workspaces: response.workspaces || [],
            currentWorkspace: response.workspaces?.[0] || null,
            isLoading: false 
          });
        } catch (error: any) {
          set({ error: error.message, isLoading: false });
          throw error;
        }
      },

      register: async (data) => {
        set({ isLoading: true, error: null });
        try {
          const response = await api.register(data);
          set({ 
            user: response.user, 
            workspaces: [response.workspace],
            currentWorkspace: response.workspace,
            isLoading: false 
          });
        } catch (error: any) {
          set({ error: error.message, isLoading: false });
          throw error;
        }
      },

      logout: async () => {
        try {
          await api.logout();
        } catch (error) {
          console.error('Logout error:', error);
        } finally {
          set({ user: null, workspaces: [], currentWorkspace: null });
        }
      },

      checkAuth: async () => {
        try {
          const response = await api.me();
          set({ 
            user: response.user, 
            workspaces: response.workspaces || [],
            currentWorkspace: response.workspaces?.[0] || null
          });
        } catch (error) {
          set({ user: null, workspaces: [], currentWorkspace: null });
        }
      },

      setCurrentWorkspace: (workspace) => set({ currentWorkspace: workspace }),
      clearError: () => set({ error: null }),
    }),
    {
      name: 'dominus-auth',
      partialize: (state) => ({ 
        currentWorkspace: state.currentWorkspace 
      }),
    }
  )
);
