import { useEffect } from "react";
import { useLocation } from "wouter";
import { useAuth } from "@/lib/abp/auth";
import { useAbpConfig } from "@/lib/abp/config";

interface AbpProviderProps {
    children: React.ReactNode;
}

export function AbpProvider({ children }: AbpProviderProps) {
    const { isAuthenticated, refreshAccessToken, expiresAt } = useAuth();
    const { fetchConfig } = useAbpConfig();

    // Fetch ABP configuration on mount and when auth changes
    useEffect(() => {
        fetchConfig();
    }, [fetchConfig, isAuthenticated]);

    // Set up token refresh timer
    useEffect(() => {
        if (!isAuthenticated || !expiresAt) return;

        // Refresh 5 minutes before expiration
        const refreshTime = expiresAt - Date.now() - 5 * 60 * 1000;

        if (refreshTime > 0) {
            const timer = setTimeout(() => {
                refreshAccessToken();
            }, refreshTime);

            return () => clearTimeout(timer);
        } else {
            // Token already expired or will expire soon
            refreshAccessToken();
        }
    }, [isAuthenticated, expiresAt, refreshAccessToken]);

    return <>{children}</>;
}

// HOC for protected routes
export function withAuth<P extends object>(Component: React.ComponentType<P>) {
    return function AuthenticatedComponent(props: P) {
        const { isAuthenticated, isLoading } = useAuth();
        const [, setLocation] = useLocation();

        useEffect(() => {
            if (!isLoading && !isAuthenticated) {
                setLocation("/auth/login");
            }
        }, [isAuthenticated, isLoading, setLocation]);

        if (isLoading) {
            return (
                <div className="flex items-center justify-center min-h-screen">
                    <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary"></div>
                </div>
            );
        }

        if (!isAuthenticated) {
            return null;
        }

        return <Component {...props} />;
    };
}

// Hook for requiring specific permission
export function useRequirePermission(permission: string) {
    const { hasPermission } = useAbpConfig();
    const [, setLocation] = useLocation();

    useEffect(() => {
        if (!hasPermission(permission)) {
            setLocation("/unauthorized");
        }
    }, [permission, hasPermission, setLocation]);

    return hasPermission(permission);
}
