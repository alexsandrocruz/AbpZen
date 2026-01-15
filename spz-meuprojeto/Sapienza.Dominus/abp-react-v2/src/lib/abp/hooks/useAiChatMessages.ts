import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAiChatMessagesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  aiChatSessionId?: string;
  }

export function useAiChatMessages(input: GetAiChatMessagesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["aiChatMessages", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/ai-chat-message", {
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

export function useAllAiChatMessages() {
  return useQuery({
    queryKey: ["aiChatMessages", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/ai-chat-message", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAiChatMessage(id: string) {
  return useQuery({
    queryKey: ["aiChatMessage", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/ai-chat-message/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAiChatMessage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/ai-chat-message", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["aiChatMessages"] });
    },
  });
}

export function useUpdateAiChatMessage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/ai-chat-message/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["aiChatMessages"] });
      queryClient.invalidateQueries({ queryKey: ["aiChatMessage", data.id] });
    },
  });
}

export function useDeleteAiChatMessage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/ai-chat-message/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["aiChatMessages"] });
    },
  });
}


