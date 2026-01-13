import { useQuery } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface DashboardStats {
    totalUsers: number;
    totalWorkspaces: number;
    totalRoles: number;
    apiRequests: string;
}

interface UseDashboardStatsReturn {
    stats: DashboardStats | null;
    isLoading: boolean;
    isError: boolean;
    refetch: () => void;
}

/**
 * Hook to fetch real-time dashboard statistics from ABP APIs.
 */
export function useDashboardStats(): UseDashboardStatsReturn {
    const usersQuery = useQuery({
        queryKey: ["dashboard", "users"],
        queryFn: async () => {
            const response = await apiClient.get("/api/identity/users", {
                params: { maxResultCount: 1 }, // Only need count
            });
            return response.data?.totalCount || 0;
        },
        staleTime: 60000, // 1 minute
    });

    const tenantsQuery = useQuery({
        queryKey: ["dashboard", "tenants"],
        queryFn: async () => {
            const response = await apiClient.get("/api/multi-tenancy/tenants", {
                params: { maxResultCount: 1 },
            });
            return response.data?.totalCount || 0;
        },
        staleTime: 60000,
    });

    const rolesQuery = useQuery({
        queryKey: ["dashboard", "roles"],
        queryFn: async () => {
            const response = await apiClient.get("/api/identity/roles", {
                params: { maxResultCount: 1 },
            });
            return response.data?.totalCount || 0;
        },
        staleTime: 60000,
    });

    const isLoading = usersQuery.isLoading || tenantsQuery.isLoading || rolesQuery.isLoading;
    const isError = usersQuery.isError || tenantsQuery.isError || rolesQuery.isError;

    const stats: DashboardStats | null = isLoading ? null : {
        totalUsers: usersQuery.data ?? 0,
        totalWorkspaces: tenantsQuery.data ?? 0,
        totalRoles: rolesQuery.data ?? 0,
        apiRequests: "1.2M", // Placeholder - would need audit log aggregation
    };

    const refetch = () => {
        usersQuery.refetch();
        tenantsQuery.refetch();
        rolesQuery.refetch();
    };

    return {
        stats,
        isLoading,
        isError,
        refetch,
    };
}
