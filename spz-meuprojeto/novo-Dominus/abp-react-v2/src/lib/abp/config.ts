import { create } from "zustand";
import { persist } from "zustand/middleware";
import { apiClient } from "./api-client";

// ABP Application Configuration Response
interface AbpApplicationConfiguration {
    localization: {
        currentCulture: {
            cultureName: string;
            displayName: string;
        };
        languages: Array<{
            cultureName: string;
            displayName: string;
        }>;
        values: Record<string, Record<string, string>>;
    };
    auth: {
        policies: Record<string, boolean>;
        grantedPolicies: Record<string, boolean>;
    };
    currentUser: {
        isAuthenticated: boolean;
        id: string | null;
        tenantId: string | null;
        userName: string | null;
        name: string | null;
        surName: string | null;
        email: string | null;
        emailVerified: boolean;
        phoneNumber: string | null;
        phoneNumberVerified: boolean;
        roles: string[];
    };
    currentTenant: {
        id: string | null;
        name: string | null;
        isAvailable: boolean;
    };
    features: {
        values: Record<string, string>;
    };
    setting: {
        values: Record<string, string>;
    };
}

interface AbpConfigStore {
    config: AbpApplicationConfiguration | null;
    isLoading: boolean;
    error: string | null;
    fetchConfig: () => Promise<void>;
    hasPermission: (policy: string) => boolean;
    L: (key: string, resourceName?: string) => string;
}

export const useAbpConfig = create<AbpConfigStore>()(
    persist(
        (set, get) => ({
            config: null,
            isLoading: false,
            error: null,

            fetchConfig: async () => {
                set({ isLoading: true, error: null });
                try {
                    const response = await apiClient.get<AbpApplicationConfiguration>(
                        "/api/abp/application-configuration"
                    );
                    set({ config: response.data, isLoading: false });
                } catch (error) {
                    set({
                        error: error instanceof Error ? error.message : "Failed to fetch config",
                        isLoading: false
                    });
                }
            },

            hasPermission: (policy: string) => {
                const config = get().config;
                if (!config) return false;
                return config.auth.grantedPolicies[policy] === true;
            },

            L: (key: string, resourceName?: string) => {
                const config = get().config;
                if (!config?.localization?.values) return key;

                // Try to find in specified resource
                if (resourceName && config.localization.values[resourceName]) {
                    const value = config.localization.values[resourceName][key];
                    if (value) return value;
                }

                // Search in all resources
                for (const resource of Object.values(config.localization.values)) {
                    if (resource[key]) return resource[key];
                }

                return key;
            },
        }),
        {
            name: "abp-config-storage",
            partialize: (state) => ({ config: state.config }),
        }
    )
);

// Hook for checking permissions
export function useAbpPermissions() {
    const { hasPermission, config } = useAbpConfig();

    return {
        hasPermission,
        isAuthenticated: config?.currentUser?.isAuthenticated ?? false,
        currentUser: config?.currentUser ?? null,
        currentTenant: config?.currentTenant ?? null,
        roles: config?.currentUser?.roles ?? [],
    };
}

// Hook for localization
export function useAbpLocalization() {
    const { L, config } = useAbpConfig();

    return {
        L,
        currentCulture: config?.localization?.currentCulture?.cultureName ?? "en",
        languages: config?.localization?.languages ?? [],
    };
}
