import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAiChatSessionsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAiChatSessions(input: GetAiChatSessionsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["aiChatSessions", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/ai-chat-session", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllAiChatSessions() {
  return useQuery({
    queryKey: ["aiChatSessions", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/ai-chat-session", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAiChatSession(id: string) {
  return useQuery({
    queryKey: ["aiChatSession", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/ai-chat-session/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAiChatSession() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/ai-chat-session", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["aiChatSessions"] });
    },
  });
}

export function useUpdateAiChatSession() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/ai-chat-session/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["aiChatSessions"] });
      queryClient.invalidateQueries({ queryKey: ["aiChatSession", data.id] });
    },
  });
}

export function useDeleteAiChatSession() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/ai-chat-session/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["aiChatSessions"] });
    },
  });
}


