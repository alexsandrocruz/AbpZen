import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetChatMessagesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  conversationId?: string;
  }

export function useChatMessages(input: GetChatMessagesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["chatMessages", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/chat-message", {
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

export function useAllChatMessages() {
  return useQuery({
    queryKey: ["chatMessages", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/chat-message", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useChatMessage(id: string) {
  return useQuery({
    queryKey: ["chatMessage", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/chat-message/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateChatMessage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/chat-message", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["chatMessages"] });
    },
  });
}

export function useUpdateChatMessage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/chat-message/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["chatMessages"] });
      queryClient.invalidateQueries({ queryKey: ["chatMessage", data.id] });
    },
  });
}

export function useDeleteChatMessage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/chat-message/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["chatMessages"] });
    },
  });
}


