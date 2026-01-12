import { create } from "zustand";
import { persist } from "zustand/middleware";
import { apiClient } from "./api-client";

// Token response from OpenIddict
interface TokenResponse {
    access_token: string;
    refresh_token: string;
    token_type: string;
    expires_in: number;
}

// User info
interface UserInfo {
    id: string;
    userName: string;
    name: string;
    email: string;
    tenantId: string | null;
    roles: string[];
}

interface AuthState {
    isAuthenticated: boolean;
    user: UserInfo | null;
    accessToken: string | null;
    refreshToken: string | null;
    expiresAt: number | null;
    isLoading: boolean;
    error: string | null;

    // Actions
    login: (username: string, password: string) => Promise<void>;
    logout: () => void;
    refreshAccessToken: () => Promise<void>;
    setTokens: (access: string, refresh: string, expiresIn: number) => void;
}

export const useAuth = create<AuthState>()(
    persist(
        (set, get) => ({
            isAuthenticated: false,
            user: null,
            accessToken: null,
            refreshToken: null,
            expiresAt: null,
            isLoading: false,
            error: null,

            login: async (username: string, password: string) => {
                set({ isLoading: true, error: null });

                try {
                    const clientId = import.meta.env.VITE_OIDC_CLIENT_ID || "Sapienza.Lexus_App";
                    const scope = import.meta.env.VITE_OIDC_SCOPE || "openid profile email offline_access Sapienza.Lexus";

                    // Request token from OpenIddict
                    const params = new URLSearchParams();
                    params.append("grant_type", "password");
                    params.append("client_id", clientId);
                    params.append("username", username);
                    params.append("password", password);
                    params.append("scope", scope);

                    const response = await apiClient.post<TokenResponse>("/connect/token", params, {
                        headers: {
                            "Content-Type": "application/x-www-form-urlencoded",
                        },
                    });

                    const { access_token, refresh_token, expires_in } = response.data;

                    // Store tokens
                    localStorage.setItem("abp_access_token", access_token);
                    localStorage.setItem("abp_refresh_token", refresh_token);

                    // Calculate expiration
                    const expiresAt = Date.now() + expires_in * 1000;

                    set({
                        isAuthenticated: true,
                        accessToken: access_token,
                        refreshToken: refresh_token,
                        expiresAt,
                        isLoading: false,
                    });

                    // Fetch user info
                    const userResponse = await apiClient.get("/connect/userinfo");
                    set({ user: userResponse.data });

                } catch (error) {
                    const message = error instanceof Error ? error.message : "Login failed";
                    set({
                        error: message,
                        isLoading: false,
                        isAuthenticated: false,
                    });
                    throw error;
                }
            },

            logout: () => {
                localStorage.removeItem("abp_access_token");
                localStorage.removeItem("abp_refresh_token");

                set({
                    isAuthenticated: false,
                    user: null,
                    accessToken: null,
                    refreshToken: null,
                    expiresAt: null,
                });

                // Redirect to login
                window.location.href = "/auth/login";
            },

            refreshAccessToken: async () => {
                const { refreshToken } = get();
                if (!refreshToken) {
                    get().logout();
                    return;
                }

                try {
                    const clientId = import.meta.env.VITE_OIDC_CLIENT_ID || "Sapienza.Lexus_App";

                    const params = new URLSearchParams();
                    params.append("grant_type", "refresh_token");
                    params.append("client_id", clientId);
                    params.append("refresh_token", refreshToken);

                    const response = await apiClient.post<TokenResponse>("/connect/token", params, {
                        headers: {
                            "Content-Type": "application/x-www-form-urlencoded",
                        },
                    });

                    const { access_token, refresh_token, expires_in } = response.data;

                    localStorage.setItem("abp_access_token", access_token);
                    localStorage.setItem("abp_refresh_token", refresh_token);

                    set({
                        accessToken: access_token,
                        refreshToken: refresh_token,
                        expiresAt: Date.now() + expires_in * 1000,
                    });
                } catch {
                    get().logout();
                }
            },

            setTokens: (access: string, refresh: string, expiresIn: number) => {
                localStorage.setItem("abp_access_token", access);
                localStorage.setItem("abp_refresh_token", refresh);

                set({
                    isAuthenticated: true,
                    accessToken: access,
                    refreshToken: refresh,
                    expiresAt: Date.now() + expiresIn * 1000,
                });
            },
        }),
        {
            name: "abp-auth-storage",
            partialize: (state) => ({
                isAuthenticated: state.isAuthenticated,
                user: state.user,
                accessToken: state.accessToken,
                refreshToken: state.refreshToken,
                expiresAt: state.expiresAt,
            }),
        }
    )
);
