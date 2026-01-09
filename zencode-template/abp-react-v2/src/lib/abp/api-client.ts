import axios, { type AxiosInstance, type InternalAxiosRequestConfig } from "axios";
import { resolveTenantFromHostname } from "./tenant";

// ABP API configuration
const API_BASE_URL = import.meta.env.VITE_API_URL || "";

// Create axios instance
const apiClient: AxiosInstance = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        "Content-Type": "application/json",
    },
});

// Request interceptor for auth token
apiClient.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        // Get token from storage
        const token = localStorage.getItem("abp_access_token");
        if (token && config.headers) {
            config.headers.Authorization = `Bearer ${token}`;
        }

        // Multi-tenancy support
        const tenantFromUrl = resolveTenantFromHostname();
        const tenantId = tenantFromUrl || localStorage.getItem("abp_tenant_id");

        if (tenantId && typeof tenantId === "string" && tenantId.trim() !== "" && config.headers) {
            config.headers["__tenant"] = tenantId;
            // console.log(`[API Client] Multi-tenancy header set: __tenant=${tenantId}`);
        }

        // Accept language
        const culture = localStorage.getItem("abp_culture") || "en";
        if (config.headers) {
            config.headers["Accept-Language"] = culture;
        }

        return config;
    },
    (error) => Promise.reject(error)
);

// Response interceptor for error handling
apiClient.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;

        // Handle 401 Unauthorized - token refresh
        if (error.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;

            // Try to refresh token
            const refreshToken = localStorage.getItem("abp_refresh_token");
            if (refreshToken) {
                try {
                    const response = await axios.post(`${API_BASE_URL}/connect/token`, {
                        grant_type: "refresh_token",
                        refresh_token: refreshToken,
                        client_id: import.meta.env.VITE_OIDC_CLIENT_ID || "Sapienza.Lexus_App",
                    });

                    const { access_token, refresh_token } = response.data;
                    localStorage.setItem("abp_access_token", access_token);
                    localStorage.setItem("abp_refresh_token", refresh_token);

                    originalRequest.headers.Authorization = `Bearer ${access_token}`;
                    return apiClient(originalRequest);
                } catch (refreshError) {
                    // Refresh failed, redirect to login
                    localStorage.removeItem("abp_access_token");
                    localStorage.removeItem("abp_refresh_token");
                    window.location.href = "/auth/login";
                    return Promise.reject(refreshError);
                }
            }
        }

        // Handle ABP error format
        if (error.response?.data?.error) {
            const abpError = error.response.data.error;
            console.error("[API Client] ABP Error:", abpError);
            return Promise.reject(new Error(abpError.message || "An error occurred"));
        }

        if (error.response?.status === 500) {
            console.error("[API Client] Internal Server Error (500). Check backend logs.");
        }

        return Promise.reject(error);
    }
);

export { apiClient };

// Helper function to set tenant
export function setCurrentTenant(tenantId: string | null) {
    if (tenantId) {
        localStorage.setItem("abp_tenant_id", tenantId);
    } else {
        localStorage.removeItem("abp_tenant_id");
    }
}

// Helper function to get current tenant
export function getCurrentTenant(): string | null {
    return localStorage.getItem("abp_tenant_id");
}
